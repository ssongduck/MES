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

namespace SAMMI.QM
{
    /// <summary>
    /// QM2710 class
    /// </summary>
    public partial class QM2710 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        /// QM2710 constructor
        /// </summary>
        public QM2710()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();

            txtItemCode.Focus();
        }

        #endregion

        #region Event

        /// <summary>
        /// QM2710 Disposed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QM2710_Disposed(object sender, EventArgs e)
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
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemCodeName, "TBM0101", new object[] { this.cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemCodeName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
                , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            calFromDt.Value = DateTime.Now;
            calToDt.Value = DateTime.Now;
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            _UltraGridUtil.InitializeGrid(this.gridItemList, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 60, 60, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "WORKCENTERCODE", "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 200, 200, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "ITEMCODE", "품목코드", false, GridColDataType_emu.VarChar, 150, 150, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "ITEMNAME", "품목명", false, GridColDataType_emu.VarChar, 300, 300, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "SERIALNO", "SERIALNO", false, GridColDataType_emu.VarChar, 250, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "CREATE_DATE", "생성일자", false, GridColDataType_emu.DateTime, 200, 200, Infragistics.Win.HAlign.Center, true, false, null, "yyyy-MM-dd hh:mm:ss", null, null, null);
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.Disposed += new EventHandler(QM2710_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(QM2710_Disposed);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            DataTable dt = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[6];

            ClearAllControl();

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                base.DoInquire();

                sqlParameters[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[2] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[3] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[4] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[5] = sqlDBHelper.CreateParameter("@AS_SERIALNO", txtSerialNo.Text, SqlDbType.VarChar, ParameterDirection.Input);

                dt = sqlDBHelper.FillTable("SP_GET_SERIAL_DATA", CommandType.StoredProcedure, sqlParameters);

                gridItemList.DataSource = dt;
                gridItemList.DataBind();

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

        #endregion
    }
}
