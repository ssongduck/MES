#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM5020
//   Form Name    : 모니터링 품목별 재고관리
//   Name Space   : SAMMI.BM
//   Created Date : 2013-08-08
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
using SAMMI.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{
    public partial class BM5020 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        public BM5020()
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

            //품목 팝업
            //btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.UserPlantCode, "", "" }
            //         , new string[] { "", "" }, new object[] { });

            //gridManager.PopUpAdd("ItemCode", "ItemName", "TBM0100", new string[] { "PlantCode", "", "" });

            #region <Combo Setting>
//            rtnDtTemp = _Common.GET_TBM0000_CODE("MONITERINGDIV");

            //SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
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
            SqlParameter[] param = new SqlParameter[2];

            try
            {

                DtChange.Clear();
                base.DoInquire();

                string division = SqlDBHelper.nvlString(this.cboDivision_H.Value); //Todo : 콤보박스 이름 변경
                
                param[0] = helper.CreateParameter("@Division", division, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);
  
                rtnDtTemp = helper.FillTable("USP_BM5020_S1N", CommandType.StoredProcedure, param);
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
            try
            {
                base.DoNew();

                int iRow = _GridUtil.AddRow(this.grid1, DtChange);

                //UltraGridUtil.ActivationAllowEdit(this.grid1, "Division", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "MaxStockQty", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "MinStockQty", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "sort", iRow);
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
                            if (SqlDBHelper.nvlString(dr["Division"]) == "" || SqlDBHelper.nvlString(dr["IPADDRESS"]) == "")
                            {
                                ShowDialog("IP와  구분은 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

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

                            param[0] = helper.CreateParameter("IPADDRESS", drRow["IPADDRESS_O"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 구분
                            param[1] = helper.CreateParameter("PlantCode", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PlantCode"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);
                            helper.ExecuteNoneQuery("USP_BM5020_D1N", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region [추가]
                            param = new SqlParameter[8];

                            param[0] = helper.CreateParameter("IPADDRESS", drRow["IPADDRESS"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 구분
                            param[1] = helper.CreateParameter("WORKCENTERCODE", drRow["WORKCENTERCODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 품번
                            param[2] = helper.CreateParameter("Division", drRow["Division"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 최대수량
                            param[3] = helper.CreateParameter("LocX", drRow["LocX"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 최소수량
                            param[4] = helper.CreateParameter("LocY", drRow["LocY"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 최소수량
                            param[5] = helper.CreateParameter("UseYN", drRow["UseYN"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            param[6] = helper.CreateParameter("Maker",  SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[7] = helper.CreateParameter("PlantCode", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PlantCode"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);
                            helper.ExecuteNoneQuery("USP_BM5020_I1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region [수정]
                            param = new SqlParameter[9];

                            param[0] = helper.CreateParameter("IPADDRESS", drRow["IPADDRESS"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 구분
                            param[1] = helper.CreateParameter("WORKCENTERCODE", drRow["WORKCENTERCODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 품번
                            param[2] = helper.CreateParameter("Division", drRow["Division"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 최대수량
                            param[3] = helper.CreateParameter("LocX", drRow["LocX"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 최소수량
                            param[4] = helper.CreateParameter("LocY", drRow["LocY"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 최소수량
                            param[5] = helper.CreateParameter("UseYN", drRow["UseYN"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            param[6] = helper.CreateParameter("Maker",  SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[7] = helper.CreateParameter("IPADDRESS_O", drRow["IPADDRESS_O"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 구분
                            param[8] = helper.CreateParameter("PlantCode", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PlantCode"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);
                            helper.ExecuteNoneQuery("USP_BM5020_U1N", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                    }
                }
                //helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
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
            //e.Row.Cells["UseFlag"].Value = "Y";
        }
        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의

        private void GridInit()
        {
            //_GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IPADDRESS", "IP", false, GridColDataType_emu.VarChar, 100, 20, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IPADDRESS_O", "IP_O", false, GridColDataType_emu.VarChar, 100, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "항목명", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Division", "구분", false, GridColDataType_emu.VarChar, 250, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LocX", "X좌표", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LocY", "Y좌표", false, GridColDataType_emu.Integer, 100, 10, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseYN", "사용여부", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            rtnDtTemp = _Common.GET_TBM0000_CODE("MONITERINGDIV");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Division", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseYN", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
        }

        #endregion

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }
    }
}
