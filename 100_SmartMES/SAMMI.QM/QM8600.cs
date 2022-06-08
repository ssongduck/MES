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
using SAMMI.Control;
using Infragistics.Win.UltraWinGrid;
using System.Configuration;
using Infragistics.Win.UltraWinChart;
using Infragistics.UltraChart.Core.Layers;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Core.Primitives;
using Infragistics.UltraChart.Core.Util;
using Microsoft.VisualBasic;
using Infragistics.Win.UltraWinTree;
#endregion

namespace SAMMI.QM
{
    public partial class QM8600 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp2 = new DataTable(); // return DataTable 공통
        private DataTable GridTable = new DataTable();

        public DataTable DtTable = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        UltraTree _TreeUtil = new UltraTree();
        Common.Common _Common = new Common.Common();
        private Configuration appConfig;

        private string PlantCode; //공장코드
        public NumericSeries series_value;
        public NumericSeries series_usl;
        public NumericSeries series_lsl;
        public NumericSeries series_cl;
        public NumericSeries series_ucl;
        public NumericSeries series_lcl;
        public NumericSeries series_i_avg;
        public NumericSeries series_onesigmaU;
        public NumericSeries series_twosigmaU;
        public NumericSeries series_thrsigmaU;
        public NumericSeries series_onesigmaL;
        public NumericSeries series_twosigmaL;
        public NumericSeries series_thrsigmaL;
        public NumericSeries series_mrucl;
        public NumericSeries series_mrlcl;
        public NumericSeries series_mr_value;
        public NumericSeries series_mr_agv;
        public NumericSeries series_histogram_value;
        public double I = 0.0;
        public double USL = 0.0;
        public double LSL = 0.0;
        public double I_STDEV_ALL = 0.0;
        public double I_AVG_ALL = 0.0;
        public double CL = 0.0;
        public double I_MAX = 0.0;
        public double I_MIN = 0.0;
        public double MR = 0.0;
        public double MR_MAX = 0.0;
        public double MR_MIN = 0.0;
        public double MR_AVG = 0.0;
        public double UCL = 0.0;
        public double LCL = 0.0;
        public double RUCL = 0.0;
        public double RLCL = 0.0;
        public int CNT = 0;
        public double OneSigmaU = 0.0;
        public double TwoSigmaU = 0.0;
        public double ThrSigmaU = 0.0;
        public double OneSigmaL = 0.0;
        public double TwoSigmaL = 0.0;
        public double ThrSigmaL = 0.0;
        public decimal A2 = 0;
        public decimal D3 = 0;
        public decimal D4 = 0;
        public string SpecType = string.Empty;
        public CPChartManager CpChart;
        //private string PlantCode = string.Empty;
        // public DataTable DtTable = new DataTable();
        #endregion

        public QM8600()
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

            appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //this.PlantCode = LoginInfo.UserPlantCode;
            this.HistogramChart.DataSource = null;
            this.InitChart();
            /*트리 생성*/
            this.daTable3.Fill(this.ds.dTable3, this.PlantCode);

            this.CpChart = new CPChartManager(this.HistogramChart);
            calRegDT_TOH.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _GridUtil.InitializeGrid(this.grid1, false, false, false, "", false);
 
        }

        private ChartLayerAppearance lineLayer = new ChartLayerAppearance();
        private ChartLayerAppearance lineLayer_value = new ChartLayerAppearance();
        private ChartLayerAppearance mrlineLayer_value = new ChartLayerAppearance();
        private ChartLayerAppearance histogramlineLayer = new ChartLayerAppearance();
        private CompositeLegend myLegend = new CompositeLegend();

        #region [Tool Bar Area]
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param1 = new SqlParameter[11];

            this.DtTable.Clear();
            this.CpChart.InitChart();

            base.DoInquire();

            string WorkCenterCode = SqlDBHelper.nvlString(txtWorkCenterCode.Text);
            string WorkCenterOpCode = SqlDBHelper.nvlString(txtWorkCenterOPCode.Text);
            string ItemCode = SqlDBHelper.nvlString(txtItemCode.Text);
            string InspCode = SqlDBHelper.nvlString(txtInspCode.Text);

            rtnDtTemp.Clear();
            this.DtTable.Clear();

            int i = 0;
            bool isNum = int.TryParse(this.txtCNT.Text, out i);
            if (!isNum)
            {
                if (txtCNT.Text.Equals(string.Empty))
                {
                    txtCNT.Text = "50";
                }
                else
                {
                    ShowDialog("COUNT를 확인하세요.", Windows.Forms.DialogForm.DialogType.OK);
                    return;
                }
            }
            int CNT = Convert.ToInt32(txtCNT.Text.Trim());

            this.A2 = Convert.ToDecimal(0.0);
            this.D4 = Convert.ToDecimal(0.0);
            this.D3 = Convert.ToDecimal(0.0);

            this.A2 = Convert.ToDecimal(1.88);
            this.D4 = Convert.ToDecimal(3.267);
            this.D3 = Convert.ToDecimal(0);

            this.series_value.Points.Clear();
            this.series_usl.Points.Clear();
            this.series_lsl.Points.Clear();
            this.series_cl.Points.Clear();
            this.series_ucl.Points.Clear();
            this.series_i_avg.Points.Clear();
            this.series_lcl.Points.Clear();
            this.series_onesigmaU.Points.Clear();
            this.series_twosigmaU.Points.Clear();
            this.series_thrsigmaU.Points.Clear();
            this.series_onesigmaL.Points.Clear();
            this.series_twosigmaL.Points.Clear();
            this.series_thrsigmaL.Points.Clear();
            this.series_mr_value.Points.Clear();
            this.series_mrucl.Points.Clear();
            this.series_mrlcl.Points.Clear();
            this.series_histogram_value.Points.Clear();
            this.I_chart.CompositeChart.Legends.Clear();
            this.R_Chart.CompositeChart.Legends.Clear();

            this.txtMAX.Text = "";
            this.txtSTDEV.Text = "";
            this.txtMIN.Text = "";
            this.txtUSL.Text = "";
            this.txtI_AVG.Text = "";
            this.txtUCL.Text = "";
            this.txtK.Text = "";
            this.txtCL.Text = "";
            this.txtCP.Text = "";
            this.txtLCL.Text = "";
            this.txtCPK.Text = "";
            this.txtLSL.Text = "";
            this.txtMR_MAX.Text = "";
            this.txtMR_UCL.Text = "";
            this.txtMR_MIN.Text = "";
            this.txtMR_LCL.Text = "";
            this.txtMR_AVG.Text = "";

            // 새로 조회시 Scale 초기화
            this.lineLayer.AxisY.RangeType = AxisRangeType.Automatic;
            this.lineLayer.AxisX.RangeType = AxisRangeType.Automatic;
            this.lineLayer_value.AxisY.RangeType = AxisRangeType.Automatic;
            this.lineLayer_value.AxisX.RangeType = AxisRangeType.Automatic;
            //this.mrlineLayer.AxisY.RangeType       = AxisRangeType.Automatic;
            //this.mrlineLayer.AxisX.RangeType       = AxisRangeType.Automatic;
            this.mrlineLayer_value.AxisY.RangeType = AxisRangeType.Automatic;
            this.mrlineLayer_value.AxisX.RangeType = AxisRangeType.Automatic;
            this.I = 0.0;
            this.USL = 0.0;
            this.LSL = 0.0;
            this.I_STDEV_ALL = 0.0;
            this.I_AVG_ALL = 0.0;
            this.CL = 0.0;
            this.I_MAX = 0.0;
            this.I_MIN = 0.0;
            this.MR = 0.0;
            this.MR_MAX = 0.0;
            this.MR_MIN = 0.0;
            this.UCL = 0.0;
            this.LCL = 0.0;
            this.RUCL = 0.0;
            this.RLCL = 0.0;
            this.CNT = 0;

            try
            {

                //DateTime toRegDT = Convert.ToDateTime(Convert.ToDateTime(this.calRegDT_TOH.Value).ToString("yyyy-MM-dd hh:mm:ss"));
                DateTime toRegDT = Convert.ToDateTime(this.calRegDT_TOH.Value);
                DateTime frRegDT = toRegDT.AddHours(-1);
                string sStartDate = frRegDT.ToString("yyyy-MM-dd HH:mm:ss");                           // 생산시작일자
                string sEndDate = toRegDT.ToString("yyyy-MM-dd HH:mm:ss");                               // 생산  끝일자
            
                param1[0] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param1[1] = helper.CreateParameter("WorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[2] = helper.CreateParameter("WorkCenterOpCode", WorkCenterOpCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[3] = helper.CreateParameter("ItemCode", ItemCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[4] = helper.CreateParameter("InspCode", InspCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[5] = helper.CreateParameter("frDT", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[6] = helper.CreateParameter("toDT", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[7] = helper.CreateParameter("A2", this.A2, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[8] = helper.CreateParameter("D4", this.D3, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[9] = helper.CreateParameter("D3", this.D4, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param1[10] = helper.CreateParameter("CNT", CNT, SqlDbType.Int, ParameterDirection.Input);   // 작업장 코드
                 rtnDtTemp = helper.FillTable("USP_QM8600_S1_UNION", CommandType.StoredProcedure, param1);

                if (this.rtnDtTemp.Rows.Count == 0)
                {
                    //this.grid1.DataSource = this.DtTable;
                    //this.HistogramChart.DataSource = null;
                    //this.HistogramChart.Visible = false;
                    // 조회할 DATA가 없습니다.
                    //SException ex = new SException("R00201", null);
                    //throw ex;
                    ShowDialog("조회할 데이터가 없습니다.", Windows.Forms.DialogForm.DialogType.OK);
                    return;
                } 
                
                this.I = Convert.ToDouble(this.rtnDtTemp.Rows[0]["I"]);
                this.USL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["USL"]);
                this.LSL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["LSL"]);
                this.I_STDEV_ALL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["I_STDEV_ALL"]);
                this.I_AVG_ALL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["I_AVG_ALL"]);
                this.CL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["CL"]);
                this.I_MAX = Convert.ToDouble(this.rtnDtTemp.Rows[0]["I_MAX"]);
                this.I_MIN = Convert.ToDouble(this.rtnDtTemp.Rows[0]["I_Min"]);
                this.MR = Convert.ToDouble(this.rtnDtTemp.Rows[0]["MR"]);
                this.MR_MAX = Convert.ToDouble(this.rtnDtTemp.Rows[0]["MR_MAX"]);
                this.MR_MIN = Convert.ToDouble(this.rtnDtTemp.Rows[0]["MR_MIN"]);
                this.MR_AVG = Convert.ToDouble(this.rtnDtTemp.Rows[0]["MR_AVG_ALL"]);
                this.UCL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["UCL"]);
                this.LCL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["LCL"]);
                this.RUCL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["RUCL"]);
                this.RLCL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["RLCL"]);
                this.CNT = this.rtnDtTemp.Rows.Count;

                double k = 0.0;
                double cpk = 0.0;
                double cp = 0.0;

                this.SpecType = this.rtnDtTemp.Rows[0]["SpecType"].ToString();
                if (this.SpecType == null)
                    this.SpecType = "B";
                // K 계산
                if (this.SpecType != "B") 
                    k = 0.0;
                else
                {
                    double imsiK = System.Math.Abs(((this.USL + this.LSL) / 2 - this.I_AVG_ALL)) / ((this.USL - this.LSL) / 2);
                    string imsiKStr = imsiK.ToString();
                    if (imsiKStr != "Infinity")
                        k = System.Math.Abs(((this.USL + this.LSL) / 2 - this.I_AVG_ALL)) / ((this.USL - this.LSL) / 2);
                    else
                        k = 0.0;

                    if (this.I_STDEV_ALL == null || this.I_STDEV_ALL == 0)
                    {
                        this.txtCP.Text = "0.0";
                        this.txtCPK.Text = "0.0";
                    }
                }

                // 한쪽규격일 경우 Cp 계산
                switch (this.SpecType)
                {
                    case "B":
                        cp = (this.USL - this.LSL) / (6 * this.I_STDEV_ALL);
                        break;
                    case "U":
                        cp = (this.USL - this.I_AVG_ALL) / (3 * this.I_STDEV_ALL);
                        break;
                    case "L":
                        cp = (this.I_AVG_ALL - this.LSL) / (3 * this.I_STDEV_ALL);
                        break;
                }
                // CPK 계산
                cpk = ((1 - k) * cp);

                this.txtK.Text = k.ToString();
                this.txtCP.Text = cp.ToString();
                this.txtCPK.Text = cpk.ToString();


                double x = 0;
                x = this.I_STDEV_ALL / 3;
                this.OneSigmaU = this.I_AVG_ALL + x;
                this.TwoSigmaU = this.I_AVG_ALL + 2 * x;
                this.ThrSigmaU = this.I_AVG_ALL + 3 * x;
                this.OneSigmaL = this.I_AVG_ALL - x;
                this.TwoSigmaL = this.I_AVG_ALL - 2 * x;
                this.ThrSigmaL = this.I_AVG_ALL - 3 * x;

                this.txtCL.Text = Math.Round(this.CL, 6).ToString("0.######");
                this.txtUCL.Text = Math.Round(this.UCL, 6).ToString("0.######");
                this.txtLCL.Text = Math.Round(this.LCL, 6).ToString("0.######");
                this.txtUSL.Text = Math.Round(this.USL, 6).ToString("0.######");
                this.txtLSL.Text = Math.Round(this.LSL, 6).ToString("0.######");
                this.txtCP.Text = Math.Round(cp, 6).ToString("0.######");
                this.txtCPK.Text = Math.Round(cpk, 6).ToString("0.######");
                this.txtK.Text = Math.Round(k, 6).ToString("0.######");
                this.txtI_AVG.Text = Math.Round(this.I_AVG_ALL, 6).ToString("0.######");
                this.txtMAX.Text = Math.Round(this.I_MAX, 6).ToString("0.######");
                this.txtMIN.Text = Math.Round(this.I_MIN, 6).ToString("0.######");
                this.txtSTDEV.Text = Math.Round(this.I_STDEV_ALL, 6).ToString("0.######");
                this.txtMR_AVG.Text = Math.Round(this.MR_AVG, 6).ToString("0.######");
                this.txtMR_MAX.Text = Math.Round(this.MR_MAX, 6).ToString("0.######");
                this.txtMR_MIN.Text = Math.Round(this.MR_MIN, 6).ToString("0.######");
                this.txtMR_UCL.Text = Math.Round(this.RUCL, 6).ToString("0.######");
                this.txtMR_LCL.Text = Math.Round(this.RLCL, 6).ToString("0.######");

                //this.InitChart();

                this.GetSeries();
                switch (this.SpecType)
                {
                    case "B":
                        break;
                    case "U":
                        this.series_lsl.Visible = false;
                        this.series_lcl.Visible = false;
                        break;
                    case "L":
                        this.series_usl.Visible = false;
                        this.series_ucl.Visible = false;
                        break;
                }
                //this.series_i_avg.Visible = false;

                this.Grid_Data(this.rtnDtTemp);

                this.GetHistSeries(series_histogram_value, this.rtnDtTemp, "H_VALUE", this.USL, this.LSL);

                this.CpChart.DrawChart(this.I_MIN
                                      , this.I_MAX
                                      , this.I_AVG_ALL
                                      , this.I_STDEV_ALL
                                      , this.CL
                                      , this.USL
                                      , this.LSL);


                //레전드(범례) 속성 셋팅

                //this.myLegend.ChartLayers.Add(this.lineLayer_value);
                //this.myLegend.ChartLayers.Add(this.lineLayer);
                //this.myLegend.Bounds               = new Rectangle(0, 0, 5, 100);
                //this.myLegend.BoundsMeasureType    = MeasureType.Percentage;
                //this.myLegend.LabelStyle.Font      = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //this.myLegend.PE.ElementType       = PaintElementType.Gradient;
                //this.myLegend.PE.FillGradientStyle = GradientStyle.ForwardDiagonal;
                //this.myLegend.PE.Fill              = Color.CornflowerBlue;
                //this.myLegend.PE.FillStopColor     = Color.Transparent;
                //this.myLegend.Border.CornerRadius  = 10;
                //this.myLegend.Border.Thickness     = 0;
                //this.myLegend.Visible              = true;

                //this.I_chart.CompositeChart.Legends.Add(this.myLegend);
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
        #endregion

        #region [GetSeries]
        private void GetSeries()
        {
            for (int i = 0; i < this.CNT; i++)
            {
                series_usl.Points.Add(new NumericDataPoint(Convert.ToDouble(this.rtnDtTemp.Rows[i]["USL"]), "", false));//(i + 1).ToString(), false));
                series_lsl.Points.Add(new NumericDataPoint(Convert.ToDouble(this.rtnDtTemp.Rows[i]["LSL"]), "", false));//(i + 1).ToString(), false));
                series_cl.Points.Add(new NumericDataPoint(Convert.ToDouble(this.rtnDtTemp.Rows[i]["CL"]), "", false));//(i + 1).ToString(), false));
                series_value.Points.Add(new NumericDataPoint(Convert.ToDouble(this.rtnDtTemp.Rows[i]["I"]), "", false));//(i + 1).ToString(), false));
                series_mr_value.Points.Add(new NumericDataPoint(Convert.ToDouble(this.rtnDtTemp.Rows[i]["MR"]), "", false));//(i + 1).ToString(), false));
                series_histogram_value.Points.Add(new NumericDataPoint(Convert.ToDouble(this.rtnDtTemp.Rows[i]["H_VALUE"]), "", false));//(i + 1).ToString(), false));

            }
            //DoLegend();
        }
        #endregion

        #region < Grid Data >
        private void Grid_Data(DataTable dt)
        {
            this.DtTable.Clear();
            this.DtTable.Columns.Clear();
            this.DtTable.Columns.Add(new DataColumn("검사일자", typeof(string)));
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                this.DtTable.Columns.Add(new DataColumn((i + 1).ToString(), typeof(string)));
            }
            DataRow newrow = this.DtTable.NewRow();
            newrow["검사일자"] = "검사시간";
            this.DtTable.Rows.Add(newrow);
            DataRow newrow1 = this.DtTable.NewRow();
            newrow1["검사일자"] = "I 측정값";
            this.DtTable.Rows.Add(newrow1);
            DataRow newrow2 = this.DtTable.NewRow();
            newrow2["검사일자"] = "MR 측정값";
            this.DtTable.Rows.Add(newrow2);
            DataRow newrow3 = this.DtTable.NewRow();
            newrow3["검사일자"] = "실 측정값";
            this.DtTable.Rows.Add(newrow3);
            DataRow newrow4 = this.DtTable.NewRow();
            newrow4["검사일자"] = "S/N";
            this.DtTable.Rows.Add(newrow4);
            try
            {
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    this.DtTable.Rows[0][i + 1] = Convert.ToDateTime(dt.Rows[i]["DADT"].ToString()).ToString("HH:mm:ss");
                    this.DtTable.Rows[1][i + 1] = Math.Round(Convert.ToDouble(dt.Rows[i]["I"].ToString()), 3).ToString();
                    this.DtTable.Rows[2][i + 1] = Math.Round(Convert.ToDouble(dt.Rows[i]["MR"].ToString()), 3).ToString();
                    this.DtTable.Rows[3][i + 1] = Math.Round(Convert.ToDouble(dt.Rows[i]["H_Value"].ToString()), 6).ToString();
                    this.DtTable.Rows[4][i + 1] = dt.Rows[i]["SerialNo"].ToString();
                }
            }
            catch (Exception ex)
            {
            }
            this.grid1.DataSource = this.DtTable;
            try
            {
                this.grid1.DisplayLayout.Bands[0].Columns[0].Header.Caption = "검사일자";
                this.grid1.Rows[0].Cells[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                this.grid1.Rows[1].Cells[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                this.grid1.Rows[2].Cells[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                this.grid1.Rows[3].Cells[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                this.grid1.Rows[4].Cells[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    //this.grid1.DisplayLayout.Bands[0].Columns[i + 1].Header.Caption = Convert.ToDateTime(dt.Rows[i]["DADT"].ToString()).ToString("yy-MM-dd");
                    this.grid1.DisplayLayout.Bands[0].Columns[i + 1].Header.Caption = dt.Rows[i]["ymd"].ToString(); //Convert.ToDateTime(calRegDT_TOH.Value).ToString("yy-MM-dd");
                    
                    this.grid1.DisplayLayout.Bands[0].Columns[i].Width = 80;
                    this.grid1.DisplayLayout.Bands[0].Columns[i].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                    this.grid1.DisplayLayout.Bands[0].Columns[i].CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                    this.grid1.DisplayLayout.Bands[0].Columns[i].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                    this.grid1.Rows[0].Cells[i + 1].Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                    this.grid1.Rows[1].Cells[i + 1].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    this.grid1.Rows[2].Cells[i + 1].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    this.grid1.Rows[3].Cells[i + 1].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    this.grid1.Rows[4].Cells[i + 1].Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region [History Serise]
        private void GetHistSeries(NumericSeries series, DataTable dt, string valcolname, double usl, double lsl)
        {
            //NumericSeries series = new NumericSeries();
            double min = 0;
            double max = 0;
            double gradewidth = 0;
            int gradecnt = Convert.ToInt32(System.Math.Ceiling(1 + 3.321 * System.Math.Log10(dt.Rows.Count)));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                min = min < Convert.ToDouble(dt.Rows[i][valcolname]) ? min : Convert.ToDouble(dt.Rows[i][valcolname]);
                max = max > Convert.ToDouble(dt.Rows[i][valcolname]) ? max : Convert.ToDouble(dt.Rows[i][valcolname]);
                series.Points.Add(new NumericDataPoint(Convert.ToDouble(dt.Rows[i][valcolname]),
                                                      Convert.ToDouble(dt.Rows[i][valcolname]).ToString(), false));

            }

            this.HistogramChart.Axis.X.RangeMin = this.LCL;
            this.HistogramChart.Axis.X.RangeMax = this.UCL;

            if (lsl != 0)
                min = min < lsl ? lsl : min;

            if (usl != 0)
                max = max < usl ? max : usl;

            gradewidth = System.Math.Abs((UCL - LCL)) / gradecnt;
            this.HistogramChart.Axis.X.RangeMin = this.LCL - gradewidth * 4;
            this.HistogramChart.Axis.X.RangeMax = this.UCL + gradewidth * 4;
            this.HistogramChart.Axis.X.TickmarkInterval = gradewidth;

            if (this.I_STDEV_ALL == 0)
                this.HistogramChart.Axis.X.TickmarkInterval = this.HistogramChart.Axis.X.RangeMax - this.HistogramChart.Axis.X.RangeMin;
            else
                this.HistogramChart.Axis.X.TickmarkInterval = gradewidth;
        }
        #endregion

        #region [Init Chart]
        public void InitChart()
        {
            ChartArea iChartArea = new ChartArea();
            ChartArea mrChartArea = new ChartArea();
            ChartArea histogramChartArea = new ChartArea();

            // Create an X axis
            AxisItem xAxis = new AxisItem();
            xAxis.DataType = AxisDataType.String;
            xAxis.SetLabelAxisType = SetLabelAxisType.ContinuousData;
            xAxis.Labels.ItemFormatString = "<ITEM_LABEL>";
            xAxis.Extent = 5;
            xAxis.LineThickness = 1;

            // Create an Y axis
            AxisItem yAxis = new AxisItem();
            yAxis.axisNumber = AxisNumber.Y_Axis;
            yAxis.DataType = AxisDataType.Numeric;
            yAxis.Labels.ItemFormatString = "<DATA_VALUE:0.000>";
            yAxis.TickmarkStyle = AxisTickStyle.Smart;
            yAxis.ScrollScale.Visible = true;
            yAxis.Extent = 140;
            yAxis.LineThickness = 1;

            // Create an X axis
            AxisItem xAxis1 = new AxisItem();
            xAxis1.DataType = AxisDataType.String;
            xAxis1.SetLabelAxisType = SetLabelAxisType.ContinuousData;
            xAxis1.Labels.ItemFormatString = "<ITEM_LABEL>";
            xAxis1.Extent = 5;
            xAxis1.LineThickness = 1;

            // Create an Y axis
            AxisItem yAxis1 = new AxisItem();
            yAxis1.axisNumber = AxisNumber.Y_Axis;
            yAxis1.DataType = AxisDataType.Numeric;
            yAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.000>";
            yAxis1.TickmarkStyle = AxisTickStyle.Smart;
            yAxis1.ScrollScale.Visible = true;
            yAxis1.LineThickness = 1;
            yAxis1.Extent = 140;

            // Create an X axis
            AxisItem xAxis2 = new AxisItem();
            xAxis2.DataType = AxisDataType.String;
            xAxis2.SetLabelAxisType = SetLabelAxisType.ContinuousData;
            xAxis2.Labels.ItemFormatString = "<ITEM_LABEL>";
            xAxis2.Extent = 30;
            xAxis2.LineThickness = 1;

            // Create an Y axis
            AxisItem yAxis2 = new AxisItem();
            yAxis2.axisNumber = AxisNumber.Y_Axis;
            yAxis2.DataType = AxisDataType.Numeric;
            yAxis2.Labels.ItemFormatString = "<DATA_VALUE:#.#>";
            yAxis2.TickmarkStyle = AxisTickStyle.Smart;
            yAxis2.ScrollScale.Visible = true;
            yAxis2.Extent = 30;

            // Add the axes to the first ChartArea
            iChartArea.Axes.Add(xAxis);
            iChartArea.Axes.Add(yAxis);

            mrChartArea.Axes.Add(xAxis1);
            mrChartArea.Axes.Add(yAxis1);

            histogramChartArea.Axes.Add(xAxis2);
            histogramChartArea.Axes.Add(yAxis2);

            this.lineLayer.ChartArea = iChartArea;
            this.lineLayer.ChartType = ChartType.LineChart;
            this.lineLayer.AxisX = xAxis;
            this.lineLayer.AxisY = yAxis;

            this.lineLayer_value.ChartArea = iChartArea;
            this.lineLayer_value.ChartType = ChartType.LineChart;
            this.lineLayer_value.AxisX = xAxis;
            this.lineLayer_value.AxisY = yAxis;
            
            this.mrlineLayer_value.ChartArea = mrChartArea;
            this.mrlineLayer_value.ChartType = ChartType.LineChart;
            this.mrlineLayer_value.AxisX = xAxis1;
            this.mrlineLayer_value.AxisY = yAxis1;

            this.histogramlineLayer.ChartArea = histogramChartArea;
            this.histogramlineLayer.ChartType = ChartType.LineChart;
            this.histogramlineLayer.AxisX = xAxis2;
            this.histogramlineLayer.AxisY = yAxis2;

            series_value = new NumericSeries();
            series_usl = new NumericSeries();
            series_lsl = new NumericSeries();
            series_cl = new NumericSeries();
            series_ucl = new NumericSeries();
            series_lcl = new NumericSeries();
            series_i_avg = new NumericSeries();
            series_onesigmaU = new NumericSeries();
            series_twosigmaU = new NumericSeries();
            series_thrsigmaU = new NumericSeries();
            series_onesigmaL = new NumericSeries();
            series_twosigmaL = new NumericSeries();
            series_thrsigmaL = new NumericSeries();
            series_mr_value = new NumericSeries();
            series_mrucl = new NumericSeries();
            series_mrlcl = new NumericSeries();
            series_histogram_value = new NumericSeries();

            // 라인 색 지정
            series_usl.PEs.Add(new PaintElement(Color.Red));
            series_lsl.PEs.Add(new PaintElement(Color.Red));
            series_cl.PEs.Add(new PaintElement(Color.Purple));
            series_ucl.PEs.Add(new PaintElement(Color.Orange));
            series_lcl.PEs.Add(new PaintElement(Color.Orange));
            series_i_avg.PEs.Add(new PaintElement(Color.Green));
            series_onesigmaU.PEs.Add(new PaintElement(Color.LightSeaGreen));
            series_twosigmaU.PEs.Add(new PaintElement(Color.LightSalmon));
            series_thrsigmaU.PEs.Add(new PaintElement(Color.LightCoral));
            series_onesigmaL.PEs.Add(new PaintElement(Color.LightSeaGreen));
            series_twosigmaL.PEs.Add(new PaintElement(Color.LightSalmon));
            series_thrsigmaL.PEs.Add(new PaintElement(Color.LightCoral));
            series_mr_value.PEs.Add(new PaintElement(Color.Blue));
            series_mrucl.PEs.Add(new PaintElement(Color.Orange));
            series_mrlcl.PEs.Add(new PaintElement(Color.Orange));
            series_value.PEs.Add(new PaintElement(Color.Blue));
            this.lineLayer.Series.Add(series_usl);
            this.lineLayer.Series.Add(series_lsl);
            this.lineLayer.Series.Add(series_cl);
            this.lineLayer_value.Series.Add(series_value);
            this.mrlineLayer_value.Series.Add(series_mr_value);
                


            ((LineChartAppearance)this.lineLayer_value.ChartTypeAppearance).Thickness = 1;
            ((LineChartAppearance)this.lineLayer_value.ChartTypeAppearance).DrawStyle = LineDrawStyle.Solid;
            ((LineChartAppearance)this.mrlineLayer_value.ChartTypeAppearance).Thickness = 1;
            ((LineChartAppearance)this.mrlineLayer_value.ChartTypeAppearance).DrawStyle = LineDrawStyle.Solid;
            ((LineChartAppearance)this.mrlineLayer_value.ChartTypeAppearance).MidPointAnchors = true;

            this.I_chart.CompositeChart.ChartAreas.Add(iChartArea);
            this.R_Chart.CompositeChart.ChartAreas.Add(mrChartArea);

            this.I_chart.CompositeChart.ChartLayers.Add(this.lineLayer);
            this.I_chart.CompositeChart.ChartLayers.Add(this.lineLayer_value);

            this.R_Chart.CompositeChart.ChartLayers.Add(this.mrlineLayer_value);

            this.HistogramChart.Series.Add(series_histogram_value);

        }
        #endregion

        #region [트리 노드 클릭]
        private void treItem_AfterSelect(object sender, SelectEventArgs e)
        {
        }
        private void treItem_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.treItem.ActiveNode.Cells["InspCode"].Value.ToString() != "$")
                {
                    this.txtWorkCenterCode.Text = this.treItem.ActiveNode.Cells["WorkCenterCode"].Value.ToString();
                    this.txtWorkCenterName.Text = this.treItem.ActiveNode.Cells["WorkCenterName"].Value.ToString();
                    this.txtItemCode.Text = this.treItem.ActiveNode.Cells["ItemCode"].Value.ToString();
                    this.txtItemName.Text = this.treItem.ActiveNode.Cells["ItemName"].Value.ToString();
                    this.txtWorkCenterOPCode.Text = this.treItem.ActiveNode.Cells["WorkCenterOpCode"].Value.ToString();
                    this.txtWorkCenterOPName.Text = this.treItem.ActiveNode.Cells["WorkCenterOpName"].Value.ToString();
                    this.txtInspCode.Text = this.treItem.ActiveNode.Cells["InspCode"].Value.ToString();
                    this.txtInspName.Text = this.treItem.ActiveNode.Cells["InspCodeName"].Value.ToString();
                    this.cboPlantCode_H.Value = this.treItem.ActiveNode.Cells["PlantCode"].Value.ToString();

                    ((SAMMI.Windows.Forms.BaseMDIChildForm)this).DoToolBarClick("InqFunc");
                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #region [트리 아이템 로드]
        private void treItem_InitializeDataNode(object sender, InitializeDataNodeEventArgs e)
        {
            if (e.Node.Parent == null && e.Node.Cells["ParentKey"].Value.ToString() != "$")
            {
                e.Node.Visible = false;
                return;
            };

            if (e.Node.Level < 1)
                e.Node.Expanded = true;
        }
        #endregion

        private void HistogramChart_InvalidDataReceived(object sender, Infragistics.UltraChart.Shared.Events.ChartDataInvalidEventArgs e)
        {
            e.Text = "";
        }
        /*사용안하는중*/
        private void DoLegend()
        {
            this.I_chart.CompositeChart.ChartLayers.Add(this.lineLayer);
            this.I_chart.CompositeChart.ChartLayers.Add(this.lineLayer_value);
            //this.R_Chart.CompositeChart.ChartLayers.Add(this.mrlineLayer);
            this.R_Chart.CompositeChart.ChartLayers.Add(this.mrlineLayer_value);
            this.HistogramChart.Series.Add(series_histogram_value);
            //this.HistogramChart.CompositeChart.ChartAreas.Add(histogramChartArea);
            //this.HistogramChart.CompositeChart.ChartLayers.Add(this.histogramlineLayer);

            ////HistogramChart레전드(범례) 속성 셋팅  -- 구현 안됨
            //CompositeLegend myLegend3      = new CompositeLegend();
            //myLegend3.ChartLayers.Add(this.histogramlineLayer);
            //myLegend3.Bounds               = new Rectangle(1, 3, 12, 90);
            //myLegend3.BoundsMeasureType    = MeasureType.Percentage;
            ////myLege3nd.ChartComponent.Series.Add(); 
            //myLegend3.LabelStyle.Font      = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //myLegend3.PE.ElementType       = PaintElementType.Gradient;
            //myLegend3.PE.FillGradientStyle = GradientStyle.ForwardDiagonal;
            //myLegend3.PE.Fill              = Color.CornflowerBlue;
            //myLegend3.PE.FillStopColor     = Color.Transparent;
            //myLegend3.Border.CornerRadius  = 10;
            //myLegend3.Border.Thickness     = 0;
            //this.HistogramChart.CompositeChart.Legends.Add(myLegend3);
        }

        private void I_chart_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {

            if (this.rtnDtTemp.Rows.Count == 0)
            {
                //e.SceneGraph.Clear();
                return;
            }

            if (this.I_chart.CompositeChart.ChartLayers.Count == 0) return;
            if (this.I_chart.CompositeChart.ChartLayers[0].ChartLayer.Grid.Count == 0) return;

            //차트레이어 없을때 

            // 차트축에있는 정보를 가져와서 상대 포지션
            IAdvanceAxis axisY = this.I_chart.CompositeChart.ChartLayers[0].ChartLayer.Grid["Y"] as IAdvanceAxis;
            IAdvanceAxis axisX = this.I_chart.CompositeChart.ChartLayers[0].ChartLayer.Grid["X"] as IAdvanceAxis;

            int xStart = (int)axisX.MapMinimum;
            int xEnd = (int)axisX.MapMaximum - 80;

            int uclY = (int)axisY.Map(this.UCL);
            int lclY = (int)axisY.Map(this.LCL);
            //int uslY   = (int)axisY.Map(this.USL);
            //int lslY   = (int)axisY.Map(this.LSL);
            //int clY    = (int)axisY.Map(this.CL);
            int avgY = (int)axisY.Map(this.I_AVG_ALL);

            Line uclLn = new Line(new Point(xStart, uclY), new Point(xEnd + 80, uclY));
            Line lclLn = new Line(new Point(xStart, lclY), new Point(xEnd + 80, lclY));
            //Line uslLn = new Line(new Point(xStart, uslY), new Point(xEnd + 80, uslY));
            //Line lslLn = new Line(new Point(xStart, lslY), new Point(xEnd + 80, lslY));
            //Line clLn  = new Line(new Point(xStart, clY),  new Point(xEnd + 80, clY));
            Line avgLn = new Line(new Point(xStart, avgY), new Point(xEnd + 80, avgY));

            uclLn.PE.Stroke = Color.Orange;
            uclLn.PE.StrokeWidth = 2;
            uclLn.lineStyle.DrawStyle = LineDrawStyle.Dash;
            lclLn.PE.Stroke = Color.Orange;
            lclLn.PE.StrokeWidth = 2;
            //lclLn.lineStyle.DrawStyle = LineDrawStyle.Dash;
            //uslLn.PE.Stroke           = Color.Red;
            //uslLn.PE.StrokeWidth      = 2;
            //uslLn.lineStyle.DrawStyle = LineDrawStyle.Solid;
            //lslLn.PE.Stroke           = Color.Red;
            //lslLn.PE.StrokeWidth      = 2;
            //lslLn.lineStyle.DrawStyle = LineDrawStyle.Solid;
            //clLn.PE.Stroke            = Color.Black;
            //clLn.PE.StrokeWidth       = 2;
            //clLn.lineStyle.DrawStyle  = LineDrawStyle.Solid;
            avgLn.PE.Stroke = Color.Green;
            avgLn.PE.StrokeWidth = 2;
            avgLn.lineStyle.DrawStyle = LineDrawStyle.Solid;
            if (this.SpecType != "L")
            {
                e.SceneGraph.Add(uclLn);
                //e.SceneGraph.Add(uslLn);
            }

            if (this.SpecType != "U")
            {
                e.SceneGraph.Add(lclLn);
                //e.SceneGraph.Add(lslLn);
            }
            //e.SceneGraph.Add(clLn);
            e.SceneGraph.Add(avgLn);

            // ********************************** CL , USL , LSL  ******************************************************************

            // CL Legend 추가 라벨 색상, 라벨위치 강제 - 
            int clYCoord = (int)axisY.Map(this.CL);
            Text clLabel = new Text();
            clLabel.PE.Fill = Color.Blue;
            clLabel.SetTextString("CL: " + this.CL.ToString());
            Size clLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(clLabel.GetTextString(), clLabel.labelStyle)); //크기 따오기
            clLabel.bounds = new Rectangle(xStart + 90, clYCoord - clLabelSize.Height, clLabelSize.Width, clLabelSize.Height);// 꼭지점 , 폭 , ㅍ높이

            e.SceneGraph.Add(clLabel);
            //spectype = this.ds.dTable3.Rows[0]["SpecType"].ToString();

            // USL Legend 추가 AVG 와 USL이 같을 경우 와 SPECTYPE 가 L 일경우 그리지 않음
            if (this.CL != this.USL)
            {
                if (this.SpecType != "L")
                {
                    int uslYCoord = (int)axisY.Map(this.USL);
                    Text uslLabel = new Text();
                    uslLabel.PE.Fill = Color.Blue;
                    uslLabel.SetTextString("USL : " + this.USL.ToString());
                    Size uslLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(uslLabel.GetTextString(), uslLabel.labelStyle));
                    uslLabel.bounds = new Rectangle(xStart, uslYCoord - uslLabelSize.Height, uslLabelSize.Width, uslLabelSize.Height);

                    e.SceneGraph.Add(uslLabel);
                }
            }

            // LSL Legend 추가 AVG 와 LSL이 같을 경우 와 SPECTYPE 가 U 일경우 그리지 않음
            if (this.CL != this.LSL)
            {
                if (this.SpecType != "U")
                {
                    int lslYCoord = (int)axisY.Map(this.LSL);
                    Text lslLabel = new Text();
                    lslLabel.PE.Fill = Color.Blue;
                    lslLabel.SetTextString("LSL : " + this.LSL.ToString());
                    Size lslLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(lslLabel.GetTextString(), lslLabel.labelStyle));
                    lslLabel.bounds = new Rectangle(xStart, lslYCoord - lslLabelSize.Height, lslLabelSize.Width, lslLabelSize.Height);

                    e.SceneGraph.Add(lslLabel);
                }
            }

            // ********************************** AVG , UCL , LCL  ******************************************************************


            // 평균 Legend 추가 라벨 색상, 라벨위치 강제 - 
            int avgYCoord = (int)axisY.Map(this.I_AVG_ALL);
            Text avgLabel = new Text();
            avgLabel.PE.Fill = Color.Red;
            avgLabel.SetTextString("AVG: " + this.I_AVG_ALL.ToString());
            Size avgLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(avgLabel.GetTextString(), avgLabel.labelStyle)); //크기 따오기
            avgLabel.bounds = new Rectangle(xEnd - 100, avgYCoord - avgLabelSize.Height, avgLabelSize.Width, avgLabelSize.Height);// 꼭지점 , 폭 , ㅍ높이

            e.SceneGraph.Add(avgLabel);

            // UCL Legend 추가 AVG 와 UCL이 같을 경우 와 SPECTYPE 가 L 일경우 그리지 않음
            if (this.I_AVG_ALL != this.UCL)
            {
                if (this.SpecType != "L")
                {
                    int uclYCoord = (int)axisY.Map(this.UCL);
                    Text uclLabel = new Text();
                    uclLabel.PE.Fill = Color.Red;
                    uclLabel.SetTextString("UCL : " + this.UCL.ToString());
                    Size uclLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(uclLabel.GetTextString(), uclLabel.labelStyle));
                    uclLabel.bounds = new Rectangle(xEnd, uclYCoord - uclLabelSize.Height, uclLabelSize.Width, uclLabelSize.Height);

                    e.SceneGraph.Add(uclLabel);
                }
            }

            // LCL Legend 추가 AVG 와 LCL이 같을 경우 와 SpecType 가  일경우 그리지 않음
            if (this.I_AVG_ALL != this.LCL)
            {
                if (this.SpecType != "U")
                {
                    int lclYCoord = (int)axisY.Map(this.LCL);
                    Text lclLabel = new Text();
                    lclLabel.PE.Fill = Color.Red;
                    lclLabel.SetTextString("LCL : " + this.LCL.ToString());
                    Size lclLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(lclLabel.GetTextString(), lclLabel.labelStyle));
                    lclLabel.bounds = new Rectangle(xEnd, lclYCoord - lclLabelSize.Height, lclLabelSize.Width, lclLabelSize.Height);

                    e.SceneGraph.Add(lclLabel);
                }
            }

            // 범례 표시 (Legend)
            Rectangle legendBounds = new Rectangle(5, 5, 80, 145);

            // a GraphicsContext Primitive will be used here to reset the clipping area of this
            // layer, so that it is possible to draw over on the right side of the chart, outside
            // of the grid area (which is the default clipping area for a custom layer)
            GraphicsContext gc = new GraphicsContext();
            gc.ResetClip();
            // the GraphicsContext is added to the scene just like any other primitive.
            e.SceneGraph.Add(gc);

            // A box is used for the legend frame.
            Box legendBox = new Box(legendBounds, new LineStyle());
            // A PaintElement is used to make this box look nice.  This PaintElement uses
            // a gradient.
            PaintElement legendPE = new PaintElement();
            legendPE.ElementType = PaintElementType.Gradient;
            legendPE.Fill = Color.White;
            legendPE.FillStopColor = Color.White;
            legendPE.FillOpacity = 255;
            legendPE.FillStopOpacity = 255;
            legendPE.FillGradientStyle = GradientStyle.Horizontal;
            legendBox.PE = legendPE;
            e.SceneGraph.Add(legendBox);
            // There will be two entries in this legend: one for Y, the other for Y2.
            // This means 4 primitives must be drawn.  One colored Box and one Text label
            // for each legend entry.
            int ybox = 5;
            int ytext = 12;
            if (this.SpecType != "L")
            {
                // USL
                Box legendEntryBoxY_usl = new Box(new Point(legendBounds.X + 10, legendBounds.Y + ybox), 15, 15, new LineStyle());
                legendEntryBoxY_usl.PE.Fill = Color.Red;
                // the hard part here is always finding the right spot to place the primitives in.
                // This requires some trial and error.
                Text legendEntryTextY_usl = new Text(new Point(legendBounds.X + 35, legendBounds.Y + ytext), "USL");
                legendEntryTextY_usl.labelStyle.Font = new Font("맑은 고딕", 8);
                legendEntryTextY_usl.labelStyle.FontColor = Color.Red;
                e.SceneGraph.Add(legendEntryBoxY_usl);
                e.SceneGraph.Add(legendEntryTextY_usl);
                ybox = ybox + 20; ytext = ytext + 20;
            }

            if (this.SpecType != "U")
            {
                // LSL
                Box legendEntryBoxY_lsl = new Box(new Point(legendBounds.X + 10, legendBounds.Y + ybox), 15, 15, new LineStyle());
                legendEntryBoxY_lsl.PE.Fill = Color.Red;
                Text legendEntryTextY_lsl = new Text(new Point(legendBounds.X + 35, legendBounds.Y + ytext), "LSL");
                legendEntryTextY_lsl.labelStyle.Font = new Font("맑은 고딕", 8);
                legendEntryTextY_lsl.labelStyle.FontColor = Color.Red;
                e.SceneGraph.Add(legendEntryBoxY_lsl);
                e.SceneGraph.Add(legendEntryTextY_lsl);
                ybox = ybox + 20; ytext = ytext + 20;
            }


            if (this.SpecType != "L")
            {
                // UCL
                Box legendEntryBoxY_ucl = new Box(new Point(legendBounds.X + 10, legendBounds.Y + ybox), 15, 15, new LineStyle());
                // A Hatch PaintElement is used for the Y2 legend entry colored box.
                legendEntryBoxY_ucl.PE = new PaintElement();
                legendEntryBoxY_ucl.PE.ElementType = PaintElementType.Hatch;
                legendEntryBoxY_ucl.PE.Fill = Color.Orange;
                legendEntryBoxY_ucl.PE.FillStopColor = Color.Transparent;
                legendEntryBoxY_ucl.PE.Hatch = FillHatchStyle.DashedHorizontal;
                Text legendEntryTextY_ucl = new Text(new Point(legendBounds.X + 35, legendBounds.Y + ytext), "UCL");
                legendEntryTextY_ucl.labelStyle.Font = new Font("맑은 고딕", 8);
                legendEntryTextY_ucl.labelStyle.FontColor = Color.Orange;

                e.SceneGraph.Add(legendEntryBoxY_ucl);
                e.SceneGraph.Add(legendEntryTextY_ucl);
                ybox = ybox + 20; ytext = ytext + 20;
            }

            if (this.SpecType != "U")
            {
                // LCL
                Box legendEntryBoxY_lcl = new Box(new Point(legendBounds.X + 10, legendBounds.Y + ybox), 15, 15, new LineStyle());
                // A Hatch PaintElement is used for the Y2 legend entry colored box.
                legendEntryBoxY_lcl.PE = new PaintElement();
                legendEntryBoxY_lcl.PE.ElementType = PaintElementType.Hatch;
                legendEntryBoxY_lcl.PE.Fill = Color.Orange;
                legendEntryBoxY_lcl.PE.FillStopColor = Color.Transparent;
                legendEntryBoxY_lcl.PE.Hatch = FillHatchStyle.DashedHorizontal;
                Text legendEntryTextY_lcl = new Text(new Point(legendBounds.X + 35, legendBounds.Y + ytext), "LCL");
                legendEntryTextY_lcl.labelStyle.Font = new Font("맑은 고딕", 8);
                legendEntryTextY_lcl.labelStyle.FontColor = Color.Orange;

                e.SceneGraph.Add(legendEntryBoxY_lcl);
                e.SceneGraph.Add(legendEntryTextY_lcl);
                ybox = ybox + 20; ytext = ytext + 20;
            }


            // CL
            Box legendEntryBoxY_cl = new Box(new Point(legendBounds.X + 10, legendBounds.Y + ybox), 15, 15, new LineStyle());
            legendEntryBoxY_cl.PE.Fill = Color.Black;
            Text legendEntryTextY_cl = new Text(new Point(legendBounds.X + 35, legendBounds.Y + ytext), "CL");
            legendEntryTextY_cl.labelStyle.Font = new Font("맑은 고딕", 8);
            legendEntryTextY_cl.labelStyle.FontColor = Color.Black;
            ybox = ybox + 20; ytext = ytext + 20;

            // AVG
            Box legendEntryBoxY_avg = new Box(new Point(legendBounds.X + 10, legendBounds.Y + ybox), 15, 15, new LineStyle());
            legendEntryBoxY_avg.PE.Fill = Color.Green;
            Text legendEntryTextY_avg = new Text(new Point(legendBounds.X + 35, legendBounds.Y + ytext), "AVG");
            legendEntryTextY_avg.labelStyle.Font = new Font("맑은 고딕", 8);
            legendEntryTextY_avg.labelStyle.FontColor = Color.Green;
            ybox = ybox + 20; ytext = ytext + 20;

            // 측정 값
            Box legendEntryBoxY_value = new Box(new Point(legendBounds.X + 10, legendBounds.Y + ybox), 15, 15, new LineStyle());
            legendEntryBoxY_value.PE.Fill = Color.Blue;
            Text legendEntryTextY_value = new Text(new Point(legendBounds.X + 35, legendBounds.Y + ytext), "측정 값");
            legendEntryTextY_value.labelStyle.Font = new Font("맑은 고딕", 8);
            legendEntryTextY_value.labelStyle.FontColor = Color.Blue;

            e.SceneGraph.Add(legendEntryBoxY_cl);
            e.SceneGraph.Add(legendEntryTextY_cl);
            e.SceneGraph.Add(legendEntryBoxY_avg);
            e.SceneGraph.Add(legendEntryTextY_avg);
            e.SceneGraph.Add(legendEntryBoxY_value);
            e.SceneGraph.Add(legendEntryTextY_value);

            // 메모리 정리를 위한 COMMON CleanProcess 함수호출.
            SAMMI.Common.Common common = new Common.Common();
            common.CleanProcess("SK_MESDB_V1");
        }


    }
}
