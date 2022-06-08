#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM1500
//   Form Name    : 검사항목명 관리
//   Name Space   : SAMMI.BM
//   Created Date : 2012-02-20
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
    public partial class BM1500 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        public BM1500()
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
                btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { this.cboPlantCode_H, cboInspCase_H, "", cboUseFlag_H });
            else
                btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { this.cboPlantCode_H, cboInspCase_H, "", cboUseFlag_H });
            
            GridInit();
        }
        #endregion

        #region BM1500_Load
        private void BM1500_Load(object sender, EventArgs e)
        {
            //GridInit();

            //_biz.TBM1500_POP_Grid(arr[0], arr[1], sValueCode, sValueName, arr[2], grid, sCode, sName,sParam2);
            
        }
        #endregion BM1500_Load

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
                string inspcode = this.txtInspCode.Text;
                string inspcase = SqlDBHelper.nvlString(cboInspCase_H.Value);
                string insptype = SqlDBHelper.nvlString(cboInspType_H.Value);
                string useflag = SqlDBHelper.nvlString(cboUseFlag_H.Value);

                param[0] = helper.CreateParameter("@PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@InspCode", inspcode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@InspCase", inspcase, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@InspType", insptype, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@UseFlag", useflag, SqlDbType.VarChar, ParameterDirection.Input);
                
                //rtnDtTemp = helper.FillTable("USP_BM1500_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM1500_S1_UNION", CommandType.StoredProcedure, param);

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
            this.grid1.InsertRow();
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
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" || SqlDBHelper.nvlString(dr["InspCode"]) == "")
                            {
                                ShowDialog("검사항목 코드는 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

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
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" || SqlDBHelper.nvlString(dr["InspCode"]) == "")
                            {
                                ShowDialog("검사항목 코드는 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

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
                            #region 삭제 - 통합완료
                            drRow.RejectChanges();

                            param = new SqlParameter[2];

                            param[0] = helper.CreateParameter("@PlantCode", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PlantCode"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@InspCode", SqlDBHelper.nvlString(drRow["InspCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            helper.ExecuteNoneQuery("USP_BM1500_D1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가 - 통합완료
                            param = new SqlParameter[9];

                            param[0] = helper.CreateParameter("@PlantCode", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PlantCode"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@InspCase", string.Empty, SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@InspCode", SqlDBHelper.nvlString(drRow["InspCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@InspName", SqlDBHelper.nvlString(drRow["InspName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@InspDesc", SqlDBHelper.nvlString(drRow["InspDesc"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@InspType", SqlDBHelper.nvlString(drRow["InspType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@UnitCode", string.Empty, SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            helper.ExecuteNoneQuery("USP_BM1500_I1", CommandType.StoredProcedure, param);
                            
                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정 - 통합완료
                            param = new SqlParameter[9];

                            param[0] = helper.CreateParameter("@PlantCode", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PlantCode"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@InspCase", string.Empty, SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@InspCode", SqlDBHelper.nvlString(drRow["InspCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@InspName", SqlDBHelper.nvlString(drRow["InspName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@InspDesc", SqlDBHelper.nvlString(drRow["InspDesc"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@InspType", SqlDBHelper.nvlString(drRow["InspType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@UnitCode", string.Empty, SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            helper.ExecuteNoneQuery("USP_BM1500_U1", CommandType.StoredProcedure, param);
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

        

        private void txtInspName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                DoInquire();
              
            }
        }

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의

        private void GridInit()
        {
            //_GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCode", "검사항목코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "검사항목명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
          //  _GridUtil.InitColumnUltraGrid(grid1, "InspCase", "검사구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspType", "검사대상", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspDesc", "검사항목상세", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
          //  _GridUtil.InitColumnUltraGrid(grid1, "UnitCode", "단위", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
           // _GridUtil.InitColumnUltraGrid(grid1, "UnitType", "단위종류", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            grid1.Columns["InspCode"].Header.Appearance.ForeColor = Color.Yellow;
            grid1.Columns["InspCode"].Header.Appearance.BackColor = Color.DeepSkyBlue;
            grid1.Columns["InspName"].Header.Appearance.ForeColor = Color.Yellow;
            grid1.Columns["InspName"].Header.Appearance.BackColor = Color.DeepSkyBlue;
            grid1.Columns["InspType"].Header.Appearance.ForeColor = Color.Yellow;
            grid1.Columns["InspType"].Header.Appearance.BackColor = Color.DeepSkyBlue;
            grid1.Columns["UseFlag"].Header.Appearance.ForeColor = Color.Yellow;
            grid1.Columns["UseFlag"].Header.Appearance.BackColor = Color.DeepSkyBlue;

            #region 콤보박스
            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("INSPCASE");     //PCTYPE
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "InspCase", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("INSPTYPE");     //PCTYPE
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "InspType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion
        }
        #endregion
    }
}
