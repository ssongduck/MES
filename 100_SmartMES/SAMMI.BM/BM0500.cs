#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM0500
//   Form Name    : 라인코드 마스터
//   Name Space   : SAMMI.BM
//   Created Date : 2012-03-15
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 기준정보 ( 라인 코드 마스터 ) 정보 관리 폼
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;   
#endregion

namespace SAMMI.BM
{
    /// <summary>
    /// 라인코드 마스터
    /// </summary>
    public partial class BM0500 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        #region < CONSTRUCTOR >
        public BM0500()
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
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { this.cboPlantCode_H, txtOPCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
            }            
            else
            {

                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { LoginInfo.PlantAuth, txtOPCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
            }
            gridManager.PopUpAdd("OPCode", "OPName", "TBM0400", new string[] { "PlantCode", "" });

            //gridManager.PopUpAdd("LineCode", "LineName", "TBM0500", new string[] { "PlantCode", "" });

            GridInit();
            //this.db = DatabaseFactory.CreateDatabase();
            //conn = (SqlConnection)this.db.CreateConnection();
            //this.daTable1.Connection = conn;
            //this.daTable1.Adapter.RowUpdating += new SqlRowUpdatingEventHandler(Adapter_RowUpdating);
            //this.daTable1.Adapter.RowUpdated += new SqlRowUpdatedEventHandler(Adapter_RowUpdated);
        }
        #endregion

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
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sOpCode = txtOPCode.Text.Trim();
                string sUseFlag = SqlDBHelper.nvlString(cboUseFlag_H.Value);
                string sLineCode = txtLineCode.Text.Trim();
                string sLineName = this.txtLineName.Text;

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@OPCode", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@LineName", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                //rtnDtTemp = helper.FillTable("USP_BM0500_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0500_S1_UNION", CommandType.StoredProcedure, param);
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
            //this.daTable1.Fill(ds.dTable1, sPlantCode, sLineCode, txtLineName.Text, sUseFlag);
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
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LineCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LineName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlagNM", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkerNM", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "EditorNM", iRow);
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
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" || SqlDBHelper.nvlString(dr["OPCode"]) == "" || SqlDBHelper.nvlString(dr["LineCode"]) == "")
                            {
                                ShowDialog("공정코드, 라인코드는 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);

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

                            param = new SqlParameter[3];

                            param[0] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[2] = helper.CreateParameter("LineCode", drRow["LineCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    
                            helper.ExecuteNoneQuery("USP_BM0500_D1", CommandType.StoredProcedure, param);
                            #endregion

                            break;
                        case DataRowState.Added:
                            #region [추가]
                            param = new SqlParameter[6];

                            param[0] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[1] = helper.CreateParameter("OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[2] = helper.CreateParameter("LineCode", drRow["LineCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[3] = helper.CreateParameter("LineName", drRow["LineName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                            param[4] = helper.CreateParameter("Maker", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[5] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서

                            helper.ExecuteNoneQuery("USP_BM0500_I1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region [수정]
                            param = new SqlParameter[6];

                            param[0] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[1] = helper.CreateParameter("OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[2] = helper.CreateParameter("LineCode", drRow["LineCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[3] = helper.CreateParameter("LineName", drRow["LineName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                            param[4] = helper.CreateParameter("Editor", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[5] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    
                            
                            helper.ExecuteNoneQuery("USP_BM0500_U1", CommandType.StoredProcedure, param);
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

        

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        private void GridInit()
        {
            //_GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "라인코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자ID", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerNM", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자ID", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditorNM", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

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

            //grid1.Columns["OPCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["OPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPName"].MergedCellStyle = MergedCellStyle.Always;
        
        }
        #endregion

        private void BM0500_Load(object sender, EventArgs e)
        {
            #region [ComboBox Setting]
            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion
        }
    }
}
