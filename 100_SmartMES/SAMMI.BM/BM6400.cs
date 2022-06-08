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
    public partial class BM6400 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        //비지니스 로직 객체 생성
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        //BizGridManagerEX gridManager;

        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        DataTable DtChange = new DataTable();

        private string PlantCode = string.Empty;
        #endregion

        public BM6400()
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

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(sTxtWorkCenterCode, sTxtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                       , new string[] { "", "" }, new object[] { });

                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(sTxtWorkCenterCode, sTxtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                       , new string[] { "", "" }, new object[] { });

                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
            }

            GridInit();
        }

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
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sWorkCenterCode = sTxtWorkCenterCode.Text.Trim();
                string sInspCode = sTxtInspCode.Text.Trim();
                string sUseFlag = SqlDBHelper.gGetCode(this.sCboUseFlag.Value);

                base.DoInquire();

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@InspCode", sInspCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
               
                rtnDtTemp = helper.FillTable("USP_BM7700_S1_UNION", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;
                //_Common.Grid_Column_Width(this.grid1); //grid 정리용
                this.txtWorkCenterCode.Text = string.Empty;
                this.txtWorkCenterName.Text = string.Empty;
                this.txtInspCode.Text = string.Empty;
                this.txtSpec.Text = string.Empty;
                this.txtMaker.Text = string.Empty;
                this.cboUseFlag.Value = string.Empty;
                this.txtInspName.Text = string.Empty;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }

                //txtInspName.Appearance.BackColor = System.Drawing.Color.White;
                //txtSpec.Appearance.BackColor = System.Drawing.Color.White;

                txtReadOnly(true);
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

                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "InspCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "InspName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Spec", iRow);
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
            finally
            {
                txtReadOnly(false);
                //txtInspName.Appearance.BackColor = System.Drawing.Color.White;
                //txtSpec.Appearance.BackColor = System.Drawing.Color.White;
            }
            //base.DoNew();
            //this.grid1.InsertRow();
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

                #region [Validate 체크]
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            if (this.PlantCode.Equals(string.Empty)
                                ||txtWorkCenterCode.Text == string.Empty
                                || txtInspCode.Text == string.Empty
                                || txtInspName.Text == string.Empty)
                            {
                                ShowDialog("작업장코드, 공정감사 코드, 공정감사 내용은 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);

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
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[3];

                            param[0] = helper.CreateParameter("@PlantCode", this.PlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@WorkCenterCode", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[2] = helper.CreateParameter("@InspCode", txtInspCode.Text, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드

                            helper.ExecuteNoneQuery("USP_BM7700_D1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가
                            param = new SqlParameter[7];

                            param[0] = helper.CreateParameter("@PlantCode", this.PlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@WorkCenterCode", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[2] = helper.CreateParameter("@InspCode", txtInspCode.Text, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[3] = helper.CreateParameter("@InspName", txtSpec.Text, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[4] = helper.CreateParameter("@Spec", txtInspName.Text, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[5] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(cboUseFlag.Value), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[6] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드

                            helper.ExecuteNoneQuery("USP_BM7700_I1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                    }
                }

                if(txtSpec.Appearance.BackColor == System.Drawing.Color.Lime
                    || txtSpec.Appearance.BackColor == System.Drawing.Color.Lime)
                {
                    #region 수정
                    param = new SqlParameter[7];

                    param[0] = helper.CreateParameter("@PlantCode", this.PlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                    param[1] = helper.CreateParameter("@WorkCenterCode", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                    param[2] = helper.CreateParameter("@InspCode", txtInspCode.Text, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                    param[3] = helper.CreateParameter("@InspName", txtSpec.Text, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                    param[4] = helper.CreateParameter("@Spec", txtInspName.Text, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                    param[5] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(cboUseFlag.Value), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                    param[6] = helper.CreateParameter("@Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드

                    helper.ExecuteNoneQuery("USP_BM7700_U1", CommandType.StoredProcedure, param);
                    #endregion
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

                //txtInspName.Appearance.BackColor = System.Drawing.Color.White;
                //txtSpec.Appearance.BackColor = System.Drawing.Color.White;

                txtReadOnly(true);
            }
        }
        #endregion

        public void GridInit()
        {
            #region 그리드
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCode", "공정감사 코드", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "공정감사 대상", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Spec", "공정감사 Spec", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            DtChange = (DataTable)grid1.DataSource;
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;
            #endregion
        }

        private void grid1_ClickCell(object sender, ClickCellEventArgs e)
        {
            this.txtWorkCenterCode.Text = grid1.ActiveRow.Cells["WorkCenterCode"].Value.ToString();
            this.txtWorkCenterName.Text = grid1.ActiveRow.Cells["WorkCenterName"].Value.ToString();
            this.txtInspCode.Text = grid1.ActiveRow.Cells["InspCode"].Value.ToString();
            this.txtSpec.Text = grid1.ActiveRow.Cells["InspName"].Value.ToString();
            this.txtMaker.Text = grid1.ActiveRow.Cells["Maker"].Value.ToString();
            this.cboUseFlag.Value = grid1.ActiveRow.Cells["UseFlag"].Value.ToString();
            this.txtInspName.Text = grid1.ActiveRow.Cells["Spec"].Value.ToString();

            this.txtSpec.ReadOnly = false;
            this.txtInspName.ReadOnly = false;
            this.cboUseFlag.ReadOnly = false;
            this.PlantCode = grid1.ActiveRow.Cells["PlantCode"].Value.ToString();
           
        }

        private void txtInspName_TextChanged(object sender, EventArgs e)
        {
            //txtInspName.Appearance.BackColor = System.Drawing.Color.Lime;
        }

        private void txtSpec_ValueChanged(object sender, EventArgs e)
        {
            //txtSpec.Appearance.BackColor = System.Drawing.Color.Lime;
        }

        private void txtReadOnly(bool Flag)
        {
            this.txtWorkCenterCode.ReadOnly = Flag;
            this.txtWorkCenterName.ReadOnly = Flag;
            this.txtInspCode.ReadOnly = Flag;
            this.txtSpec.ReadOnly = Flag;
            this.txtInspName.ReadOnly = Flag;
            this.cboUseFlag.ReadOnly = Flag;

        }

        private void lblRemark_Click(object sender, EventArgs e)
        {

        }
    }
}
