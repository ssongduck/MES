#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM0600
//   Form Name    : 작업자 마스터
//   Name Space   : SAMMI.BM
//   Created Date : 2012-03-19
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 작업라인(Workcenter) 관리 화면
// *---------------------------------------------------------------------------------------------*
#endregion

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
using SAMMI.PopUp;
using SAMMI.Common;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{
    public partial class BM0600 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        //비지니스 로직 객체 생성
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        DataTable DtChange = new DataTable();
        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >
        public BM0600()
        {
            InitializeComponent();
            //this.txtPlantCode.Text = "[" + LoginInfo.UserPlantCode + "] " + LoginInfo.UserPlantName;

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
            gridManager = new BizGridManagerEX(grid1);

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOpCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOpCode, txtOpName, txtLineCode, txtLineName });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { this.cboPlantCode_H, txtOpCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
            }
            else
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOpCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOpCode, txtOpName, txtLineCode, txtLineName });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { LoginInfo.PlantAuth, txtOpCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
            }
            gridManager.PopUpAdd("OPCode", "OPName", "TBM0400", new string[] { "PlantCode", "" });
            gridManager.PopUpAdd("LineCode", "LineName", "TBM0500", new string[] { "PlantCode", "OPCode" });

        }

        private void BM0600_Load(object sender, EventArgs e)
        {
            /************************************************************************/
            /* 2013.01.29 차승영 컬럼위치 수정 & 라인명 추가 (Grid)                   */
            /************************************************************************/

            #region 그리드
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "라인코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ShortName", "약어", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "OutProcFlag", "외주구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "CostCode", "원가코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "StartDate", "적용시작일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "EndDate", "적용완료일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Manager", "책임자", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "PostWorkCenterCode", "연결작업라인", false, GridColDataType_emu.VarChar, 105, 100, Infragistics.Win.HAlign.Default, false, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "PostWorkCenterName", "연결작업라인", false, GridColDataType_emu.VarChar, 188, 100, Infragistics.Win.HAlign.Default, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MultiOrderNo", "다중지시선택", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MultiFlag", "전체실적", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "TotalFlag", "집계여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LotPrnCnt", "페이지당 Lot수", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["OPCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["OPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["LineCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["LineName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;
            #endregion

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "TotalFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Viewflag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("MULTIORDERNO");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MultiOrderNo", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("OUTPROCFLAG");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "OutProcFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            //rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");  //사업장
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("MULTIFLAG");  // 전체랑전체실적
           // SAMMI.Common.Common.FillComboboxMaster(this.cboMultiFlag, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MultiFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("MULTIORDERNO");  // 전체랑전체실적
            //SAMMI.Common.Common.FillComboboxMaster(this.cboMultiOrderNo, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MultiOrderNo", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[11];

            try
            {
                DtChange.Clear();
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sUseFlag = SqlDBHelper.gGetCode(this.cboUseFlag_H.Value);
                string sTotalFlag = SqlDBHelper.gGetCode(this.cboTotalFlag_H.Value);

                string sMultiFlag = SqlDBHelper.gGetCode(this.cboMultiFlag_H.Value);
                string sMultiOrderNo = SqlDBHelper.gGetCode(this.cboMultiOrderNo_H.Value);

                string sOpCode = txtOpCode.Text.Trim();
                string sOpName = txtOpName.Text.Trim();

                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();
                string sWorkCenterName = txtWorkCenterName.Text.Trim();

                base.DoInquire();

                param[0] = helper.CreateParameter("@pWorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@pWorkCenterName", sWorkCenterName, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@pPlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@pOpCode", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@pOpName", sOpName, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@pLineCode", txtLineCode.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@pLineName", txtLineName.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input);
                param[7] = helper.CreateParameter("@pTotalFlag", sTotalFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[8] = helper.CreateParameter("@pUseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[9] = helper.CreateParameter("@pMultiOrderNo", sMultiOrderNo, SqlDbType.VarChar, ParameterDirection.Input);
                param[10] = helper.CreateParameter("@pMultiFlag", sMultiFlag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM0600_S2_UNION", CommandType.StoredProcedure, param);
                rtnDtTemp.AcceptChanges();
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
        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            try
            {
                base.DoNew();

                int iRow = _GridUtil.AddRow(this.grid1, DtChange);

                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterName", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "VIEWNAME", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LineCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LineName", iRow);
               // UltraGridUtil.ActivationAllowEdit(this.grid1, "OutProcFlag", iRow);
               // UltraGridUtil.ActivationAllowEdit(this.grid1, "PostWorkCenterCode", iRow);
               // UltraGridUtil.ActivationAllowEdit(this.grid1, "PostWorkCenterName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MultiOrderNo", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MultiFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "TotalFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LotPrnCnt", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //base.DoNew();
            //this.grid1.InsertRow();
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();

            this.grid1.DeleteRow();
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false, false);
            SqlParameter[] param = null;

            try
            {
                string sPlantCode = "";
                this.Focus();

                #region [Validate 체크]
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" 
                                || SqlDBHelper.nvlString(dr["OPCode"]) == "" 
                                || SqlDBHelper.nvlString(dr["OPCode"]) == ""
                                || SqlDBHelper.nvlString(dr["LineCode"]) == ""
                                || SqlDBHelper.nvlString(dr["WorkCenterCode"]) == "")
                            {
                                ShowDialog("공정코드, 라인코드, 작업장코드는 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }

                            if (!LoginInfo.PlantAuth.Equals("") &&
                                !LoginInfo.PlantAuth.Equals(SqlDBHelper.nvlString(dr["PlantCode"])))
                            {
                                ShowDialog("[" + SqlDBHelper.nvlString(dr["PlantCode"]) + "] 등록권한이 없습니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            break;
                    }
                }
                #endregion

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
                            #region 삭제 - 사용안함
                            drRow.RejectChanges();

                            param = new SqlParameter[4];

                            sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                            param[0] = helper.CreateParameter("@pPlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@pWorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드

                            param[2] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[3] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM0600_D1", CommandType.StoredProcedure, param);

                            if (param[2].Value.ToString() == "E") throw new Exception(param[3].Value.ToString());
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가 - 사용안함
                            param = new SqlParameter[14];

                            param[0] = helper.CreateParameter("@pPlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@pWorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@pWorkCenterName", SqlDBHelper.nvlString(drRow["WorkCenterName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@pOPCode", SqlDBHelper.nvlString(drRow["OPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@pLineCode", SqlDBHelper.nvlString(drRow["LineCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            // param[5] = helper.CreateParameter("@pOutProcFlag", SqlDBHelper.nvlString(drRow["OutProcFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            // param[6] = helper.CreateParameter("@pCostCode", SqlDBHelper.nvlString(drRow["CostCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@pMultiFlag", SqlDBHelper.nvlString(drRow["MultiFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@pMultiOrderNo", SqlDBHelper.nvlString(drRow["MultiOrderNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[9] = helper.CreateParameter("@pPostWorkCenter", SqlDBHelper.nvlString(drRow["PostWorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[10] = helper.CreateParameter("@pStartDate", SqlDBHelper.nvlString(drRow["StartDate"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[11] = helper.CreateParameter("@pEndDate", SqlDBHelper.nvlString(drRow["EndDate"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[12] = helper.CreateParameter("@pManager", SqlDBHelper.nvlString(drRow["Manager"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@pTotalFlag", SqlDBHelper.nvlString(drRow["TotalFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@pUseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[16] = helper.CreateParameter("@pViewName", SqlDBHelper.nvlString(drRow["ViewName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@pMaker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@pLotPrnCnt", SqlDBHelper.nvlString(drRow["LotPrnCnt"]), SqlDbType.Int, ParameterDirection.Input);

                            param[11] = helper.CreateParameter("@ShortName", SqlDBHelper.nvlString(drRow["ShortName"]), SqlDbType.VarChar, ParameterDirection.Input);


                            param[12] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[13] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);
                            helper.ExecuteNoneQuery("USP_BM0600_I2", CommandType.StoredProcedure, param);

                            if (param[11].Value.ToString() == "E") throw new Exception(param[12].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정 - 통합완료
                            param = new SqlParameter[14];

                            param[0] = helper.CreateParameter("@pPlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@pWorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@pWorkCenterName", SqlDBHelper.nvlString(drRow["WorkCenterName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@pOPCode", SqlDBHelper.nvlString(drRow["OPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@pLineCode", SqlDBHelper.nvlString(drRow["LineCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@pMultiFlag", SqlDBHelper.nvlString(drRow["MultiFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@pMultiOrderNo", SqlDBHelper.nvlString(drRow["MultiOrderNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@pTotalFlag", SqlDBHelper.nvlString(drRow["TotalFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@pUseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@pEditor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@pLotPrnCnt", SqlDBHelper.nvlString(drRow["LotPrnCnt"]), SqlDbType.Int, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@ShortName", SqlDBHelper.nvlString(drRow["ShortName"]), SqlDbType.VarChar, ParameterDirection.Input);


                            param[12] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[13] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);
                            helper.ExecuteNoneQuery("USP_BM0600_U2", CommandType.StoredProcedure, param);

                            if (param[11].Value.ToString() == "E") throw new Exception(param[12].Value.ToString());

                            #endregion
                            break;
                    }
                }

                //helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                CancelProcess = true;
                //helper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
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

        #endregion

        private void lblUseFlag_Click(object sender, EventArgs e)
        {

        }


    }
}
