#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM0100
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
    public partial class BM0100 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통 
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;
        private int _Fix_Col = 0;



        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        private string PlantCode = string.Empty;

        #endregion

        #region < CONSTRUCTOR >
        public BM0100()
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
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                DtChange.Clear();
                base.DoInquire();

                string sUseFlag = SqlDBHelper.nvlString(cboUseFlag_H.Value);
                //string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.SelectedValue);
                string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);
                string sItemType = SqlDBHelper.nvlString(cboItemType_H.Value);
                string sItemCode = SqlDBHelper.nvlString(txtItemCode.Text);

                param[0] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@ItemType", sItemType, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@ItemName", txtItemName.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_BM0100_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0100_S1_UNION", CommandType.StoredProcedure, param);

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
            base.DoNew();

            int iRow = _GridUtil.AddRow(this.grid1, DtChange);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemType", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "PTItemName", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemSpec", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "StorageLocCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "LotManaFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MaterialGroup", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemGroup", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "BaseUnit", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "InLotSizeType", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "InspFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MinLotSize", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MaxLotSize", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "FixLotSize", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "CycleTime", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UPH", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "CarType", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "BoxSpec", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UnitLength", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UnitThick", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UnitWidth", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "TestItemFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Remark", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UseMatQty", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "StQty2", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "StQty3", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);

            grid1.Rows[iRow].Cells["PlantCode"].Value = LoginInfo.PlantAuth.Equals("") ? "" : LoginInfo.PlantAuth; 

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
                            if (!LoginInfo.PlantAuth.Equals("") &&
                                !LoginInfo.PlantAuth.Equals(SqlDBHelper.nvlString(dr["PlantCode"])))
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
                            drRow.RejectChanges();

                            param = new SqlParameter[2];

                            param[0] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드

                            helper.ExecuteNoneQuery("USP_BM0100_D1", CommandType.StoredProcedure, param);
                            #endregion
                            break;

                        case DataRowState.Added:
                            #region 추가 - 사용안함
                            param = new SqlParameter[34];

                            param[0] = helper.CreateParameter("PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         
                            param[1] = helper.CreateParameter("ItemType", drRow["ItemType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           
                            param[2] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           
                            param[3] = helper.CreateParameter("ItemName", drRow["ItemName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           
                            param[4] = helper.CreateParameter("ItemSpec", drRow["ItemSpec"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           
                            param[5] = helper.CreateParameter("StorageLocCode", drRow["StorageLocCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input); 
                            param[6] = helper.CreateParameter("LotManaFlag", drRow["LotManaFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       
                            param[7] = helper.CreateParameter("MaterialGroup", drRow["MaterialGroup"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            param[8] = helper.CreateParameter("ItemGroup", drRow["ItemGroup"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           
                            param[9] = helper.CreateParameter("BaseUnit", drRow["BaseUnit"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             
                            param[10] = helper.CreateParameter("InLotSizeType", drRow["InLotSizeType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[11] = helper.CreateParameter("InspFlag", drRow["InspFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);            
                            param[12] = helper.CreateParameter("MinLotSize", drRow["MinLotSize"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);        
                            param[13] = helper.CreateParameter("MaxLotSize", drRow["MaxLotSize"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);        
                            param[14] = helper.CreateParameter("FixLotSize", drRow["FixLotSize"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);        
                            param[15] = helper.CreateParameter("CycleTime", drRow["CycleTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[16] = helper.CreateParameter("UPH", drRow["UPH"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             
                            param[17] = helper.CreateParameter("CarType", drRow["CarType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              
                            param[18] = helper.CreateParameter("BoxSpec", drRow["BoxSpec"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              
                            param[19] = helper.CreateParameter("UnitLength", drRow["UnitLength"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);        
                            param[20] = helper.CreateParameter("UnitThick", drRow["UnitThick"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[21] = helper.CreateParameter("UnitWidth", drRow["UnitWidth"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[22] = helper.CreateParameter("TestItemFlag", drRow["TestItemFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    
                            param[23] = helper.CreateParameter("Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       
                            param[24] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[25] = helper.CreateParameter("StQty1", drRow["StQty1"].ToString(), SqlDbType.Int, ParameterDirection.Input);           
                            param[26] = helper.CreateParameter("StQty2", drRow["StQty2"].ToString(), SqlDbType.Int, ParameterDirection.Input);           
                            param[27] = helper.CreateParameter("StQty3", drRow["StQty3"].ToString(), SqlDbType.Int, ParameterDirection.Input);           
                            param[28] = helper.CreateParameter("Maker", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);     
                            param[29] = helper.CreateParameter("Editor", drRow["Editor"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       
                            param[30] = helper.CreateParameter("UseMatQty", drRow["UseMatQty"].ToString(), SqlDbType.Float, ParameterDirection.Input);   
                            param[31] = helper.CreateParameter("ShortCarType", drRow["ShortCarType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);      
                            param[32] = helper.CreateParameter("ShortName", drRow["ShortName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);            
                            param[33] = helper.CreateParameter("HamchimFlag", drRow["HamchimFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);        

                            helper.ExecuteNoneQuery("USP_BM0100_I1", CommandType.StoredProcedure, param);
                            #endregion
                            break;

                        case DataRowState.Modified:
                            #region 수정 - 통합완료
                            param = new SqlParameter[38];

                            param[0] = helper.CreateParameter("PlantCode",(LoginInfo.PlantAuth.Equals("")) ? drRow["PlantCode"].ToString() : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[1] = helper.CreateParameter("ItemType", drRow["ItemType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);            
                            param[2] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[3] = helper.CreateParameter("ItemName", drRow["ItemName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[4] = helper.CreateParameter("ItemSpec", drRow["ItemSpec"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[5] = helper.CreateParameter("StorageLocCode", drRow["StorageLocCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("LotManaFlag", drRow["LotManaFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);      
                            param[7] = helper.CreateParameter("MaterialGroup", drRow["MaterialGroup"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[8] = helper.CreateParameter("ItemGroup", drRow["ItemGroup"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[9] = helper.CreateParameter("BaseUnit", drRow["BaseUnit"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);            
                            param[10] = helper.CreateParameter("InLotSizeType", drRow["InLotSizeType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input); 
                            param[11] = helper.CreateParameter("InspFlag", drRow["InspFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           
                            param[12] = helper.CreateParameter("MinLotSize", drRow["MinLotSize"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       
                            param[13] = helper.CreateParameter("MaxLotSize", drRow["MaxLotSize"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       
                            param[14] = helper.CreateParameter("FixLotSize", drRow["FixLotSize"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       
                            param[15] = helper.CreateParameter("CycleTime", drRow["CycleTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         
                            param[16] = helper.CreateParameter("UPH", drRow["UPH"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              
                            param[17] = helper.CreateParameter("CarType", drRow["CarType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);      
                            param[18] = helper.CreateParameter("BoxSpec", drRow["BoxSpec"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);      
                            param[19] = helper.CreateParameter("UnitLength", drRow["UnitLength"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[20] = helper.CreateParameter("UnitThick", drRow["UnitThick"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[21] = helper.CreateParameter("UnitWidth", drRow["UnitWidth"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[22] = helper.CreateParameter("TestItemFlag", drRow["TestItemFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             
                            param[23] = helper.CreateParameter("Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         
                            param[24] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       
                            param[25] = helper.CreateParameter("StQty1", drRow["StQty1"].ToString(), SqlDbType.Int, ParameterDirection.Input);             
                            param[26] = helper.CreateParameter("StQty2", drRow["StQty2"].ToString(), SqlDbType.Int, ParameterDirection.Input);             
                            param[27] = helper.CreateParameter("StQty3", drRow["StQty3"].ToString(), SqlDbType.Int, ParameterDirection.Input);             
                            param[28] = helper.CreateParameter("Maker", drRow["Maker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           
                            param[29] = helper.CreateParameter("Editor", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);      
                            param[30] = helper.CreateParameter("UseMatQty", drRow["UseMatQty"].ToString() == "" ? "0" : drRow["UseMatQty"].ToString(), SqlDbType.Float, ParameterDirection.Input);              // 공정순서
                            param[31] = helper.CreateParameter("ShortCarType", drRow["ShortCarType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);      
                            param[32] = helper.CreateParameter("ShortName", drRow["ShortName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);            
                            param[33] = helper.CreateParameter("PTItemName", drRow["PTItemName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[34] = helper.CreateParameter("PTItemCode", drRow["PTItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[35] = helper.CreateParameter("DMItemCode", drRow["DMItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[36] = helper.CreateParameter("DMItemName", drRow["DMItemName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          
                            param[37] = helper.CreateParameter("HamchimFlag", drRow["HamchimFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);        

                            helper.ExecuteNoneQuery("USP_BM0100_U1", CommandType.StoredProcedure, param);

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

        #region < EVENT AREA >
        /// <summary>
        /// Form이 Close 되기전에 발생
        /// e.Cancel을 true로 설정 하면, Form이 close되지 않음
        /// 수정 내역이 있는지를 확인 후 저장여부를 물어보고 저장, 저장하지 않기, 또는 화면 닫기를 Cancel 함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {

        }

        /// <summary>
        /// DATABASE UPDATE전 VALIDATEION CHECK 및 값을 수정한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    e.Row.RowError = "공정코드가 있습니다.";
                    throw (new SException("C:S00099", e.Errors));
                default:
                    break;
            }
        }


        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            // this.txtItemName.Text = string.Empty;
        }

        private void txtItemCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_Item();
            }
        }

        private void txtItemCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_Item();
        }

        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
            //  this.txtItemCode.Text = string.Empty;
        }

        private void txtItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_Item();
            }
        }

        private void txtItemName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_Item();
        }

        #endregion

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의

        #region 텍스트 박스에서 팝업창에서 값 가져오기
        private void Search_Pop_Item()
        {
            string sitem_cd = this.txtItemCode.Text.Trim();  // 품목코드
            string sitem_name = this.txtItemName.Text.Trim();  // 품목명
            string splantcd = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            string sitemtype = "";

            // _biz.TBM0100_POP( sitem_cd, sitem_name,splantcd, sitemtype,txtItemCode, txtItemName);



        }
        #endregion

        private void BM0100_Load(object sender, EventArgs e)
        {

            #region [Grid]
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemType", "유형", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PTItemCode", "파워텍코드", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PTItemName", "파워텍품명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DMItemCode", "다이모스코드", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DMItemName", "다이모스품명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BaseUnit", "단위", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StorageLocCode", "저장위치", false, GridColDataType_emu.VarChar, 120, 150, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LotManaFlag", "Lot관리여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MinLotSize", "최소Lot 크기", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MaxLotSize", "최대Lot 크기", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FixLotSize", "고정Lot 크기", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InLotSizeType", "입고Lot 크기", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspFlag", "수입검사 여부", false, GridColDataType_emu.VarChar, 100, 1, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "HamchimFlag", "함침여부", false, GridColDataType_emu.VarChar, 100, 1, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CycleTime", "싸이클 타임", false, GridColDataType_emu.IntegerPositive, 100, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UPH", "UPH", false, GridColDataType_emu.DoublePositive, 40, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseMatQty", "액상사용비율", false, GridColDataType_emu.DoublePositive, 70, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemSpec", "규격", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MaterialGroup", "자재그룹", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemGroup", "제품군", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CarType", "차종", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BoxSpec", "포장사양", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UnitLength", "길이", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UnitThick", "무게", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UnitWidth", "폭", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "TestItemFlag", "시험품여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용유무", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ShortCarType", "약어(차종)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ShortName", "약어(품명)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StQty1", "용기적입량1", false, GridColDataType_emu.Integer, 70, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StQty2", "용기적입량2", false, GridColDataType_emu.Integer, 70, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StQty3", "용기적입량3", false, GridColDataType_emu.Integer, 70, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            #endregion

            //grid1.Columns[0].Header.Appearance.BackColor = Color.Yellow;
            //grid1.Columns[1].Header.Appearance.BackColor = Color.Yellow;
            //grid1.Columns[2].Header.Appearance.BackColor = Color.Yellow;
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


            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //함침여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "HamchimFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            #endregion
        }

        #endregion


        private void cboGetUseFlag_H_ValueChanged(object sender, EventArgs e)
        {

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