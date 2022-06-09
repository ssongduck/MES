#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : AP0100
//   Form Name    : 생산계획정보관리
//   Name Space   : SAMMI.AP
//   Created Date : 2012-11-27
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 생산계획정보관리 화면
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
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.AP
{
    public partial class AP0100 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();
        BizTextBoxManagerEX btbManager;
        //BizGridManagerEX gridManager;
        
        private DataTable DtChange = null;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        string PlantCode = string.Empty;
        #endregion

        //커밋 테스트

        #region < CONSTRUCTOR >

        public AP0100()
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
            if(LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, "", "" }
                , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
            }
            else
            {
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, "", "" }
                , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
            }
        }
        #endregion

        #region AP0100_Load
        private void AP0100_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);


            // InitColumnUltraGrid  (91 111 110 114 185 95 100 100 100 156 209 100 100 100 100 90 90 169 90 90 90 90 90 90 90 90 90 90 )
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Linecode", "라인코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlanNo", "작지번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderType", "유형", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlanQty", "지시량", false, GridColDataType_emu.Integer, 70, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", "nnn,nnn,nnn", null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StartPlanDate", "지시일자", false, GridColDataType_emu.YearMonthDay, 110, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EndPlanDate", "지시일자(종)", false, GridColDataType_emu.YearMonthDay, 110, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UnitCode", "단위", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlanType", "생산의뢰구분코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DelivaryDate", "납기일", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderAssignFlag",      "지시할당여부", false, GridColDataType_emu.VarChar, 95, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "PlanNo",               "계획번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "AssignWorkCenterCode", "지시할당작업장", false, GridColDataType_emu.VarChar, 105, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldAssignFlag",       "금형할당여부", false, GridColDataType_emu.VarChar, 95, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerAssignFlag",     "작업자할당여부", false, GridColDataType_emu.VarChar, 105, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StartWorkDate",        "생산시작일", false, GridColDataType_emu.YearMonthDay, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EndWorkDate", "생산완료일시", false, GridColDataType_emu.DateTime24, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "수불일자", false, GridColDataType_emu.YearMonthDay, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용유무", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일", false, GridColDataType_emu.YearMonthDay, 150, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일", false, GridColDataType_emu.YearMonthDay, 150, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);         
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            DtChange = (DataTable)grid1.DataSource;
            //     ///row number
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

            //grid1.Columns["LineCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["LineName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            

            rtnDtTemp = _Common.GET_TBM0000_CODE("PlanType");  //생산의뢰구분코드
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlanType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");  // 사용유무
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            #endregion

        }
        #endregion AP0100_Load

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[7];

            try
            {
                DtChange.Clear();

                base.DoInquire();

                string sStartDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(calRegDT_FRH.Value));
                string sEndDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(calRegDT_TOH.Value));
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sOPCode = txtOPCode.Text.Trim();
                string sLineCode       = "";  //라인코드;                                               
                string sItemCode       = txtItemCode.Text.Trim();                                               
                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();


                param[0] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);                                               
                param[3] = helper.CreateParameter("@OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);        
                param[4] = helper.CreateParameter("@LineCode ", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);      
                param[5] = helper.CreateParameter("@ItemCode ", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
 
                /*20140409 기존 SP - 사업장통합 이전*/
                //rtnDtTemp = helper.FillTable("USP_AP0100_S1N", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_AP0100_S1N_UNION", CommandType.StoredProcedure, param);
                
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

                base.ClosePrgForm();
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
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlanNo", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StartPlanDate", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "EndPlanDate", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPCode", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPName", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Linecode", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LineName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterCode", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterName", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemName", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlanQty", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UnitCode", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlanType", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "DelivaryDate", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow); 
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
            UltraGridUtil.DeleteCurrentRowGrid(this, this.grid1, DtChange);
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

                            param = new SqlParameter[3];

                            param[0] = helper.CreateParameter("PlanNo", drRow["PlanNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // Plan No

                            param[1] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[2] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_AP0100_D1N", CommandType.StoredProcedure, param);

                            if (param[1].Value.ToString() == "E") throw new Exception(param[2].Value.ToString());
                            #endregion
                            break;
                            
                        case DataRowState.Added:
                            #region 추가

                            param = new SqlParameter[16];
                            param[0]  = helper.CreateParameter("PlanNo", drRow["PlanNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[1]  = helper.CreateParameter("StartPlanDate", drRow["StartPlanDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[2]  = helper.CreateParameter("EndPlanDate", drRow["EndPlanDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3]  = helper.CreateParameter("PlantCode", this.PlantCode, SqlDbType.VarChar, ParameterDirection.Input);  
                            param[4]  = helper.CreateParameter("OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[5]  = helper.CreateParameter("Linecode", drRow["Linecode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6]  = helper.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[7]  = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[8]  = helper.CreateParameter("PlanQty", drRow["PlanQty"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[9]  = helper.CreateParameter("UnitCode", drRow["UnitCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[10] = helper.CreateParameter("PlanType", drRow["PlanType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[11] = helper.CreateParameter("DelivaryDate", drRow["DelivaryDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[12] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[13] = helper.CreateParameter("Maker", drRow["Maker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[14] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[15] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_AP0100_I1N", CommandType.StoredProcedure, param);

                            if (param[14].Value.ToString() == "E") throw new Exception(param[15].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:

                            #region 수정
                            
                            param = new SqlParameter[16];
                            param[0]  = helper.CreateParameter("PlanNo", drRow["PlanNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[1]  = helper.CreateParameter("StartPlanDate", drRow["StartPlanDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[2]  = helper.CreateParameter("EndPlanDate", drRow["EndPlanDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3]  = helper.CreateParameter("PlantCode", this.PlantCode, SqlDbType.VarChar, ParameterDirection.Input);  
                            param[4]  = helper.CreateParameter("OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[5]  = helper.CreateParameter("Linecode", drRow["Linecode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6]  = helper.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[7]  = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[8]  = helper.CreateParameter("PlanQty", drRow["PlanQty"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[9]  = helper.CreateParameter("UnitCode", drRow["UnitCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[10] = helper.CreateParameter("PlanType", drRow["PlanType"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[11] = helper.CreateParameter("DelivaryDate", drRow["DelivaryDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[12] = helper.CreateParameter("UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[13] = helper.CreateParameter("Editor", drRow["Maker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);            // 등록자   
                            param[14] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[15] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);
          
                            helper.ExecuteNoneQuery("USP_AP0100_U1N", CommandType.StoredProcedure, param);

                            if (param[14].Value.ToString() == "E") throw new Exception(param[15].Value.ToString());

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

        private void btnERPDownLoad_Click(object sender, EventArgs e)
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;

            try
            {
                param = new SqlParameter[7];
                param[0] = helper.CreateParameter("@pProgID", "ERPDownLoader", SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@pReqMessage", "Download", SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@pParam1", "TAP0100", SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@pParam2", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@pParam3", "", SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@pParam4", "", SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@pUser", this.WorkerID, SqlDbType.VarChar, ParameterDirection.Input);  

                helper.ExecuteNoneQuery("USP_IF0100_I1", CommandType.StoredProcedure, param);

                helper.Transaction.Commit();

                this.ShowDialog("C:R00124", Windows.Forms.DialogForm.DialogType.OK); 
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
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "PlanQty" });
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            if (grid1.ActiveRow == null)
                return;

            string order_No = grid1.ActiveRow.Cells["PlanNo"].Value.ToString();
            string dd="";

            if (DateTime.Now.Hour < 8) { dd = DateTime.Now.AddDays(-1).ToString("yyMMdd"); }
            else { dd = DateTime.Now.ToString("yyMMdd"); }
                
            if (order_No.StartsWith(dd) == false)
            {
                //MessageBox.Show("오늘 작업지시서만 해제할수 있습니다");
                //return;
            }
            if (MessageBox.Show(order_No + " 완료해제 하시겠습니까?", "완료해제", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SqlDBHelper helper = new SqlDBHelper(false, false);
                SqlParameter[] param = new SqlParameter[2];

                try
                {
                    this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);
                    param[0] = helper.CreateParameter("@PlantCode", this.PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                    param[1] = helper.CreateParameter("@planno",    order_No, SqlDbType.VarChar, ParameterDirection.Input);
                    helper.ExecuteNoneQuery("USP_AP0100_U2", CommandType.StoredProcedure, param);                    

                    //helper.Transaction.Commit();
                    DoInquire();
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
        }


    }


}
