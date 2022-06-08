using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SAMMI.PP
{
    /// <summary>
    /// PP0330 class
    /// </summary>
    public partial class PP0330 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variables

        /// <summary>
        /// </summary>
        /// Plant code
        private string _PlantCode = string.Empty;

        /// <summary>
        /// Return datatable
        /// </summary>
        DataTable _RtnDt1 = new DataTable();

        /// <summary>
        /// Return datatable
        /// </summary>
        DataTable _RtnDt2 = new DataTable();

        /// <summary>
        /// Biz text box manager ex
        /// </summary>
        BizTextBoxManagerEX _BizTextBoxManagerEX;

        /// <summary>
        /// Biz grid manager ex
        /// </summary>
        BizGridManagerEX _BizGridManagerEX;

        /// <summary>
        /// Grid object
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        /// <summary>
        /// Change datatable
        /// </summary>
        DataTable _ChangeDt1 = new DataTable();

        /// <summary>
        /// Change datatable
        /// </summary>
        DataTable _ChangeDt2 = new DataTable();

        #endregion

        #region Constructors

        /// <summary>
        /// PP0330 constructor
        /// </summary>
        public PP0330()
        {
            InitializeComponent();

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

            if (!(this._PlantCode.Equals("SK1") || this._PlantCode.Equals("SK2")))
            {
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;
            }

            if (LoginInfo.UserID.StartsWith("31") == false)
            {
                _BizTextBoxManagerEX = new BizTextBoxManagerEX();

                if (LoginInfo.PlantAuth.Equals(string.Empty))
                {
                    _BizTextBoxManagerEX.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, "", "" }
                             , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                    _BizTextBoxManagerEX.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                           , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
                }
                else
                {
                    _BizTextBoxManagerEX.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, "", "" }
                            , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                    _BizTextBoxManagerEX.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
                           , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// PP0330 load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP0330_Load(object sender, EventArgs e)
        {
            _UltraGridUtil.InitializeGrid(this.grid1, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this._PlantCode == "") ? true : false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "CARTYPE", "차종", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "PREVLOTNO", "투입LOT번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "LOTNO", "LOT번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "LOTQTY", "투입수량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "GOODQTY", "양품수량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "BADQTY", "불량수량", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MAKEDATE", "작업일시", false, GridColDataType_emu.DateTime24, 130, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "PITEMCODE", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "PITEMNAME", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "SEQ", "", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid1);

            _UltraGridUtil.InitializeGrid(this.grid2, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(grid2, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this._PlantCode == "") ? true : false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "CAST_WORKCENTERCODE", "주조작업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "CAST_WORKCENTERNAME", "주조작업장명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "CAST_ITEMCODE", "주조품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "CAST_ITEMNAME", "주조품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "CAST_MOLDCODE", "주조금형", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "CAST_PRODQTY", "양품수량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "CAST_MAKEDATE", "주조작업일시", false, GridColDataType_emu.DateTime24, 130, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "CAST_LOTNO", "투입LOT번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "LOTNO", "LOT번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "WORKCENTERCODE", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "ITEMCODE", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "ITEMNAME", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "CARTYPE", "차종", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "LOTQTY", "투입수량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "GOODQTY", "양품수량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "BADQTY", "불량수량", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "MAKEDATE", "작업일시", false, GridColDataType_emu.DateTime24, 130, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid2, "SEQ", "", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid2);

            _RtnDt1 = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", _RtnDt1, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid2, "PLANTCODE", _RtnDt2, "CODE_ID", "CODE_NAME");

            _ChangeDt1 = (DataTable)grid1.DataSource;
            _ChangeDt2 = (DataTable)grid2.DataSource;

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            grid2.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid2.DisplayLayout.Override.RowSelectorWidth = 40;
            grid2.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid2.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            grid1.DisplayLayout.UseFixedHeaders = true;
            grid2.DisplayLayout.UseFixedHeaders = true;
            
            if (LoginInfo.UserID.StartsWith("31"))
            {
                txtWorkCenterCode.Text = LoginInfo.UserID;
                txtWorkCenterCode.Enabled = false;
            }
        }

        /// <summary>
        /// Grid1 initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["ITEMNAME"].Value.ToString() == "품목계")
            {
                e.Row.Appearance.BackColor = Color.LightCyan;
            }
            else if (e.Row.Cells["ITEMNAME"].Value.ToString() == "작업장계")
            {
                e.Row.Appearance.BackColor = Color.LightBlue;
            }
        }

        /// <summary>
        /// Grid2 initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid2_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["CAST_ITEMNAME"].Value.ToString() == "품목계")
            {
                e.Row.Appearance.BackColor = Color.LightCyan;
            }
            else if (e.Row.Cells["CAST_ITEMNAME"].Value.ToString() == "작업장계")
            {
                e.Row.Appearance.BackColor = Color.LightBlue;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);            

            SqlParameter[] sqlParameters1 = new SqlParameter[7];
            SqlParameter[] sqlParameters2 = new SqlParameter[7];

            try
            {               
                //_ChangeDt1.Clear();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);
                string sWorkCenterCode = this.txtWorkCenterCode.Text.Trim();
                string sOPCode = this.txtOPCode.Text.Trim();
                string sLineCode = this.txtLineCode.Text.Trim();
                string sItemCode = this.txtItemCode.Text.Trim();
                string sDayNight = SqlDBHelper.nvlString(cboDaynight_H.Value, string.Empty);

                base.DoInquire();

                sqlParameters1[0] = sqlDBHelper.CreateParameter("@PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[1] = sqlDBHelper.CreateParameter("@STARTDATE", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[2] = sqlDBHelper.CreateParameter("@ENDDATE", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[3] = sqlDBHelper.CreateParameter("@WORKCENTERCODE", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[4] = sqlDBHelper.CreateParameter("@ITEMCODE", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[5] = sqlDBHelper.CreateParameter("@INLOTNO", txtInLot.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[6] = sqlDBHelper.CreateParameter("@LOTNO", txtLot.Text, SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters2[0] = sqlDBHelper.CreateParameter("@PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[1] = sqlDBHelper.CreateParameter("@STARTDATE", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[2] = sqlDBHelper.CreateParameter("@ENDDATE", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[3] = sqlDBHelper.CreateParameter("@WORKCENTERCODE", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[4] = sqlDBHelper.CreateParameter("@ITEMCODE", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[5] = sqlDBHelper.CreateParameter("@INLOTNO", txtInLot.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[6] = sqlDBHelper.CreateParameter("@LOTNO", txtLot.Text, SqlDbType.VarChar, ParameterDirection.Input);

                _RtnDt1 = sqlDBHelper.FillTable("USP_PP0330_S2N_UNION", CommandType.StoredProcedure, sqlParameters1);
                _RtnDt2 = sqlDBHelper.FillTable("USP_PP0330_S1N_UNION", CommandType.StoredProcedure, sqlParameters2);

                grid1.DataSource = _RtnDt1;
                grid1.DataBind();

                _ChangeDt1 = _RtnDt2;

                grid2.DataSource = _RtnDt2;
                grid2.DataBind();

                _ChangeDt2 = _RtnDt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                if (sqlParameters1 != null) { sqlParameters1 = null; }
            }
        }

        #endregion
    }
}
