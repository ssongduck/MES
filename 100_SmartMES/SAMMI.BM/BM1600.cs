#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM1600
//   Form Name    : 품목별 사용금형관리
//   Name Space   : SAMMI.BM
//   Created Date : 2012-02-21
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 
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
using SAMMI.Common;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{
    public partial class BM1600 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        // 변수나 Form에서 사용될 Class를 정의
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

        #region < CONSTRUCTOR >
        public BM1600()
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

            GridInit();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                //품목 팝업
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "", "" }
                         , new string[] { "", "" }, new object[] { });
                //금형 팝업 추가 필요.
                btbManager.PopUpAdd(txtMoldCode, txtMoldName, "TBM1600", new object[] { this.cboPlantCode_H, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }
            else
            {
                //품목 팝업
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "", "" }
                         , new string[] { "", "" }, new object[] { });
                //금형 팝업 추가 필요.
                btbManager.PopUpAdd(txtMoldCode, txtMoldName, "TBM1600", new object[] { LoginInfo.PlantAuth, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }

            gridManager.PopUpAdd("ItemCode", "ItemName", "TBM0100", new string[] { "PlantCode", "", "" });
            //금형 팝업
            // gridManager.PopUpAdd("MoldCode", "MoldName", "TBM1600", new string[] { "PlantCode", "", "" });

            #region <Combo Setting>
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("MOLDLOC");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MoldLoc", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("MOLDTYPE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MoldType", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("MOLDTYPE1");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MoldType1", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("MOLDTYPE2");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MoldType2", rtnDtTemp, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion
        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {

                DtChange.Clear();
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                string itemcode = SqlDBHelper.nvlString(this.txtItemCode.Text);
                string Moldcode = SqlDBHelper.nvlString(this.txtMoldCode.Text);
                string useflag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@ItemCode", itemcode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@MoldCode", Moldcode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@UseFlag", useflag, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_BM1600_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM1600_S1_UNION", CommandType.StoredProcedure, param);
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
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MoldCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MoldName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Itemname", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MoldType", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MoldType1", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MoldType2", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MoldLoc", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeCompany", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ModelName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "SerialNo", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LifeTime", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Contact", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MAWorker1", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MAWorker2", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "TechWorker", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "BuyDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "BuyCost", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Status", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "InspLastDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LimitDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Cavity", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Designshot", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Workshot", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Totshot", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Targetshot", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MoldUseCnt", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LastUseDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Remark", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
                this.Focus();

                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            // Validate 체크
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" || SqlDBHelper.nvlString(dr["MoldCode"]) == "")
                            {
                                ShowDialog("금형코드는 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

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

                foreach (DataRow drRow in DtChange.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:

                            #region [삭제]
                            drRow.RejectChanges();

                            param = new SqlParameter[2];

                            param[0] = helper.CreateParameter("PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("MoldCode", drRow["MoldCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            helper.ExecuteNoneQuery("USP_BM1600_D1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region [추가]
                            param = new SqlParameter[31];

                            param[0] = helper.CreateParameter("PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("MoldCode", drRow["MoldCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("Moldname", drRow["Moldname"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("MoldType", drRow["MoldType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("MoldType1", drRow["MoldType1"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("MoldType2", drRow["MoldType2"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("MoldLoc", drRow["MoldLoc"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("MakeCompany", drRow["MakeCompany"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("ModelName", drRow["ModelName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("SerialNo", drRow["SerialNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("LifeTime", drRow["LifeTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("Contact", drRow["Contact"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[13] = helper.CreateParameter("MAWorker1", drRow["MAWorker1"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[14] = helper.CreateParameter("MAWorker2", drRow["MAWorker2"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[15] = helper.CreateParameter("TechWorker", drRow["TechWorker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[16] = helper.CreateParameter("BuyDate", drRow["BuyDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[17] = helper.CreateParameter("BuyCost", drRow["BuyCost"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[18] = helper.CreateParameter("Status", drRow["Status"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[19] = helper.CreateParameter("InspLastDate", drRow["InspLastDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[20] = helper.CreateParameter("LimitDate", drRow["LimitDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[21] = helper.CreateParameter("Cavity", drRow["Cavity"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[22] = helper.CreateParameter("Designshot", drRow["Designshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[23] = helper.CreateParameter("Workshot", drRow["Workshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[24] = helper.CreateParameter("Totshot", drRow["Totshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[25] = helper.CreateParameter("Targetshot", drRow["Targetshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[26] = helper.CreateParameter("MoldUseCnt", drRow["MoldUseCnt"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[27] = helper.CreateParameter("LastUseDate", drRow["LastUseDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[28] = helper.CreateParameter("Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[29] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[30] = helper.CreateParameter("Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);


                            helper.ExecuteNoneQuery("USP_BM1600_I1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region [수정]
                            param = new SqlParameter[31];

                            param[0] = helper.CreateParameter("PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[1] = helper.CreateParameter("MoldCode", drRow["MoldCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[2] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[3] = helper.CreateParameter("Moldname", drRow["Moldname"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                            param[4] = helper.CreateParameter("MoldType", drRow["MoldType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[5] = helper.CreateParameter("MoldType1", drRow["MoldType1"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[6] = helper.CreateParameter("MoldType2", drRow["MoldType2"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[7] = helper.CreateParameter("MoldLoc", drRow["MoldLoc"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[8] = helper.CreateParameter("MakeCompany", drRow["MakeCompany"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[9] = helper.CreateParameter("ModelName", drRow["ModelName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                            param[10] = helper.CreateParameter("SerialNo", drRow["SerialNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[11] = helper.CreateParameter("LifeTime", drRow["LifeTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[12] = helper.CreateParameter("Contact", drRow["Contact"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[13] = helper.CreateParameter("MAWorker1", drRow["MAWorker1"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[14] = helper.CreateParameter("MAWorker2", drRow["MAWorker2"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[15] = helper.CreateParameter("TechWorker", drRow["TechWorker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                            param[16] = helper.CreateParameter("BuyDate", drRow["BuyDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[17] = helper.CreateParameter("BuyCost", drRow["BuyCost"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[18] = helper.CreateParameter("Status", drRow["Status"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[19] = helper.CreateParameter("InspLastDate", drRow["InspLastDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[20] = helper.CreateParameter("LimitDate", drRow["LimitDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[21] = helper.CreateParameter("Cavity", drRow["Cavity"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                            param[22] = helper.CreateParameter("Designshot", drRow["Designshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[23] = helper.CreateParameter("Workshot", drRow["Workshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[24] = helper.CreateParameter("Totshot", drRow["Totshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[25] = helper.CreateParameter("Targetshot", drRow["Targetshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[26] = helper.CreateParameter("MoldUseCnt", drRow["MoldUseCnt"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[27] = helper.CreateParameter("LastUseDate", drRow["LastUseDate"], SqlDbType.DateTime, ParameterDirection.Input);             // 작업장(공정)
                            param[28] = helper.CreateParameter("Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                            param[29] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[30] = helper.CreateParameter("Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드

                            helper.ExecuteNoneQuery("USP_BM1600_U1", CommandType.StoredProcedure, param);
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

        private void btnTemplateOk_Click(object sender, EventArgs e)
        {
            // This code was automatically generated by the RowEditTemplate Wizard
            // 
            // Close the template and save any pending changes.
            //   this.ultraGridRowEditTemplate1.Close(true);

        }

        private void btnTemplateCancel_Click(object sender, EventArgs e)
        {
            // This code was automatically generated by the RowEditTemplate Wizard
            // 
            // Close the template and discard any pending changes.
            // this.ultraGridRowEditTemplate1.Close(false);

        }

        private void grid1_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            e.Row.Cells["UseFlag"].Value = "Y";
        }
        
        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의

        private void GridInit()
        {
            //_GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldCode", "금형코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldName", "금형명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품목코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Itemname", "품목명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldType", "금형타입", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldType1", "분류1", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldType2", "분류2", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldLoc", "설치위치", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeCompany", "제조사", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ModelName", "모델명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SerialNo", "S/N", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LifeTime", "수명(Y)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Contact", "연락처", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAWorker1", "작업담당자1", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAWorker2", "작업담당자2", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "TechWorker", "장비기술 담당자", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BuyDate", "도입일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BuyCost", "도입가격", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Status", "상태", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspLastDate", "최종검사일자", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LimitDate", "유효일수", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Cavity", "Cavity", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Designshot", "설계사용수", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Workshot", "작업사용수", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Totshot", "합계사용수", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Targetshot", "목표쇼트", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldUseCnt", "금형사용횟수", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LastUseDate", "최종사용일자", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["MoldCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["MoldCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["MoldCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["MoldName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["MoldName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["MoldName"].MergedCellStyle = MergedCellStyle.Always;
        }

        #endregion

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "MoldName")
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
