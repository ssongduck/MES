#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : AP0800
//   Form Name    : 춸별 품목별 생산계획 추이
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
#endregion
namespace SAMMI.AP
{
    public partial class AP0800 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        public Dictionary<string, NumericSeries>  SeriesDict;

        private string PlantCode = string.Empty;

        public AP0800()
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

            this.InitChart();
            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtItemCode1, txtItemName1, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtItemCode2, txtItemName2, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtItemCode3, txtItemName3, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtItemCode4, txtItemName4, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtItemCode5, txtItemName5, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtItemCode6, txtItemName6, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtItemCode7, txtItemName7, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtItemCode8, txtItemName8, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtItemCode9, txtItemName9, "TBM0100", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtItemCode10, txtItemName10, "TBM0100", new object[] { cboPlantCode_H, "" });
            }
            else
            {
                btbManager.PopUpAdd(txtItemCode1, txtItemName1, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtItemCode2, txtItemName2, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtItemCode3, txtItemName3, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtItemCode4, txtItemName4, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtItemCode5, txtItemName5, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtItemCode6, txtItemName6, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtItemCode7, txtItemName7, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtItemCode8, txtItemName8, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtItemCode9, txtItemName9, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtItemCode10, txtItemName10, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
            }
        }

        #region AP0800_Load
        private void AP0800_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            //작업장 코드 추가 필요
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode",  "품목", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName",  "품목명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "jan", "1 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "feb", "2 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "mar", "3 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "apr", "4 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "may", "5 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "jun", "6 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "jul", "7 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "aug", "8 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "sep", "9 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "oct", "10 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "nov", "11 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "decs", "12 월", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);


            _GridUtil.SetInitUltraGridBind(grid1);

            //grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["ItemName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemName"].MergedCellStyle = MergedCellStyle.Always;

            #endregion

            #region 콤보박스
            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            //SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            #endregion

            this.ultraChart1.EmptyChartText = string.Empty;
        }
        #endregion AP0800_Load

        private ChartLayerAppearance lineLayer = new ChartLayerAppearance();

        public void InitChart()
        {

            ChartArea iChartArea = new ChartArea();

            // Create an X axis
            AxisItem xAxis                = new AxisItem();
            xAxis.DataType                = AxisDataType.String;
            xAxis.SetLabelAxisType        = SetLabelAxisType.ContinuousData;
            xAxis.Labels.ItemFormatString = "<ITEM_LABEL>";
            xAxis.Margin.Far.Value        = 2D;
            xAxis.ScrollScale.Visible     = true;
            xAxis.Margin.Near.Value       = 1.5D;
            xAxis.LineThickness = 1;
            xAxis.Extent                  = 70;

            // Create an Y axis
            AxisItem yAxis                = new AxisItem();
            yAxis.axisNumber              = AxisNumber.Y_Axis;
            yAxis.DataType                = AxisDataType.Numeric;
            yAxis.Labels.ItemFormatString = "<DATA_VALUE:0.##>";
            yAxis.TickmarkStyle           = AxisTickStyle.Smart;
            yAxis.ScrollScale.Visible     = true;
            yAxis.LineThickness = 1;
            yAxis.Extent                  = 50;

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


        #region 조회

        public override void DoInquire()
        {
            if (txtItemCode1.Text == string.Empty
                && txtItemCode2.Text == string.Empty 
                && txtItemCode3.Text == string.Empty
                && txtItemCode4.Text == string.Empty
                && txtItemCode5.Text == string.Empty
                && txtItemCode6.Text == string.Empty
                && txtItemCode7.Text == string.Empty
                && txtItemCode8.Text == string.Empty
                && txtItemCode9.Text == string.Empty
                && txtItemCode10.Text == string.Empty)
            {
                ShowDialog("품목을 하나 이상 입력하세요.", Windows.Forms.DialogForm.DialogType.OK);
                return;
            }

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[12];

            try
            {
              

                this.lineLayer.Series.Clear();

                base.DoInquire();
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                           // 사업장(공장)
                string SYyyy = Convert.ToDateTime(this.cboYear_H.Value).ToString("yyyy");                             // 년도
                string sItemCode1 = this.txtItemCode1.Text.Trim();                                                    // 품목코드
                string sItemCode2 = this.txtItemCode2.Text.Trim();                                                    // 품목코드
                string sItemCode3 = this.txtItemCode3.Text.Trim();                                                    // 품목코드
                string sItemCode4 = this.txtItemCode4.Text.Trim();                                                    // 품목코드
                string sItemCode5 = this.txtItemCode5.Text.Trim();                                                    // 품목코드
                string sItemCode6 = this.txtItemCode6.Text.Trim();                                                    // 품목코드
                string sItemCode7 = this.txtItemCode7.Text.Trim();                                                    // 품목코드
                string sItemCode8 = this.txtItemCode8.Text.Trim();                                                    // 품목코드
                string sItemCode9 = this.txtItemCode9.Text.Trim();                                                    // 품목코드
                string sItemCode10 = this.txtItemCode10.Text.Trim();                                                  // 품목코드


                param[0]  = helper.CreateParameter("@PLANTCODE",  sPlantCode,  SqlDbType.VarChar, ParameterDirection.Input);          
                param[1]  = helper.CreateParameter("@YEAR",       SYyyy,       SqlDbType.VarChar, ParameterDirection.Input);          
                param[2]  = helper.CreateParameter("@ITEMCODE1",  sItemCode1,  SqlDbType.VarChar, ParameterDirection.Input);          
                param[3]  = helper.CreateParameter("@ITEMCODE2",  sItemCode2,  SqlDbType.VarChar, ParameterDirection.Input);          
                param[4]  = helper.CreateParameter("@ITEMCODE3",  sItemCode3,  SqlDbType.VarChar, ParameterDirection.Input);          
                param[5]  = helper.CreateParameter("@ITEMCODE4",  sItemCode4,  SqlDbType.VarChar, ParameterDirection.Input);          
                param[6]  = helper.CreateParameter("@ITEMCODE5",  sItemCode5,  SqlDbType.VarChar, ParameterDirection.Input);          
                param[7]  = helper.CreateParameter("@ITEMCODE6",  sItemCode6,  SqlDbType.VarChar, ParameterDirection.Input);          
                param[8]  = helper.CreateParameter("@ITEMCODE7",  sItemCode7,  SqlDbType.VarChar, ParameterDirection.Input);          
                param[9]  = helper.CreateParameter("@ITEMCODE8",  sItemCode8,  SqlDbType.VarChar, ParameterDirection.Input);          
                param[10] = helper.CreateParameter("@ITEMCODE9",  sItemCode9,  SqlDbType.VarChar, ParameterDirection.Input);          
                param[11] = helper.CreateParameter("@ITEMCODE10", sItemCode10, SqlDbType.VarChar, ParameterDirection.Input);          


                rtnDtTemp = helper.FillTable("USP_AP0800_S1N_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();


                _Common.Grid_Column_Width(this.grid1); //grid 정리용  


                if (rtnDtTemp.Rows.Count > 0)
                {
                    ultraChart1.Series.Clear();
                    //----------------------------
                    DataTable dt = new DataTable();

                    dt.Columns.Add("ItemCode", typeof(System.String));
                    dt.Columns.Add("ItemName", typeof(System.String));
                    dt.Columns.Add("1월", typeof(System.Double));
                    dt.Columns.Add("2월", typeof(System.Double));
                    dt.Columns.Add("3월", typeof(System.Double));
                    dt.Columns.Add("4월", typeof(System.Double));
                    dt.Columns.Add("5월", typeof(System.Double));
                    dt.Columns.Add("6월", typeof(System.Double));
                    dt.Columns.Add("7월", typeof(System.Double));
                    dt.Columns.Add("8월", typeof(System.Double));
                    dt.Columns.Add("9월", typeof(System.Double));
                    dt.Columns.Add("10월", typeof(System.Double));
                    dt.Columns.Add("11월", typeof(System.Double));
                    dt.Columns.Add("12월", typeof(System.String));

                    for (int i = 0; i < rtnDtTemp.Rows.Count; i++)
                    {
                        dt.Rows.Add(new object[] {
                              rtnDtTemp.Rows[i]["ItemCode"].ToString(),
                              rtnDtTemp.Rows[i]["ItemName"].ToString(),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["jan"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["feb"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["mar"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["apr"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["may"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["jun"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["jul"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["aug"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["sep"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["oct"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["nov"]),
                              Convert.ToDouble(rtnDtTemp.Rows[i]["decs"])
                          });
                    }


                    //********************** 시리즈 선언 **************************************************
                    SeriesDict = new Dictionary<string, NumericSeries>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SeriesDict.Add(dt.Rows[i]["ItemCode"].ToString() + dt.Rows[i]["ItemName"].ToString()
                                      , new NumericSeries());
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        this.lineLayer.Series.Add(SeriesDict[dt.Rows[i]["ItemCode"].ToString() + dt.Rows[i]["ItemName"].ToString()]);
                    }

                    this.ultraChart1.CompositeChart.ChartLayers.Add(this.lineLayer);

                    //********************** 시리즈 값 입력 **************************************************

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 2; j < dt.Columns.Count; j++)
                        {
                            SeriesDict[dt.Rows[i]["ItemCode"].ToString() + dt.Rows[i]["ItemName"].ToString()].Points.Add
                            (new NumericDataPoint(Convert.ToDouble((dt.Rows[i][j].ToString())), Convert.ToString(j -1)+"월" , false));
                            SeriesDict[dt.Rows[i]["ItemCode"].ToString() + dt.Rows[i]["ItemName"].ToString()].Label 
                                = dt.Rows[i]["ItemName"].ToString();
                        }
                    }
                    //********************** 범례  **************************************************
                    CompositeLegend myLegend = new CompositeLegend();
                    myLegend.ChartLayers.Add(this.lineLayer);
                    myLegend.Bounds = new Rectangle(3, 79, 97, 20);
                    myLegend.BoundsMeasureType = MeasureType.Percentage;
                    //myLegend.ChartComponent.Series.Add(); 
                    myLegend.LabelStyle.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    myLegend.PE.ElementType = PaintElementType.Gradient;
                    myLegend.PE.FillGradientStyle = Infragistics.UltraChart.Shared.Styles.GradientStyle.ForwardDiagonal;
                    myLegend.PE.Fill = Color.Azure;
                    myLegend.PE.FillStopColor = Color.Transparent;
                    myLegend.Border.CornerRadius = 10;
                    myLegend.Border.Thickness = 0;
                    this.ultraChart1.CompositeChart.Legends.Add(myLegend);
                    
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
            }
        }
        #endregion 조회
          private void setChart()
        {
           
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
