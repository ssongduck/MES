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
namespace SAMMI.QM
{
    public partial class QM2900 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        private string ItemCode = string.Empty;
        private string InspCode = string.Empty;
        private string SerialNo = string.Empty;
        private string LotNo = string.Empty;
        private string HipisFlag = string.Empty;
        private string DayNight = string.Empty;
        private DateTime FRDT = System.DateTime.Now;
        private DateTime TODT = System.DateTime.Now;

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private int _Fix_Col = 0;
        #endregion

        public QM2900()
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

            this.cboStartDate_H.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00");
            this.cboEndDate_H.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 23:59:59");

        }

        private void QM2900_Load(object sender, EventArgs e)
        {
            GridInit();

            #region <POPUP>
            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });

                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { this.cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
           , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
                btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { this.cboPlantCode_H, "", "", "" });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });

                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
           , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
                btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { LoginInfo.PlantAuth, "", "", "" });
            }
            #endregion


        }

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            if (SqlDBHelper.nvlString(this.cboPlantCode_H.Value).Equals("") ||
                SqlDBHelper.nvlString(txtWorkCenterCode.Text).Equals(""))
            {
                ShowDialog("사업장, 작업장코드는 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);

                CancelProcess = true;
                return;
            }

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[11];
            DateTime StartInspDT = Convert.ToDateTime(((DateTime)this.cboStartDate_H.Value).ToString("yyyy-MM-dd") + " 08:00:00.00");
            DateTime EndInspDT = Convert.ToDateTime(((DateTime)this.cboEndDate_H.Value).AddDays(1).ToString("yyyy-MM-dd") + " 07:59:59.99");

            try
            {
                base.DoInquire();

                if (Convert.ToInt32(StartInspDT.ToString("yyyyMMdd")) > Convert.ToInt32(EndInspDT.ToString("yyyMMdd")))
                {
                    SException ex = new SException("R00200", null);
                    throw ex;
                }

                this.PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                this.WorkCenterCode = txtWorkCenterCode.Text;
                this.ItemCode = txtItemCode.Text;
                this.InspCode = txtInspCode.Text;
                this.SerialNo = txtSN.Text;
                this.LotNo = txtLotNo.Text;
                this.HipisFlag = SqlDBHelper.nvlString(cboHipisFlag.Value);
                this.DayNight = SqlDBHelper.nvlString(cboDayNight.Value);
                string sJudge = SqlDBHelper.nvlString(this.cboJudge.Value);
                this.FRDT = StartInspDT;
                this.TODT = EndInspDT;

                param[0] = helper.CreateParameter("@PlantCode", this.PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@WorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@ItemCode", ItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@InspCode", InspCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@SerialNo", SerialNo, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@LotNo", LotNo, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@HipisFlag", HipisFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[7] = helper.CreateParameter("@DayNight", DayNight, SqlDbType.VarChar, ParameterDirection.Input);
                param[8] = helper.CreateParameter("@FRDT", FRDT, SqlDbType.DateTime, ParameterDirection.Input);
                param[9] = helper.CreateParameter("@TODT", TODT, SqlDbType.DateTime, ParameterDirection.Input);
                param[10] = helper.CreateParameter("@Judge", sJudge, SqlDbType.VarChar, ParameterDirection.Input);
                
                
                rtnDtTemp = helper.FillTable("USP_QM2900_S1_UNION", CommandType.StoredProcedure, param);
                if (rtnDtTemp.Rows.Count == 0)
                {
                    // MessageBox.Show("DATA가 없습니다.");
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
                grid1.DisplayLayout.CaptionAppearance.BackColor = Color.White;
            }
        }

        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            base.DoNew();
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            try
            {
                //this.grid2.UpdateData();


                if (this.ShowDialog("Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                else return;
            }
            catch (SException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
            }
        }

        public override void DoDownloadExcel()
        {
        }
        #endregion

        #region <GridInit>
        private void GridInit()
        {
            #region <Grid Setting>
            _GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspNo", "검사번호", false, GridColDataType_emu.VarChar, 140, 10, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 140, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 120, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SerialNo", "S/N", false, GridColDataType_emu.Double, 120, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LotNo", "Lot No", false, GridColDataType_emu.VarChar, 120, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOpCode", "Op코드", false, GridColDataType_emu.VarChar, 90, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOpName", "OP", false, GridColDataType_emu.VarChar, 150, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCode", "검사항목코드", false, GridColDataType_emu.VarChar, 120, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "검사항목", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspVal", "측정치", false, GridColDataType_emu.Double, 70, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Judge", "판정", false, GridColDataType_emu.VarChar, 60, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "USL", "USL", false, GridColDataType_emu.Double, 50, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LSL", "LSL", false, GridColDataType_emu.Double, 50, 0, Infragistics.Win.HAlign.Center, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachCode", "설비코드", false, GridColDataType_emu.VarChar, 90, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MeasureEquID", "측정기코드", false, GridColDataType_emu.VarChar, 100, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MeasureEquName", "측정기", false, GridColDataType_emu.VarChar, 100, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Inspector", "검사자", false, GridColDataType_emu.VarChar, 80, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkDT", "작업일자", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspDT", "검사시각", false, GridColDataType_emu.DateTime24, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Shift", "주야", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "HipisFlag", "HIPIS", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("DAYNIGHT");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Shift", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "HipisFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

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

            grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["ItemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["ItemCode"].MergedCellStyle = MergedCellStyle.Always;
            grid1.Columns["ItemName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["ItemName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["ItemName"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["SerialNo"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["SerialNo"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["SerialNo"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["LotNo"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["LotNo"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["LotNo"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["WorkCenterOpCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["WorkCenterOpCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["WorkCenterOpCode"].MergedCellStyle = MergedCellStyle.Always;
            grid1.Columns["WorkCenterOpName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["WorkCenterOpName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["WorkCenterOpName"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["InspCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["InspCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["InspCode"].MergedCellStyle = MergedCellStyle.Always;
            grid1.Columns["InspName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["InspName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["InspName"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["MeasureEquID"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["MeasureEquID"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["MeasureEquID"].MergedCellStyle = MergedCellStyle.Always;
            grid1.Columns["MeasureEquName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["MeasureEquName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["MeasureEquName"].MergedCellStyle = MergedCellStyle.Always;
            #endregion Grid MERGE
        }
        #endregion

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "LotNo")
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

        private void gbxHeader_Click(object sender, EventArgs e)
        {

        }

        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["Judge"].Value.ToString() == "NG")
            {
                e.Row.Appearance.ForeColor = Color.Red;
            }

        }
    }
}
