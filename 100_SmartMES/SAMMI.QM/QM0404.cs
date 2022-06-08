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

namespace SAMMI.QM
{
    /// <summary>
    /// QM0404 class
    /// </summary>
    public partial class QM0404 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        #endregion

        #region Constructor

        /// <summary>
        /// QM0404 constructor
        /// </summary>
        public QM0404()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();

            txtItemCode.Focus();
        }

        #endregion

        #region Event

        /// <summary>
        /// PP0341 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QM0403_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();
        }

        /// <summary>
        /// Grid item initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridItemList_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["ITEM_CODE"].Value.ToString() == "합 계")
            {
                e.Row.Appearance.BackColor = Color.LightBlue;
            }
        }

        private void gridDayList_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["REC_DATE"].Value.ToString() == "합 계")
            {
                e.Row.Appearance.BackColor = Color.LightBlue;
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

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOpCode, "", "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
                btbManager.PopUpAdd(txtItemCode, txtItemCodeName, "TBM0101", new object[] { this.cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOpCode, "", "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
                btbManager.PopUpAdd(txtItemCode, txtItemCodeName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
                , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }

            ChartTitle chartTitle1 = new ChartTitle();
            chartTitle1.Text = "[ 품목별 WORST TOP 10 ]";
            chartTitle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartPieItem.Titles.Add(chartTitle1);

            ChartTitle chartTitle2 = new ChartTitle();
            chartTitle2.Text = "[ 품목별 WORST TOP 10 ]";
            chartTitle2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartStackItem.Titles.Add(chartTitle2);

            ChartTitle chartTitle3 = new ChartTitle();
            chartTitle3.Text = "[ 일별 WORST TOP 10 ]";
            chartTitle3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartPieDay.Titles.Add(chartTitle3);
            
            ChartTitle chartTitle4 = new ChartTitle();
            chartTitle4.Text = "[ 일별 WORST TOP 10 ]";
            chartTitle4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartStackDay.Titles.Add(chartTitle4);

            calFromDt.Value = DateTime.Now;
            calToDt.Value = DateTime.Now;
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            _UltraGridUtil.InitializeGrid(this.gridItemList, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "ITEM_CODE", "품번", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "ITEM_NAME", "품명", false, GridColDataType_emu.VarChar, 500, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "TOTAL_QTY", "총수량", false, GridColDataType_emu.Integer, 150, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "PROD_QTY", "양품수량", false, GridColDataType_emu.Integer, 150, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "ERROR_QTY", "불량수량", false, GridColDataType_emu.Integer, 150, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "PROD_RATE", "양품률(%)", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "ERROR_RATE", "불량률(%)", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);

            _UltraGridUtil.InitializeGrid(this.gridDayList, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(gridDayList, "REC_DATE", "일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridDayList, "ITEM_CODE", "품번", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridDayList, "ITEM_NAME", "품명", false, GridColDataType_emu.VarChar, 500, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridDayList, "TOTAL_QTY", "총수량", false, GridColDataType_emu.Integer, 150, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridDayList, "PROD_QTY", "양품수량", false, GridColDataType_emu.Integer, 150, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridDayList, "ERROR_QTY", "불량수량", false, GridColDataType_emu.Integer, 150, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridDayList, "PROD_RATE", "양품률(%)", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridDayList, "ERROR_RATE", "불량률(%)", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.Disposed += new EventHandler(QM0403_Disposed);

            gridItemList.InitializeRow += new InitializeRowEventHandler(gridItemList_InitializeRow);
            gridDayList.InitializeRow += new InitializeRowEventHandler(gridDayList_InitializeRow);
        }




        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(QM0403_Disposed);

            gridItemList.InitializeRow -= new InitializeRowEventHandler(gridItemList_InitializeRow);
            gridDayList.InitializeRow -= new InitializeRowEventHandler(gridDayList_InitializeRow);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            chartPieItem.Series.Clear();
            chartPieDay.Series.Clear();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters1 = new SqlParameter[7];
            SqlParameter[] sqlParameters2 = new SqlParameter[7];

            ClearAllControl();

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                base.DoInquire();

                sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_OPCODE", txtOpCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[5] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[6] = sqlDBHelper.CreateParameter("@AS_FLAG", "I", SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters2[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[1] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[2] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[3] = sqlDBHelper.CreateParameter("@AS_OPCODE", txtOpCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[4] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[5] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[6] = sqlDBHelper.CreateParameter("@AS_FLAG", "D", SqlDbType.VarChar, ParameterDirection.Input);

                dt1 = sqlDBHelper.FillTable("SP_GET_MANU_ITEM_LIST", CommandType.StoredProcedure, sqlParameters1);
                dt2 = sqlDBHelper.FillTable("SP_GET_MANU_ITEM_LIST", CommandType.StoredProcedure, sqlParameters2);

                gridItemList.DataSource = dt1;
                gridItemList.DataBind();

                gridDayList.DataSource = dt2;
                gridDayList.DataBind();

                BindItemPieChart(dt1);
                BindItemStackChart(dt1);
                BindDayPieChart(dt2);
                BindDayStackChart(dt2);
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
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
        /// Bind item pie chart
        /// </summary>
        /// <param name="dt"></param>
        private void BindItemPieChart(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 1)
            {
                dt.DefaultView.Sort = "ERROR_RATE DESC";
                dt = dt.DefaultView.ToTable();

                chartPieItem.Series.Clear();
                Series series = new Series("", ViewType.Pie);
                int j = 10;

                if (dt.Rows.Count < 10)
                {
                    j = dt.Rows.Count;
                }

                for (int i = 0; i < j; i++)
                {
                    if (dt.Rows[i]["ITEM_CODE"].ToString() == "합 계")
                    {
                        continue;
                    }

                    series.Points.Add(new SeriesPoint(dt.Rows[i]["ITEM_CODE"].ToString(), float.Parse(dt.Rows[i]["ERROR_RATE"].ToString())));
                }

                series.ArgumentScaleType = ScaleType.Auto;
                series.ValueScaleType = ScaleType.Numerical;

                series.Label.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

                chartPieItem.Series.Add(series);
                series.Label.TextPattern = "{A}: {VP:P0}";

                ((PieSeriesLabel)series.Label).Position = PieSeriesLabelPosition.TwoColumns;
                ((PieSeriesLabel)series.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;

                PieSeriesView pieSeriesView = (PieSeriesView)series.View;

                pieSeriesView.Titles.Add(new SeriesTitle());
                pieSeriesView.Titles[0].Text = series.Name;

                pieSeriesView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Value_1, DataFilterCondition.GreaterThanOrEqual, 9));
                pieSeriesView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.NotEqual, "Others"));
                pieSeriesView.ExplodeMode = PieExplodeMode.UseFilters;
                pieSeriesView.RuntimeExploding = true;

                chartPieItem.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            }
            else
            {
                chartPieItem.Series.Clear();
            }
        }

        /// <summary>
        /// Bind item stack chart
        /// </summary>
        /// <param name="dt"></param>
        private void BindItemStackChart(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 1)
            {
                DataTable tempDt = dt.Clone();

                dt.DefaultView.Sort = "ERROR_RATE DESC";
                dt = dt.DefaultView.ToTable();

                chartStackItem.Series.Clear();
                Series series1 = new Series("불량률(%)", ViewType.FullStackedBar);
                Series series2 = new Series("양품률(%)", ViewType.FullStackedBar);
                int j = 10;

                if (dt.Rows.Count < 10)
                {
                    j = dt.Rows.Count;
                }

                for (int i = 0; i < j; i++)
                {
                    if (dt.Rows[i]["ITEM_CODE"].ToString() == "합 계")
                    {
                        continue;
                    }

                    DataRow dr = dt.Rows[i];
                    tempDt.ImportRow(dr);
                }

                tempDt.AcceptChanges();

                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    foreach (DataRow dr in tempDt.Rows)
                    {
                        series1.Points.Add(new SeriesPoint(dr["ITEM_CODE"].ToString(), Decimal.Parse(dr["ERROR_RATE"].ToString())));
                        series2.Points.Add(new SeriesPoint(dr["ITEM_CODE"].ToString(), Decimal.Parse(dr["PROD_RATE"].ToString())));
                    }

                    series1.ArgumentScaleType = ScaleType.Auto;
                    series1.ValueScaleType = ScaleType.Numerical;
                    series1.Label.TextPattern = "{VP:P0}";

                    series2.ArgumentScaleType = ScaleType.Auto;
                    series2.ValueScaleType = ScaleType.Numerical;
                    series2.Label.TextPattern = "{VP:P0}";

                    chartStackItem.Series.AddRange(new Series[] { series1, series2 });
                    chartStackItem.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                    ((XYDiagram)chartStackItem.Diagram).AxisY.Label.TextPattern = "{VP:P0}";
                    ((XYDiagram)chartStackItem.Diagram).Rotated = true;
                    ((FullStackedBarSeriesView)series1.View).BarWidth = 0.5;
                    ((FullStackedBarSeriesView)series2.View).BarWidth = 0.5;
                    ((XYDiagram)chartStackItem.Diagram).EnableAxisXZooming = true;
                }
            }
            else
            {
                chartStackItem.Series.Clear();
            }
        }

        /// <summary>
        /// Bind day pie chart
        /// </summary>
        /// <param name="dt"></param>
        private void BindDayPieChart(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 1)
            {
                dt.DefaultView.Sort = "ERROR_RATE DESC";
                dt = dt.DefaultView.ToTable();

                chartPieDay.Series.Clear();
                Series series = new Series("", ViewType.Pie);
                int j = 10;

                if (dt.Rows.Count < 10)
                {
                    j = dt.Rows.Count;
                }

                for (int i = 0; i < j; i++)
                {
                    if (dt.Rows[i]["REC_DATE"].ToString() == "합 계")
                    {
                        continue;
                    }

                    series.Points.Add(new SeriesPoint(dt.Rows[i]["ITEM_CODE"].ToString(), float.Parse(dt.Rows[i]["ERROR_RATE"].ToString())));
                }

                series.ArgumentScaleType = ScaleType.Auto;
                series.ValueScaleType = ScaleType.Numerical;

                series.Label.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

                chartPieDay.Series.Add(series);
                series.Label.TextPattern = "{A}: {VP:P0}";

                ((PieSeriesLabel)series.Label).Position = PieSeriesLabelPosition.TwoColumns;

                ((PieSeriesLabel)series.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;

                PieSeriesView pieSeriesView = (PieSeriesView)series.View;

                pieSeriesView.Titles.Add(new SeriesTitle());
                pieSeriesView.Titles[0].Text = series.Name;

                pieSeriesView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Value_1, DataFilterCondition.GreaterThanOrEqual, 9));
                pieSeriesView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.NotEqual, "Others"));
                pieSeriesView.ExplodeMode = PieExplodeMode.UseFilters;
                pieSeriesView.RuntimeExploding = false;

                chartPieDay.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            }
            else
            {
                chartPieDay.Series.Clear();
            }
        }

        /// <summary>
        /// Bind day stack chart
        /// </summary>
        /// <param name="dt"></param>
        private void BindDayStackChart(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 1)
            {
                DataTable tempDt = dt.Clone();

                dt.DefaultView.Sort = "ERROR_RATE DESC";
                dt = dt.DefaultView.ToTable();

                chartStackDay.Series.Clear();
                Series series1 = new Series("불량률(%)", ViewType.FullStackedBar);
                Series series2 = new Series("양품률(%)", ViewType.FullStackedBar);
                int j = 10;

                if (dt.Rows.Count < 10)
                {
                    j = dt.Rows.Count;
                }

                for (int i = 0; i < j; i++)
                {
                    if (dt.Rows[i]["REC_DATE"].ToString() == "합 계")
                    {
                        continue;
                    }

                    DataRow dr = dt.Rows[i];
                    tempDt.ImportRow(dr);
                }

                tempDt.AcceptChanges();

                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    foreach (DataRow dr in tempDt.Rows)
                    {
                        series1.Points.Add(new SeriesPoint(dr["ITEM_CODE"].ToString(), Decimal.Parse(dr["ERROR_RATE"].ToString())));
                        series2.Points.Add(new SeriesPoint(dr["ITEM_CODE"].ToString(), Decimal.Parse(dr["PROD_RATE"].ToString())));
                    }

                    series1.ArgumentScaleType = ScaleType.Auto;
                    series1.ValueScaleType = ScaleType.Numerical;
                    series1.Label.TextPattern = "{VP:P0}";

                    series2.ArgumentScaleType = ScaleType.Auto;
                    series2.ValueScaleType = ScaleType.Numerical;
                    series2.Label.TextPattern = "{VP:P0}";

                    chartStackDay.Series.AddRange(new Series[] { series1, series2 });
                    chartStackDay.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                    ((XYDiagram)chartStackDay.Diagram).AxisY.Label.TextPattern = "{VP:P0}";
                    ((XYDiagram)chartStackDay.Diagram).Rotated = true;
                    ((FullStackedBarSeriesView)series1.View).BarWidth = 0.5;
                    ((FullStackedBarSeriesView)series2.View).BarWidth = 0.5;
                    ((XYDiagram)chartStackDay.Diagram).EnableAxisXZooming = true;
                }
            }
            else
            {
                chartStackDay.Series.Clear();
            }
        }

        #endregion
    }
}
