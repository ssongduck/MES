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

namespace SAMMI.PP
{
    /// <summary>
    /// PP0342 class
    /// </summary>
    public partial class PP0342 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        /// PP0342 constructor
        /// </summary>
        public PP0342()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// PP0341 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP0341_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();
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
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, "", "" }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
            }

            ChartTitle chartTitle1 = new ChartTitle();
            chartTitle1.Text = "[ 주조 보온로 온도 ]";
            chartCastTemp.Titles.Add(chartTitle1);

            calFromDt.Value = DateTime.Now.AddDays(-1);
            calToDt.Value = DateTime.Now;
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            _UltraGridUtil.InitializeGrid(this.grid1, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "CAST_TEMP", "온도", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "EVENT_TIME", "발생시간", false, GridColDataType_emu.DateTime24, 150, 150, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.Disposed += new EventHandler(PP0341_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(PP0341_Disposed);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[4];
            ClearAllControl();

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                base.DoInquire();

                sqlParameters[0] = sqlDBHelper.CreateParameter("@PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@FROM_DATE", string.Format("{0:yyyyMMdd}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[2] = sqlDBHelper.CreateParameter("@TO_DATE", string.Format("{0:yyyyMMdd}", calToDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[3] = sqlDBHelper.CreateParameter("@WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);

                _RtnDt = sqlDBHelper.FillTable("USP_PP0342_S1N_UNION", CommandType.StoredProcedure, sqlParameters);

                BindChart(_RtnDt);

                grid1.DataSource = _RtnDt;
                grid1.DataBind();
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
        /// Bind chart
        /// </summary>
        /// <param name="dt"></param>
        private void BindChart(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                chartCastTemp.Series.Clear();

                foreach (string workcenterName in dt.AsEnumerable().Select(t => t.Field<string>("WORKCENTERNAME")).Distinct().OrderBy(t => t[1]))
                {
                    DataTable tempDt = dt.Clone();

                    Series series = new Series(workcenterName, ViewType.Line);

                    foreach (DataRow dr in dt.AsEnumerable().Where(t => t.Field<string>("WORKCENTERNAME") == workcenterName))
                    {
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dr["EVENT_TIME"].ToString()), int.Parse(dr["CAST_TEMP"].ToString())));
                    }

                    series.ArgumentScaleType = ScaleType.DateTime;
                    series.ValueScaleType = ScaleType.Numerical;
                    ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Triangle;
                    ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Solid;

                    chartCastTemp.Series.Add(series);
                }

                XYDiagram diagram = chartCastTemp.Diagram as XYDiagram;
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
                diagram.AxisX.Label.TextPattern = "{V:yyyy-MM-dd HH}";
                diagram.AxisY.Label.TextPattern = "{V:n}";
                diagram.AxisY.WholeRange.SetMinMaxValues(550, 730);
            }
            else
            {
                chartCastTemp.Series.Clear();
            }
        }

        #endregion
    }
}
