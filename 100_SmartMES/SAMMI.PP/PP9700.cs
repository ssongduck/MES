using SAMMI.Common;
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using Infragistics.Win.UltraWinGrid;
using SAMMI.PopUp;
using SAMMI.PopManager;
using System.Data.Common;

namespace SAMMI.PP
{
    public partial class PP9700 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>

        // 변수나 Form에서 사용될 Class를 정의
        private string PlantCode = string.Empty;
        private string WorkCenterCode = string.Empty;

        DataTable _rtnDtTemp = new DataTable();

        DataTable _GridTable = new DataTable();     //그리드 컬럼 리네임에 사용할 데이터테이블

        BizTextBoxManagerEX btbManager;
        UltraGridUtil _GridUtil = new UltraGridUtil();
        UltraGridUtil _GridUtil2 = new UltraGridUtil();
        private DateTime FRDT = System.DateTime.Now;
        private DateTime TODT = System.DateTime.Now;
        Common.Common _Common = new Common.Common();

        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        private DataTable DtChange = null;

        private int _Fix_Col = 0;
        private int data01 = 0;

        #endregion

        public PP9700()
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

            this.cboStartDate_H.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00");
            this.cboEndDate_H.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 23:59:59");

            // 사업장 사용권한 설정
            //_Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            GridIni();

            #region <POPUP>
            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            #endregion
        }

        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                base.DoInquire();

                DateTime planstartdt = Convert.ToDateTime(((DateTime)this.cboStartDate_H.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");
                DateTime planenddt = Convert.ToDateTime(((DateTime)this.cboEndDate_H.Value).ToString("yyyy-MM-dd") + " 23:59:59.99");
                string DayNight = SqlDBHelper.nvlString(this.cboDayNight.Value);

                if (Convert.ToInt32(planstartdt.ToString("yyyyMMdd")) > Convert.ToInt32(planenddt.ToString("yyyMMdd")))
                {
                    SException ex = new SException("R00200", null);
                    throw ex;
                }

                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();

                this.PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                this.WorkCenterCode = sWorkCenterCode;
                this.FRDT = planstartdt;
                this.TODT = planenddt;
                string sFRDT = FRDT.ToString("yyyy-MM-dd");
                string sTODT = TODT.ToString("yyyy-MM-dd");

                param[0] = helper.CreateParameter("@PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@WorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@ItemCode", this.txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@FrDate", sFRDT, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@ToDate", sTODT, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@DayNight", DayNight, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_PP9700_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP9700_S1_UNION", CommandType.StoredProcedure, param);
                if (rtnDtTemp.Rows.Count == 0)
                {
                    // MessageBox.Show("DATA가 없습니다.");
                    grid1.DataSource = rtnDtTemp;
                    grid1.DataBind();
                }
                else
                {
                    grid1.DataSource = rtnDtTemp;
                    grid1.DataBind();

                    DtChange = rtnDtTemp;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
                grid1.DisplayLayout.CaptionAppearance.BackColor = Color.White;
            }
        }

        private void GridIni()
        {
            #region <Grid1 Setting>
            _GridUtil.InitializeGrid(this.grid1);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "WorkCenterCode", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CarType", "차종", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품목코드", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "작업일자", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DayNight", "주야", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SEQ", "순번", false, GridColDataType_emu.VarChar, 50, 30, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CheckSEQ", "확인순번", false, GridColDataType_emu.VarChar, 70, 30, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일시", false, GridColDataType_emu.DateTime24, 150, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ChangeType", "유형", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Message", "요청내용", false, GridColDataType_emu.VarChar, 200, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MessageSendType", "SMS유형", false, GridColDataType_emu.VarChar, 80, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CHECKTYPE", "이상확인", false, GridColDataType_emu.VarChar, 70, 30, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "comfirmMsg", "확인내용", false, GridColDataType_emu.VarChar, 200, 30, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "comfirmer", "확인자", false, GridColDataType_emu.VarChar, 70, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "comfirmDate", "확인일시", false, GridColDataType_emu.DateTime24, 150, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
           
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            
            //grid1.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.White;
            //grid1.UseAppStyling = false;
            _GridUtil.SetInitUltraGridBind(this.grid1);

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("ERRORCOMFIRM");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "CHECKTYPE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");
            #endregion
        }
        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;

            try
            {
                string sPlantCode = "";
                this.Focus();

                //foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                //{
                //    switch (dr.RowState)
                //    {
                //        case DataRowState.Added:
                //        case DataRowState.Modified:
                //            // Validate 체크
                //            //SqlDBHelper.gGetCode(drRow["PlantCode"]);
                //            //if (SqlDBHelper.nvlString(dr["PlantCode"]) == "")
                //            if (LoginInfo.UserPlantCode == "" || SqlDBHelper.nvlString(dr["WorkerID"]) == "")
                //            {
                //                ShowDialog("작업자 ID는 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

                //                CancelProcess = true;
                //                return;
                //            }

                //            break;
                //    }
                //}

                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                string sUseFlag;

                foreach (DataRow drRow in DtChange.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            //#region 삭제
                            //drRow.RejectChanges();

                            //param = new SqlParameter[4];

                            //sPlantCode = LoginInfo.UserPlantCode;

                            //param[0] = helper.CreateParameter("@WorkerID", SqlDBHelper.nvlString(drRow["WorkerID"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            //param[1] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            //param[2] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            //param[3] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);


                            //helper.ExecuteNoneQuery("USP_BM0200_D1", CommandType.StoredProcedure, param);

                            //if (param[2].Value.ToString() == "E") throw new Exception(param[3].Value.ToString());
                            //#endregion
                            break;
                        case DataRowState.Added:
                            #region 추가
                            //param = new SqlParameter[14];

                            //sPlantCode = LoginInfo.UserPlantCode;
                            //sUseFlag = SqlDBHelper.gGetCode(drRow["UseFlag"]);
                            //sUseFlag = sUseFlag == "" ? "Y" : sUseFlag;

                            //param[0] = helper.CreateParameter("@WorkerID", SqlDBHelper.nvlString(drRow["WorkerID"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[1] = helper.CreateParameter("@WorkerName", SqlDBHelper.nvlString(drRow["WorkerName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            ////param[2] = helper.CreateParameter("@WorkerName2",SqlDBHelper.nvlString(drRow["WorkerName2"]), SqlDbType.VarChar, ParameterDirection.Input);    
                            ////param[3] = helper.CreateParameter("@PassWord", SqlDBHelper.nvlString(drRow["PassWord"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[2] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            //param[3] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            ////param[6] = helper.CreateParameter("@LineCode", SqlDBHelper.nvlString(drRow["LineCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            ////param[7] = helper.CreateParameter("@OPCode", SqlDBHelper.nvlString(drRow["OPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[4] = helper.CreateParameter("@DeptCode", SqlDBHelper.nvlString(drRow["DeptCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            ////param[9] = helper.CreateParameter("@TeamCode", SqlDBHelper.nvlString(drRow["TeamCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            ////param[10] = helper.CreateParameter("@BanCode", SqlDBHelper.nvlString(drRow["BanCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[5] = helper.CreateParameter("@ClassCode", SqlDBHelper.nvlString(drRow["ClassCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[6] = helper.CreateParameter("@DayNight", SqlDBHelper.nvlString(drRow["DayNight"]), SqlDbType.VarChar, ParameterDirection.Input);
                            ////param[13] = helper.CreateParameter("@ShiftGb", SqlDBHelper.nvlString(drRow["ShiftGb"]), SqlDbType.VarChar, ParameterDirection.Input);   
                            //param[7] = helper.CreateParameter("@EmpNo", SqlDBHelper.nvlString(drRow["EmpNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            ////param[15] = helper.CreateParameter("@EmpTelNo", SqlDBHelper.nvlString(drRow["EmpTelNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[8] = helper.CreateParameter("@ProdManager", SqlDBHelper.nvlString(drRow["ProdManager"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[9] = helper.CreateParameter("@MachManager", SqlDBHelper.nvlString(drRow["MachManager"]), SqlDbType.VarChar, ParameterDirection.Input);
                            ////param[18] = helper.CreateParameter("@InDate", SqlDBHelper.nvlString(drRow["InDate"]), SqlDbType.VarChar, ParameterDirection.Input);     
                            ////param[19] = helper.CreateParameter("@OutDate", SqlDBHelper.nvlString(drRow["OutDate"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[10] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                            //param[11] = helper.CreateParameter("@Maker", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            //param[12] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            //param[13] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            //helper.ExecuteNoneQuery("USP_BM0200_I1", CommandType.StoredProcedure, param);

                            //if (param[12].Value.ToString() == "E") throw new Exception(param[13].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:

                            #region 수정
                            param = new SqlParameter[9];


                            sPlantCode = LoginInfo.UserPlantCode;
                            //sUseFlag = SqlDBHelper.gGetCode(drRow["UseFlag"]);
                            //sUseFlag = sUseFlag == "" ? "Y" : sUseFlag;

                            //param[0] = helper.CreateParameter("@WorkerID", SqlDBHelper.nvlString(drRow["WorkerID"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[1] = helper.CreateParameter("@WorkerName", SqlDBHelper.nvlString(drRow["WorkerName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            //param[2] = helper.CreateParameter("@WorkerName2",SqlDBHelper.nvlString(drRow["WorkerName2"]), SqlDbType.VarChar, ParameterDirection.Input);    
                            //param[3] = helper.CreateParameter("@PassWord", SqlDBHelper.nvlString(drRow["PassWord"]), SqlDbType.VarChar, ParameterDirection.Input);
                          
                            
                            param[0] = helper.CreateParameter("@pPlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@pWorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@pItemCode", SqlDBHelper.nvlString(drRow["ItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@pRecDate", SqlDBHelper.nvlString(drRow["RecDate"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@pDayNight", SqlDBHelper.nvlString(drRow["DayNight"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@pSEQ", SqlDBHelper.nvlString(drRow["SEQ"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@pCheckType", SqlDBHelper.nvlString(drRow["CHECKTYPE"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@pMessage", SqlDBHelper.nvlString(drRow["comfirmMsg"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@pMaker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            helper.ExecuteNoneQuery("USP_DA0130_I2", CommandType.StoredProcedure, param);

                           

                            #endregion

                            break;
                    }
                }

                helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                CancelProcess = true;
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
