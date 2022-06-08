#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : AP0700
//   Form Name    : 생산계획 대비 생산실적  조회
//   Name Space   : SAMMI.AP
//   Created Date : 2013-01-08
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
using Infragistics.Win;
using Infragistics.UltraChart.Resources.Appearance;
#endregion



namespace SAMMI.AP
{
    public partial class AP0700 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp2 = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();
        BizTextBoxManagerEX btbManager;
        //BizGridManagerEX gridManager;

        /************************************************************************/
        /* 2013.01.22 차승영 수정 NullRefenceException                                                                     */
        /************************************************************************/
        //private DataTable DtChange = null;
        //private DataTable DtChange2 = null;
        private DataTable DtChange = new DataTable();
        private DataTable DtChange2 = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();


        private ChartLayerAppearance lineLayer = new ChartLayerAppearance();
        private ChartLayerAppearance lineLayer2 = new ChartLayerAppearance();

        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >

        public AP0700()
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

            string sUseFlag = string.Empty;
            string sLineCode = string.Empty;
            string sOPCode = string.Empty;

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOPCode, txtOPName, txtLineCode, txtLineName });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { this.cboPlantCode_H, txtOPCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });

                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "", "" }
                       , new string[] { "", "" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOPCode, txtOPName, txtLineCode, txtLineName });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { LoginInfo.PlantAuth, txtOPCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });

                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "", "" }
                       , new string[] { "", "" }, new object[] { });
            }

        }
        #endregion

        #region AP0700_Load
        private void AP0700_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            //---------------------------------------------------------------------그리드 1 세팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            // InitColumnUltraGrid  (91 111 110 114 185 95 100 100 100 156 209 100 100 100 100 90 90 169 90 90 90 90 90 90 90 90 90 90 )
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정 명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "라인코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderNo", "지시번호", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
             _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderDate", "지시일자", false, GridColDataType_emu.YearMonthDay, 110, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderType", "지시유형", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SeqNo", "지시순번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderQty", "지시수량", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQty", "생산수량", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ratio", "생산율(%)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.##", null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            //grid1.Columns["OPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;

            grid1.DisplayLayout.UseFixedHeaders = true;
            for (int i = 0; i < 6; i++)
                grid1.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;



            #endregion

            DtChange = (DataTable)grid1.DataSource;

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            //SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");


            #endregion

            #region [Chart 초기화]
            this.ultraChart1.EmptyChartText = string.Empty;
            this.ultraChart2.EmptyChartText = string.Empty;
            #endregion

        }
        #endregion AP0700_Load

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            /************************************************************************/
            /* 2013.01.22 차승영 Parameter 중복 오류 수정                            */
            /************************************************************************/

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[7];
            SqlParameter[] param2 = new SqlParameter[7];

            try
            {
                DtChange.Clear();
                DtChange2.Clear();
                base.DoInquire();

                string sStartDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(calRegDT_FRH.Value));
                string sEndDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(calRegDT_TOH.Value));
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                         
                string sOPCode = txtOPCode.Text.Trim();
                string sLineCode = txtLineCode.Text.Trim();
                string sItemCode = txtItemCode.Text.Trim();
                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();



                param[0] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@LineCode ", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@ItemCode ", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);


                param2[0] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param2[1] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param2[2] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param2[3] = helper.CreateParameter("@OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param2[4] = helper.CreateParameter("@LineCode ", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                param2[5] = helper.CreateParameter("@ItemCode ", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param2[6] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_AP0700_S1N_UNION", CommandType.StoredProcedure, param);
                rtnDtTemp2 = helper.FillTable("USP_AP0700_S2N_UNION", CommandType.StoredProcedure, param2);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                if (rtnDtTemp.Rows.Count > 0)
                {
                    ultraChart1.Series.Clear();
                    ultraChart2.Series.Clear();

                    NumericSeries series = new NumericSeries();
                    series.Data.DataSource = rtnDtTemp;
                    series.Data.LabelColumn = "WorkCenterName";
                    series.Data.ValueColumn = "prodQty";
                    series.DataBind();

                    NumericSeries series1 = new NumericSeries();
                    series1.Data.DataSource = rtnDtTemp2;
                    series1.Data.LabelColumn = "WorkCenterName";
                    series1.Data.ValueColumn = "prodQty";
                    series1.DataBind();

                    ultraChart1.Series.Add(series);
                    ultraChart1.Data.DataBind();

                    ultraChart2.Series.Add(series1);
                    ultraChart2.Data.DataBind();

                    DtChange = rtnDtTemp;
                    DtChange2 = rtnDtTemp2;

                    //_Common.Grid_Column_Width(this.grid1); //grid 정리용

                    ultraChart2.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
                    ultraChart2.Tooltips.Font = new Font("맑은 고딕", 12);

                    ultraChart1.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
                    ultraChart1.Tooltips.Font = new Font("맑은 고딕", 12);
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
                if (param2 != null) { param2 = null; }
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


                            #endregion
                            break;

                        case DataRowState.Added:
                            #region 추가

                            #endregion
                            break;
                        case DataRowState.Modified:

                            #region 수정


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

        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {

        }

        private void ultraChart2_DataItemOver(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            if (ultraChart2.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                ultraChart2.Tooltips.FormatString = e.ColumnLabel + " : <DATA_VALUE:#,#>";
            }
        }

        private void ultraChart1_DataItemOver(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            if (ultraChart1.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                ultraChart1.Tooltips.FormatString = e.ColumnLabel + " : <DATA_VALUE:#,#>";
            }
        }
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "OrderQty", "ProdQty" });

        }
 

    }
}
