#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : PP0460
//   Form Name      : 작업조건
//   Name Space     : SAMMI.PP
//   Created Date   : 2022.02.14
//   Made By        : 정용석
//   Description    : 작업일보 활용#2 (작업장-품목별 주조조건)
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
using System.IO;
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
using DevExpress.Export;
using DevExpress.XtraPrinting;


namespace SAMMI.PP
{
    public partial class PP0460 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        public PP0460()
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
        private void PP0460_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();
        }

        /// <summary>
        /// Excel Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            ExportToExcel();
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
            btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "2000", "", "" }, new string[] { }, new object[] { });
            btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }, new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });

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
            this.Disposed += new EventHandler(PP0460_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(PP0460_Disposed);
        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];
            ClearAllControl();

            try
            {
                base.DoInquire();

                string sdate = string.Format("{0:yyyy-MM-dd}", deSdate.Value);
                string edate = string.Format("{0:yyyy-MM-dd}", deEdate.Value);
                string item  = txtItemCode.Text;
                string wcode = txtWorkCenterCode.Text;

                param[0] = helper.CreateParameter("@AS_PLANTCODE",      "SK2", SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@AS_WCODE",          wcode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@AS_ITEMCODE",        item, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@AS_SDATE",          sdate, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@AS_EDATE",          edate, SqlDbType.VarChar, ParameterDirection.Input);

                _dt = helper.FillTable("USP_PP0460_S1", CommandType.StoredProcedure, param);                

                gridControl1.DataSource = _dt;                

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

        private void GridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "runRate")
            {
                int irunRate = e.CellValue == DBNull.Value ? 0 : Convert.ToInt32(e.CellValue);

                if (irunRate > 79)
                {
                    e.Appearance.BackColor = Color.PowderBlue;
                }
                else if (irunRate > 49 && irunRate < 80)
                {
                    e.Appearance.BackColor = Color.FromArgb(192, 255, 192);
                }
                else
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 192, 192);
                }
            }
        }

        /// <summary>
        /// Export To Excel
        /// </summary>
        private void ExportToExcel()
        {
            XlsxExportOptionsEx xlsxOptions = new XlsxExportOptionsEx();
            xlsxOptions.ExportType = DevExpress.Export.ExportType.WYSIWYG;
            string path = "주조조건_" + DateTime.Now.ToString("YYYY-MM-DD") + ".xlsx";
            gridControl1.ExportToXlsx(path);
            Process.Start(path);
        }

        #endregion

    }
}
