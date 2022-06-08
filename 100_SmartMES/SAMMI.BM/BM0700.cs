#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : 
//   Form Name    : 
//   Name Space   : 
//   Created Date : 
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAMMI.PopUp;
using SAMMI.PopManager;
using SAMMI.Common;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{
    public partial class BM0700 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();
   
        private DataTable DtChange = null;

        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
    
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        private int _Fix_Col = 0;

        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >

        public BM0700()
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
                btbManager.PopUpAdd(txtMachCode, txtMachName, "TBM0700", new object[] { this.cboPlantCode_H, "", "", "", "" });
            }
            else
            {
                btbManager.PopUpAdd(txtMachCode, txtMachName, "TBM0700", new object[] { LoginInfo.PlantAuth, "", "", "", "" });
            }
        }

        private void BM0700_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", true, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Line", "가공라인", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "MachCode", "설비코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Machname", "설비명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SerialNo", "시리얼번호", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ModelName", "모델명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachType", "설비유형", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachCase", "설비구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "MachType1", "설비유형", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Default, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "MachType2", "분류2", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Default, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdFlag", "실적장비여부", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeCompany", "제조사", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            ////_GridUtil.InitColumnUltraGrid(grid1, "LifeTime", "수명(년)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            ////_GridUtil.InitColumnUltraGrid(grid1, "Contact", "연락처", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAWorker1", "작업담당자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            ////_GridUtil.InitColumnUltraGrid(grid1, "MAWorker2", "작업담당자2", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            ////_GridUtil.InitColumnUltraGrid(grid1, "TechWorker", "장비기술담당자", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            ////_GridUtil.InitColumnUltraGrid(grid1, "BuyDate", "도입일자", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            ////_GridUtil.InitColumnUltraGrid(grid1, "BuyCost", "도입가격", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right,true, true, "###,###,###,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Status", "상태", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspLastDate", "최종검사일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LimitDate", "유효일수", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Cavity", "CAVITY", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Designshot", "설계사용수", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Workshot", "작업사용수", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Totshot", "합계사용수", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachLoc", "설치장소", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "WorkerNM", "등록자명", false, GridColDataType_emu.VarChar, 115, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "EditorNM", "수정자명", false, GridColDataType_emu.VarChar, 115, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
                                     

            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            DtChange = (DataTable)grid1.DataSource;

         

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");


            rtnDtTemp = _Common.GET_TBM0000_CODE("MachType");  
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MachType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            //rtnDtTemp = _Common.GET_TBM0000_CODE("MachType1"); 
            //SAMMI.Common.Common.FillComboboxMaster(this.cboMachType_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MachType1", rtnDtTemp, "CODE_ID", "CODE_NAME");

            //rtnDtTemp = _Common.GET_TBM0000_CODE("MachType2"); 
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MachType2", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("MachLoc"); 
            //SAMMI.Common.Common.FillComboboxMaster(this.cboMachLoc_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MachLoc", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("MACHSTATUS"); 
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Status", rtnDtTemp, "CODE_ID", "CODE_NAME");


            rtnDtTemp = _Common.GET_TBM0000_CODE("MACHCASE"); 
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MachCase", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO"); //
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ProdFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

      
            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboMachLoc_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

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
              //SqlDBHelper helper = new SqlDBHelper(false,"Data Source=192.168.100.20;Initial Catalog=MTMES;User ID=sa;Password=qwer1234!~");
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                DtChange.Clear();

                base.DoInquire();

                //string sPlantCode   = cboPlantCode_H.SelectedValue.ToString() == "ALL" ? "" : cboPlantCode_H.SelectedValue.ToString(); 
                //string sOpCode    = txtOpCode.Text.Trim();                                                          
                //string sWorkerName    = txtWorkerName_H.Text.Trim();                                                            
                //string sClassCode  = cboClassCode_H.SelectedValue.ToString() == "ALL" ? "" : cboClassCode_H.SelectedValue.ToString();          
                //string sUseFlag  = cboUseFlag_H.SelectedValue.ToString() == "ALL" ? "" : cboUseFlag_H.SelectedValue.ToString();          

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sMachCode = this.txtMachCode.Text;
                string sMachType = SqlDBHelper.nvlString(cboMachType_H.Value);
                string sWMachName = txtMachName.Text.Trim();
                string sMachLoc = "";
                string sUseFlag = SqlDBHelper.nvlString(sCodeNMComboBox1.Value);


                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("MachCode", sMachCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("MachType", sMachType, SqlDbType.VarChar, ParameterDirection.Input);            
                param[3] = helper.CreateParameter("MachName", sWMachName, SqlDbType.VarChar, ParameterDirection.Input);       
                param[4] = helper.CreateParameter("MachLoc", sMachLoc, SqlDbType.VarChar, ParameterDirection.Input);            
                param[5] = helper.CreateParameter("UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input); 
                
                rtnDtTemp = helper.FillTable("USP_BM0700_S1_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;
                //_Common.Grid_Column_Width(this.grid1); //grid 정리용
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

                UltraGridUtil.ActivationAllowEdit(this.grid1, "MachCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Machname", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MachType", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "MachType1", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "MachType2", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ProdFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeCompany", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ModelName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "SerialNo", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "LifeTime", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MachCase", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MAWorker1", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "MAWorker2", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "TechWorker", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "BuyDate", iRow);
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "BuyCost", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Status", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "InspLastDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LimitDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Cavity", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Designshot", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Workshot", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Totshot", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Remark", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MachLoc", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);     
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);    
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);       
                UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);    
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);      
                //UltraGridUtil.ActivationAllowEdit(this.grid1, "EditorNM", iRow);    
                
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
           //UltraGridUtil.DeleteCurrentRowGrid(this, this.grid1, DtChange);
            base.DoDelete();

            this.grid1.DeleteRow();
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
           
               //SqlDBHelper helper = new SqlDBHelper(false,"Data Source=192.168.100.20;Initial Catalog=MTMES;User ID=sa;Password=qwer1234!~");
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;

            try
            {
                this.Focus();

                #region [Validation 체크]
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" || SqlDBHelper.nvlString(dr["MachCode"]) == "")
                            {
                                ShowDialog("설비코드는 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            break;
                    }
                }
                #endregion

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
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[4];

                            param[0] = helper.CreateParameter("MachCode", drRow["MachCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            
                            param[2] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[3] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM0700_D1", CommandType.StoredProcedure, param);

                            if (param[2].Value.ToString() == "E") throw new Exception(param[3].Value.ToString());
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가
                            param = new SqlParameter[25];
                            
                            param[0] = helper.CreateParameter("MachCode", drRow["MachCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            param[1] = helper.CreateParameter("Machname", drRow["Machname"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    
                            param[2] = helper.CreateParameter("MachType", drRow["MachType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    
                           // param[3] = helper.CreateParameter("MachType1",   drRow["MachType1"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       
                            //param[4] = helper.CreateParameter("MachType2", drRow["MachType2"].ToString(), SqlDbType.VarChar, ParameterDirection.Input); 
                            param[3] = helper.CreateParameter("ProdFlag", drRow["ProdFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[4] = helper.CreateParameter("MakeCompany", drRow["MakeCompany"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[5] = helper.CreateParameter("ModelName", drRow["ModelName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[6] = helper.CreateParameter("SerialNo", drRow["SerialNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            //param[9] = helper.CreateParameter("LifeTime", drRow["LifeTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            //param[10] = helper.CreateParameter("Contact", drRow["Contact"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[7] = helper.CreateParameter("MachCase", drRow["MachCase"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("MAWorker1", drRow["MAWorker1"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            //param[13] = helper.CreateParameter("MAWorker2", drRow["MAWorker2"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            //param[14] = helper.CreateParameter("TechWorker", drRow["TechWorker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            //param[10] = helper.CreateParameter("BuyDate", drRow["BuyDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            //param[11] = helper.CreateParameter("BuyCost", drRow["BuyCost"].ToString(), SqlDbType.VarChar, ParameterDirection.Input); 
                            param[9] = helper.CreateParameter("Status",  drRow["Status"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            param[10] = helper.CreateParameter("InspLastDate", drRow["InspLastDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[11] = helper.CreateParameter("LimitDate", drRow["LimitDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[12] = helper.CreateParameter("Cavity", drRow["Cavity"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[13] = helper.CreateParameter("Designshot", drRow["Designshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[14] = helper.CreateParameter("Workshot", drRow["Workshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[15] = helper.CreateParameter("Totshot", drRow["Totshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[16] = helper.CreateParameter("Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[17] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[18] = helper.CreateParameter("MachLoc", drRow["MachLoc"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[19] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[20] = helper.CreateParameter("Maker", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[21] = helper.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[22] = helper.CreateParameter("Line", drRow["Line"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);        
 
                            param[23] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[24] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM0700_I1", CommandType.StoredProcedure, param);

                            if (param[21].Value.ToString() == "E") throw new Exception(param[22].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정
                            param = new SqlParameter[25];

  
                           param[0] = helper.CreateParameter("MachCode", drRow["MachCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            param[1] = helper.CreateParameter("Machname", drRow["Machname"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    
                            param[2] = helper.CreateParameter("MachType", drRow["MachType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    
                           // param[3] = helper.CreateParameter("MachType1",   drRow["MachType1"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       
                            //param[4] = helper.CreateParameter("MachType2", drRow["MachType2"].ToString(), SqlDbType.VarChar, ParameterDirection.Input); 
                            param[3] = helper.CreateParameter("ProdFlag", drRow["ProdFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[4] = helper.CreateParameter("MakeCompany", drRow["MakeCompany"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[5] = helper.CreateParameter("ModelName", drRow["ModelName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[6] = helper.CreateParameter("SerialNo", drRow["SerialNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            //param[9] = helper.CreateParameter("LifeTime", drRow["LifeTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            //param[10] = helper.CreateParameter("Contact", drRow["Contact"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[7] = helper.CreateParameter("MachCase", drRow["MachCase"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("MAWorker1", drRow["MAWorker1"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            //param[13] = helper.CreateParameter("MAWorker2", drRow["MAWorker2"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            //param[14] = helper.CreateParameter("TechWorker", drRow["TechWorker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            //param[10] = helper.CreateParameter("BuyDate", drRow["BuyDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            //param[11] = helper.CreateParameter("BuyCost", drRow["BuyCost"].ToString(), SqlDbType.VarChar, ParameterDirection.Input); 
                            param[9] = helper.CreateParameter("Status",  drRow["Status"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   
                            param[10] = helper.CreateParameter("InspLastDate", drRow["InspLastDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[11] = helper.CreateParameter("LimitDate", drRow["LimitDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[12] = helper.CreateParameter("Cavity", drRow["Cavity"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[13] = helper.CreateParameter("Designshot", drRow["Designshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[14] = helper.CreateParameter("Workshot", drRow["Workshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[15] = helper.CreateParameter("Totshot", drRow["Totshot"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[16] = helper.CreateParameter("Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[17] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[18] = helper.CreateParameter("MachLoc", drRow["MachLoc"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     
                            param[19] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       
                            param[20] = helper.CreateParameter("Editor",  SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);        
                            param[21] = helper.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[22] = helper.CreateParameter("Line", drRow["Line"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);        
 
                            param[23] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[24] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM0700_U1", CommandType.StoredProcedure, param);

                            if (param[21].Value.ToString() == "E") throw new Exception(param[22].Value.ToString()); 

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

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "Machname")
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

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        #endregion
    }
}
