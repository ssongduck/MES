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
using SAMMI.PopUp;
using SAMMI.Common;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;

namespace SAMMI.CM
{
    public partial class CM0800 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        //비지니스 로직 객체 생성
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataSet rtnDsTemp2 = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp2 = new DataTable(); // return DataTable 공통
        DataSet rtnDsTemp3 = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp3 = new DataTable(); // return DataTable 공통

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        DataTable DtChange = new DataTable();
        DataTable DtChange2 = new DataTable();
        DataTable DtChange3 = new DataTable();

        Control.Grid curGrid =null;
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private int _Fix_Col = 0;
        private string PlantCode = string.Empty;
        #endregion

        public CM0800()
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

            btbManager = new BizTextBoxManagerEX();
            gridManager = new BizGridManagerEX(grid1);

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtMoldCode, txtMoldName, "TBM1600", new object[] { this.cboPlantCode_H, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtMoldCode, txtMoldName, "TBM1600", new object[] { LoginInfo.PlantAuth, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }

            //gridManager.PopUpAdd("ItemCode", "ItemName", "TBM0100", new string[] { "PlantCode", "", "" });
            gridManager.PopUpAdd("WorkCenterCode", "WorkCenterName", "TBM0600", new string[] { "PlantCode", "", "", "" });


            GridInit();
            curGrid = grid1;
        }

        #region <TOOL BAR AREA >
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[10];
            
            try
            {
                DtChange.Clear();
                DtChange2.Clear();  //
                DtChange3.Clear();  //

                //this.PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                base.DoInquire();


                param[0] = helper.CreateParameter("@pPlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@pWorkCenterCode", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@pMoldCode", txtMoldCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@pDT_FRH", ((DateTime)this.calRegDT_FRH.Value).ToString("yyyy-MM-dd"), SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@pDT_TOH", ((DateTime)this.calRegDT_TOH.Value).ToString("yyyy-MM-dd"), SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@pEmergencyFlag", SqlDBHelper.nvlString(cboEmergencyFlag.Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@pCauseFlag", SqlDBHelper.nvlString(cboCauseFlag.Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[7] = helper.CreateParameter("@pCompleteFlag", SqlDBHelper.nvlString(cboCompleteFlag.Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[8] = helper.CreateParameter("@pdiv", (radioButton1.Checked?"O":"R"), SqlDbType.VarChar, ParameterDirection.Input);
                param[9] = helper.CreateParameter("@pMoldName", txtMoldName.Text, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_CM0800_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_CM0800_S1_UNION", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;

                //_Common.Grid_Column_Width(this.grid1); //grid 정리용
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }

        }

        public override void DoNew()
        {
            base.DoNew();
            int iRow = _GridUtil.AddRow(curGrid,(DataTable)(curGrid.DataSource));
           
            //curGrid.Rows[iRow].Cells["WorkCenterCode"].Activate();


            //UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterName", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterOPCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterOPName", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemName", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "InspCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "InspName", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkType", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "MeasureEquID", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "MeasureEquName", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "InspCount", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "InspCyCle", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "SampleCount", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Spec", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "USL", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "LSL", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "UTolVal", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "CL", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "LTolVal", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "CTLMajorCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "CTLMinorCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "MeasureEquType", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "StatsFlag", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "CriticalApplyFlag", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "HIPISflag", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "XMLSendType", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkSheetDisplayFlag", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Empcode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Remarks", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "ProcType", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "PRECISION", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "XMLJudgeType", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);

        }

        public override void DoDelete()
        {
            base.DoDelete();

            this.curGrid.DeleteRow();
        }

        public override void DoSave()
        {
            return;

        }
        #endregion

        #region <Grid Setting>
        private void GridInit()
        {
            #region Grid1
            {
                _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
                // InitColumnUltraGrid
                // 0. gird 명, 1 칼럼명, 2.Caption  3. colNotNullable, 4.colDataType
                // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
                // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

                _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "MoldCode", "금형코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "cartype", "차종", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "itemname", "품명", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
               _GridUtil.InitColumnUltraGrid(grid1, "MoldName", "형번", true, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
               _GridUtil.InitColumnUltraGrid(grid1, "itemcode", "품번", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
               _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
               _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
               _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "생산기준일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "Seq", "순번", true, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "EmergencyFlag", "긴급여부", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "CauseFlag", "등록사유", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "ErrorDate", "고장등록일시", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "ErrorWorker", "등록자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "RepairDate", "수리처리일시", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "RepairWorker", "처리자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "RepairFlag", "수리완료", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

                _GridUtil.SetInitUltraGridBind(grid1);
                DtChange = (DataTable)grid1.DataSource;
                //     ///row number
                grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
                grid1.DisplayLayout.Override.RowSelectorWidth = 40;
                grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

                DataTable rtnDtTemp;
                rtnDtTemp = _Common.GET_TBM0000_CODE("EmergencyFlag");     //사용여부
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "EmergencyFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

                rtnDtTemp = _Common.GET_TBM0000_CODE("CauseFlag");     //사용여부
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "CauseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

                rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");     //사용여부
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "RepairFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

                rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

                SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            }
            #endregion

            #region Grid2
            {
                _GridUtil.InitializeGrid(this.grid2, false, true, false, "", false);
                // InitColumnUltraGrid
                // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
                // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
                // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

                _GridUtil.InitColumnUltraGrid(grid2, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "WorkCenterCode", "작업장코드", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "MoldCode", "금형코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "RecDate", "생산기준일자", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "Seq", "순번", true, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "UnitSeq", "순번", true, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "MoldErrCode", "고장항목(1)", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "MoldErrLocCode1", "고장항목(2)", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "MoldErrLocCode2", "고장위치", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "MoldErrDivCode", "고장현상", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "RepairCnt", "수리등록건수", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid2, "Status", "상태", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);

                _GridUtil.SetInitUltraGridBind(grid2);
                DtChange2 = (DataTable)grid2.DataSource;
                //     ///row number
                grid2.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
                grid2.DisplayLayout.Override.RowSelectorWidth = 40;
                grid2.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grid2.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

                DataTable rtnDtTemp;

                rtnDtTemp = _Common.GET_TBM0000_CODE("MD007");     //사용여부
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid2, "MoldErrCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

                rtnDtTemp = _Common.GET_TBM0000_CODE("MD008");     //사용여부
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid2, "MoldErrLocCode1", rtnDtTemp, "CODE_ID", "CODE_NAME");

                rtnDtTemp = _Common.GET_TBM0000_CODE("MD009");     //사용여부
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid2, "MoldErrLocCode2", rtnDtTemp, "CODE_ID", "CODE_NAME");

                rtnDtTemp = _Common.GET_TBM0000_CODE("MD010");     //사용여부
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid2, "MoldErrDivCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

                rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid2, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

                SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid2);
            }
            #endregion

            #region Grid3
            {
                _GridUtil.InitializeGrid(this.grid3, false, true, false, "", false);
                // InitColumnUltraGrid
                // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
                // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
                // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

                _GridUtil.InitColumnUltraGrid(grid3, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "WorkCenterCode", "작업장코드", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "MoldCode", "금형코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "RecDate", "생산기준일자", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "Seq", "순번", true, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "UnitSeq", "순번", true, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "RepairSeq", "순번", true, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "RepairItem", "수리항목", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "MoldRepairCode", "수리내용", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "MoldPartSpec", "규격", true, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "MoldPartNo", "No", true, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "MoldPartQty", "수량", true, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "Remark", "비고", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
                _GridUtil.InitColumnUltraGrid(grid3, "Status", "상태", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);

                _GridUtil.SetInitUltraGridBind(grid3);
                DtChange3 = (DataTable)grid3.DataSource;
                //     ///row number
                grid3.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
                grid3.DisplayLayout.Override.RowSelectorWidth = 40;
                grid3.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grid3.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

                DataTable rtnDtTemp;

                rtnDtTemp = _Common.GET_TBM0000_CODE("MD011");     //사용여부
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid3, "MoldRepairCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
                rtnDtTemp = _Common.GET_TBM0000_CODE("MD012");     //사용여부
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid3, "RepairItem", rtnDtTemp, "CODE_ID", "CODE_NAME");

                rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid3, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

                SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid3);
            }
            #endregion
        }
        #endregion

        private void grid1_ClickCell(object sender, ClickCellEventArgs e)
        {
            curGrid = grid1; 
            if (e.Cell == null || e.Cell.Row.Index < 0) return;
            SqlDBHelper helper = new SqlDBHelper(true, false);

            try
            {
                #region Grid2
                {
                    SqlParameter[] param = new SqlParameter[4];

                    DtChange2.Clear();

                    base.DoInquire();

                    param[0] = helper.CreateParameter("@pPlantCode", SqlDBHelper.nvlString(e.Cell.Row.Cells["PlantCode"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                    param[1] = helper.CreateParameter("@pMoldCode", SqlDBHelper.nvlString(e.Cell.Row.Cells["MoldCode"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                    param[2] = helper.CreateParameter("@pRecDate", SqlDBHelper.nvlString(e.Cell.Row.Cells["RecDate"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                    param[3] = helper.CreateParameter("@pSeq", SqlDBHelper.nvlInt(e.Cell.Row.Cells["Seq"].Value), SqlDbType.Int, ParameterDirection.Input);

                    //rtnDtTemp2 = helper.FillTable("[USP_CM0800_S2]", CommandType.StoredProcedure, param);
                    rtnDtTemp2 = helper.FillTable("[USP_CM0800_S2_UNION]", CommandType.StoredProcedure, param);
                    grid2.DataSource = rtnDtTemp2;
                    grid2.DataBind();

                    DtChange2 = rtnDtTemp2;

                    //_Common.Grid_Column_Width(this.grid1); //grid 정리용
                }
                #endregion

                #region Grid3
                {
                    if (grid2.Rows.Count== 0) return;
                     SqlParameter[] param = new SqlParameter[5];

                    DtChange3.Clear();

                    base.DoInquire();

                    param[0] = helper.CreateParameter("@pPlantCode", SqlDBHelper.nvlString(e.Cell.Row.Cells["PlantCode"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                    param[1] = helper.CreateParameter("@pMoldCode", SqlDBHelper.nvlString(grid2.Rows[0].Cells["MoldCode"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                    param[2] = helper.CreateParameter("@pRecDate", SqlDBHelper.nvlString(grid2.Rows[0].Cells["RecDate"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                    param[3] = helper.CreateParameter("@pSeq", SqlDBHelper.nvlInt(grid2.Rows[0].Cells["Seq"].Value), SqlDbType.Int, ParameterDirection.Input);
                    param[4] = helper.CreateParameter("@pUnitSeq", SqlDBHelper.nvlInt(grid2.Rows[0].Cells["UnitSeq"].Value), SqlDbType.Int, ParameterDirection.Input);

                    //rtnDtTemp3 = helper.FillTable("[USP_CM0800_S3]", CommandType.StoredProcedure, param);
                    rtnDtTemp3 = helper.FillTable("[USP_CM0800_S3_UNION]", CommandType.StoredProcedure, param);
                    grid3.DataSource = rtnDtTemp3;
                    grid3.DataBind();

                    DtChange3 = rtnDtTemp3;

                    //_Common.Grid_Column_Width(this.grid1); //grid 정리용
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                //if (param != null) { param = null; }
                this.ClosePrgForm();
            } 
            


        }

        private void grid2_ClickCell(object sender, ClickCellEventArgs e)
        {
            curGrid = grid2; 
            if (e.Cell == null || e.Cell.Row.Index < 0) return;



            SqlDBHelper helper = new SqlDBHelper(false);


            try
            {
                #region Grid3
                SqlParameter[] param = new SqlParameter[5];

                DtChange3.Clear();

                base.DoInquire();

                param[0] = helper.CreateParameter("@pPlantCode", SqlDBHelper.nvlString(e.Cell.Row.Cells["PlantCode"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@pMoldCode", SqlDBHelper.nvlString(e.Cell.Row.Cells["MoldCode"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@pRecDate", SqlDBHelper.nvlString(e.Cell.Row.Cells["RecDate"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@pSeq", SqlDBHelper.nvlInt(e.Cell.Row.Cells["Seq"].Value), SqlDbType.Int, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@pUnitSeq", SqlDBHelper.nvlInt(e.Cell.Row.Cells["UnitSeq"].Value), SqlDbType.Int, ParameterDirection.Input);

                rtnDtTemp3 = helper.FillTable("[USP_CM0800_S3]", CommandType.StoredProcedure, param);
                grid3.DataSource = rtnDtTemp3;
                grid3.DataBind();

                DtChange3 = rtnDtTemp3;

                //_Common.Grid_Column_Width(this.grid1); //grid 정리용
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                //if (param != null) { param = null; }
                this.ClosePrgForm();
            } 
            
        }

        private void grid3_ClickCell(object sender, ClickCellEventArgs e)
        {
            curGrid = grid3;
        }

        private void grid2_Click(object sender, EventArgs e)
        {
            curGrid = grid2;
        }

        private void grid3_Click(object sender, EventArgs e)
        {
            curGrid = grid3;
        }
    }
}
