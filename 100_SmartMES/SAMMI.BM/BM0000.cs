using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using SAMMI.Common;

using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;
// 커밋푸시 테스트용 주석
namespace SAMMI.BM
{
    public partial class BM0000 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <Member Area>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;
        private DataTable DtChange2 = null;

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        
        #endregion

        public BM0000()
        {
            InitializeComponent();
        }
        
        private void BM0000_Load(object sender, EventArgs e)
        {
            #region <Grid Setting>
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "MajorCode", "주코드", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MinorCode", "MinorCode", false, GridColDataType_emu.VarChar, 130, 30, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CodeName",  "코드명", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RelCode1", "기타1", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RelCode2", "기타2", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RelCode3", "기타3", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RelCode4", "기타4", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RelCode5", "비고", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MinorLen", "MinorLen", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DisplayNo", "DisplayNo", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SysFlag", "SysFlag", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 80, 1, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            _GridUtil.InitializeGrid(this.grid2, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid2, "MajorCode", "주코드", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "MinorCode", "부코드", false, GridColDataType_emu.VarChar, 130, 30, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "CodeName", "코드명", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "RelCode1", "기타1", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "RelCode2", "기타2", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "RelCode3", "기타3", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "RelCode4", "기타4", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "RelCode5", "비고", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "MinorLen", "MinorLen", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "DisplayNo", "DisplayNo", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 80, 1, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "MakeDate", "등록일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "EditDate", "수정일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            
            _GridUtil.SetInitUltraGridBind(grid2);
           
            DtChange2 = (DataTable)grid2.DataSource;
            #endregion

            #region <ComboBox Setting>
            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            #endregion
            //SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            //SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid2);

        }

        #region [ToolBar Area]
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];
            try
            {
                DtChange.Clear();

              //  base.DoInquire();

                string sMajorCode = txtMajorCode.Text.Trim(); ;
                string useFlag = cboUseFlag_H.Value.ToString() == "ALL" ? "" : cboUseFlag_H.Value.ToString();

                param[0] = helper.CreateParameter("@MajorCode", sMajorCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@UseFlag", useFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                rtnDtTemp = helper.FillTable("USP_BM0000_S1", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;

                param[0] = helper.CreateParameter("@MajorCode", sMajorCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@UseFlag", useFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                rtnDtTemp = helper.FillTable("USP_BM0000_S2", CommandType.StoredProcedure, param);

                grid2.DataSource = rtnDtTemp;
                grid2.DataBind();

                DtChange2 = rtnDtTemp;
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
            #region [신규 행 생성]
            try
            {
                base.DoNew();

                if (this.grid2.IsActivate)
                {
                    // this.grid2.InsertRow();
                    int iRow = _GridUtil.AddRow(this.grid2, DtChange2);

                 //   UltraGridUtil.ActivationAllowEdit(this.grid2, "MajorCode", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid2, "MinorCode", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid2, "CodeName", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid2, "UseFlag", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid2, "MakeDate", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid2, "Maker", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid2, "EditDate", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid2, "Editor", iRow);

                    grid2.Rows[iRow].Cells["MajorCode"].Value = grid1.ActiveRow.Cells["MajorCode"].Value;
                }
                else
                {   // this.grid1.InsertRow();

                    int iRow = _GridUtil.AddRow(this.grid1, DtChange);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "MajorCode", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "CodeName", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }

        public override void DoDelete()
        {
            #region [행삭제]
            base.DoDelete();

            if (this.grid2.IsActivate)
                this.grid2.DeleteRow();
            else
                this.grid1.DeleteRow();
            #endregion
        }

        public override void DoSave()
        {
            //base.DoSave();

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = null;

            try
            {
                this.Focus();
                

                if (grid1.IsActivate)
                {
                    UltraGridUtil.DataRowDelete(this.grid1);
                    this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                    foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                    {
                        switch (dr.RowState)
                        {
                            case DataRowState.Added:
                            case DataRowState.Modified:
                                // Validate 체크
                                
                                if (SqlDBHelper.nvlString(dr["MajorCode"]) == "")
                                {
                                    ShowDialog("C:I00014", Windows.Forms.DialogForm.DialogType.OK);

                                    CancelProcess = true;
                                    return;
                                }

                                break;
                        }
                    }

                    if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                        return;

                    base.DoSave();

                    foreach (DataRow drRow in DtChange.Rows)
                    {
                        switch (drRow.RowState)
                        {
                            case DataRowState.Deleted:
                                #region 삭제
                                drRow.RejectChanges();

                                param = new SqlParameter[5];

                                param[0] = helper.CreateParameter("MajorCode", drRow["MajorCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                                param[1] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[2] = helper.CreateParameter("MinorLen", drRow["MinorLen"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 관리항목
                                param[3] = helper.CreateParameter("Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 관리항목
                                param[4] = helper.CreateParameter("PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);  
                                helper.ExecuteNoneQuery("USP_BM0000_D1", CommandType.StoredProcedure, param);
                                #endregion
                                break;

                            case DataRowState.Added:
                                #region 추가
                                param = new SqlParameter[6];

                                param[0] = helper.CreateParameter("MajorCode", drRow["MajorCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                                param[1] = helper.CreateParameter("CodeName", drRow["CodeName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                                param[2] = helper.CreateParameter("SysFlag", drRow["SysFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                                param[3] = helper.CreateParameter("Maker", LoginInfo.UserID , SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                                param[4] = helper.CreateParameter("Editor", drRow["Editor"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[5] = helper.CreateParameter("PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서

                                helper.ExecuteNoneQuery("USP_BM0000_I1", CommandType.StoredProcedure, param);
                                #endregion
                                break;

                            case DataRowState.Modified:
                                #region 수정
                                param = new SqlParameter[7];
                                param[0] = helper.CreateParameter("MajorCode", drRow["MajorCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                param[1] = helper.CreateParameter("CodeName", drRow["CodeName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                param[2] = helper.CreateParameter("SysFlag", drRow["SysFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                param[3] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);

                                param[4] = helper.CreateParameter("MinorLen", drRow["MinorLen"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                param[5] = helper.CreateParameter("Editor", drRow["Editor"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                                param[6] = helper.CreateParameter("PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);

                                helper.ExecuteNoneQuery("USP_BM0000_U1", CommandType.StoredProcedure, param);
                           
                                #endregion
                                break;

                        }
                    }
                }
                else
                {
                    UltraGridUtil.DataRowDelete(this.grid2);
                    this.grid2.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                    foreach (DataRow drRow in DtChange2.Rows)
                    {
                        foreach (DataRow dr in ((DataTable)grid2.DataSource).Rows)
                        {
                            switch (dr.RowState)
                            {
                                case DataRowState.Added:
                                case DataRowState.Modified:
                                    // Validate 체크
                                    if (SqlDBHelper.nvlString(dr["MajorCode"]) == "")
                                    {
                                        ShowDialog("C:I00014", Windows.Forms.DialogForm.DialogType.OK);

                                        CancelProcess = true;
                                        return;
                                    }
                                    if (SqlDBHelper.nvlString(dr["MinorCode"]) == "")
                                    {
                                        ShowDialog("C:I00015", Windows.Forms.DialogForm.DialogType.OK);

                                        CancelProcess = true;
                                        return;
                                    }

                                    break;
                            }
                        }

                        if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                            return;

                        base.DoSave();

                        switch (drRow.RowState)
                        {
                            case DataRowState.Deleted:
                                #region 삭제
                                drRow.RejectChanges();

                                param = new SqlParameter[5];

                                param[0] = helper.CreateParameter("MajorCode", drRow["MajorCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                                param[1] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                                param[2] = helper.CreateParameter("MinorCode", drRow["MinorCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 관리항목
                                param[3] = helper.CreateParameter("Editor", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 관리항목
                                param[4] = helper.CreateParameter("PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                                helper.ExecuteNoneQuery("USP_BM0000_D2", CommandType.StoredProcedure, param);
                                #endregion
                                break;

                            case DataRowState.Added:
                                #region 추가
                                param = new SqlParameter[13];

                                param[0] = helper.CreateParameter("MajorCode", drRow["MajorCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                                param[1] = helper.CreateParameter("MinorCode", drRow["MinorCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                                param[2] = helper.CreateParameter("CodeName", drRow["CodeName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                                param[3] = helper.CreateParameter("RelCode1", drRow["RelCode1"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                                param[4] = helper.CreateParameter("RelCode2", drRow["RelCode2"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[5] = helper.CreateParameter("RelCode3", drRow["RelCode3"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[6] = helper.CreateParameter("RelCode4", drRow["RelCode4"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[7] = helper.CreateParameter("RelCode5", drRow["RelCode5"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[8] = helper.CreateParameter("DisplayNo", drRow["DisplayNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[9] = helper.CreateParameter("SysFlag", drRow["SysFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[10] = helper.CreateParameter("Maker",  SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[11] = helper.CreateParameter("Editor", drRow["Editor"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[12] = helper.CreateParameter("PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                                helper.ExecuteNoneQuery("USP_BM0000_I2", CommandType.StoredProcedure, param);
                                #endregion
                                break;

                            case DataRowState.Modified:
                                #region 수정
                                param = new SqlParameter[14];
                                
                                param[0] = helper.CreateParameter("MajorCode", drRow["MajorCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                                param[1] = helper.CreateParameter("MinorCode", drRow["MinorCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                                param[2] = helper.CreateParameter("CodeName", drRow["CodeName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                                param[3] = helper.CreateParameter("RelCode1", drRow["RelCode1"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                                param[4] = helper.CreateParameter("RelCode2", drRow["RelCode2"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[5] = helper.CreateParameter("RelCode3", drRow["RelCode3"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[6] = helper.CreateParameter("RelCode4", drRow["RelCode4"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[7] = helper.CreateParameter("RelCode5", drRow["RelCode5"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[8] = helper.CreateParameter("DisplayNo", drRow["DisplayNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[9] = helper.CreateParameter("SysFlag", drRow["SysFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[10] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[11] = helper.CreateParameter("Maker", drRow["Maker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[12] = helper.CreateParameter("Editor",  SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                                param[13] = helper.CreateParameter("PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                                helper.ExecuteNoneQuery("USP_BM0000_U2", CommandType.StoredProcedure, param);

                                #endregion
                                break;

                        }
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

        private void grid2_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            e.Row.Cells["MajorCode"].Value = this.grid1.ActiveRow.Cells["MajorCode"].Value;
        }

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
           

        }

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

        private void grid1_BeforeRowActivate(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
        }

       

        private void grid1_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //Grid2 조회
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                DtChange2.Clear();

              //  base.DoInquire();


                string sMajorCode = this.grid1.ActiveRow.Cells["MajorCode"].Value.ToString();
                string sUseflag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

                
                param[0] = helper.CreateParameter("@MajorCode", sMajorCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@UseFlag", sUseflag, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                rtnDtTemp = helper.FillTable("USP_BM0000_S2", CommandType.StoredProcedure, param);

                grid2.DataSource = rtnDtTemp;
                grid2.DataBind();

                DtChange2 = rtnDtTemp;
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

    }
}
