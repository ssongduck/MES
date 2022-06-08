using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using SAMMI.Common;

using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;

namespace SAMMI.PP
{
    /// <summary>
    /// PP0355 class
    /// </summary>
    public partial class PP0355 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common grid1 datatable
        /// </summary>
        DataTable _RtnComDt = new DataTable();

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
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        #endregion

        #region Constructor

        /// <summary>
        /// PP0355 constructor
        /// </summary>
        public PP0355()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();
            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// Grid1 double clicke cell event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void grid1_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        //{
        //    SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
        //    SqlParameter[] sqlParameters = new SqlParameter[2];

        //    try
        //    {
        //        if (_ChangeDt2 == null)
        //        {
        //            _ChangeDt2 = new DataTable();
        //        }

        //        _ChangeDt2.Clear();

        //        string sRecDate = this.grid1.ActiveRow.Cells["RECDATE"].Value.ToString();
        //        string sSeq = this.grid1.ActiveRow.Cells["SEQ"].Value.ToString();

        //        sqlParameters[0] = sqlDBHelper.CreateParameter("@RECDATE", sRecDate, SqlDbType.VarChar, ParameterDirection.Input);
        //        sqlParameters[1] = sqlDBHelper.CreateParameter("@SEQ", sSeq, SqlDbType.VarChar, ParameterDirection.Input);
        //        _RtnComDt = sqlDBHelper.FillTable("USP_PP0355_S2", CommandType.StoredProcedure, sqlParameters);

        //        grid2.DataSource = _RtnComDt;
        //        grid2.DataBind();

        //        _ChangeDt2 = _RtnComDt;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //    finally
        //    {
        //        if (sqlDBHelper._sConn != null)
        //        {
        //            sqlDBHelper._sConn.Close();
        //        }

        //        if (sqlParameters != null)
        //        {
        //            sqlParameters = null;
        //        }
        //    }
        //}

        /// <summary>
        /// Grid1 click cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_ClickCell(object sender, ClickCellEventArgs e)
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[2];

            try
            {
                if (_ChangeDt2 == null)
                {
                    _ChangeDt2 = new DataTable();
                }

                _ChangeDt2.Clear();

                string sRecDate = this.grid1.ActiveRow.Cells["RECDATE"].Value.ToString();
                string sSeq = this.grid1.ActiveRow.Cells["SEQ"].Value.ToString();

                sqlParameters[0] = sqlDBHelper.CreateParameter("@RECDATE", sRecDate, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@SEQ", sSeq, SqlDbType.VarChar, ParameterDirection.Input);
                _RtnComDt = sqlDBHelper.FillTable("USP_PP0355_S2", CommandType.StoredProcedure, sqlParameters);

                grid2.DataSource = _RtnComDt;
                grid2.DataBind();

                _ChangeDt2 = _RtnComDt;
            }
            catch (Exception ex)
            {
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
            }
        }

        /// <summary>
        /// PP0355 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP0355_Disposed(object sender, EventArgs e)
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

        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            string query = string.Empty;

            // 1. 운송정보 grid
            _UltraGridUtil.InitializeGrid(this.grid1, true, true, false, string.Empty, false);

            _UltraGridUtil.InitColumnUltraGrid(grid1, "RECDATE", "등록일자", false, GridColDataType_emu.DateTime, 150, 10, Infragistics.Win.HAlign.Center, true, false, "yyyy-MM-dd", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "BUYERCODE", "거래처", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "TAPPINGTEMP", "출탕온도", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "VEHICLENO", "차량번호", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "LADLECNT", "래들", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MAKEDATE", "생성일자", false, GridColDataType_emu.DateTime, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MAKER", "생성자", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "EDITDATE", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "EDITOR", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "KEYDATE", "KEYDATE", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "SEQ", "순번", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid1);
            _ChangeDt1 = (DataTable)grid1.DataSource;

            query = " SELECT MinorCode AS CODE_ID     "
                  + "      , CODENAME  AS CODE_NAME   "
                  + "   FROM TBM0000                  "
                  + "  WHERE UseFlag   =  'Y'         "
                  + "    AND MajorCode =  'BUYERCODE' "
                  + "    AND MinorCode <> '$'         ";

            DataTable dt = sqlDBHelper.FillTable(query, CommandType.Text);

            //DataTable dt = _Common.GET_TBM0000_CODE("BUYERCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "BUYERCODE", dt, "CODE_ID", "CODE_NAME");

            // 2. 래들정보 grid
            _UltraGridUtil.InitializeGrid(this.grid2, true, true, false, string.Empty, false);

            _UltraGridUtil.InitColumnUltraGrid(grid2, "LADLENO", "래들번호", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "INJECTIONWEIGHT", "주입중량", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "DEPARTURETEMP", "출발온도", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "ARRIVALTEMP", "도착온도", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "ARRIVALDATE", "도착일시", false, GridColDataType_emu.DateTime, 150, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "SEQID", "SEQID", false, GridColDataType_emu.Integer, 150, 10, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid2);

            _ChangeDt2 = (DataTable)grid2.DataSource;
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            grid1.ClickCell += new ClickCellEventHandler(grid1_ClickCell);
            //grid1.DoubleClickCell += new DoubleClickCellEventHandler(grid1_DoubleClickCell);

            this.Disposed += new EventHandler(PP0355_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            grid1.ClickCell -= new ClickCellEventHandler(grid1_ClickCell);
            //grid1.DoubleClickCell -= new DoubleClickCellEventHandler(grid1_DoubleClickCell);

            this.Disposed -= new EventHandler(PP0355_Disposed);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[2];

            try
            {
                if (_ChangeDt1 != null)
                {
                    _ChangeDt1.Clear();
                }

                if (_ChangeDt2 != null)
                {
                    _ChangeDt2.Clear();
                }

                string sDtpDate = string.Format("{0:yyyy-MM-dd}", calRegDT_FRH.Value);
                string sDtpDateTo = string.Format("{0:yyyy-MM-dd}", calRegDT_TOH.Value);

                sqlParameters[0] = sqlDBHelper.CreateParameter("@STARTDATE", sDtpDate, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@ENDDATE", sDtpDateTo, SqlDbType.VarChar, ParameterDirection.Input);

                _RtnComDt = sqlDBHelper.FillTable("USP_PP0355_S1", CommandType.StoredProcedure, sqlParameters);

                grid1.DataSource = _RtnComDt;
                grid1.DataBind();

                _ChangeDt1 = _RtnComDt;

                if (_RtnComDt.Rows.Count <= 0)
                {
                    _ChangeDt2.Clear();
                }
            }
            catch (Exception ex)
            {
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
            }
        }

        /// <summary>
        /// Do new
        /// </summary>
        public override void DoNew()
        {
            try
            {
                if (this.grid1.IsActivate)
                {
                    int iRow = _UltraGridUtil.AddRow(this.grid1, _ChangeDt1);

                    UltraGridUtil.ActivationAllowEdit(this.grid1, "RECDATE", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "BUYERCODE", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "TAPPINGTEMP", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "VEHICLENO", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "LADLECNT", iRow);
                }
                else
                {
                    if (this.grid1.Rows.Count > 0)
                    {
                        int iRow = _UltraGridUtil.AddRow(this.grid2, _ChangeDt2);

                        UltraGridUtil.ActivationAllowEdit(this.grid2, "LADLENO", iRow);
                        UltraGridUtil.ActivationAllowEdit(this.grid2, "INJECTIONWEIGHT", iRow);
                        UltraGridUtil.ActivationAllowEdit(this.grid2, "DEPARTURETEMP", iRow);
                        UltraGridUtil.ActivationAllowEdit(this.grid2, "ARRIVALTEMP", iRow);
                        UltraGridUtil.ActivationAllowEdit(this.grid2, "ARRIVALDATE", iRow);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
            //2017.5.19 by ymc : ERP 액상 물류이동처리 로직 추가되어 삭제기능 제거
            string sVendor = string.Empty;
            sVendor = this.grid1.ActiveRow.Cells["BUYERCODE"].Value.ToString();

            if (sVendor != "SK2")
            {
                base.DoDelete();

                if (this.grid1.IsActivate)
                {
                    this.grid1.DeleteRow();
                }
                else
                {
                    this.grid2.DeleteRow();
                }
            }
            else
            {
                MessageBox.Show(string.Format("{0}", "서산공장 액상운송 자료는 삭제할 수 없습니다."));
            }
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
                this.Focus();

                if (grid1.IsActivate)
                {
                    UltraGridUtil.DataRowDelete(this.grid1);
                    this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                    foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                    {
                        switch (dr.RowState)
                        {
                            case DataRowState.Added:

                            case DataRowState.Modified:
                                if (SqlDBHelper.nvlString(dr["RECDATE"]) == string.Empty)
                                {
                                    ShowDialog("C:I00025", Windows.Forms.DialogForm.DialogType.OK);

                                    CancelProcess = true;
                                    return;
                                }

                                break;
                        }
                    }

                    if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }

                    base.DoSave();

                    foreach (DataRow dr in _ChangeDt1.Rows)
                    {
                        switch (dr.RowState)
                        {
                            case DataRowState.Added:

                                sqlParameters = new SqlParameter[10];

                                sqlParameters[0] = sqlDBHelper.CreateParameter("PLANTCODE", "SK2", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[1] = sqlDBHelper.CreateParameter("RECDATE", Convert.ToDateTime(dr["RECDATE"].ToString()).ToString("yyyy-MM-dd"), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[2] = sqlDBHelper.CreateParameter("WORKCENTERCODE", "1351", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[3] = sqlDBHelper.CreateParameter("BUYERCODE", dr["BUYERCODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[4] = sqlDBHelper.CreateParameter("TAPPINGTEMP", string.IsNullOrEmpty(dr["TAPPINGTEMP"].ToString()) ? "0" : dr["TAPPINGTEMP"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                                sqlParameters[5] = sqlDBHelper.CreateParameter("VEHICLENO", dr["VEHICLENO"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[6] = sqlDBHelper.CreateParameter("LOTNO", dr["VEHICLENO"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[7] = sqlDBHelper.CreateParameter("LADLECNT", string.IsNullOrEmpty(dr["LADLECNT"].ToString()) ? "0" : dr["LADLECNT"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[8] = sqlDBHelper.CreateParameter("ARRIVEDLADLECNT", 0, SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[9] = sqlDBHelper.CreateParameter("MAKER", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                                sqlDBHelper.ExecuteNoneQuery("USP_PP0355_I1", CommandType.StoredProcedure, sqlParameters);

                                break;

                            case DataRowState.Modified:

                                sqlParameters = new SqlParameter[12];

                                sqlParameters[0] = sqlDBHelper.CreateParameter("PLANTCODE", "SK2", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[1] = sqlDBHelper.CreateParameter("RECDATE", Convert.ToDateTime(dr["RECDATE"].ToString()).ToString("yyyy-MM-dd"), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[2] = sqlDBHelper.CreateParameter("WORKCENTERCODE", "1351", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[3] = sqlDBHelper.CreateParameter("BUYERCODE", dr["BUYERCODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[4] = sqlDBHelper.CreateParameter("TAPPINGTEMP", dr["TAPPINGTEMP"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                                sqlParameters[5] = sqlDBHelper.CreateParameter("VEHICLENO", dr["VEHICLENO"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[6] = sqlDBHelper.CreateParameter("LOTNO", dr["VEHICLENO"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[7] = sqlDBHelper.CreateParameter("LADLECNT", dr["LADLECNT"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[8] = sqlDBHelper.CreateParameter("ARRIVEDLADLECNT", 0, SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[9] = sqlDBHelper.CreateParameter("MAKER", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[10] = sqlDBHelper.CreateParameter("SEQ", dr["SEQ"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[11] = sqlDBHelper.CreateParameter("KEYDATE", dr["KEYDATE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);

                                sqlDBHelper.ExecuteNoneQuery("USP_PP0355_U1", CommandType.StoredProcedure, sqlParameters);

                                break;

                            case DataRowState.Deleted:

                                dr.RejectChanges();

                                sqlParameters = new SqlParameter[4];

                                sqlParameters[0] = sqlDBHelper.CreateParameter("PLANTCODE", "SK2", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[1] = sqlDBHelper.CreateParameter("KEYDATE", dr["KEYDATE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[2] = sqlDBHelper.CreateParameter("WORKCENTERCODE", "1351", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[3] = sqlDBHelper.CreateParameter("SEQ", dr["SEQ"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);

                                sqlDBHelper.ExecuteNoneQuery("USP_PP0355_D1", CommandType.StoredProcedure, sqlParameters);

                                break;
                        }
                    }
                }
                else
                {
                    UltraGridUtil.DataRowDelete(this.grid2);
                    this.grid2.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                    foreach (DataRow dr in ((DataTable)grid2.DataSource).Rows)
                    {
                        switch (dr.RowState)
                        {
                            case DataRowState.Added:
                            case DataRowState.Modified:
                                if (SqlDBHelper.nvlString(dr["LADLENO"]) == string.Empty)
                                {
                                    ShowDialog("C:I00026", Windows.Forms.DialogForm.DialogType.OK);

                                    CancelProcess = true;
                                    return;
                                }
                                if (SqlDBHelper.nvlString(dr["INJECTIONWEIGHT"]) == string.Empty)
                                {
                                    ShowDialog("C:I00027", Windows.Forms.DialogForm.DialogType.OK);

                                    CancelProcess = true;
                                    return;
                                }

                                break;
                        }
                    }

                    if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }

                    base.DoSave();

                    foreach (DataRow dr in _ChangeDt2.Rows)
                    {
                        switch (dr.RowState)
                        {
                            case DataRowState.Added:

                                sqlParameters = new SqlParameter[10];

                                sqlParameters[0] = sqlDBHelper.CreateParameter("PLANTCODE", "SK2", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[1] = sqlDBHelper.CreateParameter("KEYDATE", this.grid1.ActiveRow.Cells["RECDATE"].Value.ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[2] = sqlDBHelper.CreateParameter("WORKCENTERCODE", "1351", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[3] = sqlDBHelper.CreateParameter("SEQ", this.grid1.ActiveRow.Cells["SEQ"].Value.ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[4] = sqlDBHelper.CreateParameter("LADLENO", dr["LADLENO"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[5] = sqlDBHelper.CreateParameter("INJECTIONWEIGHT", dr["INJECTIONWEIGHT"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                                sqlParameters[6] = sqlDBHelper.CreateParameter("DEPARTURETEMP", dr["DEPARTURETEMP"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                                sqlParameters[7] = sqlDBHelper.CreateParameter("ARRIVALTEMP", dr["ARRIVALTEMP"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                                sqlParameters[8] = sqlDBHelper.CreateParameter("ARRIVALD`ATE", string.IsNullOrEmpty(dr["ARRIVALDATE"].ToString()) ? null : dr["ARRIVALDATE"].ToString(), SqlDbType.DateTime, ParameterDirection.Input);
                                sqlParameters[9] = sqlDBHelper.CreateParameter("MAKER", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                                sqlDBHelper.ExecuteNoneQuery("USP_PP0355_I2", CommandType.StoredProcedure, sqlParameters);

                                break;

                            case DataRowState.Modified:

                                sqlParameters = new SqlParameter[11];

                                sqlParameters[0] = sqlDBHelper.CreateParameter("PLANTCODE", "SK2", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[1] = sqlDBHelper.CreateParameter("KEYDATE", this.grid1.ActiveRow.Cells["RECDATE"].Value.ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[2] = sqlDBHelper.CreateParameter("WORKCENTERCODE", "1351", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[3] = sqlDBHelper.CreateParameter("SEQ", this.grid1.ActiveRow.Cells["SEQ"].Value.ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[4] = sqlDBHelper.CreateParameter("LADLENO", dr["LADLENO"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[5] = sqlDBHelper.CreateParameter("INJECTIONWEIGHT", dr["INJECTIONWEIGHT"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                                sqlParameters[6] = sqlDBHelper.CreateParameter("DEPARTURETEMP", dr["DEPARTURETEMP"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                                sqlParameters[7] = sqlDBHelper.CreateParameter("ARRIVALTEMP", dr["ARRIVALTEMP"].ToString(), SqlDbType.Float, ParameterDirection.Input);
                                sqlParameters[8] = sqlDBHelper.CreateParameter("ARRIVALDATE", dr["ARRIVALDATE"].ToString(), SqlDbType.DateTime, ParameterDirection.Input);
                                sqlParameters[9] = sqlDBHelper.CreateParameter("MAKER", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[10] = sqlDBHelper.CreateParameter("SEQID", dr["SEQID"].ToString(), SqlDbType.Int, ParameterDirection.Input);

                                sqlDBHelper.ExecuteNoneQuery("USP_PP0355_U2", CommandType.StoredProcedure, sqlParameters);

                                break;

                            case DataRowState.Deleted:

                                dr.RejectChanges();

                                sqlParameters = new SqlParameter[5];

                                sqlParameters[0] = sqlDBHelper.CreateParameter("PLANTCODE", "SK2", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[1] = sqlDBHelper.CreateParameter("KEYDATE", this.grid1.ActiveRow.Cells["RECDATE"].Value.ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[2] = sqlDBHelper.CreateParameter("WORKCENTERCODE", "1351", SqlDbType.VarChar, ParameterDirection.Input);
                                sqlParameters[3] = sqlDBHelper.CreateParameter("SEQ", this.grid1.ActiveRow.Cells["SEQ"].Value.ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[4] = sqlDBHelper.CreateParameter("SEQID", dr["SEQID"].ToString(), SqlDbType.Int, ParameterDirection.Input);

                                sqlDBHelper.ExecuteNoneQuery("USP_PP0355_D2", CommandType.StoredProcedure, sqlParameters);

                                break;
                        }
                    }
                }

                sqlDBHelper.Transaction.Commit();
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
            }
        }

        #endregion
    }
}
