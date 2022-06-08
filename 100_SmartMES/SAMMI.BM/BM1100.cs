#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : 
//   Form Name    : 
//   Name Space   : 
//   Created Date : 
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
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
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{

    public partial class BM1100 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        // 변수나 Form에서 사용될 Class를 정의
     

        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >
        public BM1100()
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
                //btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtStopCode, txtStopName, "TBM1100", new object[] { this.cboPlantCode_H, cboStopType_H, cboStopClass_H, cboUseFlag_H }
                    , new string[] { "", "", "", "" }, new object[] { });
            }
            else
            {
               // btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtStopCode, txtStopName, "TBM1100", new object[] { LoginInfo.PlantAuth, cboStopType_H, cboStopClass_H, cboUseFlag_H }
                    , new string[] { "", "", "", "" }, new object[] { });
            
            }
            //gridManager.PopUpAdd("OPCode", "OPName", "TBM0400", new string[] { "PlantCode", "" });
           // gridManager.PopUpAdd("StopCode", "StopName", "TBM1100", new string[] { "PlantCode", "OPCode" });

            #region <그리드>
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);

            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StopCode", "비가동코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StopDesc", "비가동명", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StopType", "비가동구분", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StopClass", "비가동유형", true, GridColDataType_emu.VarChar, 120, 255, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StopMH", "인당공수 집계구분", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StopMCH", "설비공수 집계구분", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StopCL", "프로세스 분기구분", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용유무", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StopSMS", "SMS 전송여부", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.SetInitUltraGridBind(grid1);

            ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            DtChange = (DataTable)grid1.DataSource;

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");     //불량 유형
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");     //불량 유형
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "StopSMS", rtnDtTemp, "CODE_ID", "CODE_NAME");
            #endregion
        }
        #endregion

        #region BM1100_Load
        private void BM1100_Load(object sender, EventArgs e)
        {

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("STOPTYPE");  //비가동구분
            //SAMMI.Common.Common.FillComboboxMaster(this.cboStopType_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "StopType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("STOPCLASS");     //비가동유형
            //SAMMI.Common.Common.FillComboboxMaster(this.cboStopClass_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "StopClass", rtnDtTemp, "CODE_ID", "CODE_NAME");

            #endregion

        }
        #endregion BM1100_Load

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                DtChange.Clear();

                base.DoInquire();

                string PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sStopClass = SqlDBHelper.nvlString(this.cboStopClass_H.Value);
                string sStopType = SqlDBHelper.nvlString(this.cboStopType_H.Value);
                string sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

                param[0] = helper.CreateParameter("@PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@OPCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@StopType", sStopType, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@StopClass", sStopClass, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM1100_S1", CommandType.StoredProcedure, param);
                
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
            try
            {
                base.DoNew();
                int iRow = _GridUtil.AddRow(this.grid1, DtChange);

                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StopCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StopDesc", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StopType", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StopClass", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Remark", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StopSMS", iRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           // this.grid1.InsertRow();

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
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;

            try
            {
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            // Validate 체크
                            if (SqlDBHelper.nvlString(dr["PlantCOde"]) == "" || SqlDBHelper.nvlString(dr["StopCode"]) == "")
                            {
                                ShowDialog("사업장, 비가동코드는 필수 입력항목 입니다", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            break;
                    }
                }


                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);
                
                foreach (DataRow drRow in ((DataTable)grid1.DataSource).Rows)
                {
                    lblUseFlag.Focus();
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[3];

                            param[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@OPCode", SqlDBHelper.nvlString(drRow["OPCode"]), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[2] = helper.CreateParameter("@StopCode", SqlDBHelper.nvlString(drRow["StopCode"]), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드

                            helper.ExecuteNoneQuery("USP_BM1100_D1", CommandType.StoredProcedure, param);
                            #endregion

                            break;
                        case DataRowState.Added:
                            #region 추가
                            param = new SqlParameter[13];


                            param[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@OPCode", SqlDBHelper.nvlString(drRow["OPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@StopCode", SqlDBHelper.nvlString(drRow["StopCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@StopDesc", SqlDBHelper.nvlString(drRow["StopDesc"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@StopType", SqlDBHelper.nvlString(drRow["StopType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@StopClass", SqlDBHelper.nvlString(drRow["StopClass"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@StopMH", SqlDBHelper.nvlString(drRow["StopMH"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@StopMCH", SqlDBHelper.nvlString(drRow["StopMCH"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@StopCL", SqlDBHelper.nvlString(drRow["StopCL"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@Remark", SqlDBHelper.nvlString(drRow["Remark"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@Maker", SqlDBHelper.nvlString(drRow["Maker"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("@StopSMS", SqlDBHelper.nvlString(drRow["StopSMS"]), SqlDbType.VarChar, ParameterDirection.Input);
                            
                            helper.ExecuteNoneQuery("USP_BM1100_I1", CommandType.StoredProcedure, param);

                            //if (param[15].Value.ToString() == "E") 
                            //    throw new Exception(param[16].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정
                            param = new SqlParameter[13];

                            param[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@OPCode", SqlDBHelper.nvlString(drRow["OPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@StopCode", SqlDBHelper.nvlString(drRow["StopCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@StopDesc", SqlDBHelper.nvlString(drRow["StopDesc"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@StopType", SqlDBHelper.nvlString(drRow["StopType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@StopClass", SqlDBHelper.nvlString(drRow["StopClass"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@StopMH", SqlDBHelper.nvlString(drRow["StopMH"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@StopMCH", SqlDBHelper.nvlString(drRow["StopMCH"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@StopCL", SqlDBHelper.nvlString(drRow["StopCL"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@Remark", SqlDBHelper.nvlString(drRow["Remark"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@Editor", SqlDBHelper.nvlString(drRow["Editor"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("@StopSMS", SqlDBHelper.nvlString(drRow["StopSMS"]), SqlDbType.VarChar, ParameterDirection.Input);
                          
                            helper.ExecuteNoneQuery("USP_BM1100_U1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                    }
                }
                helper.Transaction.Commit();
             
            }
            catch (Exception ex)
            {
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

        #region < EVENT AREA >


        void Adapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            if (e.Row.RowState == DataRowState.Modified)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                return;
            }

            if (e.Row.RowState == DataRowState.Added)
            {
                e.Command.Parameters["@Maker"].Value = this.WorkerID;
                return;
            }
        }

        void Adapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Errors == null) return;

            switch (((SqlException)e.Errors).Number)
            {
                // 중복
                case 2627:
                    e.Row.RowError = "비가동 정보가 있습니다.";
                    throw (new SException("S00099", e.Errors));
                default:
                    break;
            }
        }

        #endregion


        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        #endregion
    }
}
