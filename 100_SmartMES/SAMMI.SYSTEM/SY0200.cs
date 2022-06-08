
#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : SY0200
//   Form Name      : (통합)사용자관리
//   Name Space     : SAMMI.SY
//   Created Date   : 2022.05.03
//   Made By        : 정용석
//   Description    : 삼기, 삼기ev 사용자 통합관리
// *---------------------------------------------------------------------------------------------*
#endregion

using Infragistics.Win.UltraWinTree;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;

namespace SAMMI.SY
{
    public partial class SY0200 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet();
        DataTable rtnDtTemp = new DataTable();

        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        DataTable DtChange = null;
        DataTable _DtTemp = new DataTable();

        public SY0200()
        {
            InitializeComponent();
        }

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {

            base.DoInquire();

            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                DtChange.Clear();
                base.DoInquire();

                string sWorkerID = txtWorkerID_H.Text;
                string sWorkerName = txtWorkerName_H.Text;
                string sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.SelectedValue);

                param[0] = helper.CreateParameter("@AS_WorkerID", sWorkerID, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@AS_WorkerName", sWorkerName, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@AS_UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_SY0200_S1_New", CommandType.StoredProcedure, param);

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
            try
            {
                base.DoNew();
                int iRow = _GridUtil.AddRow(this.grid1, DtChange);

                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkerID", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkerName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Pwd", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "GRPID", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);

                this.grid1.ActiveRow.Cells["UseFlag"].Value = "Y";
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
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
                            if (SqlDBHelper.nvlString(dr["WorkerID"]) == "" && SqlDBHelper.nvlString(dr["EV_WorkerID"]) == "")
                            {
                                ShowDialog("사용자ID 필수 입력항목 입니다.", Windows.Forms.DialogForm.DialogType.OK);
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
                            drRow.RejectChanges();
                            param = new SqlParameter[2];
                            param[0] = helper.CreateParameter("@AS_WorkerID", drRow["WorkerID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@AS_EVWokerID", drRow["EV_WorkerID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            helper.ExecuteNoneQuery("USP_SY0200_D3", CommandType.StoredProcedure, param);
                            break;
                        case DataRowState.Added:
                            break;
                        case DataRowState.Modified:
                            param = new SqlParameter[5];
                            param[0] = helper.CreateParameter("@AS_UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@AS_WorkerID", drRow["WorkerID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@AS_EVWorkerID", drRow["EV_WorkerId"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@AS_PlantAuth", drRow["PlantAuth"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@AS_EVPlantAuth", drRow["EV_PlantAuth"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            helper.ExecuteNoneQuery("USP_SY0200_U3", CommandType.StoredProcedure, param);
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
        #endregion

        private void SY0200_Load(object sender, EventArgs e)
        {
            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

            GridInit();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {

        }

        #region <METHOD AREA>
        private void GridInit()
        {
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "OverLap",       "겸임",     false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center,  true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag",       "사용",     false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center,  true, true,  null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerID",      "계정",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left,   true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerName",    "계정명",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode",     "공장",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EmpNo",         "사번",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Pwd",           "비밀번호", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left,   true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DeptCode",      "부서",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DeptName",      "부서명",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left,   true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantAuth",     "권한",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true,  null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EV_WorkerID",   "계정",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left,   true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EV_WorkerName", "계정명",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EV_PlantCode",  "공장",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EV_EmpNo",      "사번",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EV_Pwd",        "비밀번호", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left,   true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EV_DeptCode",   "부서",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EV_DeptName",   "부서명",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left,   true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EV_PlantAuth",  "권한",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true,  null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker",         "등록자",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate",      "등록일",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate",      "수정일",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            string[] sHeaderColumn = { "OverLap"       , 
                                       "UseFlag"       , 
                                       "WorkerID"      ,
                                       "WorkerName"    ,
                                       "PlantCode"     , 
                                       "EmpNo"         ,
                                       "Pwd"           , 
                                       "DeptCode"      ,  
                                       "DeptName"      ,  
                                       "PlantAuth"     ,
                                       "EV_WorkerID"   , 
                                       "EV_WorkerName" , 
                                       "EV_PlantCode"  ,  
                                       "EV_EmpNo"      ,  
                                       "EV_Pwd"        , 
                                       "EV_DeptCode"   ,  
                                       "EV_DeptName"   , 
                                       "EV_PlantAuth"  ,  
                                       "Maker"         ,  
                                       "MakeDate"      ,  
                                       "EditDate"      };

            _GridUtil.GridHeaderMerge(grid1, "SK", "삼기", new string[] { "WorkerID"   , 
                                                                          "WorkerName" , 
                                                                          "PlantCode"  , 
                                                                          "EmpNo"      , 
                                                                          "Pwd"        , 
                                                                          "DeptCode"   , 
                                                                          "DeptName"   , 
                                                                          "PlantAuth"  }, sHeaderColumn);

            _GridUtil.GridHeaderMerge(grid1, "EV", "삼기이브이", new string[] { "EV_WorkerID"   , 
                                                                               "EV_WorkerName" , 
                                                                               "EV_PlantCode"  , 
                                                                               "EV_EmpNo"      , 
                                                                               "Ev_Pwd"        , 
                                                                               "EV_DeptCode"   , 
                                                                               "EV_DeptName"   , 
                                                                               "EV_PlantAuth"  }, sHeaderColumn);

            _GridUtil.GridHeaderMergeVertical(grid1, sHeaderColumn, 0, 1);
            _GridUtil.GridHeaderMergeVertical(grid1, sHeaderColumn, 18, 20);

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PlantAuth");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantAuth", rtnDtTemp, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "EV_PlantAuth", rtnDtTemp, "CODE_ID", "CODE_NAME");
        }
        #endregion

    }
}



//////////using Infragistics.Win.UltraWinTree;
//////////using Microsoft.Practices.EnterpriseLibrary.Common;
//////////using Microsoft.Practices.EnterpriseLibrary.Data;
//////////using SAMMI.Common;
//////////using System;
//////////using System.Collections.Generic;
//////////using System.ComponentModel;
//////////using System.Data;
//////////using System.Data.SqlClient;
//////////using System.Drawing;
//////////using System.Text;
//////////using System.Windows.Forms;
//////////using Infragistics.Win.UltraWinGrid;   

//////////namespace SAMMI.SY
//////////{
//////////    public partial class SY0200 : SAMMI.Windows.Forms.BaseMDIChildForm
//////////    {
//////////        #region <MEMBER AREA>
//////////        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
//////////        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
//////////        //그리드 객체 생성
//////////        UltraGridUtil _GridUtil = new UltraGridUtil();
//////////        Common.Common _Common = new Common.Common();

//////////        private DataTable DtChange = null;

//////////        //임시로 사용할 데이터테이블 생성
//////////        DataTable _DtTemp = new DataTable();
//////////        #endregion

//////////        #region < CONSTRUCTOR >
//////////        public SY0200()
//////////        {
//////////            InitializeComponent();

//////////            //GridInit();
//////////        }
//////////        #endregion

//////////        #region <TOOL BAR AREA >
//////////        /// <summary>
//////////        /// ToolBar의 조회 버튼 클릭
//////////        /// </summary>
//////////        public override void DoInquire()
//////////        {
         
//////////            base.DoInquire();

//////////            SqlDBHelper helper = new SqlDBHelper(false);
//////////            SqlParameter[] param = new SqlParameter[4];

//////////            try
//////////            {
//////////                DtChange.Clear();
//////////                base.DoInquire();
//////////                string sPlantCode = LoginInfo.PlantAuth; //(this.PlantCode == "") ? true : false, false
//////////                string sWorkerID = txtWorkerID_H.Text;
//////////                string sWorkerName = txtWorkerName_H.Text;
//////////                string sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.SelectedValue);
                
//////////                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
//////////                param[1] = helper.CreateParameter("@WorkerID", sWorkerID, SqlDbType.VarChar, ParameterDirection.Input);
//////////                param[2] = helper.CreateParameter("@WorkerName", sWorkerName, SqlDbType.VarChar, ParameterDirection.Input);
//////////                param[3] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                
//////////                rtnDtTemp = helper.FillTable("USP_SY0200_S2", CommandType.StoredProcedure, param);

//////////                grid1.DataSource = rtnDtTemp;
//////////                grid1.DataBind();

//////////                DtChange = rtnDtTemp;
//////////            }
//////////            catch (Exception ex)
//////////            {
//////////                MessageBox.Show(ex.ToString());
//////////            }
//////////            finally
//////////            {
//////////                if (helper._sConn != null) { helper._sConn.Close(); }
//////////                if (param != null) { param = null; }
//////////            }
//////////        }

        

//////////        /// <summary>
//////////        /// ToolBar의 신규 버튼 클릭
//////////        /// </summary>
//////////        public override void DoNew()
//////////        {
//////////            base.DoNew();
//////////            try
//////////            {
//////////                base.DoNew();
//////////                int iRow = _GridUtil.AddRow(this.grid1, DtChange);

//////////                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkerID", iRow);
//////////                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkerName", iRow);
//////////                UltraGridUtil.ActivationAllowEdit(this.grid1, "Pwd", iRow);
//////////                UltraGridUtil.ActivationAllowEdit(this.grid1, "GRPID", iRow);
//////////                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
//////////                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
//////////                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
//////////                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
//////////                UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);

//////////                this.grid1.ActiveRow.Cells["UseFlag"].Value = "Y";
//////////            }
//////////            catch (Exception ex)
//////////            {
//////////                // MessageBox.Show(ex.ToString());
//////////            }

//////////        }
//////////        /// <summary>
//////////        /// ToolBar의 삭제 버튼 Click
//////////        /// </summary>
//////////        public override void DoDelete()
//////////        {
//////////            base.DoDelete();

//////////            this.grid1.DeleteRow();
//////////        }

//////////        /// <summary>
//////////        /// ToolBar의 저장 버튼 Click
//////////        /// </summary>
//////////        public override void DoSave()
//////////        {
//////////            SqlDBHelper helper = new SqlDBHelper(false);
//////////            SqlParameter[] param = null;

//////////            try
//////////            {
//////////                this.Focus();

//////////                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
//////////                {
//////////                    switch (dr.RowState)
//////////                    {
//////////                        case DataRowState.Added:
//////////                        case DataRowState.Modified:
//////////                            // Validate 체크
//////////                            if (SqlDBHelper.nvlString(dr["WorkerID"]) == "" || SqlDBHelper.nvlString(dr["WorkerName"]) == "")
//////////                            {
//////////                                ShowDialog("사용자ID 와 사용자명은 필수입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

//////////                                CancelProcess = true;
//////////                                return;
//////////                            }

//////////                            break;
//////////                    }
//////////                }

//////////                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
//////////                    return;

//////////                base.DoSave();

//////////                UltraGridUtil.DataRowDelete(this.grid1);
//////////                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

//////////                foreach (DataRow drRow in DtChange.Rows)
//////////                {
//////////                    switch (drRow.RowState)
//////////                    {
//////////                        case DataRowState.Deleted:

//////////                            #region [삭제]
//////////                            drRow.RejectChanges();

//////////                            param = new SqlParameter[2];

//////////                            param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
//////////                            param[1] = helper.CreateParameter("@WorkerID", drRow["WorkerID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드

//////////                            helper.ExecuteNoneQuery("USP_SY0200_D1", CommandType.StoredProcedure, param);
//////////                            #endregion

//////////                            break;
//////////                        case DataRowState.Added:
//////////                            #region [추가]
//////////                            param = new SqlParameter[8];

//////////                            param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
//////////                            param[1] = helper.CreateParameter("@WorkerID", drRow["WorkerID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
//////////                            param[2] = helper.CreateParameter("@WorkerName", drRow["WorkerName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
//////////                            param[3] = helper.CreateParameter("@Pwd", drRow["Pwd"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
//////////                            param[4] = helper.CreateParameter("@GRPID", drRow["GRPID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
//////////                            param[5] = helper.CreateParameter("@Maker", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
//////////                            param[6] = helper.CreateParameter("@UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
//////////                            param[7] = helper.CreateParameter("@PlantAuth", drRow["PlantAuth"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
//////////                            helper.ExecuteNoneQuery("USP_SY0200_I1N", CommandType.StoredProcedure, param);
//////////                            #endregion
//////////                            break;
//////////                        case DataRowState.Modified:
//////////                            #region [수정]
//////////                            param = new SqlParameter[8];

//////////                            param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);                  // 공장코드
//////////                            param[1] = helper.CreateParameter("@WorkerID", drRow["WorkerID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
//////////                            param[2] = helper.CreateParameter("@WorkerName", drRow["WorkerName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
//////////                            param[3] = helper.CreateParameter("@Pwd", drRow["Pwd"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);                    // 작업장(공정)
//////////                            param[4] = helper.CreateParameter("@GRPID", drRow["GRPID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
//////////                            param[5] = helper.CreateParameter("@Editor", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
//////////                            param[6] = helper.CreateParameter("@UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
//////////                            param[7] = helper.CreateParameter("@PlantAuth", drRow["PlantAuth"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
//////////                            helper.ExecuteNoneQuery("USP_SY0200_U1N", CommandType.StoredProcedure, param);
//////////                            #endregion
//////////                            break;
//////////                    }
//////////                }
//////////                helper.Transaction.Commit();
//////////            }
//////////            catch (Exception ex)
//////////            {
//////////                helper.Transaction.Rollback();
//////////                MessageBox.Show(ex.ToString());
//////////            }
//////////            finally
//////////            {
//////////                if (helper._sConn != null) { helper._sConn.Close(); }
//////////                if (param != null) { param = null; }
//////////            }
//////////        }
//////////        #endregion

//////////        #region < EVENT AREA >
//////////        #endregion

//////////        private void SY0200_Load(object sender, EventArgs e)
//////////        {
//////////            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);

//////////            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
//////////            SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

//////////            GridInit();
//////////            rtnDtTemp = _Common.GET_TBM0000_CODE("PlantAuth");  //사업장
//////////            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantAuth", rtnDtTemp, "CODE_ID", "CODE_NAME");
//////////            rtnDtTemp = _Common.GET_TBM0000_CODE("PlantCode");  //사업장
//////////            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
//////////        }

//////////        private void btnCopy_Click(object sender, EventArgs e)
//////////        {

//////////        }

//////////        #region <METHOD AREA>
//////////        private void GridInit()
//////////        {
//////////            //_GridUtil.InitializeGrid(this.grid1);
//////////            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode",  "사업장",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "WorkerID",   "사용자ID",   false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "WorkerName", "사용자명",   false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "Pwd",        "패스워드",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "GRPID",      "그룹ID",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "PlantAuth",  "사업장권한", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "SABUN",      "사번",       false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag",    "사용여부",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate",   "등록일자",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "Maker",      "등록자",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "EditDate",   "수정일자",   false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
//////////            _GridUtil.InitColumnUltraGrid(grid1, "Editor",     "수정자",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            
//////////            _GridUtil.SetInitUltraGridBind(grid1);
//////////            DtChange = (DataTable)grid1.DataSource;
//////////            ///row number
//////////            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
//////////            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
//////////            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
//////////            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

//////////            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
//////////            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
//////////        }
//////////        #endregion

//////////    }
//////////}
