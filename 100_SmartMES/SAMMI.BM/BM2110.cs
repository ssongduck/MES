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
using SAMMI.PopUp;
using SAMMI.PopManager;
using SAMMI.Common;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{
    public partial class BM2110 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        //비지니스 로직 객체 생성
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable DtChange = new DataTable();
        PopUp_Biz _biz = new PopUp_Biz();

        BizGridManagerEX gridManager;
        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        private string PlantCode = string.Empty;
        #endregion

        public BM2110()
        {
            InitializeComponent();

            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this.PlantCode.Equals("SK"))
                this.PlantCode = "SK1";
            else if (this.PlantCode.Equals("EC"))
                this.PlantCode = "SK2";
            
            if(!( this.PlantCode.Equals("SK1") || this.PlantCode.Equals("SK2") ))
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;

            gridManager = new BizGridManagerEX(grid1);

            GridInit();
        }

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
                string sLadleCode = SqlDBHelper.nvlString(txtLadleCode.Text.Trim());
                string sLadleName = SqlDBHelper.nvlString(txtLadleName.Text.Trim());
                string sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);
               
                base.DoInquire();

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@LadleCode", sLadleCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@LadleName", sLadleName, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                param[5] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                //rtnDtTemp = helper.FillTable("USP_BM2110_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM2110_S1_UNION", CommandType.StoredProcedure, param);

                if (param[4].Value.ToString() == "E")
                    throw new Exception(param[5].Value.ToString());

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

            UltraGridUtil.ActivationAllowEdit(this.grid1, "LadleCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "LadleName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
            //grid1.Rows[i + 1].Cells["PlantCode"].Value
            grid1.Rows[iRow].Cells["PlantCode"].Value = LoginInfo.PlantAuth.Equals("") ? "" : LoginInfo.PlantAuth; 
  

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
                            if (SqlDBHelper.nvlString(dr["PlantCode"]).Equals(string.Empty)
                                || SqlDBHelper.nvlString(dr["LadleCode"]).Equals(string.Empty)
                                || SqlDBHelper.nvlString(dr["LadleName"]).Equals(string.Empty))
                            {
                                ShowDialog("래들코드, 래들명은 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

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
                            // Validate 체크

                            if (SqlDBHelper.nvlString(dr["PlantCode"]).Equals(string.Empty)
                                || SqlDBHelper.nvlString(dr["LadleCode"]).Equals(string.Empty)
                                || SqlDBHelper.nvlString(dr["LadleName"]).Equals(string.Empty))
                            {
                                ShowDialog("래들코드, 래들명은 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

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

                            param = new SqlParameter[4];

                            param[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@LadleCode", SqlDBHelper.nvlString(drRow["LadleCode"]), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드

                            param[2] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[3] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM2110_D1", CommandType.StoredProcedure, param);
                            //helper.ExecuteNoneQuery("USP_BM2110_D1_UNION", CommandType.StoredProcedure, param);

                            if (param[2].Value.ToString() == "E") throw new Exception(param[3].Value.ToString());
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가 - 통합완료
                            param = new SqlParameter[7];

                            param[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@LadleCode", SqlDBHelper.nvlString(drRow["LadleCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@LadleName", SqlDBHelper.nvlString(drRow["LadleName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            param[5] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[6] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM2110_I1", CommandType.StoredProcedure, param);

                            if (param[5].Value.ToString() == "E") throw new Exception(param[6].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정 - 통합완료
                            param = new SqlParameter[7];

                            param[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@LadleCode", SqlDBHelper.nvlString(drRow["LadleCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@LadleName", SqlDBHelper.nvlString(drRow["LadleName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            param[5] = helper.CreateParameter("@pRetCode", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[6] = helper.CreateParameter("@pRetMessage", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            //helper.ExecuteNoneQuery("USP_BM2110_U1", CommandType.StoredProcedure, param);
                            helper.ExecuteNoneQuery("USP_BM2110_U1", CommandType.StoredProcedure, param);
                            if (param[5].Value.ToString() == "E") throw new Exception(param[6].Value.ToString());

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

        #region Grid 생성
        private void GridInit()
        {
            #region 그리드
            //_GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, true, null, null, null, null, null); 
            _GridUtil.InitColumnUltraGrid(grid1, "LadleCode", "래들코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LadleName", "래들명", true, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            DataTable rtnDtTemp2 = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp2, "CODE_ID", "CODE_NAME");
            rtnDtTemp2 = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp2, "CODE_ID", "CODE_NAME");
            //grid1.DisplayLayout.Bands[0].Columns["PlantCode"].Hidden = false;
            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);

            #endregion

        }
        #endregion

        private void BM2110_Load(object sender, EventArgs e)
        {
            
        }
    }
}
