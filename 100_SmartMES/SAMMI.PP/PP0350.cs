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

namespace SAMMI.PP
{
    /// <summary>
    /// PP0350 class
    /// </summary>
    public partial class PP0350 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common grid1 datatable
        /// </summary>
        DataTable _RtnComDt1 = new DataTable();

        /// <summary>
        /// Return common grid2 datatable
        /// </summary>
        DataTable _RtnComDt2 = new DataTable();

        /// <summary>
        /// Change grid1 datatable
        /// </summary>
        private DataTable _ChangeDt1 = null;

        /// <summary>
        /// Change grid2 datatable
        /// </summary>
        private DataTable _ChangeDt2 = null;

        /// <summary>
        /// Grid object
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Check variable
        /// </summary>
        string _Chk = string.Empty;

        /// <summary>
        /// Empty arrays
        /// </summary>
        string[] _EmptyArrs = { "v_txt" };

        #endregion

        #region Constructor

        /// <summary>
        /// PP0350 Constructor
        /// </summary>
        public PP0350()
        {
            InitializeComponent();
            InitializeGridControl();
            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// Grid1 Initialize row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["BUYERNAME"].Value.ToString() == "합 계")
            {
                e.Row.Appearance.BackColor = Color.LightCyan;
                e.Row.Appearance.FontData.Bold = DefaultableBoolean.True;
            }
        }

        /// <summary>
        /// Save button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Delete button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.DoDelete();
        }

        /// <summary>
        /// PP0350 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP0350_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();
        }

        #endregion

        #region Method

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

            foreach (System.Windows.Forms.Control ctl in control.Controls)
            {
                InitializeControl(ctl);

                if (ctl.GetType().Name == "TextBox")
                {
                    TextBox textBox = (TextBox)ctl;

                    foreach (string sVal in _EmptyArrs)
                    {
                        if (textBox.Name.StartsWith(sVal))
                        {
                            textBox.Text = string.Empty;
                        }
                    }
                }

                if (ctl.GetType().Name == "MaskedTextBox")
                {
                    MaskedTextBox maskedTextBox = (MaskedTextBox)ctl;

                    foreach (string sVal in _EmptyArrs)
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
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            // 1. Initialize grid1
            _UltraGridUtil.InitializeGrid(this.grid1, true, true, false, string.Empty, false);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "RECDATE", "등록일자", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "SEQ", "순번", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "MAKEDATE", "작업일시", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "BUYERNAME", "거래처", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "LOTNO", "LotNo", false, GridColDataType_emu.VarChar, 70, 10, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "TAPPINGTEMP", "출탕온도", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "VEHICLENO", "차량번호", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "LADLENO", "래들번호", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "INJECTIONWEIGHT", "주입중량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "DEPARTURETEMP", "출발온도", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "ARRIVALTEMP", "도착온도", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "ARRIVALDATE", "도착일시", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid1);

            _ChangeDt1 = (DataTable)grid1.DataSource;
            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);

            // 2. Initialize grid2
            _UltraGridUtil.InitializeGrid(this.grid2, true, true, false, string.Empty, false);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "RECDATE", "등록일자", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "SEQ", "순번", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "MAKEDATE", "작업일시", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "BUYERNAME", "거래처", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "LOTNO", "LotNo", false, GridColDataType_emu.VarChar, 70, 10, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "TAPPINGTEMP", "출탕온도", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "VEHICLENO", "차량번호", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "LADLENO", "래들번호", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "INJECTIONWEIGHT", "주입중량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "DEPARTURETEMP", "출발온도", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "ARRIVALTEMP", "도착온도", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "ARRIVALDATE", "도착일시", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid2);

            _ChangeDt2 = (DataTable)grid2.DataSource;
            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid2);
        }

        /// <summary>
        /// Clear all control
        /// </summary>
        private void ClearAllControl()
        {
            InitializeControl(this);
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            grid1.InitializeRow += new InitializeRowEventHandler(grid1_InitializeRow);

            this.Disposed += new EventHandler(PP0350_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            grid1.InitializeRow -= new InitializeRowEventHandler(grid1_InitializeRow);

            this.Disposed -= new EventHandler(PP0350_Disposed);
        }

        /// <summary>
        /// Do inqueries
        /// </summary>
        public override void DoInquire()
        {
            DataTable dt = null;
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[2];
            ClearAllControl();

            try
            {
                grid1.BeginUpdate();
                grid2.BeginUpdate();

                _ChangeDt1.Clear();
                _ChangeDt2.Clear();

                base.DoInquire();

                string sDtpDate = string.Format("{0:yyyy-MM-dd}", calRegDT_FRH.Value);
                string sDtpDateTo = string.Format("{0:yyyy-MM-dd}", calRegDT_TOH.Value);

                sqlParameters[0] = sqlDBHelper.CreateParameter("@StartDate", sDtpDate, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@EndDate", sDtpDateTo, SqlDbType.VarChar, ParameterDirection.Input);

                dt = sqlDBHelper.FillTable("USP_PP0350_S1", CommandType.StoredProcedure, sqlParameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1 && dt.Rows[0]["BUYERNAME"].ToString() == "합 계")
                    {
                        return;
                    }

                    _RtnComDt1 = dt.AsEnumerable().Where(t => t.Field<string>("BUYERNAME").Substring(t.Field<string>("BUYERNAME").Length - 1, 1) == "계").CopyToDataTable();
                    _RtnComDt2 = dt.AsEnumerable().Where(t => t.Field<string>("BUYERNAME").Substring(t.Field<string>("BUYERNAME").Length - 1, 1) != "계").CopyToDataTable();

                    _RtnComDt1.AcceptChanges();
                    _RtnComDt2.AcceptChanges();
                }

                grid1.DataSource = _RtnComDt1;
                grid1.DataBind();

                grid2.DataSource = _RtnComDt2;
                grid2.DataBind();

                _ChangeDt1 = _RtnComDt1;
                _ChangeDt2 = _RtnComDt2;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                grid1.EndUpdate();
                grid2.EndUpdate();

                _Chk = string.Empty;
            }
        }

        /// <summary>
        /// Do base sum(weight)
        /// </summary>
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ultraGridRow = grid1.DoSummaries(new string[] { "InjectionWeight" });
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
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = null;

            try
            {
                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow dr in _ChangeDt1.Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Deleted:
                            dr.RejectChanges();
                            sqlParameters = new SqlParameter[2];

                            sqlParameters[0] = sqlDBHelper.CreateParameter("@iDate", dr["iDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[1] = sqlDBHelper.CreateParameter("@LotNo", dr["LotNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);

                            sqlDBHelper.ExecuteNoneQuery("USP_PP0350_D1", CommandType.StoredProcedure, sqlParameters);

                            break;

                        case DataRowState.Modified:
                            sqlParameters = new SqlParameter[3];

                            sqlParameters[0] = sqlDBHelper.CreateParameter("@RecDate", dr["RecDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[1] = sqlDBHelper.CreateParameter("@Seq", dr["Seq"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[2] = sqlDBHelper.CreateParameter("@LotNo", dr["LotNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);

                            sqlDBHelper.ExecuteNoneQuery("USP_PP0350_U1", CommandType.StoredProcedure, sqlParameters);

                            break;
                    }
                }

                sqlDBHelper.Transaction.Commit();
                ClearAllControl();
            }
            catch (Exception ex)
            {
                sqlDBHelper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (sqlDBHelper._sConn != null)
                {
                    sqlDBHelper._sConn.Close();
                }

                if (sqlParameters != null)
                {
                    sqlParameters = null;
                }

                _Chk = string.Empty;
            }
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
            bool bFlag = true;

            for (int i = 0; i < grid2.Selected.Rows.Count; i++)
            {
                if (grid2.Selected.Rows[i].Cells["UD"].Value.ToString() == "F")
                {
                    grid2.Selected.Rows[i].Selected = false;
                    bFlag = false;
                }
            }

            if (bFlag == false)
            {
                MessageBox.Show("삭제 할수 없는 행이 포함되었습니다.");
                return;
            }

            base.DoDelete();
            this.grid2.DeleteRow();
            _Chk = "DELETE";
        }

        #endregion
    }
}
