#region <USING AREA>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
#endregion

namespace SAMMI.QM
{
    public partial class QM9201 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        // private DataTable DtChange = null;

        DataTable DtChange = new DataTable();
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string PlantCode = string.Empty;
        #endregion

        public QM9201()
        {
            InitializeComponent();

            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this.PlantCode.Equals("SK"))
                this.PlantCode = "SK1";
            else if (this.PlantCode.Equals("EC"))
                this.PlantCode = "SK2";
            if (!(this.PlantCode.Equals("SK1") || this.PlantCode.Equals("SK2")))
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;

            GridInit();

            btbManager = new BizTextBoxManagerEX();
            gridManager = new BizGridManagerEX(grid1);;

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                         , new string[] { }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                         , new string[] { }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }


            //if (LoginInfo.PlantAuth.Equals(string.Empty))
            //{
            //    btbManager.PopUpAdd(txtWorkCenterCode_Reg, txtWorkCenterName_Reg, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
            //    , new string[] { }, new object[] { });

            //    btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { this.cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
            //           , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            //    btbManager.PopUpAdd(txtItemCode_Reg, txtItemName_Reg, "TBM0101", new object[] { this.cboPlantCode_H, txtWorkCenterCode_Reg, txtWorkCenterName_Reg, }
            //   , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            //}
            //else
            //{
            //    btbManager.PopUpAdd(txtWorkCenterCode_Reg, txtWorkCenterName_Reg, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
            //    , new string[] { }, new object[] { });

            //    btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
            //           , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            //    btbManager.PopUpAdd(txtItemCode_Reg, txtItemName_Reg, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode_Reg, txtWorkCenterName_Reg, }
            //   , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            //}
        }

        #region [그리드 셋팅]
        private void GridInit()
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Seq", "순번", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IspDate", "측정일", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspFlag", "측정구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterLine", "라인", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Judgment", "판정", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DayNight", "주야", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IspType", "검사유형", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FileName", "파일명", false, GridColDataType_emu.VarChar, 320, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FileDir", "파일경로", false, GridColDataType_emu.VarChar, 640, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "makeDate", "등록일자", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "maker", "측정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            #endregion

            #region Grid MERGE

            //grid1.Columns["RecDate"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["RecDate"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["RecDate"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["RecDate"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["RecDate"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["RecDate"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["DayNight"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["DayNight"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["DayNight"].MergedCellStyle = MergedCellStyle.Always;
            #endregion Grid MERGE

            #region 콤보박스


            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("DAYNIGHT");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DayNight", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("ISPTYPE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "IspType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("INSPTYPE2");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "InspFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("JUDGE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Judgment", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");


            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion

        }
        #endregion

        #region 조회
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[7];

            string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);
            string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);
            string sJudge = SqlDBHelper.nvlString(cboJudge_Search.Value);
            string sInspFlag = SqlDBHelper.nvlString(cboInspFlag_S.Value);
            try
            {
                base.DoInquire();

                param[0] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[1] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자      
                param[2] = helper.CreateParameter("@ItemCode", txtItemCode.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드            // 생산시작일자      
                param[3] = helper.CreateParameter("@WorkCenterCode", txtWorkCenterCode.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@Judge", sJudge, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@InspFlag", sInspFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);   
                
                //DataSet ds = helper.FillDataSet("USP_QM9201_S1", CommandType.StoredProcedure, param);
                DataTable rtnDtTemp = helper.FillTable("USP_QM9201_S1_UNION", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

            }
            catch (Exception ex)
            {
                this.ShowDialog(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
            this.ClosePrgForm();
        }
        #endregion 조회

        #region 등록
        private void btnSUM1_Click(object sender, EventArgs e)
        {
            if( SqlDBHelper.nvlString(txtItemCode_Reg.Text).Equals(string.Empty) 
                || SqlDBHelper.nvlString(txtItemName_Reg.Text).Equals(string.Empty))
            {
                MessageBox.Show("품번을 선택해주세요.");
                return;
            }
            if (SqlDBHelper.nvlString(cboJudge_H.Value).Equals(string.Empty)
                || SqlDBHelper.nvlString(cboIspType_H.Value).Equals(string.Empty)
                || SqlDBHelper.nvlString(cboDayNight_H.Value).Equals(string.Empty)
                || SqlDBHelper.nvlString(cboInspFlag_I.Value).Equals(string.Empty))
            {
                MessageBox.Show("검사 등록 항목을 선택해주세요.");
                return;
            }
            if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string FileExtension = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf('.'), openFileDialog1.FileName.Length - openFileDialog1.FileName.LastIndexOf('.'));

            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[12];

            try
            {
                base.DoSave();

                string sInspDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(CboIspDate_H.Value));

                param[0] = helper.CreateParameter("@IspDate", sInspDate, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)      
                param[1] = helper.CreateParameter("@ItemCode", txtItemCode_Reg.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드  
                param[2] = helper.CreateParameter("@ItemName", txtItemName_Reg.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param[3] = helper.CreateParameter("@WorkCenterCode", txtWorkCenterCode_Reg.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드  
                param[4] = helper.CreateParameter("@WorkCenterName", txtWorkCenterName_Reg.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param[5] = helper.CreateParameter("@WorkCenterLine", txtWorkCenterLine.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param[6] = helper.CreateParameter("@Judgment", cboJudge_H.Value, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param[7] = helper.CreateParameter("@IspType", cboIspType_H.Value, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param[8] = helper.CreateParameter("@DayNight", cboDayNight_H.Value, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param[9] = helper.CreateParameter("@InspFlag", cboInspFlag_I.Value, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param[10] = helper.CreateParameter("@FileExtension", FileExtension, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param[11] = helper.CreateParameter("@RT_FileDir", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                param[11].Direction = System.Data.ParameterDirection.Output;

                DataSet ds = helper.FillDataSet("USP_QM9201_I1", CommandType.StoredProcedure, param);

                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(param[11].Value.ToString().Substring(0, param[11].Value.ToString().LastIndexOf('\\')));

                //_DtTemp = _Common.GET_TBM0000_CODE("FILESVINFO");

                //_DtTemp.DefaultView.RowFilter = "CODE_ID='SHSV'";
                //string ip = _DtTemp.DefaultView[0]["RelCode1"].ToString();
                //string id = _DtTemp.DefaultView[0]["RelCode2"].ToString();
                //string pw = _DtTemp.DefaultView[0]["RelCode3"].ToString();
                //string folder = "";// _DtTemp.DefaultView[0]["RelCode4"].ToString();

                //if (System.IO.Directory.Exists("\\\\" + ip + "\\" + folder) == false) // 이미 열려있으면 넘어간다.
                //{
                //    System.Diagnostics.Process.Start("cmd.exe", "/C Net Use \\\\" + ip + "\\" + folder + " /user:" + id + " " + pw);
                //}
                //MessageBox.Show(di.ToString());
                if (System.IO.Directory.Exists(di.ToString()) == false) // 이미 열려있으면 넘어간다.
                {
                    System.Diagnostics.Process.Start("cmd.exe", "/C Net Use " + di.ToString() + " /user:" + "attach" + " " + "skatt@ssw5rd");
                }


                if (di.Exists == false)
                    di.Create();

                System.IO.File.Copy(openFileDialog1.FileName, param[11].Value.ToString(), true);
                try
                {
                    DataRow dr = ((DataTable)grid1.DataSource).NewRow();
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        dr[dc.ColumnName] = ds.Tables[0].Rows[0][dc.ColumnName];
                    }

                    ((DataTable)grid1.DataSource).Rows.Add(dr);
                }
                catch { }
               // grid1.DataSource = ds.Tables[0];
               // grid1.DataBind();


                helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                helper.Transaction.Rollback();
                this.ShowDialog(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }

            this.ClosePrgForm();
            
           
        }
        #endregion

        public override void DoDelete()
        {
            if (this.ShowDialog("선택한 데이터를 정말 삭제하시겠습니까?") != System.Windows.Forms.DialogResult.OK)
                return;            

            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                base.DoDelete();

                string IspDate = string.Format("{0:yyyy-MM-dd}", grid1.ActiveRow.Cells["IspDate"].Value.ToString().Substring(0, 10));

                param[0] = helper.CreateParameter("@pSeq", grid1.ActiveRow.Cells["Seq"].Value.ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@pItemCode", grid1.ActiveRow.Cells["ItemCode"].Value.ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@pIspDate", IspDate, SqlDbType.VarChar, ParameterDirection.Input);

                //MessageBox.Show(grid1.ActiveRow.Cells["Seq"].Value.ToString() + "//" + grid1.ActiveRow.Cells["ItemCode"].Value.ToString() + "//" + IspDate);
                DataSet ds = helper.FillDataSet("USP_QM9201_D1", CommandType.StoredProcedure, param);


                if (System.IO.Directory.Exists(grid1.ActiveRow.Cells["FileDir"].Value.ToString()) == false) // 이미 열려있으면 넘어간다.
                {
                    System.Diagnostics.Process.Start("cmd.exe", "/C Net Use " + grid1.ActiveRow.Cells["FileDir"].Value.ToString() + " /user:" + "attach" + " " + "skatt@ssw5rd");
                }

                System.IO.File.Delete(grid1.ActiveRow.Cells["FileDir"].Value.ToString());

                helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                helper.Transaction.Rollback();
                this.ShowDialog(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }

            this.ClosePrgForm();
            DoInquire();
        }
        private void grid1_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                _DtTemp = _Common.GET_TBM0000_CODE("FILESVINFO");

                _DtTemp.DefaultView.RowFilter = "CODE_ID='SHSV'";
                string ip = _DtTemp.DefaultView[0]["RelCode1"].ToString();
                string id = _DtTemp.DefaultView[0]["RelCode2"].ToString();
                string pw = _DtTemp.DefaultView[0]["RelCode3"].ToString();
                string folder = "mes";// _DtTemp.DefaultView[0]["RelCode4"].ToString();

                if (System.IO.Directory.Exists("\\\\" + ip + "\\" + folder) == false) // 이미 열려있으면 넘어간다.
                {
                    System.Diagnostics.Process.Start("cmd.exe", "/C Net Use \\\\" + ip + "\\" + folder + " /user:" + id + " " + pw);
                }

                process.StartInfo.FileName = grid1.ActiveRow.Cells["FileDir"].Value.ToString();

                process.Start();
            }
            catch { }
        }




    }
}
