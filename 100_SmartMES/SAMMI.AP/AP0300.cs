#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : AP0300
//   Form Name    : 긴급지시편성
//   Name Space   : SAMMI.AP
//   Created Date : 2012-06-25
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 긴급지시편성
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
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.AP
{
    public partial class AP0300 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;
        //BizGridManagerEX gridManager;

        private DataTable DtChange = null;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private int _Fix_Col = 0;
        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >

        public AP0300()
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
            gridManager = new BizGridManagerEX(grid1);
            string sUseFlag = string.Empty;
            string sLineCode = string.Empty;
            string sOPCode = string.Empty;

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { this.cboPlantCode_H, txtOPCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOPCode, txtOPName, txtLineCode, txtLineName });
            }
            else
            {
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { LoginInfo.PlantAuth, txtOPCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOPCode, txtOPName, txtLineCode, txtLineName });
            }
        }
        #endregion

        #region AP0300_Load
        private void AP0300_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);


            // InitColumnUltraGrid  (91 111 110 114 185 95 100 100 100 156 209 100 100 100 100 90 90 169 90 90 90 90 90 90 90 90 90 90 )
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Linecode", "라인코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품목코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목 명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderNo", "지시번호", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderDate", "지시일자", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FastOrderType", "유형", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderQty", "계획량", false, GridColDataType_emu.Integer, 70, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", "nnn,nnn,nnn", null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UnitCode", "단위", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldCode", "투입금형", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldName", "금형형번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkInFlag", "생산투입여부", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FinishFlag", "지시완료여부", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FastOrderFlag", "긴급지시여부", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고(특기사항)", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderPriority", "지시순위", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            

            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            DtChange = (DataTable)grid1.DataSource;

            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["OPCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["OPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["ItemName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemName"].MergedCellStyle = MergedCellStyle.Always;

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            //gridManager.PopUpAdd("OPCode", "OPName", "TBM0400", new string[] { "PlantCode", "" });
            //gridManager.PopUpAdd("LineCode", "LineName", "TBM0500", new string[] { "PlantCode", "OPCode" });
            gridManager.PopUpAdd("WorkCenterCode", "WorkCenterName", "TBM0600", new string[] { this.PlantCode, "", "", "" });
            gridManager.PopUpAdd("ItemCode", "ItemName", "TBM0100", new string[] { this.PlantCode, "", "" });
            gridManager.PopUpAdd("MoldCode", "MoldName", "TBM1600", new string[] { this.PlantCode, "", "" });
            #endregion

        }
        #endregion AP0300_Load

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            //SqlParameter[] param = new SqlParameter[7];
            SqlParameter[] param = new SqlParameter[7];
            SqlParameter[] param2 = new SqlParameter[2];
            SqlParameter[] param3 = new SqlParameter[4];

            try
            {
                DtChange.Clear();

                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sStartDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(calRegDT_FRH.Value));
                string sEndDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(calRegDT_TOH.Value));
                string sOPCode = txtOPCode.Text.Trim(); ;
                string sLineCode = txtLineCode.Text.Trim();
                string sItemCode = txtItemCode.Text.Trim();
                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();


                //param[0] = helper.CreateParameter("@PlantCode", this.PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                //param[1] = helper.CreateParameter("@OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                //param[2] = helper.CreateParameter("@LineCode ", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                //param[4] = helper.CreateParameter("@ItemCode ", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                //param[3] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                //param[5] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                //param[6] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                
//                rtnDtTemp = helper.FillTable("USP_AP0301_S1", CommandType.StoredProcedure, param);

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@ItemCode ", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                
                //rtnDtTemp = helper.FillTable("USP_AP0301_S2", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_AP0301_S2_UNION", CommandType.StoredProcedure, param);

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
            base.DoNew();
            int iRow = _GridUtil.AddRow(this.grid1, DtChange);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "OrderNo", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "OrderDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "OrderQty", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UnitCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "OPCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "OPName", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "LineCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "LineName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MoldCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MoldName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkInFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "FinishFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "FastOrderFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "FastOrderType", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Remark", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "OrderPriority", iRow);
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
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;

            try
            {
                this.Focus();

                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow drRow in DtChange.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[1];

                            param[0] = helper.CreateParameter("OrderNo", drRow["OrderNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         
                            helper.ExecuteNoneQuery("USP_AP0300_D1N", CommandType.StoredProcedure, param);

                            if (param[1].Value.ToString() == "E") throw new Exception(param[2].Value.ToString());

                            #endregion
                            break;

                        case DataRowState.Added:
                            #region 추가
                            param = new SqlParameter[9];
                            param[0] = helper.CreateParameter("@PlantCode", this.PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            //param[1] = helper.CreateParameter("@OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[2] = helper.CreateParameter("@LineCode", drRow["LineCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@OrderQty", drRow["OrderQty"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@MoldCode", drRow["MoldCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[8] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);
                            
                            helper.ExecuteNoneQuery("USP_AP0301_I2", CommandType.StoredProcedure, param);

                            if (param[7].Value.ToString() == "E") throw new Exception(param[8].Value.ToString());
                            #endregion
                            break;
                        case DataRowState.Modified:

                            #region 수정

                            param = new SqlParameter[26];
                            param[0] = helper.CreateParameter("PlantCode", this.PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("OrderNo", drRow["OrderNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("OrderDate", drRow["OrderDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("OrderQty", drRow["OrderQty"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("UnitCode", drRow["UnitCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("LineCode", drRow["LineCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("PlanNo", drRow["PlanNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("WorkInFlag", drRow["WorkInFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("FinishFlag", drRow["FinishFlag"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("MakeDate", drRow["MakeDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[13] = helper.CreateParameter("Maker", drRow["Maker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[14] = helper.CreateParameter("MakerNM", drRow["MakerNM"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[15] = helper.CreateParameter("FastOrderFlag", drRow["FastOrderFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[16] = helper.CreateParameter("FastOrderType", drRow["FastOrderType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[17] = helper.CreateParameter("Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[18] = helper.CreateParameter("ProdQty", drRow["ProdQty"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[19] = helper.CreateParameter("SeqNo", drRow["SeqNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[20] = helper.CreateParameter("ShiftGb", drRow["Maker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[21] = helper.CreateParameter("OrderUsage", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[22] = helper.CreateParameter("DayNight", drRow["SeqNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[23] = helper.CreateParameter("BaseCnt", drRow["PlantTime"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                            param[24] = helper.CreateParameter("BaseWgt", drRow["PlantTime"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                            param[25] = helper.CreateParameter("PlanTime", drRow["PlantTime"].ToString(), SqlDbType.Float, ParameterDirection.Input);



                            helper.ExecuteNoneQuery("USP_AP0300_U1N", CommandType.StoredProcedure, param);

                            //if (param[12].Value.ToString() == "E") throw new Exception(param[12].Value.ToString());

                            #endregion

                            break;
                    }
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
        #endregion

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "OrderNo")
            //    {
            //        _Fix_Col = i;
            //    }
            //}

            //for (int i = 0; i < _Fix_Col + 1; i++)
            //{
            //    e.Layout.UseFixedHeaders = true;
            //    e.Layout.Bands[0].Columns[i].Header.Fixed = true;
            //}
        }

    }
}
