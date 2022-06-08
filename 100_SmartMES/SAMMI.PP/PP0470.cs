#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : PP0470
//   Form Name      : 비가동상세
//   Name Space     : SAMMI.PP
//   Created Date   : 2022.02.15
//   Made By        : 정용석
//   Description    : 작업일보 활용#5 (설비별/품목별/금형별 비가동내역)
// *---------------------------------------------------------------------------------------------*
#endregion

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

using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.Data;

namespace SAMMI.PP
{
    public partial class PP0470 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _dt = new DataTable();

        DataSet   _ds = new DataSet();


        BizTextBoxManagerEX btbManager = new BizTextBoxManagerEX();
        /// <summary>
        /// Change grid1 datatable 
        /// </summary>

        private DataTable _ChangeDt = new DataTable(); 

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();
        
        /// <summary>
        /// PlantCode
        /// </summary>
        private string _PlantCode = string.Empty;

        #endregion

        public PP0470()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();
            AttachEventHandlers();
        }        
        #region Event

        /// <summary>
        /// PP0370 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP0470_Disposed(object sender, EventArgs e)
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

            this._PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this._PlantCode.Equals("SK"))
            {
                this._PlantCode = "SK1";
            }
            else if (this._PlantCode.Equals("EC"))
            {
                this._PlantCode = "SK2";
            }

            deSdate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {

        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.Disposed += new EventHandler(PP0470_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(PP0470_Disposed);
        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[3];
            ClearAllControl();

            try
            {
                base.DoInquire();

                string sdate = string.Format("{0:yyyy-MM-dd}", deSdate.Value);
                string edate = string.Format("{0:yyyy-MM-dd}", deEdate.Value);                

                param[0] = helper.CreateParameter("@AS_PLANTCODE",      "SK2", SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@AS_SDATE",          sdate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@AS_EDATE",          edate, SqlDbType.VarChar, ParameterDirection.Input);

                _ds = helper.FillDataSet("USP_PP0470_S1", CommandType.StoredProcedure, param);                

                gridControl1.DataSource = _ds.Tables[0];
                gridControl2.DataSource = _ds.Tables[1];
                DrawChartStopRatio(_ds.Tables[1]);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }

        private void DrawChartStopRatio(DataTable dt)
        {
            chartControl1.Series.Clear();

            Series series1 = new Series("품목별 비가동", ViewType.Pie);
            //SeriesTitle seriesTitle1 = new DevExpress.XtraCharts.SeriesTitle();                                 
            //PieSeriesView pieSeriesView1 = new DevExpress.XtraCharts.PieSeriesView();;

            //seriesTitle1.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
            //seriesTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));                
            //pieSeriesView1.Titles.AddRange(new DevExpress.XtraCharts.SeriesTitle[] { seriesTitle1 });
            //series1.View = pieSeriesView1;     

            //series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            //series1.Label.TextPattern = "{A}: {VP:P0}";
            //series1.Label.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));                
            //((PieSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.Outside;               

            series1.LegendTextPattern = "{A}";

            foreach (DataRow dr in dt.Rows)
            {
                series1.Points.Add(new SeriesPoint(dr["StopDesc"].ToString(), dr["StopTime"] == DBNull.Value ? 0 : Convert.ToInt32(dr["StopTime"])));
            }

            chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            chartControl1.Series.Add(series1);
        }


        /// <summary>
        /// Do new
        /// </summary>
        public override void DoNew()
        {
        }

        /// <summary>
        /// Do save
        /// </summary>
        public override void DoSave()
        {
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
        }

        /// <summary>
        /// Clear all control
        /// </summary>
        private void ClearAllControl()
        {
            InitializeControl(this);
        }

        /// <summary>
        /// Initialize control
        /// </summary>
        /// <param name="control"></param>
        private void InitializeControl(System.Windows.Forms.Control control)
        {
            if (control == null)
            {
                return;
            }

            foreach (System.Windows.Forms.Control ctrl in control.Controls)
            {
                InitializeControl(ctrl);

                if (ctrl.GetType().Name == "TextBox")
                {
                    TextBox textBox = (TextBox)ctrl;

                    //foreach (string s in _EmptyArrs)
                    //{
                    //    if (textBox.Name.StartsWith(s))
                    //    {
                    //        textBox.Text = string.Empty;
                    //    }
                    //}
                }

                if (ctrl.GetType().Name == "MaskedTextBox")
                {
                    MaskedTextBox maskedTextBox = (MaskedTextBox)ctrl;

                    //foreach (string sVal in _EmptyArrs)
                    //{
                    //    if (maskedTextBox.Name.StartsWith(sVal))
                    //    {
                    //        maskedTextBox.Text = string.Empty;
                    //    }
                    //}
                }
            }
            return;
        }

        #endregion
    }
}
