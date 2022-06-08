using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using SAMMI.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace SmartMES
{

    static class Program
    {
        //public static string UserPlantCode = string.Empty;

        [DllImport("user32.dll")]
        public static extern void BringWindowToTop(IntPtr hWnd);

        [DllImport("User32", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #region <SSO-openAPI>

        [DllImport("SA_CSI.dll")]
        public static extern int SA_CSI_CheckServer(string strCheckServerURL, int strServerPort);

        [DllImport("SA_CSI.dll")]
        public static extern int SA_CSI_SSO_Login(string strServiceID, int port, string strLogInURL, string strGetChallengeURL, string strGetServiceInfoURL, string strGetPublicKeyURL, string strChangePWURL, string strTokenAuthoriaztionURL);

        [DllImport("SA_CSI.dll")]
        public static extern int SA_CSI_SSO_Logout(string strLogOutURL, int port);

        [DllImport("SA_CSI.dll")]
        public static extern int SA_CSI_SSO_TokenLogin(string strLogInURL, int port, string strServiceID);

        [DllImport("SA_CSI.dll")]
        public static extern int SA_CSI_IDPW_Authentication(string strLogInURL, int port, string strServiceID, string id, string pwd);

        [DllImport("SA_CSI.dll")]
        public static extern int SA_CSI_GetLoginUserInfo(string strKey, StringBuilder strValue, int buffsize);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("SA_CSI.dll")]
        public static extern int SA_CSI_SetEnableEncToken(Boolean mode);

        [DllImport("SA_CSI.dll")]
        public static extern int SA_CSI_GetLastErrorInfo(StringBuilder strErrorCode, int codebuff, StringBuilder strErrorMsg, int msgbuff);

        private static string strLogInURL          = "https://sso.samkee.com/openapi/authentication/login";
        private static string strLogOutURL         = "https://sso.samkee.com/openapi/authentication/logout";
        private static string strGetChallengeURL   = "https://sso.samkee.com/openapi/authentication/challenge/get";
        private static string strGetServiceInfoURL = "https://sso.samkee.com/openapi/service/info/get";
        private static string strGetPubkeyURL      = "https://sso.samkee.com/openapi/authentication/publickey/get";
        private static string strAuthURL           = "https://sso.samkee.com/token/authorization";
        private static string strChangePWURL       = "https://sso.samkee.com/openapi/authentication/password/change";
        private static string strCheckServerURL    = "https://sso.samkee.com/openapi/checkserver/";  //도메인

        // ERP:4 , MES:5 && Token Encrypt Mode, Encrypt = true
        private static string strServiceID  = "5";
        private static string strServerPort = "443";
        private static Boolean TokenEncMode = true;

        #endregion

        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string userid = string.Empty;
            string userName = string.Empty;

            // 동일프로그램 실행 확인(동일프로그램일 경우, 실행중인 프로그램을 맨앞으로 가지고 온다.)
            if (SmartMES.Program.CheckMultiProcess()) return;

            // 프로그램 전체에서 사용되어지는 Style 파일을 정의
            Infragistics.Win.AppStyling.StyleManager.Load(Application.StartupPath + @"\Style.isl");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region <File Update>
            SAMMI.Windows.Forms.UpgradeForm zz0100 = new SAMMI.Windows.Forms.UpgradeForm(userid, global::SmartMES.Properties.Resources.LOGIN3);

            // 라이브 업데이트시 LOCK이 걸린 경우 RESTART
            if (zz0100.DialogResult == DialogResult.Cancel)
            {
                MessageBox.Show("프로그램 구성이 바뀌었습니다. 재실행해 주십시오.");
                //Application.Restart();
                return;
            }
            #endregion

            #region <SSO Login>

            if (WrapperCheckServer() == true) //SSO서버 生死 확인
            {
                if (WrapperTokenLogin() == true) // 토큰확인
                {
                    userid = WrapperGetLoginUserInfo();
                    if (!string.IsNullOrEmpty(userid))
                    {
                        //사용자 기본정보
                        Database db = DatabaseFactory.CreateDatabase();
                        DataTable dt = db.ExecuteDataSet("USP_ZGETWORKERINFO_S2", userid).Tables[0];

                        if (SqlDBHelper.nvlInt(dt.Rows.Count) > 0)
                        {
                            if (SqlDBHelper.nvlString(dt.Rows[0]["OverLap"]) == "Y") // 삼기, 삼기ev 중복사용자는 공장선택 팝업 
                            {
                                ZZ0000 zz0000 = new ZZ0000("T"); //토큰생성
                                if (zz0000.DialogResult != DialogResult.OK)
                                    return;

                                userid = zz0000.UserID;
                                userName = zz0000.UserName;

                                SmartMES.Program.RunApplication(new string[] { userid, userName });
                                return;
                            }
                            else
                            {
                                Configuration appConfig1 = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                                System.Collections.Specialized.NameValueCollection sitecollection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("site");

                                string serverType = string.Empty;
                                if (SqlDBHelper.nvlString(dt.Rows[0]["WorkerID"]) != "") //삼기직원
                                {
                                    if (SqlDBHelper.nvlString(dt.Rows[0]["PlantCode"]) == "SK1")
                                    {
                                        serverType = "SK";
                                    }
                                    else
                                    {
                                        serverType = "SKS";
                                    }
                                }
                                else if (SqlDBHelper.nvlString(dt.Rows[0]["EV_WorkerID"]) != "") //EV직원
                                {
                                    if (SqlDBHelper.nvlString(dt.Rows[0]["EV_PlantCode"]) == "SK1")
                                    {
                                        serverType = "EVP";
                                    }
                                    else
                                    {
                                        serverType = "EV";
                                    }
                                }
                                else //해당인원 1명 존재.. EV직원
                                {
                                    serverType = "EVP";
                                }
                                // 모듈 및 메인 ConnectionString정보 업데이트
                                for (int i = 0; i < appConfig1.ConnectionStrings.ConnectionStrings.Count; i++)
                                {
                                    appConfig1.ConnectionStrings.ConnectionStrings[i].ConnectionString = sitecollection[serverType].ToString();
                                }
                                appConfig1.ConnectionStrings.ConnectionStrings["ConnectionString"].ConnectionString = sitecollection[serverType].ToString();
                                string[] serverip = sitecollection[serverType].Split(new char[] { '=', ';' });

                                // AppConfig정보 업데이트
                                appConfig1.AppSettings.Settings["SITE"].Value = serverType;  // 연결MES서버 Alias
                                appConfig1.AppSettings.Settings["DBSERVER1"].Value = serverip[1]; // 연결서버주조
                                appConfig1.AppSettings.Settings["DBUSER1"].Value = serverip[5]; // DB계정
                                appConfig1.AppSettings.Settings["DBPASSWORD1"].Value = serverip[7]; // DB패스워드

                                appConfig1.Save();
                                ConfigurationManager.RefreshSection("configuration");

                                switch (serverType)
                                {
                                    case "SK":
                                    case "SKS":
                                        userName = SqlDBHelper.nvlString(dt.Rows[0]["WorkerName"]);
                                        LoginInfo.UserID = userid = SqlDBHelper.nvlString(dt.Rows[0]["WorkerID"]);
                                        LoginInfo.UserPlantCode = SqlDBHelper.nvlString(dt.Rows[0]["PlantCode"]);
                                        LoginInfo.UserPlantName = "삼기";
                                        LoginInfo.PlantAuth = SqlDBHelper.nvlString(dt.Rows[0]["PlantAuth"]);
                                        break;
                                    case "EV":
                                    case "EVP":
                                        userName = SqlDBHelper.nvlString(dt.Rows[0]["EV_WorkerName"]);
                                        LoginInfo.UserID = userid = SqlDBHelper.nvlString(dt.Rows[0]["EV_WorkerID"]);
                                        LoginInfo.UserPlantCode = SqlDBHelper.nvlString(dt.Rows[0]["EV_PlantCode"]);
                                        LoginInfo.UserPlantName = "삼기ev";
                                        LoginInfo.PlantAuth = SqlDBHelper.nvlString(dt.Rows[0]["EV_PlantAuth"]);
                                        break;
                                }
                                SmartMES.Program.RunApplication(new string[] { userid, userName });
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("사용자가 없거나 비활성화 상태입니다. 다시 확인하세요.");
                            return;
                        }
                    }
                }
                else // 신규토큰생성
                {
                    ZZ0000 zz0000 = new ZZ0000("T");
                    if (zz0000.DialogResult != DialogResult.OK)
                        return;

                    userid = zz0000.UserID;
                    userName = zz0000.UserName;

                    SmartMES.Program.RunApplication(new string[] { userid, userName });
                    return;
                }
            }
            else // SSO서버다운(ERP로그인)
            {
                ZZ0000 zz0000 = new ZZ0000("S");
                if (zz0000.DialogResult != DialogResult.OK)
                    return;

                userid = zz0000.UserID;
                userName = zz0000.UserName;

                SmartMES.Program.RunApplication(new string[] { userid, userName });
            }
            #endregion

            #region <기존 Login(주석처리)>
            //// 로그인 실패시 종료
            //ZZ0000 zz0000 = new ZZ0000((args.Length > 0 ? args[0] : ""));
            // if (zz0000.DialogResult != DialogResult.OK)
            //    return;

            //userid = zz0000.UserID;
            //userName = zz0000.UserName;

            //// 프로그램 실행
            //SmartMES.Program.RunApplication(new string[] { userid, userName });
            #endregion
        }

        /// <summary>
        /// 중복프로그램 실행 확인
        /// </summary>
        /// <returns></returns>
        public static Boolean CheckMultiProcess()
        {
            int thisID = System.Diagnostics.Process.GetCurrentProcess().Id; // 현재 기동한 프로그램 id

            //실행중인 프로그램중 현재 기동한 프로그램과 같은 프로그램들 수집
            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

            if (p.Length > 1)
            {
                for (int i = 0; i < p.Length; i++)
                {
                    if (p[i].Id == thisID) continue;

                    ShowWindow(p[i].MainWindowHandle, 1);
                    BringWindowToTop(p[i].MainWindowHandle);
                    SetForegroundWindow(p[i].MainWindowHandle);

                    break;
                }
                return true;
            }

            return false;
        }
        /// <summary>
        /// APPLICATION 실행
        /// </summary>
        /// <param name="args"></param>
        public static void RunApplication(params object[] args)
        {
            Assembly assembly;
            Type typeForm;
            Form newForm;
            Configuration appconfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            assembly = Assembly.LoadFrom(Application.StartupPath + @"\" + appconfig.AppSettings.Settings["STARTFORMFILE"].Value.ToString());
            typeForm = assembly.GetType(appconfig.AppSettings.Settings["STARTFORM"].Value.ToString(), true);
            newForm = (Form)Activator.CreateInstance(typeForm, args);

            Application.Run(newForm);
        }
        #region <SSO Function>
        //////////////////////////////////////////
        // CheckServer Test
        //////////////////////////////////////////
        private static Boolean WrapperCheckServer()
        {
            Boolean ret = true;

            int result = SA_CSI_CheckServer(strCheckServerURL.ToString(), Int32.Parse(strServerPort.ToString()));

            if (result != 0) { ret = false; }
            return ret;
        }
        private static Boolean WrapperIDPWLogin(string sId, string sPw)
        {
            Boolean ret = true;
            int apiResult = 0;

            WrapperSetEnableEncToken(TokenEncMode);
            apiResult = SA_CSI_IDPW_Authentication(strLogInURL,
                                                     Int32.Parse(strServerPort.ToString()),
                                                     strServiceID,
                                                     sId,
                                                     sPw);
            if (0 == apiResult) { }
            else { ret = false; }

            return ret;
        }
        //////////////////////////////////////////
        // Token Login Test
        //////////////////////////////////////////
        private static Boolean WrapperTokenLogin()
        {
            Boolean ret = true;

            WrapperSetEnableEncToken(TokenEncMode);

            int result = SA_CSI_SSO_TokenLogin(strAuthURL.ToString(),
                                                Int32.Parse(strServerPort.ToString()),
                                                strServiceID.ToString());
            if (0 == result) { }//Console.WriteLine("    토큰 로그인 성공!! \n");   
            else { ret = false; }

            return ret;
        }
        //////////////////////////////////////////
        // LogoutTest
        //////////////////////////////////////////
        private static Boolean WrapperLogOut()
        {
            Boolean ret = true;
            int result = SA_CSI_SSO_Logout(strLogOutURL, Int32.Parse(strServerPort.ToString()));
            if (0 == result) { }
            else { ret = false; }

            return ret;
        }
        //////////////////////////////////////////
        // Set Token Encrypt Mode
        // param - Encrypt mode = true 
        //////////////////////////////////////////
        private static Boolean WrapperSetEnableEncToken(bool mode)
        {
            int result = SA_CSI_SetEnableEncToken(mode);

            if (result == 0) { return true; }

            return false;
        }
        //////////////////////////////////////////
        // GetLoginInfo Test
        // 로그인 성공한 경우 User 정보를 확인한다.
        //////////////////////////////////////////
        private static string WrapperGetLoginUserInfo()
        {
            StringBuilder strKey = new StringBuilder();
            strKey.Append("id");

            StringBuilder strUserID = new StringBuilder(128, 128);

            int result = SA_CSI_GetLoginUserInfo(strKey.ToString(), strUserID, 128);

            string resValue = string.Empty;

            if (0 == result)
            {
                //로그인 사용자 정보 조회 >  사용자 ID = " + strUserID);                
                resValue = strUserID.ToString();
            }
            return resValue;
        }

        #endregion
    }
}



//////using System;
//////using System.Collections.Generic;
//////using System.Linq;
//////using System.Reflection;
//////using System.Windows.Forms;
//////using System.Runtime.InteropServices;
//////using System.Configuration;
//////using System.Data.SqlClient;
//////using System.Text;
//////using System.Data;
//////using SAMMI.Common;
//////using Microsoft.Practices.EnterpriseLibrary.Data;
//////using Microsoft.Practices.EnterpriseLibrary.Common;

//////namespace SmartMES
//////{

//////    static class Program
//////    {
//////        //public static string UserPlantCode = string.Empty;

//////        [DllImport("user32.dll")]
//////        public static extern void BringWindowToTop(IntPtr hWnd);

//////        [DllImport("User32", EntryPoint = "SetForegroundWindow")]
//////        private static extern bool SetForegroundWindow(IntPtr hWnd);

//////        [DllImport("User32")]
//////        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

//////        #region <SSO-openAPI>

//////        [DllImport("SA_CSI.dll")]
//////        public static extern int SA_CSI_CheckServer(string strCheckServerURL, int strServerPort);

//////        [DllImport("SA_CSI.dll")]
//////        public static extern int SA_CSI_SSO_Login(string strServiceID, int port, string strLogInURL, string strGetChallengeURL, string strGetServiceInfoURL, string strGetPublicKeyURL, string strChangePWURL, string strTokenAuthoriaztionURL);

//////        [DllImport("SA_CSI.dll")]
//////        public static extern int SA_CSI_SSO_Logout(string strLogOutURL, int port);

//////        [DllImport("SA_CSI.dll")]
//////        public static extern int SA_CSI_SSO_TokenLogin(string strLogInURL, int port, string strServiceID);

//////        [DllImport("SA_CSI.dll")]
//////        public static extern int SA_CSI_IDPW_Authentication(string strLogInURL, int port, string strServiceID, string id, string pwd);

//////        [DllImport("SA_CSI.dll")]
//////        public static extern int SA_CSI_GetLoginUserInfo(string strKey, StringBuilder strValue, int buffsize);

//////        [DllImport("kernel32")]
//////        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

//////        [DllImport("SA_CSI.dll")]
//////        public static extern int SA_CSI_SetEnableEncToken(Boolean mode);

//////        [DllImport("SA_CSI.dll")]
//////        public static extern int SA_CSI_GetLastErrorInfo(StringBuilder strErrorCode, int codebuff, StringBuilder strErrorMsg, int msgbuff);

//////        private static string strLogInURL          = "https://sso.samkee.com/openapi/authentication/login";
//////        private static string strLogOutURL         = "https://sso.samkee.com/openapi/authentication/logout";
//////        private static string strGetChallengeURL   = "https://sso.samkee.com/openapi/authentication/challenge/get";
//////        private static string strGetServiceInfoURL = "https://sso.samkee.com/openapi/service/info/get";
//////        private static string strGetPubkeyURL      = "https://sso.samkee.com/openapi/authentication/publickey/get";
//////        private static string strAuthURL           = "https://sso.samkee.com/token/authorization";
//////        private static string strChangePWURL       = "https://sso.samkee.com/openapi/authentication/password/change";
//////        private static string strCheckServerURL    = "https://sso.samkee.com/openapi/checkserver/";  //도메인

//////        // ERP:4 , MES:5 && Token Encrypt Mode, Encrypt = true
//////        private static string  strServiceID  = "5";
//////        private static string  strServerPort = "443";                
//////        private static Boolean TokenEncMode = true;

//////        #endregion

//////        /// <summary>
//////        /// 해당 응용 프로그램의 주 진입점입니다.
//////        /// </summary>
//////        [STAThread]
//////        static void Main(string[] args)
//////        {
//////            string userid = string.Empty;
//////            string userName = string.Empty;

//////            // 동일프로그램 실행 확인(동일프로그램일 경우, 실행중인 프로그램을 맨앞으로 가지고 온다.)
//////            if (SmartMES.Program.CheckMultiProcess()) return;

//////            // 프로그램 전체에서 사용되어지는 Style 파일을 정의
//////            Infragistics.Win.AppStyling.StyleManager.Load(Application.StartupPath + @"\Style.isl");
//////            Application.EnableVisualStyles();
//////            Application.SetCompatibleTextRenderingDefault(false);

//////            #region <File Update>
//////            SAMMI.Windows.Forms.UpgradeForm zz0100 = new SAMMI.Windows.Forms.UpgradeForm(userid, global::SmartMES.Properties.Resources.LOGIN3);

//////            // 라이브 업데이트시 LOCK이 걸린 경우 RESTART
//////            if (zz0100.DialogResult == DialogResult.Cancel)
//////            {
//////                MessageBox.Show("프로그램 구성이 바뀌었습니다. 재실행해 주십시오.");
//////                //Application.Restart();
//////                return;
//////            }
//////            #endregion

//////            #region <SSO Login>

//////            //Configuration appconfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

//////            if (WrapperCheckServer() == true) //SSO서버 살아있는지 확인
//////            {
//////                if (WrapperTokenLogin() == true) // 로그인토큰 생성 확인
//////                {
//////                    userid = WrapperGetLoginUserInfo();
//////                    if (!string.IsNullOrEmpty(userid))
//////                    {                        
//////                        //사용자 기본정보
//////                        Database db = DatabaseFactory.CreateDatabase();                        
//////                        DataTable dt = db.ExecuteDataSet("USP_ZGETWORKERINFO_S1", userid).Tables[0];

//////                        if (SqlDBHelper.nvlInt(dt.Rows.Count) == 0)
//////                        {
//////                            System.Collections.Specialized.NameValueCollection sitecollection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("site");
//////                            string conStr = sitecollection["EV"].ToString();

//////                            using (SqlConnection connection = new SqlConnection(conStr))
//////                            {                                
//////                                SqlCommand command = new SqlCommand("USP_ZGETWORKERINFO_S1", connection);
//////                                command.CommandType = CommandType.StoredProcedure;
//////                                command.Parameters.Add(new SqlParameter("pWorkerID", userid));
//////                                command.Connection.Open();
//////                                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
//////                                sqlDataAdapter.Fill(dt);                                    
//////                                //if (dt != null && Convert.ToInt16(dt.Rows[0][0]) > 0) { return true; }
//////                            }
//////                            if (SqlDBHelper.nvlInt(dt.Rows.Count) > 0)
//////                            {
//////                                ZZ0000 zz0000 = new ZZ0000("T");
//////                                if (zz0000.DialogResult != DialogResult.OK)
//////                                    return;

//////                                userid = zz0000.UserID;
//////                                userName = zz0000.UserName;

//////                                SmartMES.Program.RunApplication(new string[] { userid, userName });
//////                                return;
//////                            }
//////                            else
//////                            {
//////                                MessageBox.Show("사용자 정보가 없습니다. 다시 확인해주세요!");
//////                                return;
//////                            }                                                       
//////                            //if (SqlDBHelper.nvlInt(dt.Rows.Count) == 0)
//////                            //{
//////                            //    MessageBox.Show("사용자 정보가 없습니다. 다시 확인해주세요!");                                
//////                            //    return;
//////                            //}
//////                        }                             
//////                        userName = SqlDBHelper.nvlString(dt.Rows[0]["WorkerName"]);

//////                        LoginInfo.UserPlantCode = SqlDBHelper.nvlString(dt.Rows[0]["PlantCode"]);
//////                        LoginInfo.UserPlantName = SqlDBHelper.nvlString(dt.Rows[0]["PlantName"]);
//////                        LoginInfo.PlantAuth = SqlDBHelper.nvlString(dt.Rows[0]["PlantAuth"]);

//////                        SmartMES.Program.RunApplication(new string[] { userid, userName });
//////                        return;
//////                    }
//////                }
//////                else 
//////                {
//////                    ZZ0000 zz0000 = new ZZ0000("T");
//////                    if (zz0000.DialogResult != DialogResult.OK)
//////                        return;

//////                    userid = zz0000.UserID;
//////                    userName = zz0000.UserName;

//////                    SmartMES.Program.RunApplication(new string[] { userid, userName });
//////                    return;
//////                }
//////            }
//////            else // SSO서버다운 (ERP로그인)
//////            {
//////                ZZ0000 zz0000 = new ZZ0000("S");
//////                if (zz0000.DialogResult != DialogResult.OK)
//////                    return;

//////                userid = zz0000.UserID;
//////                userName = zz0000.UserName;

//////                SmartMES.Program.RunApplication(new string[] { userid, userName });
//////            }
//////            #endregion

//////            #region <기존 Login(주석처리)>
//////            //// 로그인 실패시 종료
//////            //ZZ0000 zz0000 = new ZZ0000((args.Length > 0 ? args[0] : ""));
//////            // if (zz0000.DialogResult != DialogResult.OK)
//////            //    return;

//////            //userid = zz0000.UserID;
//////            //userName = zz0000.UserName;

//////            //// 프로그램 실행
//////            //SmartMES.Program.RunApplication(new string[] { userid, userName });
//////            #endregion
//////        }

//////        /// <summary>
//////        /// 중복프로그램 실행 확인
//////        /// </summary>
//////        /// <returns></returns>
//////        public static Boolean CheckMultiProcess()
//////        {
//////            int thisID = System.Diagnostics.Process.GetCurrentProcess().Id; // 현재 기동한 프로그램 id

//////            //실행중인 프로그램중 현재 기동한 프로그램과 같은 프로그램들 수집
//////            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

//////            if (p.Length > 1)
//////            {
//////                for (int i = 0; i < p.Length; i++)
//////                {
//////                    if (p[i].Id == thisID) continue;

//////                    ShowWindow(p[i].MainWindowHandle, 1);
//////                    BringWindowToTop(p[i].MainWindowHandle);
//////                    SetForegroundWindow(p[i].MainWindowHandle);

//////                    break;
//////                }
//////                return true;
//////            }

//////            return false;
//////        }
//////        /// <summary>
//////        /// APPLICATION 실행
//////        /// </summary>
//////        /// <param name="args"></param>
//////        public static void RunApplication(params object[] args)
//////        {
//////            Assembly assembly;
//////            Type typeForm;
//////            Form newForm;
//////            Configuration appconfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

//////            assembly = Assembly.LoadFrom(Application.StartupPath + @"\" + appconfig.AppSettings.Settings["STARTFORMFILE"].Value.ToString());
//////            typeForm = assembly.GetType(appconfig.AppSettings.Settings["STARTFORM"].Value.ToString(), true);
//////            newForm = (Form)Activator.CreateInstance(typeForm, args);            

//////            Application.Run(newForm);
//////        }
//////        #region <SSO Function>
//////        //////////////////////////////////////////
//////        // CheckServer Test
//////        //////////////////////////////////////////
//////        private static Boolean WrapperCheckServer()
//////        {
//////            Boolean ret = true;

//////            int result = SA_CSI_CheckServer(strCheckServerURL.ToString(), Int32.Parse(strServerPort.ToString()));

//////            if (result != 0) { ret = false; }
//////            return ret;
//////        }
//////        private static Boolean WrapperIDPWLogin(string sId, string sPw)
//////        {
//////            Boolean ret = true;
//////            int apiResult = 0;

//////            WrapperSetEnableEncToken(TokenEncMode);
//////            apiResult = SA_CSI_IDPW_Authentication(strLogInURL,
//////                                                     Int32.Parse(strServerPort.ToString()),
//////                                                     strServiceID,
//////                                                     sId,
//////                                                     sPw);
//////            if (0 == apiResult) { } 
//////            else { ret = false; }   

//////            return ret;
//////        }
//////        //////////////////////////////////////////
//////        // Token Login Test
//////        //////////////////////////////////////////
//////        private static Boolean WrapperTokenLogin()
//////        {
//////            Boolean ret = true;

//////            WrapperSetEnableEncToken(TokenEncMode);

//////            int result = SA_CSI_SSO_TokenLogin(strAuthURL.ToString(),
//////                                                Int32.Parse(strServerPort.ToString()),
//////                                                strServiceID.ToString());
//////            if (0 == result) { }//Console.WriteLine("    토큰 로그인 성공!! \n");   
//////            else { ret = false; }

//////            return ret;
//////        }
//////        //////////////////////////////////////////
//////        // LogoutTest
//////        //////////////////////////////////////////
//////        private static Boolean WrapperLogOut()
//////        {
//////            Boolean ret = true;
//////            int result = SA_CSI_SSO_Logout(strLogOutURL, Int32.Parse(strServerPort.ToString()));
//////            if (0 == result) { } 
//////            else { ret = false; }

//////            return ret;
//////        }
//////        //////////////////////////////////////////
//////        // Set Token Encrypt Mode
//////        // param - Encrypt mode = true 
//////        //////////////////////////////////////////
//////        private static Boolean WrapperSetEnableEncToken(bool mode)
//////        {
//////            int result = SA_CSI_SetEnableEncToken(mode);

//////            if (result == 0) { return true; } 
            
//////            return false;
//////        }
//////        //////////////////////////////////////////
//////        // GetLoginInfo Test
//////        // 로그인 성공한 경우 User 정보를 확인한다.
//////        //////////////////////////////////////////
//////        private static string WrapperGetLoginUserInfo()
//////        {
//////            StringBuilder strKey = new StringBuilder();
//////            strKey.Append("id");

//////            StringBuilder strUserID = new StringBuilder(128, 128);

//////            int result = SA_CSI_GetLoginUserInfo(strKey.ToString(), strUserID, 128);

//////            string resValue = string.Empty;

//////            if (0 == result)
//////            {
//////                //로그인 사용자 정보 조회 >  사용자 ID = " + strUserID);                
//////                resValue = strUserID.ToString();
//////            }
//////            return resValue;
//////        }

//////        #endregion
//////    }
//////}
