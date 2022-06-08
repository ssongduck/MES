#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM1000
//   Form Name    : 작업자 마스터
//   Name Space   : SAMMI.BM
//   Created Date : 2012-03-19
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 작업라인(Workcenter) 관리 화면
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
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
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{
    public partial class BM1000 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        //비지니스 로직 객체 생성
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable DtChange = new DataTable();
        PopUp_Biz _biz = new PopUp_Biz();

        BizGridManagerEX gridManager;
        BizTextBoxManagerEX btbManager;
        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();
        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >
        public BM1000()
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

            gridManager = new BizGridManagerEX(grid1);

            GridInit();

        }

        private void BM1000_Load(object sender, EventArgs e)
        {
           // #region 그리드
           // _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
           // // InitColumnUltraGrid
           // // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
           // // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
           // // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern
            
           // _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "공장코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Default, true, false, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "ErrorCode", "불량코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Default, true, false, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "ErrorDesc", "불량명", true, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Default, true, true, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "ErrorClass", "불량구분", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Default, true, false, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "ErrorType", "불량구분(사용안함)", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Default, false, false, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", true, GridColDataType_emu.VarChar, 206, 255, Infragistics.Win.HAlign.Default, true, true, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Default, true, true, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Default, true, false, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Default, true, false, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            
           // _GridUtil.SetInitUltraGridBind(grid1);

           /////row number
           // grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
           // grid1.DisplayLayout.Override.RowSelectorWidth = 40;
           // grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
           // grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

           // #endregion

           // DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");     //사용여부
           // SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
           // SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
           // rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");     //사용여부
           // SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
           // SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
           // rtnDtTemp = _Common.GET_TBM0000_CODE("ERRORCLASS");     //불량 유형
           // SAMMI.Common.Common.FillComboboxMaster(this.cboErrorClass_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
           // SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ERRORCLASS", rtnDtTemp, "CODE_ID", "CODE_NAME");


           // btbManager = new BizTextBoxManagerEX();
           // btbManager.PopUpAdd(txtErrorCode, txtErrorName, "TBM1000", new object[] { cboErrorClass_H, cboErrorClass_H, "Y"});
        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            DtChange.Clear();
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sErrorCode = txtErrorCode.Text.Trim();
                string sErrorName = txtErrorName.Text.Trim();
                string sUseFlag = SqlDBHelper.gGetCode(this.cboUseFlag_H.Value);
                string sErrorClass = SqlDBHelper.gGetCode(this.cboErrorClass_H.Value);


                base.DoInquire();

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@ErrorType", sErrorClass, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@ErrorCode", sErrorCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@ErrorName", sErrorName, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@ErrorClass", sErrorClass, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
 
                rtnDtTemp = helper.FillTable("USP_BM1000_S1", CommandType.StoredProcedure, param);
                rtnDtTemp.AcceptChanges();
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;
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
        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            base.DoNew();

            int iRow = _GridUtil.AddRow(this.grid1, (DataTable)grid1.DataSource);

            UltraGridUtil.ActivationAllowEdit(this.grid1, "ErrorCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ErrorDesc", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ErrorClass", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Remark", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
            
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();

            this.grid1.DeleteRow();
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false, false);
            SqlParameter[] param = null;

            try
            {
                this.Focus();
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            // Validate 체크
                            if (LoginInfo.UserPlantCode == "" || SqlDBHelper.nvlString(dr["ErrorCode"]) == "" || SqlDBHelper.nvlString(dr["ErrorClass"]) == "")
                            {
                                ShowDialog("불량코드, 불량구분은 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            break;
                    }
                }

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
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[4];

                            param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@ErrorCode", SqlDBHelper.nvlString(drRow["ErrorCode"]), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            
                            param[2] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[3] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM1000_D1", CommandType.StoredProcedure, param);

                            if (param[2].Value.ToString() == "E") throw new Exception(param[3].Value.ToString());
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가
                            param = new SqlParameter[10];

                            param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@ErrorCode", SqlDBHelper.nvlString(drRow["ErrorCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@ErrorDesc", SqlDBHelper.nvlString(drRow["ErrorDesc"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@ErrorType", SqlDBHelper.nvlString(drRow["ErrorClass"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@ErrorClass", SqlDBHelper.nvlString(drRow["ErrorClass"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@Remark", SqlDBHelper.nvlString(drRow["Remark"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@pMaker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                         
                            param[8] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[9] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM1000_I1", CommandType.StoredProcedure, param);

                            if (param[8].Value.ToString() == "E") throw new Exception(param[9].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정
                           param = new SqlParameter[13];

                           param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@ErrorCode", SqlDBHelper.nvlString(drRow["ErrorCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@ErrorDesc", SqlDBHelper.nvlString(drRow["ErrorDesc"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@ErrorType", SqlDBHelper.nvlString(drRow["ErrorClass"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@ErrorClass", SqlDBHelper.nvlString(drRow["ErrorClass"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@Remark", SqlDBHelper.nvlString(drRow["Remark"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@cast_use", SqlDBHelper.nvlString(drRow["cast_use"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@mf_use", SqlDBHelper.nvlString(drRow["mf_use"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@ss_use", SqlDBHelper.nvlString(drRow["ss_use"]), SqlDbType.VarChar, ParameterDirection.Input);
                          
                            param[11] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[12] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM1000_U1", CommandType.StoredProcedure, param);

                            if (param[8].Value.ToString() == "E") throw new Exception(param[9].Value.ToString());

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

        private void GridInit()
        {
            #region 그리드
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorCode", "불량코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorDesc", "불량명", true, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorClass", "불량유형", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorType", "불량구분", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "cast_use", "주조사용", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "mf_use", "가공사용", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ss_use", "사상사용", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", true, GridColDataType_emu.VarChar, 200, 255, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);

            ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            #endregion

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "cast_use", rtnDtTemp, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "mf_use", rtnDtTemp, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ss_use", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");     //불량 유형
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("ERRORCLASS");     //불량 유형
            //SAMMI.Common.Common.FillComboboxMaster(this.cboErrorClass_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ERRORCLASS", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("ERRORTYPE");     //불량 구분
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ERRORType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            btbManager = new BizTextBoxManagerEX();
            btbManager.PopUpAdd(txtErrorCode, txtErrorName, "TBM1000", new object[] { cboErrorClass_H, cboErrorClass_H, "Y"});
        }
        #endregion

        #region < EVENT AREA >
        /// <summary>
        /// Form이 Close 되기전에 발생
        /// e.Cancel을 true로 설정 하면, Form이 close되지 않음
        /// 수정 내역이 있는지를 확인 후 저장여부를 물어보고 저장, 저장하지 않기, 또는 화면 닫기를 Cancel 함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {

        }

        #endregion

    }
}