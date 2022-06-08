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
    /// PP6000 class
    /// </summary>
    public partial class PP6000 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        /// PP6000 Constructor
        /// </summary>
        public PP6000()
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
            if (e.Row.Cells["ITEM"].Value.ToString() == "합 계")
            {
                e.Row.Appearance.BackColor = Color.LightCyan;
                e.Row.Appearance.FontData.Bold = DefaultableBoolean.True;
            }
        }

        /// <summary>
        /// PP6000 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP6000_Disposed(object sender, EventArgs e)
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
                            maskedTextBox.Text =string.Empty;
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
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "RECDATE", "작업일", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "SEQ", "순번", false, GridColDataType_emu.VarChar, 50, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "CARNO", "차량번호", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "CUST", "거래처", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "ITEM", "품명", false, GridColDataType_emu.VarChar, 120, 10, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "TOTWEIGHT", "총중량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "TWTIME", "총중시각", false, GridColDataType_emu.VarChar, 70, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "NONWEIGHT", "공차중량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "NWTIME", "공차시각", false, GridColDataType_emu.VarChar, 70, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "NETWEIGHT", "실중량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "DISCRATE", "감율", false, GridColDataType_emu.Integer, 50, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "DISCWEIGHT", "감량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "INWEIGHT", "인수량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "UNITAMT", "단가", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "AMT", "금액", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "REMARK", "비고", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "DIV", "구분", false, GridColDataType_emu.VarChar, 50, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid1, "MAKEDATE", "등록일시", false, GridColDataType_emu.DateTime24, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid1);
            _ChangeDt1 = (DataTable)grid1.DataSource;
            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);

            // 2. Initialize grid2
            _UltraGridUtil.InitializeGrid(this.grid2, true, true, false, string.Empty, false);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "RECDATE", "작업일", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "SEQ", "순번", false, GridColDataType_emu.VarChar, 50, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "CARNO", "차량번호", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "CUST", "거래처", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "ITEM", "품명", false, GridColDataType_emu.VarChar, 120, 10, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "TOTWEIGHT", "총중량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "TWTIME", "총중시각", false, GridColDataType_emu.VarChar, 70, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "NONWEIGHT", "공차중량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "NWTIME", "공차시각", false, GridColDataType_emu.VarChar, 70, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "NETWEIGHT", "실중량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "DISCRATE", "감율", false, GridColDataType_emu.Integer, 50, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "DISCWEIGHT", "감량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "INWEIGHT", "인수량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "UNITAMT", "단가", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "AMT", "금액", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "REMARK", "비고", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "DIV", "구분", false, GridColDataType_emu.VarChar, 50, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(this.grid2, "MAKEDATE", "등록일시", false, GridColDataType_emu.DateTime24, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

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

            this.Disposed += new EventHandler(PP6000_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            grid1.InitializeRow -= new InitializeRowEventHandler(grid1_InitializeRow);

            this.Disposed -= new EventHandler(PP6000_Disposed);
        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            DataTable dt = null;
            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
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

                dt = sqlDBHelper.FillTable("USP_PP6000_S1", CommandType.StoredProcedure, sqlParameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1 && dt.Rows[0]["ITEM"].ToString() == "합 계")
                    {
                        return;
                    }

                    _RtnComDt1 = dt.AsEnumerable().Where(t => t.Field<string>("ITEM").Substring(t.Field<string>("ITEM").Length - 1, 1) == "계").CopyToDataTable();
                    _RtnComDt2 = dt.AsEnumerable().Where(t => t.Field<string>("ITEM").Substring(t.Field<string>("ITEM").Length - 1, 1) != "계").CopyToDataTable();

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
        /// Do base sum(TotWeight, NonWeight, NetWeight, DiscWeight, InWeight, Amt)
        /// </summary>
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "TotWeight", "NonWeight", "NetWeight", "DiscWeight", "InWeight", "Amt" });

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
                        case DataRowState.Modified:
                            sqlParameters = new SqlParameter[3];
                            sqlParameters[0] = sqlDBHelper.CreateParameter("@RecDate", dr["RecDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[1] = sqlDBHelper.CreateParameter("@Seq", dr["Seq"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[2] = sqlDBHelper.CreateParameter("@LotNo", dr["LotNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);

                            sqlDBHelper.ExecuteNoneQuery("USP_PP6000_U1", CommandType.StoredProcedure, sqlParameters);
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

            for (int i = 0; i < grid1.Selected.Rows.Count; i++)
            {
                if (grid1.Selected.Rows[i].Cells["UD"].Value.ToString() == "F")
                {
                    grid1.Selected.Rows[i].Selected = false;
                    bFlag = false;
                }
            }

            if (bFlag == false)
            {
                MessageBox.Show("삭제 할수 없는 행이 포함되었습니다.");
                return;
            }

            base.DoDelete();
            this.grid1.DeleteRow();
            _Chk = "DELETE";
        }

        #endregion
    }
}
