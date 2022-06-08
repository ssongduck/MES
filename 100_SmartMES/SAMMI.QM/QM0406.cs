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
    /// QM0406 class
    /// </summary>
    public partial class QM0406 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        /// Complete datatable
        /// </summary>
        DataTable _CompleteDt = null;

        /// <summary>
        /// Error datatable
        /// </summary>
        DataTable _ErrorDt = null;

        /// <summary>
        /// Ing datatable
        /// </summary>
        DataTable _IngDt = null;

        /// <summary>
        /// Data datatable
        /// </summary>
        DataTable _DataDt = null;

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
        /// QM0406 constructor
        /// </summary>
        public QM0406()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// QM0406 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QM0406_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();
        }

        /// <summary>
        /// Complete grid view focused row changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewComplete_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridViewComplete.GetFocusedDataRow();
            DataRow historyDr = null;

            if (dr != null)
            {
                if (_CompleteDt != null && _CompleteDt.Rows.Count > 0)
                {
                    historyDr = _CompleteDt.AsEnumerable().Where(t => t.Field<string>("LOT_NO") == dr["LOT_NO"].ToString()).FirstOrDefault();
                }

                if (historyDr != null)
                {
                    lbl400OKNG_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE01"])) ? "N/A" : historyDr["JUDGE01"].ToString();
                    lbl400OKNG_COM.BackColor = (lbl400OKNG_COM.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl400OKNG_COM.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl410OKNG_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE02"])) ? "N/A" : historyDr["JUDGE02"].ToString();
                    lbl410OKNG_COM.BackColor = (lbl410OKNG_COM.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl410OKNG_COM.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl420OKNG_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE04"])) ? "N/A" : historyDr["JUDGE04"].ToString();
                    lbl420OKNG_COM.BackColor = (lbl420OKNG_COM.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl420OKNG_COM.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl430OKNG_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE09"])) ? "N/A" : historyDr["JUDGE09"].ToString();
                    lbl430OKNG_COM.BackColor = (lbl430OKNG_COM.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl430OKNG_COM.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl440OKNG_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE24"])) ? "N/A" : historyDr["JUDGE24"].ToString();
                    lbl440OKNG_COM.BackColor = (lbl440OKNG_COM.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl440OKNG_COM.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl450OKNG_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE25"])) ? "N/A" : historyDr["JUDGE25"].ToString();
                    lbl450OKNG_COM.BackColor = (lbl450OKNG_COM.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl450OKNG_COM.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl460OKNG_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE26"])) ? "N/A" : historyDr["JUDGE26"].ToString();
                    lbl460OKNG_COM.BackColor = (lbl460OKNG_COM.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl460OKNG_COM.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;

                    lblData01_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA01"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA01"].ToString()));
                    lblData02_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA02"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA02"].ToString()));
                    lblData03_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA03"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA03"].ToString()));
                    lblData04_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA04"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA04"].ToString()));
                    lblData05_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA05"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA05"].ToString()));
                    lblData06_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA06"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA06"].ToString()));
                    lblData07_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA07"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA07"].ToString()));
                    lblData08_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA08"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA08"].ToString()));
                    lblData09_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA09"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA09"].ToString()));
                    lblData10_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA10"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA10"].ToString()));
                    lblData11_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA11"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA11"].ToString()));
                    lblData12_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA12"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA12"].ToString()));
                    lblData13_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA13"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA13"].ToString()));
                    lblData14_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA14"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA14"].ToString()));
                    lblData15_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA15"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA15"].ToString()));
                    lblData16_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA16"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA16"].ToString()));
                    lblData17_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA17"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA17"].ToString()));
                    lblData18_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA18"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA18"].ToString()));
                    lblData19_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA19"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA19"].ToString()));
                    lblData20_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA20"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA20"].ToString()));
                    lblData21_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA21"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA21"].ToString()));
                    lblData22_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA22"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA22"].ToString()));
                    lblData23_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA23"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA23"].ToString()));
                    lblData24_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA24"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA24"].ToString()));
                    lblData25_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA25"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA25"].ToString()));
                    lblData26_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA26"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA26"].ToString()));
                    lblData27_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA27"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA27"].ToString()));
                    lblData28_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA28"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA28"].ToString()));
                    lblData29_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA29"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA29"].ToString()));
                    lblData30_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA30"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA30"].ToString()));
                    lblData31_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA31"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA31"].ToString()));
                    lblData32_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA32"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA32"].ToString()));
                    lblData33_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA33"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA33"].ToString()));
                    lblData34_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA34"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA34"].ToString()));
                    lblData35_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA35"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA35"].ToString()));
                    lblData36_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA36"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA36"].ToString()));

                    lblLastEventTime01_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME01"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME01"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime02_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME02"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME02"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime03_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME03"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME03"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime04_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME04"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME04"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime05_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME05"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME05"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime06_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME06"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME06"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime07_COM.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME07"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME07"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        /// <summary>
        /// Error grid view focused row changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewError_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridViewError.GetFocusedDataRow();
            DataRow historyDr = null;

            if (dr != null)
            {
                if (_ErrorDt != null && _ErrorDt.Rows.Count > 0)
                {
                    historyDr = _ErrorDt.AsEnumerable().Where(t => t.Field<string>("LOT_NO") == dr["LOT_NO"].ToString()).FirstOrDefault();
                }

                if (historyDr != null)
                {
                    lbl400OKNG_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE01"])) ? "N/A" : historyDr["JUDGE01"].ToString();
                    lbl400OKNG_ERR.BackColor = (lbl400OKNG_ERR.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl400OKNG_ERR.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl410OKNG_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE02"])) ? "N/A" : historyDr["JUDGE02"].ToString();
                    lbl410OKNG_ERR.BackColor = (lbl410OKNG_ERR.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl410OKNG_ERR.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl420OKNG_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE04"])) ? "N/A" : historyDr["JUDGE04"].ToString();
                    lbl420OKNG_ERR.BackColor = (lbl420OKNG_ERR.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl420OKNG_ERR.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl430OKNG_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE09"])) ? "N/A" : historyDr["JUDGE09"].ToString();
                    lbl430OKNG_ERR.BackColor = (lbl430OKNG_ERR.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl430OKNG_ERR.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl440OKNG_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE24"])) ? "N/A" : historyDr["JUDGE24"].ToString();
                    lbl440OKNG_ERR.BackColor = (lbl440OKNG_ERR.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl440OKNG_ERR.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl450OKNG_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE25"])) ? "N/A" : historyDr["JUDGE25"].ToString();
                    lbl450OKNG_ERR.BackColor = (lbl450OKNG_ERR.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl450OKNG_ERR.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl460OKNG_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE26"])) ? "N/A" : historyDr["JUDGE26"].ToString();
                    lbl460OKNG_ERR.BackColor = (lbl460OKNG_ERR.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl460OKNG_ERR.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;

                    lblData01_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA01"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA01"].ToString()));
                    lblData02_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA02"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA02"].ToString()));
                    lblData03_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA03"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA03"].ToString()));
                    lblData04_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA04"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA04"].ToString()));
                    lblData05_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA05"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA05"].ToString()));
                    lblData06_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA06"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA06"].ToString()));
                    lblData07_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA07"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA07"].ToString()));
                    lblData08_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA08"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA08"].ToString()));
                    lblData09_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA09"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA09"].ToString()));
                    lblData10_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA10"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA10"].ToString()));
                    lblData11_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA11"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA11"].ToString()));
                    lblData12_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA12"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA12"].ToString()));
                    lblData13_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA13"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA13"].ToString()));
                    lblData14_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA14"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA14"].ToString()));
                    lblData15_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA15"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA15"].ToString()));
                    lblData16_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA16"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA16"].ToString()));
                    lblData17_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA17"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA17"].ToString()));
                    lblData18_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA18"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA18"].ToString()));
                    lblData19_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA19"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA19"].ToString()));
                    lblData20_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA20"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA20"].ToString()));
                    lblData21_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA21"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA21"].ToString()));
                    lblData22_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA22"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA22"].ToString()));
                    lblData23_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA23"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA23"].ToString()));
                    lblData24_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA24"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA24"].ToString()));
                    lblData25_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA25"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA25"].ToString()));
                    lblData26_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA26"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA26"].ToString()));
                    lblData27_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA27"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA27"].ToString()));
                    lblData28_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA28"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA28"].ToString()));
                    lblData29_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA29"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA29"].ToString()));
                    lblData30_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA30"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA30"].ToString()));
                    lblData31_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA31"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA31"].ToString()));
                    lblData32_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA32"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA32"].ToString()));
                    lblData33_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA33"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA33"].ToString()));
                    lblData34_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA34"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA34"].ToString()));
                    lblData35_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA35"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA35"].ToString()));
                    lblData36_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA36"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA36"].ToString()));

                    lblLastEventTime01_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME01"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME01"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime02_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME02"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME02"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime03_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME03"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME03"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime04_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME04"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME04"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime05_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME05"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME05"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime06_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME06"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME06"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime07_ERR.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME07"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME07"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        /// <summary>
        /// Ing grid view focused row changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewIng_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridViewIng.GetFocusedDataRow();
            DataRow historyDr = null;

            if (dr != null)
            {
                if (_IngDt != null && _IngDt.Rows.Count > 0)
                {
                    historyDr = _IngDt.AsEnumerable().Where(t => t.Field<string>("LOT_NO") == dr["LOT_NO"].ToString()).FirstOrDefault();
                }

                if (historyDr != null)
                {
                    lbl400OKNG_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE01"])) ? "N/A" : historyDr["JUDGE01"].ToString();
                    lbl400OKNG_ING.BackColor = (lbl400OKNG_ING.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl400OKNG_ING.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl410OKNG_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE02"])) ? "N/A" : historyDr["JUDGE02"].ToString();
                    lbl410OKNG_ING.BackColor = (lbl410OKNG_ING.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl410OKNG_ING.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl420OKNG_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE04"])) ? "N/A" : historyDr["JUDGE04"].ToString();
                    lbl420OKNG_ING.BackColor = (lbl420OKNG_ING.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl420OKNG_ING.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl430OKNG_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE09"])) ? "N/A" : historyDr["JUDGE09"].ToString();
                    lbl430OKNG_ING.BackColor = (lbl430OKNG_ING.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl430OKNG_ING.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl440OKNG_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE24"])) ? "N/A" : historyDr["JUDGE24"].ToString();
                    lbl440OKNG_ING.BackColor = (lbl440OKNG_ING.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl440OKNG_ING.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl450OKNG_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE25"])) ? "N/A" : historyDr["JUDGE25"].ToString();
                    lbl450OKNG_ING.BackColor = (lbl450OKNG_ING.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl450OKNG_ING.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;
                    lbl460OKNG_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["JUDGE26"])) ? "N/A" : historyDr["JUDGE26"].ToString();
                    lbl460OKNG_ING.BackColor = (lbl460OKNG_ING.Text == "OK") ? Color.FromArgb(103, 228, 119) : (lbl460OKNG_ING.Text == "NG") ? Color.FromArgb(242, 119, 122) : Color.Transparent;

                    lblData01_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA01"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA01"].ToString()));
                    lblData02_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA02"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA02"].ToString()));
                    lblData03_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA03"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA03"].ToString()));
                    lblData04_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA04"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA04"].ToString()));
                    lblData05_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA05"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA05"].ToString()));
                    lblData06_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA06"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA06"].ToString()));
                    lblData07_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA07"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA07"].ToString()));
                    lblData08_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA08"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA08"].ToString()));
                    lblData09_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA09"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA09"].ToString()));
                    lblData10_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA10"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA10"].ToString()));
                    lblData11_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA11"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA11"].ToString()));
                    lblData12_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA12"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA12"].ToString()));
                    lblData13_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA13"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA13"].ToString()));
                    lblData14_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA14"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA14"].ToString()));
                    lblData15_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA15"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA15"].ToString()));
                    lblData16_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA16"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA16"].ToString()));
                    lblData17_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA17"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA17"].ToString()));
                    lblData18_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA18"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA18"].ToString()));
                    lblData19_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA19"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA19"].ToString()));
                    lblData20_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA20"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA20"].ToString()));
                    lblData21_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA21"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA21"].ToString()));
                    lblData22_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA22"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA22"].ToString()));
                    lblData23_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA23"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA23"].ToString()));
                    lblData24_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA24"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA24"].ToString()));
                    lblData25_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA25"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA25"].ToString()));
                    lblData26_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA26"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA26"].ToString()));
                    lblData27_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA27"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA27"].ToString()));
                    lblData28_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA28"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA28"].ToString()));
                    lblData29_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA29"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA29"].ToString()));
                    lblData30_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA30"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA30"].ToString()));
                    lblData31_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA31"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA31"].ToString()));
                    lblData32_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA32"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA32"].ToString()));
                    lblData33_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA33"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA33"].ToString()));
                    lblData34_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA34"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA34"].ToString()));
                    lblData35_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA35"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA35"].ToString()));
                    lblData36_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["DATA36"])) ? string.Empty : ConvertDecimalToString(decimal.Parse(historyDr["DATA36"].ToString()));

                    lblLastEventTime01_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME01"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME01"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime02_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME02"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME02"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime03_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME03"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME03"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime04_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME04"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME04"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime05_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME05"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME05"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime06_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME06"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME06"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    lblLastEventTime07_ING.Text = string.IsNullOrEmpty(string.Format("{0}", historyDr["LASTEVENT_TIME07"])) ? string.Empty : DateTime.Parse(historyDr["LASTEVENT_TIME07"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        /// <summary>
        /// Complete grid view popupmenu showing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewComplete_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.Menu == null) return;

                DXMenuItem dXMenuItem = new DevExpress.Utils.Menu.DXMenuItem("엑셀 내보내기", new EventHandler(this.exportCompleteMenu_Click));
                e.Menu.Items.Add(dXMenuItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Error grid view popupmenu showing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewError_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.Menu == null) return;

                DXMenuItem dXMenuItem = new DevExpress.Utils.Menu.DXMenuItem("엑셀 내보내기", new EventHandler(this.exportErrorMenu_Click));
                e.Menu.Items.Add(dXMenuItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Ing grid view popupmenu showing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewIng_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.Menu == null) return;

                DXMenuItem dXMenuItem = new DevExpress.Utils.Menu.DXMenuItem("엑셀 내보내기", new EventHandler(this.exportIngMenu_Click));
                e.Menu.Items.Add(dXMenuItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Data grid view poupmenu showing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bandedGridView1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.Menu == null) return;

                DXMenuItem dXMenuItem = new DevExpress.Utils.Menu.DXMenuItem("엑셀 내보내기", new EventHandler(this.exportDataMenu_Click));
                e.Menu.Items.Add(dXMenuItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bandedGridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (e.RowHandle >= 0)
            {
                string mode = gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["FLAG"]);

                if (mode == "불합격")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 196, 196);
                }
                else if(mode == "공정 불합격")
                {
                    e.Appearance.BackColor = Color.FromArgb(242, 120, 120);
                }
                else if (mode == "진행중")
                {
                    e.Appearance.BackColor = Color.FromArgb(200, 255, 120);
                }
                else if (mode == "합격")
                {
                    e.Appearance.BackColor = Color.FromArgb(112, 146, 190);
                }
            }
        }

        /// <summary>
        /// Sig sigma level button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSixsigmaLevel_Click(object sender, EventArgs e)
        {
            bool bFlag = false;

            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "SixSigmaLevel")
                {
                    bFlag = true;
                }
            }

            if (!bFlag)
            {
                SixSigmaLevel sixSigmaLevel = new SixSigmaLevel();
                sixSigmaLevel.Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton simpleButton = sender as DevExpress.XtraEditors.SimpleButton;

            if (simpleButton.Text == "전체선택")
            {
                chk14.Checked = true;
                chk15.Checked = true;
                chk16.Checked = true;
                chk17.Checked = true;
                chk18.Checked = true;
                chk19.Checked = true;
                chk20.Checked = true;
                chk21.Checked = true;
                chk22.Checked = true;
                chk23.Checked = true;
                chk24.Checked = true;
                chk25.Checked = true;

                simpleButton.Text = "전체해제";
            }
            else
            {
                chk14.Checked = false;
                chk15.Checked = false;
                chk16.Checked = false;
                chk17.Checked = false;
                chk18.Checked = false;
                chk19.Checked = false;
                chk20.Checked = false;
                chk21.Checked = false;
                chk22.Checked = false;
                chk23.Checked = false;
                chk24.Checked = false;
                chk25.Checked = false;

                simpleButton.Text = "전체선택";
            }
        }

        /// <summary>
        /// Measure code combobox selected value changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMeasureCode_SelectedValueChanged(object sender, EventArgs e)
        {
            BindMeanLineChart();
            BindMeanHistogramChart();
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

            _SpecDt = GetItemSpecData();

            cmbMeasureCode.SelectedIndex = 0;
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            if (_SpecDt != null && _SpecDt.Rows.Count > 0)
            {
                _MeasureDt = null;

                if (cmbMeasureCode.Properties.Items != null && cmbMeasureCode.Properties.Items.Count > 0)
                {
                    cmbMeasureCode.Properties.Items.Clear();
                }

                for (int i = 1; i <= _SpecDt.Rows.Count; i++)
                {
                    cmbMeasureCode.Properties.Items.Add(_SpecDt.Rows[i - 1]["INSP_NAME"].ToString());
                }

                cmbMeasureCode.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Initialize grid contril count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listArray"></param>
        /// <param name="colName"></param>
        /// <param name="captionName"></param>
        private void InitializeGridControlCount<T>(List<T> listArray, string colName, string captionName)
        {
            if (gridControlCount.DataSource != null)
            {
                gridControlCount.DataSource = null;
            }

            if (gridViewCount.Columns != null && gridViewCount.Columns.Count > 0)
            {
                gridViewCount.Columns.Clear();
            }

            gridViewCount.Columns.AddField("Val");
            gridViewCount.Columns["Val"].VisibleIndex = 0;
            gridViewCount.Columns["Val"].Caption = captionName;
            gridViewCount.Columns["Val"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridViewCount.Columns["Val"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridViewCount.Columns["Val"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridViewCount.Columns["Val"].DisplayFormat.FormatString = "#,#0.000";

            gridViewCount.Columns.AddField("Count");
            gridViewCount.Columns["Count"].VisibleIndex = 1;
            gridViewCount.Columns["Count"].Caption = "빈도";
            gridViewCount.Columns["Count"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridViewCount.Columns["Count"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridViewCount.Columns["Count"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridViewCount.Columns["Count"].DisplayFormat.FormatString = "#,##";

            gridViewCount.BeginSort();
            gridControlCount.DataSource = listArray;

            gridViewCount.Columns["Val"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Count, "Val", "전체 행:{0:#,###}건"));

            gridViewCount.ClearSorting();
            gridViewCount.Columns["Val"].SortOrder = ColumnSortOrder.Ascending;
            gridViewCount.EndSort();
            gridViewCount.BestFitColumns();
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.Disposed += new EventHandler(QM0406_Disposed);

            gridViewComplete.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewComplete_FocusedRowChanged);
            gridViewError.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewError_FocusedRowChanged);
            gridViewIng.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewIng_FocusedRowChanged);

            gridViewComplete.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewComplete_PopupMenuShowing);
            gridViewError.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewError_PopupMenuShowing);
            gridViewIng.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewIng_PopupMenuShowing);
            bandedGridView1.PopupMenuShowing += new PopupMenuShowingEventHandler(bandedGridView1_PopupMenuShowing);
            bandedGridView1.RowStyle += new RowStyleEventHandler(bandedGridView1_RowStyle);
            
            btnSixsigmaLevel.Click += new EventHandler(btnSixsigmaLevel_Click);
            btnSelect.Click += new EventHandler(btnSelect_Click);

            cmbMeasureCode.SelectedValueChanged += new EventHandler(cmbMeasureCode_SelectedValueChanged);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(QM0406_Disposed);

            gridViewComplete.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewComplete_FocusedRowChanged);
            gridViewError.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewError_FocusedRowChanged);
            gridViewIng.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewIng_FocusedRowChanged);

            gridViewComplete.PopupMenuShowing -= new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewComplete_PopupMenuShowing);
            gridViewError.PopupMenuShowing -= new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewError_PopupMenuShowing);
            gridViewIng.PopupMenuShowing -= new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewIng_PopupMenuShowing);
            bandedGridView1.PopupMenuShowing -= new PopupMenuShowingEventHandler(bandedGridView1_PopupMenuShowing);
            bandedGridView1.RowStyle -= new RowStyleEventHandler(bandedGridView1_RowStyle);

            btnSixsigmaLevel.Click -= new EventHandler(btnSixsigmaLevel_Click);
            btnSelect.Click -= new EventHandler(btnSelect_Click);

            cmbMeasureCode.SelectedValueChanged -= new EventHandler(cmbMeasureCode_SelectedValueChanged);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            using (DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("잠시만 기다려주세요!", "데이터 로딩 중입니다...", new Size(200, 50), ParentForm))
            {
                BindCompleteDataList();
                BindErrorDataList();
                BindIngDataList();
                BindData();
                BinChartData();

                BindMeanLineChart();
                BindMeanHistogramChart();
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
        /// Bind complete data list
        /// </summary>
        private void BindCompleteDataList()
        {
            if (gridViewComplete.Columns["LASTEVENT_TIME"].Summary.Count > 0) gridViewComplete.Columns["LASTEVENT_TIME"].Summary.Clear();

            _CompleteDt = GetCompleteHistoryData();

            if (_CompleteDt != null && _CompleteDt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("LOT_NO", typeof(string));
                dt.Columns.Add("LASTEVENT_TIME", typeof(DateTime));

                foreach (DataRow dr in _CompleteDt.Rows)
                {
                    DataRow tempDr = dt.NewRow();
                    tempDr["LOT_NO"] = dr["LOT_NO"];
                    tempDr["LASTEVENT_TIME"] = dr["LASTEVENT_TIME"];

                    dt.Rows.Add(tempDr);
                }

                dt.DefaultView.Sort = string.Format("{0} {1}", "LASTEVENT_TIME", "DESC");
                dt.AcceptChanges();

                gridControlComplete.DataSource = dt;

                gridViewComplete.Columns["LASTEVENT_TIME"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Count, "LASTEVENT_TIME", "전체 행:{0:#,###}건"));
            }
            else
            {
                gridControlComplete.DataSource = null;
                ClearCompleteControl();
            }
        }

        /// <summary>
        /// Bind error data list
        /// </summary>
        private void BindErrorDataList()
        {
            if (gridViewError.Columns["LASTEVENT_TIME01"].Summary.Count > 0) gridViewError.Columns["LASTEVENT_TIME01"].Summary.Clear();

            _ErrorDt = GetErrorHistoryData();

            if (_ErrorDt != null && _ErrorDt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("LOT_NO", typeof(string));
                dt.Columns.Add("LASTEVENT_TIME01", typeof(DateTime));

                foreach (DataRow dr in _ErrorDt.Rows)
                {
                    DataRow tempDr = dt.NewRow();
                    tempDr["LOT_NO"] = dr["LOT_NO"];
                    tempDr["LASTEVENT_TIME01"] = dr["LASTEVENT_TIME01"];

                    dt.Rows.Add(tempDr);
                }

                dt.DefaultView.Sort = string.Format("{0} {1}", "LASTEVENT_TIME01", "DESC");
                dt.AcceptChanges();

                gridControlError.DataSource = dt;

                gridViewError.Columns["LASTEVENT_TIME01"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Count, "LASTEVENT_TIME", "전체 행:{0:#,###}건"));
            }
            else
            {
                gridControlError.DataSource = null;
                ClearErrorControl();
            }
        }

        /// <summary>
        /// Bind ing data list
        /// </summary>
        private void BindIngDataList()
        {
            if (gridViewIng.Columns["LASTEVENT_TIME01"].Summary.Count > 0) gridViewIng.Columns["LASTEVENT_TIME01"].Summary.Clear();

            _IngDt = GetIngHistoryData();

            if (_IngDt != null && _IngDt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("LOT_NO", typeof(string));
                dt.Columns.Add("LASTEVENT_TIME01", typeof(DateTime));

                foreach (DataRow dr in _IngDt.Rows)
                {
                    DataRow tempDr = dt.NewRow();
                    tempDr["LOT_NO"] = dr["LOT_NO"];
                    tempDr["LASTEVENT_TIME01"] = dr["LASTEVENT_TIME01"];

                    dt.Rows.Add(tempDr);
                }
                
                dt.DefaultView.Sort = string.Format("{0} {1}", "LASTEVENT_TIME01", "DESC");
                dt.AcceptChanges();

                gridControlIng.DataSource = dt;

                gridViewIng.Columns["LASTEVENT_TIME01"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Count, "LASTEVENT_TIME", "전체 행:{0:#,###}건"));
            }
            else
            {
                gridControlIng.DataSource = null;
                ClearIngControl();
            }
        }

        /// <summary>
        /// Bind chart data
        /// </summary>
        private void BinChartData()
        {
            List<string> chkList = GetChkList();

            if (chkList == null)
            {
                return;
            }

            _MeasureDt = GetMeasureData();

            if (_MeasureDt != null && _MeasureDt.Rows.Count > 0)
            {
                #region Chart

                XYDiagram tempXYDiagram = chartMeasureData.Diagram as XYDiagram;

                if (tempXYDiagram != null && tempXYDiagram.Panes.Count > 0)
                {
                    tempXYDiagram.Panes.Clear();
                    tempXYDiagram.SecondaryAxesY.Clear();
                }

                if (chartMeasureData.Series.Count > 0)
                {
                    chartMeasureData.Series.Clear();
                }

                chartMeasureData.AutoLayout = false;
                chartMeasureData.CrosshairOptions.ArgumentLineColor = System.Drawing.Color.DeepSkyBlue;
                chartMeasureData.CrosshairOptions.ArgumentLineStyle.Thickness = 2;
                chartMeasureData.CrosshairOptions.ShowOnlyInFocusedPane = false;

                chartMeasureData.CrosshairOptions.GroupHeaderPattern = "<b>{A:yyyy-MM-dd HH:mm:ss}</b>";

                chartMeasureData.CrosshairOptions.ShowValueLabels = true;
                chartMeasureData.CrosshairOptions.ShowValueLine = true;
                chartMeasureData.CrosshairOptions.ArgumentLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                chartMeasureData.CrosshairOptions.ValueLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

                List<Series> seriesList = GetSeriesList(chkList, _MeasureDt);

                #endregion
            }
            else
            {
                chartMeasureData.Series.Clear();
            }
        }

        /// <summary>
        /// Bind data
        /// </summary>
        private void BindData()
        {
            if (bandedGridView1.Columns["LOT_NO"].Summary.Count > 0) bandedGridView1.Columns["LOT_NO"].Summary.Clear();

            _DataDt = GetData();

            if (_DataDt != null && _DataDt.Rows.Count > 0)
            {
                gridControlData.DataSource = _DataDt;
                bandedGridView1.Columns["LOT_NO"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Count, "LOT_NO", "전체 행:{0:#,###}건"));
            }
            else
            {
                gridControlIng.DataSource = null;
                ClearIngControl();
            }
        }

        /// <summary>
        /// Bind mean line chart
        /// </summary>
        private void BindMeanLineChart()
        {
            string measureCode = cmbMeasureCode.Text;

            chartMeanLine.Series.Clear();

            if (_MeasureDt != null && _MeasureDt.Rows.Count > 0)
            {
                decimal usl = 0;
                decimal lsl = 0;

                if (_SpecDt != null && _SpecDt.Rows.Count > 0)
                {
                    usl = _SpecDt.AsEnumerable().Where(t => t.Field<string>("INSP_NAME") == measureCode).Select(t => t.Field<decimal>("UCL")).FirstOrDefault();
                    lsl = _SpecDt.AsEnumerable().Where(t => t.Field<string>("INSP_NAME") == measureCode).Select(t => t.Field<decimal>("LCL")).FirstOrDefault();
                }

                string colName = bandedGridView1.Columns.Where(t => t.Caption == measureCode).Select(t => t.FieldName).FirstOrDefault();

                BindCPKData(colName);

                decimal minVal = _MeasureDt.AsEnumerable().Select(t => t.Field<decimal>(colName)).Min();
                decimal maxVal = _MeasureDt.AsEnumerable().Select(t => t.Field<decimal>(colName)).Max();

                if (minVal > lsl)
                {
                    minVal = lsl;
                }

                if (maxVal < usl)
                {
                    maxVal = usl;
                }

                minVal = minVal - decimal.Parse("0.001");
                maxVal = maxVal + decimal.Parse("0.001");

                if (!string.IsNullOrEmpty(colName))
                {
                    Series series = new Series(string.Format("{0} MEAN", measureCode), ViewType.Point);

                    PointSeriesView pointSeriesView = (PointSeriesView)series.View;
                    pointSeriesView.PointMarkerOptions.Kind = MarkerKind.Circle;
                    pointSeriesView.PointMarkerOptions.Size = 7;

                    series.Points.BeginUpdate();

                    foreach (DataRow dr in _MeasureDt.Rows)
                    {
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dr["LASTEVENT_TIME"].ToString()), decimal.Parse(dr[colName].ToString())));
                    }

                    LineChartOption(chartMeanLine, series, lsl, usl, minVal, maxVal, measureCode, true);

                    series.Points.EndUpdate();
                }
            }
            else
            {
                chartMeanLine.Series.Clear();
            }
        }

        /// <summary>
        /// Bind mead histogram chart
        /// </summary>
        private void BindMeanHistogramChart()
        {
            string measureCode = cmbMeasureCode.Text;

            decimal usl = 0;
            decimal lsl = 0;

            if (_SpecDt != null && _SpecDt.Rows.Count > 0)
            {
                usl = _SpecDt.AsEnumerable().Where(t => t.Field<string>("INSP_NAME") == measureCode).Select(t => t.Field<decimal>("UCL")).FirstOrDefault();
                lsl = _SpecDt.AsEnumerable().Where(t => t.Field<string>("INSP_NAME") == measureCode).Select(t => t.Field<decimal>("LCL")).FirstOrDefault();
            }

            chartMeanHistogram.Series.Clear();

            if (_MeasureDt != null && _MeasureDt.Rows.Count > 0)
            {
                string colName = bandedGridView1.Columns.Where(t => t.Caption == measureCode).Select(t => t.FieldName).FirstOrDefault();

                var results = from row in _MeasureDt.AsEnumerable()
                              group row by new { Val = row.Field<decimal>(colName) } into grp

                              select new
                              {
                                  Val = grp.Key.Val,
                                  Count = grp.Count()
                              };

                decimal xMinVal = results.AsEnumerable().Select(t => t.Val).Min();
                decimal xMaxVal = results.AsEnumerable().Select(t => t.Val).Max();
                int yMinVal = results.AsEnumerable().Select(t => t.Count).Min();
                int yMaxVal = results.AsEnumerable().Select(t => t.Count).Max();

                InitializeGridControlCount(results.ToList(), colName, measureCode);

                if (results != null)
                {
                    foreach (var item in results)
                    {
                        Series series = new Series(item.Val.ToString(), ViewType.Bar);
                        series.Points.Add(new SeriesPoint(item.Val, item.Count));
                        series.ArgumentScaleType = ScaleType.Auto;
                        series.ValueScaleType = ScaleType.Numerical;
                        series.CrosshairLabelPattern = "<b>{A} : {V}</b>";
                        SideBySideBarSeriesView sideBySideBarSeriesView = new SideBySideBarSeriesView();
                        sideBySideBarSeriesView.BarWidth = 0.02;
                        series.View = sideBySideBarSeriesView;
                        chartMeanHistogram.Series.Add(series);
                    }

                    BarChartOption(chartMeanHistogram, lsl, usl, yMinVal, yMaxVal, measureCode);
                }
            }
        }

        /// <summary>
        /// Bar chart option
        /// </summary>
        /// <param name="chartControl"></param>
        /// <param name="xMinVal"></param>
        /// <param name="xMaxVal"></param>
        /// <param name="yMinVal"></param>
        /// <param name="yMaxVal"></param>
        /// <param name="measureCode"></param>
        private void BarChartOption(ChartControl chartControl, decimal xMinVal, decimal xMaxVal, int yMinVal, int yMaxVal, string measureCode)
        {
            chartControl.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            XYDiagram diagram = chartControl.Diagram as XYDiagram;

            if (diagram != null)
            {
                ConstantLine xBarLine = new ConstantLine();
                xBarLine.Name = string.Format("{0} : {1}", "XBAR", ((xMinVal + xMaxVal) / 2).ToString());
                xBarLine.AxisValueSerializable = ((xMinVal + xMaxVal) / 2).ToString();

                diagram.AxisX.VisibleInPanesSerializable = "-1";
                diagram.AxisX.Label.TextPattern = "{V:F3}";
                diagram.AxisX.WholeRange.Auto = false;
                diagram.AxisX.VisualRange.AutoSideMargins = false;
                diagram.AxisX.WholeRange.SideMarginsValue = 0;
                diagram.AxisX.WholeRange.SetMinMaxValues(xMinVal, xMaxVal);
                diagram.AxisY.Label.TextPattern = "{V}";
                diagram.AxisY.VisibleInPanesSerializable = "-1";
                diagram.AxisY.WholeRange.Auto = false;
                diagram.AxisY.WholeRange.SideMarginsValue = 0;
                diagram.AxisY.VisualRange.AutoSideMargins = false;
                diagram.AxisY.WholeRange.SetMinMaxValues(yMinVal, yMaxVal);

                if (diagram.AxisY.ConstantLines != null)
                {
                    diagram.AxisY.ConstantLines.Clear();
                }

                if (diagram.AxisX.ConstantLines != null)
                {
                    diagram.AxisX.ConstantLines.Clear();
                }

                diagram.AxisX.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] { xBarLine });

                diagram.EnableAxisXScrolling = true;
                diagram.EnableAxisXZooming = true;
                diagram.EnableAxisYScrolling = true;
                diagram.EnableAxisYZooming = true;
            }
        }

        /// <summary>
        /// Bind CPK data
        /// </summary>
        /// <param name="sFiledName"></param>
        private void BindCPKData(string sFiledName)
        {
            double usl = 0f;
            double lsl = 0f;

            double min = 0f;
            double max = 0f;
            double ave = 0f;
            double stDev = 0f;
            double var = 0f;
            double cp = 0f;
            double cpu = 0f;
            double cpl = 0f;
            double k = 0f;
            double cpk = 0f;

            string measureCode = cmbMeasureCode.Text;

            double[] valDecList = null;

            if (_SpecDt != null && _SpecDt.Rows.Count > 0)
            {
                usl = _SpecDt.AsEnumerable().Where(t => t.Field<string>("INSP_NAME") == measureCode).Select(t => double.Parse(t.Field<decimal>("UCL").ToString())).FirstOrDefault();
                lsl = _SpecDt.AsEnumerable().Where(t => t.Field<string>("INSP_NAME") == measureCode).Select(t => double.Parse(t.Field<decimal>("LCL").ToString())).FirstOrDefault();
            }

            if (_MeasureDt != null && _MeasureDt.Rows.Count > 0)
            {
                min = _MeasureDt.AsEnumerable().Select(t => (double.Parse(t.Field<decimal>(sFiledName).ToString()))).Min();
                max = _MeasureDt.AsEnumerable().Select(t => (double.Parse(t.Field<decimal>(sFiledName).ToString()))).Max();
                valDecList = _MeasureDt.AsEnumerable().Select(t => (double.Parse(t.Field<decimal>(sFiledName).ToString()))).ToArray();

                ave = GetAverage(valDecList, sFiledName);
                stDev = GetStDev(valDecList, ave);
                var = GetVariance(valDecList, ave);
                cp = GetCP(usl, lsl, stDev);
                cpu = GetCPU(usl, ave, stDev);
                cpl = GetCPL(ave, lsl, stDev);
                k = GetK(usl, lsl, ave);
                cpk = GetCPK(k, cp);
            }

            lblMeasureCodeData.Text = cmbMeasureCode.Text;

            lblMinData.Text = min.ToString();
            lblMaxData.Text = max.ToString();
            lblLSLData.Text = lsl.ToString();
            lblUSLData.Text = usl.ToString();
            lblAveData.Text = ave.ToString("F3");
            lblStDevData.Text = stDev.ToString("F3");
            lblVarianceData.Text = var.ToString("F3");
            lblCPData.Text = cp.ToString("F3");
            lblCPUData.Text = cpu.ToString("F3");
            lblCPLData.Text = cpl.ToString("F3");
            lblCPKData.Text = cpk.ToString("F3");
        }

        /// <summary>
        /// Get check list
        /// </summary>
        /// <returns></returns>
        private List<string> GetChkList()
        {
            List<string> chkList = new List<string>();

            if (chk14.Checked) { chkList.Add("DATA14"); }
            if (chk15.Checked) { chkList.Add("DATA15"); }
            if (chk16.Checked) { chkList.Add("DATA16"); }
            if (chk17.Checked) { chkList.Add("DATA17"); }
            if (chk18.Checked) { chkList.Add("DATA18"); }
            if (chk19.Checked) { chkList.Add("DATA19"); }
            if (chk20.Checked) { chkList.Add("DATA20"); }
            if (chk21.Checked) { chkList.Add("DATA21"); }
            if (chk22.Checked) { chkList.Add("DATA22"); }
            if (chk23.Checked) { chkList.Add("DATA23"); }
            if (chk24.Checked) { chkList.Add("DATA24"); }
            if (chk25.Checked) { chkList.Add("DATA25"); }

            return chkList;
        }

        /// <summary>
        /// Get series list
        /// </summary>
        /// <param name="chkList"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<Series> GetSeriesList(List<string> chkList, DataTable dt)
        {
            List<Series> seriesList = new List<Series>();

            string typeName = string.Empty;
            string labelTextPattern = string.Empty;
            decimal decUsl;
            decimal decLsl;
            float usl = 0f;
            float lsl = 0f;

            foreach (string type in chkList)
            {
                switch (type)
                {
                    case "DATA14":
                        typeName = "ID 1";
                        labelTextPattern = "{V:F3}";
                        usl = 64.019f;
                        lsl = 64.000f;
                        break;

                    case "DATA15":
                        typeName = "ID 2";
                        labelTextPattern = "{V:F3}";
                        usl = 55.959f;
                        lsl = 55.931f;
                        break;

                    case "DATA16":
                        typeName = "ID 3";
                        labelTextPattern = "{V:F3}";
                        usl = 65.046f;
                        lsl = 65.000f;
                        break;

                    case "DATA17":
                        typeName = "ID 4";
                        labelTextPattern = "{V:F3}";
                        usl = 79.974f;
                        lsl = 79.955f;
                        break;

                    case "DATA18":
                        typeName = "ID 5";
                        labelTextPattern = "{V:F3}";
                        usl = 57.974f;
                        lsl = 57.955f;
                        break;

                    case "DATA19":
                        typeName = "DEPTH 1";
                        labelTextPattern = "{V:F3}";
                        usl = 15.600f;
                        lsl = 15.400f;
                        break;

                    case "DATA20":
                        typeName = "DEPTH 2";
                        labelTextPattern = "{V:F3}";
                        usl = 78.600f;
                        lsl = 78.500f;
                        break;

                    case "DATA21":
                        typeName = "DEPTH 3";
                        labelTextPattern = "{V:F3}";
                        usl = 78.600f;
                        lsl = 78.500f;
                        break;

                    case "DATA22":
                        typeName = "DEPTH 4";
                        labelTextPattern = "{V:F3}";
                        usl = 37.900f;
                        lsl = 37.800f;
                        break;

                    case "DATA23":
                        typeName = "DEPTH 5";
                        labelTextPattern = "{V:F3}";
                        usl = 37.900f;
                        lsl = 37.800f;
                        break;

                    case "DATA24":
                        typeName = "FLATNESS";
                        labelTextPattern = "{V:F3}";
                        usl = 0.05f;
                        lsl = 0f;
                        break;

                    case "DATA25":
                        typeName = "PARALLELISM";
                        labelTextPattern = "{V:F3}";
                        usl = 0.05f;
                        lsl = 0f;
                        break;

                    default:
                        break;
                }

                decUsl = decimal.Parse(usl.ToString());
                decLsl = decimal.Parse(lsl.ToString());
                decimal minVal = dt.AsEnumerable().Select(t => t.Field<decimal>(type)).Min();
                decimal maxVal = dt.AsEnumerable().Select(t => t.Field<decimal>(type)).Max();

                if (labelTextPattern == "{V:F0}")
                {
                    minVal = minVal - decimal.Parse("1");
                    maxVal = maxVal + decimal.Parse("1");
                }
                else if (labelTextPattern == "{V:F1}")
                {
                    minVal = minVal - decimal.Parse("0.1");
                    maxVal = maxVal + decimal.Parse("0.1");
                }
                else if (labelTextPattern == "{V:F2}")
                {
                    minVal = minVal - decimal.Parse("0.01");
                    maxVal = maxVal + decimal.Parse("0.01");
                }
                else if (labelTextPattern == "{V:F3}")
                {
                    minVal = minVal - decimal.Parse("0.001");
                    maxVal = maxVal + decimal.Parse("0.001");
                }

                if (minVal > decLsl)
                {
                    minVal = decLsl;
                }

                if (maxVal < decUsl)
                {
                    maxVal = decUsl;
                }

                Series series = new Series(string.Format("{0}", typeName), ViewType.Line);
                series.Tag = minVal.ToString() + "|" + maxVal.ToString() + "|" + usl.ToString() + "|" + lsl.ToString() + "|" + typeName;

                series.ArgumentScaleType = ScaleType.DateTime;
                series.ValueScaleType = ScaleType.Numerical;
                series.Visible = true;

                foreach (DataRow dr in dt.AsEnumerable())
                {
                    SeriesPoint seriesPoint = new SeriesPoint(DateTime.Parse(dr["LASTEVENT_TIME"].ToString()), Decimal.Parse(dr[type].ToString()));
                    seriesPoint.Tag = string.Format("{0} => (로트번호:{1}, 라인 : {2}, 값:{3})", typeName, dr["LOT_NO"], dr["LINE"], dr[type]);

                    series.Points.Add(seriesPoint);
                }

                seriesList.Add(series);

                ((LineSeriesView)series.View).LineMarkerOptions.Size = 5;
                ((LineSeriesView)series.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Solid;
            }

            if (seriesList != null && seriesList.Count > 0)
            {
                foreach (Series series in seriesList)
                {
                    chartMeasureData.Series.Add(series);
                }

                XYDiagram diagram = chartMeasureData.Diagram as XYDiagram;

                for (int i = 1; i < seriesList.Count; i++)
                {
                    LineChartOption(diagram, seriesList[i], labelTextPattern,
                        decimal.Parse(seriesList[i].Tag.ToString().Split('|')[2]),
                        decimal.Parse(seriesList[i].Tag.ToString().Split('|')[3]));
                }

                List<string> valList = seriesList[0].Tag.ToString().Split('|').ToList();

                diagram.EnableAxisXScrolling = true;
                diagram.EnableAxisXZooming = true;
                diagram.EnableAxisYScrolling = true;
                diagram.EnableAxisYZooming = true;

                diagram.AxisX.Range.Auto = true;
                diagram.AxisY.Range.Auto = true;
                diagram.AxisX.Range.ScrollingRange.Auto = true;
                diagram.AxisY.Range.ScrollingRange.Auto = true;

                diagram.AxisX.DateTimeScaleOptions.ScaleMode = DevExpress.XtraCharts.ScaleMode.Continuous;
                diagram.AxisX.DateTimeScaleOptions.AutoGrid = true;
                diagram.AxisX.Label.TextPattern = "{A:yyyy-MM-dd HH:mm}";
                diagram.AxisY.Label.TextPattern = labelTextPattern;
                diagram.AxisY.WholeRange.Auto = false;
                diagram.AxisX.Label.Staggered = true;
                diagram.AxisY.WholeRange.SetMinMaxValues(decimal.Parse(valList[0]), decimal.Parse(valList[1]));

                diagram.AxisX.GridLines.Visible = true;
                diagram.AxisX.Interlaced = true;
                diagram.AxisX.Label.Staggered = true;
                diagram.AxisX.Title.Text = "Date";

                diagram.AxisX.VisibleInPanesSerializable = (chkList.Count - 2).ToString();
                diagram.AxisX.VisualRange.Auto = false;
                diagram.AxisY.GridLines.MinorVisible = true;
                diagram.AxisY.Title.Text = valList[4];
                diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.VisibleInPanesSerializable = "-1";
                diagram.AxisY.Title.Font = new System.Drawing.Font("Tahoma", 9F);

                //if (valList[2] != "0" && valList[3] != "0")
                //{
                    ConstantLine uclLine = new ConstantLine();
                    ConstantLine lclLine = new ConstantLine();

                    uclLine.Name = string.Format("{0} : {1}", "UCL", valList[2]);
                    lclLine.Name = string.Format("{0} : {1}", "LCL", valList[3]);
                    uclLine.AxisValueSerializable = valList[2];
                    lclLine.AxisValueSerializable = valList[3];

                    diagram.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] { uclLine, lclLine });

                    uclLine.ShowInLegend = false;
                    uclLine.Color = Color.Red;
                    uclLine.LineStyle.DashStyle = DashStyle.Dash;
                    uclLine.LineStyle.Thickness = 2;

                    lclLine.ShowInLegend = false;
                    lclLine.Color = Color.Red;
                    lclLine.LineStyle.DashStyle = DashStyle.Dash;
                    lclLine.LineStyle.Thickness = 2;
                //}
            }

            return seriesList;
        }

        /// <summary>
        /// Line chart option
        /// </summary>
        /// <param name="diagram"></param>
        /// <param name="series"></param>
        /// <param name="labelTextPattern"></param>
        /// <param name="ucl"></param>
        /// <param name="lcl"></param>
        private void LineChartOption(XYDiagram diagram, Series series, string labelTextPattern, decimal ucl, decimal lcl)
        {
            ConstantLine uclLine = new ConstantLine();
            ConstantLine lclLine = new ConstantLine();

            uclLine.Name = string.Format("{0} : {1}", "UCL", ucl.ToString());
            lclLine.Name = string.Format("{0} : {1}", "LCL", lcl.ToString());
            uclLine.AxisValueSerializable = ucl.ToString();
            lclLine.AxisValueSerializable = lcl.ToString();

            List<string> valList = series.Tag.ToString().Split('|').ToList();
            LineSeriesView lineSeriesView = (LineSeriesView)series.View;

            diagram.Panes.Add(new XYDiagramPane(series.Name));

            SecondaryAxisY secondaryAxisY = new SecondaryAxisY();
            secondaryAxisY.WholeRange.SetMinMaxValues(decimal.Parse(valList[0]), decimal.Parse(valList[1]));
            secondaryAxisY.Label.TextPattern = labelTextPattern;
            secondaryAxisY.Name = valList[4];
            secondaryAxisY.VisualRange.Auto = false;
            secondaryAxisY.Alignment = AxisAlignment.Near;
            secondaryAxisY.VisualRange.AutoSideMargins = true;
            secondaryAxisY.Title.Text = valList[4];
            secondaryAxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            secondaryAxisY.GridLines.MinorVisible = true;
            secondaryAxisY.Title.Font = new System.Drawing.Font("Tahoma", 9F);

            diagram.SecondaryAxesY.Add(secondaryAxisY);

            lineSeriesView.Pane = diagram.Panes[diagram.Panes.Count - 1];

            lineSeriesView.AxisY = secondaryAxisY;

            //if (ucl > 0 && lcl > 0)
            //{
                uclLine.ShowInLegend = false;
                uclLine.Color = Color.Red;
                uclLine.LineStyle.DashStyle = DashStyle.Dash;
                uclLine.LineStyle.Thickness = 2;

                lclLine.ShowInLegend = false;
                lclLine.Color = Color.Red;
                lclLine.LineStyle.DashStyle = DashStyle.Dash;
                lclLine.LineStyle.Thickness = 2;

                secondaryAxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] { uclLine, lclLine });
            //}
        }

        /// <summary>
        /// Line chart option
        /// </summary>
        /// <param name="chartControl"></param>
        /// <param name="series"></param>
        /// <param name="lcl"></param>
        /// <param name="ucl"></param>
        /// <param name="minVal"></param>
        /// <param name="maxVal"></param>
        /// <param name="measureCode"></param>
        /// <param name="visible"></param>
        private void LineChartOption(ChartControl chartControl, Series series, decimal lcl, decimal ucl, decimal minVal, decimal maxVal, string measureCode, bool visible)
        {
            chartControl.Series.Clear();

            series.ValueScaleType = ScaleType.Numerical;

            if (!visible)
            {
                ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Solid;
            }

            chartControl.Series.Add(series);

            ConstantLine uclLine = new ConstantLine();
            ConstantLine lclLine = new ConstantLine();
            ConstantLine xBarLine = new ConstantLine();

            uclLine.Name = string.Format("{0} : {1}", "UCL", ucl.ToString());
            lclLine.Name = string.Format("{0} : {1}", "LCL", lcl.ToString());
            uclLine.AxisValueSerializable = ucl.ToString();
            lclLine.AxisValueSerializable = lcl.ToString();

            if (visible)
            {
                xBarLine.Name = string.Format("{0} : {1}", "XBAR", ((ucl + lcl) / 2).ToString());
                xBarLine.AxisValueSerializable = ((ucl + lcl) / 2).ToString();
            }

            XYDiagram diagram = chartControl.Diagram as XYDiagram;
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

            if (!visible)
            {
                diagram.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] { uclLine, lclLine });
            }
            else
            {
                diagram.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] { uclLine, lclLine, xBarLine });
            }

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.EnableAxisYScrolling = true;
            diagram.EnableAxisYZooming = true;

            chartControl.CrosshairOptions.ShowValueLabels = true;
            chartControl.CrosshairOptions.ShowValueLine = true;
            chartControl.CrosshairOptions.GroupHeaderPattern = "<b>{A:yyyy-MM-dd HH:mm:ss}</b>";
            series.CrosshairLabelPattern = "<b>" + measureCode + " : {V:F3}</b>";

            chartControl.CrosshairOptions.ShowValueLabels = true;
            chartControl.CrosshairOptions.ShowValueLine = true;
            chartControl.CrosshairOptions.ArgumentLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartControl.CrosshairOptions.ValueLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        /// <summary>
        /// Get complete history data
        /// </summary>
        /// <returns></returns>
        public DataTable GetCompleteHistoryData()
        {
            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sPlantCode = string.IsNullOrEmpty(sPlantCode) ? "%" : sPlantCode;

            DataTable rtnDt = new DataTable();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (connectionString.Contains("192.168.50.2"))
            {
                connectionString = connectionString.Replace("192.168.50.2", "192.168.10.165");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_sqlGetCompleteHistoryData, connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "PLANT_CODE"), sPlantCode));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "WORKCENTER_CODE"), "4M54"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "FROM_DT"), string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value)));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "TO_DT"), string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1))));

                    command.Connection.Open();

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);

                    sqlDataAdapter.Fill(rtnDt);
                    rtnDt.TableName = "COMPLETE_TABLE";

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
        /// Get error history data
        /// </summary>
        /// <param name="hashtable"></param>
        /// <returns></returns>
        public DataTable GetErrorHistoryData()
        {
            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sPlantCode = string.IsNullOrEmpty(sPlantCode) ? "%" : sPlantCode;

            DataTable rtnDt = new DataTable();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (connectionString.Contains("192.168.50.2"))
            {
                connectionString = connectionString.Replace("192.168.50.2", "192.168.10.165");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_sqlErrorHistoryData, connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "PLANT_CODE"), sPlantCode));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "WORKCENTER_CODE"), "4M54"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "FROM_DT"), string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value)));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "TO_DT"), string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1))));

                    command.Connection.Open();

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);

                    sqlDataAdapter.Fill(rtnDt);
                    rtnDt.TableName = "ERROR_TABLE";

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
        /// Get ing history data
        /// </summary>
        /// <param name="hashtable"></param>
        /// <returns></returns>
        public DataTable GetIngHistoryData()
        {
            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sPlantCode = string.IsNullOrEmpty(sPlantCode) ? "%" : sPlantCode;

            DataTable rtnDt = new DataTable();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (connectionString.Contains("192.168.50.2"))
            {
                connectionString = connectionString.Replace("192.168.50.2", "192.168.10.165");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_sqlIngHistoryData, connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "PLANT_CODE"), sPlantCode));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "WORKCENTER_CODE"), "4M54"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "FROM_DT"), string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value)));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "TO_DT"), string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1))));

                    command.Connection.Open();

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);

                    sqlDataAdapter.Fill(rtnDt);
                    rtnDt.TableName = "ING_TABLE";

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
        /// Get data
        /// </summary>
        /// <param name="hashtable"></param>
        /// <returns></returns>
        public DataTable GetData()
        {
            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sPlantCode = string.IsNullOrEmpty(sPlantCode) ? "%" : sPlantCode;

            DataTable rtnDt = new DataTable();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (connectionString.Contains("192.168.50.2"))
            {
                connectionString = connectionString.Replace("192.168.50.2", "192.168.10.165");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_sqlData, connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "PLANT_CODE"), sPlantCode));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "WORKCENTER_CODE"), "4M54"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "FROM_DT"), string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value)));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "TO_DT"), string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1))));

                    command.Connection.Open();

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);

                    sqlDataAdapter.Fill(rtnDt);
                    rtnDt.TableName = "DATA_TABLE";

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
        /// Get measure data
        /// </summary>
        /// <returns></returns>
        public DataTable GetMeasureData()
        {
            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sPlantCode = string.IsNullOrEmpty(sPlantCode) ? "%" : sPlantCode;

            DataTable rtnDt = new DataTable();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (connectionString.Contains("192.168.50.2"))
            {
                connectionString = connectionString.Replace("192.168.50.2", "192.168.10.165");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_sqlMeasureData, connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "PLANT_CODE"), sPlantCode));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "WORKCENTER_CODE"), "4M54"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "FROM_DT"), string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value)));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "TO_DT"), string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1))));

                    command.Connection.Open();

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);

                    sqlDataAdapter.Fill(rtnDt);
                    rtnDt.TableName = "DATA_TABLE";

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
                connectionString = connectionString.Replace("192.168.50.2", "192.168.10.165");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_sqlGetItemSpecData, connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "PLANT_CODE"), "SK2"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "WORKCENTER_CODE"), "4M54"));
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", "ITEM_CODE"), "48230-2H000"));

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
        /// Get average
        /// </summary>
        /// <param name="valDecList"></param>
        /// <param name="sFildName"></param>
        /// <returns></returns>
        private double GetAverage(double[] valDecList, string sFildName)
        {
            return Math.Round(valDecList.Average(), 3);
        }

        /// <summary>
        /// Get standard deviation
        /// </summary>
        /// <param name="valDecList"></param>
        /// <param name="ave"></param>
        /// <returns></returns>
        private double GetStDev(double[] valDecList, double ave)
        {
            double stDev = 0;

            if (valDecList != null && valDecList.Length > 0)
            {
                stDev = valDecList.Select(t => (t - ave) * (t - ave)).Sum();
            }

            return Math.Sqrt((double)stDev / (valDecList.Length - 1));
        }

        /// <summary>
        /// Get variance
        /// </summary>
        /// <param name="valDecList"></param>
        /// <param name="ave"></param>
        /// <returns></returns>
        private double GetVariance(double[] valDecList, double ave)
        {
            double variance = 0f;

            for (int i = 0; i < valDecList.Length; i++)
            {
                variance += Math.Pow(valDecList[i] - ave, 2);
            }

            return variance;
        }

        /// <summary>
        /// Get CP
        /// </summary>
        /// <param name="usl"></param>
        /// <param name="lsl"></param>
        /// <param name="stDev"></param>
        /// <returns></returns>
        private double GetCP(double usl, double lsl, double stDev)
        {
            return (usl - lsl) / (6 * stDev);
        }

        /// <summary>
        /// Get CPU
        /// </summary>
        /// <param name="usl"></param>
        /// <param name="ave"></param>
        /// <param name="stDev"></param>
        /// <returns></returns>
        private double GetCPU(double usl, double ave, double stDev)
        {
            return (usl - ave) / (3 * stDev);
        }

        /// <summary>
        /// Get CPL
        /// </summary>
        /// <param name="ave"></param>
        /// <param name="lsl"></param>
        /// <param name="stDev"></param>
        /// <returns></returns>
        private double GetCPL(double ave, double lsl, double stDev)
        {
            return (ave - lsl) / (3 * stDev);
        }

        /// <summary>
        /// Get K
        /// </summary>
        /// <param name="usl"></param>
        /// <param name="lsl"></param>
        /// <param name="ave"></param>
        /// <returns></returns>
        private double GetK(double usl, double lsl, double ave)
        {
            return ((usl + lsl) / 2 - ave) / ((usl - lsl) / 2);
        }

        /// <summary>
        /// Get CPK
        /// </summary>
        /// <param name="k"></param>
        /// <param name="cp"></param>
        /// <returns></returns>
        private double GetCPK(double k, double cp)
        {
            return (1 - k) * cp;
        }

        /// <summary>
        /// Convert decimal to string
        /// </summary>
        /// <param name="decVal"></param>
        /// <returns></returns>
        public string ConvertDecimalToString(decimal decVal)
        {
            return string.Format("{0}", decVal.ToString("F2"));
        }

        /// <summary>
        /// Export complete menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportCompleteMenu_Click(object sender, EventArgs e)
        {
            ExportExcel(gridViewComplete);
        }

        /// <summary>
        /// Export error menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportErrorMenu_Click(object sender, EventArgs e)
        {
            ExportExcel(gridViewError);
        }

        /// <summary>
        /// Export ing menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportIngMenu_Click(object sender, EventArgs e)
        {
            ExportExcel(gridViewIng);
        }

        /// <summary>
        /// Export data menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportDataMenu_Click(object sender, EventArgs e)
        {
            ExportExcel(gridViewData);
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
        /// Clear complete control
        /// </summary>
        private void ClearCompleteControl()
        {
            lbl400OKNG_COM.Text = "N/A";
            lbl400OKNG_COM.BackColor = Color.Transparent;
            lbl410OKNG_COM.Text = "N/A";
            lbl410OKNG_COM.BackColor = Color.Transparent;
            lbl420OKNG_COM.Text = "N/A";
            lbl420OKNG_COM.BackColor = Color.Transparent;
            lbl430OKNG_COM.Text = "N/A";
            lbl430OKNG_COM.BackColor = Color.Transparent;
            lbl440OKNG_COM.Text = "N/A";
            lbl440OKNG_COM.BackColor = Color.Transparent;
            lbl450OKNG_COM.Text = "N/A";
            lbl450OKNG_COM.BackColor = Color.Transparent;
            lbl460OKNG_COM.Text = "N/A";
            lbl460OKNG_COM.BackColor = Color.Transparent;

            lblData01_COM.Text = string.Empty;
            lblData02_COM.Text = string.Empty;
            lblData03_COM.Text = string.Empty;
            lblData04_COM.Text = string.Empty;
            lblData05_COM.Text = string.Empty;
            lblData06_COM.Text = string.Empty;
            lblData07_COM.Text = string.Empty;
            lblData08_COM.Text = string.Empty;
            lblData09_COM.Text = string.Empty;
            lblData10_COM.Text = string.Empty;
            lblData11_COM.Text = string.Empty;
            lblData12_COM.Text = string.Empty;
            lblData13_COM.Text = string.Empty;
            lblData14_COM.Text = string.Empty;
            lblData15_COM.Text = string.Empty;
            lblData16_COM.Text = string.Empty;
            lblData17_COM.Text = string.Empty;
            lblData18_COM.Text = string.Empty;
            lblData19_COM.Text = string.Empty;
            lblData20_COM.Text = string.Empty;
            lblData21_COM.Text = string.Empty;
            lblData22_COM.Text = string.Empty;
            lblData23_COM.Text = string.Empty;
            lblData24_COM.Text = string.Empty;
            lblData25_COM.Text = string.Empty;
            lblData26_COM.Text = string.Empty;
            lblData27_COM.Text = string.Empty;
            lblData28_COM.Text = string.Empty;
            lblData29_COM.Text = string.Empty;
            lblData30_COM.Text = string.Empty;
            lblData31_COM.Text = string.Empty;
            lblData32_COM.Text = string.Empty;
            lblData33_COM.Text = string.Empty;
            lblData34_COM.Text = string.Empty;
            lblData35_COM.Text = string.Empty;
            lblData36_COM.Text = string.Empty;

            lblLastEventTime01_COM.Text = string.Empty;
            lblLastEventTime02_COM.Text = string.Empty;
            lblLastEventTime03_COM.Text = string.Empty;
            lblLastEventTime04_COM.Text = string.Empty;
            lblLastEventTime05_COM.Text = string.Empty;
            lblLastEventTime06_COM.Text = string.Empty;
            lblLastEventTime07_COM.Text = string.Empty;
        }

        /// <summary>
        /// Clear error control
        /// </summary>
        private void ClearErrorControl()
        {
            lbl400OKNG_ERR.Text = "N/A";
            lbl400OKNG_ERR.BackColor = Color.Transparent;
            lbl410OKNG_ERR.Text = "N/A";
            lbl410OKNG_ERR.BackColor = Color.Transparent;
            lbl420OKNG_ERR.Text = "N/A";
            lbl420OKNG_ERR.BackColor = Color.Transparent;
            lbl430OKNG_ERR.Text = "N/A";
            lbl430OKNG_ERR.BackColor = Color.Transparent;
            lbl440OKNG_ERR.Text = "N/A";
            lbl440OKNG_ERR.BackColor = Color.Transparent;
            lbl450OKNG_ERR.Text = "N/A";
            lbl450OKNG_ERR.BackColor = Color.Transparent;
            lbl460OKNG_ERR.Text = "N/A";
            lbl460OKNG_ERR.BackColor = Color.Transparent;

            lblData01_ERR.Text = string.Empty;
            lblData02_ERR.Text = string.Empty;
            lblData03_ERR.Text = string.Empty;
            lblData04_ERR.Text = string.Empty;
            lblData05_ERR.Text = string.Empty;
            lblData06_ERR.Text = string.Empty;
            lblData07_ERR.Text = string.Empty;
            lblData08_ERR.Text = string.Empty;
            lblData09_ERR.Text = string.Empty;
            lblData10_ERR.Text = string.Empty;
            lblData11_ERR.Text = string.Empty;
            lblData12_ERR.Text = string.Empty;
            lblData13_ERR.Text = string.Empty;
            lblData14_ERR.Text = string.Empty;
            lblData15_ERR.Text = string.Empty;
            lblData16_ERR.Text = string.Empty;
            lblData17_ERR.Text = string.Empty;
            lblData18_ERR.Text = string.Empty;
            lblData19_ERR.Text = string.Empty;
            lblData20_ERR.Text = string.Empty;
            lblData21_ERR.Text = string.Empty;
            lblData22_ERR.Text = string.Empty;
            lblData23_ERR.Text = string.Empty;
            lblData24_ERR.Text = string.Empty;
            lblData25_ERR.Text = string.Empty;
            lblData26_ERR.Text = string.Empty;
            lblData27_ERR.Text = string.Empty;
            lblData28_ERR.Text = string.Empty;
            lblData29_ERR.Text = string.Empty;
            lblData30_ERR.Text = string.Empty;
            lblData31_ERR.Text = string.Empty;
            lblData32_ERR.Text = string.Empty;
            lblData33_ERR.Text = string.Empty;
            lblData34_ERR.Text = string.Empty;
            lblData35_ERR.Text = string.Empty;
            lblData36_ERR.Text = string.Empty;

            lblLastEventTime01_ERR.Text = string.Empty;
            lblLastEventTime02_ERR.Text = string.Empty;
            lblLastEventTime03_ERR.Text = string.Empty;
            lblLastEventTime04_ERR.Text = string.Empty;
            lblLastEventTime05_ERR.Text = string.Empty;
            lblLastEventTime06_ERR.Text = string.Empty;
            lblLastEventTime07_ERR.Text = string.Empty;
        }

        /// <summary>
        /// Clear ing control
        /// </summary>
        private void ClearIngControl()
        {
            lbl400OKNG_ING.Text = "N/A";
            lbl400OKNG_ING.BackColor = Color.Transparent;
            lbl410OKNG_ING.Text = "N/A";
            lbl410OKNG_ING.BackColor = Color.Transparent;
            lbl420OKNG_ING.Text = "N/A";
            lbl420OKNG_ING.BackColor = Color.Transparent;
            lbl430OKNG_ING.Text = "N/A";
            lbl430OKNG_ING.BackColor = Color.Transparent;
            lbl440OKNG_ING.Text = "N/A";
            lbl440OKNG_ING.BackColor = Color.Transparent;
            lbl450OKNG_ING.Text = "N/A";
            lbl450OKNG_ING.BackColor = Color.Transparent;
            lbl460OKNG_ING.Text = "N/A";
            lbl460OKNG_ING.BackColor = Color.Transparent;

            lblData01_ING.Text = string.Empty;
            lblData02_ING.Text = string.Empty;
            lblData03_ING.Text = string.Empty;
            lblData04_ING.Text = string.Empty;
            lblData05_ING.Text = string.Empty;
            lblData06_ING.Text = string.Empty;
            lblData07_ING.Text = string.Empty;
            lblData08_ING.Text = string.Empty;
            lblData09_ING.Text = string.Empty;
            lblData10_ING.Text = string.Empty;
            lblData11_ING.Text = string.Empty;
            lblData12_ING.Text = string.Empty;
            lblData13_ING.Text = string.Empty;
            lblData14_ING.Text = string.Empty;
            lblData15_ING.Text = string.Empty;
            lblData16_ING.Text = string.Empty;
            lblData17_ING.Text = string.Empty;
            lblData18_ING.Text = string.Empty;
            lblData19_ING.Text = string.Empty;
            lblData20_ING.Text = string.Empty;
            lblData21_ING.Text = string.Empty;
            lblData22_ING.Text = string.Empty;
            lblData23_ING.Text = string.Empty;
            lblData24_ING.Text = string.Empty;
            lblData25_ING.Text = string.Empty;
            lblData26_ING.Text = string.Empty;
            lblData27_ING.Text = string.Empty;
            lblData28_ING.Text = string.Empty;
            lblData29_ING.Text = string.Empty;
            lblData30_ING.Text = string.Empty;
            lblData31_ING.Text = string.Empty;
            lblData32_ING.Text = string.Empty;
            lblData33_ING.Text = string.Empty;
            lblData34_ING.Text = string.Empty;
            lblData35_ING.Text = string.Empty;
            lblData36_ING.Text = string.Empty;

            lblLastEventTime01_ING.Text = string.Empty;
            lblLastEventTime02_ING.Text = string.Empty;
            lblLastEventTime03_ING.Text = string.Empty;
            lblLastEventTime04_ING.Text = string.Empty;
            lblLastEventTime05_ING.Text = string.Empty;
            lblLastEventTime06_ING.Text = string.Empty;
            lblLastEventTime07_ING.Text = string.Empty;
        }

        #endregion

        #region SQL

        /// <summary>
        /// Get complete data (TDS3002)
        /// </summary>
        public const string _sqlGetCompleteHistoryData =
            @"SELECT A.PLANT_CODE									                            AS PLANT_CODE
                    ,A.FACTORY									                                AS FACTORY
                    ,A.WORKCENTER_CODE							                                AS WORKCENTER_CODE
                    ,A.LOT_NO                                                                   AS LOT_NO
                    ,A.OPERATION01                                                              AS OPERATION01
                    ,A.OPERATION02                                                              AS OPERATION02
                    ,A.OPERATION03                                                              AS OPERATION03
                    ,A.OPERATION04                                                              AS OPERATION04
                    ,A.OPERATION05                                                              AS OPERATION05
                    ,A.OPERATION06                                                              AS OPERATION06
                    ,A.OPERATION07                                                              AS OPERATION07
                    ,A.ITEM_CODE                                                                AS ITEM_CODE
                    ,A.JUDGE01                                                                  AS JUDGE01
                    ,A.JUDGE02                                                                  AS JUDGE02
                    ,A.JUDGE03                                                                  AS JUDGE03
                    ,A.JUDGE04                                                                  AS JUDGE04
                    ,A.JUDGE05                                                                  AS JUDGE05
                    ,A.JUDGE06                                                                  AS JUDGE06
                    ,A.JUDGE07                                                                  AS JUDGE07
                    ,A.JUDGE08                                                                  AS JUDGE08
                    ,A.JUDGE09                                                                  AS JUDGE09
                    ,A.JUDGE10                                                                  AS JUDGE10
                    ,A.JUDGE11                                                                  AS JUDGE11
                    ,A.JUDGE12                                                                  AS JUDGE12
                    ,A.JUDGE13                                                                  AS JUDGE13
                    ,A.JUDGE14                                                                  AS JUDGE14
                    ,A.JUDGE15                                                                  AS JUDGE15
                    ,A.JUDGE16                                                                  AS JUDGE16
                    ,A.JUDGE17                                                                  AS JUDGE17
                    ,A.JUDGE18                                                                  AS JUDGE18
                    ,A.JUDGE19                                                                  AS JUDGE19
                    ,A.JUDGE20                                                                  AS JUDGE20
                    ,A.JUDGE21                                                                  AS JUDGE21
                    ,A.JUDGE22                                                                  AS JUDGE22
                    ,A.JUDGE23                                                                  AS JUDGE23
                    ,A.JUDGE24                                                                  AS JUDGE24
                    ,A.JUDGE25                                                                  AS JUDGE25
                    ,A.JUDGE26                                                                  AS JUDGE26
                    ,A.DATA01                                                                   AS DATA01
                    ,A.DATA02                                                                   AS DATA02
                    ,A.DATA03                                                                   AS DATA03
                    ,A.DATA04                                                                   AS DATA04
                    ,A.DATA05                                                                   AS DATA05
                    ,A.DATA06                                                                   AS DATA06
                    ,A.DATA07                                                                   AS DATA07
                    ,A.DATA08                                                                   AS DATA08
                    ,A.DATA09                                                                   AS DATA09
                    ,A.DATA10                                                                   AS DATA10
                    ,A.DATA11                                                                   AS DATA11
                    ,A.DATA12                                                                   AS DATA12
                    ,A.DATA13                                                                   AS DATA13
                    ,A.DATA14                                                                   AS DATA14
                    ,A.DATA15                                                                   AS DATA15
                    ,A.DATA16                                                                   AS DATA16
                    ,A.DATA17                                                                   AS DATA17
                    ,A.DATA18                                                                   AS DATA18
                    ,A.DATA19                                                                   AS DATA19
                    ,A.DATA20                                                                   AS DATA20
                    ,A.DATA21                                                                   AS DATA21
                    ,A.DATA22                                                                   AS DATA22
                    ,A.DATA23                                                                   AS DATA23
                    ,A.DATA24                                                                   AS DATA24
                    ,A.DATA25                                                                   AS DATA25
                    ,A.DATA26                                                                   AS DATA26
                    ,A.DATA27                                                                   AS DATA27
                    ,A.DATA28                                                                   AS DATA28
                    ,A.DATA29                                                                   AS DATA29
                    ,A.DATA30                                                                   AS DATA30
                    ,A.DATA31                                                                   AS DATA31
                    ,A.DATA32                                                                   AS DATA32
                    ,A.DATA33                                                                   AS DATA33
                    ,A.DATA34                                                                   AS DATA34
                    ,A.DATA35                                                                   AS DATA35
                    ,A.DATA36                                                                   AS DATA36
                    ,A.REWORK01                                                                 AS REWORK01
                    ,A.REWORK02                                                                 AS REWORK02
                    ,A.REWORK03                                                                 AS REWORK03
                    ,A.REWORK04                                                                 AS REWORK04
                    ,A.REWORK05                                                                 AS REWORK05
                    ,A.REWORK06                                                                 AS REWORK06
                    ,A.REWORK07                                                                 AS REWORK07
                    ,A.LASTEVENT_TIME01                                                         AS LASTEVENT_TIME01
                    ,A.LASTEVENT_TIME02                                                         AS LASTEVENT_TIME02
                    ,A.LASTEVENT_TIME03                                                         AS LASTEVENT_TIME03
                    ,A.LASTEVENT_TIME04                                                         AS LASTEVENT_TIME04
                    ,A.LASTEVENT_TIME05                                                         AS LASTEVENT_TIME05
                    ,A.LASTEVENT_TIME06                                                         AS LASTEVENT_TIME06
                    ,A.LASTEVENT_TIME07                                                         AS LASTEVENT_TIME07
                    ,A.LASTEVENT_TIME                                                           AS LASTEVENT_TIME
                FROM TDS3002 A WITH(NOLOCK)
               WHERE 1 = 1
                 AND A.PLANT_CODE		LIKE @PLANT_CODE
                 AND A.WORKCENTER_CODE  = @WORKCENTER_CODE
                 AND A.LASTEVENT_TIME   BETWEEN @FROM_DT AND @TO_DT
                 AND A.JUDGE            = 'OK'";

        /// <summary>
        /// Get error history data (TDS3001)
        /// </summary>
        public const string _sqlErrorHistoryData =
            @"SELECT A.PLANT_CODE									                            AS PLANT_CODE
                    ,A.FACTORY									                                AS FACTORY
                    ,A.WORKCENTER_CODE							                                AS WORKCENTER_CODE
                    ,A.LOT_NO                                                                   AS LOT_NO
                    ,A.OPERATION01                                                              AS OPERATION01
                    ,A.OPERATION02                                                              AS OPERATION02
                    ,A.OPERATION03                                                              AS OPERATION03
                    ,A.OPERATION04                                                              AS OPERATION04
                    ,A.OPERATION05                                                              AS OPERATION05
                    ,A.OPERATION06                                                              AS OPERATION06
                    ,A.OPERATION07                                                              AS OPERATION07
                    ,A.ITEM_CODE                                                                AS ITEM_CODE
                    ,A.JUDGE01                                                                  AS JUDGE01
                    ,A.JUDGE02                                                                  AS JUDGE02
                    ,A.JUDGE03                                                                  AS JUDGE03
                    ,A.JUDGE04                                                                  AS JUDGE04
                    ,A.JUDGE05                                                                  AS JUDGE05
                    ,A.JUDGE06                                                                  AS JUDGE06
                    ,A.JUDGE07                                                                  AS JUDGE07
                    ,A.JUDGE08                                                                  AS JUDGE08
                    ,A.JUDGE09                                                                  AS JUDGE09
                    ,A.JUDGE10                                                                  AS JUDGE10
                    ,A.JUDGE11                                                                  AS JUDGE11
                    ,A.JUDGE12                                                                  AS JUDGE12
                    ,A.JUDGE13                                                                  AS JUDGE13
                    ,A.JUDGE14                                                                  AS JUDGE14
                    ,A.JUDGE15                                                                  AS JUDGE15
                    ,A.JUDGE16                                                                  AS JUDGE16
                    ,A.JUDGE17                                                                  AS JUDGE17
                    ,A.JUDGE18                                                                  AS JUDGE18
                    ,A.JUDGE19                                                                  AS JUDGE19
                    ,A.JUDGE20                                                                  AS JUDGE20
                    ,A.JUDGE21                                                                  AS JUDGE21
                    ,A.JUDGE22                                                                  AS JUDGE22
                    ,A.JUDGE23                                                                  AS JUDGE23
                    ,A.JUDGE24                                                                  AS JUDGE24
                    ,A.JUDGE25                                                                  AS JUDGE25
                    ,A.JUDGE26                                                                  AS JUDGE26
                    ,A.DATA01                                                                   AS DATA01
                    ,A.DATA02                                                                   AS DATA02
                    ,A.DATA03                                                                   AS DATA03
                    ,A.DATA04                                                                   AS DATA04
                    ,A.DATA05                                                                   AS DATA05
                    ,A.DATA06                                                                   AS DATA06
                    ,A.DATA07                                                                   AS DATA07
                    ,A.DATA08                                                                   AS DATA08
                    ,A.DATA09                                                                   AS DATA09
                    ,A.DATA10                                                                   AS DATA10
                    ,A.DATA11                                                                   AS DATA11
                    ,A.DATA12                                                                   AS DATA12
                    ,A.DATA13                                                                   AS DATA13
                    ,A.DATA14                                                                   AS DATA14
                    ,A.DATA15                                                                   AS DATA15
                    ,A.DATA16                                                                   AS DATA16
                    ,A.DATA17                                                                   AS DATA17
                    ,A.DATA18                                                                   AS DATA18
                    ,A.DATA19                                                                   AS DATA19
                    ,A.DATA20                                                                   AS DATA20
                    ,A.DATA21                                                                   AS DATA21
                    ,A.DATA22                                                                   AS DATA22
                    ,A.DATA23                                                                   AS DATA23
                    ,A.DATA24                                                                   AS DATA24
                    ,A.DATA25                                                                   AS DATA25
                    ,A.DATA26                                                                   AS DATA26
                    ,A.DATA27                                                                   AS DATA27
                    ,A.DATA28                                                                   AS DATA28
                    ,A.DATA29                                                                   AS DATA29
                    ,A.DATA30                                                                   AS DATA30
                    ,A.DATA31                                                                   AS DATA31
                    ,A.DATA32                                                                   AS DATA32
                    ,A.DATA33                                                                   AS DATA33
                    ,A.DATA34                                                                   AS DATA34
                    ,A.DATA35                                                                   AS DATA35
                    ,A.DATA36                                                                   AS DATA36
                    ,A.REWORK01                                                                 AS REWORK01
                    ,A.REWORK02                                                                 AS REWORK02
                    ,A.REWORK03                                                                 AS REWORK03
                    ,A.REWORK04                                                                 AS REWORK04
                    ,A.REWORK05                                                                 AS REWORK05
                    ,A.REWORK06                                                                 AS REWORK06
                    ,A.REWORK07                                                                 AS REWORK07
                    ,A.LASTEVENT_TIME01                                                         AS LASTEVENT_TIME01
                    ,A.LASTEVENT_TIME02                                                         AS LASTEVENT_TIME02
                    ,A.LASTEVENT_TIME03                                                         AS LASTEVENT_TIME03
                    ,A.LASTEVENT_TIME04                                                         AS LASTEVENT_TIME04
                    ,A.LASTEVENT_TIME05                                                         AS LASTEVENT_TIME05
                    ,A.LASTEVENT_TIME06                                                         AS LASTEVENT_TIME06
                    ,A.LASTEVENT_TIME07                                                         AS LASTEVENT_TIME07
                FROM TDS3001 A WITH(NOLOCK)
               WHERE 1 = 1
                 AND A.PLANT_CODE		        LIKE @PLANT_CODE
                 AND A.WORKCENTER_CODE          = @WORKCENTER_CODE
                 AND A.LASTEVENT_TIME01         BETWEEN @FROM_DT AND @TO_DT
                 AND A.JUDGE                    = 'NG'";

        /// <summary>
        /// Get ing history data (TDS3001)
        /// </summary>
        public const string _sqlIngHistoryData =
            @"SELECT A.PLANT_CODE									                            AS PLANT_CODE
                    ,A.FACTORY									                                AS FACTORY
                    ,A.WORKCENTER_CODE							                                AS WORKCENTER_CODE
                    ,A.LOT_NO                                                                   AS LOT_NO
                    ,A.OPERATION01                                                              AS OPERATION01
                    ,A.OPERATION02                                                              AS OPERATION02
                    ,A.OPERATION03                                                              AS OPERATION03
                    ,A.OPERATION04                                                              AS OPERATION04
                    ,A.OPERATION05                                                              AS OPERATION05
                    ,A.OPERATION06                                                              AS OPERATION06
                    ,A.OPERATION07                                                              AS OPERATION07
                    ,A.ITEM_CODE                                                                AS ITEM_CODE
                    ,A.JUDGE01                                                                  AS JUDGE01
                    ,A.JUDGE02                                                                  AS JUDGE02
                    ,A.JUDGE03                                                                  AS JUDGE03
                    ,A.JUDGE04                                                                  AS JUDGE04
                    ,A.JUDGE05                                                                  AS JUDGE05
                    ,A.JUDGE06                                                                  AS JUDGE06
                    ,A.JUDGE07                                                                  AS JUDGE07
                    ,A.JUDGE08                                                                  AS JUDGE08
                    ,A.JUDGE09                                                                  AS JUDGE09
                    ,A.JUDGE10                                                                  AS JUDGE10
                    ,A.JUDGE11                                                                  AS JUDGE11
                    ,A.JUDGE12                                                                  AS JUDGE12
                    ,A.JUDGE13                                                                  AS JUDGE13
                    ,A.JUDGE14                                                                  AS JUDGE14
                    ,A.JUDGE15                                                                  AS JUDGE15
                    ,A.JUDGE16                                                                  AS JUDGE16
                    ,A.JUDGE17                                                                  AS JUDGE17
                    ,A.JUDGE18                                                                  AS JUDGE18
                    ,A.JUDGE19                                                                  AS JUDGE19
                    ,A.JUDGE20                                                                  AS JUDGE20
                    ,A.JUDGE21                                                                  AS JUDGE21
                    ,A.JUDGE22                                                                  AS JUDGE22
                    ,A.JUDGE23                                                                  AS JUDGE23
                    ,A.JUDGE24                                                                  AS JUDGE24
                    ,A.JUDGE25                                                                  AS JUDGE25
                    ,A.JUDGE26                                                                  AS JUDGE26
                    ,A.DATA01                                                                   AS DATA01
                    ,A.DATA02                                                                   AS DATA02
                    ,A.DATA03                                                                   AS DATA03
                    ,A.DATA04                                                                   AS DATA04
                    ,A.DATA05                                                                   AS DATA05
                    ,A.DATA06                                                                   AS DATA06
                    ,A.DATA07                                                                   AS DATA07
                    ,A.DATA08                                                                   AS DATA08
                    ,A.DATA09                                                                   AS DATA09
                    ,A.DATA10                                                                   AS DATA10
                    ,A.DATA11                                                                   AS DATA11
                    ,A.DATA12                                                                   AS DATA12
                    ,A.DATA13                                                                   AS DATA13
                    ,A.DATA14                                                                   AS DATA14
                    ,A.DATA15                                                                   AS DATA15
                    ,A.DATA16                                                                   AS DATA16
                    ,A.DATA17                                                                   AS DATA17
                    ,A.DATA18                                                                   AS DATA18
                    ,A.DATA19                                                                   AS DATA19
                    ,A.DATA20                                                                   AS DATA20
                    ,A.DATA21                                                                   AS DATA21
                    ,A.DATA22                                                                   AS DATA22
                    ,A.DATA23                                                                   AS DATA23
                    ,A.DATA24                                                                   AS DATA24
                    ,A.DATA25                                                                   AS DATA25
                    ,A.DATA26                                                                   AS DATA26
                    ,A.DATA27                                                                   AS DATA27
                    ,A.DATA28                                                                   AS DATA28
                    ,A.DATA29                                                                   AS DATA29
                    ,A.DATA30                                                                   AS DATA30
                    ,A.DATA31                                                                   AS DATA31
                    ,A.DATA32                                                                   AS DATA32
                    ,A.DATA33                                                                   AS DATA33
                    ,A.DATA34                                                                   AS DATA34
                    ,A.DATA35                                                                   AS DATA35
                    ,A.DATA36                                                                   AS DATA36
                    ,A.REWORK01                                                                 AS REWORK01
                    ,A.REWORK02                                                                 AS REWORK02
                    ,A.REWORK03                                                                 AS REWORK03
                    ,A.REWORK04                                                                 AS REWORK04
                    ,A.REWORK05                                                                 AS REWORK05
                    ,A.REWORK06                                                                 AS REWORK06
                    ,A.REWORK07                                                                 AS REWORK07
                    ,A.LASTEVENT_TIME01                                                         AS LASTEVENT_TIME01
                    ,A.LASTEVENT_TIME02                                                         AS LASTEVENT_TIME02
                    ,A.LASTEVENT_TIME03                                                         AS LASTEVENT_TIME03
                    ,A.LASTEVENT_TIME04                                                         AS LASTEVENT_TIME04
                    ,A.LASTEVENT_TIME05                                                         AS LASTEVENT_TIME05
                    ,A.LASTEVENT_TIME06                                                         AS LASTEVENT_TIME06
                    ,A.LASTEVENT_TIME07                                                         AS LASTEVENT_TIME07
                FROM TDS3001 A WITH(NOLOCK)
               WHERE 1 = 1
                 AND A.PLANT_CODE		        LIKE @PLANT_CODE
                 AND A.WORKCENTER_CODE          = @WORKCENTER_CODE
                 AND A.LASTEVENT_TIME01         BETWEEN @FROM_DT AND @TO_DT
                 AND ISNULL(A.JUDGE01, '')     <> 'NG'
                 AND ISNULL(A.JUDGE02, '')     <> 'NG'
                 AND ISNULL(A.JUDGE04, '')     <> 'NG'
                 AND ISNULL(A.JUDGE09, '')     <> 'NG'
                 AND ISNULL(A.JUDGE24, '')     <> 'NG'
                 AND ISNULL(A.JUDGE25, '')     <> 'NG'
                 AND ISNULL(A.JUDGE26, '')     <> 'NG'";

        /// <summary>
        /// Sql data
        /// </summary>
        public const string _sqlData =
            @"SELECT '공정 불합격'																AS FLAG
					,A.LOT_NO							                                        AS LOT_NO
                    ,A.OPERATION01					                                            AS OPERATION01
	                ,A.OPERATION02					                                            AS OPERATION02
	                ,A.OPERATION03					                                            AS OPERATION03
	                ,A.OPERATION04					                                            AS OPERATION04
	                ,A.OPERATION05					                                            AS OPERATION05
	                ,A.OPERATION06					                                            AS OPERATION06
	                ,A.OPERATION07					                                            AS OPERATION07
	                ,A.ITEM_CODE						                                        AS ITEM_CODE
	                ,A.JUDGE01						                                            AS JUDGE01
	                ,A.JUDGE02						                                            AS JUDGE02
	                ,A.JUDGE03						                                            AS JUDGE03
	                ,A.JUDGE04						                                            AS JUDGE04
	                ,A.JUDGE05						                                            AS JUDGE05
	                ,A.JUDGE06						                                            AS JUDGE06
	                ,A.JUDGE07						                                            AS JUDGE07
	                ,A.JUDGE08						                                            AS JUDGE08
	                ,A.JUDGE09						                                            AS JUDGE09
	                ,A.JUDGE10						                                            AS JUDGE10
	                ,A.JUDGE11						                                            AS JUDGE11
	                ,A.JUDGE12						                                            AS JUDGE12
	                ,A.JUDGE13						                                            AS JUDGE13
	                ,A.JUDGE14						                                            AS JUDGE14
	                ,A.JUDGE15						                                            AS JUDGE15
	                ,A.JUDGE16						                                            AS JUDGE16
	                ,A.JUDGE17						                                            AS JUDGE17
	                ,A.JUDGE18						                                            AS JUDGE18
	                ,A.JUDGE19						                                            AS JUDGE19
	                ,A.JUDGE20						                                            AS JUDGE20
	                ,A.JUDGE21						                                            AS JUDGE21
	                ,A.JUDGE22						                                            AS JUDGE22
	                ,A.JUDGE23						                                            AS JUDGE23
	                ,A.JUDGE24						                                            AS JUDGE24
	                ,A.JUDGE25						                                            AS JUDGE25
	                ,A.JUDGE26						                                            AS JUDGE26
	                ,A.DATA01							                                        AS DATA01
	                ,A.DATA02							                                        AS DATA02
	                ,A.DATA03							                                        AS DATA03
	                ,A.DATA04							                                        AS DATA04
	                ,A.DATA05							                                        AS DATA05
	                ,A.DATA06							                                        AS DATA06
	                ,A.DATA07							                                        AS DATA07
	                ,A.DATA08							                                        AS DATA08
	                ,A.DATA09							                                        AS DATA09
	                ,A.DATA10							                                        AS DATA10
	                ,A.DATA11							                                        AS DATA11
	                ,A.DATA12							                                        AS DATA12
	                ,A.DATA13							                                        AS DATA13
	                ,A.DATA14							                                        AS DATA14
	                ,A.DATA15							                                        AS DATA15
	                ,A.DATA16							                                        AS DATA16
	                ,A.DATA17							                                        AS DATA17
	                ,A.DATA18							                                        AS DATA18
	                ,A.DATA19							                                        AS DATA19
	                ,A.DATA20							                                        AS DATA20
	                ,A.DATA21							                                        AS DATA21
	                ,A.DATA22							                                        AS DATA22
	                ,A.DATA23							                                        AS DATA23
	                ,A.DATA24							                                        AS DATA24
	                ,A.DATA25							                                        AS DATA25
	                ,A.DATA26							                                        AS DATA26
	                ,A.DATA27							                                        AS DATA27
	                ,A.DATA28							                                        AS DATA28
	                ,A.DATA29							                                        AS DATA29
	                ,A.DATA30							                                        AS DATA30
	                ,A.DATA31							                                        AS DATA31
	                ,A.DATA32							                                        AS DATA32
	                ,A.DATA33							                                        AS DATA33
	                ,A.DATA34							                                        AS DATA34
	                ,A.DATA35							                                        AS DATA35
	                ,A.DATA36							                                        AS DATA36
	                ,A.REWORK01						                                            AS REWORK01
	                ,A.REWORK02						                                            AS REWORK02
	                ,A.REWORK03						                                            AS REWORK03
	                ,A.REWORK04						                                            AS REWORK04
	                ,A.REWORK05						                                            AS REWORK05
	                ,A.REWORK06						                                            AS REWORK06
	                ,A.REWORK07						                                            AS REWORK07
	                ,A.LASTEVENT_TIME01				                                            AS LASTEVENT_TIME01
	                ,A.LASTEVENT_TIME02				                                            AS LASTEVENT_TIME02
	                ,A.LASTEVENT_TIME03				                                            AS LASTEVENT_TIME03
	                ,A.LASTEVENT_TIME04				                                            AS LASTEVENT_TIME04
	                ,A.LASTEVENT_TIME05				                                            AS LASTEVENT_TIME05
	                ,A.LASTEVENT_TIME06				                                            AS LASTEVENT_TIME06
	                ,A.LASTEVENT_TIME07				                                            AS LASTEVENT_TIME07
                FROM TDS3001 A WITH(NOLOCK)
                WHERE 1 = 1
                AND A.PLANT_CODE		    LIKE @PLANT_CODE
                AND A.WORKCENTER_CODE    = @WORKCENTER_CODE
                AND A.LASTEVENT_TIME01	BETWEEN @FROM_DT AND @TO_DT
                AND (ISNULL(A.JUDGE01, '')     = 'NG'
                 OR  ISNULL(A.JUDGE02, '')     = 'NG'
                 OR  ISNULL(A.JUDGE04, '')     = 'NG'
                 OR  ISNULL(A.JUDGE09, '')     = 'NG'
                 OR  ISNULL(A.JUDGE24, '')     = 'NG'
                 OR  ISNULL(A.JUDGE25, '')     = 'NG'
                 OR  ISNULL(A.JUDGE26, '')     = 'NG')

              UNION ALL

			  SELECT '진행중'																	AS FLAG
					,A.LOT_NO							                                        AS LOT_NO
                    ,A.OPERATION01					                                            AS OPERATION01
	                ,A.OPERATION02					                                            AS OPERATION02
	                ,A.OPERATION03					                                            AS OPERATION03
	                ,A.OPERATION04					                                            AS OPERATION04
	                ,A.OPERATION05					                                            AS OPERATION05
	                ,A.OPERATION06					                                            AS OPERATION06
	                ,A.OPERATION07					                                            AS OPERATION07
	                ,A.ITEM_CODE						                                        AS ITEM_CODE
	                ,A.JUDGE01						                                            AS JUDGE01
	                ,A.JUDGE02						                                            AS JUDGE02
	                ,A.JUDGE03						                                            AS JUDGE03
	                ,A.JUDGE04						                                            AS JUDGE04
	                ,A.JUDGE05						                                            AS JUDGE05
	                ,A.JUDGE06						                                            AS JUDGE06
	                ,A.JUDGE07						                                            AS JUDGE07
	                ,A.JUDGE08						                                            AS JUDGE08
	                ,A.JUDGE09						                                            AS JUDGE09
	                ,A.JUDGE10						                                            AS JUDGE10
	                ,A.JUDGE11						                                            AS JUDGE11
	                ,A.JUDGE12						                                            AS JUDGE12
	                ,A.JUDGE13						                                            AS JUDGE13
	                ,A.JUDGE14						                                            AS JUDGE14
	                ,A.JUDGE15						                                            AS JUDGE15
	                ,A.JUDGE16						                                            AS JUDGE16
	                ,A.JUDGE17						                                            AS JUDGE17
	                ,A.JUDGE18						                                            AS JUDGE18
	                ,A.JUDGE19						                                            AS JUDGE19
	                ,A.JUDGE20						                                            AS JUDGE20
	                ,A.JUDGE21						                                            AS JUDGE21
	                ,A.JUDGE22						                                            AS JUDGE22
	                ,A.JUDGE23						                                            AS JUDGE23
	                ,A.JUDGE24						                                            AS JUDGE24
	                ,A.JUDGE25						                                            AS JUDGE25
	                ,A.JUDGE26						                                            AS JUDGE26
	                ,A.DATA01							                                        AS DATA01
	                ,A.DATA02							                                        AS DATA02
	                ,A.DATA03							                                        AS DATA03
	                ,A.DATA04							                                        AS DATA04
	                ,A.DATA05							                                        AS DATA05
	                ,A.DATA06							                                        AS DATA06
	                ,A.DATA07							                                        AS DATA07
	                ,A.DATA08							                                        AS DATA08
	                ,A.DATA09							                                        AS DATA09
	                ,A.DATA10							                                        AS DATA10
	                ,A.DATA11							                                        AS DATA11
	                ,A.DATA12							                                        AS DATA12
	                ,A.DATA13							                                        AS DATA13
	                ,A.DATA14							                                        AS DATA14
	                ,A.DATA15							                                        AS DATA15
	                ,A.DATA16							                                        AS DATA16
	                ,A.DATA17							                                        AS DATA17
	                ,A.DATA18							                                        AS DATA18
	                ,A.DATA19							                                        AS DATA19
	                ,A.DATA20							                                        AS DATA20
	                ,A.DATA21							                                        AS DATA21
	                ,A.DATA22							                                        AS DATA22
	                ,A.DATA23							                                        AS DATA23
	                ,A.DATA24							                                        AS DATA24
	                ,A.DATA25							                                        AS DATA25
	                ,A.DATA26							                                        AS DATA26
	                ,A.DATA27							                                        AS DATA27
	                ,A.DATA28							                                        AS DATA28
	                ,A.DATA29							                                        AS DATA29
	                ,A.DATA30							                                        AS DATA30
	                ,A.DATA31							                                        AS DATA31
	                ,A.DATA32							                                        AS DATA32
	                ,A.DATA33							                                        AS DATA33
	                ,A.DATA34							                                        AS DATA34
	                ,A.DATA35							                                        AS DATA35
	                ,A.DATA36							                                        AS DATA36
	                ,A.REWORK01						                                            AS REWORK01
	                ,A.REWORK02						                                            AS REWORK02
	                ,A.REWORK03						                                            AS REWORK03
	                ,A.REWORK04						                                            AS REWORK04
	                ,A.REWORK05						                                            AS REWORK05
	                ,A.REWORK06						                                            AS REWORK06
	                ,A.REWORK07						                                            AS REWORK07
	                ,A.LASTEVENT_TIME01				                                            AS LASTEVENT_TIME01
	                ,A.LASTEVENT_TIME02				                                            AS LASTEVENT_TIME02
	                ,A.LASTEVENT_TIME03				                                            AS LASTEVENT_TIME03
	                ,A.LASTEVENT_TIME04				                                            AS LASTEVENT_TIME04
	                ,A.LASTEVENT_TIME05				                                            AS LASTEVENT_TIME05
	                ,A.LASTEVENT_TIME06				                                            AS LASTEVENT_TIME06
	                ,A.LASTEVENT_TIME07				                                            AS LASTEVENT_TIME07
                FROM TDS3001 A WITH(NOLOCK)
                WHERE 1 = 1
                AND A.PLANT_CODE		    LIKE @PLANT_CODE
                AND A.WORKCENTER_CODE    = @WORKCENTER_CODE
                AND A.LASTEVENT_TIME01	BETWEEN @FROM_DT AND @TO_DT
                AND ISNULL(A.JUDGE01, '')     <> 'NG'
                AND ISNULL(A.JUDGE02, '')     <> 'NG'
                AND ISNULL(A.JUDGE04, '')     <> 'NG'
                AND ISNULL(A.JUDGE09, '')     <> 'NG'
                AND ISNULL(A.JUDGE24, '')     <> 'NG'
                AND ISNULL(A.JUDGE25, '')     <> 'NG'
                AND ISNULL(A.JUDGE26, '')     <> 'NG'

              UNION ALL
  
              SELECT CASE WHEN ISNULL(A.OPERATION01, '') = ''	THEN '불합격'
						  WHEN ISNULL(A.OPERATION02, '') = ''	THEN '불합격'
						  WHEN ISNULL(A.OPERATION03, '') = ''	THEN '불합격'
						  WHEN ISNULL(A.OPERATION04, '') = ''	THEN '불합격'
						  WHEN ISNULL(A.OPERATION05, '') = ''	THEN '불합격'
						  WHEN ISNULL(A.OPERATION06, '') = ''	THEN '불합격'
						  WHEN ISNULL(A.OPERATION07, '') = ''	THEN '불합격'
						  ELSE '합격'
					 END																		AS FLAG
					,A.LOT_NO							                                        AS LOT_NO
	                ,A.OPERATION01					                                            AS OPERATION01
	                ,A.OPERATION02					                                            AS OPERATION02
	                ,A.OPERATION03					                                            AS OPERATION03
	                ,A.OPERATION04					                                            AS OPERATION04
	                ,A.OPERATION05					                                            AS OPERATION05
	                ,A.OPERATION06					                                            AS OPERATION06
	                ,A.OPERATION07					                                            AS OPERATION07
	                ,A.ITEM_CODE						                                        AS ITEM_CODE
	                ,A.JUDGE01						                                            AS JUDGE01
	                ,A.JUDGE02						                                            AS JUDGE02
	                ,A.JUDGE03						                                            AS JUDGE03
	                ,A.JUDGE04						                                            AS JUDGE04
	                ,A.JUDGE05						                                            AS JUDGE05
	                ,A.JUDGE06						                                            AS JUDGE06
	                ,A.JUDGE07						                                            AS JUDGE07
	                ,A.JUDGE08						                                            AS JUDGE08
	                ,A.JUDGE09						                                            AS JUDGE09
	                ,A.JUDGE10						                                            AS JUDGE10
	                ,A.JUDGE11						                                            AS JUDGE11
	                ,A.JUDGE12						                                            AS JUDGE12
	                ,A.JUDGE13						                                            AS JUDGE13
	                ,A.JUDGE14						                                            AS JUDGE14
	                ,A.JUDGE15						                                            AS JUDGE15
	                ,A.JUDGE16						                                            AS JUDGE16
	                ,A.JUDGE17						                                            AS JUDGE17
	                ,A.JUDGE18						                                            AS JUDGE18
	                ,A.JUDGE19						                                            AS JUDGE19
	                ,A.JUDGE20						                                            AS JUDGE20
	                ,A.JUDGE21						                                            AS JUDGE21
	                ,A.JUDGE22						                                            AS JUDGE22
	                ,A.JUDGE23						                                            AS JUDGE23
	                ,A.JUDGE24						                                            AS JUDGE24
	                ,A.JUDGE25						                                            AS JUDGE25
	                ,A.JUDGE26						                                            AS JUDGE26
	                ,A.DATA01							                                        AS DATA01
	                ,A.DATA02							                                        AS DATA02
	                ,A.DATA03							                                        AS DATA03
	                ,A.DATA04							                                        AS DATA04
	                ,A.DATA05							                                        AS DATA05
	                ,A.DATA06							                                        AS DATA06
	                ,A.DATA07							                                        AS DATA07
	                ,A.DATA08							                                        AS DATA08
	                ,A.DATA09							                                        AS DATA09
	                ,A.DATA10							                                        AS DATA10
	                ,A.DATA11							                                        AS DATA11
	                ,A.DATA12							                                        AS DATA12
	                ,A.DATA13							                                        AS DATA13
	                ,A.DATA14							                                        AS DATA14
	                ,A.DATA15							                                        AS DATA15
	                ,A.DATA16							                                        AS DATA16
	                ,A.DATA17							                                        AS DATA17
	                ,A.DATA18							                                        AS DATA18
	                ,A.DATA19							                                        AS DATA19
	                ,A.DATA20							                                        AS DATA20
	                ,A.DATA21							                                        AS DATA21
	                ,A.DATA22							                                        AS DATA22
	                ,A.DATA23							                                        AS DATA23
	                ,A.DATA24							                                        AS DATA24
	                ,A.DATA25							                                        AS DATA25
	                ,A.DATA26							                                        AS DATA26
	                ,A.DATA27							                                        AS DATA27
	                ,A.DATA28							                                        AS DATA28
	                ,A.DATA29							                                        AS DATA29
	                ,A.DATA30							                                        AS DATA30
	                ,A.DATA31							                                        AS DATA31
	                ,A.DATA32							                                        AS DATA32
	                ,A.DATA33							                                        AS DATA33
	                ,A.DATA34							                                        AS DATA34
	                ,A.DATA35							                                        AS DATA35
	                ,A.DATA36							                                        AS DATA36
	                ,A.REWORK01						                                            AS REWORK01
	                ,A.REWORK02						                                            AS REWORK02
	                ,A.REWORK03						                                            AS REWORK03
	                ,A.REWORK04						                                            AS REWORK04
	                ,A.REWORK05						                                            AS REWORK05
	                ,A.REWORK06						                                            AS REWORK06
	                ,A.REWORK07						                                            AS REWORK07
	                ,A.LASTEVENT_TIME01				                                            AS LASTEVENT_TIME01
	                ,A.LASTEVENT_TIME02				                                            AS LASTEVENT_TIME02
	                ,A.LASTEVENT_TIME03				                                            AS LASTEVENT_TIME03
	                ,A.LASTEVENT_TIME04				                                            AS LASTEVENT_TIME04
	                ,A.LASTEVENT_TIME05				                                            AS LASTEVENT_TIME05
	                ,A.LASTEVENT_TIME06				                                            AS LASTEVENT_TIME06
	                ,A.LASTEVENT_TIME07				                                            AS LASTEVENT_TIME07
                FROM TDS3002 A WITH(NOLOCK)
               WHERE 1 = 1
                 AND A.PLANT_CODE        LIKE @PLANT_CODE
                 AND A.WORKCENTER_CODE   = @WORKCENTER_CODE
                 AND A.LASTEVENT_TIME	 BETWEEN @FROM_DT AND @TO_DT";

        /// <summary>
        /// Get measure data
        /// </summary>
        public const string _sqlMeasureData =
            @"SELECT A.PLANT_CODE									                            AS PLANT_CODE
                    ,A.FACTORY									                                AS FACTORY
                    ,A.WORKCENTER_CODE							                                AS WORKCENTER_CODE
                    ,A.LOT_NO                                                                   AS LOT_NO
                    ,A.DATA14                                                                   AS DATA14
                    ,A.DATA15                                                                   AS DATA15
                    ,A.DATA16                                                                   AS DATA16
                    ,A.DATA17                                                                   AS DATA17
                    ,A.DATA18                                                                   AS DATA18
                    ,A.DATA19                                                                   AS DATA19
                    ,A.DATA20                                                                   AS DATA20
                    ,A.DATA21                                                                   AS DATA21
                    ,A.DATA22                                                                   AS DATA22
                    ,A.DATA23                                                                   AS DATA23
                    ,A.DATA24                                                                   AS DATA24
                    ,A.DATA25                                                                   AS DATA25
                    ,A.LASTEVENT_TIME05                                                         AS LASTEVENT_TIME
                    ,A.LINE                                                                     AS LINE
                FROM TDS3001 A WITH(NOLOCK)
               WHERE 1 = 1
                 AND ISNULL(A.OPERATION05, '')	            <> ''
                 AND A.PLANT_CODE		                    = @PLANT_CODE
                 AND A.WORKCENTER_CODE                      = @WORKCENTER_CODE
                 AND A.LASTEVENT_TIME05                     BETWEEN @FROM_DT AND @TO_DT
                 AND ISNULL(A.JUDGE24, '')                  = 'OK'

              UNION ALL

              SELECT A.PLANT_CODE									                            AS PLANT_CODE
                    ,A.FACTORY									                                AS FACTORY
                    ,A.WORKCENTER_CODE							                                AS WORKCENTER_CODE
                    ,A.LOT_NO                                                                   AS LOT_NO
                    ,A.DATA14                                                                   AS DATA14
                    ,A.DATA15                                                                   AS DATA15
                    ,A.DATA16                                                                   AS DATA16
                    ,A.DATA17                                                                   AS DATA17
                    ,A.DATA18                                                                   AS DATA18
                    ,A.DATA19                                                                   AS DATA19
                    ,A.DATA20                                                                   AS DATA20
                    ,A.DATA21                                                                   AS DATA21
                    ,A.DATA22                                                                   AS DATA22
                    ,A.DATA23                                                                   AS DATA23
                    ,A.DATA24                                                                   AS DATA24
                    ,A.DATA25                                                                   AS DATA25
                    ,A.LASTEVENT_TIME05                                                         AS LASTEVENT_TIME
                    ,A.LINE                                                                     AS LINE
                FROM TDS3002 A WITH(NOLOCK)
               WHERE 1 = 1
                 AND ISNULL(A.OPERATION05, '')	            <> ''
                 AND A.PLANT_CODE		                    = @PLANT_CODE
                 AND A.WORKCENTER_CODE                      = @WORKCENTER_CODE
                 AND A.LASTEVENT_TIME05                     BETWEEN @FROM_DT AND @TO_DT
                 AND ISNULL(A.JUDGE24, '')                  = 'OK';";

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
