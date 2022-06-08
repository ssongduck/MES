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
    public partial class PP0343 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _RtnDt = new DataTable();

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

        /// <summary>
        /// save flag
        /// </summary>
        private bool _SaveFlag = false;

        BizTextBoxManagerEX btbManager;

        #endregion

        #region Constructor

        /// <summary>
        /// QM0405 constructor
        /// </summary>
        public PP0343()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();
            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// Disposed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PP0343_Disposed(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// btn new
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            DoInquire();
            base.ClosePrgForm();
        }


        /// <summary>
        /// btn save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save_Prod();
        }

        /// <summary>
        /// gridView workreport rowclick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewWorkReportlist_RowClick(object sender, RowClickEventArgs e)
        {
            ClearUserControl();

            DataRowView dataRowView = gridViewProdlist.GetRow(e.RowHandle) as DataRowView;

            if (dataRowView != null)
            {
                if (dataRowView.Row != null)
                {
                    lblRecDate.Text = dataRowView.Row["RECDATE"].ToString();
                    lblWorkCenterCode.Text = dataRowView.Row["WORKCENTERCODE"].ToString();
                    lblWorkCenterName.Text = dataRowView.Row["WORKCENTERNAME"].ToString();
                    lblOrderNo.Text = dataRowView.Row["ORDERNO"].ToString();
                    lblItemName.Text = dataRowView.Row["ITEMNAME"].ToString();
                    lblItemCode.Text = dataRowView.Row["ITEMCODE"].ToString();
                    lblOrderqty.Text = string.IsNullOrEmpty(dataRowView.Row["PLANQTY"].ToString()) ? "0" : string.Format("{0:#,##0}", Convert.ToDecimal(dataRowView.Row["PLANQTY"].ToString()));
                    txtProdqty.Text = string.IsNullOrEmpty(dataRowView.Row["PRODQTY"].ToString()) ? "0" : string.Format("{0:#,##0}", Convert.ToDecimal(dataRowView.Row["PRODQTY"].ToString()));
                    txtSilaGateQty.Text = string.IsNullOrEmpty(dataRowView.Row["GATEQTY"].ToString()) ? "0" : string.Format("{0:#,##0}", Convert.ToDecimal(dataRowView.Row["GATEQTY"].ToString()));
                    txtSilaIngotQty.Text = string.IsNullOrEmpty(dataRowView.Row["INGOTQTY"].ToString()) ? "0" : string.Format("{0:#,##0}", Convert.ToDecimal(dataRowView.Row["INGOTQTY"].ToString()));
                    lblStatus.Text = "등록";
                }
            }

            _SaveFlag = false;
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

            ClearUserControl();

            fromDt.Value = DateTime.Now;

            _SaveFlag = false;
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
            this.Disposed += new EventHandler(PP0343_Disposed);

            btnNew.Click += new EventHandler(btnNew_Click);
            btnSave.Click += new EventHandler(btnSave_Click);

            gridViewProdlist.RowClick += new RowClickEventHandler(gridViewWorkReportlist_RowClick);
            gridViewProdlist.PopupMenuShowing += new PopupMenuShowingEventHandler(gridViewWorkReportlist_PopupMenuShowing);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(PP0343_Disposed);

            btnNew.Click -= new EventHandler(btnNew_Click);
            btnSave.Click -= new EventHandler(btnSave_Click);
            
            gridViewProdlist.RowClick -= new RowClickEventHandler(gridViewWorkReportlist_RowClick);
            gridViewProdlist.PopupMenuShowing -= new PopupMenuShowingEventHandler(gridViewWorkReportlist_PopupMenuShowing);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            ClearUserControl();

            DataTable dt = new DataTable();

            dt = GetSilafontOrder();

            if (dt != null && dt.Rows.Count > 0)
            {
                lblRecDate.Text = dt.Rows[0]["RECDATE"].ToString();
                lblWorkCenterCode.Text = dt.Rows[0]["WORKCENTERCODE"].ToString();
                lblWorkCenterName.Text = dt.Rows[0]["WORKCENTERNAME"].ToString();
                lblOrderNo.Text = dt.Rows[0]["ORDERNO"].ToString();
                lblItemName.Text = dt.Rows[0]["ITEMNAME"].ToString();
                lblItemCode.Text = dt.Rows[0]["ITEMCODE"].ToString();
                lblStatus.Text = dt.Rows[0]["ORDER_STATUS"].ToString();
                lblOrderqty.Text = string.IsNullOrEmpty(dt.Rows[0]["PLANQTY"].ToString()) ? "0" : string.Format("{0:#,##0}", Convert.ToDecimal(dt.Rows[0]["PLANQTY"].ToString()));
                txtProdqty.Text = string.IsNullOrEmpty(dt.Rows[0]["PRODQTY"].ToString()) ? "0" : string.Format("{0:#,##0}", Convert.ToDecimal(dt.Rows[0]["PRODQTY"].ToString()));
                txtSilaGateQty.Text = string.IsNullOrEmpty(dt.Rows[0]["GATEQTY"].ToString()) ? "0" : string.Format("{0:#,##0}", Convert.ToDecimal(dt.Rows[0]["GATEQTY"].ToString()));
                txtSilaIngotQty.Text = string.IsNullOrEmpty(dt.Rows[0]["INGOTQTY"].ToString()) ? "0" : string.Format("{0:#,##0}", Convert.ToDecimal(dt.Rows[0]["INGOTQTY"].ToString()));
            }

            if (txtProdqty.Text == "0" )
            {
                _SaveFlag = true;
            }
            else
            {
                _SaveFlag = false;
            }

            GetSilafontList();
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
            Save_Prod();
        }

        /// <summary>
        /// Clear user control
        /// </summary>
        private void ClearUserControl()
        {
            lblRecDate.Text = string.Empty;
            lblWorkCenterCode.Text = string.Empty;
            lblWorkCenterName.Text = string.Empty;
            lblOrderNo.Text = string.Empty;
            lblItemName.Text = string.Empty;
            lblItemCode.Text = string.Empty;
            lblOrderqty.Text = string.Empty;
            txtProdqty.Text = string.Empty;
            lblStatus.Text = string.Empty;
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
            ExportExcel(gridViewProdlist);
        }

        /// <summary>
        /// Save Product
        /// </summary>
        private void Save_Prod()
        {
            string flag = string.Empty;
            bool isNum = true;
            bool isNumSila = true;
            bool isNumALDC = true;
            int chkNum = 0;

            if (string.IsNullOrEmpty(lblOrderNo.Text))
            {
                this.ShowDialog("작업지시서가 존재하지 않습니다.\r\n생산관리 담당자에게 문의하세요.", Windows.Forms.DialogForm.DialogType.OK);
                ClearUserControl();
                DoInquire();
                base.ClosePrgForm();
                return;
            }

            if (!_SaveFlag)
            {
                this.ShowDialog("이미 등록된 실적입니다. 실적 변경시 관리자에게 문의하세요.", Windows.Forms.DialogForm.DialogType.OK);
                ClearUserControl();
                DoInquire();
                base.ClosePrgForm();
                return;
            }

            if (lblStatus.Text != "발주")
            {
                this.ShowDialog("작업지시서가 발주 상태가 아닙니다. 생산관리팀에 문의하세요.", Windows.Forms.DialogForm.DialogType.OK);
                ClearUserControl();
                DoInquire();
                base.ClosePrgForm();
                return;
            }

            isNum = int.TryParse(txtProdqty.Text, out chkNum);            
            isNumSila = int.TryParse(txtSilaGateQty.Text, out chkNum);
            isNumALDC = int.TryParse(txtSilaIngotQty.Text, out chkNum);

            if (!isNum || !isNumSila || !isNumALDC)
            {
                this.ShowDialog("숫자만 입력가능 합니다.", Windows.Forms.DialogForm.DialogType.OK);
                ClearUserControl();
                DoInquire();
                base.ClosePrgForm();
                return;
            }
            else
            {
                if (int.Parse(txtProdqty.Text) < 1)
                {
                    this.ShowDialog("생산량이 없습니다. 생산량을 입력해 주세요.", Windows.Forms.DialogForm.DialogType.OK);
                    ClearUserControl();
                    DoInquire();
                    base.ClosePrgForm();
                    return;
                }
            }
            
            if (!string.IsNullOrEmpty(lblOrderNo.Text) || isNum == true)
            {
                if (this.ShowDialog(" 실적을 등록 하시겠습니까?") == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable dt = new DataTable();

                    SqlDBHelper sqlDBHelper = new SqlDBHelper(false, false);
                    SqlParameter[] sqlParameters = new SqlParameter[9];

                    try
                    {
                        string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                        sqlParameters[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", lblWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters[2] = sqlDBHelper.CreateParameter("@AS_RECDATE", lblRecDate.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters[3] = sqlDBHelper.CreateParameter("@AS_ORDERNO", lblOrderNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters[4] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", lblItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                        sqlParameters[5] = sqlDBHelper.CreateParameter("@AS_PRODQTY", txtProdqty.Text, SqlDbType.Decimal, ParameterDirection.Input);

                        /* 중량입력 파라미터 추가 */
                        sqlParameters[6] = sqlDBHelper.CreateParameter("@AS_GATEQTY", txtSilaGateQty.Text, SqlDbType.Decimal, ParameterDirection.Input);
                        sqlParameters[7] = sqlDBHelper.CreateParameter("@AS_INGOTQTY", txtSilaIngotQty.Text, SqlDbType.Decimal, ParameterDirection.Input);

                        sqlParameters[8] = sqlDBHelper.CreateParameter("@AS_CREATER", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                        sqlDBHelper.ExecuteNoneQuery("SP_SAVE_SILAFONT", CommandType.StoredProcedure, sqlParameters);
                    }
                    catch
                    {
                        Console.WriteLine("저장오류");
                    }
                    finally
                    {
                        if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                        if (sqlParameters != null) { sqlParameters = null; }
                    }
                }
                ClearUserControl();
                DoInquire();
                base.ClosePrgForm();
            }
            else
            {
                this.ShowDialog("작업지시서가 없습니다. 작업지시서를 확인하세요.", Windows.Forms.DialogForm.DialogType.OK);
                ClearUserControl();
                DoInquire();
                base.ClosePrgForm();
            }
        }

        /// <summary>
        /// Get silafont list
        /// </summary>
        private void GetSilafontList()
        {
            DataTable dt = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
            SqlParameter[] sqlParameters = new SqlParameter[3];

            try
            {
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                sqlParameters[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", lblWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[2] = sqlDBHelper.CreateParameter("@AS_RECDATE", fromDt.Text, SqlDbType.VarChar, ParameterDirection.Input);

                dt = sqlDBHelper.FillTable("SP_GET_SILAFONT_LIST", CommandType.StoredProcedure, sqlParameters);

                gridProdlist.DataSource = dt;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                if (sqlParameters != null) { sqlParameters = null; }
            }
        }

        /// <summary>
        /// Get silafont order
        /// </summary>
        private DataTable GetSilafontOrder()
        {
            DataTable dt = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
            SqlParameter[] sqlParameters = new SqlParameter[3];

            ClearUserControl();

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                sqlParameters[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", "", SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[2] = sqlDBHelper.CreateParameter("@AS_RECDATE", fromDt.Text, SqlDbType.VarChar, ParameterDirection.Input);

                dt = sqlDBHelper.FillTable("SP_GET_SILAFONT_ORDER", CommandType.StoredProcedure, sqlParameters);

                return dt;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());

                return null;
            }
            finally
            {
                if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                if (sqlParameters != null) { sqlParameters = null; }
            }
        }

        #endregion
    }
}
