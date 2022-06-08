#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : PP0430
//   Form Name      : (서산)주조작업일보
//   Name Space     : SAMMI.PP
//   Created Date   : 2022.01.10
//   Made By        : 정용석
//   Description    : 서산 주조 작업일보 전산화(현장단말입력)에 따른 MES조회 및 출력 화면
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using System.Configuration;
using Infragistics.Win.UltraWinGrid;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;                         

#endregion

namespace SAMMI.PP
{
    public partial class PP0430 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        DataSet   rtnDsTemp = new DataSet();   // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        BizTextBoxManagerEX btbManager = new BizTextBoxManagerEX();


        Configuration appConfig;


        private string PlantCode = string.Empty;
        #endregion
        
        public PP0430()
        {
            InitializeComponent();
            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);
            this.appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (this.PlantCode.Equals("SK"))
                this.PlantCode = "SK1";
            else if (this.PlantCode.Equals("EC"))
                this.PlantCode = "SK2";

            if (!(this.PlantCode.Equals("SK1") || this.PlantCode.Equals("SK2")))
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;

            CboStartdate_H.Value = System.DateTime.Now.ToString("yyyy-MM-dd");

            btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600",  new object[] { this.cboPlantCode_H, "2000", "", "" },  new string[] { }, new object[] { });
                                //new string[] { "OPCode", "OPName" }, new object[] {null,  null});
        }
        private void GridInit()
        {

            gridBand1.Caption  = "용탕" + Environment.NewLine + "온도";
            gridBand2.Caption  = "저속" + Environment.NewLine + "위치";
            gridBand3.Caption  = "저속" + Environment.NewLine + "속도";

            gridBand6.Caption  = "고속" + Environment.NewLine + "위치";            
            gridBand7.Caption  = "고속" + Environment.NewLine + "속도";
            gridBand8.Caption  = "고속" + Environment.NewLine + "구간";
            gridBand9.Caption  = "응고" + Environment.NewLine + "시간";
            gridBand10.Caption = "주조" + Environment.NewLine + "압력";
            gridBand11.Caption = "승압" + Environment.NewLine + "시간";
            gridBand12.Caption = "ACC"  + Environment.NewLine + "압력";
            gridBand13.Caption = "VP"   + Environment.NewLine + "절환";
            gridBand14.Caption = "진공" + Environment.NewLine + "개시";
            gridBand15.Caption = "진공" + Environment.NewLine + "도";
            gridBand16.Caption = "Bis"  + Environment.NewLine + "두께";


            gridBand134.Caption = "미" + Environment.NewLine + "성형";
            gridBand135.Caption = "크랙";
            gridBand136.Caption = "소착" + Environment.NewLine + "(뜯김)";
            gridBand137.Caption = "살떨" + Environment.NewLine + "어짐";
            gridBand138.Caption = "박리";
            gridBand139.Caption = "형상" + Environment.NewLine + "(치수)";
            gridBand140.Caption = "PIN" + Environment.NewLine + "파손/휨";
            gridBand141.Caption = "변색"  + Environment.NewLine + "(물기)";
            gridBand142.Caption = "공타" + Environment.NewLine + "Trial";
            gridBand143.Caption = "소재" + Environment.NewLine + "절단" + Environment.NewLine + "검사";
            gridBand144.Caption = "금형" + Environment.NewLine + "깨짐";
            gridBand145.Caption = "부" + Environment.NewLine + "풀음";
            gridBand146.Caption = "탕" + Environment.NewLine + "겹침";
            gridBand147.Caption = "수축";
            gridBand148.Caption = "게이트" + Environment.NewLine + "탈락";
            gridBand149.Caption = "찍힘";
            gridBand150.Caption = "사출" + Environment.NewLine + "박힘";
            gridBand151.Caption = "기타";
            gridBand152.Caption = "샘플";
            gridBand153.Caption = "T/O";
            gridBand154.Caption = "변형" + Environment.NewLine + "(주조)";
            gridBand155.Caption = "생산" + Environment.NewLine + "수량"; 
        }


        private void DataClear()
        {
            lblDayWorker.Text = string.Empty;
            lblNightWorker.Text = string.Empty;
            lblItemCode.Text = string.Empty;
            lblItemName.Text = string.Empty;
            lblMoldName.Text = string.Empty;
            lblMoldCode.Text = string.Empty;
            lbDayNorun.Items.Clear();
            lbNightNorun.Items.Clear();
            gridControl1.DataSource = null;
            gridControl2.DataSource = null;
            gridControl3.DataSource = null;
        }

        private bool WriteExcelData()
        {            
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            DataTable dtProdInfo   = rtnDsTemp.Tables[0];
            DataTable dtCastInfo   = rtnDsTemp.Tables[1];
            DataTable dtSqTimeInfo = rtnDsTemp.Tables[2];
            DataTable dtProdQty    = rtnDsTemp.Tables[3];
            DataTable dtJitCooler  = rtnDsTemp.Tables[4];
            DataTable dtNorun      = rtnDsTemp.Tables[5];
            DataTable dtRemarks    = rtnDsTemp.Tables[6];                    
            
            try
            {
                excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                wb = excelApp.Workbooks.Open(Application.StartupPath + @"\CastReport.xlsx");
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;
                                
                
                /*생산정보*/
                Excel.Range d1 = ws.Cells[2, 4];
                d1.Value = dtProdInfo.Rows[0]["MachName"].ToString();

                Excel.Range d2 = ws.Cells[2, 11];
                d2.Value = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);

                Excel.Range d3  = ws.Cells[3, 14];  // 주간근무시간
                Excel.Range d4  = ws.Cells[4, 14];  // 야간근무시간
                Excel.Range d5  = ws.Cells[3, 5];   // 주간작업자
                Excel.Range d6  = ws.Cells[4, 5];   // 야간작업자
                Excel.Range d7  = ws.Cells[3, 22];  // 품명
                Excel.Range d8  = ws.Cells[4, 22];  // 품번
                Excel.Range d9  = ws.Cells[3, 34];  // 금형번호
                Excel.Range d10 = ws.Cells[4, 34];  // 자산번호

                d3.Value = "08:00 ~ 20:00";
                d4.Value = "20:00 ~ 08:00";

                string DayWorker = string.Empty;
                string NgtWorker = string.Empty;
                string MoldCode  = string.Empty;
                string MoldName  = string.Empty;

                foreach (DataRow dr in dtProdInfo.Rows)
                {
                    if (dr["DayNight"].ToString() == "D")
                    {
                        if (!DayWorker.Contains(dr["Worker"].ToString()))
                        {
                            if (DayWorker == string.Empty)
                            {
                                DayWorker = dr["Worker"].ToString();
                            }
                            else
                            {
                                DayWorker += ", " + dr["Worker"].ToString();
                            }
                        }
                    }
                    else if (dr["DayNight"].ToString() == "N")
                    {
                        if (!NgtWorker.Contains(dr["Worker"].ToString()))
                        {
                            if (NgtWorker == string.Empty)
                            {
                                NgtWorker = dr["Worker"].ToString();
                            }
                            else
                            {
                                NgtWorker += ", " + dr["Worker"].ToString();
                            }
                        }
                    }
                    d7.Value = dr["ItemName"].ToString();
                    d8.Value = dr["ItemCode"].ToString();


                    if (!MoldCode.Contains(dr["MoldCode"].ToString()))
                    {
                        if (MoldCode == string.Empty)
                        {
                            MoldCode = dr["MoldCode"].ToString();
                            MoldName = dr["MoldName"].ToString();
                        }
                        else
                        {
                            MoldCode += ", " + dr["MoldCode"].ToString();
                            MoldName += ", " + dr["MoldName"].ToString();
                        }
                    }                       
                }
                d5.Value  = DayWorker;
                d6.Value  = NgtWorker;
                d9.Value  = MoldName;
                d10.Value = MoldCode;

                /* 주조조건 */
                if (dtCastInfo != null || dtCastInfo.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCastInfo.Rows.Count; i++)
                    {
                        for (int j = 1; j < dtCastInfo.Columns.Count; j++)
                        {
                            Excel.Range d11 = ws.Cells[i + 8, j + 3];
                            d11.Value = dtCastInfo.Rows[i][j].ToString();
                        }
                    }
                }
                /* 스퀴즈타임 */
                if (dtSqTimeInfo != null || dtSqTimeInfo.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSqTimeInfo.Rows.Count; i++)
                    {
                        for (int j = 1; j < dtSqTimeInfo.Columns.Count; j++)
                        {
                            Excel.Range d12 = ws.Cells[i + 17, j + 3];
                            d12.Value = dtSqTimeInfo.Rows[i][j].ToString();
                        }
                    }
                }
                /* 작업현황 */
                int mergeRow = 0;
                if (dtProdQty != null || dtProdQty.Rows.Count > 0)
                {
                    for (int i = 0; i < dtProdQty.Rows.Count; i++)
                    {                        
                        for (int j = 1; j < dtProdQty.Columns.Count; j++)
                        {
                            Excel.Range d13 = ws.Cells[26 + mergeRow, j + 3];
                            d13.Value = dtProdQty.Rows[i][j].ToString();
                        }
                        mergeRow += 3;
                    }
                }
                /* JIT쿨러 */
                if (dtJitCooler != null || dtJitCooler.Rows.Count > 0)
                {
                    for (int i = 0; i < dtJitCooler.Rows.Count; i++)
                    {
                        for (int j = 2; j < dtJitCooler.Columns.Count; j++)
                        {
                            Excel.Range d14 = ws.Cells[i + 26, j + 24];
                            d14.Value = dtJitCooler.Rows[i][j].ToString();
                        }
                    }
                }
                /* 비가동 */
                int dRow = 0;
                int nRow = 0;
                if (dtNorun != null || dtNorun.Rows.Count > 0)
                {
                    for (int i = 0; i < dtNorun.Rows.Count; i++)
                    {
                        string noRun = string.Format("{0} ~ {1} : [{2}]{3} {4}", dtNorun.Rows[i]["sTime"].ToString(), 
                                                                                 dtNorun.Rows[i]["eTime"].ToString(), 
                                                                                 dtNorun.Rows[i]["StopCode"].ToString(), 
                                                                                 dtNorun.Rows[i]["CodeName"].ToString(),
                                                                                 dtNorun.Rows[i]["Remarks"].ToString());

                        if (dtNorun.Rows[i]["DayNight"].ToString() == "D")
                        {
                            Excel.Range d15 = ws.Cells[34 + dRow, 4];
                            d15.Value = noRun;
                            dRow++;
                        }
                        if (dtNorun.Rows[i]["DayNight"].ToString() == "N")
                        {
                            Excel.Range d16 = ws.Cells[34 + nRow, 26];
                            d16.Value = noRun;
                            nRow++;
                        }
                    }
                }
                /* 특이사항 */
                if (dtRemarks != null || dtRemarks.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRemarks.Rows.Count; i++)
                    {
                        string remarks = string.Format("※ {0}:{1}", dtRemarks.Rows[i]["Seq"].ToString(), dtRemarks.Rows[i]["Remarks"].ToString());

                        if (dtRemarks.Rows[i]["DayNight"].ToString() == "D")
                        {
                            Excel.Range d17 = ws.Cells[34 + dRow + i, 4];
                            d17.Value = remarks;
                        }
                        if (dtRemarks.Rows[i]["DayNight"].ToString() == "N")
                        {
                            Excel.Range d18 = ws.Cells[34 + nRow + i, 26];
                            d18.Value = remarks;
                        }
                    }
                }
                using (var foldDirDlg = new FolderBrowserDialog())
                {
                    if (foldDirDlg.ShowDialog() == DialogResult.OK)
                    {
                        string path = foldDirDlg.SelectedPath + @"\" + string.Format("주조작업일보({0})_{1}.xlsx", dtProdInfo.Rows[0]["MachName"].ToString(), Convert.ToDateTime(CboStartdate_H.Value).ToString("yyMMdd"));
                        wb.SaveCopyAs(path);                        
                        //wb.SaveCopyAs(foldDirDlg.SelectedPath + @"\" + string.Format("주조작업일보({0})_{1}.xlsx", dtProdInfo.Rows[0]["MachName"].ToString(), CboStartdate_H.Value));
                    }
                }
                //엑셀시트 이미지 추가 
                //ws.Shapes.AddPicture(Application.StartupPath + @"\image.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 25, 90, 530, 350);
                //wb.SaveCopyAs(Application.StartupPath + @"\" + string.Format("{0}.xlsx", "DailyWorkReport#1"));
                wb.Close();
                excelApp.Quit();
                return true;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }


        private void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        public override void ExportExcel()
        {
            base.ExportExcel();            
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                if (WriteExcelData() == true)
                {
                    SAMMI.Windows.Forms.MessageForm message = new SAMMI.Windows.Forms.MessageForm("엑셀 파일로 다운로드가 완료되었습니다.");
                    message.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            DataClear();            

            SqlDBHelper helper   = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();
                string sDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);

                if (sPlantCode == "SK1") 
                {
                    SAMMI.Windows.Forms.MessageForm message = new SAMMI.Windows.Forms.MessageForm("조회된 데이터가 없습니다.");
                    message.ShowDialog();
                    return; 
                }
                if (sWorkCenterCode == "")
                {
                    SAMMI.Windows.Forms.MessageForm message = new SAMMI.Windows.Forms.MessageForm("작업장을 선택하세요.");
                    message.ShowDialog();
                    return;
                }
                base.DoInquire();                

                param[0] = helper.CreateParameter("@AS_PLANTCODE",      sPlantCode,      SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@AS_WORKCENTERCODE", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@AS_DATE",           sDate,           SqlDbType.VarChar, ParameterDirection.Input);


                if (LoginInfo.UserID == "SKEV01")
                {
                    rtnDsTemp = helper.FillDataSet("USP_PP0430_S2", CommandType.StoredProcedure, param);
                }
                else
                {
                    rtnDsTemp = helper.FillDataSet("USP_PP0430_S1", CommandType.StoredProcedure, param);
                }

                if (rtnDsTemp.Tables[0].Rows.Count == 0)  { return;  }
                
                //lblMachName.Text = string.Format(" 설비명 : {0}", rtnDsTemp.Tables[0].Rows[0]["MachName"].ToString());
                //lblWorkDate.Text = string.Format(" / 조회일 : {0}", sDate);
                
                foreach (DataRow dr in rtnDsTemp.Tables[0].Rows)
                {                    
                    if (dr["DayNight"].ToString() == "D")
                    {                        
                        if (!lblDayWorker.Text.Contains(dr["Worker"].ToString()))
                        {
                            if (lblDayWorker.Text == "")
                            {
                                lblDayWorker.Text += dr["Worker"].ToString();
                            }
                            else
                            {
                                lblDayWorker.Text += ", " + dr["Worker"].ToString();
                            }
                        }
                    }
                    else if (dr["DayNight"].ToString() == "N")
                    {
                        if (!lblNightWorker.Text.Contains(dr["Worker"].ToString()))
                        {
                            if (lblNightWorker.Text == "")
                            {
                                lblNightWorker.Text += dr["Worker"].ToString();
                            }
                            else
                            {
                                lblNightWorker.Text += ", " + dr["Worker"].ToString();
                            }
                        }
                    }                                        
                    lblItemName.Text = dr["ItemName"].ToString();
                    lblItemCode.Text = dr["ItemCode"].ToString();

                    if (!lblMoldCode.Text.Contains(dr["MoldCode"].ToString()))
                    {
                        if (lblMoldCode.Text == "")
                        {
                            lblMoldCode.Text += dr["MoldCode"].ToString();
                            lblMoldName.Text += dr["MoldName"].ToString();
                        }
                        else
                        {
                            lblMoldCode.Text += ", " + dr["MoldCode"].ToString();
                            lblMoldName.Text += ", " + dr["MoldName"].ToString();
                        }

                    }
                }                                                
                                
                gridControl1.DataSource = rtnDsTemp.Tables[1];
                gridControl2.DataSource = rtnDsTemp.Tables[2];
                gridControl3.DataSource = rtnDsTemp.Tables[3];
                gridControl4.DataSource = rtnDsTemp.Tables[4];                    

                //lbDayNorun.Items.Clear();
                //lbNightNorun.Items.Clear();

                foreach (DataRow dr in rtnDsTemp.Tables[5].Rows)
                {
                    if (dr["DayNight"].ToString() == "D")
                    {
                        lbDayNorun.Items.Add(string.Format("{0} ~ {1} : [{2}]{3} {4}",  dr["sTime"].ToString(), 
                                                                                        dr["eTime"].ToString(), 
                                                                                        dr["StopCode"].ToString(), 
                                                                                        dr["CodeName"].ToString(),
                                                                                        dr["Remarks"].ToString()));
                    }
                    else if (dr["DayNight"].ToString() == "N")
                    {
                        lbNightNorun.Items.Add(string.Format("{0} ~ {1} : [{2}]{3} {4}", dr["sTime"].ToString(), 
                                                                                         dr["eTime"].ToString(), 
                                                                                         dr["StopCode"].ToString(), 
                                                                                         dr["CodeName"].ToString(),
                                                                                         dr["Remarks"].ToString()));
                    }
                }
                lbDayNorun.Items.Add("");                                               
                foreach (DataRow dr in rtnDsTemp.Tables[6].Rows)
                {
                    if (dr["DayNight"].ToString() == "D" && dr["Remarks"].ToString() != "")
                    {
                        lbDayNorun.Items.Add(string.Format("※ {0}:{1}", dr["Seq"].ToString(), dr["Remarks"].ToString()));
                    }
                    else if (dr["DayNight"].ToString() == "N" && dr["Remarks"].ToString() != "")
                    {
                        lbNightNorun.Items.Add(string.Format("※ {0}:{1}",  dr["Seq"].ToString(), dr["Remarks"].ToString() ));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }

                base.ClosePrgForm();
            }
        }
        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
                     
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {  
        }
        #endregion

        #region < EVENT AREA >
        /// <summary>
        /// Form이 Close 되기전에 발생
        /// e.Cancel을 true로 설정 하면, Form이 close되지 않음
        /// 수정 내역이 있는지를 확인 후 저장여부를 물어보고 저장, 저장하지 않기, 또는 화면 닫기를 Cancel 함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {

        }

        /// <summary>
        /// DATABASE UPDATE전 VALIDATEION CHECK 및 값을 수정한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            //if (e.Row.RowState == DataRowState.Modified)
            //{
            //    e.Command.Parameters["@Editor"].Value = this.WorkerID;
            //    return;
            //}

            //if (e.Row.RowState == DataRowState.Added)
            //{
            //    e.Command.Parameters["@Maker"].Value = this.WorkerID;
            //    return;
            //}
        }

        /// <summary>
        /// 저장처리시 오류가 발생한 경우 오류 메세지에 대한 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            //if (e.Errors == null) return;

            //switch (((SqlException)e.Errors).Number)
            //{
            //    // 중복
            //    case 2627:
            //        e.Row.RowError = "공정코드가 있습니다.";
            //        throw (new SException("S00099", e.Errors));
            //    default:
            //        break;
            //}
        }

      

        #endregion

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의


        private void PP9400_Load(object sender, EventArgs e)
        {
            GridInit();
            GetWorkCenterList();

            //DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            //rtnDtTemp = _Common.GET_TBM0000_CODE("REQUESTFLAG");     //사용여부
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "REQUESTFLAG", rtnDtTemp, "CODE_ID", "CODE_NAME");

            //SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
        }


        int maxNum = 0;
        private void GetWorkCenterList()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[1];
            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

            string query = string.Empty;


            switch (SqlDBHelper.nvlString(appConfig.AppSettings.Settings["SITE"].Value))
            {
                case "EV":
                case "SKS":
                    query = @"select row_number() over(order by cast(replace(workcentername, '호기','') as int) ) as rowNum, 
                                 WorkCenterCode, 
                                 WorkCenterName 
                            from tbm0600  
                           where PlantCode = @AS_PLANTCODE 
                             and useflag = 'Y' 
                             and opcode = 2000 
                             and substring(WorkCenterCode, 2, 1) <> 'Z' 
                            order by cast(replace(workcentername, '호기','') as int)";
                    break;
                case "SK":
                    query = @"select row_number() over(order by cast(replace(workcentername, '호기','') as int) ) as rowNum, 
                                 WorkCenterCode, 
                                 WorkCenterName 
                            from ecomes.sk_mesdb_v1.dbo.tbm0600  
                           where PlantCode = @AS_PLANTCODE 
                             and useflag = 'Y' 
                             and opcode = 2000 
                             and substring(WorkCenterCode, 2, 1) <> 'Z'   
                           order by cast(replace(workcentername, '호기','') as int)";
                    break;
                case "EVP":
                    query = @"select row_number() over(order by cast(replace(workcentername, '호기','') as int) ) as rowNum, 
                                 WorkCenterCode, 
                                 WorkCenterName 
                            from evmes.sk_mesdb_v1.dbo.tbm0600  
                           where PlantCode = @AS_PLANTCODE 
                             and useflag = 'Y' 
                             and opcode = 2000 
                             and substring(WorkCenterCode, 2, 1) <> 'Z'   
                           order by cast(replace(workcentername, '호기','') as int)";
                    break;                    
            }

            try
            {
                param[0] = helper.CreateParameter("@AS_PLANTCODE", "SK2", SqlDbType.VarChar, ParameterDirection.Input);

                _DtTemp = helper.FillTable(query, CommandType.Text, param);

                maxNum = _DtTemp.Rows.Count == 0 ? 1 : Convert.ToInt32(_DtTemp.Compute("MAX(rowNum)", null));
                //maxNum = Convert.ToInt32(_DtTemp.Compute("MAX(rowNum)", null));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }


        #endregion

        private void gridView1_MouseWheel(object sender, MouseEventArgs e)
        {
            (e as DevExpress.Utils.DXMouseEventArgs).Handled = true;
        }

        private void gridView3_MouseWheel(object sender, MouseEventArgs e)
        {
            (e as DevExpress.Utils.DXMouseEventArgs).Handled = true;
        }


        int selNum = 0;
        private void btnNext_Click(object sender, EventArgs e)
        {            
            if (selNum == maxNum) { selNum = 1; }
            else { selNum += 1; }

            foreach (DataRow dr in _DtTemp.AsEnumerable().Where(t => Convert.ToInt16(t["rowNum"]) == selNum))
            {
                txtWorkCenterCode.Text = dr["WorkCenterCode"].ToString();
                txtWorkCenterName.Text = dr["WorkCenterName"].ToString();
            }            
            DoInquire();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {            
            if (selNum == 0 || selNum == 1) { selNum = maxNum; }
            else { selNum -= 1; }

            //var selWorkCenter = _DtTemp.AsEnumerable().Where(t => Convert.ToInt32(t["rowNum"]) == selNum);
            foreach (DataRow dr in _DtTemp.AsEnumerable().Where(t => Convert.ToInt16(t["rowNum"]) == selNum))
            {
                txtWorkCenterCode.Text = dr["WorkCenterCode"].ToString();
                txtWorkCenterName.Text = dr["WorkCenterName"].ToString();
            }                        
            DoInquire();
        }
        
    }
}
