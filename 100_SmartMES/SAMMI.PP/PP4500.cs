using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Infragistics.UltraChart.Resources.Appearance;

using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using SAMMI.Windows.Forms;

namespace SAMMI.PP
{
    /// <summary>
    /// PP4500 class
    /// </summary>
    public partial class PP4500 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common grid1 datatable
        /// </summary>
        DataTable _RtnComDt = new DataTable();

        /// <summary>
        /// Grid object
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        /// <summary>
        /// Button manager
        /// </summary>
        BizTextBoxManagerEX _BtbManager;

        /// <summary>
        /// Plant code
        /// </summary>
        private string _PlantCode = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// PP4500 constructor
        /// </summary>
        public PP4500()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();
            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// Ultrachart1 data item over
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraChart1_DataItemOver(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            if (ultraChart1.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                ultraChart1.Tooltips.FormatString = e.RowLabel + " : <DATA_VALUE:#,#>";
            }
        }

        /// <summary>
        /// PP4500 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP4500_Disposed(object sender, EventArgs e)
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

            if (this._PlantCode.Equals("SK1"))
            {
                this._PlantCode = "SK1";
            }
            else if (this._PlantCode.Equals("SK2"))
            {
                this._PlantCode = "SK2";
            }
            else
            {
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;
            }

            _BtbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                _BtbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { this.cboPlantCode_H, txtOPCode, "" }
                      , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                _BtbManager.PopUpAdd(txtItemCode1, txtItemName1, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtItemCode2, txtItemName2, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtItemCode3, txtItemName3, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtItemCode4, txtItemName4, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtItemCode5, txtItemName5, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtItemCode6, txtItemName6, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtItemCode7, txtItemName7, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtItemCode8, txtItemName8, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtItemCode9, txtItemName9, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtItemCode10, txtItemName10, "TBM0100", new object[] { this.cboPlantCode_H, "" });
            }
            else
            {
                _BtbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { LoginInfo.PlantAuth, txtOPCode, "" }
                      , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                _BtbManager.PopUpAdd(txtItemCode1, txtItemName1, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtItemCode2, txtItemName2, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtItemCode3, txtItemName3, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtItemCode4, txtItemName4, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtItemCode5, txtItemName5, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtItemCode6, txtItemName6, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtItemCode7, txtItemName7, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtItemCode8, txtItemName8, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtItemCode9, txtItemName9, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtItemCode10, txtItemName10, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
            }
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            _UltraGridUtil.InitializeGrid(this.grid1, true, false, false, "", false);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "CARTYPE", "차종", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품번", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "PLANQTY", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "TOTQTY", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "TOTALRATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P01", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M01", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M01RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P02", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M02", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M02RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P03", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M03", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M03RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P04", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M04", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M04RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P05", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M05", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M05RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P06", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M06", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M06RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P07", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M07", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M07RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P08", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M08", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M08RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P09", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M09", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M09RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P10", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M10", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M10RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P11", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M11", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M11RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "P12", "계획량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M12", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "M12RATE", "달성율(%)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,0", null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid1);

            string[] sMergeColumn = { "PLANTCODE", "CARTYPE", "ITEMCODE", "ITEMNAME" };
            string[] sMergeColumnSum = { "PLANQTY", "TOTQTY", "TOTALRATE" };
            string[] sMergeColumn1 = { "P01", "M01", "M01RATE" };
            string[] sMergeColumn2 = { "P02", "M02", "M02RATE" };
            string[] sMergeColumn3 = { "P03", "M03", "M03RATE" };
            string[] sMergeColumn4 = { "P04", "M04", "M04RATE" };
            string[] sMergeColumn5 = { "P05", "M05", "M05RATE" };
            string[] sMergeColumn6 = { "P06", "M06", "M06RATE" };
            string[] sMergeColumn7 = { "P07", "M07", "M07RATE" };
            string[] sMergeColumn8 = { "P08", "M08", "M08RATE" };
            string[] sMergeColumn9 = { "P09", "M09", "M09RATE" };
            string[] sMergeColumn10 = { "P10", "M10", "M10RATE" };
            string[] sMergeColumn11 = { "P11", "M11", "M11RATE" };
            string[] sMergeColumn12 = { "P12", "M12", "M12RATE" };

            string[] sHeadColumn = {"PLANTCODE", "CARTYPE", "ITEMCODE","ITEMNAME", "PLANQTY", "TOTQTY", "TOTALRATE","P01","M01","M01RATE", "P02", "M02", "M02RATE", "P03", "M03", "M03RATE", 
            "P04","M04", "M04RATE", "P05", "M05", "M05RATE", "P06", "M06", "M06RATE", "P07", "M07", "M07RATE", 
            "P08", "M08","M08RATE", "P09", "M09", "M09RATE", "P10", "M10", "M10RATE", "P11", "M11", "M11RATE", 
            "P12", "M12","M12RATE"};

            _UltraGridUtil.GridHeaderMerge(grid1, "G1", "합계", sMergeColumnSum, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G2", "1월", sMergeColumn1, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G3", "2월", sMergeColumn2, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G4", "3월", sMergeColumn3, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G5", "4월", sMergeColumn4, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G6", "5월", sMergeColumn5, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G7", "6월", sMergeColumn6, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G8", "7월", sMergeColumn7, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G9", "8월", sMergeColumn8, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G10", "9월", sMergeColumn9, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G11", "10월", sMergeColumn10, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G12", "11월", sMergeColumn11, sHeadColumn);
            _UltraGridUtil.GridHeaderMerge(grid1, "G13", "12월", sMergeColumn12, sHeadColumn);

            _UltraGridUtil.GridHeaderMergeVertical(grid1, sHeadColumn, 0, 3);

            _RtnComDt = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", _RtnComDt, "CODE_ID", "CODE_NAME");

            this.ultraChart1.EmptyChartText = string.Empty;
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            ultraChart1.DataItemOver += new Infragistics.UltraChart.Shared.Events.DataItemOverEventHandler(ultraChart1_DataItemOver);
            this.Disposed += new EventHandler(PP4500_Disposed);

        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            ultraChart1.DataItemOver -= new Infragistics.UltraChart.Shared.Events.DataItemOverEventHandler(ultraChart1_DataItemOver);
            this.Disposed -= new EventHandler(PP4500_Disposed);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
            SqlParameter[] sqlParameters = new SqlParameter[14];

            if (SqlDBHelper.nvlString(this.cboPlantCode_H.Value).Equals(string.Empty))
            {
                ShowDialog("사업장은 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);
                return;
            }

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);
                string sYyyy = Convert.ToDateTime(this.cboYear_H.Value).ToString("yyyy");
                string sOPCode = this.txtOPCode.Text.Trim();
                string sItemCode1 = this.txtItemCode1.Text.Trim();
                string sItemCode2 = this.txtItemCode2.Text.Trim();
                string sItemCode3 = this.txtItemCode3.Text.Trim();
                string sItemCode4 = this.txtItemCode4.Text.Trim();
                string sItemCode5 = this.txtItemCode5.Text.Trim();
                string sItemCode6 = this.txtItemCode6.Text.Trim();
                string sItemCode7 = this.txtItemCode7.Text.Trim();
                string sItemCode8 = this.txtItemCode8.Text.Trim();
                string sItemCode9 = this.txtItemCode9.Text.Trim();
                string sItemCode10 = this.txtItemCode10.Text.Trim();

                sqlParameters[0] = sqlDBHelper.CreateParameter("PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("YYYY", sYyyy, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[2] = sqlDBHelper.CreateParameter("OPCODE", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[3] = sqlDBHelper.CreateParameter("ITEMCODE1", sItemCode1, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[4] = sqlDBHelper.CreateParameter("ITEMCODE2", sItemCode2, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[5] = sqlDBHelper.CreateParameter("ITEMCODE3", sItemCode3, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[6] = sqlDBHelper.CreateParameter("ITEMCODE4", sItemCode4, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[7] = sqlDBHelper.CreateParameter("ITEMCODE5", sItemCode5, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[8] = sqlDBHelper.CreateParameter("ITEMCODE6", sItemCode6, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[9] = sqlDBHelper.CreateParameter("ITEMCODE7", sItemCode7, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[10] = sqlDBHelper.CreateParameter("ITEMCODE8", sItemCode8, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[11] = sqlDBHelper.CreateParameter("ITEMCODE9", sItemCode9, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[12] = sqlDBHelper.CreateParameter("ITEMCODE10", sItemCode10, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[13] = sqlDBHelper.CreateParameter("LINECODE", txtLineCode.Text, SqlDbType.VarChar, ParameterDirection.Input);

                _RtnComDt = sqlDBHelper.FillTable("USP_PP4500_S3N", CommandType.StoredProcedure, sqlParameters);

                grid1.DataSource = _RtnComDt;
                grid1.DataBind();

                if (_RtnComDt.Rows.Count > 0)
                {
                    ultraChart1.Series.Clear();

                    DataTable dt = new DataTable();

                    dt.Columns.Add("ITEMCODE", typeof(System.String));
                    dt.Columns.Add("1월", typeof(System.Double));
                    dt.Columns.Add("2월", typeof(System.Double));
                    dt.Columns.Add("3월", typeof(System.Double));
                    dt.Columns.Add("4월", typeof(System.Double));
                    dt.Columns.Add("5월", typeof(System.Double));
                    dt.Columns.Add("6월", typeof(System.Double));
                    dt.Columns.Add("7월", typeof(System.Double));
                    dt.Columns.Add("8월", typeof(System.Double));
                    dt.Columns.Add("9월", typeof(System.Double));
                    dt.Columns.Add("10월", typeof(System.Double));
                    dt.Columns.Add("11월", typeof(System.Double));
                    dt.Columns.Add("12월", typeof(System.Double));

                    for (int i = 0; i < _RtnComDt.Rows.Count; i++)
                    {
                        dt.Rows.Add(new object[] {
                            _RtnComDt.Rows[i]["ITEMCODE"].ToString(),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M01"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M02"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M03"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M04"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M05"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M06"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M07"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M08"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M09"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M10"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M11"])),
                            GetNVLNull(Convert.ToDouble(_RtnComDt.Rows[i]["M12"]))
                        });
                    }

                    ultraChart1.DataSource = dt;

                    ultraChart1.LineChart.NullHandling = Infragistics.UltraChart.Shared.Styles.NullHandling.DontPlot;
                    ultraChart1.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
                    ultraChart1.Tooltips.Font = new Font("맑은 고딕", 12);
                }
            }
            catch (Exception ex)
            {
                this.ShowDialog(ex.ToString(), DialogForm.DialogType.OK);
                Debug.WriteLine(ex.ToString());
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
        /// Get nvl null
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public string GetNVLNull(double obj, string def = null)
        {
            if (obj == 0)
            {
                return def;
            }

            return obj.ToString();
        }

        #endregion
    }
}
