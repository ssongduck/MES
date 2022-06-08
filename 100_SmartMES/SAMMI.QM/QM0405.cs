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
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.Data;

namespace SAMMI.QM
{
    /// <summary>
    /// QM0405 class
    /// </summary>
    public partial class QM0405 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        /// QM0405 constructor
        /// </summary>
        public QM0405()
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
        private void QM0403_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();
        }

        /// <summary>
        /// Grid view line poup menu showing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewLine_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.Menu == null) return;

                DXMenuItem dXMenuItem = new DevExpress.Utils.Menu.DXMenuItem("엑셀 내보내기", new EventHandler(this.exportLineMenu_Click));
                e.Menu.Items.Add(dXMenuItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Grid view line spl poup menu showing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewLineSpl_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.Menu == null) return;

                DXMenuItem dXMenuItem = new DevExpress.Utils.Menu.DXMenuItem("엑셀 내보내기", new EventHandler(this.exportLineSplMenu_click));
                e.Menu.Items.Add(dXMenuItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Grid view line spl custom summary calculate event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewLineSpl_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            //if (e.IsTotalSummary)
            //{
            //    GridView view = sender as GridView;

            //    //if (view.Columns["TOTAL"].SummaryItem.SummaryValue != null)
            //    //{
            //        e.TotalValue = 100;
            //    //}

            //    e.TotalValueReady = true;
            //}
        }

        /// <summary>
        /// Grid view line spl row style event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewLineSpl_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;

            GridView view = sender as GridView;
            string location = view.GetRowCellValue(e.RowHandle, "LOCATION").ToString();

            if (location == "합 계")
            {
                e.Appearance.BackColor = Color.SteelBlue;
                e.Appearance.ForeColor = Color.White;
            }
        }

        /// <summary>
        /// Export menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportLineMenu_Click(object sender, EventArgs e)
        {
            ExportExcel(gridViewLine);
        }

        /// <summary>
        /// Eport menu line spl click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportLineSplMenu_click(object sender, EventArgs e)
        {
            ExportExcel(gridViewLineSpl);
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
            }
            else
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOpCode, "", "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
            }

            calDt.Value = DateTime.Now;
            cmbFaultCode.SelectedIndex = 0;

            ChartTitle chartTitle1 = new ChartTitle();
            chartTitle1.Text = "[ 라인별 불량 추이 ]";
            chartTitle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartLine.Titles.Add(chartTitle1);

            ChartTitle chartTitle2 = new ChartTitle();
            chartTitle2.Text = "[ 스플별 불량 추이 ]";
            chartTitle2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartLineSpl.Titles.Add(chartTitle2);

            this.layoutControl.CanvasWidth = layoutControl.Width;
            this.layoutControl.CanvasHeight = layoutControl.Height;

            this.layoutControl.Bind(global::SAMMI.QM.Properties.Resources.BACK_0BH325065H1, FrontBack.BACK);
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
            this.Disposed += new EventHandler(QM0403_Disposed);

            gridViewLine.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewLine_PopupMenuShowing);
            gridViewLineSpl.PopupMenuShowing += new PopupMenuShowingEventHandler(gridViewLineSpl_PopupMenuShowing);

            gridViewLineSpl.CustomSummaryCalculate += new CustomSummaryEventHandler(gridViewLineSpl_CustomSummaryCalculate);
            gridViewLineSpl.RowStyle += new RowStyleEventHandler(gridViewLineSpl_RowStyle);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(QM0403_Disposed);

            gridViewLine.PopupMenuShowing -= new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewLine_PopupMenuShowing);
            gridViewLineSpl.PopupMenuShowing -= new PopupMenuShowingEventHandler(gridViewLineSpl_PopupMenuShowing);

            gridViewLineSpl.RowStyle -= new RowStyleEventHandler(gridViewLineSpl_RowStyle);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters1 = new SqlParameter[5];
            SqlParameter[] sqlParameters2 = new SqlParameter[5];

            ClearAllControl();

            try
            {
                if (gridViewLine.Columns["LINE"].Summary.Count > 0) gridViewLine.Columns["LINE"].Summary.Clear();
                if (gridViewLine.Columns["1"].Summary.Count > 0) gridViewLine.Columns["1"].Summary.Clear();
                if (gridViewLine.Columns["2"].Summary.Count > 0) gridViewLine.Columns["2"].Summary.Clear();
                if (gridViewLine.Columns["3"].Summary.Count > 0) gridViewLine.Columns["3"].Summary.Clear();
                if (gridViewLine.Columns["4"].Summary.Count > 0) gridViewLine.Columns["4"].Summary.Clear();
                if (gridViewLine.Columns["5"].Summary.Count > 0) gridViewLine.Columns["5"].Summary.Clear();
                if (gridViewLine.Columns["6"].Summary.Count > 0) gridViewLine.Columns["6"].Summary.Clear();
                if (gridViewLine.Columns["7"].Summary.Count > 0) gridViewLine.Columns["7"].Summary.Clear();
                if (gridViewLine.Columns["8"].Summary.Count > 0) gridViewLine.Columns["8"].Summary.Clear();
                if (gridViewLine.Columns["9"].Summary.Count > 0) gridViewLine.Columns["9"].Summary.Clear();
                if (gridViewLine.Columns["10"].Summary.Count > 0) gridViewLine.Columns["10"].Summary.Clear();
                if (gridViewLine.Columns["11"].Summary.Count > 0) gridViewLine.Columns["11"].Summary.Clear();
                if (gridViewLine.Columns["12"].Summary.Count > 0) gridViewLine.Columns["12"].Summary.Clear();
                if (gridViewLine.Columns["13"].Summary.Count > 0) gridViewLine.Columns["13"].Summary.Clear();
                if (gridViewLine.Columns["14"].Summary.Count > 0) gridViewLine.Columns["14"].Summary.Clear();
                if (gridViewLine.Columns["15"].Summary.Count > 0) gridViewLine.Columns["15"].Summary.Clear();
                if (gridViewLine.Columns["16"].Summary.Count > 0) gridViewLine.Columns["16"].Summary.Clear();
                if (gridViewLine.Columns["17"].Summary.Count > 0) gridViewLine.Columns["17"].Summary.Clear();
                if (gridViewLine.Columns["18"].Summary.Count > 0) gridViewLine.Columns["18"].Summary.Clear();
                if (gridViewLine.Columns["19"].Summary.Count > 0) gridViewLine.Columns["19"].Summary.Clear();
                if (gridViewLine.Columns["20"].Summary.Count > 0) gridViewLine.Columns["20"].Summary.Clear();
                if (gridViewLine.Columns["21"].Summary.Count > 0) gridViewLine.Columns["21"].Summary.Clear();
                if (gridViewLine.Columns["22"].Summary.Count > 0) gridViewLine.Columns["22"].Summary.Clear();
                if (gridViewLine.Columns["23"].Summary.Count > 0) gridViewLine.Columns["23"].Summary.Clear();
                if (gridViewLine.Columns["24"].Summary.Count > 0) gridViewLine.Columns["24"].Summary.Clear();
                if (gridViewLine.Columns["25"].Summary.Count > 0) gridViewLine.Columns["25"].Summary.Clear();
                if (gridViewLine.Columns["26"].Summary.Count > 0) gridViewLine.Columns["26"].Summary.Clear();
                if (gridViewLine.Columns["27"].Summary.Count > 0) gridViewLine.Columns["27"].Summary.Clear();
                if (gridViewLine.Columns["28"].Summary.Count > 0) gridViewLine.Columns["28"].Summary.Clear();
                if (gridViewLine.Columns["29"].Summary.Count > 0) gridViewLine.Columns["29"].Summary.Clear();
                if (gridViewLine.Columns["30"].Summary.Count > 0) gridViewLine.Columns["30"].Summary.Clear();
                if (gridViewLine.Columns["31"].Summary.Count > 0) gridViewLine.Columns["31"].Summary.Clear();
                if (gridViewLine.Columns["TOTAL"].Summary.Count > 0) gridViewLine.Columns["TOTAL"].Summary.Clear();

                if (gridViewLineSpl.Columns["LINE"].Summary.Count > 0) gridViewLineSpl.Columns["LINE"].Summary.Clear();
                if (gridViewLineSpl.Columns["1"].Summary.Count > 0) gridViewLineSpl.Columns["1"].Summary.Clear();
                if (gridViewLineSpl.Columns["2"].Summary.Count > 0) gridViewLineSpl.Columns["2"].Summary.Clear();
                if (gridViewLineSpl.Columns["3"].Summary.Count > 0) gridViewLineSpl.Columns["3"].Summary.Clear();
                if (gridViewLineSpl.Columns["4"].Summary.Count > 0) gridViewLineSpl.Columns["4"].Summary.Clear();
                if (gridViewLineSpl.Columns["5"].Summary.Count > 0) gridViewLineSpl.Columns["5"].Summary.Clear();
                if (gridViewLineSpl.Columns["6"].Summary.Count > 0) gridViewLineSpl.Columns["6"].Summary.Clear();
                if (gridViewLineSpl.Columns["7"].Summary.Count > 0) gridViewLineSpl.Columns["7"].Summary.Clear();
                if (gridViewLineSpl.Columns["8"].Summary.Count > 0) gridViewLineSpl.Columns["8"].Summary.Clear();
                if (gridViewLineSpl.Columns["9"].Summary.Count > 0) gridViewLineSpl.Columns["9"].Summary.Clear();
                if (gridViewLineSpl.Columns["10"].Summary.Count > 0) gridViewLineSpl.Columns["10"].Summary.Clear();
                if (gridViewLineSpl.Columns["11"].Summary.Count > 0) gridViewLineSpl.Columns["11"].Summary.Clear();
                if (gridViewLineSpl.Columns["12"].Summary.Count > 0) gridViewLineSpl.Columns["12"].Summary.Clear();
                if (gridViewLineSpl.Columns["13"].Summary.Count > 0) gridViewLineSpl.Columns["13"].Summary.Clear();
                if (gridViewLineSpl.Columns["14"].Summary.Count > 0) gridViewLineSpl.Columns["14"].Summary.Clear();
                if (gridViewLineSpl.Columns["15"].Summary.Count > 0) gridViewLineSpl.Columns["15"].Summary.Clear();
                if (gridViewLineSpl.Columns["16"].Summary.Count > 0) gridViewLineSpl.Columns["16"].Summary.Clear();
                if (gridViewLineSpl.Columns["17"].Summary.Count > 0) gridViewLineSpl.Columns["17"].Summary.Clear();
                if (gridViewLineSpl.Columns["18"].Summary.Count > 0) gridViewLineSpl.Columns["18"].Summary.Clear();
                if (gridViewLineSpl.Columns["19"].Summary.Count > 0) gridViewLineSpl.Columns["19"].Summary.Clear();
                if (gridViewLineSpl.Columns["20"].Summary.Count > 0) gridViewLineSpl.Columns["20"].Summary.Clear();
                if (gridViewLineSpl.Columns["21"].Summary.Count > 0) gridViewLineSpl.Columns["21"].Summary.Clear();
                if (gridViewLineSpl.Columns["22"].Summary.Count > 0) gridViewLineSpl.Columns["22"].Summary.Clear();
                if (gridViewLineSpl.Columns["23"].Summary.Count > 0) gridViewLineSpl.Columns["23"].Summary.Clear();
                if (gridViewLineSpl.Columns["24"].Summary.Count > 0) gridViewLineSpl.Columns["24"].Summary.Clear();
                if (gridViewLineSpl.Columns["25"].Summary.Count > 0) gridViewLineSpl.Columns["25"].Summary.Clear();
                if (gridViewLineSpl.Columns["26"].Summary.Count > 0) gridViewLineSpl.Columns["26"].Summary.Clear();
                if (gridViewLineSpl.Columns["27"].Summary.Count > 0) gridViewLineSpl.Columns["27"].Summary.Clear();
                if (gridViewLineSpl.Columns["28"].Summary.Count > 0) gridViewLineSpl.Columns["28"].Summary.Clear();
                if (gridViewLineSpl.Columns["29"].Summary.Count > 0) gridViewLineSpl.Columns["29"].Summary.Clear();
                if (gridViewLineSpl.Columns["30"].Summary.Count > 0) gridViewLineSpl.Columns["30"].Summary.Clear();
                if (gridViewLineSpl.Columns["31"].Summary.Count > 0) gridViewLineSpl.Columns["31"].Summary.Clear();
                if (gridViewLineSpl.Columns["TOTAL"].Summary.Count > 0) gridViewLineSpl.Columns["TOTAL"].Summary.Clear();

                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                base.DoInquire();

                sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_OPCODE", txtOpCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_FAULT_CODE", cmbFaultCode.Text.Split('-')[1].Trim() == "ALL" ? "%" : cmbFaultCode.Text.Split('-')[1].Trim(), SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters2[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[2] = sqlDBHelper.CreateParameter("@AS_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[3] = sqlDBHelper.CreateParameter("@AS_OPCODE", txtOpCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[4] = sqlDBHelper.CreateParameter("@AS_FAULT_CODE", cmbFaultCode.Text.Split('-')[1].Trim() == "ALL" ? "%" : cmbFaultCode.Text.Split('-')[1].Trim(), SqlDbType.VarChar, ParameterDirection.Input);

                int iLastDay = DateTime.DaysInMonth(DateTime.Parse(string.Format("{0:yyyy-MM-dd 08:00:00}", calDt.Value)).Year, DateTime.Parse(string.Format("{0:yyyy-MM-dd 08:00:00}", calDt.Value)).Month);

                dt1 = sqlDBHelper.FillTable("SP_GET_VW065_FAULT_LINE", CommandType.StoredProcedure, sqlParameters1);
                dt2 = sqlDBHelper.FillTable("SP_GET_VW065_FAULT_LINE_SPL", CommandType.StoredProcedure, sqlParameters2);

                gridControlLine.DataSource = dt1;
                gridControlLineSpl.DataSource = dt2;

                GridViewLineVisibleIndex(iLastDay);
                GridViewLineSplVisibleIndex(iLastDay);

                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    GridColumnSummaryItem summary = new GridColumnSummaryItem(SummaryItemType.Count, "LINE", "전체 행:{0:#,###}건");
                    gridViewLine.Columns["LINE"].Summary.Add(summary);
                    gridViewLine.Columns["1"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "1", "{0:#,###}"));
                    gridViewLine.Columns["2"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "2", "{0:#,###}"));
                    gridViewLine.Columns["3"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "3", "{0:#,###}"));
                    gridViewLine.Columns["4"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "4", "{0:#,###}"));
                    gridViewLine.Columns["5"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "5", "{0:#,###}"));
                    gridViewLine.Columns["6"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "6", "{0:#,###}"));
                    gridViewLine.Columns["7"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "7", "{0:#,###}"));
                    gridViewLine.Columns["8"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "8", "{0:#,###}"));
                    gridViewLine.Columns["9"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "9", "{0:#,###}"));
                    gridViewLine.Columns["10"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "10", "{0:#,###}"));
                    gridViewLine.Columns["11"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "11", "{0:#,###}"));
                    gridViewLine.Columns["12"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "12", "{0:#,###}"));
                    gridViewLine.Columns["13"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "13", "{0:#,###}"));
                    gridViewLine.Columns["14"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "14", "{0:#,###}"));
                    gridViewLine.Columns["15"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "15", "{0:#,###}"));
                    gridViewLine.Columns["16"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "16", "{0:#,###}"));
                    gridViewLine.Columns["17"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "17", "{0:#,###}"));
                    gridViewLine.Columns["18"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "18", "{0:#,###}"));
                    gridViewLine.Columns["19"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "19", "{0:#,###}"));
                    gridViewLine.Columns["20"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "20", "{0:#,###}"));
                    gridViewLine.Columns["21"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "21", "{0:#,###}"));
                    gridViewLine.Columns["22"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "22", "{0:#,###}"));
                    gridViewLine.Columns["23"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "23", "{0:#,###}"));
                    gridViewLine.Columns["24"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "24", "{0:#,###}"));
                    gridViewLine.Columns["25"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "25", "{0:#,###}"));
                    gridViewLine.Columns["26"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "26", "{0:#,###}"));
                    gridViewLine.Columns["27"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "27", "{0:#,###}"));
                    gridViewLine.Columns["28"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "28", "{0:#,###}"));

                    if (iLastDay == 29)
                    {
                        gridViewLine.Columns["29"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "29", "{0:#,###}"));
                    }
                    else if (iLastDay == 30)
                    {
                        gridViewLine.Columns["29"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "29", "{0:#,###}"));
                        gridViewLine.Columns["30"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "30", "{0:#,###}"));
                    }
                    else if (iLastDay == 31)
                    {
                        gridViewLine.Columns["29"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "29", "{0:#,###}"));
                        gridViewLine.Columns["30"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "30", "{0:#,###}"));
                        gridViewLine.Columns["31"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "31", "{0:#,###}"));
                    }

                    gridViewLine.Columns["TOTAL"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "TOTAL", "{0:#,###}"));

                    BindLineChart(dt1);
                }
                else
                {
                    chartLine.Series.Clear();
                }

                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    GridColumnSummaryItem summary1 = new GridColumnSummaryItem(SummaryItemType.Count, "LINE", "전체 행:{0:#,###}건");
                    gridViewLineSpl.Columns["LINE"].Summary.Add(summary1);
                    gridViewLineSpl.Columns["1"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "1", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("1")).Sum().ToString()));
                    gridViewLineSpl.Columns["2"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "2", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("2")).Sum().ToString()));
                    gridViewLineSpl.Columns["3"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "3", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("3")).Sum().ToString()));
                    gridViewLineSpl.Columns["4"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "4", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("4")).Sum().ToString()));
                    gridViewLineSpl.Columns["5"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "5", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("5")).Sum().ToString()));
                    gridViewLineSpl.Columns["6"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "6", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("6")).Sum().ToString()));
                    gridViewLineSpl.Columns["7"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "7", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("7")).Sum().ToString()));
                    gridViewLineSpl.Columns["8"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "8", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("8")).Sum().ToString()));
                    gridViewLineSpl.Columns["9"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "9", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("9")).Sum().ToString()));
                    gridViewLineSpl.Columns["10"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "10", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("10")).Sum().ToString()));
                    gridViewLineSpl.Columns["11"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "11", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("11")).Sum().ToString()));
                    gridViewLineSpl.Columns["12"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "12", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("12")).Sum().ToString()));
                    gridViewLineSpl.Columns["13"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "13", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("13")).Sum().ToString()));
                    gridViewLineSpl.Columns["14"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "14", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("14")).Sum().ToString()));
                    gridViewLineSpl.Columns["15"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "15", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("15")).Sum().ToString()));
                    gridViewLineSpl.Columns["16"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "16", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("16")).Sum().ToString()));
                    gridViewLineSpl.Columns["17"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "17", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("17")).Sum().ToString()));
                    gridViewLineSpl.Columns["18"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "18", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("18")).Sum().ToString()));
                    gridViewLineSpl.Columns["19"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "19", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("19")).Sum().ToString()));
                    gridViewLineSpl.Columns["20"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "20", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("20")).Sum().ToString()));
                    gridViewLineSpl.Columns["21"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "21", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("21")).Sum().ToString()));
                    gridViewLineSpl.Columns["22"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "22", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("22")).Sum().ToString()));
                    gridViewLineSpl.Columns["23"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "23", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("23")).Sum().ToString()));
                    gridViewLineSpl.Columns["24"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "24", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("24")).Sum().ToString()));
                    gridViewLineSpl.Columns["25"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "25", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("25")).Sum().ToString()));
                    gridViewLineSpl.Columns["26"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "26", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("26")).Sum().ToString()));
                    gridViewLineSpl.Columns["27"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "27", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("27")).Sum().ToString()));
                    gridViewLineSpl.Columns["28"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "28", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("28")).Sum().ToString()));

                    if (iLastDay == 29)
                    {
                        gridViewLineSpl.Columns["29"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "29", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("29")).Sum().ToString()));
                    }
                    else if (iLastDay == 30)
                    {
                        gridViewLineSpl.Columns["29"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "29", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("29")).Sum().ToString()));
                        gridViewLineSpl.Columns["30"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "30", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("30")).Sum().ToString()));
                    }
                    else if (iLastDay == 31)
                    {
                        gridViewLineSpl.Columns["29"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "29", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("29")).Sum().ToString()));
                        gridViewLineSpl.Columns["30"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "30", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("30")).Sum().ToString()));
                        gridViewLineSpl.Columns["31"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Sum, "31", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("31")).Sum().ToString()));
                    }

                    gridViewLineSpl.Columns["TOTAL"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Custom, "TOTAL", dt2.AsEnumerable().Where(t => t.Field<string>("LOCATION") != "합 계").Select(t => t.Field<int>("TOTAL")).Sum().ToString()));

                    BindLineSplChart(dt2);
                }
                else
                {
                    chartLineSpl.Series.Clear();
                }
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

        /// <summary>
        /// Bind line chart
        /// </summary>
        /// <param name="dt"></param>
        private void BindLineChart(DataTable dt)
        {
            chartLine.Series.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                chartLine.Series.Clear();

                int iDay1 = dt.AsEnumerable().Select(t => t.Field<int>("1")).Sum();
                int iDay2 = dt.AsEnumerable().Select(t => t.Field<int>("2")).Sum();
                int iDay3 = dt.AsEnumerable().Select(t => t.Field<int>("3")).Sum();
                int iDay4 = dt.AsEnumerable().Select(t => t.Field<int>("4")).Sum();
                int iDay5 = dt.AsEnumerable().Select(t => t.Field<int>("5")).Sum();
                int iDay6 = dt.AsEnumerable().Select(t => t.Field<int>("6")).Sum();
                int iDay7 = dt.AsEnumerable().Select(t => t.Field<int>("7")).Sum();
                int iDay8 = dt.AsEnumerable().Select(t => t.Field<int>("8")).Sum();
                int iDay9 = dt.AsEnumerable().Select(t => t.Field<int>("9")).Sum();
                int iDay10 = dt.AsEnumerable().Select(t => t.Field<int>("10")).Sum();
                int iDay11 = dt.AsEnumerable().Select(t => t.Field<int>("11")).Sum();
                int iDay12 = dt.AsEnumerable().Select(t => t.Field<int>("12")).Sum();
                int iDay13 = dt.AsEnumerable().Select(t => t.Field<int>("13")).Sum();
                int iDay14 = dt.AsEnumerable().Select(t => t.Field<int>("14")).Sum();
                int iDay15 = dt.AsEnumerable().Select(t => t.Field<int>("15")).Sum();
                int iDay16 = dt.AsEnumerable().Select(t => t.Field<int>("16")).Sum();
                int iDay17 = dt.AsEnumerable().Select(t => t.Field<int>("17")).Sum();
                int iDay18 = dt.AsEnumerable().Select(t => t.Field<int>("18")).Sum();
                int iDay19 = dt.AsEnumerable().Select(t => t.Field<int>("19")).Sum();
                int iDay20 = dt.AsEnumerable().Select(t => t.Field<int>("20")).Sum();
                int iDay21 = dt.AsEnumerable().Select(t => t.Field<int>("21")).Sum();
                int iDay22 = dt.AsEnumerable().Select(t => t.Field<int>("22")).Sum();
                int iDay23 = dt.AsEnumerable().Select(t => t.Field<int>("23")).Sum();
                int iDay24 = dt.AsEnumerable().Select(t => t.Field<int>("24")).Sum();
                int iDay25 = dt.AsEnumerable().Select(t => t.Field<int>("25")).Sum();
                int iDay26 = dt.AsEnumerable().Select(t => t.Field<int>("26")).Sum();
                int iDay27 = dt.AsEnumerable().Select(t => t.Field<int>("27")).Sum();
                int iDay28 = dt.AsEnumerable().Select(t => t.Field<int>("28")).Sum();
                int iDay29 = 0;
                int iDay30 = 0;
                int iDay31 = 0;

                if (dt.Columns.Contains("29"))
                {
                    iDay29 = dt.AsEnumerable().Select(t => t.Field<int>("29")).Sum();
                }

                if (dt.Columns.Contains("30"))
                {
                    iDay30 = dt.AsEnumerable().Select(t => t.Field<int>("30")).Sum();
                }

                if (dt.Columns.Contains("31"))
                {
                    iDay31 = dt.AsEnumerable().Select(t => t.Field<int>("31")).Sum();
                }

                Series series = new Series("일자별 불량", ViewType.Line);

                series.Points.Add(new SeriesPoint("1일", iDay1));
                series.Points.Add(new SeriesPoint("2일", iDay2));
                series.Points.Add(new SeriesPoint("3일", iDay3));
                series.Points.Add(new SeriesPoint("4일", iDay4));
                series.Points.Add(new SeriesPoint("5일", iDay5));
                series.Points.Add(new SeriesPoint("6일", iDay6));
                series.Points.Add(new SeriesPoint("7일", iDay7));
                series.Points.Add(new SeriesPoint("8일", iDay8));
                series.Points.Add(new SeriesPoint("9일", iDay9));
                series.Points.Add(new SeriesPoint("10일", iDay10));
                series.Points.Add(new SeriesPoint("11일", iDay11));
                series.Points.Add(new SeriesPoint("12일", iDay12));
                series.Points.Add(new SeriesPoint("13일", iDay13));
                series.Points.Add(new SeriesPoint("14일", iDay14));
                series.Points.Add(new SeriesPoint("15일", iDay15));
                series.Points.Add(new SeriesPoint("16일", iDay16));
                series.Points.Add(new SeriesPoint("17일", iDay17));
                series.Points.Add(new SeriesPoint("18일", iDay18));
                series.Points.Add(new SeriesPoint("19일", iDay19));
                series.Points.Add(new SeriesPoint("20일", iDay20));
                series.Points.Add(new SeriesPoint("21일", iDay21));
                series.Points.Add(new SeriesPoint("22일", iDay22));
                series.Points.Add(new SeriesPoint("23일", iDay23));
                series.Points.Add(new SeriesPoint("24일", iDay24));
                series.Points.Add(new SeriesPoint("25일", iDay25));
                series.Points.Add(new SeriesPoint("26일", iDay26));
                series.Points.Add(new SeriesPoint("27일", iDay27));
                series.Points.Add(new SeriesPoint("28일", iDay28));

                if (dt.Columns.Contains("29"))
                {
                    series.Points.Add(new SeriesPoint("29일", iDay29));
                }

                if (dt.Columns.Contains("30"))
                {
                    series.Points.Add(new SeriesPoint("30일", iDay30));
                }

                if (dt.Columns.Contains("31"))
                {
                    series.Points.Add(new SeriesPoint("31일", iDay31));
                }

                series.ArgumentScaleType = ScaleType.Qualitative;
                series.ValueScaleType = ScaleType.Numerical;
                ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Triangle;
                ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Solid;

                chartLine.Series.Add(series);

                XYDiagram diagram = chartLine.Diagram as XYDiagram;
                diagram.AxisX.Label.TextPattern = "{V}";
                diagram.AxisY.Label.TextPattern = "{V}";
                diagram.EnableAxisXScrolling = true;
                diagram.EnableAxisXZooming = true;
                diagram.EnableAxisYScrolling = true;
                diagram.EnableAxisYZooming = true;
            }
            else
            {
                chartLine.Series.Clear();
            }
        }

        /// <summary>
        /// Bind line spl chart
        /// </summary>
        /// <param name="dt"></param>
        private void BindLineSplChart(DataTable dt)
        {
            chartLineSpl.Series.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                chartLineSpl.Series.Clear();

                DataTable distinctDt = dt.DefaultView.ToTable(true, "LOCATION");

                if (distinctDt != null && distinctDt.Rows.Count > 0)
                {
                    foreach (DataRow dr in distinctDt.Rows)
                    {
                        if (dr["LOCATION"].ToString() == "합 계")
                        {
                            continue;
                        }

                        int iDay1 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("1")).Sum();
                        int iDay2 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("2")).Sum();
                        int iDay3 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("3")).Sum();
                        int iDay4 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("4")).Sum();
                        int iDay5 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("5")).Sum();
                        int iDay6 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("6")).Sum();
                        int iDay7 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("7")).Sum();
                        int iDay8 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("8")).Sum();
                        int iDay9 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("9")).Sum();
                        int iDay10 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("10")).Sum();
                        int iDay11 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("11")).Sum();
                        int iDay12 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("12")).Sum();
                        int iDay13 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("13")).Sum();
                        int iDay14 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("14")).Sum();
                        int iDay15 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("15")).Sum();
                        int iDay16 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("16")).Sum();
                        int iDay17 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("17")).Sum();
                        int iDay18 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("18")).Sum();
                        int iDay19 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("19")).Sum();
                        int iDay20 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("20")).Sum();
                        int iDay21 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("21")).Sum();
                        int iDay22 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("22")).Sum();
                        int iDay23 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("23")).Sum();
                        int iDay24 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("24")).Sum();
                        int iDay25 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("25")).Sum();
                        int iDay26 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("26")).Sum();
                        int iDay27 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("27")).Sum();
                        int iDay28 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("28")).Sum();
                        int iDay29 = 0;
                        int iDay30 = 0;
                        int iDay31 = 0;

                        if (dt.Columns.Contains("29"))
                        {
                            iDay29 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("29")).Sum();
                        }

                        if (dt.Columns.Contains("30"))
                        {
                            iDay30 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("30")).Sum();
                        }

                        if (dt.Columns.Contains("31"))
                        {
                            iDay31 = dt.AsEnumerable().Where(t => t.Field<string>("LOCATION") == dr["LOCATION"].ToString()).Select(t => t.Field<int>("31")).Sum();
                        }

                        Series series = new Series(dr["LOCATION"].ToString(), ViewType.Line);

                        series.Points.Add(new SeriesPoint("1일", iDay1));
                        series.Points.Add(new SeriesPoint("2일", iDay2));
                        series.Points.Add(new SeriesPoint("3일", iDay3));
                        series.Points.Add(new SeriesPoint("4일", iDay4));
                        series.Points.Add(new SeriesPoint("5일", iDay5));
                        series.Points.Add(new SeriesPoint("6일", iDay6));
                        series.Points.Add(new SeriesPoint("7일", iDay7));
                        series.Points.Add(new SeriesPoint("8일", iDay8));
                        series.Points.Add(new SeriesPoint("9일", iDay9));
                        series.Points.Add(new SeriesPoint("10일", iDay10));
                        series.Points.Add(new SeriesPoint("11일", iDay11));
                        series.Points.Add(new SeriesPoint("12일", iDay12));
                        series.Points.Add(new SeriesPoint("13일", iDay13));
                        series.Points.Add(new SeriesPoint("14일", iDay14));
                        series.Points.Add(new SeriesPoint("15일", iDay15));
                        series.Points.Add(new SeriesPoint("16일", iDay16));
                        series.Points.Add(new SeriesPoint("17일", iDay17));
                        series.Points.Add(new SeriesPoint("18일", iDay18));
                        series.Points.Add(new SeriesPoint("19일", iDay19));
                        series.Points.Add(new SeriesPoint("20일", iDay20));
                        series.Points.Add(new SeriesPoint("21일", iDay21));
                        series.Points.Add(new SeriesPoint("22일", iDay22));
                        series.Points.Add(new SeriesPoint("23일", iDay23));
                        series.Points.Add(new SeriesPoint("24일", iDay24));
                        series.Points.Add(new SeriesPoint("25일", iDay25));
                        series.Points.Add(new SeriesPoint("26일", iDay26));
                        series.Points.Add(new SeriesPoint("27일", iDay27));
                        series.Points.Add(new SeriesPoint("28일", iDay28));

                        if (dt.Columns.Contains("29"))
                        {
                            series.Points.Add(new SeriesPoint("29일", iDay29));
                        }

                        if (dt.Columns.Contains("30"))
                        {
                            series.Points.Add(new SeriesPoint("30일", iDay30));
                        }

                        if (dt.Columns.Contains("31"))
                        {
                            series.Points.Add(new SeriesPoint("31일", iDay31));
                        }

                        series.ArgumentScaleType = ScaleType.Qualitative;
                        series.ValueScaleType = ScaleType.Numerical;
                        ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Triangle;
                        ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Solid;

                        chartLineSpl.Series.Add(series);
                    }
                }

                XYDiagram diagram = chartLineSpl.Diagram as XYDiagram;
                diagram.AxisX.Label.TextPattern = "{V}";
                diagram.AxisY.Label.TextPattern = "{V}";
                diagram.EnableAxisXScrolling = true;
                diagram.EnableAxisXZooming = true;
                diagram.EnableAxisYScrolling = true;
                diagram.EnableAxisYZooming = true;
            }
            else
            {
                chartLineSpl.Series.Clear();
            }
        }

        /// <summary>
        /// Grid view line visible index
        /// </summary>
        /// <param name="iLastDay"></param>
        private void GridViewLineVisibleIndex(int iLastDay)
        {
            gridViewLine.Columns["LINE"].VisibleIndex = 0;
            gridViewLine.Columns["1"].VisibleIndex = 1;
            gridViewLine.Columns["2"].VisibleIndex = 2;
            gridViewLine.Columns["3"].VisibleIndex = 3;
            gridViewLine.Columns["4"].VisibleIndex = 4;
            gridViewLine.Columns["5"].VisibleIndex = 5;
            gridViewLine.Columns["6"].VisibleIndex = 6;
            gridViewLine.Columns["7"].VisibleIndex = 7;
            gridViewLine.Columns["8"].VisibleIndex = 8;
            gridViewLine.Columns["9"].VisibleIndex = 9;
            gridViewLine.Columns["10"].VisibleIndex = 10;
            gridViewLine.Columns["11"].VisibleIndex = 11;
            gridViewLine.Columns["12"].VisibleIndex = 12;
            gridViewLine.Columns["13"].VisibleIndex = 13;
            gridViewLine.Columns["14"].VisibleIndex = 14;
            gridViewLine.Columns["15"].VisibleIndex = 15;
            gridViewLine.Columns["16"].VisibleIndex = 16;
            gridViewLine.Columns["17"].VisibleIndex = 17;
            gridViewLine.Columns["18"].VisibleIndex = 18;
            gridViewLine.Columns["19"].VisibleIndex = 19;
            gridViewLine.Columns["20"].VisibleIndex = 20;
            gridViewLine.Columns["21"].VisibleIndex = 21;
            gridViewLine.Columns["22"].VisibleIndex = 22;
            gridViewLine.Columns["23"].VisibleIndex = 23;
            gridViewLine.Columns["24"].VisibleIndex = 24;
            gridViewLine.Columns["25"].VisibleIndex = 25;
            gridViewLine.Columns["26"].VisibleIndex = 26;
            gridViewLine.Columns["27"].VisibleIndex = 27;
            gridViewLine.Columns["28"].VisibleIndex = 28;

            if (iLastDay == 28)
            {
                gridViewLine.Columns["29"].Visible = false;
                gridViewLine.Columns["30"].Visible = false;
                gridViewLine.Columns["31"].Visible = false;
            }
            if (iLastDay == 29)
            {
                gridViewLine.Columns["29"].Visible = true;
                gridViewLine.Columns["30"].Visible = false;
                gridViewLine.Columns["31"].Visible = false;
                gridViewLine.Columns["29"].VisibleIndex = 29;
            }
            else if (iLastDay == 30)
            {
                gridViewLine.Columns["29"].Visible = true;
                gridViewLine.Columns["30"].Visible = true;
                gridViewLine.Columns["31"].Visible = false;
                gridViewLine.Columns["29"].VisibleIndex = 29;
                gridViewLine.Columns["30"].VisibleIndex = 30;
            }
            else if (iLastDay == 31)
            {
                gridViewLine.Columns["29"].Visible = true;
                gridViewLine.Columns["30"].Visible = true;
                gridViewLine.Columns["31"].Visible = true;
                gridViewLine.Columns["29"].VisibleIndex = 29;
                gridViewLine.Columns["30"].VisibleIndex = 30;
                gridViewLine.Columns["31"].VisibleIndex = 31;
            }

            gridViewLine.Columns["TOTAL"].VisibleIndex = 32;
        }

        /// <summary>
        /// Grid view line spl visible index
        /// </summary>
        /// <param name="iLastDay"></param>
        private void GridViewLineSplVisibleIndex(int iLastDay)
        {
            gridViewLineSpl.Columns["LINE"].VisibleIndex = 0;
            gridViewLineSpl.Columns["LOCATION"].VisibleIndex = 1;
            gridViewLineSpl.Columns["1"].VisibleIndex = 2;
            gridViewLineSpl.Columns["2"].VisibleIndex = 3;
            gridViewLineSpl.Columns["3"].VisibleIndex = 4;
            gridViewLineSpl.Columns["4"].VisibleIndex = 5;
            gridViewLineSpl.Columns["5"].VisibleIndex = 6;
            gridViewLineSpl.Columns["6"].VisibleIndex = 7;
            gridViewLineSpl.Columns["7"].VisibleIndex = 8;
            gridViewLineSpl.Columns["8"].VisibleIndex = 9;
            gridViewLineSpl.Columns["9"].VisibleIndex = 10;
            gridViewLineSpl.Columns["10"].VisibleIndex = 11;
            gridViewLineSpl.Columns["11"].VisibleIndex = 12;
            gridViewLineSpl.Columns["12"].VisibleIndex = 13;
            gridViewLineSpl.Columns["13"].VisibleIndex = 14;
            gridViewLineSpl.Columns["14"].VisibleIndex = 15;
            gridViewLineSpl.Columns["15"].VisibleIndex = 16;
            gridViewLineSpl.Columns["16"].VisibleIndex = 17;
            gridViewLineSpl.Columns["17"].VisibleIndex = 18;
            gridViewLineSpl.Columns["18"].VisibleIndex = 19;
            gridViewLineSpl.Columns["19"].VisibleIndex = 20;
            gridViewLineSpl.Columns["20"].VisibleIndex = 21;
            gridViewLineSpl.Columns["21"].VisibleIndex = 22;
            gridViewLineSpl.Columns["22"].VisibleIndex = 23;
            gridViewLineSpl.Columns["23"].VisibleIndex = 24;
            gridViewLineSpl.Columns["24"].VisibleIndex = 25;
            gridViewLineSpl.Columns["25"].VisibleIndex = 26;
            gridViewLineSpl.Columns["26"].VisibleIndex = 27;
            gridViewLineSpl.Columns["27"].VisibleIndex = 28;
            gridViewLineSpl.Columns["28"].VisibleIndex = 29;

            if (iLastDay == 28)
            {
                gridViewLineSpl.Columns["29"].Visible = false;
                gridViewLineSpl.Columns["30"].Visible = false;
                gridViewLineSpl.Columns["31"].Visible = false;
            }
            if (iLastDay == 29)
            {
                gridViewLineSpl.Columns["29"].Visible = true;
                gridViewLineSpl.Columns["30"].Visible = false;
                gridViewLineSpl.Columns["31"].Visible = false;
                gridViewLineSpl.Columns["29"].VisibleIndex = 30;
            }
            else if (iLastDay == 30)
            {
                gridViewLineSpl.Columns["29"].Visible = true;
                gridViewLineSpl.Columns["30"].Visible = true;
                gridViewLineSpl.Columns["31"].Visible = false;
                gridViewLineSpl.Columns["29"].VisibleIndex = 31;
                gridViewLineSpl.Columns["30"].VisibleIndex = 32;
            }
            else if (iLastDay == 31)
            {
                gridViewLineSpl.Columns["29"].Visible = true;
                gridViewLineSpl.Columns["30"].Visible = true;
                gridViewLineSpl.Columns["31"].Visible = true;
                gridViewLineSpl.Columns["29"].VisibleIndex = 30;
                gridViewLineSpl.Columns["30"].VisibleIndex = 31;
                gridViewLineSpl.Columns["31"].VisibleIndex = 32;
            }

            gridViewLineSpl.Columns["TOTAL"].VisibleIndex = 33;
        }

        #endregion
    }
}
