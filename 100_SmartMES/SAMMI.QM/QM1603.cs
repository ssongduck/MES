using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using SAMMI.Common;
using SAMMI.Windows.Forms;
using System.Diagnostics;
using Infragistics.UltraChart.Resources.Appearance;
using System.Linq;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid;
using DevExpress.Data;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Grid;

namespace SAMMI.QM
{
    /// <summary>
    /// QM1603 class
    /// </summary>
    public partial class QM1603 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _RtnDt = new DataTable();

        /// <summary>
        /// 
        /// </summary>
        string[] _EmptyArray = { "v_txt" };

        /// <summary>
        /// Plant code
        /// </summary>
        private string _sPlantCode = string.Empty;

        /// <summary>
        /// Grid util
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        BizTextBoxManagerEX btbManager;

        /// <summary>
        /// Measure datatable
        /// </summary>
        DataTable _MeasureDt = null;

        /// <summary>
        /// Spec datatable
        /// </summary>
        DataTable _SpecDt = null;

        #endregion

        #region Constructor

        /// <summary>
        /// QM1603 constructor
        /// </summary>
        public QM1603()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// QM1603 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QM1603_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();
        }

        /// <summary>
        /// Measure grid view focused row changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMeasure_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridViewMeasure.GetFocusedDataRow();
            DataRow historyDr = null;

            if (dr != null)
            {
                if (_MeasureDt != null && _MeasureDt.Rows.Count > 0)
                {
                    historyDr = _MeasureDt.AsEnumerable().Where(t => t.Field<string>("LOT_NO") == dr["LOT_NO"].ToString()).FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Measure gird view rowclick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMeasure_RowClick(object sender, RowClickEventArgs e)
        {
            DataRow dr = gridViewMeasure.GetFocusedDataRow();

            if (dr != null)
            {
            }
        }

        /// <summary>
        /// Measure grid view popup menu showing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMeasure_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.Menu == null) return;

                DXMenuItem dXMenuItem = new DevExpress.Utils.Menu.DXMenuItem("엑셀 내보내기", new EventHandler(this.exportMeasureMenu_Click));
                e.Menu.Items.Add(dXMenuItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Measure grid view row style event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMeasure_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (e.RowHandle >= 0)
            {
                if (gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE01"]) == "NG" ||
                    gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE02"]) == "NG" ||
                    gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE03"]) == "NG" ||
                    gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE04"]) == "NG" ||
                    gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE05"]) == "NG" ||
                    gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE06"]) == "NG" ||
                    gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE07"]) == "NG" ||
                    gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE08"]) == "NG" ||
                    gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE09"]) == "NG" ||
                    gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE10"]) == "NG" ||
                    gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["JUDGE11"]) == "NG")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 218, 218);
                }
            }
        }

        /// <summary>
        /// Measure code combobox selected value changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMeasureCode_SelectedValueChanged(object sender, EventArgs e)
        {
            ChangeChartTitle();
            BindLineChart();
        }

        /// <summary>
        /// Item code selected value changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbItemCode_SelectedValueChanged(object sender, EventArgs e)
        {
            _SpecDt = GetItemSpecData();
            InitializeGridControl();
        }

        /// <summary>
        /// Chart line custom draw series event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartLine_CustomDrawSeries(object sender, CustomDrawSeriesEventArgs e)
        {
            LineDrawOptions drawOptions = e.SeriesDrawOptions as LineDrawOptions;
            if (drawOptions == null)
                return;

            drawOptions.Marker.Color = Color.FromArgb(255, 249, 134);
            drawOptions.Marker.BorderColor = Color.FromArgb(255, 249, 134);
        }

        /// <summary>
        /// Chart line mouseclick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartLine_MouseClick(object sender, MouseEventArgs e)
        {
            ChartHitInfo hi = chartLine.CalcHitInfo(e.X, e.Y);

            SeriesPoint point = hi.SeriesPoint;

            if (point != null)
            {
                string argument = point.Argument.ToString();

                if (!string.IsNullOrEmpty(argument))
                {
                    int row = gridViewMeasure.LocateByValue("LASTEVENT_TIME", DateTime.Parse(argument));
                    gridViewMeasure.ClearSelection();
                    gridViewMeasure.FocusedRowHandle = row;
                    gridViewMeasure.SelectRow(row);
                }
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Initialize control
        /// </summary>
        private void InitializeControl()
        {
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this._sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this._sPlantCode.Equals("SK"))
            {
                this._sPlantCode = "SK1";
            }
            else if (this._sPlantCode.Equals("EC"))
            {
                this._sPlantCode = "SK2";
            }
            else
            {
                if (cboPlantCode_H.Value == null)
                {
                    cboPlantCode_H.Value = "ALL";
                    this._sPlantCode = "ALL";
                }
            }

            btbManager = new BizTextBoxManagerEX();

            calFromDt.Value = DateTime.Now;
            calToDt.Value = DateTime.Now;
            cmbItemCode.SelectedIndex = 0;
            cmbMeasureCode.SelectedIndex = 0;

            ChartTitle chartTitle1 = new ChartTitle();
            chartTitle1.Text = string.Empty;
            chartTitle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartLine.Titles.Add(chartTitle1);

            _SpecDt = GetItemSpecData();
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            if (_SpecDt != null && _SpecDt.Rows.Count > 0)
            {
                _MeasureDt = null;

                chartLine.Series.Clear();

                if (cmbMeasureCode.Properties.Items != null && cmbMeasureCode.Properties.Items.Count > 0)
                {
                    cmbMeasureCode.Properties.Items.Clear();
                }

                if (gridControlMeasure.DataSource != null)
                {
                    gridControlMeasure.DataSource = null;
                }

                if (gridViewMeasure.Columns != null && gridViewMeasure.Columns.Count > 0)
                {
                    gridViewMeasure.Columns.Clear();
                }

                gridViewMeasure.Columns.AddField("LASTEVENT_TIME");
                gridViewMeasure.Columns["LASTEVENT_TIME"].VisibleIndex = 0;
                gridViewMeasure.Columns["LASTEVENT_TIME"].Caption = "측정일시";
                gridViewMeasure.Columns["LASTEVENT_TIME"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gridViewMeasure.Columns["LASTEVENT_TIME"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gridViewMeasure.Columns["LASTEVENT_TIME"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridViewMeasure.Columns["LASTEVENT_TIME"].DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";

                gridViewMeasure.Columns.AddField("LOT_NO");
                gridViewMeasure.Columns["LOT_NO"].VisibleIndex = 1;
                gridViewMeasure.Columns["LOT_NO"].Caption = "바코드번호";
                gridViewMeasure.Columns["LOT_NO"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gridViewMeasure.Columns["LOT_NO"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                gridViewMeasure.Columns.AddField("ITEM_CODE");
                gridViewMeasure.Columns["ITEM_CODE"].VisibleIndex = 2;
                gridViewMeasure.Columns["ITEM_CODE"].Caption = "품목코드";
                gridViewMeasure.Columns["ITEM_CODE"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gridViewMeasure.Columns["ITEM_CODE"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                gridViewMeasure.Columns.AddField("JUDGE11");
                gridViewMeasure.Columns["JUDGE11"].VisibleIndex = 3;
                gridViewMeasure.Columns["JUDGE11"].Caption = "최종판정";
                gridViewMeasure.Columns["JUDGE11"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gridViewMeasure.Columns["JUDGE11"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                for (int i = 1; i <= _SpecDt.Rows.Count; i++)
                {
                    cmbMeasureCode.Properties.Items.Add(_SpecDt.Rows[i - 1]["INSP_NAME"].ToString());

                    gridViewMeasure.Columns.AddField(string.Format("DATA{0}", i.ToString("00")));
                    gridViewMeasure.Columns[string.Format("DATA{0}", i.ToString("00"))].VisibleIndex = (i * 2) + 3;
                    gridViewMeasure.Columns[string.Format("DATA{0}", i.ToString("00"))].Caption = _SpecDt.Rows[i - 1]["INSP_NAME"].ToString();
                    gridViewMeasure.Columns[string.Format("DATA{0}", i.ToString("00"))].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridViewMeasure.Columns[string.Format("DATA{0}", i.ToString("00"))].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    gridViewMeasure.Columns[string.Format("DATA{0}", i.ToString("00"))].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridViewMeasure.Columns[string.Format("DATA{0}", i.ToString("00"))].DisplayFormat.FormatString = "#,###.000";

                    gridViewMeasure.Columns.AddField(string.Format("JUDGE{0}", i.ToString("00")));
                    gridViewMeasure.Columns[string.Format("JUDGE{0}", i.ToString("00"))].VisibleIndex = (i * 2) + 4;
                    gridViewMeasure.Columns[string.Format("JUDGE{0}", i.ToString("00"))].Caption = string.Format("{0}판정", _SpecDt.Rows[i - 1]["INSP_NAME"].ToString());
                    gridViewMeasure.Columns[string.Format("JUDGE{0}", i.ToString("00"))].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridViewMeasure.Columns[string.Format("JUDGE{0}", i.ToString("00"))].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }

                cmbMeasureCode.SelectedIndex = 0;
                gridViewMeasure.BestFitColumns();

                ChangeChartTitle();
            }
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.Disposed += new EventHandler(QM1603_Disposed);

            gridViewMeasure.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewMeasure_FocusedRowChanged);
            gridViewMeasure.RowClick += new RowClickEventHandler(gridViewMeasure_RowClick);
            gridViewMeasure.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewMeasure_PopupMenuShowing);
            gridViewMeasure.RowStyle += new RowStyleEventHandler(gridViewMeasure_RowStyle);

            cmbMeasureCode.SelectedValueChanged += new EventHandler(cmbMeasureCode_SelectedValueChanged);
            cmbItemCode.SelectedValueChanged += new EventHandler(cmbItemCode_SelectedValueChanged);

            chartLine.CustomDrawSeries += new CustomDrawSeriesEventHandler(chartLine_CustomDrawSeries);
            chartLine.MouseClick += new MouseEventHandler(chartLine_MouseClick);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(QM1603_Disposed);

            gridViewMeasure.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewMeasure_FocusedRowChanged);
            gridViewMeasure.RowClick -= new RowClickEventHandler(gridViewMeasure_RowClick);
            gridViewMeasure.PopupMenuShowing -= new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewMeasure_PopupMenuShowing);
            gridViewMeasure.RowStyle -= new RowStyleEventHandler(gridViewMeasure_RowStyle);

            cmbMeasureCode.SelectedValueChanged -= new EventHandler(cmbMeasureCode_SelectedValueChanged);
            cmbItemCode.SelectedValueChanged -= new EventHandler(cmbItemCode_SelectedValueChanged);

            chartLine.CustomDrawSeries -= new CustomDrawSeriesEventHandler(chartLine_CustomDrawSeries);
            chartLine.MouseClick -= new MouseEventHandler(chartLine_MouseClick);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            BindMeasureDataList();
            BindLineChart();
        }

        /// <summary>
        /// Do new
        /// </summary>
        public override void DoNew()
        {
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
        }

        /// <summary>
        /// Do save
        /// </summary>
        public override void DoSave()
        {
        }

        /// <summary>
        /// Clear control
        /// </summary>
        /// <param name="control"></param>
        private void ClearControl(System.Windows.Forms.Control control)
        {
            if (control == null)
            {
                return;
            }

            foreach (System.Windows.Forms.Control ctrl in control.Controls)
            {
                ClearControl(ctrl);

                if (ctrl.GetType().Name == "TextBox")
                {
                    TextBox textBox = (TextBox)ctrl;

                    foreach (string sVal in _EmptyArray)
                    {
                        if (textBox.Name.StartsWith(sVal))
                        {
                            textBox.Text = string.Empty;
                        }
                    }
                }

                if (ctrl.GetType().Name == "MaskedTextBox")
                {
                    MaskedTextBox maskedTextBox = (MaskedTextBox)ctrl;

                    foreach (string sVal in _EmptyArray)
                    {
                        if (maskedTextBox.Name.StartsWith(sVal))
                        {
                            maskedTextBox.Text = string.Empty;
                        }
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Clear all control
        /// </summary>
        private void ClearAllControl()
        {
            ClearControl(this);
        }

        /// <summary>
        /// Bind measure data list
        /// </summary>
        private void BindMeasureDataList()
        {
            if (gridViewMeasure.Columns["LASTEVENT_TIME"].Summary.Count > 0) gridViewMeasure.Columns["LASTEVENT_TIME"].Summary.Clear();

            _MeasureDt = GetMeasureyData();

            if (_MeasureDt != null && _MeasureDt.Rows.Count > 0)
            {
                gridControlMeasure.DataSource = _MeasureDt;

                gridViewMeasure.Columns["LASTEVENT_TIME"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Count, "LASTEVENT_TIME", "전체 행:{0:#,###}건"));

                gridViewMeasure.BestFitColumns();
            }
            else
            {
                gridControlMeasure.DataSource = null;
            }
        }

        /// <summary>
        /// Get measure data
        /// </summary>
        /// <returns></returns>
        private DataTable GetMeasureyData()
        {
            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sPlantCode = string.IsNullOrEmpty(sPlantCode) ? "%" : sPlantCode;

            DataTable rtnDt = new DataTable();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (connectionString.Contains("192.168.50.2"))
            {
                connectionString = connectionString.Replace("192.168.50.2", "192.168.60.2");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_sqlGetMeasureData, connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "PLANT_CODE"), "SK2"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "WORKCENTER_CODE"), "4D54"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "ITEM_CODE"), cmbItemCode.Text));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "FROM_DT"), string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value)));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "TO_DT"), string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1))));

                    command.Connection.Open();

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);

                    sqlDataAdapter.Fill(rtnDt);
                    rtnDt.TableName = "MEASURE_TABLE";

                    return rtnDt;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        /// <summary>
        /// Get item spec data
        /// </summary>
        /// <returns></returns>
        private DataTable GetItemSpecData()
        {
            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sPlantCode = string.IsNullOrEmpty(sPlantCode) ? "%" : sPlantCode;

            DataTable rtnDt = new DataTable();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (connectionString.Contains("192.168.50.2"))
            {
                connectionString = connectionString.Replace("192.168.50.2", "192.168.60.2");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_sqlGetItemSpecData, connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "PLANT_CODE"), "SK2"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "WORKCENTER_CODE"), "4D54"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "ITEM_CODE"), cmbItemCode.Text));

                    command.Connection.Open();

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);

                    sqlDataAdapter.Fill(rtnDt);
                    rtnDt.TableName = "ITEM_SPEC_TABLE";

                    return rtnDt;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        /// <summary>
        /// Bind line chart
        /// </summary>
        private void BindLineChart()
        {
            string measureCode = cmbMeasureCode.Text;

            chartLine.Series.Clear();

            if (_MeasureDt != null && _MeasureDt.Rows.Count > 0)
            {
                decimal ucl = 0;
                decimal lcl = 0;

                if (_SpecDt != null && _SpecDt.Rows.Count > 0)
                {
                    ucl = _SpecDt.AsEnumerable().Where(t => t.Field<string>("INSP_NAME") == measureCode).Select(t => t.Field<decimal>("UCL")).FirstOrDefault();
                    lcl = _SpecDt.AsEnumerable().Where(t => t.Field<string>("INSP_NAME") == measureCode).Select(t => t.Field<decimal>("LCL")).FirstOrDefault();
                }

                string colName = gridViewMeasure.Columns.Where(t => t.Caption == measureCode).Select(t => t.FieldName).FirstOrDefault();

                decimal minVal = _MeasureDt.AsEnumerable().Select(t => t.Field<decimal>(colName)).Min();
                decimal maxVal = _MeasureDt.AsEnumerable().Select(t => t.Field<decimal>(colName)).Max();

                if (minVal > lcl)
                {
                    minVal = lcl;
                }

                if (maxVal < ucl)
                {
                    maxVal = ucl;
                }

                minVal = minVal - decimal.Parse("0.001");
                maxVal = maxVal + decimal.Parse("0.001");

                if (!string.IsNullOrEmpty(colName))
                {
                    Series series = new Series(string.Format("{0} 추세", measureCode), ViewType.Line);

                    series.Points.BeginUpdate();

                    foreach (DataRow dr in _MeasureDt.Rows)
                    {
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dr["LASTEVENT_TIME"].ToString()), decimal.Parse(dr[colName].ToString())));
                    }

                    ChartOption(series, lcl, ucl, minVal, maxVal, measureCode);

                    series.Points.EndUpdate();
                }
            }
            else
            {
                chartLine.Series.Clear();
            }
        }

        /// <summary>
        /// Change chart title
        /// </summary>
        private void ChangeChartTitle()
        {
            chartLine.Titles[0].Text = string.Format("[ {0} 추세 ]", cmbMeasureCode.Text);
        }

        /// <summary>
        /// Chart option
        /// </summary>
        /// <param name="series"></param>
        /// <param name="lcl"></param>
        /// <param name="ucl"></param>
        /// <param name="minVal"></param>
        /// <param name="maxVal"></param>
        /// <param name="measureCode"></param>
        private void ChartOption(Series series, decimal lcl, decimal ucl, decimal minVal, decimal maxVal, string measureCode)
        {
            chartLine.Series.Clear();

            series.ValueScaleType = ScaleType.Numerical;
            ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
            ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Solid;

            chartLine.Series.Add(series);

            ConstantLine uclLine = new ConstantLine();
            ConstantLine lclLine = new ConstantLine();

            uclLine.Name = string.Format("{0} : {1}", "UCL", ucl.ToString());
            lclLine.Name = string.Format("{0} : {1}", "LCL", lcl.ToString());
            uclLine.AxisValueSerializable = ucl.ToString();
            lclLine.AxisValueSerializable = lcl.ToString();

            XYDiagram diagram = chartLine.Diagram as XYDiagram;
            diagram.AxisX.DateTimeScaleOptions.ScaleMode = DevExpress.XtraCharts.ScaleMode.Continuous;
            diagram.AxisX.DateTimeScaleOptions.AutoGrid = true;
            diagram.AxisX.Label.TextPattern = "{A:yyyy-MM-dd HH:mm}";
            diagram.AxisX.VisibleInPanesSerializable = "-1";
            diagram.AxisX.Label.Staggered = true;

            diagram.AxisY.Label.TextPattern = "{V:F3}";
            diagram.AxisY.WholeRange.Auto = false;
            diagram.AxisY.WholeRange.SetMinMaxValues(minVal, maxVal);

            if (diagram.AxisY.ConstantLines != null)
            {
                diagram.AxisY.ConstantLines.Clear();
            }

            diagram.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] { uclLine, lclLine });
            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.EnableAxisYScrolling = true;
            diagram.EnableAxisYZooming = true;

            chartLine.CrosshairOptions.ShowValueLabels = true;
            chartLine.CrosshairOptions.ShowValueLine = true;
            chartLine.CrosshairOptions.GroupHeaderPattern = "<b>{A:yyyy-MM-dd HH:mm:ss}</b>";
            series.CrosshairLabelPattern = "<b>" + measureCode + " : {V:F3}</b>";

            chartLine.CrosshairOptions.ShowValueLabels = true;
            chartLine.CrosshairOptions.ShowValueLine = true;
            chartLine.CrosshairOptions.ArgumentLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartLine.CrosshairOptions.ValueLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        /// <summary>
        /// Convert decimal to string
        /// </summary>
        /// <param name="decVal"></param>
        /// <returns></returns>
        private string ConvertDecimalToString(decimal decVal)
        {
            return string.Format("{0}", decVal.ToString("F2"));
        }

        /// <summary>
        /// Export measure menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportMeasureMenu_Click(object sender, EventArgs e)
        {
            ExportExcel(gridViewMeasure);
        }

        /// <summary>
        /// Export excel
        /// </summary>
        /// <param name="gridView"></param>
        private void ExportExcel(GridView gridView)
        {
            try
            {
                gridView.OptionsPrint.AutoWidth = false;
                gridView.OptionsPrint.PrintHeader = true;
                gridView.OptionsPrint.PrintSelectedRowsOnly = false;
                gridView.OptionsPrint.ExpandAllDetails = true;
                gridView.OptionsPrint.ExpandAllGroups = true;
                gridView.OptionsPrint.PrintDetails = true;
                gridView.OptionsPrint.UsePrintStyles = true;

                SaveFileDialog dialog = new SaveFileDialog();

                dialog.Filter = "Excel File(*.xlsx)|*.xlsx|All Files(*.*)|*.*";
                dialog.Title = "엑셀로 저장";

                if ((dialog.ShowDialog()) == DialogResult.OK)
                {
                    string filePath = dialog.FileName.ToString();
                    gridView.ExportToXlsx(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #region SQL

        /// <summary>
        /// Get measure data (TDS3004)
        /// </summary>
        public const string _sqlGetMeasureData =
            @"SELECT A.LOT_NO								                                    AS LOT_NO
	                ,A.ITEM_CODE							                                    AS ITEM_CODE
	                ,A.JUDGE01							                                        AS JUDGE01
	                ,A.JUDGE02							                                        AS JUDGE02
	                ,A.JUDGE03							                                        AS JUDGE03
	                ,A.JUDGE04							                                        AS JUDGE04
	                ,A.JUDGE05							                                        AS JUDGE05
	                ,A.JUDGE06							                                        AS JUDGE06
	                ,A.JUDGE07							                                        AS JUDGE07
	                ,A.JUDGE08							                                        AS JUDGE08
	                ,A.JUDGE09							                                        AS JUDGE09
	                ,A.JUDGE10							                                        AS JUDGE10
	                ,A.JUDGE11							                                        AS JUDGE11
	                ,A.DATA01								                                    AS DATA01
	                ,A.DATA02								                                    AS DATA02
	                ,A.DATA03								                                    AS DATA03
	                ,A.DATA04								                                    AS DATA04
	                ,A.DATA05								                                    AS DATA05
	                ,A.DATA06								                                    AS DATA06
	                ,A.DATA07								                                    AS DATA07
	                ,A.DATA08								                                    AS DATA08
	                ,A.DATA09								                                    AS DATA09
	                ,A.DATA10								                                    AS DATA10
	                ,A.REWORK								                                    AS REWORK
	                ,A.LASTEVENT_TIME						                                    AS LASTEVENT_TIME
                FROM TDS3004 A WITH(NOLOCK)
               WHERE 1 = 1
                 AND A.PLANT_CODE		LIKE @PLANT_CODE
                 AND A.WORKCENTER_CODE  = @WORKCENTER_CODE
                 AND A.ITEM_CODE        = @ITEM_CODE
                 AND A.LASTEVENT_TIME   BETWEEN @FROM_DT AND @TO_DT";

        /// <summary>
        /// Get item spec data
        /// </summary>
        public const string _sqlGetItemSpecData =
            @"SELECT A.ITEM_CODE										                        AS ITEM_CODE
                    ,B.INSP_NAME										                        AS INSP_NAME
                    ,B.LCL											                            AS LCL
                    ,B.UCL											                            AS UCL
                    ,B.SORT                                                                     AS SORT
                FROM TBM0620 A WITH(NOLOCK)
                     LEFT JOIN TBM0630 B WITH(NOLOCK)
                     ON  A.INSP_CLASS_CODE	= B.INSP_CLASS_CODE
               WHERE 1 = 1
                 AND A.PLANT_CODE		= @PLANT_CODE
                 AND A.WORKCENTER_CODE  = @WORKCENTER_CODE
                 AND A.ITEM_CODE		= @ITEM_CODE";

        #endregion
    }
}
