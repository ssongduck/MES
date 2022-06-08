#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM3800
//   Form Name    : MES P/C 관리
//   Name Space   : 
//   Created Date : 
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 
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
    public partial class BM3800 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        #endregion

        #region < CONSTRUCTOR >
        public BM3800()
        {
            InitializeComponent();

            this.txtPlantCode.Text = "[" + LoginInfo.UserPlantCode + "] " + LoginInfo.UserPlantName;
            GridInit();
           
        }
        #endregion

        #region BM3800_Load
        private void BM3800_Load(object sender, EventArgs e)
        {
        }
        #endregion BM3800_Load

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                base.DoInquire();

                string PlantCode = LoginInfo.UserPlantCode;
                string pctype = SqlDBHelper.nvlString(this.cboPCType_H.Value);
                string ipaddress = SqlDBHelper.nvlString(this.txtIPAddress_H.Value);
                string useflag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

                param[0] = helper.CreateParameter("@PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@PCType", pctype, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@IPAddress", ipaddress, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@UseFlag", useflag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM3800_S1", CommandType.StoredProcedure, param);

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
                UltraGridUtil.ActivationAllowEdit(this.grid1, "IPAddress", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PCDesc", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PCType", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "BarPrtUseFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "BarPrtPortNo", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "BarPrtComSet", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "BarScanUseFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "BarScanPortNo", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "BarScanComSet", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Remark", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "BarPrintType", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
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
                            if (LoginInfo.UserPlantCode == "" || SqlDBHelper.nvlString(dr["IPAddress"]) == "")
                            {
                                ShowDialog("IP 주소는 필수입력사항 입니다.", Windows.Forms.DialogForm.DialogType.OK);

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

                            param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@IPAddress", drRow["IPAddress"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드

                            helper.ExecuteNoneQuery("USP_BM3800_D1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region [추가]
                            param = new SqlParameter[13];

                            param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[1] = helper.CreateParameter("@IPAddress", drRow["IPAddress"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[2] = helper.CreateParameter("@PCDesc", drRow["PCDesc"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@PCType", drRow["PCType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[4] = helper.CreateParameter("@BarPrtUseFlag", drRow["BarPrtUseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[5] = helper.CreateParameter("@BarPrtPortNo", drRow["BarPrtPortNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[6] = helper.CreateParameter("@BarPrtComSet", drRow["BarPrtComSet"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@BarScanUseFlag", drRow["BarScanUseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[8] = helper.CreateParameter("@BarScanPortNo", drRow["BarScanPortNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[9] = helper.CreateParameter("@BarScanComSet", drRow["BarScanComSet"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[10] = helper.CreateParameter("@Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[12] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서

                            helper.ExecuteNoneQuery("USP_BM3800_I1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region [수정]
                            param = new SqlParameter[13];

                           param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[1] = helper.CreateParameter("@IPAddress", drRow["IPAddress"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[2] = helper.CreateParameter("@PCDesc", drRow["PCDesc"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@PCType", drRow["PCType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[4] = helper.CreateParameter("@BarPrtUseFlag", drRow["BarPrtUseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[5] = helper.CreateParameter("@BarPrtPortNo", drRow["BarPrtPortNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[6] = helper.CreateParameter("@BarPrtComSet", drRow["BarPrtComSet"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@BarScanUseFlag", drRow["BarScanUseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[8] = helper.CreateParameter("@BarScanPortNo", drRow["BarScanPortNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[9] = helper.CreateParameter("@BarScanComSet", drRow["BarScanComSet"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[10] = helper.CreateParameter("@Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[12] = helper.CreateParameter("@Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서

                            helper.ExecuteNoneQuery("USP_BM3800_U1", CommandType.StoredProcedure, param);
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

        /// <summary>
        /// DATABASE UPDATE전 VALIDATEION CHECK 및 값을 수정한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        void Adapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            if (e.Row.RowState == DataRowState.Modified)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                return;
            }

            if (e.Row.RowState == DataRowState.Added)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                e.Command.Parameters["@Maker"].Value = this.WorkerID;
                return;
            }
        }

        /// <summary>
        /// 저장처리시 오류가 발생한 경우 오류 메세지에 대한 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Errors == null) return;

            switch (((SqlException)e.Errors).Number)
            {
                // 중복
                case 2627:
                    e.Row.RowError = "창고정보가 있습니다.";
                    throw (new SException("S00099", e.Errors));
                default:
                    break;
            }
        }
        #endregion

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의

        private void GridInit()
        {
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IPAddress", "IP", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PCDesc", "단말기", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PCType", "단말타입", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BarPrtUseFlag", "라벨프린터 여부", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BarPrtPortNo", "라벨프린터 포트", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BarPrtComSet", "라벨프린터 포트설정", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BarScanUseFlag", "유선스캐너 여부", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BarScanPortNo", "유선스캐너 포트", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BarScanComSet", "유선스캐너 포트설정", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BarPrintType", "라벨프린터 타입", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
            //rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PCTYPE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PCType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
        }
        #endregion
    }
}
