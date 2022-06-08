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
    public partial class BM0630 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        private string PlantCode = string.Empty;
        #endregion

        #region <Constructor>
        public BM0630()
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

            //this.txtPlantCode.Text = "[" + LoginInfo.UserPlantCode + "] " + LoginInfo.UserPlantName;

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                     , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtWorkCenterOPCode, txtWorkCenterOPName, "TBM0610", new object[] { this.cboPlantCode_H, txtWorkCenterCode, "", "", "" }
                   , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { txtWorkCenterCode, txtWorkCenterName });
                btbManager.PopUpAdd(txtMachCode, txtMachName, "TBM0700", new object[] { cboPlantCode_H, "", "", "", "" });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                     , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtWorkCenterOPCode, txtWorkCenterOPName, "TBM0610", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, "", "", "" }
                   , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { txtWorkCenterCode, txtWorkCenterName });
                btbManager.PopUpAdd(txtMachCode, txtMachName, "TBM0700", new object[] { LoginInfo.PlantAuth, "", "", "", "" });
            }

            gridManager.PopUpAdd("OPCode", "OPName", "TBM0400", new string[] { "PlantCode", "" });
            gridManager.PopUpAdd("LineCode", "LineName", "TBM0500", new string[] { "PlantCode", "OPCode" });
            gridManager.PopUpAdd("WorkCenterCode", "WorkCenterName", "TBM0600", new string[] { "PlantCode", "OPCode", "LineCode", "" });
            gridManager.PopUpAdd("WorkCenterOPCode", "WorkCenterOPName", "TBM0610", new string[] { "PlantCode", "WorkCenterCode", "" });
            gridManager.PopUpAdd("WorkCenterLineCode", "WorkCenterLineName", "TBM5210", new string[] { "PlantCode", "WorkCenterCode", "" });
            gridManager.PopUpAdd("MachCode", "MachName", "TBM0700", new string[] { "PlantCode", "", "", "", "" });

            GridInit();
        }
        #endregion

        #region <TOOL BAR AREA >
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                DtChange.Clear();
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();
                string sWorkCenterOPCode = txtWorkCenterOPCode.Text.Trim();
                string sMachCode = txtMachCode.Text.Trim();
                string sUseFlag = SqlDBHelper.gGetCode(this.cboUseFlag_H.Value);

                base.DoInquire();

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@WorkCenterOPCode", sWorkCenterOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@MachCode", sMachCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM0630_S1_UNION", CommandType.StoredProcedure, param);
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
            //base.DoNew();
            //this.grid1.InsertRow();

            base.DoNew();

            int iRow = _GridUtil.AddRow(this.grid1, DtChange);

            UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "OPCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "OPName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "LineCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "LineName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterOPCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterOPName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MachCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MachName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterLineCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
            grid1.Rows[iRow].Cells["PlantCode"].Value = LoginInfo.PlantAuth.Equals("ALL") ? "" : LoginInfo.PlantAuth; 

        }

        public override void DoDelete()
        {
            base.DoDelete();

            this.grid1.DeleteRow();
        }

        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
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
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == ""
                                 || SqlDBHelper.nvlString(dr["WorkCenterCode"]) == ""
                                 || SqlDBHelper.nvlString(dr["WorkCenterOPCode"]) == "")
                                 //|| SqlDBHelper.nvlString(dr["MachCode"]) == "")
                            {
                                ShowDialog("작업장코드, 작업장Op코드는 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            if (!this.PlantCode.Equals("") &&
                                !SqlDBHelper.nvlString(this.cboPlantCode_H.Value).Equals(SqlDBHelper.nvlString(dr["PlantCode"])))
                            {
                                ShowDialog("[" + SqlDBHelper.nvlString(dr["PlantCode"]) + "] 등록권한이 없습니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }

                            break;
                        case DataRowState.Modified:
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == ""
                                || SqlDBHelper.nvlString(dr["WorkCenterCode"]) == ""
                                || SqlDBHelper.nvlString(dr["WorkCenterOPCode"]) == "")
                                //|| SqlDBHelper.nvlString(dr["MachCode"]) == "")
                            {
                                ShowDialog("작업장코드, 작업장Op코드는 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            if (!this.PlantCode.Equals("") &&
                                !SqlDBHelper.nvlString(this.cboPlantCode_H.Value).Equals(SqlDBHelper.nvlString(dr["PlantCode"])))
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
                            #region 삭제 - 통합완료
                            drRow.RejectChanges();

                            param = new SqlParameter[5];

                            sPlantCode = SqlDBHelper.nvlString(drRow["PlantCode"]);

                            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[2] = helper.CreateParameter("@WorkCenterOPCode", SqlDBHelper.nvlString(drRow["WorkCenterOPCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[3] = helper.CreateParameter("@MachCode", SqlDBHelper.nvlString(drRow["MachCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[4] = helper.CreateParameter("@WorkCenterLineCode", SqlDBHelper.nvlString(drRow["WorkCenterLineCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            helper.ExecuteNoneQuery("USP_BM0630_D1", CommandType.StoredProcedure, param);

                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가 - 통합완료
                            param = new SqlParameter[7];

                            sPlantCode = SqlDBHelper.nvlString(drRow["PlantCode"]);

                            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@WorkCenterOPCode", SqlDBHelper.nvlString(drRow["WorkCenterOPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@WorkCenterLineCode", SqlDBHelper.nvlString(drRow["WorkCenterLineCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@MachCode", SqlDBHelper.nvlString(drRow["MachCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            
                            helper.ExecuteNoneQuery("USP_BM0630_I1", CommandType.StoredProcedure, param);

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정 - 통합완료
                            param = new SqlParameter[8];

                            sPlantCode = SqlDBHelper.nvlString(drRow["PlantCode"]);

                            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]).Trim(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@WorkCenterOPCode", SqlDBHelper.nvlString(drRow["WorkCenterOPCode"]).Trim(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@MachCode", SqlDBHelper.nvlString(drRow["MachCode"]).Trim(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@WorkCenterLineCode", SqlDBHelper.nvlString(drRow["WorkCenterLineCode"]).Trim(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@WorkCenterOPCode_org", SqlDBHelper.nvlString(drRow["WorkCenterOPCode_org"]).Trim(), SqlDbType.VarChar, ParameterDirection.Input);

                            helper.ExecuteNoneQuery("USP_BM0630_U1", CommandType.StoredProcedure, param);

                            #endregion
                            break;
                    }
                }

                helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                CancelProcess = true;
                helper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion

        #region <Method Area>
        private void GridInit()
        {

            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", true, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", true, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "라인", true, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", true, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOPCode", "작업장 OP코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOPName", "작업장 OP명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachCode", "설비코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachName", "설비명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterLineCode", "가공라인코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterLineName", "가공라인", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOPCode_org", "가공라인코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
           
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["OPCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["OPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["LineCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["LineName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineName"].MergedCellStyle = MergedCellStyle.Always;

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

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            
            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
        }
        #endregion

        private void txtWorkCenterOPCode_KeyDown(object sender, KeyEventArgs e)
        {
           // this.txtWorkCenterOPCode.Text = string.Empty;
        }

        private void txtMachCode_KeyDown(object sender, KeyEventArgs e)
        {
            //this.txtMachCode.Text = string.Empty;
        }

        private void txtWorkCenterCode_KeyDown(object sender, KeyEventArgs e)
        {
            //this.txtWorkCenterCode.Text = string.Empty;
        }
    
    }
}
