#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : PP0440
//   Form Name      : 작업종합현황
//   Name Space     : SAMMI.PP
//   Created Date   : 2022.02.08
//   Made By        : 정용석
//   Description    : 작업일보 활용#1-종합화면 
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
    public partial class PP0440 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _dt = new DataTable();

        DataSet   _ds = new DataSet();

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

        public PP0440()
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
        private void PP0440_Disposed(object sender, EventArgs e)
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

            this.Disposed += new EventHandler(PP0440_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(PP0440_Disposed);
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
                _ChangeDt.Clear();

                base.DoInquire();

                string sdate = string.Format("{0:yyyy-MM-dd}", deSdate.Value);
                string edate = string.Format("{0:yyyy-MM-dd}", deEdate.Value);

                param[0] = helper.CreateParameter("@AS_PLANTCODE", "SK2", SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@AS_SDATE",     sdate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@AS_EDATE",     edate, SqlDbType.VarChar, ParameterDirection.Input);                

                _ds = helper.FillDataSet("USP_PP0440_S1", CommandType.StoredProcedure, param);
                DrawChart(_ds);                
                gridControl1.DataSource = _ds.Tables[4];                
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

        void DrawChart(DataSet ds)
        {
            // 설비종합가동율
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                chartControl1.Series.Clear();

                Series series = new Series("종합가동율", ViewType.Bar);
                series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                //(series.Label as SideBySideBarSeriesLabel).Position = BarSeriesLabelPosition.Top;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    series.Points.Add(new SeriesPoint(dr["workcentercode"].ToString(), dr["runRate"] == DBNull.Value ? 0 : Convert.ToDouble(dr["runRate"])));

                }
                chartControl1.Series.Add(series);

                XYDiagram diagram = chartControl1.Diagram as XYDiagram;
                diagram.AxisX.Label.Font = new System.Drawing.Font("맑은 고딕", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                diagram.AxisY.Label.TextPattern = "{V:n0}";
                diagram.AxisY.WholeRange.Auto = false;
                diagram.AxisY.WholeRange.SetMinMaxValues(0, 100);                
                diagram.EnableAxisXScrolling = true;
                diagram.EnableAxisXZooming   = true;
                diagram.EnableAxisYScrolling = true;
                diagram.EnableAxisYZooming   = true;                
            }
            else { chartControl1.Series.Clear(); }
            // 유형별 비가동 Worst 5
            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
            {
                chartControl2.Series.Clear();

                Series series1 = new Series("설비부문", ViewType.Pie);
                Series series2 = new Series("금형부문", ViewType.Pie);
                Series series3 = new Series("생산부문", ViewType.Pie);                
                
                SeriesTitle     seriesTitle1 = new DevExpress.XtraCharts.SeriesTitle();
                SeriesTitle     seriesTitle2 = new DevExpress.XtraCharts.SeriesTitle();
                SeriesTitle     seriesTitle3 = new DevExpress.XtraCharts.SeriesTitle();                
                
                PieSeriesView pieSeriesView1 = new DevExpress.XtraCharts.PieSeriesView();
                PieSeriesView pieSeriesView2 = new DevExpress.XtraCharts.PieSeriesView();
                PieSeriesView pieSeriesView3 = new DevExpress.XtraCharts.PieSeriesView();

                seriesTitle1.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
                seriesTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                seriesTitle1.Text = "──────\r\n 설비부문";
                pieSeriesView1.Titles.AddRange(new DevExpress.XtraCharts.SeriesTitle[] {seriesTitle1});
                series1.View = pieSeriesView1;

                seriesTitle2.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
                seriesTitle2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                seriesTitle2.Text = "──────\r\n 금형부문";
                pieSeriesView2.Titles.AddRange(new DevExpress.XtraCharts.SeriesTitle[] { seriesTitle2 });
                series2.View = pieSeriesView2;

                seriesTitle3.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
                seriesTitle3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                seriesTitle3.Text = "──────\r\n 생산부문";
                pieSeriesView3.Titles.AddRange(new DevExpress.XtraCharts.SeriesTitle[] { seriesTitle3 });
                series3.View = pieSeriesView3;                   

                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                series3.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                series1.Label.TextPattern = "{A}: {VP:P0}";
                series2.Label.TextPattern = "{A}: {VP:P0}";
                series3.Label.TextPattern = "{A}: {VP:P0}";
                

                series1.Label.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                series2.Label.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                series3.Label.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                ((PieSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.Outside;
                ((PieSeriesLabel)series2.Label).Position = PieSeriesLabelPosition.Outside;
                ((PieSeriesLabel)series3.Label).Position = PieSeriesLabelPosition.Outside;

                foreach (DataRow dr in ds.Tables[1].AsEnumerable().Where(t => t.Field<string>("StopType") == "X"))
                {
                    series1.Points.Add(new SeriesPoint(dr["StopDesc"].ToString(), dr["TotalStopTime"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TotalStopTime"])));                           
                }
                foreach (DataRow dr in ds.Tables[1].AsEnumerable().Where(t => t.Field<string>("StopType") == "Y"))
                {
                    series2.Points.Add(new SeriesPoint(dr["StopDesc"].ToString(), dr["TotalStopTime"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TotalStopTime"])));                    
                }
                foreach (DataRow dr in ds.Tables[1].AsEnumerable().Where(t => t.Field<string>("StopType") == "Z"))
                {
                    series3.Points.Add(new SeriesPoint(dr["StopDesc"].ToString(), dr["TotalStopTime"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TotalStopTime"])));                    
                }
                chartControl2.Series.Add(series1);
                chartControl2.Series.Add(series2);
                chartControl2.Series.Add(series3);
            }
            else { chartControl2.Series.Clear(); }

            // 작업자 가동율 Best 10
            if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
            {
                chartControl3.Series.Clear();

                Series series = new Series("작업자 가동율", ViewType.Bar);
                series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                (series.Label as SideBySideBarSeriesLabel).Position = BarSeriesLabelPosition.Top;

                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    series.Points.Add(new SeriesPoint(dr["workername"] == DBNull.Value ? "empty" :dr["workername"].ToString(),
                                                      dr["runRate"] == DBNull.Value ? 0 : Convert.ToDouble(dr["runRate"])));

                }
                chartControl3.Series.Add(series);
            }
            else { chartControl3.Series.Clear(); }

            // 유형별 비가동 Worst 5
            if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
            {
                chartControl4.Series.Clear();

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

                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    series1.Points.Add(new SeriesPoint(dr["ItemName"] == DBNull.Value ? "empty" : dr["ItemName"].ToString(), 
                                                       dr["TotalStopTime"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TotalStopTime"]) ));
                }                
                chartControl4.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl4.Series.Add(series1);
            }
            else { chartControl4.Series.Clear(); }

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
