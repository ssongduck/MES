
 //*---------------------------------------------------------------------------------------------*
 //  Form ID        : ZZ0000
 //  Form Name      : 로그인
 //  Name Space     : SmartMES
 //  Created Date   : 2012.03.16
 //  Made By        : SAMMI INFORMATION SYSTEM CO.,LTD
 //  Edit By        : 정용석
 //  Edited  Date   : 2022.05.10
 //  Description    : 삼기, 삼기이브이 통합로그인 
 //*---------------------------------------------------------------------------------------------*

using System;
using System.Configuration;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using SAMMI.Common;
using System.Runtime.InteropServices;

namespace SmartMES
{
    public partial class ZZ0000 : Form
    {
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
        private static string otpVerifyUrl         = "https://sso.samkee.com/api/v1/auth/otp/signin";
        private static string otpPushUrl           = "https://sso.samkee.com/api/v1/auth/otp/push";
        private static string apcPolicyUrl         = "https://sso.samkee.com/api/v1/policy";
        private static string apcTokenLogin        = "https://sso.samkee.com/api/v1/sso/validate";
        private static string strCheckServerURL    = "https://sso.samkee.com/openapi/checkserver/";  //도메인

        // ERP:4 , MES:5
        private static string strServiceID = "5";
        private static string strServerPort = "443";
        private static Boolean TokenEncMode = true;

        #endregion

        private const int LOGIN_COUNT = 3;
        private bool ismove = false;
        private System.Drawing.Point temp;
        private Configuration appConfig;
        private Database db;
        private DataTable dtSite;
        public DbCommand InsertCmd;
        private int loginCnt = 0;
        public string UserID = "SYSTEM";
        public string UserName = "";
        //string _session_key = "";
        string _status = "";
        bool bSloLogin = false;

        public ZZ0000(string skey)
        {
            // 회사구분 선택시 Default ConnectionStinrg의 변경됨. 
            // 로그인정보는 MES평택(190.168.50.2)서버로 접속해야됨.
            this.db = DatabaseFactory.CreateDatabase();

            _status = skey;
            InitializeComponent();

            appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            System.Collections.Specialized.NameValueCollection sitecollection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("site");
            System.Collections.Specialized.NameValueCollection sitenamecollection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("sitename");

            dtSite = new DataTable();
            dtSite.Columns.Add("SITE");
            dtSite.Columns.Add("SITENAME");

            for (int i = 0; i < sitenamecollection.Count; i++)
            {
                DataRow row = dtSite.NewRow();
                row["SITE"] = sitenamecollection.Keys[i];
                row["SITENAME"] = sitenamecollection[sitenamecollection.Keys[i]];
                dtSite.Rows.Add(row);
            }

            this.cboSite.DataSource = dtSite;
            this.cboSite.ValueMember = "SITE";
            this.cboSite.DisplayMember = "SITENAME";
            this.cboSite.Value = appConfig.AppSettings.Settings["SITE"].Value;

            this.cboSite.DisplayLayout.Bands[0].Columns[0].Width = 40;
            this.cboSite.DisplayLayout.Bands[0].Columns[1].Width = 100;
            this.cboSite.DisplayLayout.Bands[0].Columns[0].Header.Caption = "코드";
            this.cboSite.DisplayLayout.Bands[0].Columns[1].Header.Caption = "회사(공장)";
            this.cboSite.DisplayLayout.Bands[0].Columns[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            this.cboSite.DisplayLayout.Bands[0].Columns[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

            //if (appConfig.AppSettings.Settings["SITE"].Value == "EC") // sitecollection.GetValues(0)[0].IndexOf("60.2") != -1 )            
            //this.Icon             = SmartMES.Properties.Resources.Lock;

            this.txtWorkerID.Text = appConfig.AppSettings.Settings["LOGINID"].Value.ToString();
            this.loginCnt = LOGIN_COUNT;
            this.ShowDialog();
            this.txtPassword.Focus();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string myip = SAMMI.Common.Common.GetIPAddress();

            this.txtWorkerID.Enabled = false;
            this.txtPassword.Enabled = false;
            this.btnConfirm.Enabled = false;
            this.btnConfig.Enabled = false;
            this.btnClose.Enabled = false;
            this.btnChange.Enabled = false;

            if ((this.txtWorkerID.Text == "SYSTEM") && (this.txtPassword.Text.ToUpper() == "S491@0341"))
            {
                appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                appConfig.AppSettings.Settings["LOGINID"].Value = this.txtWorkerID.Text;
                appConfig.Save();

                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                SaveDoWorkType("SYSTEM", myip, "LOGIN");

                this.Close();
                return;
            }
            try
            {
                ConfigurationManager.RefreshSection("connectionStrings");
                ConfigurationManager.RefreshSection("configuration");

                string grpid = string.Empty;
                string pwd = string.Empty;

                //this.db = DatabaseFactory.CreateDatabase(((System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("site"))[0].ToString());
                //this.db = DatabaseFactory.CreateDatabase();

                DataTable dt = this.db.ExecuteDataSet("USP_ZGETWORKERINFO_S2", this.txtWorkerID.Text).Tables[0];

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("사용자 정보가 없거나 비활성 상태입니다. 다시 확인하세요.", "로그인실패", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button3);
                    this.txtWorkerID.Enabled = true;
                    this.txtPassword.Enabled = true;
                    this.btnConfirm.Enabled = true;
                    this.btnConfig.Enabled = true;
                    this.btnClose.Enabled = true;
                    this.btnChange.Enabled = true;
                    return;
                }

                if (cboSite.Value.ToString().Contains("SK"))
                {
                    if (SqlDBHelper.nvlString(dt.Rows[0]["WorkerID"]) == "")
                    {
                        MessageBox.Show("선택한 회사의 로그인 권한이 없습니다.", "로그인실패", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button3);
                        this.txtWorkerID.Enabled = true;
                        this.txtPassword.Enabled = true;
                        this.btnConfirm.Enabled = true;
                        this.btnConfig.Enabled = true;
                        this.btnClose.Enabled = true;
                        this.btnChange.Enabled = true;
                        return;
                    }
                    UserName = SqlDBHelper.nvlString(dt.Rows[0]["WorkerName"]);
                    pwd = SqlDBHelper.nvlString(dt.Rows[0]["Pwd"]);
                    LoginInfo.UserID = UserID = SqlDBHelper.nvlString(dt.Rows[0]["WorkerID"]);
                    LoginInfo.UserPlantCode = SqlDBHelper.nvlString(dt.Rows[0]["PlantCode"]);
                    LoginInfo.PlantAuth = SqlDBHelper.nvlString(dt.Rows[0]["PlantAuth"]);
                    LoginInfo.UserPlantName = "삼기";

                }
                else
                {
                    if (SqlDBHelper.nvlString(dt.Rows[0]["EV_WorkerID"]) == "")
                    {
                        MessageBox.Show("선택한 회사의 로그인 권한이 없습니다.", "로그인실패", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button3);
                        this.txtWorkerID.Enabled = true;
                        this.txtPassword.Enabled = true;
                        this.btnConfirm.Enabled = true;
                        this.btnConfig.Enabled = true;
                        this.btnClose.Enabled = true;
                        this.btnChange.Enabled = true;
                        return;
                    }
                    UserName = SqlDBHelper.nvlString(dt.Rows[0]["EV_WorkerName"]);
                    pwd = SqlDBHelper.nvlString(dt.Rows[0]["EV_Pwd"]);
                    LoginInfo.UserID = UserID = SqlDBHelper.nvlString(dt.Rows[0]["EV_WorkerID"]);
                    LoginInfo.UserPlantCode = SqlDBHelper.nvlString(dt.Rows[0]["EV_PlantCode"]);
                    LoginInfo.PlantAuth = SqlDBHelper.nvlString(dt.Rows[0]["EV_PlantAuth"]);
                    LoginInfo.UserPlantName = "삼기ev";
                }
                grpid = SqlDBHelper.nvlString(dt.Rows[0]["GRPID"]);
                string cid = this.txtWorkerID.Text.Trim();
                string cpwd = this.txtPassword.Text.Trim();

                #region <기존 SLO로그인(주석처리)>
                //if (bSloLogin == false)
                //{
                //    // MD5 암호화 
                //  cpwd= System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(cpwd, "MD5");
                //}
                //if (pwd != cpwd)
                ////if ( pwd == "2564EAD42B0E95078F497F60C2B01BA9")
                //{
                //    loginCnt--;
                //    if (loginCnt == 0)
                //    {
                //        MessageBox.Show("로그인에 실패 했습니다. 시스템을 종료합니다.");
                //        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                //        this.Close();
                //    }
                //    else
                //    {
                //        MessageBox.Show("사용자 아이디나, 패스워드가 틀립니다.");
                //        this.txtWorkerID.Enabled = true;
                //        this.txtPassword.Enabled = true;
                //        this.btnConfirm.Enabled = true;
                //        this.btnConfig.Enabled = true;
                //        this.btnClose.Enabled = true;
                //        this.btnChange.Enabled = true;
                //        bSloLogin = false;
                //    }                
                //    return;
                //}
                #endregion

                #region <SSO 로그인>
                switch (_status)
                {
                    case "T": // SSO서버 로그인 (토큰생성)
                        if (WrapperIDPWLogin(cid, cpwd))
                        {
                            WrapperVerifyToken(); //로그인 인증 완료 후 발급되는 임시토큰을 정상토큰으로 업데이트                     
                        }
                        else // ERP계정 정보로 로그인 (토큰생성X)
                        {
                            if (pwd != cpwd)
                            {
                                loginCnt--;
                                if (loginCnt == 0)
                                {
                                    MessageBox.Show("로그인에 실패 했습니다. 시스템을 종료합니다.");
                                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("사용자 정보와 비밀번호가 일치하지 않습니다.");
                                    this.txtWorkerID.Enabled = true;
                                    this.txtPassword.Enabled = true;
                                    this.btnConfirm.Enabled = true;
                                    this.btnConfig.Enabled = true;
                                    this.btnClose.Enabled = true;
                                    this.btnChange.Enabled = true;
                                    bSloLogin = false;
                                }
                                return;
                            }
                        }
                        break;
                    case "S": // ERP계정 정보로 로그인 (토큰생성X)
                        if (pwd != cpwd)
                        {
                            loginCnt--;
                            if (loginCnt == 0)
                            {
                                MessageBox.Show("로그인에 실패 했습니다. 시스템을 종료합니다.");
                                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("[통합인증서버 다운]ERP계정 비밀번호로 접속 바랍니다.");
                                this.txtWorkerID.Enabled = true;
                                this.txtPassword.Enabled = true;
                                this.btnConfirm.Enabled = true;
                                this.btnConfig.Enabled = true;
                                this.btnClose.Enabled = true;
                                this.btnChange.Enabled = true;
                                bSloLogin = false;
                            }
                            return;
                        }
                        break;
                }
                #endregion

                if (grpid == "")
                {
                    MessageBox.Show("그룹권한이 없습니다. 관리자에게 문의하시기 바랍니다.");
                    this.txtWorkerID.Enabled = true;
                    this.txtPassword.Enabled = true;
                    this.btnConfirm.Enabled = true;
                    this.btnConfig.Enabled = true;
                    this.btnClose.Enabled = true;
                    this.btnChange.Enabled = true;
                    return;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("연결 상태를 확인하세요.");
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            #region 로그인내역
            using (DbConnection dbconn = this.db.CreateConnection())
            {
                dbconn.Open();
                DbTransaction dbtrans = dbconn.BeginTransaction();
                try
                {
                    this.InsertCmd = this.db.GetStoredProcCommand("USP_SY0300_I1");
                    this.db.AddInParameter(InsertCmd, "@MakeDate", DbType.Date);
                    this.db.AddInParameter(InsertCmd, "@Maker", DbType.String);
                    this.db.AddInParameter(InsertCmd, "@IPAddress", DbType.String);

                    this.db.SetParameterValue(InsertCmd, "@MakeDate", System.DateTime.Now);
                    this.db.SetParameterValue(InsertCmd, "@Maker", this.txtWorkerID.Text);
                    this.db.SetParameterValue(InsertCmd, "@IPAddress", myip);
                    this.db.ExecuteNonQuery(InsertCmd, dbtrans);

                    dbtrans.Commit();
                }
                catch (Exception)
                {
                    dbtrans.Rollback();
                }
                dbconn.Close();
            }
            #endregion

            this.UserID = this.txtWorkerID.Text.ToUpper();

            SaveDoWorkType(this.UserID, myip, "LOGIN"); //로그인상세내역

            appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            appConfig.AppSettings.Settings["LOGINID"].Value = this.txtWorkerID.Text;
            appConfig.Save();
        }

        private void SaveDoWorkType(string userid, string IP, string sType)
        {
            try
            {
                SAMMI.Common.SqlDBHelper helper = new SAMMI.Common.SqlDBHelper(false);
                SqlParameter[] param = null;
                try
                {
                    param = new SqlParameter[6];

                    param[0] = helper.CreateParameter("@pProgramID", "MAIN", SqlDbType.VarChar, ParameterDirection.Input);   // 공장코드
                    param[1] = helper.CreateParameter("@pWorkerID", userid, SqlDbType.VarChar, ParameterDirection.Input);    // 품목코드
                    param[2] = helper.CreateParameter("@pWorkType", sType, SqlDbType.VarChar, ParameterDirection.Input);     // 관리항목
                    param[3] = helper.CreateParameter("@pIP", IP, SqlDbType.VarChar, ParameterDirection.Input);              // 관리항목
                    param[4] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                    param[5] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                    helper.ExecuteNoneQuery("USP_SY0700_I1", CommandType.StoredProcedure, param);

                    if (param[4].Value.ToString() != "")
                    {
                        throw new Exception(param[5].Value.ToString());
                    }

                    helper.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    helper.Transaction.Rollback();
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    if (helper._sConn != null) { helper._sConn.Close(); }
                    if (param != null) { param = null; }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("인터넷 연결을 확인하세요.");
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            ZZ0200 zz0200 = new ZZ0200();
            zz0200.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.cboSite.Value = appConfig.AppSettings.Settings["SITE"].Value;
            this.Close();
        }

        private void gbxLogin_MouseUp(object sender, MouseEventArgs e)
        {
            this.ismove = false;
        }

        private void gbxLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.ismove)
            {
                System.Drawing.Point d = Control.MousePosition;
                d.X = d.X - temp.X;
                d.Y = d.Y - temp.Y;
                this.Location = d;
            }
        }

        private void gbxLogin_MouseDown(object sender, MouseEventArgs e)
        {
            this.ismove = true;

            temp.X = Control.MousePosition.X - this.Location.X;
            temp.Y = Control.MousePosition.Y - this.Location.Y;
        }

        private void ZZ0000_Load(object sender, EventArgs e)
        {
            try
            {
                DbCommand controlNameCmd = this.db.GetStoredProcCommand("USP_ZZ0000_S1");
                this.db.AddInParameter(controlNameCmd, "@ProgramID", DbType.String, this.Name);
                this.db.AddInParameter(controlNameCmd, "@Lang", DbType.String, "KO");
                DataSet dsCaption = this.db.ExecuteDataSet(controlNameCmd);

                for (int i = 0; i < dsCaption.Tables[0].Rows.Count; i++)
                {
                    SAMMI.Common.Common.FindControl(this.Controls, dsCaption.Tables[0].Rows[i]["ControlID"].ToString()).Text = dsCaption.Tables[0].Rows[i]["Caption"].ToString();
                }

            }
            catch (Exception)
            {
            }
            #region <SLO 로그인(주석처리)>
            //if (_session_key != "") // SLO 
            //{
            //    /*그룹웨어 로그인*/
            //    try
            //    {

            //        SLOAGENTLib.JHWebCtl slo = new SLOAGENTLib.JHWebCtl();
            //        slo.login("gw.samkee.com", "80", _session_key);
            //        this.txtWorkerID.Text = slo.getvalue("username");
            //        this.txtPassword.Text = slo.getvalue("sessionpassword");

            //        if (this.txtWorkerID.Text != "")
            //        {
            //            bSloLogin = true;
            //            this.Invoke(new EventHandler(btnConfirm_Click));
            //        }
            //    }
            //    catch { }
            //}
            #endregion
        }

        private void cboSite_ValueChanged(object sender, EventArgs e)
        {
            if (!this.btnConfirm.Enabled) { return; }

            Configuration appConfig1 = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            System.Collections.Specialized.NameValueCollection sitecollection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("site");

            for (int i = 0; i < appConfig1.ConnectionStrings.ConnectionStrings.Count; i++)
            {
                appConfig1.ConnectionStrings.ConnectionStrings[i].ConnectionString = sitecollection[cboSite.Value.ToString()].ToString();
            }

            appConfig1.ConnectionStrings.ConnectionStrings["ConnectionString"].ConnectionString = sitecollection[cboSite.Value.ToString()].ToString();
            string[] serverip = sitecollection[cboSite.Value.ToString()].Split(new char[] { '=', ';' });

            appConfig1.AppSettings.Settings["SITE"].Value = this.cboSite.Value.ToString();  // 연결MES서버 Alias
            appConfig1.AppSettings.Settings["DBSERVER1"].Value = serverip[1];                    // 연결서버주조
            appConfig1.AppSettings.Settings["DBUSER1"].Value = serverip[5];                    // DB계정
            appConfig1.AppSettings.Settings["DBPASSWORD1"].Value = serverip[7];                    // DB패스워드

            appConfig1.Save();
            ConfigurationManager.RefreshSection("configuration");

            if (this.cboSite.Value.ToString().Contains("SK"))
            {
                gbxLogin.Appearance.ImageBackground = global::SmartMES.Properties.Resources.SK_logo;
            }
            else
            {
                gbxLogin.Appearance.ImageBackground = global::SmartMES.Properties.Resources.SKev_logo;
            }
        }

        private void cboSite_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            ZZ0300 zz0300 = new ZZ0300(txtWorkerID.Text.Trim());
            zz0300.ShowDialog();
        }
        #region <SSO Function>
        /// <summary>
        /// SSO서버 Alive 체크
        /// </summary>
        /// <returns></returns>
        private Boolean WrapperCheckServer()
        {
            Boolean ret = true;

            int result = SA_CSI_CheckServer(strCheckServerURL.ToString(), Int32.Parse(strServerPort.ToString()));

            if (result != 0) { ret = false; }
            return ret;
        }
        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="sId"></param>
        /// <param name="sPw"></param>
        /// <returns></returns>
        private Boolean WrapperIDPWLogin(string sId, string sPw)
        {
            Boolean ret = true;
            int apiResult = 0;

            //WrapperSetEnableEncToken(TokenEncMode);
            apiResult = SA_CSI_IDPW_Authentication(strLogInURL,
                                                     Int32.Parse(strServerPort.ToString()),
                                                     strServiceID,
                                                     sId,
                                                     sPw);
            if (0 == apiResult) { }
            else { ret = false; }

            return ret;
        }
        /// <summary>
        /// 토큰 로그인
        /// </summary>
        /// <returns></returns>
        private Boolean WrapperTokenLogin()
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
        /// <summary>
        /// 로그아웃
        /// </summary>
        /// <returns></returns>
        private Boolean WrapperLogOut()
        {
            Boolean ret = true;
            int result = SA_CSI_SSO_Logout(strLogOutURL, Int32.Parse(strServerPort.ToString()));
            if (0 == result) { }
            else { ret = false; }

            return ret;
        }
        /// <summary>
        /// 토큰 Verify
        /// </summary>
        /// <returns></returns>
        private Boolean WrapperVerifyToken()
        {
            Boolean ret = true;

            WrapperSetEnableEncToken(TokenEncMode);

            int result = SA_CSI_SSO_TokenLogin(strAuthURL.ToString(),
                                                Int32.Parse(strServerPort.ToString()),
                                                strServiceID.ToString());
            if (0 == result)
            {
            }
            else
            {
                ret = false;
            }
            return ret;
        }
        //////////////////////////////////////////
        // Set Token Encrypt Mode
        // param - Encrypt mode = true 
        //////////////////////////////////////////
        private Boolean WrapperSetEnableEncToken(bool mode)
        {
            int result = SA_CSI_SetEnableEncToken(mode);

            if (result == 0) { return true; }
            return false;
        }
        /// <summary>
        /// 사용자 정보
        /// </summary>
        /// <returns></returns>
        private string WrapperGetLoginUserInfo()
        {
            StringBuilder strKey = new StringBuilder();
            strKey.Append("id");

            StringBuilder strUserID = new StringBuilder(128, 128);

            int result = SA_CSI_GetLoginUserInfo(strKey.ToString(), strUserID, 128);

            string resValue = string.Empty;

            if (0 == result)
            {
                resValue = strUserID.ToString();
            }
            return resValue;
        }
        /// <summary>
        /// 에러정보 확인
        /// </summary>
        private static void WrapperGetLastErrorInfo()
        {
            StringBuilder strErrorCode = new StringBuilder(128, 128);
            StringBuilder strErrorMsg = new StringBuilder(1024, 1024);
            int result = SA_CSI_GetLastErrorInfo(strErrorCode, 128, strErrorMsg, 1024);

            if (0 == result)
            {
            }
            else
            {
            }
        }
        #endregion
    }
}


//////// *---------------------------------------------------------------------------------------------*
////////   Form ID        : ZZ0000
////////   Form Name      : 코드마스터
////////   Name Space     : MAIN
////////   Created Date   : 2012.03.16
////////   Made By        : SAMMI INFORMATION SYSTEM CO.,LTD
////////   Edit By        : 정용석
////////   Edited  Date   : 2021.04.18
////////   Description    : SSO(Single Sign On) Login로직 변경
//////// *---------------------------------------------------------------------------------------------*

//////using System;
//////using System.Configuration;
//////using System.Net;
//////using System.Collections.Generic;
//////using System.ComponentModel;
//////using System.Data;
//////using System.Data.Common;
//////using System.Data.SqlClient;
//////using System.Drawing;
//////using System.Linq;
//////using System.Reflection;
//////using System.Text;
//////using System.Windows.Forms;
//////using Microsoft.Practices.EnterpriseLibrary.Data;
//////using Microsoft.Practices.EnterpriseLibrary.Common;
//////using SAMMI.Common;
//////using System.Runtime.InteropServices;

//////namespace SmartMES
//////{    
//////    public partial class ZZ0000 : Form
//////    {
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
//////        private static string otpVerifyUrl         = "https://sso.samkee.com/api/v1/auth/otp/signin";
//////        private static string otpPushUrl           = "https://sso.samkee.com/api/v1/auth/otp/push";
//////        private static string apcPolicyUrl         = "https://sso.samkee.com/api/v1/policy";
//////        private static string apcTokenLogin        = "https://sso.samkee.com/api/v1/sso/validate";
//////        private static string strCheckServerURL    = "https://sso.samkee.com/openapi/checkserver/";  //도메인

//////        // ERP:4 , MES:5
//////        private static string strServiceID  = "5";
//////        private static string strServerPort = "443";                
//////        private static Boolean TokenEncMode = true;

//////        #endregion

//////        private const int LOGIN_COUNT = 3;
//////        private bool ismove           = false;
//////        private System.Drawing.Point  temp;
//////        private Configuration         appConfig;
//////        private Database              db;
//////        private DataTable             dtSite;
//////        public  DbCommand             InsertCmd;
//////        private int                   loginCnt    = 0;
//////        public  string                UserID      = "SYSTEM";
//////        public string UserName = "";
//////        //string _session_key = "";
//////        string _status   = "";
//////        bool   bSloLogin = false;
        
//////        public ZZ0000(string skey)
//////        {          
//////            _status = skey;
//////            InitializeComponent();

//////            appConfig             = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

//////            System.Collections.Specialized.NameValueCollection sitecollection =  (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("site");
//////            System.Collections.Specialized.NameValueCollection sitenamecollection =  (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("sitename");

//////            dtSite = new DataTable();
//////            dtSite.Columns.Add("SITE");
//////            dtSite.Columns.Add("SITENAME");

//////            for (int i = 0; i < sitenamecollection.Count; i++)
//////            {
//////                DataRow row = dtSite.NewRow();
//////                row["SITE"] = sitenamecollection.Keys[i];
//////                row["SITENAME"] = sitenamecollection[sitenamecollection.Keys[i]];
//////                dtSite.Rows.Add(row);
//////            }             
//////            this.cboSite.DataSource = dtSite;
//////            this.cboSite.ValueMember = "SITE"; 
//////            this.cboSite.DisplayMember = "SITENAME";
//////            this.cboSite.Value = appConfig.AppSettings.Settings["SITE"].Value;
            
//////            //if (appConfig.AppSettings.Settings["SITE"].Value == "EC") // sitecollection.GetValues(0)[0].IndexOf("60.2") != -1 )
//////            //    this.gbxLogin.Appearance.ImageBackground = global::SmartMES.Properties.Resources.eco;

//////            // if (appConfig.AppSettings.Settings["LOGINID"].Value != "SYSTEM")
//////            {
//////                lblSite.Visible = false;
//////                cboSite.Visible = true;
//////            }                
//////            this.txtWorkerID.Text = appConfig.AppSettings.Settings["LOGINID"].Value.ToString();
//////            //this.Icon             = SmartMES.Properties.Resources.Lock;
//////            this.loginCnt         = LOGIN_COUNT;
//////            this.ShowDialog();
//////            this.txtPassword.Focus();
//////        }        
        
//////        private void btnConfirm_Click(object sender, EventArgs e)
//////        {
//////            string myip =SAMMI.Common.Common.GetIPAddress();
           
//////            this.txtWorkerID.Enabled = false;
//////            this.txtPassword.Enabled = false;
//////            this.btnConfirm.Enabled  = false;
//////            this.btnConfig.Enabled   = false;
//////            this.btnClose.Enabled    = false;
//////            this.btnChange.Enabled   = false;

//////            if ((this.txtWorkerID.Text == "SYSTEM") && (this.txtPassword.Text.ToUpper() == "S491@0341"))
//////            {
//////                appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

//////                appConfig.AppSettings.Settings["LOGINID"].Value = this.txtWorkerID.Text;
//////                appConfig.Save();

//////                this.DialogResult = System.Windows.Forms.DialogResult.OK;

//////                SaveDoWorkType("SYSTEM", myip, "LOGIN");

//////                this.Close();
//////                return;
//////            } 
//////            try
//////            {
//////                ConfigurationManager.RefreshSection("connectionStrings");
//////                ConfigurationManager.RefreshSection("configuration");
//////                this.db = DatabaseFactory.CreateDatabase();
//////                string grpid = string.Empty;
//////                DataTable dt = this.db.ExecuteDataSet("USP_ZGETWORKERINFO_S1", this.txtWorkerID.Text).Tables[0];

//////                if (dt.Rows.Count == 0)
//////                {

//////                    System.Collections.Specialized.NameValueCollection sitecollection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("site");
//////                    string conStr = sitecollection["EV"].ToString();

//////                    using (SqlConnection connection = new SqlConnection(conStr))
//////                    {
//////                        SqlCommand command = new SqlCommand("USP_ZGETWORKERINFO_S1", connection);
//////                        command.CommandType = CommandType.StoredProcedure;
//////                        command.Parameters.Add(new SqlParameter("pWorkerID", this.txtWorkerID.Text));
//////                        command.Connection.Open();
//////                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
//////                        sqlDataAdapter.Fill(dt);
//////                        //if (dt != null && Convert.ToInt16(dt.Rows[0][0]) > 0) { return true; }
//////                    }
//////                    if (SqlDBHelper.nvlInt(dt.Rows.Count) == 0)
//////                    {
//////                        MessageBox.Show("MEMBER ID를 확인하세요.");
//////                        this.txtWorkerID.Enabled = true;
//////                        this.txtPassword.Enabled = true;
//////                        this.btnConfirm.Enabled = true;
//////                        this.btnConfig.Enabled = true;
//////                        this.btnClose.Enabled = true;
//////                        this.btnChange.Enabled = true;
//////                        return;
//////                    }
//////                }

//////                UserName = SqlDBHelper.nvlString(dt.Rows[0]["WorkerName"]);
//////                string pwd = SqlDBHelper.nvlString(dt.Rows[0]["Password"]);
//////                grpid = SqlDBHelper.nvlString(dt.Rows[0]["GRPID"]);
//////                LoginInfo.UserPlantCode = SqlDBHelper.nvlString(dt.Rows[0]["PlantCode"]);
//////                LoginInfo.UserPlantName = SqlDBHelper.nvlString(dt.Rows[0]["PlantName"]);
//////                LoginInfo.PlantAuth = SqlDBHelper.nvlString(dt.Rows[0]["PlantAuth"]);

//////                string cid  = this.txtWorkerID.Text.Trim();
//////                string cpwd = this.txtPassword.Text.Trim();
                
//////                #region <기존 SLO로그인(주석처리)>
//////                //if (bSloLogin == false)
//////                //{
//////                //    // MD5 암호화 
//////                //  cpwd= System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(cpwd, "MD5");
//////                //}
//////                //if (pwd != cpwd)
//////                ////if ( pwd == "2564EAD42B0E95078F497F60C2B01BA9")
//////                //{
//////                //    loginCnt--;
//////                //    if (loginCnt == 0)
//////                //    {
//////                //        MessageBox.Show("로그인에 실패 했습니다. 시스템을 종료합니다.");
//////                //        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
//////                //        this.Close();
//////                //    }
//////                //    else
//////                //    {
//////                //        MessageBox.Show("사용자 아이디나, 패스워드가 틀립니다.");
//////                //        this.txtWorkerID.Enabled = true;
//////                //        this.txtPassword.Enabled = true;
//////                //        this.btnConfirm.Enabled = true;
//////                //        this.btnConfig.Enabled = true;
//////                //        this.btnClose.Enabled = true;
//////                //        this.btnChange.Enabled = true;
//////                //        bSloLogin = false;
//////                //    }                
//////                //    return;
//////                //}
//////                #endregion

//////                #region <SSO 로그인>
//////                switch (_status)
//////                {
//////                    case "T":
//////                        if (WrapperIDPWLogin(cid, cpwd))
//////                        {
//////                            WrapperVerifyToken(); //로그인 인증 완료 후 발급되는 임시토큰을 정상토큰으로 업데이트                     
//////                        }
//////                        else
//////                        {
//////                            if (pwd != cpwd)
//////                            {
//////                                loginCnt--;
//////                                if (loginCnt == 0)
//////                                {
//////                                    MessageBox.Show("로그인에 실패 했습니다. 시스템을 종료합니다.");
//////                                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
//////                                    this.Close();
//////                                }
//////                                else
//////                                {
//////                                    MessageBox.Show("사용자 정보와 비밀번호가 일치하지 않습니다.");
//////                                    this.txtWorkerID.Enabled = true;
//////                                    this.txtPassword.Enabled = true;
//////                                    this.btnConfirm.Enabled = true;
//////                                    this.btnConfig.Enabled = true;
//////                                    this.btnClose.Enabled = true;
//////                                    this.btnChange.Enabled = true;
//////                                    bSloLogin = false;
//////                                }
//////                                return;
//////                            }                            
//////                        }
//////                        break;
//////                    case "S":
//////                        if (pwd != cpwd)
//////                        {
//////                            loginCnt--;
//////                            if (loginCnt == 0)
//////                            {
//////                                MessageBox.Show("로그인에 실패 했습니다. 시스템을 종료합니다.");
//////                                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
//////                                this.Close();
//////                            }
//////                            else
//////                            {
//////                                MessageBox.Show("[통합인증서버 다운]ERP계정 비밀번호로 접속 바랍니다.");
//////                                this.txtWorkerID.Enabled = true;
//////                                this.txtPassword.Enabled = true;
//////                                this.btnConfirm.Enabled = true;
//////                                this.btnConfig.Enabled = true;
//////                                this.btnClose.Enabled = true;
//////                                this.btnChange.Enabled = true;
//////                                bSloLogin = false;
//////                            }
//////                            return;
//////                        }
//////                        break;
//////                    default:
//////                        break;
//////                }
//////                #endregion

//////                if (grpid == "")
//////                {
//////                    MessageBox.Show("그룹권한이 없습니다. 관리자에게 문의하시기 바랍니다.");
//////                    this.txtWorkerID.Enabled = true;
//////                    this.txtPassword.Enabled = true;
//////                    this.btnConfirm.Enabled = true;
//////                    this.btnConfig.Enabled = true;
//////                    this.btnClose.Enabled = true;
//////                    this.btnChange.Enabled = true;
//////                    return;
//////                }
//////            }
//////            catch (SqlException)
//////            {
//////                MessageBox.Show("연결 상태를 확인하세요.");
//////                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
//////                this.Close();
//////                return;
//////            }
//////            catch (Exception ex)
//////            {
//////                MessageBox.Show(ex.Message);
//////                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
//////                this.Close();
//////                return;
//////            }
//////            // config.connect 변경
//////            if (LoginInfo.PlantAuth == "ALL" || LoginInfo.PlantAuth == "SK1")
//////            {
//////                cboSite.Value = "SK";
//////                LoginInfo.UserPlantCode = "SK1";
//////            }            
//////            else if (LoginInfo.PlantAuth == "SK2")
//////            {
//////                cboSite.Value = "EV";
//////                LoginInfo.UserPlantCode = "SK2";
//////            }

//////            this.DialogResult = System.Windows.Forms.DialogResult.OK;

//////            using (DbConnection dbconn = this.db.CreateConnection())
//////            {
//////                dbconn.Open();
//////                DbTransaction dbtrans = dbconn.BeginTransaction();
//////                try
//////                {
//////                    this.InsertCmd = this.db.GetStoredProcCommand("USP_SY0300_I1");
//////                    this.db.AddInParameter(InsertCmd, "@MakeDate", DbType.Date);
//////                    this.db.AddInParameter(InsertCmd, "@Maker", DbType.String);
//////                    this.db.AddInParameter(InsertCmd, "@IPAddress", DbType.String);

//////                    this.db.SetParameterValue(InsertCmd, "@MakeDate", System.DateTime.Now);
//////                    this.db.SetParameterValue(InsertCmd, "@Maker", this.txtWorkerID.Text);
//////                    this.db.SetParameterValue(InsertCmd, "@IPAddress", myip);
//////                    this.db.ExecuteNonQuery(InsertCmd, dbtrans);

//////                    dbtrans.Commit();
//////                }
//////                catch (Exception)
//////                {
//////                    dbtrans.Rollback();
//////                }
//////                dbconn.Close();
//////            }

//////            this.UserID = this.txtWorkerID.Text.ToUpper();

//////            SaveDoWorkType(this.UserID, myip, "LOGIN");

//////            appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
//////            appConfig.AppSettings.Settings["LOGINID"].Value = this.txtWorkerID.Text;
//////            appConfig.Save();
//////        }

//////        private void SaveDoWorkType(string userid, string IP, string sType)
//////        {
//////            try
//////            {
//////                SAMMI.Common.SqlDBHelper helper = new SAMMI.Common.SqlDBHelper(false);
//////                SqlParameter[] param = null;
//////                try
//////                {
//////                    param = new SqlParameter[6];

//////                    param[0] = helper.CreateParameter("@pProgramID", "MAIN", SqlDbType.VarChar, ParameterDirection.Input);   // 공장코드
//////                    param[1] = helper.CreateParameter("@pWorkerID", userid, SqlDbType.VarChar, ParameterDirection.Input);    // 품목코드
//////                    param[2] = helper.CreateParameter("@pWorkType", sType, SqlDbType.VarChar, ParameterDirection.Input);     // 관리항목
//////                    param[3] = helper.CreateParameter("@pIP", IP,   SqlDbType.VarChar, ParameterDirection.Input);              // 관리항목
//////                    param[4] = helper.CreateParameter("@RS_CODE",   SqlDbType.VarChar, ParameterDirection.Output, null, 1);
//////                    param[5] = helper.CreateParameter("@RS_MSG",    SqlDbType.VarChar, ParameterDirection.Output, null, 200);

//////                    helper.ExecuteNoneQuery("USP_SY0700_I1", CommandType.StoredProcedure, param);

//////                    if (param[4].Value.ToString() != "")
//////                    {
//////                        throw new Exception(param[5].Value.ToString());
//////                    }

//////                    helper.Transaction.Commit();
//////                }
//////                catch (Exception ex)
//////                {
//////                    helper.Transaction.Rollback();
//////                    MessageBox.Show(ex.ToString());
//////                }
//////                finally
//////                {
//////                    if (helper._sConn != null) { helper._sConn.Close(); }
//////                    if (param != null) { param = null; }
//////                }
//////            }
//////            catch (Exception)
//////            {
//////                MessageBox.Show("인터넷 연결을 확인하세요.");
//////            }
//////        }

//////        private void btnConfig_Click(object sender, EventArgs e)
//////        {
//////            ZZ0200 zz0200 = new ZZ0200();
//////            zz0200.ShowDialog();
//////        }

//////        private void btnClose_Click(object sender, EventArgs e)
//////        {
//////            this.cboSite.Value = appConfig.AppSettings.Settings["SITE"].Value;
//////            this.Close();
//////        }

//////        private void gbxLogin_MouseUp(object sender, MouseEventArgs e)
//////        {
//////            this.ismove = false;
//////        }

//////        private void gbxLogin_MouseMove(object sender, MouseEventArgs e)
//////        {
//////            if (this.ismove)
//////            {
//////                System.Drawing.Point d = Control.MousePosition;
//////                d.X = d.X - temp.X;
//////                d.Y = d.Y - temp.Y;
//////                this.Location = d;
//////            }
//////        }

//////        private void gbxLogin_MouseDown(object sender, MouseEventArgs e)
//////        {
//////            this.ismove = true;

//////            temp.X = Control.MousePosition.X - this.Location.X;
//////            temp.Y = Control.MousePosition.Y - this.Location.Y;
//////        }         

//////        private void ZZ0000_Load(object sender, EventArgs e)
//////        {
//////            try
//////            {
//////                DbCommand controlNameCmd = this.db.GetStoredProcCommand("USP_ZZ0000_S1");
//////                this.db.AddInParameter(controlNameCmd, "@ProgramID", DbType.String, this.Name);
//////                this.db.AddInParameter(controlNameCmd, "@Lang", DbType.String, "KO");
//////                DataSet dsCaption = this.db.ExecuteDataSet(controlNameCmd);

//////                for (int i = 0; i < dsCaption.Tables[0].Rows.Count; i++)
//////                {
//////                    SAMMI.Common.Common.FindControl(this.Controls, dsCaption.Tables[0].Rows[i]["ControlID"].ToString()).Text = dsCaption.Tables[0].Rows[i]["Caption"].ToString();
//////                }

//////            }
//////            catch (Exception)
//////            {
//////            }
//////            #region <SLO 로그인(주석처리)>
//////            //if (_session_key != "") // SLO 
//////            //{
//////            //    /*그룹웨어 로그인*/
//////            //    try
//////            //    {
                   
//////            //        SLOAGENTLib.JHWebCtl slo = new SLOAGENTLib.JHWebCtl();
//////            //        slo.login("gw.samkee.com", "80", _session_key);
//////            //        this.txtWorkerID.Text = slo.getvalue("username");
//////            //        this.txtPassword.Text = slo.getvalue("sessionpassword");

//////            //        if (this.txtWorkerID.Text != "")
//////            //        {
//////            //            bSloLogin = true;
//////            //            this.Invoke(new EventHandler(btnConfirm_Click));
//////            //        }
//////            //    }
//////            //    catch { }
//////            //}
//////            #endregion
//////        } 

//////        private void cboSite_ValueChanged(object sender, EventArgs e)
//////        {
//////            if (!this.btnConfirm.Enabled) { return; }

//////            Configuration appConfig1 = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
//////            System.Collections.Specialized.NameValueCollection sitecollection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("site");

//////            for (int i = 0; i < appConfig1.ConnectionStrings.ConnectionStrings.Count; i++)
//////            {
//////                appConfig1.ConnectionStrings.ConnectionStrings[i].ConnectionString = sitecollection[cboSite.Value.ToString()].ToString();
//////            }

//////            appConfig1.ConnectionStrings.ConnectionStrings["ConnectionString"].ConnectionString = sitecollection[cboSite.Value.ToString()].ToString();
//////            string[] serverip = sitecollection[cboSite.Value.ToString()].Split(new char[] { '=', ';' });

//////            appConfig1.AppSettings.Settings["SITE"].Value = this.cboSite.Value.ToString();
//////            appConfig1.AppSettings.Settings["DBSERVER1"].Value = serverip[1];
//////            appConfig1.AppSettings.Settings["DBUSER1"].Value = serverip[5];
//////            appConfig1.AppSettings.Settings["DBPASSWORD1"].Value = serverip[7];

//////            appConfig1.Save();
//////            ConfigurationManager.RefreshSection("configuration");
            
//////            //ConfigurationManager.RefreshSection("connectionStrings");
            
//////            //// this.db = DatabaseFactory.CreateDatabase();
//////            if (this.cboSite.Value.ToString() == "SK") 
//////            {                
//////                gbxLogin.Appearance.ImageBackground = global::SmartMES.Properties.Resources.MES_Logo;
//////            }
//////            else
//////            {
//////                gbxLogin.Appearance.ImageBackground = global::SmartMES.Properties.Resources.EV_Logo;
//////            }
//////        }

//////        private void cboSite_TextChanged(object sender, EventArgs e)
//////        {

//////        }

//////        private void btnChange_Click(object sender, EventArgs e)
//////        {
//////            ZZ0300 zz0300 = new ZZ0300(txtWorkerID.Text.Trim());
//////            zz0300.ShowDialog();
//////        }
//////        #region <SSO Function>
//////        /// <summary>
//////        /// SSO서버 Alive 체크
//////        /// </summary>
//////        /// <returns></returns>
//////        private Boolean WrapperCheckServer()
//////        {
//////            Boolean ret = true;

//////            int result = SA_CSI_CheckServer(strCheckServerURL.ToString(), Int32.Parse(strServerPort.ToString()));

//////            if (result != 0) { ret = false; }
//////            return ret;
//////        }
//////        /// <summary>
//////        /// 로그인
//////        /// </summary>
//////        /// <param name="sId"></param>
//////        /// <param name="sPw"></param>
//////        /// <returns></returns>
//////        private Boolean WrapperIDPWLogin(string sId, string sPw)
//////        {
//////            Boolean ret = true;
//////            int apiResult = 0;

//////            //WrapperSetEnableEncToken(TokenEncMode);
//////            apiResult = SA_CSI_IDPW_Authentication(strLogInURL,
//////                                                     Int32.Parse(strServerPort.ToString()),
//////                                                     strServiceID,
//////                                                     sId,
//////                                                     sPw);
//////            if (0 == apiResult) { } 
//////            else { ret = false; }   

//////            return ret;
//////        }
//////        /// <summary>
//////        /// 토큰 로그인
//////        /// </summary>
//////        /// <returns></returns>
//////        private Boolean WrapperTokenLogin()
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
//////        /// <summary>
//////        /// 로그아웃
//////        /// </summary>
//////        /// <returns></returns>
//////        private Boolean WrapperLogOut()
//////        {
//////            Boolean ret = true;
//////            int result = SA_CSI_SSO_Logout(strLogOutURL, Int32.Parse(strServerPort.ToString()));
//////            if (0 == result) { }  
//////            else { ret = false; } 

//////            return ret;
//////        }
//////        /// <summary>
//////        /// 토큰 Verify
//////        /// </summary>
//////        /// <returns></returns>
//////        private Boolean WrapperVerifyToken()
//////        {
//////            Boolean ret = true;                        

//////            WrapperSetEnableEncToken(TokenEncMode);

//////            int result = SA_CSI_SSO_TokenLogin(strAuthURL.ToString(),
//////                                                Int32.Parse(strServerPort.ToString()),
//////                                                strServiceID.ToString());
//////            if (0 == result)
//////            {                              
//////            }
//////            else
//////            {                
//////                ret = false;
//////            }
//////            return ret;
//////        }
//////        //////////////////////////////////////////
//////        // Set Token Encrypt Mode
//////        // param - Encrypt mode = true 
//////        //////////////////////////////////////////
//////        private Boolean WrapperSetEnableEncToken(bool mode)
//////        {
//////            int result = SA_CSI_SetEnableEncToken(mode);

//////            if (result == 0) { return true; } 
//////            return false;
//////        }
//////        /// <summary>
//////        /// 사용자 정보
//////        /// </summary>
//////        /// <returns></returns>
//////        private string WrapperGetLoginUserInfo()
//////        {
//////            StringBuilder strKey = new StringBuilder();
//////            strKey.Append("id");

//////            StringBuilder strUserID = new StringBuilder(128, 128);

//////            int result = SA_CSI_GetLoginUserInfo(strKey.ToString(), strUserID, 128);

//////            string resValue = string.Empty;

//////            if (0 == result)
//////            {
//////                resValue = strUserID.ToString();
//////            }
//////            return resValue;
//////        }
//////        /// <summary>
//////        /// 에러정보 확인
//////        /// </summary>
//////        private static void WrapperGetLastErrorInfo()
//////        {
//////            StringBuilder strErrorCode = new StringBuilder(128, 128);
//////            StringBuilder strErrorMsg = new StringBuilder(1024, 1024);
//////            int result = SA_CSI_GetLastErrorInfo(strErrorCode, 128, strErrorMsg, 1024);

//////            if (0 == result)
//////            {                
//////            }
//////            else
//////            {                
//////            }            
//////        }
//////        #endregion       
//////    }    
//////}