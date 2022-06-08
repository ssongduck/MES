#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM5100
//   Form Name    : 품목코드 마스터
//   Name Space   : SAMMI.BM
//   Created Date : 2012-06-08
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 기준정보 ( 품목 마스터 ) 정보 관리 폼 
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
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using SAMMI.Common;
#endregion

namespace SAMMI.BM
{
    public partial class BM5100 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통 
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;


        private DataTable DtChange = null;
        private int _Fix_Col = 0;

     

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >
        public BM5100()
        {
            InitializeComponent();
            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (!(this.PlantCode.Equals("SK1") || this.PlantCode.Equals("SK2")))
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;

            btbManager = new BizTextBoxManagerEX();
            gridManager = new BizGridManagerEX(grid1);

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "", "" }
                        , new string[] { "", "" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "", "" }
                        , new string[] { "", "" }, new object[] { });
            }

            gridManager.PopUpAdd("SS_WorkCenterCode", "SS_WorkCenterName", "TBM0600", new string[] { "PlantCode", "", "", "" });
            gridManager.PopUpAdd("SS_ItemCode", "SS_ItemName", "TBM0100", new string[] { "PlantCode", "", "" });
        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            #region <조회>
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];
            
            try
            {
                base.DoInquire();

                string sUseFlag = SqlDBHelper.nvlString(cboUseFlag_H.Value);
                //string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.SelectedValue);
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sItemType = SqlDBHelper.nvlString(cboItemType_H.Value);
                string sItemCode = SqlDBHelper.nvlString(txtItemCode.Text);

                param[0] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@ItemType", sItemType, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_BM5100_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM5100_S1_UNION", CommandType.StoredProcedure, param);

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
            #endregion
        }
        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            //base.DoNew();

            //int iRow = _GridUtil.AddRow(this.grid1, DtChange);
            ////UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
            ////UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemType", iRow);
            ////UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow);
            ////UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemName", iRow);
            ////UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemSpec", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "StQty1", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "StQty2", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "StQty3", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "SS_itemcode", iRow);

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
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            // Validate 체크
                            if (SqlDBHelper.nvlString(dr["ItemCode"]) == "" || SqlDBHelper.nvlString(dr["PlantCode"]) == "")
                            {
                                ShowDialog("사업장, 품목은 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

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
                    return;

                base.DoSave();
                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow drRow in DtChange.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제 - 사용안함
                             #endregion
                            break;

                        case DataRowState.Added:
                            #region 추가 - 사용안함
                                 #endregion
                            break;

                        case DataRowState.Modified:
                            #region 수정 - 통합완료
                            param = new SqlParameter[7];

                            param[0] = helper.CreateParameter("PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[1] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                            param[2] = helper.CreateParameter("StQty1", drRow["StQty1"].ToString(), SqlDbType.Int, ParameterDirection.Input);              // 공정순서
                            param[3] = helper.CreateParameter("StQty2", drRow["StQty2"].ToString(), SqlDbType.Int, ParameterDirection.Input);              // 공정순서
                            param[4] = helper.CreateParameter("StQty3", drRow["StQty3"].ToString(), SqlDbType.Int, ParameterDirection.Input);              // 공정순서
                            param[5] = helper.CreateParameter("SS_Itemcode", drRow["SS_ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[6] = helper.CreateParameter("SS_WorkCenterCode", drRow["SS_WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
 
                            helper.ExecuteNoneQuery("USP_BM5100_U1", CommandType.StoredProcedure, param);

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

        public override void DoDownloadExcel()
        {
            if (this.grid1.Rows.Count == 0)
            {
                SAMMI.Windows.Forms.MessageForm message = new SAMMI.Windows.Forms.MessageForm("조회된 Data가 없습니다.");
                message.ShowDialog();
                return;
            }
            base.DoDownloadExcel();
            this.grid1.ExportExcel();
        }
        #endregion

        private void BM5100_Load(object sender, EventArgs e)
        {
            
            #region [Grid]
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth.Equals("")) ? true : false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemType", "유형", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StQty1", "용기적입량1", false, GridColDataType_emu.Integer, 70, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StQty2", "용기적입량2", false, GridColDataType_emu.Integer, 70, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StQty3", "용기적입량3", false, GridColDataType_emu.Integer, 70, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SS_ItemCode", "사상품번", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SS_ItemName", "사상품명", false, GridColDataType_emu.VarChar, 200, 30, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SS_WorkCenterCode", "사상작업장", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SS_WorkCenterName", "사상작업장명", false, GridColDataType_emu.VarChar, 200, 30, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            #endregion

            #region <Combo Setting>
            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("ItemType");     //제품유형
           // SAMMI.Common.Common.FillComboboxMaster(this.cboItemType_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ItemType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("STORAGELOCTYPE");     //자재그룹
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "StorageLocCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("MATERIALGROUP");     //자재그룹
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MaterialGroup", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("ITEMGROUP");     //제품군
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ItemGroup", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("ITEMSPEC");     //제품규격
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ItemSpec", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");     //Lot관리여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "LotManaFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");     //시험품 여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "TestItemFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("INSPFLAG");     //차종
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "INSPFLAG", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("CARTYPE");     //차종
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "CarType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
            #endregion
        }

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "ItemName")
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