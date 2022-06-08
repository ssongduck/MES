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
using Infragistics.Win;
using System.IO;
#endregion

namespace SAMMI.BM
{
    public partial class MO0000 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        // private DataTable DtChange = null;

        private DataTable DtChange = null;
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string PlantCode = string.Empty;
        #endregion

        public MO0000()
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

            gridManager.PopUpAdd("ImageSeq", "ImageName", "TMO0000", new string [] {});

            GridInit();


        }
        
        #region [그리드 셋팅]
        private void GridInit()
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "DisplayIP", "현황판IP", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DisplayName", "현황판명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "AllFlag", "전체사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);

            _GridUtil.InitColumnUltraGrid(grid1, "RefreshTime", "갱신주기(초)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DisplayTime", "화면전환주기(초)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);

            _GridUtil.InitColumnUltraGrid(grid1, "ImageSeq", "메시지순번", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ImageName", "메시지명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display14", "메시지관리", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);

            _GridUtil.InitColumnUltraGrid(grid1, "Display1", "생산실적(주조)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display2", "생산실적(가공조립)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display3", "생산실적(ValveBody)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display4", "설비고장현황(주조)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display5", "설비고장현황(가공)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display6", "금형고장현황", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display7", "설비가동(주조)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display8", "설비가동(가공조립)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display9", "주요재고현황(소재창고)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display10", "주요재고현황(영업창고)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display11", "품질불량현황", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display12", "작업장별 작업인원", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Display13", "IT 모니터링(네트워크)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);

            
            
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            #endregion

            #region 콤보박스

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");  //현황판IP
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "AllFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");  //메시지 관리
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Display14", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion

        }
        #endregion

        #region 조회
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                base.DoInquire();

                param[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@DisplayIP", txtMonitorIP.Text, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@DisplayName", txtMonitorName.Text, SqlDbType.VarChar, ParameterDirection.Input);        
                //param[3] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(this.cboUseFlag_H.Value), SqlDbType.VarChar, ParameterDirection.Input); 

                //DataSet ds = helper.FillDataSet("USP_MO0000_S1", CommandType.StoredProcedure, param);
                DataTable rtnDtTemp = helper.FillTable("USP_MO0000_S1", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;
            }
            catch (Exception ex)
            {
                this.ShowDialog(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
            this.ClosePrgForm();
        }
        #endregion 조회

        #region 등록
        public override void DoNew()
        {
            base.DoNew();

            int iRow = _GridUtil.AddRow(this.grid1, DtChange);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "DisplayIP", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "DisplayName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "RefreshTime", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "DisplayTime", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display1", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display2", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display3", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display4", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display5", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display6", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display7", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display8", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display9", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display10", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display11", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display12", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display13", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Display14", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ImageSeq", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "AllFlag", iRow);

            this.grid1.Rows[iRow].Cells["RefreshTime"].Value = 15;
            this.grid1.Rows[iRow].Cells["DisplayTime"].Value = 30;
            this.grid1.Rows[iRow].Cells["Display1"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display2"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display3"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display4"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display5"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display6"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display7"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display8"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display9"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display10"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display11"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display12"].Value = 0;
            this.grid1.Rows[iRow].Cells["Display13"].Value = 0;
            this.grid1.Rows[iRow].Cells["ImageSeq"].Value = 0;
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
        }
        #endregion

        #region 삭제
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();

            this.grid1.DeleteRow();
        }
        #endregion

        #region 저장
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;

            try
            {
                this.Focus();

                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow drRow in DtChange.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[3];

                            param[0] = helper.CreateParameter("DisplayIP", drRow["DisplayIP"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // Plan No

                            param[1] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[2] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_MO0000_D1", CommandType.StoredProcedure, param);

                            if (param[1].Value.ToString() == "E") throw new Exception(param[2].Value.ToString());
                            #endregion
                            break;
                            
                        case DataRowState.Added:
                            #region 추가

                            param = new SqlParameter[23];
                            param[0] = helper.CreateParameter("DisplayIP", drRow["DisplayIP"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("DisplayName", drRow["DisplayName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("RefreshTime", Convert.ToInt32(drRow["RefreshTime"]), SqlDbType.Int, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("DisplayTime", Convert.ToInt32(drRow["DisplayTime"]), SqlDbType.Int, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("Display1", Convert.ToInt32(drRow["Display1"]), SqlDbType.Int, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("Display2", Convert.ToInt32(drRow["Display2"]), SqlDbType.Int, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("Display3", Convert.ToInt32(drRow["Display3"]), SqlDbType.Int, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("Display4", Convert.ToInt32(drRow["Display4"]), SqlDbType.Int, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("Display5", Convert.ToInt32(drRow["Display5"]), SqlDbType.Int, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("Display6", Convert.ToInt32(drRow["Display6"]), SqlDbType.Int, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("Display7", Convert.ToInt32(drRow["Display7"]), SqlDbType.Int, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("Display8", Convert.ToInt32(drRow["Display8"]), SqlDbType.Int, ParameterDirection.Input);
                            param[13] = helper.CreateParameter("Display9", Convert.ToInt32(drRow["Display9"]), SqlDbType.Int, ParameterDirection.Input);
                            param[14] = helper.CreateParameter("Display10", Convert.ToInt32(drRow["Display10"]), SqlDbType.Int, ParameterDirection.Input);
                            param[15] = helper.CreateParameter("Display11", Convert.ToInt32(drRow["Display11"]), SqlDbType.Int, ParameterDirection.Input);
                            param[16] = helper.CreateParameter("Display12", Convert.ToInt32(drRow["Display12"]), SqlDbType.Int, ParameterDirection.Input);
                            param[17] = helper.CreateParameter("Display13", Convert.ToInt32(drRow["Display13"]), SqlDbType.Int, ParameterDirection.Input);
                            param[18] = helper.CreateParameter("Display14", SqlDBHelper.nvlString(drRow["Display14"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[19] = helper.CreateParameter("ImageSeq", Convert.ToInt32(drRow["ImageSeq"]), SqlDbType.Int, ParameterDirection.Input);
                            param[20] = helper.CreateParameter("AllFlag", SqlDBHelper.nvlString(drRow["AllFlag"]), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[21] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[22] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_MO0000_I1", CommandType.StoredProcedure, param);

                            if (param[21].Value.ToString() == "E") throw new Exception(param[22].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:

                            #region 수정
                            
                            param = new SqlParameter[23];
                            param[0] = helper.CreateParameter("DisplayIP", drRow["DisplayIP"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("DisplayName", drRow["DisplayName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("RefreshTime", Convert.ToInt32(drRow["RefreshTime"]), SqlDbType.Int, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("DisplayTime", Convert.ToInt32(drRow["DisplayTime"]), SqlDbType.Int, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("Display1", Convert.ToInt32(drRow["Display1"]), SqlDbType.Int, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("Display2", Convert.ToInt32(drRow["Display2"]), SqlDbType.Int, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("Display3", Convert.ToInt32(drRow["Display3"]), SqlDbType.Int, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("Display4", Convert.ToInt32(drRow["Display4"]), SqlDbType.Int, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("Display5", Convert.ToInt32(drRow["Display5"]), SqlDbType.Int, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("Display6", Convert.ToInt32(drRow["Display6"]), SqlDbType.Int, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("Display7", Convert.ToInt32(drRow["Display7"]), SqlDbType.Int, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("Display8", Convert.ToInt32(drRow["Display8"]), SqlDbType.Int, ParameterDirection.Input);
                            param[13] = helper.CreateParameter("Display9", Convert.ToInt32(drRow["Display9"]), SqlDbType.Int, ParameterDirection.Input);
                            param[14] = helper.CreateParameter("Display10", Convert.ToInt32(drRow["Display10"]), SqlDbType.Int, ParameterDirection.Input);
                            param[15] = helper.CreateParameter("Display11", Convert.ToInt32(drRow["Display11"]), SqlDbType.Int, ParameterDirection.Input);
                            param[16] = helper.CreateParameter("Display12", Convert.ToInt32(drRow["Display12"]), SqlDbType.Int, ParameterDirection.Input);
                            param[17] = helper.CreateParameter("Display13", Convert.ToInt32(drRow["Display13"]), SqlDbType.Int, ParameterDirection.Input);
                            param[18] = helper.CreateParameter("Display14", SqlDBHelper.nvlString(drRow["Display14"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[19] = helper.CreateParameter("ImageSeq", Convert.ToInt32(drRow["ImageSeq"]), SqlDbType.Int, ParameterDirection.Input);
                            param[20] = helper.CreateParameter("AllFlag", SqlDBHelper.nvlString(drRow["AllFlag"]), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[21] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[22] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_MO0000_U1", CommandType.StoredProcedure, param);

                            if (param[21].Value.ToString() == "E") throw new Exception(param[22].Value.ToString());

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
    }
}
