#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : PP0490
//   Form Name      : 주조생산현황
//   Name Space     : SAMMI.PP
//   Created Date   : 2022.06.08
//   Made By        : 정용석
//   Description    : 작업일보 활용#6 
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
    public partial class PP0490 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        public PP0490()
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
        private void PP0490_Disposed(object sender, EventArgs e)
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

            //deEdate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
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
            this.Disposed += new EventHandler(PP0490_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(PP0490_Disposed);
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
                base.DoInquire();

                string plantcode = SqlDBHelper.nvlString(cboPlantCode_H.Value);
                string sdate = string.Format("{0:yyyy-MM}-{1}", deEdate.Value, "01");
                string edate = string.Format("{0:yyyy-MM-dd}", deEdate.Value);

                param[0] = helper.CreateParameter("@as_plantcode",  plantcode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@as_sdate",          sdate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@as_edate",          edate, SqlDbType.VarChar, ParameterDirection.Input);

                _dt = helper.FillTable("USP_PP0490_S1", CommandType.StoredProcedure, param);                

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

        #endregion

        private void GridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "d01":
                case "d02":
                case "d03":
                case "d04":
                case "d05":
                case "d06":
                case "d07":
                case "d08":
                case "d09":
                case "d10":
                case "d11":
                case "d12":
                case "d13":
                case "d14":
                case "d15":
                case "d16":
                case "d17":
                case "d18":
                case "d19":
                case "d20":
                case "d21":
                case "d22":
                case "d23":
                case "d24":
                case "d25":
                case "d26":
                case "d27":
                case "d28":
                case "d29":
                case "d30":
                case "d31":
                    e.Appearance.ForeColor = Color.Blue;
                    break;
                case "n01":
                case "n02":
                case "n03":
                case "n04":
                case "n05":
                case "n06":
                case "n07":
                case "n08":
                case "n09":
                case "n10":
                case "n11":
                case "n12":
                case "n13":
                case "n14":
                case "n15":
                case "n16":
                case "n17":
                case "n18":
                case "n19":
                case "n20":
                case "n21":
                case "n22":
                case "n23":
                case "n24":
                case "n25":
                case "n26":
                case "n27":
                case "n28":
                case "n29":
                case "n30":
                case "n31":
                    e.Appearance.ForeColor = Color.Red;
                    break;
                case "t01":
                case "t02":
                case "t03":
                case "t04":
                case "t05":
                case "t06":
                case "t07":
                case "t08":
                case "t09":
                case "t10":
                case "t11":
                case "t12":
                case "t13":
                case "t14":
                case "t15":
                case "t16":
                case "t17":
                case "t18":
                case "t19":
                case "t20":
                case "t21":
                case "t22":
                case "t23":
                case "t24":
                case "t25":
                case "t26":
                case "t27":
                case "t28":
                case "t29":
                case "t30":
                case "t31":
                    e.Appearance.BackColor = Color.Azure;                
                    break;
            }

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dt != null || _dt.Rows.Count != 0)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();

                    if (dialog.ShowDialog() != DialogResult.OK) { return; }

                    string path = dialog.SelectedPath;

                    string fileName = @"\" + string.Format("{0:yyyy-MM}-{1}", deEdate.Value, "01") + '~' + string.Format("{0:yyyy-MM-dd}", deEdate.Value) +".xlsx";

                    gridControl1.ExportToXlsx(path + fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
