#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM0200
//   Form Name    : 작업자 마스터
//   Name Space   : SAMMI.BM
//   Created Date : 2012-03-19
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 기준정보(작업자마스터) 관리 화면
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
using SAMMI.PopUp;
using SAMMI.PopManager;
using SAMMI.Common;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{
    public partial class BM0200 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
       
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        DataTable DtChange = new DataTable();
       
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >
        public BM0200()
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

            //btbManager = new BizTextBoxManagerEX();
            //btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.PlantCode, txtWorkCenterCode, "", "" }
            //        , new string[] { "WorkCenterCode", "WorkCenterName"}, new object[] { txtWorkCenterCode, txtWorkCenterName});
            
            gridManager = new BizGridManagerEX(grid1);
            //gridManager.PopUpAdd("OPCode", "OPName", "TBM0400", new string[] { LoginInfo.UserPlantCode, "" });
            //gridManager.PopUpAdd("LineCode", "LineName", "TBM0500", new string[] { LoginInfo.UserPlantCode, "" });
            gridManager.PopUpAdd("WorkCenterCode", "WorkCenterName", "TBM0600", new string[] { "PlantCode", "", "", "" });

            GridInit();


            
        }
        #endregion

        private void BM0200_Load(object sender, EventArgs e)
        {
          
        }

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                DtChange.Clear();

                base.DoInquire();

                //string sPlantCode = "ALL";
                string sOpCode    = txtWorkCenterCode.Text.Trim();                                                          
                string sWorkerName    = txtWorkerName_H.Text.Trim();                                                            
                string sDeptCode  = SqlDBHelper.nvlString(cboDeptCode_H.Value);
                string sUseFlag = SqlDBHelper.nvlString(cboUseFlag_H.Value);

                param[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);           
                param[1] = helper.CreateParameter("@OpCode", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);            
                param[2] = helper.CreateParameter("@WorkerName", sWorkerName, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@DeptCode", sDeptCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@WorkCenterCode", txtWorkCenterCode.Text.Trim(), SqlDbType.VarChar, ParameterDirection.Input); 
                
                //rtnDtTemp = helper.FillTable("USP_BM0200_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0200_S1_UNION", CommandType.StoredProcedure, param);

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

            int iRow = _GridUtil.AddRow(this.grid1, DtChange);

            UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkerID", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkerName", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkerName2", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "PassWord", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterCode", iRow);
            UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkCenterName", iRow);// NO
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "LineCode", iRow);   // 특별특성
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "LineName", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "OPCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "OPName", iRow);// 관리규격
            UltraGridUtil.ActivationAllowEdit(this.grid1, "DeptCode", iRow);    // 측정구
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "TeamCode", iRow);       // 관리하한치
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "BanCode", iRow);       // 관리상한치
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ClassCode", iRow);       // 규격하한치 
            UltraGridUtil.ActivationAllowEdit(this.grid1, "DayNight", iRow);       // 규격상한치
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "ShiftGb", iRow);  // 검사필수여부
            UltraGridUtil.ActivationAllowEdit(this.grid1, "EmpNo", iRow);    // 검사주기(일/주/월)
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "EmpTelNo", iRow);     // 검사주기(별)
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ProdManager", iRow);    // 검사수집장비  
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MachManager", iRow);     // 검사횟수
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "InDate", iRow);   // 검사정보구분(양호/불량, OK/NOK, 확인, 값입력)
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "OutDate", iRow);      // 표시순서
            UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);       // 사용유무  
            UltraGridUtil.ActivationAllowEdit(this.grid1, "ForeignYN", iRow);       // 사용유무  
            UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);      // 표시순서
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);      // 표시순서
            UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);      // 표시순서
            UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);      // 표시순서
            //(this.PlantCode == "") ? true : false
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
                string sPlantCode = "";
                this.grid1.Focus();

                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" || SqlDBHelper.nvlString(dr["WorkerID"]) == "")
                            {
                                ShowDialog("사업장, 작업자 ID는 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

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
                        case DataRowState.Modified:
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" || SqlDBHelper.nvlString(dr["WorkerID"]) == "")
                            {
                                ShowDialog("사업장, 작업자 ID는 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

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

                            param = new SqlParameter[4];

                            sPlantCode = SqlDBHelper.nvlString(drRow["PlantCode"]);

                            param[0] = helper.CreateParameter("@WorkerID", SqlDBHelper.nvlString(drRow["WorkerID"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[1] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[3] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);


                            helper.ExecuteNoneQuery("USP_BM0200_D1", CommandType.StoredProcedure, param);

                            if (param[2].Value.ToString() == "E") throw new Exception(param[3].Value.ToString());
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가 - 통합완료
                            param = new SqlParameter[14];

                            param[0] = helper.CreateParameter("@WorkerID", SqlDBHelper.nvlString(drRow["WorkerID"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkerName", SqlDBHelper.nvlString(drRow["WorkerName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@DeptCode", SqlDBHelper.nvlString(drRow["DeptCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@ClassCode", SqlDBHelper.nvlString(drRow["ClassCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@DayNight", SqlDBHelper.nvlString(drRow["DayNight"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@EmpNo", SqlDBHelper.nvlString(drRow["EmpNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@ProdManager", SqlDBHelper.nvlString(drRow["ProdManager"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@MachManager", SqlDBHelper.nvlString(drRow["MachManager"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@UseFlag", SqlDBHelper.gGetCode(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@Maker", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            param[12] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[13] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM0200_I1", CommandType.StoredProcedure, param);

                            if (param[12].Value.ToString() == "E") throw new Exception(param[13].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정 - 통합완료
                            param = new SqlParameter[15];


                            param[0] = helper.CreateParameter("@WorkerID", SqlDBHelper.nvlString(drRow["WorkerID"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkerName", SqlDBHelper.nvlString(drRow["WorkerName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@DeptCode", SqlDBHelper.nvlString(drRow["DeptCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@ClassCode", SqlDBHelper.nvlString(drRow["ClassCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@DayNight", SqlDBHelper.nvlString(drRow["DayNight"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@EmpNo", SqlDBHelper.nvlString(drRow["EmpNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@ProdManager", SqlDBHelper.nvlString(drRow["ProdManager"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@MachManager", SqlDBHelper.nvlString(drRow["MachManager"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@ForeignYN", drRow["ForeignYN"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("@Editor", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            param[13] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[14] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM0200_U1", CommandType.StoredProcedure, param);

                            if (param[13].Value.ToString() == "E") throw new Exception(param[14].Value.ToString());

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

        #region <METHOD AREA>
       
        private void GridInit()
        {
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth.Equals("")) ? true : false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerID", "작업자ID", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerName", "작업자명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "WorkerName2", "작업자명(현장)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "PassWord", "비밀번호", false, GridColDataType_emu.VarChar, 167, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "LineCode", "라인코드", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DeptCode", "부서코드", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "TeamCode", "팀코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "BanCode", "반코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ClassCode", "고용형태", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DayNight", "주야구분", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ShiftGb", "조구분", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EmpNo", "사번", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "EmpTelNo", "비상연락", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdManager", "생산담당자", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachManager", "설비보전 담당자", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "InDate", "입사일", false, GridColDataType_emu.YearMonthDay, 120, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "OutDate", "퇴사일", false, GridColDataType_emu.YearMonthDay, 120, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ForeignYN", "외국인여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);


            _GridUtil.SetInitUltraGridBind(grid1);
             DtChange = (DataTable)grid1.DataSource;

            // row numbering
             grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
             grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            grid1.DisplayLayout.UseFixedHeaders = true;
            //for (int i = 0; i < 5; i++)
            //    grid1.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;

            #region 콤보박스
            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("ClassCode");  //고용형태
            //SAMMI.Common.Common.FillComboboxMaster(this.cboClassCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ClassCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("DeptCode");  
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DeptCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("DAYNIGHT"); 
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DayNight", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ProdManager", rtnDtTemp, "CODE_ID", "CODE_NAME");

            //rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MachManager", rtnDtTemp, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ForeignYN", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("DEPTCODE"); //
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "TeamCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "BanCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
  
            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion
        }

        #endregion

        private void gbxHeader_Click(object sender, EventArgs e)
        {

        }

    }
}
