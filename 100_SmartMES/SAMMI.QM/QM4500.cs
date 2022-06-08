#region <USING AREA>
using SAMMI.Common;
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using SAMMI.PopUp;
using SAMMI.PopManager;
#endregion

namespace SAMMI.QM
{
    public partial class QM4500 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        // 변수나 Form에서 사용될 Class를 정의
        DataTable _rtnDtTemp = new DataTable();
        DataSet rtnDsTemp = new DataSet();
        DataTable rtnDtTemp = new DataTable();
        private DataTable DtChange = null;
        BizGridManagerEX gridManager;
        private string PlantCode = string.Empty;
        private string WorkCenterCode = string.Empty;
        private string WorkCenterOPCode = string.Empty;
        private string InspCode = string.Empty;
        private string ItemCode = string.Empty;
        private string MeasureEquID = string.Empty;
        private string MACHCode = string.Empty;
        private string WorkType = string.Empty;
        private DateTime FRDT = System.DateTime.Now;
        private DateTime TODT = System.DateTime.Now;

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private int _Fix_Col = 0;
        #endregion

        public QM4500()
        {
            InitializeComponent();

            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this.PlantCode.Equals("SK"))
                this.PlantCode = "SK1";
            else if (this.PlantCode.Equals("EC"))
                this.PlantCode = "SK2";
            if (!(this.PlantCode.Equals("SK1") || this.PlantCode.Equals("SK2")))
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;

            this.calRegDT_FRH.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00");
            this.calRegDT_TOH.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 23:59:59");

            
            #region <POPUP>
            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtWorkCenterOPCode, txtWorkCenterOPName, "TBM0610", new object[] { this.cboPlantCode_H, txtWorkCenterCode, "", "", "" }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { txtWorkCenterCode, txtWorkCenterName });
                btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { this.cboPlantCode_H, "", "", "", "" });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtWorkCenterOPCode, txtWorkCenterOPName, "TBM0610", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, "", "", "" }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { txtWorkCenterCode, txtWorkCenterName });
                btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { LoginInfo.PlantAuth, "", "", "", "" });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }
            #endregion
        }

        private void QM4500_Load(object sender, EventArgs e)
        {
            #region <Grid Setting>

            _GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 140, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOpName", "OP", false, GridColDataType_emu.VarChar, 120, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "검사항목", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspType", "구분", false, GridColDataType_emu.Double, 70, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MeasureEquID", "측정장비", false, GridColDataType_emu.VarChar, 90, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachCode", "설비", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IODCode", "No", false, GridColDataType_emu.VarChar, 40, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Shift", "Shift", false, GridColDataType_emu.VarChar, 80, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SPEC", "SPEC", false, GridColDataType_emu.Double, 50, 0, Infragistics.Win.HAlign.Center, false, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "USL", "USL", false, GridColDataType_emu.Double, 50, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LSL", "LSL", false, GridColDataType_emu.Double, 50, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "A_Value", "측정값", false, GridColDataType_emu.Double, 70, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "A_Judge", "판정", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "A_Time", "검사시각", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Inspector", "검사자", false, GridColDataType_emu.VarChar, 60, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("DAYNIGHT");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Shift", rtnDtTemp, "CODE_ID", "CODE_NAME");

            DtChange = (DataTable)grid1.DataSource;
            _GridUtil.SetInitUltraGridBind(this.grid1);
            #endregion

            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            #region Grid MERGE
            grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["WorkCenterOpName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["WorkCenterOpName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["WorkCenterOpName"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["ItemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["ItemCode"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["InspName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["InspName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["InspName"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["Inspector"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["Inspector"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["Inspector"].MergedCellStyle = MergedCellStyle.Always;
            #endregion Grid MERGE
        }

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            base.DoInquire();

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[10];

            DateTime planstartdt = Convert.ToDateTime(((DateTime)this.calRegDT_FRH.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");
            DateTime planenddt = Convert.ToDateTime(((DateTime)this.calRegDT_TOH.Value).ToString("yyyy-MM-dd") + " 23:59:59.99");
            if (Convert.ToInt32(planstartdt.ToString("yyyyMMdd")) > Convert.ToInt32(planenddt.ToString("yyyMMdd")))
            {
                SException ex = new SException("R00200", null);
                throw ex;
            }

            string sWorkCenterCode = txtWorkCenterCode.Text.Trim();
            string sWorkCenterOPCode = txtWorkCenterOPCode.Text.Trim();
            string sInspCode = this.txtInspCode.Text.Trim();
            string sItemCode = this.txtItemCode.Text.Trim();
            string sMeasureEquID = this.txtMeasureEquID.Text.Trim();
            string sJudge = SqlDBHelper.nvlString(this.cboJudge.Value);
            string sDayNight = SqlDBHelper.nvlString(this.cboDayNight.Value);

            //string sWorkType = SqlDBHelper.nvlString(cboWorkType_H.Value);


            this.PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            this.WorkCenterCode = sWorkCenterCode;
            this.WorkCenterOPCode = sWorkCenterOPCode;
            this.InspCode = sInspCode;
            this.ItemCode = sItemCode;
            //this.WorkType = sWorkType;
            this.FRDT = planstartdt;
            this.TODT = planenddt;

            param[0] = helper.CreateParameter("@PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
            param[1] = helper.CreateParameter("@WorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
            param[2] = helper.CreateParameter("@WorkCenterOPCode", WorkCenterOPCode, SqlDbType.VarChar, ParameterDirection.Input);
            param[3] = helper.CreateParameter("@InspCode", InspCode, SqlDbType.VarChar, ParameterDirection.Input);
            param[4] = helper.CreateParameter("@ItemCode", ItemCode, SqlDbType.VarChar, ParameterDirection.Input);
            param[5] = helper.CreateParameter("@MeasureEquID", sMeasureEquID, SqlDbType.VarChar, ParameterDirection.Input);
            param[6] = helper.CreateParameter("@FRDT", FRDT, SqlDbType.DateTime, ParameterDirection.Input);
            param[7] = helper.CreateParameter("@TODT", TODT, SqlDbType.DateTime, ParameterDirection.Input);
            param[8] = helper.CreateParameter("@DayNight", sDayNight, SqlDbType.VarChar, ParameterDirection.Input);
            param[9] = helper.CreateParameter("@Judge", sJudge, SqlDbType.VarChar, ParameterDirection.Input);
            //param[8] = helper.CreateParameter("@WorkType", sWorkType, SqlDbType.VarChar, ParameterDirection.Input);

            rtnDtTemp = helper.FillTable("USP_QM2800_S3N_UNION", CommandType.StoredProcedure, param);
            if (rtnDtTemp.Rows.Count == 0)
            {
                //MessageBox.Show("DATA가 없습니다.");
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();
            }
            else
            {
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;
            }
        }
        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
        }
        #endregion

        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["A_Judge"].Value.ToString() == "NG")
            {
                e.Row.Cells["A_Value"].Appearance.ForeColor = Color.Red;
                e.Row.Cells["A_Judge"].Appearance.ForeColor = Color.Red;
            }

        }
    }
}
