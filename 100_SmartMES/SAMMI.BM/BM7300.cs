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
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using SAMMI.Common;
namespace SAMMI.BM
{
    public partial class BM7300 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        //비지니스 로직 객체 생성
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        DataTable DtChange = new DataTable();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private int _Fix_Col = 0;

        private string PlantCode = string.Empty;
        #endregion

        public BM7300()
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
                btbManager.PopUpAdd(txtWorkCenterOPCode, txtWorkCenterOPName, "TBM0610", new object[] { this.cboPlantCode_H, txtWorkCenterCode, "", "", "" }
                        , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { txtWorkCenterCode, txtWorkCenterName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { this.cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                  , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
                btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { this.cboPlantCode_H, "", "", cboUseFlag_H });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtWorkCenterOPCode, txtWorkCenterOPName, "TBM0610", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, "", "", "" }
                        , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { txtWorkCenterCode, txtWorkCenterName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
                  , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
                btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { LoginInfo.PlantAuth, "", "", cboUseFlag_H });
            }

            gridManager.PopUpAdd("WorkCenterCode", "WorkCenterName", "TBM0600", new string[] { "PlantCode", "", "", "" });
            gridManager.PopUpAdd("WorkCenterOPCode", "WorkCenterOPName", "TBM0610", new string[] { "PlantCode", "WorkCenterCode", "" });
            gridManager.PopUpAdd("ItemCode", "ItemName", "TBM0101", new string[] { "PlantCode", "WorkCenterCode", "" });
            gridManager.PopUpAdd("InspCode", "InspName", "TBM1500", new string[] { "PlantCode", "", "", "" });
            
            GridInit();
        }

        #region <TOOL BAR AREA >
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[7];

            try
            {
                DtChange.Clear();
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();
                string sWorkCenterOPCode = txtWorkCenterOPCode.Text.Trim();
                string sItemCode = txtItemCode.Text.Trim();
                string sInspCode = txtInspCode.Text.Trim();
                string sWorkType = SqlDBHelper.gGetCode(this.cboWorkType_H.Value);
                string sUseFlag = SqlDBHelper.gGetCode(this.cboUseFlag_H.Value);

                base.DoInquire();

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@WorkCenterOPCode", sWorkCenterOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@InspCode", sInspCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@WorkType", sWorkType, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM7300_S1_UNION", CommandType.StoredProcedure, param);
                rtnDtTemp.AcceptChanges();
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
            int iRow = _GridUtil.AddRow(this.grid1, DtChange);

            UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterOPCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterOPName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "InspCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "InspName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkType", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MeasureEquID", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MeasureEquName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "InspCount", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "InspCyCle", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "SampleCount", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Spec", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "USL", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "LSL", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UTolVal", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "CL", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "LTolVal", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "CTLMajorCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "CTLMinorCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MeasureEquType", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "StatsFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "CriticalApplyFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "HIPISflag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "XMLSendType", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkSheetDisplayFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Empcode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Remarks", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ProcType", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "PRECISION", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "XMLJudgeType", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "PTInspCode", iRow);
          
        }

        public override void DoDelete()
        {
            base.DoDelete();

            this.grid1.DeleteRow();
        }

        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false, false);
            SqlParameter[] param = null;

            try
            {
                string sPlantCode = "";
                this.Focus();

                #region [Validate 체크]
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == ""
                                || SqlDBHelper.nvlString(dr["WorkCenterCode"]) == ""
                                || SqlDBHelper.nvlString(dr["WorkCenterOPCode"]) == ""
                                || SqlDBHelper.nvlString(dr["ItemCode"]) == ""
                                || SqlDBHelper.nvlString(dr["InspCode"]) == ""
                                || SqlDBHelper.nvlString(dr["WorkType"]) == "")
                            {
                                ShowDialog("작업장코드, 작업장Op코드, 품목코드, 검사항목, 작업구분은 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            if (!LoginInfo.PlantAuth.Equals("") &&
                                !LoginInfo.PlantAuth.Equals(SqlDBHelper.nvlString(dr["PlantCode"])))
                            {
                                ShowDialog("[" + SqlDBHelper.nvlString(dr["PlantCode"]) + "] 등록권한이 없습니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }

                            break;
                    }
                }
                #endregion

                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                {
                    CancelProcess = true;
                    return;
                }

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow drRow in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제 - 사용안함
                            drRow.RejectChanges();

                            param = new SqlParameter[6];

                            sPlantCode = SqlDBHelper.nvlString(drRow["PlantCode"]);

                            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[2] = helper.CreateParameter("@WorkCenterOpCode", SqlDBHelper.nvlString(drRow["WorkCenterOpCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[3] = helper.CreateParameter("@ItemCode", SqlDBHelper.nvlString(drRow["ItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[4] = helper.CreateParameter("@InspCode", SqlDBHelper.nvlString(drRow["InspCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[5] = helper.CreateParameter("@WorkType", SqlDBHelper.nvlString(drRow["WorkType"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드

                            helper.ExecuteNoneQuery("USP_BM7300_D1", CommandType.StoredProcedure, param);

                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가 - 사용안함
                            param = new SqlParameter[35];

                            sPlantCode = SqlDBHelper.nvlString(drRow["PlantCode"]);
                            //InspVal = String.Format("{0:F3}", Convert.ToDouble(SqlDBHelper.nvlString(drRow["USL"])));
                            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@WorkCenterOPCode", SqlDBHelper.nvlString(drRow["WorkCenterOPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@ItemCode", SqlDBHelper.nvlString(drRow["ItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@InspCode", SqlDBHelper.nvlString(drRow["InspCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@WorkType", SqlDBHelper.nvlString(drRow["WorkType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@MeasureEquID", SqlDBHelper.nvlString(drRow["MeasureEquID"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@InspName", SqlDBHelper.nvlString(drRow["InspName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@InspCount", SqlDBHelper.nvlString(drRow["InspCount"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@InspCycle", SqlDBHelper.nvlString(drRow["InspCycle"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@SampleCount", SqlDBHelper.nvlString(drRow["SampleCount"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@Spec", SqlDBHelper.nvlString(drRow["Spec"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[12] = helper.CreateParameter("@USL", SqlDBHelper.nvlString(drRow["USL"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[13] = helper.CreateParameter("@LSL", SqlDBHelper.nvlString(drRow["LSL"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("@USL", String.Format("{0:F3}", Convert.ToDouble(SqlDBHelper.nvlString(drRow["USL"],"0"))), SqlDbType.VarChar, ParameterDirection.Input);
                            param[13] = helper.CreateParameter("@LSL", String.Format("{0:F3}", Convert.ToDouble(SqlDBHelper.nvlString(drRow["LSL"],"0"))), SqlDbType.VarChar, ParameterDirection.Input);
                            param[14] = helper.CreateParameter("@UTolVal", SqlDBHelper.nvlString(drRow["UTolVal"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[15] = helper.CreateParameter("@CL", SqlDBHelper.nvlString(drRow["CL"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[16] = helper.CreateParameter("@LTolVal", SqlDBHelper.nvlString(drRow["LTolVal"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[17] = helper.CreateParameter("@CTLMajorCode", SqlDBHelper.nvlString(drRow["CTLMajorCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[18] = helper.CreateParameter("@CTLMinorCode", SqlDBHelper.nvlString(drRow["CTLMinorCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[19] = helper.CreateParameter("@MeasureEquType", SqlDBHelper.nvlString(drRow["MeasureEquType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[20] = helper.CreateParameter("@StatsFlag", SqlDBHelper.nvlString(drRow["StatsFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[21] = helper.CreateParameter("@CriticalApplyFlag", SqlDBHelper.nvlString(drRow["CriticalApplyFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[22] = helper.CreateParameter("@HIPISFlag", SqlDBHelper.nvlString(drRow["HIPISFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[23] = helper.CreateParameter("@XMLSendType", SqlDBHelper.nvlString(drRow["XMLSendType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[24] = helper.CreateParameter("@WorkSheetDisplayFlag", SqlDBHelper.nvlString(drRow["WorkSheetDisplayFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[25] = helper.CreateParameter("@EmpCode", SqlDBHelper.nvlString(drRow["EmpCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[26] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[27] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[28] = helper.CreateParameter("@Remarks", SqlDBHelper.nvlString(drRow["Remarks"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[29] = helper.CreateParameter("@ProcType", SqlDBHelper.nvlString(drRow["ProcType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[30] = helper.CreateParameter("@PRECISION", SqlDBHelper.nvlString(drRow["PRECISION"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[31] = helper.CreateParameter("@XMLJudgeType", SqlDBHelper.nvlString(drRow["XMLJudgeType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[32] = helper.CreateParameter("@VerFlag", SqlDBHelper.nvlString(drRow["VerFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[33] = helper.CreateParameter("@PTItemCode", SqlDBHelper.nvlString(drRow["PTItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[34] = helper.CreateParameter("@PTInspCode", SqlDBHelper.nvlString(drRow["PTInspCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            
                            helper.ExecuteNoneQuery("USP_BM7300_I1", CommandType.StoredProcedure, param);

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정 - 통합완료
                            param = new SqlParameter[37];

                            sPlantCode = SqlDBHelper.nvlString(drRow["PlantCode"]);
                            string sUSL = String.Format("{0:F3}", Convert.ToDouble(SqlDBHelper.nvlString(drRow["USL"], "0")));
                            string sLSL = String.Format("{0:F3}", Convert.ToDouble(SqlDBHelper.nvlString(drRow["LSL"], "0")));
                            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@WorkCenterOPCode", SqlDBHelper.nvlString(drRow["WorkCenterOPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@ItemCode", SqlDBHelper.nvlString(drRow["ItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@InspCode", SqlDBHelper.nvlString(drRow["InspCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@WorkType", SqlDBHelper.nvlString(drRow["WorkType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@MeasureEquID", SqlDBHelper.nvlString(drRow["MeasureEquID"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@InspName", SqlDBHelper.nvlString(drRow["InspName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@InspCount", SqlDBHelper.nvlString(drRow["InspCount"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@InspCycle", SqlDBHelper.nvlString(drRow["InspCycle"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@SampleCount", SqlDBHelper.nvlString(drRow["SampleCount"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@Spec", SqlDBHelper.nvlString(drRow["Spec"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[12] = helper.CreateParameter("@USL", SqlDBHelper.nvlString(drRow["USL"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[13] = helper.CreateParameter("@LSL", SqlDBHelper.nvlString(drRow["LSL"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("@USL", sUSL, SqlDbType.VarChar, ParameterDirection.Input);
                            param[13] = helper.CreateParameter("@LSL", sLSL, SqlDbType.VarChar, ParameterDirection.Input);
                            param[14] = helper.CreateParameter("@UTolVal", SqlDBHelper.nvlString(drRow["UTolVal"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[15] = helper.CreateParameter("@CL", SqlDBHelper.nvlString(drRow["CL"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[16] = helper.CreateParameter("@LTolVal", SqlDBHelper.nvlString(drRow["LTolVal"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[17] = helper.CreateParameter("@CTLMajorCode", SqlDBHelper.nvlString(drRow["CTLMajorCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[18] = helper.CreateParameter("@CTLMinorCode", SqlDBHelper.nvlString(drRow["CTLMinorCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[19] = helper.CreateParameter("@MeasureEquType", SqlDBHelper.nvlString(drRow["MeasureEquType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[20] = helper.CreateParameter("@StatsFlag", SqlDBHelper.nvlString(drRow["StatsFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[21] = helper.CreateParameter("@CriticalApplyFlag", SqlDBHelper.nvlString(drRow["CriticalApplyFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[22] = helper.CreateParameter("@HIPISFlag", SqlDBHelper.nvlString(drRow["HIPISFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[23] = helper.CreateParameter("@XMLSendType", SqlDBHelper.nvlString(drRow["XMLSendType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[24] = helper.CreateParameter("@WorkSheetDisplayFlag", SqlDBHelper.nvlString(drRow["WorkSheetDisplayFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[25] = helper.CreateParameter("@EmpCode", SqlDBHelper.nvlString(drRow["EmpCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[26] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[27] = helper.CreateParameter("@Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[28] = helper.CreateParameter("@Remarks", SqlDBHelper.nvlString(drRow["Remarks"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[29] = helper.CreateParameter("@ProcType", SqlDBHelper.nvlString(drRow["ProcType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[30] = helper.CreateParameter("@PRECISION", SqlDBHelper.nvlString(drRow["PRECISION"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[31] = helper.CreateParameter("@XMLJudgeType", SqlDBHelper.nvlString(drRow["XMLJudgeType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[32] = helper.CreateParameter("@VerFlag", SqlDBHelper.nvlString(drRow["VerFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[33] = helper.CreateParameter("@PTItemCode", SqlDBHelper.nvlString(drRow["PTItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[33] = helper.CreateParameter("@PTInspCode", SqlDBHelper.nvlString(drRow["PTInspCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[34] = helper.CreateParameter("@DMInspCode", SqlDBHelper.nvlString(drRow["DMInspCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[35] = helper.CreateParameter("@vInspCode", SqlDBHelper.nvlString(drRow["vInspCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[36] = helper.CreateParameter("@Sort", SqlDBHelper.nvlString(drRow["Sort"]), SqlDbType.VarChar, ParameterDirection.Input);
 
                            helper.ExecuteNoneQuery("USP_BM7300_U1", CommandType.StoredProcedure, param);

                            #endregion
                            break;
                    }
                }

                //helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                CancelProcess = true;
                //helper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion

        #region <Grid Setting>
        private void GridInit()
        {
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품목코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOPCode", "작업장 OP코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOPName", "작업장 OP명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCode", "검사코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "검사명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkType", "작업구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true,true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MeasureEquID", "측정장비ID", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MeasureEquName", "측정장비명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCount", "검사횟수", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCycle", "검사주기", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SampleCount", "시료수", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Spec", "규격", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "USL", "규격상한선", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LSL", "규격하한선", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UTolVal", "상한공차", false, GridColDataType_emu.Double, 80, 100, Infragistics.Win.HAlign.Right, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CL", "중심값", false, GridColDataType_emu.Double, 80, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LTolVal", "하한공차", false, GridColDataType_emu.Double, 80, 100, Infragistics.Win.HAlign.Right, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PTItemCode", "파워텍품목코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PTInspCode", "파워텍검사코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DMItemCode", "다이모스품목코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DMInspCode", "다이모스검사코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "vInspCode", "경측정코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Sort", "도면번호", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CTLMajorCode", "현대 중점항목 관리(대)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CTLMinorCode", "현대 중점항목 관리(소)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MeasureEquType", "측정방법(자동,수동)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StatsFlag", "통계적용 관리여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CriticalApplyFlag", "이상발생 관리여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "HIPISflag", "HIPIS 대상 여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "XMLSendType", "XML 전송 기준", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkSheetDisplayFlag", "작업표준서 표기여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Empcode", "담당부서", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remarks", "비고", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProcType", "계산유형(정밀도/측정값)", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PRECISION", "정밀도", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "XMLJudgeType", "XML판정유형", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "VerFlag", "버니어 사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            #region Grid MERGE
            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterOPCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterOPCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterOPCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["WorkCenterOPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterOPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterOPName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["ItemName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemName"].MergedCellStyle = MergedCellStyle.Always;
            #endregion Grid MERGE


            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "VerFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("WORKTYPE");  //작업구분
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "WorkType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("MeasureEquType");  //측정구분 (A:자동, M:수동)
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MeasureEquType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");  //작업표준서 표기여부 (Y/N)
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "CriticalApplyFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");  //작업표준서 표기여부 (Y/N)
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "StatsFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("XMLSendType");  //XMLJudgeType xml 전송기준 (J:판정,V:측정치,D:측정값.판정)
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "XMLSendType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");  //XMLJudgeType xml 전송기준 (J:판정,V:측정치,D:측정값.판정)
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "HIPISflag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");  //작업표준서 표기여부 (Y/N)
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "WorkSheetDisplayFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
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
    }
}
