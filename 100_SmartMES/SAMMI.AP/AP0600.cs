#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : AP0600
//   Form Name    : 생산 계획 대비 실적
//   Name Space   : SAMMI.AP
//   Created Date : 
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 
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
using Infragistics.UltraChart.Core.Layers;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Core.Primitives;
using Infragistics.UltraChart.Core.Util;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.AP
{
    public partial class AP0600 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();
        BizTextBoxManagerEX btbManager;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        private NumericSeries seriesQ = new NumericSeries();

        private string PlantCode = string.Empty;

        #endregion

        public AP0600()
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

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { cboPlantCode_H, "" });
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
        private void InitChart()
        {
          
        }
        private ChartLayerAppearance lineLayer = new ChartLayerAppearance();

        private void DoChart()
        {

            if (rtnDtTemp.Rows.Count > 0)
            {

                NumericSeries series = new NumericSeries();
                DataTable table = new DataTable();

                table.Columns.Add("품명", typeof(string));
                table.Columns.Add("계획량", typeof(double));
                table.Columns.Add("생산량", typeof(double));
                if (grid1.Rows.Count != 0)
                {
                    for (int i = 0; i < this.rtnDtTemp.Rows.Count; i++)
                    {
                        table.Rows.Add(new object[] {
                              rtnDtTemp.Rows[i]["WorkCenterName"].ToString()+"\n"+rtnDtTemp.Rows[i]["ItemCode"].ToString(),
                               rtnDtTemp.Rows[i]["PlanQty"].ToString(),
                              rtnDtTemp.Rows[i]["ProdQty"].ToString()
                     });
                    }
                }
                
                ultraChart1.Visible = true;

                ultraChart1.Data.DataSource = table;
                ultraChart1.Axis.X.TimeAxisStyle.TimeAxisStyle = Infragistics.UltraChart.Shared.Styles.RulerGenre.Continuous;
                ultraChart1.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
                series.DataBind();

                ultraChart1.Data.DataBind();

                ultraChart1.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
                ultraChart1.Tooltips.Font = new Font("맑은 고딕", 12);

                ////************************   컬럼 차트 데이터 값 표시 *********************************

                //for (int k = 0; k < table.Rows.Count; k++)
                //{
                //    ChartTextAppearance PlantValue = new ChartTextAppearance();
                //    ChartTextAppearance ResultValue = new ChartTextAppearance();

                //    PlantValue.Row = k;
                //    PlantValue.Column = k;
                //    ResultValue.Row = k;
                //    ResultValue.Column = k;


                //    PlantValue.ItemFormatString = table.Rows[k]["계획량"].ToString();
                //    PlantValue.ItemFormatString = "<DATA_VALUE:0.#>";
                //    PlantValue.VerticalAlign = StringAlignment.Far;
                //    PlantValue.Visible = true;

                //    ResultValue.ItemFormatString = table.Rows[k]["생산량"].ToString();
                //    ResultValue.ItemFormatString = "<DATA_VALUE:0.#>";
                //    ResultValue.VerticalAlign = StringAlignment.Far;
                //    ResultValue.Visible = true;


                //    this.ultraChart1.ColumnChart.ChartText.Add(PlantValue);
                //    this.ultraChart1.ColumnChart.ChartText.Add(ResultValue);

                //    PlantValue.ChartTextFont = new Font("맑은고딕", 10f);
                //    PlantValue.FontColor = Color.Black;
                //    ResultValue.ChartTextFont = new Font("맑은고딕", 10f);
                //    ResultValue.FontColor = Color.Black;
                //}
            }
            else
            {
                ultraChart1.Series.Clear();
                ultraChart1.Visible = false;
            }
        }

        private void AP0600_Load(object sender, EventArgs e)
        {
            DoChart();

            #region Grid 셋팅

            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "라인코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품목",            false, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목명",           false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlanQty", "계획량",            false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQty", "생산량",            false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UnitCode", "단위",             false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Rate", "생산율(%)",              false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "color", " ", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);


            _GridUtil.SetInitUltraGridBind(grid1);

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
            #endregion

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            //SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            #endregion
        }
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "PlanQty", "ProdQty" });

        }
 
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[7];
            base.DoInquire();

            try
            {
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                 // 사업장(공장)
                string sStartDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(calRegDT_FRH.Value));                                            // 시작 일자
                string sEndDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(calRegDT_TOH.Value));                                                // 종료 일자                           // 년도
                string sOPCode = this.txtOPCode.Text.Trim();                                                                          // 품목코드
                string sOPName = this.txtOPName.Text.Trim();
                string sWorkCenterCode = this.txtWorkCenterCode.Text.Trim();
                string sWorkCenterName = this.txtWorkCenterName.Text.Trim();                                                          // 품목코드
                string sItemCode = this.txtItemCode.Text.Trim();
                string sLineCode = txtLineCode.Text.Trim();
                string sItemName = this.txtItemName.Text.Trim();

                param[0] = helper.CreateParameter("@PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@LineCode ", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_AP0600_S1N_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();
                _Common.Grid_Column_Width(this.grid1); //grid 정리용   

                DoChart();

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

        private void ultraChart1_DataItemOver(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            if (ultraChart1.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                ultraChart1.Tooltips.FormatString = e.RowLabel.Trim() + " : <DATA_VALUE:#,#>";
            }
        }

        private void grid1_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (e.Row.Cells["color"].Value.ToString() == "RED")
                e.Row.Cells["Rate"].Appearance.ForeColor = Color.Red;

        }

    }
}