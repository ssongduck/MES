using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Infragistics.UltraChart.Resources.Appearance;

using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using SAMMI.Windows.Forms;
using System.Diagnostics;

namespace SAMMI.PP
{
    /// <summary>
    /// PP4600 class
    /// </summary>
    public partial class PP4600 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        /// Popup biz
        /// </summary>
        PopUp_Biz _PopUp_Biz = new PopUp_Biz();

        /// <summary>
        /// Button manager
        /// </summary>
        BizTextBoxManagerEX _BtbManager;

        /// <summary>
        /// Temp datatable
        /// </summary>
        DataTable _TempDt = new DataTable();

        /// <summary>
        /// Plant code
        /// </summary>
        private string _PlantCode = string.Empty;

        #endregion

        #region Constructor

        public PP4600()
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
        /// OP Code textbox key down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOPCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPName.Text = string.Empty;
        }

        /// <summary>
        /// OP Code textbox key press event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOPCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SearchPopupTBM0400();
            }
        }

        /// <summary>
        /// OP Code textbox mouse double click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOPCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SearchPopupTBM0400();
        }

        /// <summary>
        /// OP Name textbox key down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOPNAME_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPCode.Text = string.Empty;
        }

        /// <summary>
        /// OP Name textbox key press event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOPName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SearchPopupTBM0400();
            }
        }

        /// <summary>
        /// OP Name mouse double click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOPName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SearchPopupTBM0400();
        }

        /// <summary>
        /// PP4600 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP4600_Disposed(object sender, EventArgs e)
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
                _BtbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                _BtbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { this.cboPlantCode_H, txtOPCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                _BtbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOPCode, txtOPName, txtLineCode, txtLineName });
            }
            else
            {
                _BtbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                _BtbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { LoginInfo.PlantAuth, txtOPCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                _BtbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOPCode, txtOPName, txtLineCode, txtLineName });
            }
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            _UltraGridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "OPCODE", "공정", false, GridColDataType_emu.VarChar, 95, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "OPNAME", "공정명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
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

            string[] sMergeColumn = { "PLANTCODE", "OPCODE", "OPNAME", "WORKCENTERCODE", "WORKCENTERNAME" };
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

            string[] sHeadColumn = {"PLANTCODE","OPCODE", "OPNAME", "WORKCENTERCODE", "WORKCENTERNAME","PLANQTY", "TOTQTY", "TOTALRATE","P01","M01","M01RATE", "P02", "M02", "M02RATE", "P03", "M03", "M03RATE", 
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

            _UltraGridUtil.GridHeaderMergeVertical(grid1, sHeadColumn, 0, 4);

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

            txtOPCode.KeyDown += new System.Windows.Forms.KeyEventHandler(txtOPCode_KeyDown);
            txtOPCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOPCode_KeyPress);
            txtOPCode.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(txtOPCode_MouseDoubleClick);
            txtOPName.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(txtOPName_MouseDoubleClick);

            txtOPName.KeyDown += new System.Windows.Forms.KeyEventHandler(txtOPNAME_KeyDown);
            txtOPName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOPName_KeyPress);

            this.Disposed += new EventHandler(PP4600_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            ultraChart1.DataItemOver -= new Infragistics.UltraChart.Shared.Events.DataItemOverEventHandler(ultraChart1_DataItemOver);

            txtOPCode.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(txtOPCode_MouseDoubleClick);
            txtOPName.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(txtOPName_MouseDoubleClick);

            this.Disposed -= new EventHandler(PP4600_Disposed);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
            SqlParameter[] sqlParameters = new SqlParameter[5];

            if (string.IsNullOrEmpty(SqlDBHelper.nvlString(this.cboPlantCode_H.Value)))
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

                sqlParameters[0] = sqlDBHelper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("Yyyy", sYyyy, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[2] = sqlDBHelper.CreateParameter("OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[3] = sqlDBHelper.CreateParameter("@LineCode", txtLineCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[4] = sqlDBHelper.CreateParameter("@WorkCenterCode", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);

                _RtnComDt = sqlDBHelper.FillTable("USP_PP4600_S2N", CommandType.StoredProcedure, sqlParameters);

                grid1.DataSource = _RtnComDt;
                grid1.DataBind();

                if (_RtnComDt.Rows.Count > 0)
                {
                    ultraChart1.Series.Clear();

                    DataTable dt = new DataTable();

                    dt.Columns.Add("WORKCENTERCODE", typeof(System.String));
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
                            _RtnComDt.Rows[i]["WORKCENTERCODE"].ToString(),
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
        /// Get NVL null
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public string GetNVLNull(double obj, string def = null)
        {
            if (obj == 0)
                return def;

            return obj.ToString();
        }

        /// <summary>
        /// Search popup item
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <param name="ItemName"></param>
        private void SearchPopupItem(TextBox ItemCode, TextBox ItemName)
        {
            string sItemCd = ItemCode.Text.Trim();
            string sItemName = ItemName.Text.Trim();
            string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value.ToString());
            string sItemType = string.Empty;

            try
            {
                _TempDt = _PopUp_Biz.SEL_TBM0100(sPlantCode, sItemCd, sItemName, sItemType);

                if (_TempDt.Rows.Count > 1)
                {
                    PopUPManager popUPManager = new PopUPManager();
                    _TempDt = popUPManager.OpenPopUp("Item", new string[] { sPlantCode, sItemType, sItemCd, sItemName });

                    if (_TempDt != null && _TempDt.Rows.Count > 0)
                    {
                        ItemCode.Text = Convert.ToString(_TempDt.Rows[0]["ItemCode"]);
                        ItemName.Text = Convert.ToString(_TempDt.Rows[0]["Itemname"]);
                    }
                }
                else
                {
                    if (_TempDt.Rows.Count == 1)
                    {
                        ItemCode.Text = Convert.ToString(_TempDt.Rows[0]["ItemCode"]);
                        ItemName.Text = Convert.ToString(_TempDt.Rows[0]["Itemname"]);
                    }
                    else
                    {
                        MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                        ItemCode.Text = string.Empty;
                        ItemName.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show("ERROR", ex.Message);
            }
        }

        /// <summary>
        /// SearchPopupTBM400
        /// </summary>
        private void SearchPopupTBM0400()
        {
            string sPlantCode = string.Empty;
            string sOPCode = txtOPCode.Text.Trim();
            string sOPName = txtOPName.Text.Trim();
            string sUseFlag = string.Empty;

            if (this.cboPlantCode_H.Value != null)
            {
                sPlantCode = cboPlantCode_H.Value.ToString() == "ALL" ? "" : cboPlantCode_H.Value.ToString();
            }
            sUseFlag = string.Empty;

            try
            {
                _PopUp_Biz.TBM0400_POP(sPlantCode, sOPCode, sOPName, sUseFlag, txtOPCode, txtOPName);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show("ERROR", ex.Message);
            }
        }

        #endregion
    }
}
