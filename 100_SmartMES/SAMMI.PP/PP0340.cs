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
    /// PP0340 class
    /// </summary>
    public partial class PP0340 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _RtnComDt = new DataTable();

        /// <summary>
        /// Change grid1 datatable
        /// </summary>
        private DataTable DtChange = null;

        /// <summary>
        /// Check
        /// </summary>
        string _Check = string.Empty;

        /// <summary>
        /// Header columns
        /// </summary>
        string[] _HeadColumnArray = null;

        /// <summary>
        /// 
        /// </summary>
        string[] _EmptyArray = { "v_txt" };

        /// <summary>
        /// Plant code
        /// </summary>
        private string _sPlantCode = string.Empty;

        /// <summary>
        /// Current row
        /// </summary>
        int _iCurrentRow = -1;

        /// <summary>
        /// Grid util
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        //비지니스 로직 객체 생성
        BizGridManagerEX gridManager; 
        PopUp_Biz _biz = new PopUp_Biz();

        #endregion

        #region Constructor

        /// <summary>
        /// PP0340 constructor
        /// </summary>
        public PP0340()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// Grid1 initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["iDate"].Value.ToString() == "합계")
            {
                e.Row.Appearance.BackColor = Color.LightCyan;
            }
        }

        /// <summary>
        /// Grid1 before row activate event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_BeforeRowActivate(object sender, RowEventArgs e)
        {
            if (grid1.Rows[e.Row.Index].Cells["UD"].Value.ToString() == "F")
            {
                grid1.Rows[e.Row.Index].Activation = Activation.ActivateOnly;
            }
        }

        /// <summary>
        /// Grid1 before cell activate event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_BeforeCellActivate(object sender, CancelableCellEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Cell.Row.Cells["UD"].Value.ToString()) && (e.Cell.Column.Key == "iDate" || e.Cell.Column.Key == "LotNo"))
            {
                e.Cell.Activation = Activation.ActivateOnly;
            }
        }

        /// <summary>
        /// Grid1 after cell update event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_AfterCellUpdate(object sender, CellEventArgs e)
        {
            //if (e.Cell.Column.Key == "WorkCenterCode" || e.Cell.Column.Key == "iDate")
            //{
            //    string sDate = e.Cell.Row.Cells["iDate"].Text.ToString();
            //    string sWorkCenterCode = e.Cell.Row.Cells["WorkCenterCode"].Value.ToString();
            //    string sOrderNo = e.Cell.Row.Cells["OrderNo"].Text.ToString();

            //    if (string.IsNullOrEmpty(sOrderNo) || ("20" + sOrderNo).StartsWith(sDate.Replace("-", "") + sWorkCenterCode) == false)
            //    {
            //        SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            //        DataTable dt = sqlDBHelper.FillTable("select top 1 PlanNo,dbo.FN_ItemName(ItemCode,'SK1') as  ItemName  from tap0100 where workcentercode='" +
            //            sWorkCenterCode + "' and recdate='" + sDate + "'", CommandType.Text);

            //        if (dt.Rows.Count == 1)
            //        {
            //            e.Cell.Row.Cells["OrderNo"].Value = dt.Rows[0][0].ToString();
            //            e.Cell.Row.Cells["ItemName"].Value = dt.Rows[0][1].ToString();
            //        }
            //    }
            //}
        }

        /// <summary>
        /// PP0340 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP0340_Disposed(object sender, EventArgs e)
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

            gridManager = new BizGridManagerEX(grid1);
            gridManager.PopUpAdd("OrderNo", "ItemName", "ORDERNO_HG", new string[] { _sPlantCode, "iDate", "WorkCenterCode" });
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[0];

            int iColSeq = 5;
            int iMergeColSeq = 0;

            _UltraGridUtil.InitializeGrid(this.grid1, true, true, false, string.Empty, false);

            DataTable dt = sqlDBHelper.FillTable("USP_PP0340_S2", CommandType.StoredProcedure, sqlParameters);

            string[] sMergeColumn = { "iDate", "LotNo", "WorkCenterCode", "OrderNo", "ItemName" };
            dt.DefaultView.RowFilter = "Div='Y'";
            string[] sMergeColumn1 = new string[dt.DefaultView.Count + 5];
            dt.DefaultView.RowFilter = "Div='H'";
            string[] sMergeColumn2 = new string[dt.DefaultView.Count + 6];

            _HeadColumnArray = new string[dt.Rows.Count + 11];
            _HeadColumnArray[0] = "iDate";
            _HeadColumnArray[1] = "LotNo";
            _HeadColumnArray[2] = "WorkCenterCode";
            _HeadColumnArray[3] = "OrderNo";
            _HeadColumnArray[4] = "ItemName";
            _HeadColumnArray[5] = "YStQty";

            _UltraGridUtil.InitColumnUltraGrid(grid1, "PlantCode", "PlantCode", false, GridColDataType_emu.VarChar, 60, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "iDate", "작업일자", false, GridColDataType_emu.YearMonthDay, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "LotNo", "LotNo", false, GridColDataType_emu.VarChar, 60, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 120, 4, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "OrderNo", "작업지시번호", false, GridColDataType_emu.VarChar, 120, 13, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 120, 13, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "YStQty", "기초중량[Y]", false, GridColDataType_emu.Integer, 80, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            
            sMergeColumn1[iMergeColSeq] = "YStQty";

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Div"].ToString() == "Y")
                {
                    _UltraGridUtil.InitColumnUltraGrid(grid1, dr["ItemCode"].ToString(), dr["ItemName"].ToString() + "[Y]", false, GridColDataType_emu.Integer, 80, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
                    _HeadColumnArray[++iColSeq] = dr["ItemCode"].ToString();
                    sMergeColumn1[++iMergeColSeq] = dr["ItemCode"].ToString();
                }
            }

            _UltraGridUtil.InitColumnUltraGrid(grid1, "YProdQty", "생산량[Y]", false, GridColDataType_emu.Integer, 80, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "YAmt", "용탕가[Y]", false, GridColDataType_emu.Integer, 80, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _HeadColumnArray[++iColSeq] = "YProdQty";
            _HeadColumnArray[++iColSeq] = "YAmt";
            sMergeColumn1[++iMergeColSeq] = "YProdQty";
            sMergeColumn1[++iMergeColSeq] = "YAmt";
            _HeadColumnArray[++iColSeq] = "HStQty";
            iMergeColSeq = 0;
            sMergeColumn2[iMergeColSeq] = "HStQty";
            _UltraGridUtil.InitColumnUltraGrid(grid1, "HStQty", "수탕[H]", false, GridColDataType_emu.Integer, 80, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Div"].ToString() == "H")
                {
                    _UltraGridUtil.InitColumnUltraGrid(grid1, dr["ItemCode"].ToString(), dr["ItemName"].ToString() + "[H]", false, GridColDataType_emu.Integer, 80, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
                    _HeadColumnArray[++iColSeq] = dr["ItemCode"].ToString();
                    sMergeColumn2[++iMergeColSeq] = dr["ItemCode"].ToString();
                }
            }

            _UltraGridUtil.InitColumnUltraGrid(grid1, "HProdQty", "생산량[H]", false, GridColDataType_emu.Integer, 80, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "HAmt", "용탕가[H]", false, GridColDataType_emu.Integer, 80, 10, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _HeadColumnArray[++iColSeq] = "HProdQty";
            _HeadColumnArray[++iColSeq] = "HAmt";
            sMergeColumn2[++iMergeColSeq] = "HProdQty";
            sMergeColumn2[++iMergeColSeq] = "HAmt";
            _UltraGridUtil.InitColumnUltraGrid(grid1, "Maker", "", false, GridColDataType_emu.VarChar, 1, 50, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "Editor", "", false, GridColDataType_emu.VarChar, 1, 50, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "UD", "", false, GridColDataType_emu.VarChar, 1, 50, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP_IF_DT", "ERP I/F", false, GridColDataType_emu.VarChar, 100, 50, Infragistics.Win.HAlign.Right, true, false, "yyyy-MM-dd HH:mm:ss", null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid1);

            DataTable workCenterDt = _Common.GET_TBM0000_CODE("ALLOY_WORKCENTER");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "WorkCenterCode", workCenterDt, "CODE_ID", "CODE_NAME");

            grid1.DisplayLayout.UseFixedHeaders = true;

            for (int i = 0; i < 2; i++)
            {
                grid1.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;
            }

            DtChange = (DataTable)grid1.DataSource;

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            grid1.InitializeRow += new InitializeRowEventHandler(grid1_InitializeRow);
            grid1.BeforeRowActivate += new RowEventHandler(grid1_BeforeRowActivate);
            grid1.BeforeCellActivate += new CancelableCellEventHandler(grid1_BeforeCellActivate);
            grid1.AfterCellUpdate += new CellEventHandler(grid1_AfterCellUpdate);

            this.Disposed += new EventHandler(PP0340_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            grid1.InitializeRow -= new InitializeRowEventHandler(grid1_InitializeRow);
            grid1.BeforeRowActivate -= new RowEventHandler(grid1_BeforeRowActivate);
            grid1.BeforeCellActivate -= new CancelableCellEventHandler(grid1_BeforeCellActivate);
            grid1.AfterCellUpdate -= new CellEventHandler(grid1_AfterCellUpdate);

            this.Disposed -= new EventHandler(PP0340_Disposed);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[2];
            ClearAllControl();

            try
            {
                DtChange.Clear();

                base.DoInquire();

                string sDtpDate = string.Format("{0:yyyy-MM-dd}", calRegDT_FRH.Value);
                string sDtpDateTo = string.Format("{0:yyyy-MM-dd}", calRegDT_TOH.Value);

                sqlParameters[0] = sqlDBHelper.CreateParameter("@StartDate", sDtpDate, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@EndDate", sDtpDateTo, SqlDbType.VarChar, ParameterDirection.Input);

                _RtnComDt = sqlDBHelper.FillTable("USP_PP0340_S1", CommandType.StoredProcedure, sqlParameters);

                grid1.DataSource = _RtnComDt;
                grid1.DataBind();

                DtChange = _RtnComDt;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                _Check = string.Empty;
            }
        }

        /// <summary>
        /// Do new
        /// </summary>
        public override void DoNew()
        {
            base.DoNew();

            _iCurrentRow = _UltraGridUtil.AddRow(this.grid1, DtChange);
            grid1.Rows[_iCurrentRow].Cells["iDate"].Value = DateTime.Now.ToString("yyyy-MM-dd");
            grid1.Rows[_iCurrentRow].Cells["Maker"].Value = this.WorkerID;
            grid1.Rows[_iCurrentRow].Cells["WorkCenterCode"].Value = string.Empty;
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
            bool bOK = true;

            for (int i = 0; i < grid1.Selected.Rows.Count; i++)
            {
                if (grid1.Selected.Rows[i].Cells["UD"].Value.ToString() == "F")
                {
                    grid1.Selected.Rows[i].Selected = false;
                    bOK = false;
                }
            }

            if (bOK == false)
            {
                MessageBox.Show("삭제 할수 없는 행이 포함되었습니다.");
                return;
            }

            base.DoDelete();
            this.grid1.DeleteRow();
            _Check = "DELETE";
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

                foreach (DataRow drRow in DtChange.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            drRow.RejectChanges();
                            sqlParameters = new SqlParameter[2];

                            sqlParameters[0] = sqlDBHelper.CreateParameter("@iDate", drRow["iDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[1] = sqlDBHelper.CreateParameter("@LotNo", drRow["LotNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlDBHelper.ExecuteNoneQuery("USP_PP0340_D1", CommandType.StoredProcedure, sqlParameters);

                            break;

                        case DataRowState.Added:

                            if (drRow["iDate"].ToString() == "")
                            {
                                MessageBox.Show("작업일자는 필수정보입니다.");
                                continue;
                            }

                            if (drRow["LotNo"].ToString().Trim() == "")
                            {
                                MessageBox.Show("LotNo는 필수정보입니다.");

                                continue;
                            }

                            sqlParameters = new SqlParameter[4];

                            sqlParameters[0] = sqlDBHelper.CreateParameter("@iDate", drRow["iDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[1] = sqlDBHelper.CreateParameter("@LotNo", drRow["LotNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[2] = sqlDBHelper.CreateParameter("@Maker", drRow["Maker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[3] = sqlDBHelper.CreateParameter("@OrderNo", drRow["OrderNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);

                            try
                            {
                                sqlDBHelper.ExecuteNoneQuery("USP_PP0340_I1", CommandType.StoredProcedure, sqlParameters);

                                for (int i = 2; i < _HeadColumnArray.Length; i++)
                                {
                                    if (drRow[_HeadColumnArray[i]].ToString() != "")
                                    {
                                        sqlParameters = new SqlParameter[4];
                                        sqlParameters[0] = sqlDBHelper.CreateParameter("@iDate", drRow["iDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                        sqlParameters[1] = sqlDBHelper.CreateParameter("@LotNo", drRow["LotNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                        sqlParameters[2] = sqlDBHelper.CreateParameter("@Col", _HeadColumnArray[i], SqlDbType.VarChar, ParameterDirection.Input);
                                        sqlParameters[3] = sqlDBHelper.CreateParameter("@Qty", drRow[_HeadColumnArray[i]].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                        sqlDBHelper.ExecuteNoneQuery("USP_PP0340_U1", CommandType.StoredProcedure, sqlParameters);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                            break;

                        case DataRowState.Modified:
                            sqlParameters = new SqlParameter[5];

                            for (int i = 2; i < _HeadColumnArray.Length; i++)
                            {
                                if (drRow[_HeadColumnArray[i]].ToString() != "")
                                {
                                    sqlParameters = new SqlParameter[5];
                                    sqlParameters[0] = sqlDBHelper.CreateParameter("@iDate", drRow["iDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                    sqlParameters[1] = sqlDBHelper.CreateParameter("@LotNo", drRow["LotNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                    sqlParameters[2] = sqlDBHelper.CreateParameter("@Col", _HeadColumnArray[i], SqlDbType.VarChar, ParameterDirection.Input);
                                    sqlParameters[3] = sqlDBHelper.CreateParameter("@Qty", drRow[_HeadColumnArray[i]].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                    sqlParameters[4] = sqlDBHelper.CreateParameter("@Editor", this.WorkerID, SqlDbType.VarChar, ParameterDirection.Input);
                                    sqlDBHelper.ExecuteNoneQuery("USP_PP0340_U1", CommandType.StoredProcedure, sqlParameters);
                                }
                            }

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

                _Check = string.Empty;
            }
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

        #endregion
    }
}
