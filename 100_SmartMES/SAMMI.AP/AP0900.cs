#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : AP0900
//   Form Name    : 월별 품목별 WorkCenter별 작업지시추이
//   Name Space   : SAMMI.AP
//   Created Date : 2012-06-25
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 월별 품목별 WorkCenter별 작업지시추이
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
#endregion

namespace SAMMI.AP
{
    public partial class AP0900 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();
        BizTextBoxManagerEX btbManager;

        private DataTable DtChange = null;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        public Dictionary<string, NumericSeries> SeriesDict;

        private string PlantCode = string.Empty;
        public AP0900()
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
                btbManager.PopUpAdd(txtItemCode_H, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode_H, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                        , new string[] { "OPCode", "OPName" });
            }
            else
            {
                btbManager.PopUpAdd(txtItemCode_H, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode_H, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                        , new string[] { "OPCode", "OPName" });
            }
           // this.InitChart();
        }

        private ChartLayerAppearance lineLayer = new ChartLayerAppearance();

        public void InitChart()
        {

            ChartArea iChartArea = new ChartArea();

            // Create an X axis
            AxisItem xAxis = new AxisItem();
            xAxis.DataType = AxisDataType.String;
            xAxis.SetLabelAxisType = SetLabelAxisType.ContinuousData;
            xAxis.Labels.ItemFormatString = "<ITEM_LABEL>";
            xAxis.Margin.Far.Value = 2D;
            xAxis.ScrollScale.Visible = true;
            xAxis.Margin.Near.Value = 1.5D;
            xAxis.LineThickness = 1;
            xAxis.Extent = 70;

            // Create an Y axis
            AxisItem yAxis = new AxisItem();
            yAxis.axisNumber = AxisNumber.Y_Axis;
            yAxis.DataType = AxisDataType.Numeric;
            yAxis.Labels.ItemFormatString = "<DATA_VALUE:0.##>";
            yAxis.TickmarkStyle = AxisTickStyle.Smart;
            yAxis.ScrollScale.Visible = true;
            yAxis.LineThickness = 1;
            yAxis.Extent = 50;

            // Add the axes to the first ChartArea
            iChartArea.Axes.Add(xAxis);
            iChartArea.Axes.Add(yAxis);

            this.lineLayer.ChartArea = iChartArea;
            this.lineLayer.ChartType = ChartType.LineChart;
            this.lineLayer.AxisX = xAxis;
            this.lineLayer.AxisY = yAxis;


            this.ultraChart1.CompositeChart.ChartAreas.Add(iChartArea);
            this.ultraChart1.CompositeChart.ChartLayers.Add(this.lineLayer);

        }

        #region <조회>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {


                this.lineLayer.Series.Clear();

                base.DoInquire();
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                              // 사업장(공장)
                string SYyyy = Convert.ToDateTime(cboYear_H.Value).ToString("yyyy");                             // 년도
                string sItemCode = this.txtItemCode_H.Text.Trim();                                                    // 품목코드
                string sWorkCenterCode = this.txtWorkCenterCode_H.Text.Trim();                                      // 작업장코드
               
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@YEAR", SYyyy, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@OrderType", (cboOrdertype_H.Value.ToString()=="ALL"?"":cboOrdertype_H.Value), SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_AP0900_S3N_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();


                _Common.Grid_Column_Width(this.grid1); //grid 정리용  


                if (rtnDtTemp.Rows.Count > 0)
                {
                    ultraChart1.Series.Clear();
                    //----------------------------
                    DataTable dt = new DataTable();

                    dt.Columns.Add("WorkCenterCode", typeof(System.String));
                    //dt.Columns.Add("WorkCenterName", typeof(System.String));
                    //dt.Columns.Add("ItemCode", typeof(System.String));
                    //dt.Columns.Add("ItemName", typeof(System.String));
                    dt.Columns.Add("1월", typeof(System.Int32));
                    dt.Columns.Add("2월", typeof(System.Int32));
                    dt.Columns.Add("3월", typeof(System.Int32));
                    dt.Columns.Add("4월", typeof(System.Int32));
                    dt.Columns.Add("5월", typeof(System.Int32));
                    dt.Columns.Add("6월", typeof(System.Int32));
                    dt.Columns.Add("7월", typeof(System.Int32));
                    dt.Columns.Add("8월", typeof(System.Int32));
                    dt.Columns.Add("9월", typeof(System.Int32));
                    dt.Columns.Add("10월", typeof(System.Int32));
                    dt.Columns.Add("11월", typeof(System.Int32));
                    dt.Columns.Add("12월", typeof(System.Int32));

                    for (int i = 0; i < rtnDtTemp.Rows.Count; i++)
                    {
                        dt.Rows.Add(new object[] 
                        {
                              Convert.ToString(rtnDtTemp.Rows[i]["WorkCenterCode"])+":"+Convert.ToString(rtnDtTemp.Rows[i]["ItemCode"]),
                              //Convert.ToString(rtnDtTemp.Rows[i]["WorkCenterName"]),
                              //Convert.ToString(rtnDtTemp.Rows[i]["ItemCode"]),
                              //Convert.ToString(rtnDtTemp.Rows[i]["ItemName"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["JAN"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["FEB"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["MAR"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["APR"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["MAY"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["JUN"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["JUL"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["AUG"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["SEP"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["OCT"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["NOV"]),
                              Convert.ToInt32(rtnDtTemp.Rows[i]["DECS"])
                        });
                    }

                    ultraChart1.DataSource = dt;

                    ultraChart1.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
                    ultraChart1.Tooltips.Font = new Font("맑은 고딕", 12);

                    //********************** 시리즈 선언 **************************************************
                    //SeriesDict = new Dictionary<string, NumericSeries>();
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    SeriesDict.Add(dt.Rows[i]["WorkCenterCode"].ToString() + dt.Rows[i]["WorkCenterName"].ToString() + dt.Rows[i]["ItemCode"].ToString()
                    //                  , new NumericSeries());
                    //}

                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    this.lineLayer.Series.Add(SeriesDict[dt.Rows[i]["WorkCenterCode"].ToString() + dt.Rows[i]["WorkCenterName"].ToString() + dt.Rows[i]["ItemCode"].ToString()]);
                    //}

                    //this.ultraChart1.CompositeChart.ChartLayers.Add(this.lineLayer);

                    ////********************** 시리즈 값 입력 **************************************************

                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    for (int j = 4; j < dt.Columns.Count; j++)
                    //    {
                    //        SeriesDict[dt.Rows[i]["WorkCenterCode"].ToString() + dt.Rows[i]["WorkCenterName"].ToString() + dt.Rows[i]["ItemCode"].ToString()].Points.Add
                    //        (new NumericDataPoint(Convert.ToDouble((dt.Rows[i][j].ToString())), Convert.ToString(j - 1) + "월", false));
                    //        SeriesDict[dt.Rows[i]["WorkCenterCode"].ToString() + dt.Rows[i]["WorkCenterName"].ToString() + dt.Rows[i]["ItemCode"].ToString()].Label = dt.Rows[i]["ItemCode"].ToString();
                    //    }
                    //}
                    ////********************** 범례  **************************************************
                    //CompositeLegend myLegend = new CompositeLegend();
                    //myLegend.ChartLayers.Add(this.lineLayer);
                    //myLegend.Bounds = new Rectangle(3, 79, 97, 20);
                    //myLegend.BoundsMeasureType = MeasureType.Percentage;
                    ////myLegend.ChartComponent.Series.Add(); 
                    //myLegend.LabelStyle.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    //myLegend.PE.ElementType = PaintElementType.Gradient;
                    //myLegend.PE.FillGradientStyle = Infragistics.UltraChart.Shared.Styles.GradientStyle.ForwardDiagonal;
                    //myLegend.PE.Fill = Color.Azure;
                    //myLegend.PE.FillStopColor = Color.Transparent;
                    //myLegend.Border.CornerRadius = 10;
                    //myLegend.Border.Thickness = 0;
                    //this.ultraChart1.CompositeChart.Legends.Add(myLegend);

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
            }

        }
        #endregion

        private void ultraChart1_ChartDataClicked(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {

        }

        private void gbxHeader_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void AP0900_Load(object sender, EventArgs e)
        {
            btbManager = new BizTextBoxManagerEX();
            // TBM0100 : 품번
            // 1 : 품번, 2 : 품명, param[0] : PlantCode, param[1] : ItemType
            btbManager.PopUpAdd(txtItemCode_H, txtItemName, "TBM0100", new object[] { cboPlantCode_H, "" });
            btbManager.PopUpAdd(txtWorkCenterCode_H, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                    , new string[] { "OPCode", "OPName" }, new object[] { "", "" });
            this.ultraChart1.EmptyChartText = string.Empty;

            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

         //   _GridUtil.InitColumnUltraGrid(grid1, "OrderType", "지시구분", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품목", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "JAN", "1 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FEB", "2 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAR", "3 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "APR", "4 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAY", "5 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "JUN", "6 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "JUL", "7 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "AUG", "8 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SEP", "9 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OCT", "10 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "NOV", "11 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DECS", "12 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);

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

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion
        }

        private void ultraChart1_ChartDrawItem(object sender, Infragistics.UltraChart.Shared.Events.ChartDrawItemEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Primitive.Path) || e.Primitive.Path.EndsWith("Legend") == false)
            {
                return;
            }

            Polyline polyline = e.Primitive as Polyline;
            if (polyline == null)
            {
                return;
            }
            //polyline.PE.Fill = Color.Red;
            polyline.PE.StrokeWidth = 10;
        }

        private void ultraChart1_DataItemOver(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            if (ultraChart1.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                ultraChart1.Tooltips.FormatString = e.RowLabel.Trim() + " : <DATA_VALUE:#,#>";
            }
        }
    }
}
