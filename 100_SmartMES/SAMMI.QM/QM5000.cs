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
    public partial class QM5000 : SAMMI.Windows.Forms.BaseMDIChildForm
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


        #endregion

        public QM5000()
        {
            InitializeComponent();

            GridInit();

            btbManager = new BizTextBoxManagerEX();
            gridManager = new BizGridManagerEX(grid1);
            
            btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.UserPlantCode, "", "", "" }
                     , new string[] { }, new object[] { });
            gridManager.PopUpAdd("WorkCenterCode", "WorkCenterName", "TBM0600", new string[] { LoginInfo.UserPlantCode, "", "", "" });

            btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.UserPlantCode, txtWorkCenterCode, txtWorkCenterName, }
                   , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });

            //gridManager.PopUpAdd("LineCode", "LineName", "TBM0500", new string[] { LoginInfo.UserPlantCode, "OPCode" });
            gridManager.PopUpAdd("ItemCode", "ItemName", "TBM0101", new string[] { LoginInfo.UserPlantCode, "WorkCenterCode", "" });

            // ErrorType, ErrorClass, ErrorCode, ErrorDesc, UseFlag, TextBox1, TextBox2
           //  sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag
            //_biz.TBM1000_POP(arr[0], arr[1], sValueCode, sValueName, arr[2], tCodeBox, tNameBox);
            btbManager.PopUpAdd(txtErrorCode, txtErrorName, "TBM1000", new object[] { "", "", "Y" });
            gridManager.PopUpAdd("ErrorCode", "ErrorDesc", "TBM1000", new string[] { "", "", "", "", "", "Y" });

            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);
        }

        #region [그리드 셋팅]
        private void GridInit()

        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1);

            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "일자", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderNo", "지시번호", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorClass", "불량유형", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorCode", "불량코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorDesc", "불량명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorQty", "불량수량", false, GridColDataType_emu.Double, 80, 100, Infragistics.Win.HAlign.Right, true,true, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DayNight", "주야", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Seq", "Seq", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Impute", "귀책처", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Insist", "부적합처리", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ERP_IF", "불량 확정", false, GridColDataType_emu.CheckBox, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);

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

            //grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["ItemName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemName"].MergedCellStyle = MergedCellStyle.Always;
            grid1.Columns["ErrorDesc"].Header.Appearance.ForeColor = Color.Yellow;
            grid1.Columns["ErrorDesc"].Header.Appearance.BackColor = Color.DeepSkyBlue;
            grid1.Columns["ErrorQty"].Header.Appearance.ForeColor = Color.Yellow;
            grid1.Columns["ErrorQty"].Header.Appearance.BackColor = Color.DeepSkyBlue;
            grid1.Columns["Impute"].Header.Appearance.ForeColor = Color.Yellow;
            grid1.Columns["Impute"].Header.Appearance.BackColor = Color.DeepSkyBlue;
            grid1.Columns["Insist"].Header.Appearance.ForeColor = Color.Yellow;
            grid1.Columns["Insist"].Header.Appearance.BackColor = Color.DeepSkyBlue;
            grid1.Columns["ERP_IF"].Header.Appearance.ForeColor = Color.Yellow;
            grid1.Columns["ERP_IF"].Header.Appearance.BackColor = Color.DeepSkyBlue;
            #endregion Grid MERGE

            #region 콤보박스

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("ERRORCLASS");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ErrorClass", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("ERRORTYPE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ErrorType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("DAYNIGHT");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DayNight", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Insist", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("IMPUTE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Impute", rtnDtTemp, "CODE_ID", "CODE_NAME");

            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "RecDate", null, null, null);

            grid1.Columns["RecDate"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownCalendar;

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion

        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[7];

            try
            {
                base.DoInquire();

                string sPlantCode = LoginInfo.UserPlantCode;                                                                        // 사업장(공장)
                string sStartDate = SqlDBHelper.nvlDateTime(cboStartDate_H.Value).ToString("yyyy-MM-dd");                           // 생산시작일자
                string sEndDate = SqlDBHelper.nvlDateTime(cboEndDate_H.Value).ToString("yyyy-MM-dd");                               // 생산  끝일자
                string sWorkCenterCode = SqlDBHelper.nvlString(this.txtWorkCenterCode.Text.Trim());                                 // 작업장 코드
                string sErrorCode = SqlDBHelper.nvlString(this.txtErrorCode.Text.Trim());                                           // 에러
                string sDayNight = SqlDBHelper.nvlString(this.cboDayNight_H.Value);
                string sOPCode = SqlDBHelper.nvlString(this.cboOpCode_H.Value); 


                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[1] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param[2] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                param[3] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param[4] = helper.CreateParameter("ErrorCode", sErrorCode, SqlDbType.VarChar, ParameterDirection.Input);             // 공정 코드       
                param[5] = helper.CreateParameter("DayNight", sDayNight, SqlDbType.VarChar, ParameterDirection.Input);           // 공정 코드       
                param[6] = helper.CreateParameter("OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);  
                rtnDtTemp = helper.FillTable("USP_QM5000_S1N", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();


                DtChange = rtnDtTemp;

                //_Common.Grid_Column_Width(this.grid1); //grid 정리용   
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

        public override void DoNew()
        {
            try
            {

                base.DoNew();
                int iRow = _GridUtil.AddRow(this.grid1, DtChange);

                UltraGridUtil.ActivationAllowEdit(this.grid1, "RecDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OrderNo", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ErrorClass", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ErrorCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ErrorDesc", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ErrorQty", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "DayNight", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Seq", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Impute", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Insist", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ERP_IF", iRow);
                grid1.Rows[iRow].Cells["Insist"].Value = "N";
                grid1.Rows[iRow].Cells["ERP_IF"].Value = "False";
                //if (this.grid1.Rows.Count > 1)
                //{
                //    if (grid1.ActiveRow == null)
                //        return;
                //    GirdRowAdd();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public override void DoDelete()
        {
            base.DoDelete();

            this.grid1.DeleteRow();
        }

        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;
            string sPlantCode = string.Empty;
            sPlantCode = LoginInfo.UserPlantCode;
            try
            {
                this.Focus();

                #region [Validate 체크]
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            if (LoginInfo.UserPlantCode == ""
                                || SqlDBHelper.nvlString(dr["ErrorCode"]) == ""
                                || SqlDBHelper.nvlString(dr["ItemCode"]) == "")
                            {
                                ShowDialog("품목코드, 불량코드는 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }

                            //if (!grid1.ActiveRow.Cells["ERP_IF"].Text.Equals("True"))
                            //{
                            //    ShowDialog("불량확정을 체크해주세요.", Windows.Forms.DialogForm.DialogType.OK);

                            //    CancelProcess = true;
                            //    return;
                            //}

                            break;
                    }
                }
                #endregion

                //string i = grid1.ActiveRow.Cells["ERP_IF"].Text;
                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                {
                    CancelProcess = true;
                    return;
                }

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow drRow in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[4];

                            sPlantCode = LoginInfo.UserPlantCode;

                            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[2] = helper.CreateParameter("@OrderNo", SqlDBHelper.nvlString(drRow["OrderNo"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[3] = helper.CreateParameter("@Seq", SqlDBHelper.nvlString(drRow["Seq"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            
                            helper.ExecuteNoneQuery("USP_QM5000_D1", CommandType.StoredProcedure, param);

                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가

                            if (SqlDBHelper.nvlString(drRow["ERP_IF"]).Equals("True"))
                            {
                                param = new SqlParameter[13];

                                sPlantCode = LoginInfo.UserPlantCode;
                                //InspVal = String.Format("{0:F3}", Convert.ToDouble(SqlDBHelper.nvlString(drRow["USL"])));
                                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                                param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[2] = helper.CreateParameter("@OrderNo", SqlDBHelper.nvlString(drRow["OrderNo"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                //param[3] = helper.CreateParameter("@Seq", SqlDBHelper.nvlString(drRow["Seq"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[3] = helper.CreateParameter("@ItemCode", SqlDBHelper.nvlString(drRow["ItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                                param[4] = helper.CreateParameter("@ErrorCode", SqlDBHelper.nvlString(drRow["ErrorCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                                param[5] = helper.CreateParameter("@ErrorQty", SqlDBHelper.nvlString(drRow["ErrorQty"]), SqlDbType.VarChar, ParameterDirection.Input);
                                param[6] = helper.CreateParameter("@DayNight", SqlDBHelper.nvlString(drRow["DayNight"]), SqlDbType.VarChar, ParameterDirection.Input);
                                param[7] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                                param[8] = helper.CreateParameter("@Impute", SqlDBHelper.nvlString(drRow["Impute"]), SqlDbType.VarChar, ParameterDirection.Input);
                                param[9] = helper.CreateParameter("@Insist", SqlDBHelper.nvlString(drRow["Insist"]), SqlDbType.VarChar, ParameterDirection.Input);
                                param[12] = helper.CreateParameter("@RecDate", SqlDBHelper.nvlString(drRow["RecDate"]), SqlDbType.VarChar, ParameterDirection.Input);

                                param[10] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                                param[11] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                                helper.ExecuteNoneQuery("USP_QM5000_I2N", CommandType.StoredProcedure, param);

                                if (param[10].Value.ToString() == "E") throw new Exception(param[11].Value.ToString());
                            }
                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정

                            if (SqlDBHelper.nvlString(drRow["ERP_IF"]).Equals("True"))
                            {

                                param = new SqlParameter[11];

                                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                                param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[2] = helper.CreateParameter("@OrderNo", SqlDBHelper.nvlString(drRow["OrderNo"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[3] = helper.CreateParameter("@Seq", SqlDBHelper.nvlString(drRow["Seq"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[4] = helper.CreateParameter("@ErrorQty", SqlDBHelper.nvlString(drRow["ErrorQty"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[5] = helper.CreateParameter("@Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[6] = helper.CreateParameter("@ErrorCode", SqlDBHelper.nvlString(drRow["ErrorCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[7] = helper.CreateParameter("@DayNight", SqlDBHelper.nvlString(drRow["DayNight"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[8] = helper.CreateParameter("@Impute", SqlDBHelper.nvlString(drRow["Impute"]), SqlDbType.VarChar, ParameterDirection.Input);
                                param[9] = helper.CreateParameter("@Insist", SqlDBHelper.nvlString(drRow["Insist"]), SqlDbType.VarChar, ParameterDirection.Input);
                                param[10] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                                helper.ExecuteNoneQuery("USP_QM5000_U2", CommandType.StoredProcedure, param);
                            }


                            #endregion
                            break;

                        case DataRowState.Unchanged :

                            if (SqlDBHelper.nvlString(drRow["ERP_IF"]).Equals("True"))
                            {
                                param = new SqlParameter[11];

                                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                                param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[2] = helper.CreateParameter("@OrderNo", SqlDBHelper.nvlString(drRow["OrderNo"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[3] = helper.CreateParameter("@Seq", SqlDBHelper.nvlString(drRow["Seq"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[4] = helper.CreateParameter("@ErrorQty", SqlDBHelper.nvlString(drRow["ErrorQty"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[5] = helper.CreateParameter("@Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[6] = helper.CreateParameter("@ErrorCode", SqlDBHelper.nvlString(drRow["ErrorCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[7] = helper.CreateParameter("@DayNight", SqlDBHelper.nvlString(drRow["DayNight"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[8] = helper.CreateParameter("@Impute", SqlDBHelper.nvlString(drRow["Impute"]), SqlDbType.VarChar, ParameterDirection.Input);
                                param[9] = helper.CreateParameter("@Insist", SqlDBHelper.nvlString(drRow["Insist"]), SqlDbType.VarChar, ParameterDirection.Input);
                                param[10] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                                helper.ExecuteNoneQuery("USP_QM5000_U2", CommandType.StoredProcedure, param);
                            }

                            break;
                    }
                }
 
                helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                CancelProcess = true;
                helper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion

        private void GirdRowAdd()
        {
            try
            {
                

                int i = grid1.ActiveRow.Index;
                int j = grid1.Rows.Count - 1;
                grid1.Rows[i + 1].Cells["RecDate"].Value = grid1.Rows[i].Cells["RecDate"].Value;
                grid1.Rows[i + 1].Cells["PlantCode"].Value = grid1.Rows[i].Cells["PlantCode"].Value;
                grid1.Rows[i + 1].Cells["WorkCenterCode"].Value = grid1.Rows[i].Cells["WorkCenterCode"].Value;
                grid1.Rows[i + 1].Cells["WorkCenterName"].Value = grid1.Rows[i].Cells["WorkCenterName"].Value;
                grid1.Rows[i + 1].Cells["OrderNo"].Value = grid1.Rows[i].Cells["OrderNo"].Value;
                grid1.Rows[i + 1].Cells["ItemCode"].Value = grid1.Rows[i].Cells["ItemCode"].Value;
                grid1.Rows[i + 1].Cells["ItemName"].Value = grid1.Rows[i].Cells["ItemName"].Value;
                grid1.Rows[i + 1].Cells["ErrorClass"].Value = grid1.Rows[i].Cells["ErrorClass"].Value;
                grid1.Rows[i + 1].Cells["ErrorCode"].Value = grid1.Rows[i].Cells["ErrorCode"].Value;
                grid1.Rows[i + 1].Cells["ErrorDesc"].Value = grid1.Rows[i].Cells["ErrorDesc"].Value;
                grid1.Rows[i + 1].Cells["ErrorQty"].Value = grid1.Rows[i].Cells["ErrorQty"].Value;
                grid1.Rows[i + 1].Cells["DayNight"].Value = grid1.Rows[i].Cells["DayNight"].Value;
                grid1.Rows[i + 1].Cells["Impute"].Value = grid1.Rows[i].Cells["Impute"].Value;
                grid1.Rows[i + 1].Cells["Insist"].Value = grid1.Rows[i].Cells["Insist"].Value;
                grid1.Rows[i + 1].Cells["ERP_IF"].Value = "False";
                //grid1.Rows[i + 1].Cells["Seq"].Value = (Convert.ToInt32(grid1.Rows[i].Cells["Seq"].Value) + 1).ToString().Trim();
             }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void QM5000_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //GirdRowAdd();
        }
    }
}
