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
//using Infragistics.Win;
using System.Configuration;
using Infragistics.UltraChart.Core.Layers;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Core.Primitives;
using Infragistics.UltraChart.Core.Util;
#endregion

namespace SAMMI.QM
{
    public partial class QM8100 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp2 = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp3 = new DataTable(); // return DataTable 공통

        public DataTable DtTable = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        // private DataTable DtChange = null;

        DataTable DtChange = new DataTable();
        DataTable DtChange2 = new DataTable();
        DataTable DtChange3 = new DataTable();
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private TimeSpan TS;
        private string pCP = string.Empty;
        private string PlantCode = string.Empty;
        public Dictionary<string, NumericSeries> SeriesDict;
        #endregion

        public QM8100()
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
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "4", "", "" }
                        , new string[] { }, new object[] { });
                btbManager.PopUpAdd(txtWorkCenterOPCode, txtWorkCenterOPName, "TBM0610", new object[] { this.cboPlantCode_H, txtWorkCenterCode, "", "", "" }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { txtWorkCenterCode, txtWorkCenterName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { this.cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "4", "", "" }
                        , new string[] { }, new object[] { });
                btbManager.PopUpAdd(txtWorkCenterOPCode, txtWorkCenterOPName, "TBM0610", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, "", "", "" }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { txtWorkCenterCode, txtWorkCenterName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }

            GridInit1();
            GridInit2();
            initChart();
        }
        private ChartLayerAppearance lineLayer = new ChartLayerAppearance();

        public override void DoInquire()
        {
            if (txtWorkCenterCode.Text.Equals(string.Empty) || txtItemCode.Text.Equals(string.Empty) 
                || SqlDBHelper.nvlString(this.cboPlantCode_H.Value).Equals(string.Empty))
            {
                ShowDialog("사업장, 작업장, 품목은 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);
                return;
            }

            string CP = string.Empty;

            CP = SqlDBHelper.nvlString(cboCPType.Value);
            if (CP.Equals(string.Empty))
                CP = "ALL";
            //MessageBox.Show("[" + CP + "]");
           

            this.CP_Chart.CompositeChart.Legends.Clear();
            this.lineLayer.Series.Clear();
            this.rtnDtTemp.Clear();
            this.rtnDtTemp2.Clear();
            this.DtTable.Clear();

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];
            SqlParameter[] param2 = new SqlParameter[7];

            try
            {
                base.DoInquire();

                #region [검사항목 조회]
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                                                      // 사업장(공장)
                string sWorkCenterCode = SqlDBHelper.nvlString(this.txtWorkCenterCode.Text.Trim());                                 // 작업장 코드
                string sWorkCenterOpCode = SqlDBHelper.nvlString(this.txtWorkCenterOPCode.Text.Trim());                                 // 작업장 코드
                string sItemCode = SqlDBHelper.nvlString(this.txtItemCode.Text.Trim());

                string sStartDate = SqlDBHelper.nvlDateTime(cboStartDate_H.Value).ToString("yyyy-MM-dd");                           // 생산시작일자
                string sEndDate = SqlDBHelper.nvlDateTime(cboEndDate_H.Value).ToString("yyyy-MM-dd");                               // 생산  끝일자
                DateTime frRegDT = Convert.ToDateTime(((DateTime)this.cboStartDate_H.Value).ToString("yyyy-MM-dd") + " 08:00:00.00");
                DateTime toRegDT = Convert.ToDateTime(((DateTime)this.cboEndDate_H.Value).AddDays(1).ToString("yyyy-MM-dd") + " 07:59:59.99");
                this.TS = toRegDT - frRegDT;
               
                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[1] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param[2] = helper.CreateParameter("WorkCenterOpCode", sWorkCenterOpCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param[3] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     

                rtnDtTemp = helper.FillTable("USP_QM8100_S1_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;

                if (rtnDtTemp.Rows.Count == 0)
                {
                    this.ClosePrgForm();
                    SAMMI.Windows.Forms.CheckForm checkform = new Windows.Forms.CheckForm("조회할 데이터를 선택하세요.");
                    checkform.ShowDialog();
                    return;
                }
                #endregion

               // MessageBox.Show(CP);
                #region [일별 공정능력 조회]

                param2[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param2[1] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param2[2] = helper.CreateParameter("WorkCenterOpCode", sWorkCenterOpCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param2[3] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param2[4] = helper.CreateParameter("frRegDT", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param2[5] = helper.CreateParameter("toRegDT", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param2[6] = helper.CreateParameter("CP", "ALL", SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     

                rtnDtTemp2 = helper.FillTable("USP_QM8100_S2N_UNION", CommandType.StoredProcedure, param2);
                //rtnDtTemp2 = helper.FillTable("USP_QM8100_S2", CommandType.StoredProcedure, param2);
                //grid2.DataSource = rtnDtTemp2;
                //grid2.DataBind();

                DtChange2 = rtnDtTemp2;

                if (rtnDtTemp2.Rows.Count == 0)
                {
                    this.ClosePrgForm();
                    SAMMI.Windows.Forms.CheckForm checkform = new Windows.Forms.CheckForm("조회된 데이터가 없습니다.");
                    checkform.ShowDialog();
                    return;
                }
                #endregion

                #region [시리즈 값 입력 - CP값 조건확인할것]
                SeriesDict = new Dictionary<string, NumericSeries>();
                SeriesDict.Clear();
                for (int i = 0; i < rtnDtTemp.Rows.Count; i++)
                {

                    SeriesDict.Add(rtnDtTemp.Rows[i]["InspCode"].ToString() + rtnDtTemp.Rows[i]["InspName"].ToString() + "CP"
                        , new NumericSeries());
                    SeriesDict.Add(rtnDtTemp.Rows[i]["InspCode"].ToString() + rtnDtTemp.Rows[i]["InspName"].ToString() + "CPK"
                        , new NumericSeries());
                }

                for (int i = 0; i < rtnDtTemp.Rows.Count; i++)
                {
                    this.lineLayer.Series.Add(SeriesDict[rtnDtTemp.Rows[i]["InspCode"].ToString() + rtnDtTemp.Rows[i]["InspName"].ToString() + "CP"]);
                    this.lineLayer.Series.Add(SeriesDict[rtnDtTemp.Rows[i]["InspCode"].ToString() + rtnDtTemp.Rows[i]["InspName"].ToString() + "CPK"]);
                }
                #endregion

                this.CP_Chart.CompositeChart.ChartLayers.Add(this.lineLayer);

                this.Grid_Data(this.rtnDtTemp2, CP);

                setChart();

                // ***************** 라인차트 , 그리드 데이터 숨기기 ******************************
                if (DtChange2.Rows.Count > 0)
                {
                    for (int k = 0; k < grid1.Rows.Count; k++)
                    {
                        try
                        {
                            SeriesDict[this.grid1.Rows[k].Cells["InspCode"].Value.ToString()
                                      + this.grid1.Rows[k].Cells["InspName"].Value.ToString()
                                      + "CP"].Visible = false;
                            SeriesDict[this.grid1.Rows[k].Cells["InspCode"].Value.ToString()
                                       + this.grid1.Rows[k].Cells["InspName"].Value.ToString()
                                       + "CPK"].Visible = false;
                        }
                        catch (Exception ex)
                        {

                        }

                        for (int i = 0; i < this.DtTable.Rows.Count; i++)
                        {
                            if (this.grid1.Rows[k].Cells["InspCode"].Value.ToString()
                                 + this.grid1.Rows[k].Cells["InspName"].Value.ToString()
                                 + "CP" == DtTable.Rows[i]["검사코드"].ToString() + DtTable.Rows[i]["검사항목"].ToString() + "CP")
                            {
                                grid2.Rows[i].Hidden = true;
                            }
                            if (this.grid1.Rows[k].Cells["InspCode"].Value.ToString()
                                + this.grid1.Rows[k].Cells["InspName"].Value.ToString()
                                + "CPK" == DtTable.Rows[i]["검사코드"].ToString() + DtTable.Rows[i]["검사항목"].ToString() + "CPK")
                            {
                                grid2.Rows[i].Hidden = true;
                            }
                        }
                    }
                }
                //레전드(범례) 속성 셋팅

                CompositeLegend myLegend = new CompositeLegend();
                myLegend.ChartLayers.Add(this.lineLayer);
//                MessageBox.Show(this.Height.ToString()+" : "+CP_Chart.Height.ToString());
                myLegend.Bounds = new Rectangle(7, 84, 92, 16);
                myLegend.BoundsMeasureType = MeasureType.Percentage;
                //myLegend.ChartComponent.Series.Add(); 
                myLegend.LabelStyle.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                myLegend.PE.ElementType = PaintElementType.Gradient;
                myLegend.PE.FillGradientStyle = GradientStyle.ForwardDiagonal;
                myLegend.PE.Fill = Color.Azure;
                myLegend.PE.FillStopColor = Color.Transparent;
                myLegend.Border.CornerRadius = 10;
                myLegend.Border.Thickness = 0;
                this.CP_Chart.CompositeChart.Legends.Add(myLegend);
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


        #region [Grid1 셋팅]
        private void GridInit1()
        {
            _GridUtil.InitializeGrid(this.grid1);

            _GridUtil.InitColumnUltraGrid(grid1, "vcheck", "선택", false, GridColDataType_emu.CheckBox, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOpCode", "작업장Op코드", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOpName", "작업장Op", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCode", "검사코드", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "검사명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            //grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 1;
            //grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            //grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

        }
        #endregion

        #region [Grid2 셋팅]
        private void GridInit2()
        {
            _GridUtil.InitializeGrid(this.grid2);
            grid2.DisplayLayout.Override.RowSelectorWidth = 1;
 
           // _GridUtil.InitColumnUltraGrid(grid1, "검사항목", "검사항목", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            
           
       }
        #endregion

        #region [Chart 초기화]
        private void initChart()
        {
            ChartArea iChartArea = new ChartArea();

            // Create an X axis
            AxisItem xAxis = new AxisItem();
            xAxis.DataType = AxisDataType.String;
            xAxis.SetLabelAxisType = SetLabelAxisType.ContinuousData;
            xAxis.Labels.ItemFormatString = "<ITEM_LABEL>";
            xAxis.LineThickness = 1;
            xAxis.Extent = 100;

            // Create an Y axis
            AxisItem yAxis = new AxisItem();
            yAxis.axisNumber = AxisNumber.Y_Axis;
            yAxis.DataType = AxisDataType.Numeric;
            yAxis.Labels.ItemFormatString = "<DATA_VALUE:0.00>";
            yAxis.TickmarkStyle = AxisTickStyle.Smart;
            yAxis.LineThickness = 1;
            yAxis.ScrollScale.Visible = true;
            yAxis.Extent = 40;

            // Add the axes to the first ChartArea
            iChartArea.Axes.Add(xAxis);
            iChartArea.Axes.Add(yAxis);

            this.lineLayer.ChartArea = iChartArea;
            this.lineLayer.ChartType = ChartType.LineChart;
            this.lineLayer.AxisX = xAxis;
            this.lineLayer.AxisY = yAxis;


            ////레전드(범례) 속성 셋팅

            //CompositeLegend myLegend = new CompositeLegend();
            //myLegend.ChartLayers.Add(this.lineLayer);
            //myLegend.Bounds = new Rectangle(7, 76, 92, 20);
            //myLegend.BoundsMeasureType = MeasureType.Percentage;
            ////myLegend.ChartComponent.Series.Add(); 
            //myLegend.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //myLegend.PE.ElementType = PaintElementType.Gradient;
            //myLegend.PE.FillGradientStyle = GradientStyle.ForwardDiagonal;
            //myLegend.PE.Fill = Color.Azure;
            //myLegend.PE.FillStopColor = Color.Transparent;
            //myLegend.Border.CornerRadius = 10;
            //myLegend.Border.Thickness = 0;
            //this.CP_Chart.CompositeChart.Legends.Add(myLegend);

            // ################################## 차트 배경 이미지 설정 ###################################

            //this.CP_Chart.BackColor = System.Drawing.Color.FromArgb(172,196,224);
            //iChartArea.Border.Color = System.Drawing.Color.FromArgb(172, 196, 224);
            //this.CP_Chart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;

            //this.splitContainer1.Panel1.Border.Color = System.Drawing.Color.Yellow;
            //this.splitContainer1.Panel1.Border.CornerRadius = 20;
            //this.splitContainer1.Panel1.Border.Raised = true;


            this.CP_Chart.CompositeChart.ChartAreas.Add(iChartArea);
            this.CP_Chart.CompositeChart.ChartLayers.Add(this.lineLayer);
        }
        #endregion

        #region [Chart 셋팅]
        private void setChart()
        {
            try
            {

                foreach (KeyValuePair<string, NumericSeries> kvp in SeriesDict)
                {
                    string key = kvp.Key;
                    DateTime frRegDT = Convert.ToDateTime(((DateTime)this.cboStartDate_H.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");

                    for (int i = 0; i < Convert.ToInt32(this.TS.Days.ToString()) + 1; i++)
                    {
                        bool isinsert = false;
                        for (int j = 0; j < this.DtChange2.Rows.Count; j++)
                        {
                            if (frRegDT.ToString() == this.DtChange2.Rows[j]["WorkDT"].ToString())
                            {

                                if (key == this.DtChange2.Rows[j]["InspCode"].ToString()
                                           + this.DtChange2.Rows[j]["InspName"].ToString()
                                           + this.DtChange2.Rows[j]["CPType"].ToString())
                                {
                                    SeriesDict[key].Points.Add(new NumericDataPoint(Convert.ToDouble((this.DtChange2.Rows[j]["VALUE"].ToString()))
                                                                                   , frRegDT.ToString("dd"), false));
                                    SeriesDict[key].Label = "[" + this.DtChange2.Rows[j]["InspCode"].ToString() + "]"
                                                            + this.DtChange2.Rows[j]["InspName"].ToString()
                                                            + "[" + this.DtChange2.Rows[j]["CPType"].ToString() + "]";
                                    isinsert = true;
                                }
                            }
                        }
                        // if (!isinsert)
                        //     SeriesDict[key].Points.Add(new NumericDataPoint(0, frRegDT.ToString("dd"), false));
                        frRegDT = frRegDT.AddDays(1);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("SetChart" + ex.ToString());
            }
        }
        #endregion

        #region [Grid2 날짜별 데이터 셋팅]
        private void Grid_Data(DataTable dt, string CP)
        {
            this.DtTable.Clear();
            this.DtTable.Columns.Clear();
            this.DtTable.Columns.Add(new DataColumn("검사코드", typeof(string)));
            this.DtTable.Columns.Add(new DataColumn("검사항목", typeof(string)));
            this.DtTable.Columns.Add(new DataColumn("검사구분", typeof(string)));
            this.DtTable.Columns.Add(new DataColumn("평균", typeof(string)));
 
            DateTime frRegDT = Convert.ToDateTime(((DateTime)this.cboStartDate_H.Value).ToString("yyyy-MM-dd") + " 08:00:00.00");
            DateTime toRegDT = Convert.ToDateTime(((DateTime)this.cboEndDate_H.Value).AddDays(1).ToString("yyyy-MM-dd") + " 07:59:59.99");
            TimeSpan t3 = toRegDT.Subtract(frRegDT);
            TimeSpan t4 = toRegDT - frRegDT;
            int t5 = t3.Days;
            string date2 = t5.ToString();

            DateTime frdt = frRegDT;
            DataView dv = DtChange2.DefaultView;
             for (int i = 0; i < t5 + 1; i++)
            {
                dv.RowFilter = "Convert(WorkDT,'System.String') Like '" + frdt.ToString("yyyy-MM-dd") + "%'";
                if (dv.Count > 0)
                    this.DtTable.Columns.Add(new DataColumn(frdt.ToString("MM-dd"), typeof(string)));
                frdt = frdt.AddDays(1);
            }
            for (int i = 0; i < rtnDtTemp.Rows.Count; i++)
            {
                DataRow newrow = this.DtTable.NewRow();
                newrow["검사코드"] = rtnDtTemp.Rows[i]["InspCode"].ToString();
                newrow["검사항목"] = rtnDtTemp.Rows[i]["InspName"].ToString();
                newrow["검사구분"] = "CP";
                this.DtTable.Rows.Add(newrow);

                DataRow newrow1 = this.DtTable.NewRow();
                newrow1["검사코드"] = rtnDtTemp.Rows[i]["InspCode"].ToString();
                newrow1["검사항목"] = rtnDtTemp.Rows[i]["InspName"].ToString();
                newrow1["검사구분"] = "CPK";
                this.DtTable.Rows.Add(newrow1);
            }

            try
            {
   
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int rowid = -1;
                    for (int j = 0; j < DtTable.Rows.Count; j++)
                    {
                        if (dt.Rows[i]["InspCode"].ToString() == DtTable.Rows[j]["검사코드"].ToString()
                            && dt.Rows[i]["CpType"].ToString() == DtTable.Rows[j]["검사구분"].ToString())
                        {
                            rowid = j;
                            break;
                        }
                    }
                    if (rowid >= 0)
                    {
                        string colname = Convert.ToDateTime(dt.Rows[i]["WorkDT"]).ToString("MM-dd");
                        DtTable.Rows[rowid][colname] = Math.Round(Convert.ToDouble(dt.Rows[i]["VALUE"]), 3);
                        DtTable.Rows[rowid]["검사구분"] = dt.Rows[i]["CpType"].ToString();
                     }
                }
                double cp = 0;
                for (int j = 0; j < DtTable.Rows.Count; j++)
                {
                    cp = 0;
                    for (int i = 4; i < DtTable.Columns.Count; i++)
                    {
                        cp += Convert.ToDouble(DtTable.Rows[j][i]);
                    }
                    DtTable.Rows[j]["평균"] = Math.Round(cp / (DtTable.Columns.Count-4), 3);

                }


              
            }
            catch (Exception ex)
            {
            }

            this.grid2.DataSource = this.DtTable;
            for (int i = 0; i < this.DtTable.Columns.Count; i++)
            {
                this.grid2.DisplayLayout.Bands[0].Columns[i].CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                this.grid2.DisplayLayout.Bands[0].Columns[i].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                this.grid2.DisplayLayout.Bands[0].Columns[i].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                if (i > 2)
                {
                    this.grid2.DisplayLayout.Bands[0].Columns[i].Width = 80;
                    this.grid2.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
                else
                {
                    this.grid2.DisplayLayout.Bands[0].Columns[i].Width = (i == 1 ?160 : 70);
                    this.grid2.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }
            }
        }
        #endregion

        #region [검사항목 클릭]
        private void grid1_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            string CP = string.Empty;
            CP = SqlDBHelper.nvlString(cboCPType.Value);

            if (rtnDtTemp2.Rows.Count > 0)
            {
                /*체크된 검사항목 - Chart 비활성화*/
                if (this.grid1.ActiveRow.Cells["vcheck"].Value.ToString() == "1")
                {
                    this.grid1.ActiveRow.Cells["vcheck"].Value = false;

                    if (CP.Equals("CP"))
                    {
                        SeriesDict[this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                               + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                               + "CP"].Visible = false;
                    }
                    else if (CP.Equals("CPK"))
                    {
                        SeriesDict[this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                               + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                               + "CPK"].Visible = false;
                    }
                    else
                    {
                        SeriesDict[this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                                   + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                                   + "CP"].Visible = false;
                        SeriesDict[this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                                   + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                                   + "CPK"].Visible = false;
                    }

                    this.grid1.ActiveRow.Cells["vcheck"].Appearance.BackColor = Color.Black;
                    this.grid1.ActiveRow.Cells["InspName"].Appearance.BackColor = Color.Black;
                    for (int i = 0; i < this.DtTable.Rows.Count; i++)
                    {
                        if (this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                            + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                            + "CP" == DtTable.Rows[i]["검사코드"].ToString() + DtTable.Rows[i]["검사항목"].ToString() + DtTable.Rows[i]["검사구분"].ToString())
                        {
                            grid2.Rows[i].Hidden = true;
                        }
                        if (this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                            + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                            + "CPK" == DtTable.Rows[i]["검사코드"].ToString() + DtTable.Rows[i]["검사항목"].ToString() + DtTable.Rows[i]["검사구분"].ToString())
                        {
                            grid2.Rows[i].Hidden = true;
                        }
                    }

                }
                /*체크되지 않은 검사항목 - Chart활성화*/
                else
                {
                    this.grid1.ActiveRow.Cells["vcheck"].Value = true;
                    if (CP.Equals("CP"))
                    {
                        SeriesDict[this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                                                       + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                                                       + "CP"].Visible = true;
                    }
                    else if (CP.Equals("CPK"))
                    {
                        SeriesDict[this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                                                       + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                                                       + "CPK"].Visible = true;
                    }
                    else
                    {
                        SeriesDict[this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                                                       + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                                                       + "CP"].Visible = true;

                        SeriesDict[this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                                   + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                                   + "CPK"].Visible = true;
                    }

                    for (int i = 0; i < this.DtTable.Rows.Count; i++)
                    {
                        if (this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                            + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                            + "CP"
                            == DtTable.Rows[i]["검사코드"].ToString() + DtTable.Rows[i]["검사항목"].ToString() + DtTable.Rows[i]["검사구분"].ToString())
                        {
                            if (CP.Equals("CP") || CP.Equals(string.Empty))
                                grid2.Rows[i].Hidden = false;
                        }

                        if (this.grid1.ActiveRow.Cells["InspCode"].Value.ToString()
                            + this.grid1.ActiveRow.Cells["InspName"].Value.ToString()
                            + "CPK"
                            == DtTable.Rows[i]["검사코드"].ToString() + DtTable.Rows[i]["검사항목"].ToString() + DtTable.Rows[i]["검사구분"].ToString())
                        {
                            if (CP.Equals("CPK") || CP.Equals(string.Empty))
                                grid2.Rows[i].Hidden = false;
                        }

                    }
                }
            }
            /*데이터가 없을때*/
            else
            {

            }
        }
        #endregion

        private void CP_Chart_ChartDrawItem(object sender, Infragistics.UltraChart.Shared.Events.ChartDrawItemEventArgs e)
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

        private void grid1_ClickCell(object sender, ClickCellEventArgs e)
        {
          
        }

        private void btnSUM1_Click(object sender, EventArgs e)
        {

        }

        private void QM8100_Load(object sender, EventArgs e)
        {
            this.cboStartDate_H.Value = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01 00:00:00"));

        }

        private void cboCPType_ValueChanged(object sender, EventArgs e)
        {
            foreach (UltraGridRow ur in this.grid1.Rows)
            {
                ur.Cells["vcheck"].Value = false;
                SeriesDict[ur.Cells["InspCode"].Value.ToString() + ur.Cells["InspName"].Value.ToString() + "CP"].Visible = false;
                SeriesDict[ur.Cells["InspCode"].Value.ToString() + ur.Cells["InspName"].Value.ToString() + "CPK"].Visible = false;
            }
            foreach (UltraGridRow ur in this.grid2.Rows)
            {
                ur.Hidden = true;
            }

        }


    }

}
