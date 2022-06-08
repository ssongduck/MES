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
    public partial class QM8300 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        // private DataTable DtChange = null;

        DataTable DtChange = new DataTable();
        DataTable DtChange2 = new DataTable();
        DataTable DtTree = new DataTable();
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private TimeSpan TS;
        public string SpecType = string.Empty;
        public Dictionary<string, NumericSeries> SeriesDict;
        public Dictionary<string, string> GridDict;
        public double USL = 0.0;
        public double LSL = 0.0;
        public double UCL = 0.0;
        public double LCL = 0.0;
        public double CL = 0.0;
        public double I_AVG_ALL = 0.0;
        public double RUCL = 0.0;
        public double RLCL = 0.0;
        public double MR = 0.0;

        private Configuration appConfig;
        private string PlantCode = string.Empty;
        #endregion

        public QM8300()
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

            this.InitChart();

            /*트리 생성*/
            this.daTable2.Fill(this.ds.dTable2, this.PlantCode);
            _GridUtil.InitializeGrid(this.grid1, false, false, false, "", false);
        }

        #region <TOOL BAR AREA >
        public override void DoInquire()
        {
            base.DoInquire();


            this.txtSTDEV.Text = "";
            this.txtUSL.Text = "";
            this.txtUCL.Text = "";
            this.txtCL.Text = "";
            this.txtLSL.Text = "";
            this.txtLCL.Text = "";
            this.txtMAX.Text = "";
            this.txtMIN.Text = "";
            this.txtXbar_AVG.Text = "";
            this.txtK.Text = "";
            this.txtCP.Text = "";
            this.txtCPK.Text = "";
            this.txtR_MAX.Text = "";
            this.txtR_MIN.Text = "";
            this.txtR_AVG.Text = "";
            this.txtR_UCL.Text = "";
            this.txtR_CL.Text = "";

            this.USL = 0.0;
            this.LSL = 0.0;
            this.UCL = 0.0;
            this.LCL = 0.0;
            this.CL = 0.0;
            this.I_AVG_ALL = 0.0;
            this.RUCL = 0.0;
            this.RLCL = 0.0;
            this.MR = 0.0;

            this.XbarChart.CompositeChart.Legends.Clear();
            this.R_Chart.CompositeChart.Legends.Clear();

            this.xbar_lineLayer.Series.Clear();
            this.xbar_lineLayer.AxisY.ScrollScale.Scale = 1;
            this.xbar_lineLayer.AxisX.ScrollScale.Scale = 1;
            this.r_lineLayer.Series.Clear();
            this.r_lineLayer.AxisY.ScrollScale.Scale = 1;
            this.r_lineLayer.AxisX.ScrollScale.Scale = 1;

            //this.ds.dTable3.Clear();
            this.DtTable.Clear();

            if (SeriesDict != null)
                foreach (KeyValuePair<string, NumericSeries> kvp in SeriesDict)
                {
                    string key = kvp.Key;
                    SeriesDict[key].Points.Clear();
                }

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param1 = new SqlParameter[7];

            string PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            string WorkCenterCode = SqlDBHelper.nvlString(txtWorkCenterCode.Text);
            string WorkCenterOpCode = SqlDBHelper.nvlString(txtWorkCenterOPCode.Text);
            string ItemCode = SqlDBHelper.nvlString(txtItemCode.Text);
            string InspCode = SqlDBHelper.nvlString(txtInspCode.Text);
          
            DateTime frRegDT = Convert.ToDateTime(((DateTime)this.cboStartDate_H.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");
            DateTime toRegDT = Convert.ToDateTime(((DateTime)this.cboEndDate_H.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");
            string sStartDate = SqlDBHelper.nvlDateTime(frRegDT).ToString("yyyy-MM-dd");                           // 생산시작일자
            string sEndDate = SqlDBHelper.nvlDateTime(toRegDT).ToString("yyyy-MM-dd");                               // 생산  끝일자
            this.TS = toRegDT - frRegDT;

            try
            {
                param1[0] = helper.CreateParameter("PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param1[1] = helper.CreateParameter("WorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[2] = helper.CreateParameter("WorkCenterOpCode", WorkCenterOpCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[3] = helper.CreateParameter("InspCode", InspCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[4] = helper.CreateParameter("ItemCode", ItemCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[5] = helper.CreateParameter("frDT", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param1[6] = helper.CreateParameter("toDT", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드  

                rtnDtTemp = helper.FillTable("USP_QM8300_S1_UNION", CommandType.StoredProcedure, param1);
            }
            catch (Exception ex)
            {   
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param1 != null) { param1 = null; }
            }

            if (rtnDtTemp.Rows.Count == 0)
            {
                this.grid1.DataSource = this.DtTable;
                // 조회할 DATA가 없습니다.
                SException ex = new SException("R00111", null);
                throw ex;
            }
            this.DtTable = this.rtnDtTemp;

            this.GetSeries();
            this.GetGridData(this.rtnDtTemp);
            this.Result();
        }
        #endregion


        #region < Chart 기본 속성 >
        private ChartLayerAppearance xbar_lineLayer = new ChartLayerAppearance();
        //private ChartLayerAppearance xbar_lineLayer_value = new ChartLayerAppearance();
        private ChartLayerAppearance r_lineLayer = new ChartLayerAppearance();
        //private ChartLayerAppearance r_lineLayer_value    = new ChartLayerAppearance();

        public void InitChart()
        {

            ChartArea xbar_ChartArea = new ChartArea();
            ChartArea r_ChartArea = new ChartArea();

            //ChartArea xbar_ChartArea_value = new ChartArea();
            //ChartArea r_ChartArea_value    = new ChartArea();

            // Create an X axis
            AxisItem xAxis = new AxisItem();
            xAxis.DataType = AxisDataType.String;
            xAxis.SetLabelAxisType = SetLabelAxisType.ContinuousData;
            xAxis.Labels.ItemFormatString = "<ITEM_LABEL>";
            xAxis.Extent = 30;
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
            xAxis1.Extent = 30;
            xAxis1.LineThickness = 1;

            // Create an Y axis
            AxisItem yAxis1 = new AxisItem();
            yAxis1.axisNumber = AxisNumber.Y_Axis;
            yAxis1.DataType = AxisDataType.Numeric;
            yAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.000>";
            yAxis1.TickmarkStyle = AxisTickStyle.Smart;
            yAxis1.ScrollScale.Visible = true;
            yAxis1.Extent = 140;
            yAxis1.LineThickness = 1;

            // Add the axes to the first ChartArea
            xbar_ChartArea.Axes.Add(xAxis);
            xbar_ChartArea.Axes.Add(yAxis);
            // Add the axes to the first ChartArea
            r_ChartArea.Axes.Add(xAxis1);
            r_ChartArea.Axes.Add(yAxis1);
            // Add the axes to the first ChartArea
            //xbar_ChartArea_value.Axes.Add(xAxis2);
            //xbar_ChartArea_value.Axes.Add(yAxis2);
            // Add the axes to the first ChartArea
            //r_ChartArea_value.Axes.Add(xAxis2);
            //r_ChartArea_value.Axes.Add(yAxis2);

            this.xbar_lineLayer.ChartArea = xbar_ChartArea;
            this.xbar_lineLayer.ChartType = ChartType.LineChart;
            this.xbar_lineLayer.AxisX = xAxis;
            this.xbar_lineLayer.AxisY = yAxis;

            //((LineChartAppearance)this.xbar_lineLayer.ChartTypeAppearance).NullHandling = NullHandling.DontPlot;
            //((LineChartAppearance)this.xbar_lineLayer.ChartTypeAppearance).Thickness = 1;
            //((LineChartAppearance)this.xbar_lineLayer.ChartTypeAppearance).MidPointAnchors = false;

            this.r_lineLayer.ChartArea = r_ChartArea;
            this.r_lineLayer.ChartType = ChartType.LineChart;
            this.r_lineLayer.AxisX = xAxis1;
            this.r_lineLayer.AxisY = yAxis1;

            this.XbarChart.CompositeChart.ChartLayers.Add(this.xbar_lineLayer);
            //this.XbarChart.CompositeChart.ChartLayers.Add(this.xbar_lineLayer_value);
            this.XbarChart.CompositeChart.ChartAreas.Add(xbar_ChartArea);
            //this.XbarChart.CompositeChart.ChartAreas.Add(xbar_ChartArea_value);

            this.R_Chart.CompositeChart.ChartLayers.Add(this.r_lineLayer);
            //this.R_Chart.CompositeChart.ChartLayers.Add(this.r_lineLayer_value);
            this.R_Chart.CompositeChart.ChartAreas.Add(r_ChartArea);
            //this.R_Chart.CompositeChart.ChartAreas.Add(r_ChartArea_value);
        }
        #endregion

        #region < Graph >
        private void GetSeries()
        {
            try
            {
                //********************** 시리즈 값 입력 **************************************************
                DataView dv = rtnDtTemp.DefaultView;
                SeriesDict = new Dictionary<string, NumericSeries>();
                for (int i = 0; i < this.rtnDtTemp.Columns.Count; i++)
                {
                    if (this.DtTable.Columns[i].ColumnName.Length > 1 && (this.DtTable.Columns[i].ColumnName.Substring(0, 2) == "XX" || this.DtTable.Columns[i].ColumnName.Substring(0, 2) == "RR"))
                        SeriesDict.Add(this.DtTable.Columns[i].ColumnName.ToString(), new NumericSeries());
                }

                SeriesDict["XX_USL"].PEs.Add(new PaintElement(Color.Red));
                SeriesDict["XX_LSL"].PEs.Add(new PaintElement(Color.Red));
                SeriesDict["XX_CL"].PEs.Add(new PaintElement(Color.Black));
                SeriesDict["XX_UCL"].PEs.Add(new PaintElement(Color.Orange));
                SeriesDict["XX_LCL"].PEs.Add(new PaintElement(Color.Orange));
                SeriesDict["XX_AVG"].PEs.Add(new PaintElement(Color.Green));
                SeriesDict["XX_X"].PEs.Add(new PaintElement(Color.Blue));
                SeriesDict["RR_UCL"].PEs.Add(new PaintElement(Color.Red));
                SeriesDict["RR_CL"].PEs.Add(new PaintElement(Color.Red));
                SeriesDict["RR_R"].PEs.Add(new PaintElement(Color.Blue));

                this.USL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_USL"]);
                this.LSL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_LSL"]);
                this.CL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_CL"]);
                this.UCL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_UCL"]);
                this.LCL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_LCL"]);
                this.I_AVG_ALL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_AVG"]);
                this.RLCL = 0.0;
                this.RUCL = Convert.ToDouble(this.rtnDtTemp.Rows[0]["RR_UCL"]);
                this.MR = Convert.ToDouble(this.rtnDtTemp.Rows[0]["RR_CL"]);

                foreach (KeyValuePair<string, NumericSeries> kvp in SeriesDict)
                {
                    string key = kvp.Key;
                    DateTime frRegDT = Convert.ToDateTime(((DateTime)this.cboStartDate_H.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");
                    
                    for (int i = 0; i < Convert.ToInt32(TS.Days.ToString()); i++)
                    {
                        dv.RowFilter = "Convert(WorkDT,'System.String') Like '" + frRegDT.ToString("yyyy-MM-dd")+ "%'";
                        if (dv.Count == 0)
                        {
                            frRegDT = frRegDT.AddDays(1);
                            continue;
                        }

                        if (key.Substring(0, 2) == "XX" && key.ToString() != "XX_X")
                        {
                            this.xbar_lineLayer.Series.Add(SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()]);
                            SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()].Points
                                               .Add(new NumericDataPoint(Convert.ToDouble(this.DtTable.Rows[0][key])
                                                                        , frRegDT.ToString("dd"), false));
                        }
                        else if (key.Substring(0, 2) == "RR" && key.ToString() != "RR_R")
                        {
                            this.r_lineLayer.Series.Add(SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()]);
                            SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()].Points
                                               .Add(new NumericDataPoint(Convert.ToDouble(this.DtTable.Rows[0][key])
                                                                        , frRegDT.ToString("dd"), false));
                        }
                        frRegDT = frRegDT.AddDays(1);
                    }
                }

                foreach (KeyValuePair<string, NumericSeries> kvp in SeriesDict)
                {
                    string key = kvp.Key;
                    DateTime frRegDT = Convert.ToDateTime(((DateTime)this.cboStartDate_H.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");

                    int k = 0;
                    for (int i = 0; i < Convert.ToInt32(TS.Days.ToString()); i++)
                    {
                        dv.RowFilter = "Convert(WorkDT,'System.String') Like '" + frRegDT.ToString("yyyy-MM-dd") + "%'";
                        if (dv.Count == 0)
                        {
                            frRegDT = frRegDT.AddDays(1);
                            continue;
                        }
                        if (key == "XX_X")
                        {
                            if (this.DtTable.Rows[k]["WorkDT"].ToString() == frRegDT.ToString())
                            {
                                this.xbar_lineLayer.Series.Add(SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()]);

                                if (Convert.ToDouble(this.DtTable.Rows[k][key]) == 0)
                                    SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()].Points
                                                       .Add(new NumericDataPoint(Convert.ToDouble(null)
                                                                                , frRegDT.ToString("dd"), true));
                                else
                                    SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()].Points
                                                       .Add(new NumericDataPoint(Convert.ToDouble(this.DtTable.Rows[k][key])
                                                                                , frRegDT.ToString("dd"), false));
                                k++;
                            }
                            else
                                SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()].Points
                                                   .Add(new NumericDataPoint(0, frRegDT.ToString("dd"), false));
                        }

                        if (key == "RR_R")
                        {
                            if (this.DtTable.Rows[k]["WorkDT"].ToString() == frRegDT.ToString())
                            {
                                this.r_lineLayer.Series.Add(SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()]);
                                SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()].Points
                                                   .Add(new NumericDataPoint(Convert.ToDouble(this.DtTable.Rows[k][key])
                                                                            , frRegDT.ToString("dd"), false));
                                k++;
                            }
                            else
                                SeriesDict[this.DtTable.Columns[key].ColumnName.ToString()].Points
                                                   .Add(new NumericDataPoint(0, frRegDT.ToString("dd"), false));
                        }
                        if (k == this.DtTable.Rows.Count)
                            k = 0;
                        frRegDT = frRegDT.AddDays(1);
  
                    }
                }
                ((LineChartAppearance)this.xbar_lineLayer.ChartTypeAppearance).Thickness = 2;
                ((LineChartAppearance)this.xbar_lineLayer.ChartTypeAppearance).DrawStyle = LineDrawStyle.Solid;
                ((LineChartAppearance)this.r_lineLayer.ChartTypeAppearance).Thickness = 2;
                ((LineChartAppearance)this.r_lineLayer.ChartTypeAppearance).DrawStyle = LineDrawStyle.Solid;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region < Grid_Data >
        private void GetGridData(DataTable dt)
        {
            this.GridTable.Clear();
            this.GridTable.Columns.Clear();
            this.GridTable.Columns.Add(new DataColumn("검사일자", typeof(string)));
            DateTime frRegDT = Convert.ToDateTime(((DateTime)this.cboStartDate_H.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");
            DateTime toRegDT = Convert.ToDateTime(((DateTime)this.cboEndDate_H.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");

            // 날짜 빼기
            this.TS = toRegDT - frRegDT;
            for (int i = 0; i < Convert.ToInt32(TS.Days.ToString()) + 1; i++)
            {
                this.GridTable.Columns.Add(new DataColumn(frRegDT.ToString("MM-dd"), typeof(string)));
                frRegDT = frRegDT.AddDays(1);
            }
            DataRow newrow = this.GridTable.NewRow();
            newrow["검사일자"] = "X Bar 측정값";
            this.GridTable.Rows.Add(newrow);
            DataRow newrow1 = this.GridTable.NewRow();
            newrow1["검사일자"] = "R 측정값";
            this.GridTable.Rows.Add(newrow1);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.GridTable.Rows[0][Convert.ToDateTime(dt.Rows[i]["WorkDT"]).ToString("MM-dd")] = Math.Round(Convert.ToDouble(dt.Rows[i]["XX_X"]), 3);
                this.GridTable.Rows[1][Convert.ToDateTime(dt.Rows[i]["WorkDT"]).ToString("MM-dd")] = Math.Round(Convert.ToDouble(dt.Rows[i]["RR_R"]), 3);
            }

            this.grid1.DataSource = this.GridTable;
            // Grid 정렬, 폭, NoEdit 설정
            this.grid1.Rows[0].Cells[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            this.grid1.Rows[1].Cells[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            for (int i = 0; i < Convert.ToInt32(TS.Days.ToString()) + 1; i++)
            {
                this.grid1.DisplayLayout.Bands[0].Columns[i].Width = 80;
                this.grid1.DisplayLayout.Bands[0].Columns[i].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                this.grid1.DisplayLayout.Bands[0].Columns[i].CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                this.grid1.DisplayLayout.Bands[0].Columns[i].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                this.grid1.Rows[0].Cells[i + 1].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grid1.Rows[1].Cells[i + 1].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            }

            if (GridTable.Rows.Count > 0)
            {

                //SeriesDict["XX_CL"].Visible = false;
                //SeriesDict["RR_R"].Visible = false;

                for (int i = 0; i < this.DtTable.Rows.Count; i++)
                {
                    if (this.grid1.Rows[0].Cells[i].Value.ToString().Equals(string.Empty) && this.grid1.Rows[1].Cells[i].Value.ToString().Equals(string.Empty))
                    {
                        grid1.DisplayLayout.Bands[0].Columns[i].Hidden = true;
                    }
                }

            }
        }
        #endregion

        #region < Result >
        private void Result()
        {
            this.txtCL.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_CL"]),   6).ToString("0.######");
            this.txtUCL.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_UCL"]), 6).ToString("0.######");
            this.txtLCL.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_LCL"]), 6).ToString("0.######");
            this.txtUSL.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_USL"]), 6).ToString("0.######");
            this.txtLSL.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_LSL"]), 6).ToString("0.######");
            this.txtXbar_AVG.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_AVG"]), 6).ToString("0.######");
            this.txtMAX.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["M_MAX"]), 6).ToString("0.######");
            this.txtMIN.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["M_MIN"]), 6).ToString("0.######");
            this.txtSTDEV.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["X_STDEV"]), 6).ToString("0.######");
            this.txtR_AVG.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["RR_CL"]), 6).ToString("0.######");
            this.txtR_MAX.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["R_MAX"]), 6).ToString("0.######");
            this.txtR_MIN.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["R_MIN"]), 6).ToString("0.######");
            this.txtR_UCL.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["RR_UCL"]), 6).ToString("0.######");
            this.txtR_CL.Text = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["RR_CL"]), 6).ToString("0.######");


            double k   = 0.0;
            double cpk = 0.0;
            double cp  = 0.0;

            double usl = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_USL"]), 6);
            double lsl = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_LSL"]), 6);
            double avg = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["XX_AVG"]), 6);
            double stdev = Math.Round(Convert.ToDouble(this.rtnDtTemp.Rows[0]["X_STDEV"]), 6);

            //this.SpecType = this.rtnDtTemp.Rows[0]["SpecType"].ToString();
            this.SpecType = "B";
            //if (this.SpecType == null)
            //    this.SpecType = "B";

            // K 계산
            if (this.SpecType != "B") 
                k = 0.0;
            else
            {
                double imsiK    = System.Math.Abs((((usl + lsl) / 2) - avg) / ((usl - lsl) / 2));
                string imsiKStr = imsiK.ToString();
                if (imsiKStr != "Infinity")
                    k = System.Math.Abs(((usl + lsl) / 2 - avg)) / ((usl - lsl) / 2);
                else
                    k = 0.0;

                //if (stdev == null || stdev == 0)
                if (stdev == 0)
                {
                    this.txtCP.Text  = "0.0";
                    this.txtCPK.Text = "0.0";
                }
            }

            // 한쪽규격일 경우 Cp 계산
            switch (this.SpecType)
            {
                case "B":
                    cp = (usl - lsl) / (6 * stdev);
                    break;
                case "U":
                    cp = (usl - avg) / (3 * stdev);
                    SeriesDict["XX_LSL"].Visible = false;
                    SeriesDict["XX_LCL"].Visible = false;

                    break;
                case "L":
                    cp = (avg - lsl) / (3 * stdev);
                    SeriesDict["XX_USL"].Visible = false;
                    SeriesDict["XX_UCL"].Visible = false;
                    break;
            }
            //SeriesDict["XX_UCL"].Visible = false;
            //SeriesDict["XX_LCL"].Visible = false;
            SeriesDict["XX_AVG"].Visible = false;
            //SeriesDict["RR_UCL"].Visible = false;
            SeriesDict["RR_CL"].Visible  = false;
            // CPK 계산
            cpk = ((1 - k) * cp);

            this.txtK.Text   = k.ToString();
            this.txtCP.Text  = cp.ToString();
            this.txtCPK.Text = cpk.ToString();
        }
        #endregion

        #region [트리 데이터 생성]
        private void treItem_InitializeDataNode(object sender, Infragistics.Win.UltraWinTree.InitializeDataNodeEventArgs e)
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

        #region [폼 로드]
        private void QM8300_Load(object sender, EventArgs e)
        {
            this.cboStartDate_H.Value = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01 00:00:00"));
        }
        #endregion

        #region [트리 클릭]
        private void treItem_AfterSelect(object sender, SelectEventArgs e)
        {
        }
        #endregion

        #region [X-Bar Chart Legend]
        private void XbarChart_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            if (this.rtnDtTemp.Rows.Count == 0)
            {
                //e.SceneGraph.Clear();
                return;
            }

            if (this.XbarChart.CompositeChart.ChartLayers.Count == 0) return;
            if (this.XbarChart.CompositeChart.ChartLayers[0].ChartLayer.Grid.Count == 0) return;

            //차트레이어 없을때 

            // 차트축에있는 정보를 가져와서 상대 포지션
            IAdvanceAxis axisY = this.XbarChart.CompositeChart.ChartLayers[0].ChartLayer.Grid["Y"] as IAdvanceAxis;
            IAdvanceAxis axisX = this.XbarChart.CompositeChart.ChartLayers[0].ChartLayer.Grid["X"] as IAdvanceAxis;

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
        #endregion

        #region [R-Chart Graph Legend]
        private void R_Chart_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            //if (this.rtnDtTemp.Rows.Count == 0)
            //{
            //    //e.SceneGraph.Clear();
            //    return;
            //}

            ////차트레이어 없을때 
            //if (this.R_Chart.CompositeChart.ChartLayers.Count == 0) return;
            //if (this.R_Chart.CompositeChart.ChartLayers[0].ChartLayer.Grid.Count == 0) return;

            //// 차트축에있는 정보를 가져와서 상대 포지션
            //IAdvanceAxis axisY = this.R_Chart.CompositeChart.ChartLayers[0].ChartLayer.Grid["Y"] as IAdvanceAxis;
            //IAdvanceAxis axisX = this.R_Chart.CompositeChart.ChartLayers[0].ChartLayer.Grid["X"] as IAdvanceAxis;

            //int xStart = (int)axisX.MapMinimum;
            //int xEnd = (int)axisX.MapMaximum;

            //int uclY = (int)axisY.Map(this.RUCL);
            //int lclY = (int)axisY.Map(this.RLCL);
            //int clY = (int)axisY.Map(this.MR);

            //Line uclLn = new Line(new Point(xStart, uclY), new Point(xEnd, uclY));
            //Line lclLn = new Line(new Point(xStart, lclY), new Point(xEnd, lclY));
            //Line avgLn = new Line(new Point(xStart, clY), new Point(xEnd, clY));

            //uclLn.PE.Stroke = Color.Orange;
            //uclLn.PE.StrokeWidth = 2;
            //uclLn.lineStyle.DrawStyle = LineDrawStyle.Dash;
            //lclLn.PE.Stroke = Color.Orange;
            //lclLn.PE.StrokeWidth = 2;
            //lclLn.lineStyle.DrawStyle = LineDrawStyle.Dash;
            //avgLn.PE.Stroke = Color.Green;
            //avgLn.PE.StrokeWidth = 2;
            //avgLn.lineStyle.DrawStyle = LineDrawStyle.Solid;

            //if (this.SpecType != "L")
            //    e.SceneGraph.Add(uclLn);

            //if (this.SpecType != "U")
            //    e.SceneGraph.Add(lclLn);
            //e.SceneGraph.Add(avgLn);

            //// ********************************** MR , UCL , LCL  ******************************************************************

            //// VALUE Legend 추가 라벨 색상, 라벨위치 강제 - 
            //int clYCoord = (int)axisY.Map(this.MR);
            //Text clLabel = new Text();
            //clLabel.PE.Fill = Color.Green;
            //clLabel.SetTextString("VALUE: " + this.MR.ToString());
            //Size clLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(clLabel.GetTextString(), clLabel.labelStyle)); //크기 따오기
            //clLabel.bounds = new Rectangle(xStart + 90, clYCoord - 20, clLabelSize.Width, clLabelSize.Height);// 꼭지점 , 폭 , ㅍ높이

            //e.SceneGraph.Add(clLabel);

            //// RUCL Legend 추가 MR 와 USL이 같을 경우 그리지 않음
            //if (this.MR != this.RUCL)
            //{
            //    int uslYCoord = (int)axisY.Map(this.RUCL);
            //    Text uslLabel = new Text();
            //    uslLabel.PE.Fill = Color.Green;
            //    uslLabel.SetTextString("UCL : " + this.RUCL.ToString());
            //    Size uslLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(uslLabel.GetTextString(), uslLabel.labelStyle));
            //    uslLabel.bounds = new Rectangle(xStart, uslYCoord - uslLabelSize.Height, uslLabelSize.Width, uslLabelSize.Height);

            //    e.SceneGraph.Add(uslLabel);
            //}

            //// RLCL Legend 추가 MR 와 LSL이 같을 경우 그리지 않음
            //if (this.MR != this.RLCL)
            //{
            //    int lslYCoord = (int)axisY.Map(this.RLCL);
            //    Text lslLabel = new Text();
            //    lslLabel.PE.Fill = Color.Green;
            //    lslLabel.SetTextString("LCL : " + this.RLCL.ToString());
            //    Size lslLabelSize = Size.Ceiling(Platform.GetLabelSizePixels(lslLabel.GetTextString(), lslLabel.labelStyle));
            //    lslLabel.bounds = new Rectangle(xStart, lslYCoord - lslLabelSize.Height, lslLabelSize.Width, lslLabelSize.Height);

            //    e.SceneGraph.Add(lslLabel);
            //}

            //Rectangle legendBounds = new Rectangle(5, 5, 80, 95);
            //// a GraphicsContext Primitive will be used here to reset the clipping area of this
            //// layer, so that it is possible to draw over on the right side of the chart, outside
            //// of the grid area (which is the default clipping area for a custom layer)
            //GraphicsContext gc = new GraphicsContext();
            //gc.ResetClip();
            //// the GraphicsContext is added to the scene just like any other primitive.
            //e.SceneGraph.Add(gc);

            //// A box is used for the legend frame.
            //Box legendBox = new Box(legendBounds, new LineStyle());
            //// A PaintElement is used to make this box look nice.  This PaintElement uses
            //// a gradient.
            //PaintElement legendPE = new PaintElement();
            //legendPE.ElementType = PaintElementType.Gradient;
            //legendPE.Fill = Color.White;
            //legendPE.FillStopColor = Color.White;
            //legendPE.FillOpacity = 255;
            //legendPE.FillStopOpacity = 255;
            //legendPE.FillGradientStyle = GradientStyle.Horizontal;
            //legendBox.PE = legendPE;
            //e.SceneGraph.Add(legendBox);
            //// There will be two entries in this legend: one for Y, the other for Y2.
            //// This means 4 primitives must be drawn.  One colored Box and one Text label
            //// for each legend entry.

            ////UCL
            //Box legendEntryBoxY_ucl = new Box(new Point(legendBounds.X + 10, legendBounds.Y + 10), 15, 15, new LineStyle());
            //// A Hatch PaintElement is used for the Y2 legend entry colored box.
            //legendEntryBoxY_ucl.PE = new PaintElement();
            //legendEntryBoxY_ucl.PE.ElementType = PaintElementType.Hatch;
            //legendEntryBoxY_ucl.PE.Fill = Color.Orange;
            //legendEntryBoxY_ucl.PE.FillStopColor = Color.Transparent;
            //legendEntryBoxY_ucl.PE.Hatch = FillHatchStyle.DashedHorizontal;
            //e.SceneGraph.Add(legendEntryBoxY_ucl);
            //Text legendEntryTextY_ucl = new Text(new Point(legendBounds.X + 35, legendBounds.Y + 17), "UCL");
            //legendEntryTextY_ucl.labelStyle.Font = new Font("맑은 고딕", 8);
            //legendEntryTextY_ucl.labelStyle.FontColor = Color.Orange;
            //e.SceneGraph.Add(legendEntryTextY_ucl);

            ////LCL
            //Box legendEntryBoxY_lcl = new Box(new Point(legendBounds.X + 10, legendBounds.Y + 30), 15, 15, new LineStyle());
            //// A Hatch PaintElement is used for the Y2 legend entry colored box.
            //legendEntryBoxY_lcl.PE = new PaintElement();
            //legendEntryBoxY_lcl.PE.ElementType = PaintElementType.Hatch;
            //legendEntryBoxY_lcl.PE.Fill = Color.Orange;
            //legendEntryBoxY_lcl.PE.FillStopColor = Color.Transparent;
            //legendEntryBoxY_lcl.PE.Hatch = FillHatchStyle.DashedHorizontal;
            //e.SceneGraph.Add(legendEntryBoxY_lcl);
            //Text legendEntryTextY_lcl = new Text(new Point(legendBounds.X + 35, legendBounds.Y + 37), "LCL");
            //legendEntryTextY_lcl.labelStyle.Font = new Font("맑은 고딕", 8);
            //legendEntryTextY_lcl.labelStyle.FontColor = Color.Orange;
            //e.SceneGraph.Add(legendEntryTextY_lcl);

            //// AVG
            //Box legendEntryBoxY_avg = new Box(new Point(legendBounds.X + 10, legendBounds.Y + 50), 15, 15, new LineStyle());
            //legendEntryBoxY_avg.PE.Fill = Color.Green;
            //e.SceneGraph.Add(legendEntryBoxY_avg);
            //Text legendEntryTextY_avg = new Text(new Point(legendBounds.X + 35, legendBounds.Y + 57), "AVG");
            //legendEntryTextY_avg.labelStyle.Font = new Font("맑은 고딕", 8);
            //legendEntryTextY_avg.labelStyle.FontColor = Color.Green;
            //e.SceneGraph.Add(legendEntryTextY_avg);

            //// 측정 값
            //Box legendEntryBoxY_value = new Box(new Point(legendBounds.X + 10, legendBounds.Y + 70), 15, 15, new LineStyle());
            //legendEntryBoxY_value.PE.Fill = Color.Blue;
            //e.SceneGraph.Add(legendEntryBoxY_value);
            //Text legendEntryTextY_value = new Text(new Point(legendBounds.X + 35, legendBounds.Y + 77), "측정 값");
            //legendEntryTextY_value.labelStyle.Font = new Font("맑은 고딕", 8);
            //legendEntryTextY_value.labelStyle.FontColor = Color.Blue;
            //e.SceneGraph.Add(legendEntryTextY_value);

            //// 메모리 정리를 위한 COMMON CleanProcess 함수호출.
            //SAMMI.Common common = new Common();
            //common.CleanProcess("SK_MESDB_V1");
        }
        #endregion

        private void treItem_Click(object sender, EventArgs e)
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
    }
}
