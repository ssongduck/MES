#region <Using Area>
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
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using SAMMI.Common;
#endregion

namespace SAMMI.BM
{
    public partial class BM8200 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        //비지니스 로직 객체 생성
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp1 = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp2 = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp3 = new DataTable(); // return DataTable 공통

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;


        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        DataTable DtChange = new DataTable();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        private string PlantCode = string.Empty;
        #endregion

        public BM8200()
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

            btbManager = new BizTextBoxManagerEX();
            
            GridInit1();
            GridInit2();
            GridInit3();

            btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H.Value, "", "", "" }
                    , new string[] { "", "" }, new object[] { });
            
        }


        #region <Gird1 Init>
        private void GridInit1()
        {
            _GridUtil.InitializeGrid(this.grid1, false, false, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, (LoginInfo.UserPlantCode == "ALL") ? true : false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", true, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UserCNT", "담당자수", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
           
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            rtnDtTemp1 = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp1, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
        }
        #endregion

        #region <Gird2 Init>
        private void GridInit2()
        {
            _GridUtil.InitializeGrid(this.grid2, false, false, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid2, "PlantCode", "사업장", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, (LoginInfo.UserPlantCode == "ALL") ? true : false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "WorkerID", "작업자ID", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "WorkerName", "작업자명", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "WorkCenterCode", "작업장코드", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "WorkCenterName", "작업장명", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "UseFlag", "사용여부", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "Remarks", "비고", true, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            
            _GridUtil.SetInitUltraGridBind(grid2);
            DtChange = (DataTable)grid2.DataSource;
            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            rtnDtTemp2 = _Common.GET_TBM0000_CODE("USEFLAG");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid2, "UseFlag", rtnDtTemp2, "CODE_ID", "CODE_NAME");

            rtnDtTemp2 = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid2, "PLANTCODE", rtnDtTemp2, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid2);
        }
        #endregion

        #region <Gird3 Init>
        private void GridInit3()
        {
            _GridUtil.InitializeGrid(this.grid3, false, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern



            _GridUtil.InitColumnUltraGrid(grid3, "PlantCode", "사업장", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, (LoginInfo.UserPlantCode == "ALL") ? true : false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "WorkCenterCode", "작업장", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "WorkCenterName", "작업장명", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "MACHID", "설비", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "UseFlag", "사용여부", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical01", "관리이탈", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical02", "런(RUN)", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical03", "경향", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical04", "진동", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical05", "영역경고(3SIGMA)", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical06", "영역경고(2SIGMA)", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical07", "영역경고(1SIGMA01)", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical08", "영역경고(1SIGMA02)", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical09", "Critical09", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical10", "Critical10", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical11", "Critical11", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical12", "Critical12", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical13", "Critical13", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical14", "Critical14", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Critical15", "Critical15", true, GridColDataType_emu.CheckBox, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid3, "Remarks", "비고", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);


            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid3, "UseFlag", _Common.GET_TBM0000_CODE("USEFLAG"), "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", _Common.GET_TBM0000_CODE("PLANTCODE"), "CODE_ID", "CODE_NAME");

            _GridUtil.SetInitUltraGridBind(grid3);
            DtChange = (DataTable)grid3.DataSource;
            //     ///row number
            grid3.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid3.DisplayLayout.Override.RowSelectorWidth = 40;
            grid3.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid3.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            //rtnDtTemp3 = _Common.GET_TBM0000_CODE("USEFLAG");  //사업장
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid3, "UseFlag", rtnDtTemp3, "CODE_ID", "CODE_NAME");

            //SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid3);
        }
        #endregion

        #region <TOOL BAR AREA >
        public override void DoInquire()
        {
            string rbtn = "";
            if (rbtnALL.Checked == true)
            {
                rbtn = "1";
            }
            else if (rbtnOver.Checked == true)
            {
                rbtn = "2";
            }
            else if (rbtnZero.Checked == true)
            {
                rbtn = "3";
            }

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param1 = new SqlParameter[3];

            try
            {
                base.DoInquire();


                this.rtnDtTemp1.Clear();
                this.rtnDtTemp2.Clear();
                this.rtnDtTemp3.Clear();

                string PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string WorkCenterCode = SqlDBHelper.nvlString(txtWorkCenterCode.Text);

                param1[0] = helper.CreateParameter("PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param1[1] = helper.CreateParameter("WorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[2] = helper.CreateParameter("btnCheck", rbtn, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     

                //this.rtnDtTemp1 = helper.FillTable("USP_BM8200_S1", CommandType.StoredProcedure, param1);
                this.rtnDtTemp1 = helper.FillTable("USP_BM8200_S1_UNION", CommandType.StoredProcedure, param1);
                grid1.DataSource = this.rtnDtTemp1;
                grid1.DataBind();

                //if (rtnDtTemp1.Rows.Count == 0)
                //{
                //    rtnDtTemp2.Clear();
                //    rtnDtTemp3.Clear();
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param1 != null) { param1 = null; }
                SAMMI.Common.Common common = new Common.Common();
                common.CleanProcess("SK_MESDB_V1");
            }
        }

        public override void DoNew()
        {
            if (this.rtnDtTemp1.Rows.Count > 0)
            {
                base.DoNew();
                string PlantCode = LoginInfo.UserPlantCode;
                string WorkCenterCode = SqlDBHelper.nvlString(grid1.ActiveRow.Cells["WorkCenterCode"].Value.ToString());
                if (WorkCenterCode.Equals(string.Empty))
                {
                    MessageBox.Show("데이터를 등록할 작업장을 선택하세요.");
                    //throw (new SException("E00001", null));
                }
                //if (this.grid2.IsActivate)
                //{
                    this.grid2.InsertRow();
                    int iRow = _GridUtil.AddRow(this.grid2, DtChange);

                    this.grid2.ActiveRow.Cells["PlantCode"].Value = PlantCode;
                    this.grid2.ActiveRow.Cells["UseFlag"].Value = "Y";
                    this.grid2.ActiveRow.Cells["WorkCenterCode"].Value = this.grid1.ActiveRow.Cells["WorkCenterCode"].Value.ToString();
                    this.grid2.ActiveRow.Cells["WorkCenterName"].Value = this.grid1.ActiveRow.Cells["WorkCenterName"].Value.ToString();
                    //this.grid2.ActiveRow.Cells["Maker"].Value = this.WorkerID;
                    //this.grid2.ActiveRow.Cells["Editor"].Value = this.WorkerID;
                    //this.grid2.ActiveRow.Cells["MakerDate"].Value = System.DateTime.Now;
                    //this.grid2.ActiveRow.Cells["EditDate"].Value = System.DateTime.Now;
                    return;
                //}
                //if (this.grid3.IsActivate)
                //{
                //    if (this.grid3.Rows.Count == 1)
                //        return;
                //    this.grid3.InsertRow();

                //    this.grid3.ActiveRow.Cells["PlantCode"].Value = PlantCode;
                //    this.grid3.ActiveRow.Cells["WorCenterCode"].Value = this.grid1.ActiveRow.Cells["WorkCenterCode"].Value.ToString();
                //    this.grid3.ActiveRow.Cells["MACHID"].Value = "*";
                //    this.grid3.ActiveRow.Cells["Remarks"].Value = "";
                //    this.grid3.ActiveRow.Cells["UseFlag"].Value = "N";
                //    this.grid3.ActiveRow.Cells["Maker"].Value = this.WorkerID;
                //    this.grid3.ActiveRow.Cells["Editor"].Value = this.WorkerID;

                //    string criticalno = string.Empty;
                //    for (int i = 0; i < 15; i++)
                //    {
                //        criticalno = "Critical" + (101 + i).ToString().Substring(1, 2);
                //        this.grid3.ActiveRow.Cells[criticalno].Value = 0;
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("데이터를 등록할 그리드를 클릭하세요.");
                //}

            }
            else return;
        }

        public override void DoDelete()
        {
            base.DoDelete();

            if (this.grid3.IsActivate)
            {
                this.grid3.DeleteRow();
                return;
            }
            if (this.grid2.IsActivate)
            {
                this.grid2.DeleteRow();
            }
        }


        //2013.08.16 수정_육근영
        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false, false);
            SqlParameter[] param = null;
            SqlDBHelper helper1 = new SqlDBHelper(false, false);
            SqlParameter[] param1 = null;
            try
            {
                

                this.Focus();

                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                base.DoSave();                    

                UltraGridUtil.DataRowDelete(this.grid3);
                this.grid3.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                //foreach (DataRow drRow in DtChange.Rows)
                foreach(DataRow drRow in rtnDtTemp3.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Modified:
                            #region 수정

                            param = new SqlParameter[14];
                            param[0] = helper.CreateParameter("PLANTCODE", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);      // 사업장                                                                      
                            param[1] = helper.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 일자                                        
                            param[2] = helper.CreateParameter("MACHID", drRow["MACHID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 주야                                          
                            param[4] = helper.CreateParameter("Critical01 ", drRow["Critical01"], SqlDbType.Bit, ParameterDirection.Input);       // 작업자ID                                    
                            param[5] = helper.CreateParameter("Critical02 ", drRow["Critical02"], SqlDbType.Bit, ParameterDirection.Input);    // 생산담당자                                    
                            param[6] = helper.CreateParameter("Critical03", drRow["Critical03"], SqlDbType.Bit, ParameterDirection.Input);    // 보전수신자                                    
                            param[7] = helper.CreateParameter("Critical04", drRow["Critical04"], SqlDbType.Bit, ParameterDirection.Input);       // 연락처
                            param[8] = helper.CreateParameter("Critical05 ", drRow["Critical05"], SqlDbType.Bit, ParameterDirection.Input);       // 작업자ID                                    
                            param[9] = helper.CreateParameter("Critical06 ", drRow["Critical06"], SqlDbType.Bit, ParameterDirection.Input);    // 생산담당자                                    
                            param[10] = helper.CreateParameter("Critical07", drRow["Critical07"], SqlDbType.Bit, ParameterDirection.Input);    // 보전수신자                                    
                            param[11] = helper.CreateParameter("Critical08", drRow["Critical08"], SqlDbType.Bit, ParameterDirection.Input);       // 연락처

                            param[12] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[13] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM8200_U1", CommandType.StoredProcedure, param);

                            if (param[12].Value.ToString() == "E") throw new Exception(param[13].Value.ToString());

                            #endregion
                            break;

                        case DataRowState.Added:
                            #region 수정

                            param = new SqlParameter[14];
                            param[0] = helper.CreateParameter("PLANTCODE", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);      // 사업장                                                                      
                            param[1] = helper.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 일자                                        
                            param[2] = helper.CreateParameter("MACHID", drRow["MACHID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 주야                                          
                            param[4] = helper.CreateParameter("Critical01 ", drRow["Critical01"], SqlDbType.Bit, ParameterDirection.Input);       // 작업자ID                                    
                            param[5] = helper.CreateParameter("Critical02 ", drRow["Critical02"], SqlDbType.Bit, ParameterDirection.Input);    // 생산담당자                                    
                            param[6] = helper.CreateParameter("Critical03", drRow["Critical03"], SqlDbType.Bit, ParameterDirection.Input);    // 보전수신자                                    
                            param[7] = helper.CreateParameter("Critical04", drRow["Critical04"], SqlDbType.Bit, ParameterDirection.Input);       // 연락처
                            param[8] = helper.CreateParameter("Critical05 ", drRow["Critical05"], SqlDbType.Bit, ParameterDirection.Input);       // 작업자ID                                    
                            param[9] = helper.CreateParameter("Critical06 ", drRow["Critical06"], SqlDbType.Bit, ParameterDirection.Input);    // 생산담당자                                    
                            param[10] = helper.CreateParameter("Critical07", drRow["Critical07"], SqlDbType.Bit, ParameterDirection.Input);    // 보전수신자                                    
                            param[11] = helper.CreateParameter("Critical08", drRow["Critical08"], SqlDbType.Bit, ParameterDirection.Input);       // 연락처

                            param[12] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[13] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM8200_I1", CommandType.StoredProcedure, param);

                            if (param[12].Value.ToString() == "E") throw new Exception(param[13].Value.ToString());

                            #endregion
                            break;
                    }

                }
                //helper.Transaction.Commit();

                UltraGridUtil.DataRowDelete(this.grid2);
                this.grid2.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);
                this.Focus();
                foreach (DataRow drRow in ((DataTable)grid2.DataSource).Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Modified:
                            #region 수정

                            param1 = new SqlParameter[8];
                            param1[0] = helper1.CreateParameter("PlantCode", "SK1", SqlDbType.VarChar, ParameterDirection.Input);      // 사업장                                                                      
                            param1[1] = helper1.CreateParameter("WorkerID", drRow["WorkerID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 일자                                        
                            param1[2] = helper1.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 주야                                          
                            param1[3] = helper1.CreateParameter("Remarks ", drRow["Remarks"], SqlDbType.VarChar, ParameterDirection.Input);       // 작업자ID                                    
                            param1[4] = helper1.CreateParameter("UseFlag ", drRow["UseFlag"], SqlDbType.VarChar, ParameterDirection.Input);    // 생산담당자                                    
                            param1[5] = helper1.CreateParameter("Editor ", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);    // 생산담당자                                    

                            param1[6] = helper1.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param1[7] = helper1.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM8100_U1", CommandType.StoredProcedure, param1);

                            if (param1[6].Value.ToString() == "E") throw new Exception(param1[7].Value.ToString());

                            #endregion
                            break;
                            
                        case DataRowState.Added:
                            #region 수정

                            param1 = new SqlParameter[8];
                            param1[0] = helper1.CreateParameter("PlantCode", "SK1", SqlDbType.VarChar, ParameterDirection.Input);      // 사업장                                                                      
                            param1[1] = helper1.CreateParameter("WorkerID", drRow["WorkerID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 일자                                        
                            param1[2] = helper1.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 주야                                          
                            param1[3] = helper1.CreateParameter("Remarks ", drRow["Remarks"], SqlDbType.VarChar, ParameterDirection.Input);       // 작업자ID                                    
                            param1[4] = helper1.CreateParameter("UseFlag ", drRow["UseFlag"], SqlDbType.VarChar, ParameterDirection.Input);    // 생산담당자                                    
                            param1[5] = helper1.CreateParameter("Maker ", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);    // 생산담당자                                    

                            param1[6] = helper1.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param1[7] = helper1.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM8100_I1", CommandType.StoredProcedure, param1);

                            if (param1[6].Value.ToString() == "E") throw new Exception(param1[7].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Deleted:
                            #region 삭제

                            param1 = new SqlParameter[5];
                            param1[0] = helper1.CreateParameter("PlantCode", "SK1", SqlDbType.VarChar, ParameterDirection.Input);      // 사업장                                                                      
                            param1[1] = helper1.CreateParameter("WorkerID", drRow["WorkerID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 일자                                        
                            param1[2] = helper1.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 주야                                          
                            
                            param1[3] = helper1.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param1[4] = helper1.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM8100_D1", CommandType.StoredProcedure, param1);

                            if (param1[3].Value.ToString() == "E") throw new Exception(param1[4].Value.ToString());

                            #endregion
                            break;
                        
                    }
                    
                }
                //helper1.Transaction.Commit();
                
            }
            catch (Exception ex)
            {
                //helper.Transaction.Rollback();
                //helper1.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
                if (param1 != null) { param1 = null; }

                DoInquire2();
            }

            
        }
        #endregion

        private void DoInquire2()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param2 = new SqlParameter[2];
            SqlParameter[] param3 = new SqlParameter[2];


            this.rtnDtTemp2.Clear();
            this.rtnDtTemp3.Clear();
            //F6WDG6N5RBZM
            try
            {
                base.DoInquire();

                string PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string WorkCenterCode = this.grid1.ActiveRow.Cells["WorkCenterCode"].Value.ToString();

                param2[0] = helper.CreateParameter("PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param2[1] = helper.CreateParameter("WorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     

                this.rtnDtTemp2 = helper.FillTable("USP_BM8100_S1_UNION", CommandType.StoredProcedure, param2);
                grid2.DataSource = this.rtnDtTemp2;
                grid2.DataBind();

                param3[0] = helper.CreateParameter("PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param3[1] = helper.CreateParameter("WorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     

                this.rtnDtTemp3 = helper.FillTable("USP_BM8200_S3_UNION", CommandType.StoredProcedure, param3);
                grid3.DataSource = this.rtnDtTemp3;
                grid3.DataBind();

                base.ClosePrgForm();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param2 != null) { param2 = null; }
                if (param3 != null) { param3 = null; }
                //SAMMI.Common common = new Common();
                //this.ClosePrgForm();
                //common.CleanProcess("SK_MESDB_V1");
            }
        }


        private void grid1_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            DoInquire2(); 
        }

        private void grid2_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            grid_POP_UP();
        }

        private void grid2_KeyPress(object sender, KeyPressEventArgs e)
        {
            grid_POP_UP();
        }

        #region grid POP UP 처리
        private void grid_POP_UP()
        {
            int iRow = this.grid2.ActiveRow.Index;

            string sUseFlag = "Y"; //사용여부 

            string sWorkerID = this.grid2.Rows[iRow].Cells["WorkerID"].Text.Trim();  // 
            string sWorkerName = this.grid2.Rows[iRow].Cells["WorkerName"].Text.Trim();  // 

            if (this.grid2.ActiveCell.Column.ToString() == "WorkerID" || this.grid2.ActiveCell.Column.ToString() == "WorkerName")
            {
                _biz.TSY0200_POP_Grid("", "", "", "", sWorkerID, sWorkerName, sUseFlag, grid2, "WorkerID", "WorkerName");
            }

        }
        #endregion
    }
}
