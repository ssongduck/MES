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

namespace SAMMI.PP
{
    /// <summary>
    /// QM0405 class
    /// </summary>
    public partial class PP0410 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        public PP0410()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// PP0410 Disposed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP0410_Disposed(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// btn Confirm1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm1_Click(object sender, EventArgs e)
        {
            string flag = "1";

            if (!string.IsNullOrEmpty(lbl_OrderNo.Text))
            {
                if (this.ShowDialog(" 조장 검토 확인란 입니다. \r\n 작업일보를 승인 하시겠습니까?") == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable dt = new DataTable();

                    SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
                    SqlParameter[] sqlParameters1 = new SqlParameter[6];

                    try
                    {
                        string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                        sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", lbl_WorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_RECDATE", lbl_WorkDT.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_ORDERNO", lbl_OrderNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_REMARK", txt_Remark.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[5] = sqlDBHelper.CreateParameter("@AS_FLAG", flag, SqlDbType.VarChar, ParameterDirection.Input);

                        sqlDBHelper.ExecuteNoneQuery("SP_SAVE_WORKREPORT_CONFIRM", CommandType.StoredProcedure, sqlParameters1);
                    }
                    catch
                    {
                        Console.WriteLine("저장오류");
                    }
                    finally
                    {
                        if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                        if (sqlParameters1 != null) { sqlParameters1 = null; }

                        ClearChartControl();
                        ClearUserControl();
                        DoInquire();
                        base.ClosePrgForm();
                    }
                }
            }
        }

        /// <summary>
        /// btn Confirm2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm2_Click(object sender, EventArgs e)
        {
            string flag = "2";

            if (!string.IsNullOrEmpty(lbl_OrderNo.Text))
            {
                if (this.ShowDialog(" 반장/기장 검토 확인란 입니다. \r\n 작업일보를 승인 하시겠습니까?") == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable dt = new DataTable();

                    SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
                    SqlParameter[] sqlParameters1 = new SqlParameter[6];

                    try
                    {
                        string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                        sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", lbl_WorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_RECDATE", lbl_WorkDT.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_ORDERNO", lbl_OrderNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_REMARK", txt_Remark.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[5] = sqlDBHelper.CreateParameter("@AS_FLAG", flag, SqlDbType.VarChar, ParameterDirection.Input);

                        sqlDBHelper.ExecuteNoneQuery("SP_SAVE_WORKREPORT_CONFIRM", CommandType.StoredProcedure, sqlParameters1);
                    }
                    catch
                    {
                        Console.WriteLine("저장오류");
                    }
                    finally
                    {
                        if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                        if (sqlParameters1 != null) { sqlParameters1 = null; }

                        ClearChartControl();
                        ClearUserControl();
                        DoInquire();
                        base.ClosePrgForm();
                    }
                }
            }
        }

        /// <summary>
        /// btn Confirm3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm3_Click(object sender, EventArgs e)
        {
            string flag = "3";

            if (!string.IsNullOrEmpty(lbl_OrderNo.Text))
            {
                if (this.ShowDialog(" 팀장 검토 확인란 입니다. \r\n 작업일보를 승인 하시겠습니까?") == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable dt = new DataTable();

                    SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
                    SqlParameter[] sqlParameters1 = new SqlParameter[6];

                    try
                    {
                        string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                        sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", lbl_WorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_RECDATE", lbl_WorkDT.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_ORDERNO", lbl_OrderNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_REMARK", txt_Remark.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[5] = sqlDBHelper.CreateParameter("@AS_FLAG", flag, SqlDbType.VarChar, ParameterDirection.Input);

                        sqlDBHelper.ExecuteNoneQuery("SP_SAVE_WORKREPORT_CONFIRM", CommandType.StoredProcedure, sqlParameters1);
                    }
                    catch
                    {
                        Console.WriteLine("저장오류");
                    }
                    finally
                    {
                        if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                        if (sqlParameters1 != null) { sqlParameters1 = null; }

                        ClearChartControl();
                        ClearUserControl();
                        DoInquire();
                        base.ClosePrgForm();
                    }
                }
            }
        }

        /// <summary>
        /// btn Confirm4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm4_Click(object sender, EventArgs e)
        {
            string flag = "4";

            if (!string.IsNullOrEmpty(lbl_OrderNo.Text))
            {
                if (this.ShowDialog(" 임원 검토 확인란 입니다. \r\n 작업일보를 승인 하시겠습니까?") == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable dt = new DataTable();

                    SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
                    SqlParameter[] sqlParameters1 = new SqlParameter[6];

                    try
                    {
                        string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                        sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", lbl_WorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_RECDATE", lbl_WorkDT.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_ORDERNO", lbl_OrderNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_REMARK", txt_Remark.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[5] = sqlDBHelper.CreateParameter("@AS_FLAG", flag, SqlDbType.VarChar, ParameterDirection.Input);

                        sqlDBHelper.ExecuteNoneQuery("SP_SAVE_WORKREPORT_CONFIRM", CommandType.StoredProcedure, sqlParameters1);
                    }
                    catch
                    {
                        Console.WriteLine("저장오류");
                    }
                    finally
                    {
                        if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                        if (sqlParameters1 != null) { sqlParameters1 = null; }

                        ClearChartControl();
                        ClearUserControl();
                        DoInquire();
                        base.ClosePrgForm();
                    }
                }
            }
        }

        /// <summary>
        /// btn Confirm5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm5_Click(object sender, EventArgs e)
        {
            string flag = "5";

            if (!string.IsNullOrEmpty(lbl_OrderNo.Text))
            {
                if (this.ShowDialog(" 대표님 최종 승인 확인란 입니다. \r\n 작업일보를 승인 하시겠습니까?") == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable dt = new DataTable();

                    SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
                    SqlParameter[] sqlParameters1 = new SqlParameter[6];

                    try
                    {
                        string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                        sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", lbl_WorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_RECDATE", lbl_WorkDT.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_ORDERNO", lbl_OrderNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_REMARK", txt_Remark.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[5] = sqlDBHelper.CreateParameter("@AS_FLAG", flag, SqlDbType.VarChar, ParameterDirection.Input);

                        sqlDBHelper.ExecuteNoneQuery("SP_SAVE_WORKREPORT_CONFIRM", CommandType.StoredProcedure, sqlParameters1);
                    }
                    catch
                    {
                        Console.WriteLine("저장오류");
                    }
                    finally
                    {
                        if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                        if (sqlParameters1 != null) { sqlParameters1 = null; }

                        ClearChartControl();
                        ClearUserControl();
                        DoInquire();
                        base.ClosePrgForm();
                    }
                }
            }
        }

        /// <summary>
        /// btn remark
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemark_Click(object sender, EventArgs e)
        {
            string flag = string.Empty;

            if (!string.IsNullOrEmpty(lbl_OrderNo.Text))
            {
                if (this.ShowDialog(" 비고를 저장 하시겠습니까?") == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable dt = new DataTable();

                    SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
                    SqlParameter[] sqlParameters1 = new SqlParameter[6];

                    try
                    {
                        string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                        sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", lbl_WorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_RECDATE", lbl_WorkDT.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_ORDERNO", lbl_OrderNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_REMARK", txt_Remark.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters1[5] = sqlDBHelper.CreateParameter("@AS_FLAG", flag, SqlDbType.VarChar, ParameterDirection.Input);

                        sqlDBHelper.ExecuteNoneQuery("SP_SAVE_WORKREPORT_CONFIRM", CommandType.StoredProcedure, sqlParameters1);
                    }
                    catch
                    {
                        Console.WriteLine("저장오류");
                    }
                    finally
                    {
                        if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                        if (sqlParameters1 != null) { sqlParameters1 = null; }
                    }

                    ClearChartControl();
                    ClearUserControl();
                    DoInquire();
                    base.ClosePrgForm();
                }
            }
        }

        /// <summary>
        /// btnDay1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDay1_Click(object sender, EventArgs e)
        {
            DateTime from_dt = DateTime.Parse(string.Format("{0}{1}", lbl_WorkDT.Text, " 08:00"));
            DateTime to_dt = DateTime.Parse(string.Format("{0}{1}", lbl_WorkDT.Text, " 12:00"));

            GetCastCondition(lbl_WorkCenterCode.Text, lbl_OrderNo.Text, from_dt, to_dt);
        }

        /// <summary>
        /// btnDay2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDay2_Click(object sender, EventArgs e)
        {
            DateTime from_dt = DateTime.Parse(string.Format("{0}{1}", lbl_WorkDT.Text, " 12:00"));
            DateTime to_dt = DateTime.Parse(string.Format("{0}{1}", lbl_WorkDT.Text, " 16:00"));

            GetCastCondition(lbl_WorkCenterCode.Text, lbl_OrderNo.Text, from_dt, to_dt);
        }

        /// <summary>
        /// btnDay3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDay3_Click(object sender, EventArgs e)
        {
            DateTime from_dt = DateTime.Parse(string.Format("{0}{1}", lbl_WorkDT.Text, " 16:00"));
            DateTime to_dt = DateTime.Parse(string.Format("{0}{1}", lbl_WorkDT.Text, " 20:00"));

            GetCastCondition(lbl_WorkCenterCode.Text, lbl_OrderNo.Text, from_dt, to_dt);
        }

        /// <summary>
        /// btnDay4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDay4_Click(object sender, EventArgs e)
        {
            DateTime from_dt = DateTime.Parse(string.Format("{0}{1}", lbl_WorkDT.Text, " 20:00"));
            DateTime to_dt = DateTime.Parse(string.Format("{0}{1}", DateTime.Parse(lbl_WorkDT.Text).AddDays(1).ToString("yyyy-MM-dd"), " 00:00"));

            GetCastCondition(lbl_WorkCenterCode.Text, lbl_OrderNo.Text, from_dt, to_dt);
        }

        /// <summary>
        /// btnDay5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDay5_Click(object sender, EventArgs e)
        {
            DateTime from_dt = DateTime.Parse(string.Format("{0}{1}", DateTime.Parse(lbl_WorkDT.Text).AddDays(1).ToString("yyyy-MM-dd"), " 00:00"));
            DateTime to_dt = DateTime.Parse(string.Format("{0}{1}", DateTime.Parse(lbl_WorkDT.Text).AddDays(1).ToString("yyyy-MM-dd"), " 04:00"));

            GetCastCondition(lbl_WorkCenterCode.Text, lbl_OrderNo.Text, from_dt, to_dt);
        }

        /// <summary>
        /// btnDay6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDay6_Click(object sender, EventArgs e)
        {
            DateTime from_dt = DateTime.Parse(string.Format("{0}{1}", DateTime.Parse(lbl_WorkDT.Text).AddDays(1).ToString("yyyy-MM-dd"), " 04:00"));
            DateTime to_dt = DateTime.Parse(string.Format("{0}{1}", DateTime.Parse(lbl_WorkDT.Text).AddDays(1).ToString("yyyy-MM-dd"), " 08:00"));

            GetCastCondition(lbl_WorkCenterCode.Text, lbl_OrderNo.Text, from_dt, to_dt);
        }

        /// <summary>
        /// btnDay7
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDay7_Click(object sender, EventArgs e)
        {
            DateTime from_dt = DateTime.Parse(string.Format("{0}{1}", lbl_WorkDT.Text, " 08:00"));
            DateTime to_dt = DateTime.Parse(string.Format("{0}{1}", DateTime.Parse(lbl_WorkDT.Text).AddDays(1).ToString("yyyy-MM-dd"), " 08:00"));

            GetCastCondition(lbl_WorkCenterCode.Text, lbl_OrderNo.Text, from_dt, to_dt);
        }

        /// <summary>
        /// gridView workreport rowclick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewWorkReportlist_RowClick(object sender, RowClickEventArgs e)
        {
            ClearUserControl();

            DataRowView dataRowView = gridViewWorkReportlist.GetRow(e.RowHandle) as DataRowView;

            if (dataRowView != null)
            {
                if (dataRowView.Row != null)
                {
                    lbl_WorkDT.Text = dataRowView.Row["RECDATE"].ToString();
                    lbl_WorkCenterCode.Text = dataRowView.Row["WORKCENTERCODE"].ToString();
                    lbl_WorkCenterName.Text = dataRowView.Row["WORKCENTERNAME"].ToString();
                    lbl_OrderNo.Text = dataRowView.Row["ORDERNO"].ToString();
                    lbl_Worker.Text = dataRowView.Row["WORKER"].ToString();
                    lbl_CreateDT.Text = dataRowView.Row["CREATE_DT"].ToString();
                    lbl_ItemName.Text = dataRowView.Row["ITEMNAME"].ToString();
                    lbl_ItemCode.Text = dataRowView.Row["ITEMCODE"].ToString();
                    lbl_MoldName.Text = dataRowView.Row["MOLDCODE"].ToString();
                    lbl_Cavity.Text = dataRowView.Row["CAVITY"].ToString();
                    lbl_OrderQty.Text = dataRowView.Row["ORDERQTY"].ToString();
                    lbl_ShotQty_H.Text = dataRowView.Row["SHOTQTY_H"].ToString();
                    lbl_ShotQty_L.Text = dataRowView.Row["SHOTQTY_L"].ToString();
                    lbl_ShotQty_T.Text = dataRowView.Row["SHOTQTY_T"].ToString();
                    lbl_ProdQty_D.Text = dataRowView.Row["LOTQTY_D"].ToString();
                    lbl_ProdQty_N.Text = dataRowView.Row["LOTQTY_N"].ToString();
                    lbl_ProdQty_T.Text = dataRowView.Row["LOTQTY_T"].ToString();
                    lbl_BadQty_D.Text = dataRowView.Row["BADQTY_D"].ToString();
                    lbl_BadQty_N.Text = dataRowView.Row["BADQTY_N"].ToString();
                    lbl_BadQty_T.Text = dataRowView.Row["BADQTY_T"].ToString();
                    lbl_DeadQty_D.Text = dataRowView.Row["DEADQTY_D"].ToString();
                    lbl_DeadQty_N.Text = dataRowView.Row["DEADQTY_N"].ToString();
                    lbl_DeadQty_T.Text = dataRowView.Row["DEADQTY_T"].ToString();
                    lbl_Downtime_D.Text = dataRowView.Row["DOWNTIME_D"].ToString();
                    lbl_Downtime_N.Text = dataRowView.Row["DOWNTIME_N"].ToString();
                    lbl_Downtime_T.Text = dataRowView.Row["DOWNTIME_T"].ToString();

                    lbl_ConfirmDt1.Text = dataRowView.Row["CONFIRM1_DT"].ToString();
                    lbl_Confirm1.Text = dataRowView.Row["CONFIRM1"].ToString();
                    lbl_ConfirmDt2.Text = dataRowView.Row["CONFIRM2_DT"].ToString();
                    lbl_Confirm2.Text = dataRowView.Row["CONFIRM2"].ToString();
                    lbl_ConfirmDt3.Text = dataRowView.Row["CONFIRM3_DT"].ToString();
                    lbl_Confirm3.Text = dataRowView.Row["CONFIRM3"].ToString();
                    lbl_ConfirmDt4.Text = dataRowView.Row["CONFIRM4_DT"].ToString();
                    lbl_Confirm4.Text = dataRowView.Row["CONFIRM4"].ToString();
                    lbl_ConfirmDt5.Text = dataRowView.Row["CONFIRM5_DT"].ToString();
                    lbl_Confirm5.Text = dataRowView.Row["CONFIRM5"].ToString();

                    txt_Remark.Text = dataRowView.Row["REMARK"].ToString();

                    DateTime from_dt = DateTime.Parse(string.Format("{0}{1}", lbl_WorkDT.Text, " 08:00"));
                    DateTime to_dt = DateTime.Parse(string.Format("{0}{1}", DateTime.Parse(lbl_WorkDT.Text).AddDays(1).ToString("yyyy-MM-dd"), " 08:00"));

                    GetCastCondition(lbl_WorkCenterCode.Text, lbl_OrderNo.Text, from_dt, to_dt);
                }
            }
        }

        /// <summary>
        /// gridViewWorkReportlist_PopupMenuShowing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewWorkReportlist_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.Menu == null) return;

                DXMenuItem dXMenuItem = new DevExpress.Utils.Menu.DXMenuItem("엑셀 내보내기", new EventHandler(this.exportMenu_Click));
                e.Menu.Items.Add(dXMenuItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// gridViewWorkReportlist_RowStyle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewWorkReportlist_RowStyle(object sender, RowStyleEventArgs e)
        {
            string Confirm1 = string.Empty;
            string Confirm2 = string.Empty;
            string Confirm3 = string.Empty;
            string Confirm4 = string.Empty;
            string Confirm5 = string.Empty;
            
            GridView gridView = sender as GridView;

            if (e.RowHandle >= 0)
            {
                Confirm1 = gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["CONFIRM1"]);
                Confirm2 = gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["CONFIRM2"]);
                Confirm3 = gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["CONFIRM3"]);
                Confirm4 = gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["CONFIRM4"]);
                Confirm5 = gridView.GetRowCellDisplayText(e.RowHandle, gridView.Columns["CONFIRM5"]);

                if (Confirm1 == "승인")
                {
                    gridView.Columns["CONFIRM1"].AppearanceCell.BackColor = Color.FromArgb(100, 90, 255);
                    gridView.Columns["CONFIRM1"].AppearanceCell.ForeColor = Color.FromArgb(255, 255, 255);
                }
                else if (Confirm1 == "미승인")
                {
                    gridView.Columns["CONFIRM1"].AppearanceCell.BackColor = Color.FromArgb(255, 0, 0);
                    gridView.Columns["CONFIRM1"].AppearanceCell.ForeColor = Color.FromArgb(255, 255, 255);
                }

                if (Confirm2 == "승인")
                {
                    gridView.Columns["CONFIRM2"].AppearanceCell.BackColor = Color.FromArgb(100, 90, 255);
                    gridView.Columns["CONFIRM2"].AppearanceCell.ForeColor = Color.FromArgb(255, 255, 255);
                }
                else if (Confirm2 == "미승인")
                {
                    gridView.Columns["CONFIRM2"].AppearanceCell.BackColor = Color.FromArgb(255, 0, 0);
                    gridView.Columns["CONFIRM2"].AppearanceCell.ForeColor = Color.FromArgb(255, 255, 255);
                }

                if (Confirm3 == "승인")
                {
                    gridView.Columns["CONFIRM3"].AppearanceCell.BackColor = Color.FromArgb(100, 90, 255);
                    gridView.Columns["CONFIRM3"].AppearanceCell.ForeColor = Color.FromArgb(255, 255, 255);
                }
                else if (Confirm3 == "미승인")
                {
                    gridView.Columns["CONFIRM3"].AppearanceCell.BackColor = Color.FromArgb(255, 0, 0);
                    gridView.Columns["CONFIRM3"].AppearanceCell.ForeColor = Color.FromArgb(255, 255, 255);
                }

                if (Confirm4 == "승인")
                {
                    gridView.Columns["CONFIRM4"].AppearanceCell.BackColor = Color.FromArgb(100, 90, 255);
                    gridView.Columns["CONFIRM4"].AppearanceCell.ForeColor = Color.FromArgb(255, 255, 255);
                }
                else if (Confirm4 == "미승인")
                {
                    gridView.Columns["CONFIRM4"].AppearanceCell.BackColor = Color.FromArgb(255, 0, 0);
                    gridView.Columns["CONFIRM4"].AppearanceCell.ForeColor = Color.FromArgb(255, 255, 255);
                }

                if (Confirm5 == "승인")
                {
                    gridView.Columns["CONFIRM5"].AppearanceCell.BackColor = Color.FromArgb(100, 90, 255);
                    gridView.Columns["CONFIRM5"].AppearanceCell.ForeColor = Color.FromArgb(255, 255, 255);
                }
                else if (Confirm5 == "미승인")
                {
                    gridView.Columns["CONFIRM5"].AppearanceCell.BackColor = Color.FromArgb(255, 0, 0);
                    gridView.Columns["CONFIRM5"].AppearanceCell.ForeColor = Color.FromArgb(255, 255, 255);
                }
            }
        }

        /// <summary>
        /// gridViewCastCondition_RowClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCastCondition_RowClick(object sender, RowClickEventArgs e)
        {
            DataTable dt = new DataTable();
            string shotNo = string.Empty;
            string asxiDateTime = string.Empty;

            DataRowView dataRowView = gridViewCastCondition.GetRow(e.RowHandle) as DataRowView;

            if (dataRowView != null)
            {
                if (dataRowView.Row != null)
                {
                    shotNo = dataRowView.Row["SHOTNO"].ToString();

                    if (chartCastCondition.Series != null && chartCastCondition.Series.Count > 0)
                    {
                        foreach (Series series in chartCastCondition.Series)
                        {
                            foreach (SeriesPoint seriesPoint in series.Points)
                            {
                                if (seriesPoint.Tag.ToString().Contains("ShotNo:" + shotNo))
                                {
                                    asxiDateTime = seriesPoint.Argument;
                                    seriesPoint.Color = Color.Red;
                                }
                                else
                                {
                                    seriesPoint.Color = Color.Transparent;
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(asxiDateTime))
                        {
                            Console.WriteLine("해당 쇼트번호는 존재하지 않습니다.");
                            return;
                        }
                    }
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

            if (this._sPlantCode.Equals("SK1"))
            {
                cboPlantCode_H.Value = "SK1";
                this._sPlantCode = "SK1";
            }
            else if (this._sPlantCode.Equals("SK2"))
            {
                cboPlantCode_H.Value = "SK2";
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

            btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
            btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOpCode, "", "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });

            ClearUserControl();
            ClearChartControl();

            fromDt.Value = DateTime.Now;
            toDt.Value = DateTime.Now;
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
            this.Disposed += new EventHandler(PP0410_Disposed);

            btnConfirm1.Click += new EventHandler(btnConfirm1_Click);
            btnConfirm2.Click += new EventHandler(btnConfirm2_Click);
            btnConfirm3.Click += new EventHandler(btnConfirm3_Click);
            btnConfirm4.Click += new EventHandler(btnConfirm4_Click);
            btnConfirm5.Click += new EventHandler(btnConfirm5_Click);
            btnRemark.Click += new EventHandler(btnRemark_Click);

            btnDay1.Click += new EventHandler(btnDay1_Click);
            btnDay2.Click += new EventHandler(btnDay2_Click);
            btnDay3.Click += new EventHandler(btnDay3_Click);
            btnDay4.Click += new EventHandler(btnDay4_Click);
            btnDay5.Click += new EventHandler(btnDay5_Click);
            btnDay6.Click += new EventHandler(btnDay6_Click);
            btnDay7.Click += new EventHandler(btnDay7_Click);

            gridViewWorkReportlist.RowClick += new RowClickEventHandler(gridViewWorkReportlist_RowClick);
            gridViewWorkReportlist.PopupMenuShowing += new PopupMenuShowingEventHandler(gridViewWorkReportlist_PopupMenuShowing);
            gridViewWorkReportlist.RowStyle += new RowStyleEventHandler(gridViewWorkReportlist_RowStyle);
            gridViewCastCondition.RowClick += new RowClickEventHandler(gridViewCastCondition_RowClick);
            
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(PP0410_Disposed);

            btnConfirm1.Click -= new EventHandler(btnConfirm1_Click);
            btnConfirm2.Click -= new EventHandler(btnConfirm2_Click);
            btnConfirm3.Click -= new EventHandler(btnConfirm3_Click);
            btnConfirm4.Click -= new EventHandler(btnConfirm4_Click);
            btnConfirm5.Click -= new EventHandler(btnConfirm5_Click);
            btnRemark.Click -= new EventHandler(btnRemark_Click);

            btnDay1.Click -= new EventHandler(btnDay1_Click);
            btnDay2.Click -= new EventHandler(btnDay2_Click);
            btnDay3.Click -= new EventHandler(btnDay3_Click);
            btnDay4.Click -= new EventHandler(btnDay4_Click);
            btnDay5.Click -= new EventHandler(btnDay5_Click);
            btnDay6.Click -= new EventHandler(btnDay6_Click);
            btnDay7.Click -= new EventHandler(btnDay7_Click);

            gridViewWorkReportlist.RowClick -= new RowClickEventHandler(gridViewWorkReportlist_RowClick);
            gridViewWorkReportlist.PopupMenuShowing -= new PopupMenuShowingEventHandler(gridViewWorkReportlist_PopupMenuShowing);
            gridViewWorkReportlist.RowStyle -= new RowStyleEventHandler(gridViewWorkReportlist_RowStyle);
            gridViewCastCondition.RowClick -= new RowClickEventHandler(gridViewCastCondition_RowClick);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            DataTable dt = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
            SqlParameter[] sqlParameters1 = new SqlParameter[5];

            ClearAllControl();
            ClearUserControl();

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_OPCODE", txtOpCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", fromDt.Value), SqlDbType.DateTime, ParameterDirection.Input);
                sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", toDt.Value), SqlDbType.DateTime, ParameterDirection.Input);
                
                dt = sqlDBHelper.FillTable("SP_GET_WORKREPORT_LIST", CommandType.StoredProcedure, sqlParameters1);

                gridWorkReportlist.DataSource = dt;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                if (sqlParameters1 != null) { sqlParameters1 = null; }
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
        /// Clear user control
        /// </summary>
        private void ClearUserControl()
        {
            lbl_WorkDT.Text = string.Empty;
            lbl_WorkCenterCode.Text = string.Empty;
            lbl_WorkCenterName.Text = string.Empty;
            lbl_OrderNo.Text = string.Empty;
            lbl_Worker.Text = string.Empty;
            lbl_CreateDT.Text = string.Empty;
            lbl_ItemName.Text = string.Empty;
            lbl_ItemCode.Text = string.Empty;
            lbl_MoldName.Text = string.Empty;
            lbl_Cavity.Text = string.Empty;
            lbl_OrderQty.Text = string.Empty;
            lbl_ShotQty_H.Text = string.Empty;
            lbl_ShotQty_L.Text = string.Empty;
            lbl_ShotQty_T.Text = string.Empty;
            lbl_ProdQty_D.Text = string.Empty;
            lbl_ProdQty_N.Text = string.Empty;
            lbl_ProdQty_T.Text = string.Empty;
            lbl_BadQty_D.Text = string.Empty;
            lbl_BadQty_N.Text = string.Empty;
            lbl_BadQty_T.Text = string.Empty;
            lbl_DeadQty_D.Text = string.Empty;
            lbl_DeadQty_N.Text = string.Empty;
            lbl_DeadQty_T.Text = string.Empty;
            lbl_Downtime_D.Text = string.Empty;
            lbl_Downtime_N.Text = string.Empty;
            lbl_Downtime_T.Text = string.Empty;

            lbl_ConfirmDt1.Text = string.Empty;
            lbl_Confirm1.Text = string.Empty;
            lbl_ConfirmDt2.Text = string.Empty;
            lbl_Confirm2.Text = string.Empty;
            lbl_ConfirmDt3.Text = string.Empty;
            lbl_Confirm3.Text = string.Empty;
            lbl_ConfirmDt4.Text = string.Empty;
            lbl_Confirm4.Text = string.Empty;
            lbl_ConfirmDt5.Text = string.Empty;
            lbl_Confirm5.Text = string.Empty;

            txt_Remark.Text = string.Empty;
        }

        /// <summary>
        /// Clear chart control
        /// </summary>
        private void ClearChartControl()
        {
            gridCastCondition.DataSource = null;
            BindChartCastCondition(null);
            chartCastCondition.Series.Clear();
        }

        /// <summary>
        /// Clear all control
        /// </summary>
        private void ClearAllControl()
        {
            ClearControl(this);
        }

        /// <summary>
        /// Get Cast Condition
        /// </summary>
        private void GetCastCondition(string sWorkcentercode, string sOrderNo, DateTime from_dt, DateTime to_dt)
        {

            DataTable dt = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
            SqlParameter[] sqlParameters1 = new SqlParameter[6];

            ClearAllControl();
            ClearChartControl();

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", sWorkcentercode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_RECDATE", lbl_WorkDT.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_ORDERNO", sOrderNo, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_FROM_DT", from_dt, SqlDbType.DateTime, ParameterDirection.Input);
                sqlParameters1[5] = sqlDBHelper.CreateParameter("@AS_TO_DT", to_dt, SqlDbType.DateTime, ParameterDirection.Input);

                dt = sqlDBHelper.FillTable("SP_GET_WORKREPORT_SHOT", CommandType.StoredProcedure, sqlParameters1);

                if (dt != null && dt.Rows.Count > 0)
                {
                    gridCastCondition.DataSource = dt;
                    BindChartCastCondition(dt);
                }
                else
                {
                    gridCastCondition.DataSource = null;
                    chartCastCondition.Series.Clear();
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                if (sqlParameters1 != null) { sqlParameters1 = null; }
                base.ClosePrgForm();
            }
        }

        /// <summary>
        /// Chart BindChartCastCondition
        /// </summary>
        /// <param name="dt"></param>
        private void BindChartCastCondition(DataTable dt)
        {
            try
            {
                List<string> lineChartList = GetLineChartList();

                if (lineChartList == null)
                {
                    lineChartList.Clear();
                    return;
                }

                if (dt != null)
                {
                    #region Chart

                    XYDiagram tempXYDiagram = chartCastCondition.Diagram as XYDiagram;

                    if (tempXYDiagram != null && tempXYDiagram.Panes.Count > 0)
                    {
                        tempXYDiagram.Panes.Clear();
                        tempXYDiagram.SecondaryAxesY.Clear();
                    }

                    if (chartCastCondition.Series.Count > 0)
                    {
                        chartCastCondition.Series.Clear();
                    }

                    chartCastCondition.AutoLayout = false;
                    chartCastCondition.CrosshairOptions.ArgumentLineColor = System.Drawing.Color.DarkGreen;
                    chartCastCondition.CrosshairOptions.ArgumentLineStyle.Thickness = 2;
                    chartCastCondition.CrosshairOptions.ShowOnlyInFocusedPane = false;

                    chartCastCondition.CrosshairOptions.GroupHeaderPattern = "<b>{A:yyyy-MM-dd HH:mm:ss}</b>";

                    chartCastCondition.CrosshairOptions.ShowValueLabels = true;
                    chartCastCondition.CrosshairOptions.ShowValueLine = true;
                    chartCastCondition.CrosshairOptions.ArgumentLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    chartCastCondition.CrosshairOptions.ValueLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

                    GetSeriesList(dt, lineChartList);

                    #endregion
                }
                else
                {
                    XYDiagram tempXYDiagram = chartCastCondition.Diagram as XYDiagram;

                    if (tempXYDiagram != null && tempXYDiagram.Panes.Count > 0)
                    {
                        tempXYDiagram.Panes.Clear();
                        tempXYDiagram.SecondaryAxesY.Clear();
                    }

                    chartCastCondition.Series.Clear();
                    lineChartList.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// Get series list
        /// </summary>
        private void GetSeriesList(DataTable dt, List<string> lineChartList)
        {
            List<Series> seriesList = new List<Series>();

            string typeName = string.Empty;
            string shotNo = string.Empty;
            string labelTextPattern = string.Empty;

            foreach (string type in lineChartList)
            {
                switch (type)
                {
                    case "CASTTEMP":
                        typeName = "용탕온도";
                        labelTextPattern = "{V:F0}";
                        break;

                    case "DATA04":
                        typeName = "고속구간(V4)";
                        labelTextPattern = "{V:F2}";
                        break;

                    case "DATA07":
                        typeName = "비스켓두께";
                        labelTextPattern = "{V:F0}";
                        break;

                    case "DATA08":
                        typeName = "승압시간";
                        labelTextPattern = "{V:F0}";
                        break;
                }

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

                Series series = new Series(string.Format("{0}", typeName), ViewType.Line);
                series.Tag = minVal.ToString() + "|" + maxVal.ToString() + "|" + typeName;

                series.ArgumentScaleType = ScaleType.DateTime;
                series.ValueScaleType = ScaleType.Numerical;
                series.Visible = true;

                foreach (DataRow dr in dt.AsEnumerable())
                {
                    SeriesPoint seriesPoint = new SeriesPoint(DateTime.Parse(dr["CREATE_DT"].ToString()), Decimal.Parse(dr[type].ToString()));
                    seriesPoint.Tag = string.Format("{0} : {1} (ShotNo:{2}))", typeName, Decimal.Parse(dr[type].ToString()).ToString(labelTextPattern.Replace("{V:", string.Empty).Replace("}", string.Empty)), dr["SHOTNO"]);

                    series.Points.Add(seriesPoint);
                }

                seriesList.Add(series);

                ((LineSeriesView)series.View).LineMarkerOptions.Size = 7;
                ((LineSeriesView)series.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Solid;

            }

            if (seriesList != null && seriesList.Count > 0)
            {
                foreach (Series series in seriesList)
                {
                    chartCastCondition.Series.Add(series);
                }

                XYDiagram diagram = chartCastCondition.Diagram as XYDiagram;

                List<string> valList = seriesList[0].Tag.ToString().Split('|').ToList();

                for (int i = 1; i < seriesList.Count; i++)
                {
                    LineChartOption(diagram, seriesList[i], labelTextPattern);
                }

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
                diagram.AxisY.Label.TextPattern = labelTextPattern;
                diagram.AxisY.WholeRange.Auto = true;
                diagram.AxisY.WholeRange.SetMinMaxValues(decimal.Parse(valList[0]), decimal.Parse(valList[1]));

                diagram.AxisX.GridLines.Visible = true;
                diagram.AxisX.Interlaced = true;
                diagram.AxisX.Label.Staggered = false;
                diagram.AxisX.Title.Text = "Date";

                diagram.AxisX.VisibleInPanesSerializable = (lineChartList.Count - 2).ToString();
                diagram.AxisX.VisualRange.Auto = true;
                diagram.AxisY.GridLines.MinorVisible = true;
                diagram.AxisY.Title.Text = valList[2];
                diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.VisibleInPanesSerializable = "-1";
                diagram.AxisY.Title.Font = new System.Drawing.Font("NanumGothicExtraBold", 7F);
                diagram.AxisY.Label.Font = new System.Drawing.Font("NanumGothic", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }

        }

        /// <summary>
        /// Line chart option
        /// </summary>
        /// <param name="diagram"></param>
        /// <param name="series"></param>
        /// <param name="labelTextPattern"></param>
        private void LineChartOption(XYDiagram diagram, Series series, string labelTextPattern)
        {
            List<string> valList = series.Tag.ToString().Split('|').ToList();
            LineSeriesView lineSeriesView = (LineSeriesView)series.View;

            diagram.Panes.Add(new XYDiagramPane(series.Name));

            SecondaryAxisY secondaryAxisY = new SecondaryAxisY();

            secondaryAxisY.Range.Auto = true;
            secondaryAxisY.Range.ScrollingRange.Auto = true;

            secondaryAxisY.Label.TextPattern = labelTextPattern;
            secondaryAxisY.WholeRange.Auto = true;
            secondaryAxisY.WholeRange.SetMinMaxValues(decimal.Parse(valList[0]), decimal.Parse(valList[1]));
            secondaryAxisY.GridLines.Visible = true;
            secondaryAxisY.Interlaced = true;
            secondaryAxisY.VisualRange.Auto = false;
            secondaryAxisY.Alignment = AxisAlignment.Near;
            secondaryAxisY.Title.Text = valList[2];
            secondaryAxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            secondaryAxisY.Title.Font = new System.Drawing.Font("NanumGothicExtraBold", 7F);
            secondaryAxisY.Label.Font = new System.Drawing.Font("NanumGothic", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            diagram.SecondaryAxesY.Add(secondaryAxisY);

            lineSeriesView.Pane = diagram.Panes[diagram.Panes.Count - 1];

            lineSeriesView.AxisY = secondaryAxisY;
        }

        /// <summary>
        /// Get Line Chart List
        /// </summary>
        /// <returns></returns>
        private List<string> GetLineChartList()
        {
            List<string> lineChartList = new List<string>();

            lineChartList.Add("CASTTEMP");  //용탕온도
            lineChartList.Add("DATA04");    //고속구간2
            lineChartList.Add("DATA07");    //비스켓두께
            lineChartList.Add("DATA08");    //승압시간

            return lineChartList;
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
        /// Export menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportMenu_Click(object sender, EventArgs e)
        {
            ExportExcel(gridViewWorkReportlist);
        }

        #endregion
    }
}
