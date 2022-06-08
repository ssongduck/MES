#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : QM2800
//   Form Name      : 자주 검사 실적 조회
//   Name Space     : SAMMI.QM
//   Created Date   : 2012.03.09
//   Made By        : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description    :
//   DB Table       : 
//   StoreProcedure : 
// *---------------------------------------------------------------------------------------------*
#endregion

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
    public partial class QM2800 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        #region <Form Load>
        private void QM2800_Load(object sender, EventArgs e)
        {
            #region <Grid Setting>
            string site = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["SITE"].Value;

            _GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 140, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOpName", "OP", false, GridColDataType_emu.VarChar, 120, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "검사항목", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspType", "구분", false, GridColDataType_emu.Double, 70, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MeasureEquID", "측정장비", false, GridColDataType_emu.VarChar, 90, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachCode", "설비", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IODCode", "가공라인", false, GridColDataType_emu.VarChar, 80, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Shift", "Shift", false, GridColDataType_emu.VarChar, 80, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SPEC", "SPEC", false, GridColDataType_emu.Double, 50, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "USL", "USL", false, GridColDataType_emu.Double, 50, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LSL", "LSL", false, GridColDataType_emu.Double, 50, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "A_Value", "1차값", false, GridColDataType_emu.Double, 70, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "A_Judge", "1차판정", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "A_Time", "1차시간", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //if (site == "EC")
                _GridUtil.InitColumnUltraGrid(grid1, "A_SN", "1차 S/N", false, GridColDataType_emu.VarChar, 90, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "B_Value", "2차값", false, GridColDataType_emu.Double, 70, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "B_Judge", "2차판정", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "B_Time", "2차시간", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //if (site == "EC")
                _GridUtil.InitColumnUltraGrid(grid1, "B_SN", "2차 S/N", false, GridColDataType_emu.VarChar, 90, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "C_Value", "3차값", false, GridColDataType_emu.Double, 70, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "C_Judge", "3차판정", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "C_Time", "3차시간", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //if (site == "EC")
                _GridUtil.InitColumnUltraGrid(grid1, "C_SN", "3차 S/N", false, GridColDataType_emu.VarChar, 90, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Inspector", "검사자", false, GridColDataType_emu.VarChar, 80, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            
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
            #endregion Grid MERGE


            //grid1.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.White;
            //grid1.DisplayLayout.Override.HeaderAppearance.BackColor = Color.Blue;
            //grid1.UseAppStyling = false;

            
        }
        #endregion

        #region < CONSTRUCTOR >
        public QM2800()
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

            #region <콤보파일 셋팅>
            _rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            //SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, _rtnDtTemp, _rtnDtTemp.Columns["CODE_ID"].ColumnName, _rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", _rtnDtTemp, "CODE_ID", "CODE_NAME");
            //this.cboPlantCode_H.SelectedIndex = 1;
            this.cboWorkType_H.SelectionStart  = 2;
            #endregion

            #region <POPUP>
            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtWorkCenterOPCode, txtWorkCenterOPName, "TBM0610", new object[] { this.cboPlantCode_H, txtWorkCenterCode, "", "", "" }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { txtWorkCenterCode, txtWorkCenterName });
                btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { this.cboPlantCode_H, "", "", "", "" });
                //btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.PlantCode, "", "" }
                //         , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
           , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtWorkCenterOPCode, txtWorkCenterOPName, "TBM0610", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, "", "", "" }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { txtWorkCenterCode, txtWorkCenterName });
                btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { LoginInfo.PlantAuth, "", "", "", "" });
                //btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "", "" }
                //         , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
           , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }

            #endregion

        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            base.DoInquire();

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[11];

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
            //string sWorkType = SqlDBHelper.nvlString(cboWorkType_H.Value);
            string sDayNight = SqlDBHelper.nvlString(this.cboDayNight.Value);
            string sJudge = SqlDBHelper.nvlString(this.cboJudge.Value);
            string sInspProc = SqlDBHelper.nvlString(this.cboInspProc.Value);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);
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
            param[10] = helper.CreateParameter("@InspProc", sInspProc, SqlDbType.VarChar, ParameterDirection.Input);
            
            rtnDtTemp = helper.FillTable("USP_QM2800_S3_UNION", CommandType.StoredProcedure, param);
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

        #region <검사코드>
        private void txtMachCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //Search_Pop_TBM0700();
            }
        }


        private void txtMachName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //Search_Pop_TBM0700();
            }
        }


        private void txtMachCode_KeyDown(object sender, KeyEventArgs e)
        {
            //this.txtMachCode.Text = string.Empty;
        }

        private void txtMachName_KeyDown(object sender, KeyEventArgs e)
        {
            //this.txtMachName.Text = string.Empty;
        }

        private void txtMachCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_TBM0700();
        }

        private void txtMachName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_TBM0700();
        }

        private void Search_Pop_TBM0700()
        {
            //string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.SelectedValue);
            //string sMachCode = txtMachCode.Text.Trim();       //설비코드
            //string sMachName = txtMachName.Text.Trim();      //설비명 

            //try
            //{
            //    _biz.TBM0700_POP(sPlantCode, sMachCode, sMachName, "", "", "", "", txtMachCode, txtMachName);

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("ERROR", ex.Message);
            //}

        }
        #endregion

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "InspName")
            //    {
            //        _Fix_Col = i;
            //    }
            //}

            //for (int i = 0; i < _Fix_Col + 1; i++)
            //{
            //    e.Layout.UseFixedHeaders = true;
            //    e.Layout.Bands[0].Columns[i].Header.Fixed = true;

            //}
        }

        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["A_Judge"].Value.ToString() == "NG")
            {
                e.Row.Cells["A_Value"].Appearance.ForeColor = Color.Red;
                e.Row.Cells["A_Judge"].Appearance.ForeColor = Color.Red;
            }
            if (e.Row.Cells["B_Judge"].Value.ToString() == "NG")
            {
                e.Row.Cells["B_Value"].Appearance.ForeColor = Color.Red;
                e.Row.Cells["B_Judge"].Appearance.ForeColor = Color.Red;
            }
            if (e.Row.Cells["C_Judge"].Value.ToString() == "NG")
            {
                e.Row.Cells["C_Value"].Appearance.ForeColor = Color.Red;
                e.Row.Cells["C_Judge"].Appearance.ForeColor = Color.Red;
            }
        }

        
    }    
}
