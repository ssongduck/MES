using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;

using SAMMI.PopUp;
using SAMMI.PopManager;
using SAMMI.Windows.Forms;
using SAMMI.Common;

namespace SAMMI.CM
{
    /// <summary>
    /// CM0200 class [설비 고장 및 수리 등록]
    /// </summary>
    public partial class CM0200 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variables

        DataSet rtnDsTemp = new DataSet();

        DataTable rtnDtTemp = new DataTable();

        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;

        string CHK = string.Empty;

        PopUp_Biz _biz = new PopUp_Biz();

        BizTextBoxManagerEX btbManager;
        
        DataTable _DtTemp = new DataTable();

        DataTable CboDtTemp = new DataTable();

        int currow = -1;

        string[] sEmptyArr = { "v_txt" };

        private string PlantCode = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// CM0200 constructor
        /// </summary>
        public CM0200()
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
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { cboPlantCode_H, null, "", "" });
                btbManager.PopUpAdd(txtMachCode, txtMachName, "TBM0700", new object[] { cboPlantCode_H, "", "", "", "" });
                btbManager.PopUpAdd(v_txtMachCode, v_txtMachName, "TBM0700", new object[] { cboPlantCode_H, "", "", "", "" });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, null, "", "" });
                btbManager.PopUpAdd(txtMachCode, txtMachName, "TBM0700", new object[] { LoginInfo.PlantAuth, "", "", "", "" });
                btbManager.PopUpAdd(v_txtMachCode, v_txtMachName, "TBM0700", new object[] { LoginInfo.PlantAuth, "", "", "", "" });
            }
            btbManager.PopUpAdd(txtMCType, txtMCTypeName, "TCM0200", new string[] { "1", "설비종류" }, null, new object[] { v_txtWorkCenterCode });
            btbManager.PopUpAdd(txtReason, txtReasonName, "TCM0200", new string[] { "3", "고장원인" });
            btbManager.PopUpAdd(txtWorkerID, txtWorkName, "TBM0200", new string[] { "", "", "", "", "", "1" });
            btbManager.PopUpAdd(txtRepaircode, txtRepairname, "TCM0200", new string[] { "4", "수리유형" });
            btbManager.PopUpAdd(txtFaultcode, txtFaultname, "TBM3400", new object[] { cboFAULTTYPE, "" });

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
            SqlParameter[] param = new SqlParameter[9];
            ClearAll();

            try
            {
                DtChange.Clear();
               
                base.DoInquire();

                string sPlantCode  = SqlDBHelper.nvlString(cboPlantCode_H.Value);  // 공장코드 
                string sMachCode   = txtMachCode.Text.Trim();  
                string sWMachName  = txtMachName.Text.Trim();  
                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();
                string sWorkCenterName = txtWorkCenterName.Text.Trim();

                string sDtp_date   = string.Format("{0:yyyy-MM-dd}", cbo_date.Value);
                string sDtp_date_to   = string.Format("{0:yyyy-MM-dd}", cbo_dateto.Value);
               
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);           
                param[1] = helper.CreateParameter("@MachCode", sMachCode, SqlDbType.VarChar, ParameterDirection.Input);            
                param[2] = helper.CreateParameter("@Machname", sWMachName, SqlDbType.VarChar, ParameterDirection.Input);       
                param[3] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);       
                param[4] = helper.CreateParameter("@WorkCenterName", sWorkCenterName, SqlDbType.VarChar, ParameterDirection.Input);       
                param[5] = helper.CreateParameter("@Dtp_date", sDtp_date, SqlDbType.VarChar, ParameterDirection.Input);       
                param[6] = helper.CreateParameter("@Dtp_date_to", sDtp_date_to, SqlDbType.VarChar, ParameterDirection.Input);       
                
                param[7] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                param[8] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                rtnDtTemp = helper.FillTable("USP_CM0200_S1_UNION", CommandType.StoredProcedure, param);
                


                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;
              
            }

            catch (SqlException )
            {
                
            }
            finally
            {
                  CHK = "";
            }

        }

        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            base.DoNew();

            currow = _GridUtil.AddRow(this.grid1, DtChange);
            grid1.Rows[currow].Cells["WorkCenterCode"].Activate();

            grid1_ClickCell(null, null);
        }

        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();
            this.grid1.DeleteRow();
            CHK = "DELETE";
                     
          

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
                //base.DoSave();
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
                            param[0] = helper.CreateParameter("@SeqID", drRow["seqId"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@PlantCode", drRow["PLANTCODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 10);
                            param[3] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 100);

                            helper.ExecuteNoneQuery("USP_CM0200_D2N", CommandType.StoredProcedure, param);
                            if (param[2].Value.ToString() == "E") throw new Exception(param[3].Value.ToString());

                            #endregion
                            break;

                        case DataRowState.Added:
                            #region 추가
                            if (drRow["WorkCenterCode"].ToString() == "")
                            {
                                MessageBox.Show("작업장은 필수정보입니다.");
                                continue;
                            }
                            if (drRow["StartDate"].ToString().StartsWith(" "))
                            {
                                MessageBox.Show("신고시간은 필수정보입니다.");

                                continue;
                            }
                            if (drRow["MachCode"].ToString() == "")
                            {
                                MessageBox.Show("설비는 필수정보입니다.");

                                continue;
                            }
                            param = new SqlParameter[19];

                            param[0] = helper.CreateParameter("@PlantCode", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PlantCode"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@MachCode", drRow["MachCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@StartDate", drRow["StartDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@FaultStartDate", drRow["FaultStartDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@EndDate", drRow["EndDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@MachFaultCode", drRow["MachFaultCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@MaStartTime", drRow["MaStartTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@MaEndTime", drRow["MaEndTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@Madesc", drRow["Madesc"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@Worker", drRow["Worker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@Faultcode", drRow["Faultcode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("@Repaircode", drRow["MACODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[13] = helper.CreateParameter("@Maker", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[14] = helper.CreateParameter("@ReasonCode", drRow["ReasonCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[15] = helper.CreateParameter("@Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[16] = helper.CreateParameter("@WorkerName", drRow["WorkerName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);

                            param[17] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[18] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_CM0200_I1", CommandType.StoredProcedure, param);
                            #endregion
                            break;

                        case DataRowState.Modified:
                            #region 수정
                            //param = new SqlParameter[30];

                            param = new SqlParameter[20];

                            param[0] = helper.CreateParameter("@PlantCode", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PlantCode"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@MachCode", drRow["MachCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@StartDate", drRow["StartDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@FaultStartDate", drRow["FaultStartDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@EndDate", drRow["EndDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@MachFaultCode", drRow["MachFaultCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@MaStartTime", drRow["MaStartTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@MaEndTime", drRow["MaEndTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@Madesc", drRow["Madesc"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@Worker", drRow["Worker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@Faultcode", drRow["Faultcode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("@Repaircode", drRow["MACODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[13] = helper.CreateParameter("@Maker", SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[14] = helper.CreateParameter("@ReasonCode", drRow["ReasonCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[15] = helper.CreateParameter("@Remark", drRow["Remark"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[16] = helper.CreateParameter("@WorkerName", drRow["WorkerName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[17] = helper.CreateParameter("@SeqID", drRow["SeqID"].ToString(), SqlDbType.Int, ParameterDirection.Input);

                            param[18] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[19] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_CM0200_U2", CommandType.StoredProcedure, param);

                            if (param[18].Value.ToString() == "E")
                            {
                                throw new Exception(param[19].Value.ToString());
                            }
                            #endregion
                            break;
                    }
                }

                //helper.Transaction.Commit();
                ClearAll();             


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
               // ClearAll();
                CHK = string.Empty;
               
            }
        }

        #endregion

        #region < EVENT AREA >

        public override void DoToolBarClick(string key)
        {
            try
            {
                if (key == "SaveFunc")
                    btn_add_Click(null, null);
            }
            catch { }
            base.DoToolBarClick(key);
        }

        private void CM0200_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern


            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachCode", "설비코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Machname", "설비명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultDate", "고장발생일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StartDate", "고장신고일시", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FAULTFLAGNAME", "요청유형", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultStartDate", "고장시작시간", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EndDate", "고장종료시간", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultTime", "소요시간(분)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultCode", "고장코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FAULTNAME", "고장명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachFaultCode", "설비코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachFaultName", "설비종류명", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ReasonCode", "고장사유코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Reasonname", "고장원인명", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAStartTime", "MA시작시간", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAEndTime", "MA종료시간", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "maTime", "MA소요시간(분)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MACODE", "수리코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "REPAIRNAME", "수리명", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MADesc", "MA내역", false, GridColDataType_emu.VarChar, 250, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.InitColumnUltraGrid(grid1, "FaultType", "검사구분", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ArrivalTime", "보전자도착시간", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Worker", "보전원ID", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerName", "보전원명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "MA등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "MA등록일시", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "crud", "CRUD", false, GridColDataType_emu.VarChar, 10, 10, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "seqId", "seqId", false, GridColDataType_emu.VarChar, 10, 10, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);

            grid1.DisplayLayout.UseFixedHeaders = true;
            for (int i = 0; i < 7; i++)
                grid1.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;

            #region Grid MERGE
            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            #endregion Grid MERGE

            #endregion

            DtChange = (DataTable)grid1.DataSource;

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            //SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);

            rtnDtTemp = _Common.GET_TBM0000_CODE("FAULTTYPE");
            SAMMI.Common.Common.FillComboboxMaster(this.cboFAULTTYPE, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "FaultType", rtnDtTemp, "CODE_ID", "CODE_NAME");
            #endregion
        }

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
                    e.Row.RowError = "설비코드가 있습니다.";
                    throw (new SException("C:S00099", e.Errors));
                default:
                    break;
            }
        }


        #endregion

        #region <METHOD AREA>

        private void InitControl(System.Windows.Forms.Control con)
        {
            if (con == null)
                return;

            foreach (System.Windows.Forms.Control c in con.Controls)
            {
                InitControl(c);

                // 초기화 코드
                if (c.GetType().Name == "TextBox")
                {
                    TextBox ul = (TextBox)c;

                    foreach (string s in sEmptyArr)
                    {
                        if (ul.Name.StartsWith(s))
                        {
                            ul.Text = "";
                        }
                    }
                }
                if (c.GetType().Name == "MaskedTextBox")
                {
                    MaskedTextBox ul = (MaskedTextBox)c;

                    foreach (string s in sEmptyArr)
                    {
                        if (ul.Name.StartsWith(s))
                        {
                            ul.Text = "";
                        }
                    }
                }
            }

            return;
        }

        private void ClearAll()
        {
            InitControl(this);

            cboFAULTTYPE.SelectedValue = string.Empty;
        }

        #region 텍스트 박스에서 팝업창에서 값 가져오기 (라인)
/*         private void Search_Pop_TBM0600()
        {

            string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.SelectedValue);                            //사업장코드
            string sOPCode = "";                       //공정코드
            //string sOPName = "";                       //공정명 
            string sLineCode = string.Empty;                              //라인코드
            string sWorkcenterCode = txtWorkCenterCode.Text.Trim();       //작업호기(라인)코드
            string sWorkCenterName = txtWorkCenterName.Text.Trim();       //작업호기(라인)명 
            string sUseFlag = "Y";                               //사용여부         

            try
            {
                _biz.TBM0600_POP(sPlantCode, sWorkcenterCode, sWorkCenterName, sOPCode, sLineCode, sUseFlag, txtWorkCenterCode, txtWorkCenterName);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }

         private void txtWorkCenterCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0600();
        }

        private void txtWorkCenterName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0600();
        }

        private void txtWorkCenterCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkCenterName.Text = string.Empty;
        }

        private void txtWorkCenterCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                Search_Pop_TBM0600();
            }
        }

        private void txtWorkCenterName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkCenterCode.Text = string.Empty;
        }

        private void txtWorkCenterName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                Search_Pop_TBM0600();
            }
        }
         #endregion        //라인

         #region 설비정보
        private void Search_Pop_TBM0700()
        {

            string sMachCode = txtMachCode.Text.Trim();       //설비코드
            string sMachName = txtMachName.Text.Trim();      //설비명 


            try
            {
                //_biz.TBM0700_POP(sMachCode, sMachName, "", "", "", "", txtMachCode, txtMachName);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }
       

        private void txtMachCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtMachName.Text = string.Empty;
        }

        private void txtMachCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0700();
            }
        }

        private void txtMachCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0700();
        }

        private void txtMachName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtMachCode.Text = string.Empty;
        }

        private void txtMachName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0700();
            }
        }

        private void txtMachName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0700();
        }

     
        private void grid1_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {

            if (grid1.Rows.Count > 0)
            {
                if (CHK == "DELETE")
                {
                    DialogForm dialogform;
                    dialogform = new DialogForm("이미 삭제된 데이터가 있습니다. 저장하시고 수정하여 주십시오.",DialogForm.DialogType.OK);
                    dialogform.ShowDialog();
                    return;

                }

                //if (grid1.ActiveRow.Cells["EndDate"].Value.ToString() == string.Empty)
                //{
                //    DialogForm dialogform;
                //    dialogform = new DialogForm("아직 설비 수리가 완료되지 않았습니다.", DialogForm.DialogType.OK);
                //    dialogform.ShowDialog();
                //    ClearAll();

                //    return;
                   
                //}
                //else
                {
                   if (e !=null)
                      currow = e.Cell.Row.Index;
                   v_txtplantcode.Text = grid1.ActiveRow.Cells["PLANTCODE"].Value.ToString(); 
                   v_txtWorkCenterCode.Text = grid1.ActiveRow.Cells["WorkCenterCode"].Value.ToString(); 
                   v_txtWorkCenterName.Text = grid1.ActiveRow.Cells["WorkCenterName"].Value.ToString(); 
                   v_txtMachCode.Text = grid1.ActiveRow.Cells["MachCode"].Value.ToString(); 
                   v_txtMachName.Text = grid1.ActiveRow.Cells["MachName"].Value.ToString(); 
                   v_txtFaultDate.Text = grid1.ActiveRow.Cells["FaultDate"].Value.ToString(); 
                   v_txtStartDate.Text = grid1.ActiveRow.Cells["StartDate"].Value.ToString(); 
                   txtFaultstartDate.Text = grid1.ActiveRow.Cells["FaultStartDate"].Value.ToString(); 
                   txtEndDate.Text = grid1.ActiveRow.Cells["EndDate"].Value.ToString(); 
                   txtFaulttime.Text = grid1.ActiveRow.Cells["FaultTime"].Value.ToString(); 
                   txtFaultcode.Text = grid1.ActiveRow.Cells["FaultCode"].Value.ToString(); 
                   v_txtArrivalTime.Text = grid1.ActiveRow.Cells["ArrivalTime"].Value.ToString(); 
                   txtMaStartTime.Text = grid1.ActiveRow.Cells["MAStartTime"].Value.ToString(); 
                   txtMaEndtime.Text = grid1.ActiveRow.Cells["MAEndTime"].Value.ToString(); 
                   txtMaDesc.Text = grid1.ActiveRow.Cells["MaDesc"].Value.ToString();
                   txtWorkerID.Text = grid1.ActiveRow.Cells["WORKER"].Value.ToString();
                   txtWorkName.Text = grid1.ActiveRow.Cells["WorkerName"].Value.ToString();
                   txtFaultname.Text = grid1.ActiveRow.Cells["FAULTNAME"].Value.ToString(); 
                   //cboFAULTTYPE.SelectedValue = grid1.ActiveRow.Cells["FaultType"].Value.ToString();
                   txtRepaircode.Text = grid1.ActiveRow.Cells["MACODE"].Value.ToString();
                   txtRepairname.Text = grid1.ActiveRow.Cells["REPAIRNAME"].Value.ToString();
                   txtMCType.Text = grid1.ActiveRow.Cells["MachFaultCode"].Value.ToString();
                   txtMCTypeName.Text = grid1.ActiveRow.Cells["MachFaultNAME"].Value.ToString();
                   txtReason.Text = grid1.ActiveRow.Cells["ReasonCode"].Value.ToString();
                   txtReasonName.Text = grid1.ActiveRow.Cells["Reasonname"].Value.ToString();
                   txtRemark.Text = grid1.ActiveRow.Cells["Remark"].Value.ToString();
                }
               

            }




        }

        private void txtMachCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblMachCode_Click(object sender, EventArgs e)
        {

        }

        private void txtMachName_TextChanged(object sender, EventArgs e)
        {

        }
        */
        private void txtEndDate_Leave(object sender, EventArgs e)
        {
            if (txtEndDate.Text == "")
                return;
            try
            {
                TimeSpan tmDiff = Convert.ToDateTime(txtEndDate.Text) - Convert.ToDateTime(txtFaultstartDate.Text);

                if (tmDiff.Seconds < 0)
                {
                    MessageBox.Show("고장신고시간 보다 이전시간을 입력할수 없습니다'");
                    txtEndDate.Focus();
                }
                txtFaulttime.Text = (tmDiff.Days*24*60+tmDiff.Hours * 60 + tmDiff.Minutes + (tmDiff.Seconds > 0 ? 1 : 0)).ToString();
            }
            catch { }
        }

        private void txtMaStartTime_Leave(object sender, EventArgs e)
        {
            if (txtMaStartTime.Text == "")
                return;
            try
            {
                TimeSpan tmDiff = Convert.ToDateTime(txtMaStartTime.Text) - Convert.ToDateTime(txtFaultstartDate.Text);

                if (tmDiff.Seconds < 0)
                {
                    MessageBox.Show("고장신고시간 보다 이전시간을 입력할수 없습니다'");
                    txtMaStartTime.Focus();
                }
            }
            catch { }

        }

        private void txtMaEndtime_Leave(object sender, EventArgs e)
        {
            if (txtMaEndtime.Text == "")
                return;
            try
            {
                TimeSpan tmDiff = Convert.ToDateTime(txtMaEndtime.Text) - Convert.ToDateTime(txtMaStartTime.Text);

                if (tmDiff.Seconds < 0)
                {
                    MessageBox.Show("수리시작시간 보다 이전시간을 입력할수 없습니다'");
                    txtMaEndtime.Focus();

                }
                txtMatime.Text = (tmDiff.Days * 24 * 60 + tmDiff.Hours * 60 + tmDiff.Minutes + (tmDiff.Seconds > 0 ? 1 : 0)).ToString();
 
            }
            catch { }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {

            if ( v_txtWorkCenterCode.Text == "")
            {
                MessageBox.Show("작업장은 필수정보입니다.");
                v_txtWorkCenterCode.Focus();
                return;
            }
            if ( v_txtMachCode.Text == "")
            {
                MessageBox.Show("설비는 필수정보입니다.");
                v_txtMachCode.Focus();
                return;
            }
            if ( txtFaultstartDate.Text.StartsWith(" "))
            {
                MessageBox.Show("고장시간은 필수정보입니다.");
                txtFaultstartDate.Focus();
                return;
            }


            grid1.Rows[currow].Cells["PLANTCODE"].Value = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            grid1.Rows[currow].Cells["WorkCenterCode"].Value = v_txtWorkCenterCode.Text;
            grid1.Rows[currow].Cells["WorkCenterName"].Value = v_txtWorkCenterName.Text;
            grid1.Rows[currow].Cells["MachCode"].Value = v_txtMachCode.Text;
            grid1.Rows[currow].Cells["MachName"].Value = v_txtMachName.Text;
            grid1.Rows[currow].Cells["FaultDate"].Value = v_txtFaultDate.Text;
            grid1.Rows[currow].Cells["StartDate"].Value = txtFaultstartDate.Text;
            grid1.Rows[currow].Cells["FaultStartDate"].Value = txtFaultstartDate.Text;

            grid1.Rows[currow].Cells["EndDate"].Value = txtEndDate.Text;
            grid1.Rows[currow].Cells["FaultTime"].Value = txtFaulttime.Text;
            grid1.Rows[currow].Cells["FaultCode"].Value = txtFaultcode.Text;
            grid1.Rows[currow].Cells["ArrivalTime"].Value = txtMaStartTime.Text;
            grid1.Rows[currow].Cells["MAStartTime"].Value = txtMaStartTime.Text;
            grid1.Rows[currow].Cells["MAEndTime"].Value = txtMaEndtime.Text;

            grid1.Rows[currow].Cells["MaDesc"].Value = txtMaDesc.Text;
            grid1.Rows[currow].Cells["WORKER"].Value = txtWorkerID.Text;
            grid1.Rows[currow].Cells["WorkerName"].Value = txtWorkName.Text;
            grid1.Rows[currow].Cells["FAULTNAME"].Value = txtFaultname.Text;
            grid1.Rows[currow].Cells["MACODE"].Value = txtRepaircode.Text;
            grid1.Rows[currow].Cells["REPAIRNAME"].Value = txtRepairname.Text;
            grid1.Rows[currow].Cells["MachFaultCode"].Value = txtMCType.Text;
            grid1.Rows[currow].Cells["MachFaultNAME"].Value = txtMCTypeName.Text;
            grid1.Rows[currow].Cells["ReasonCode"].Value = txtReason.Text;
            grid1.Rows[currow].Cells["Reasonname"].Value = txtReasonName.Text;
            grid1.Rows[currow].Cells["Remark"].Value = txtRemark.Text;
            grid1.Rows[currow].Cells["maTime"].Value = txtMatime.Text;
            
            grid1.Rows[currow].Update();
        }

        private void sLabel14_Click(object sender, EventArgs e)
        {

        }

        private void txtMCType_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMCTypeName_TextChanged(object sender, EventArgs e)
        {

        }

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        private void grid1_ClickCell(object sender, ClickCellEventArgs e)
        {
            if (grid1.Rows.Count > 0)
            {
                if (CHK == "DELETE")
                {
                    DialogForm dialogform;
                    dialogform = new DialogForm("이미 삭제된 데이터가 있습니다. 저장하시고 수정하여 주십시오.", DialogForm.DialogType.OK);
                    dialogform.ShowDialog();
                    return;

                }

                //if (grid1.ActiveRow.Cells["EndDate"].Value.ToString() == string.Empty)
                //{
                //    DialogForm dialogform;
                //    dialogform = new DialogForm("아직 설비 수리가 완료되지 않았습니다.", DialogForm.DialogType.OK);
                //    dialogform.ShowDialog();
                //    ClearAll();

                //    return;

                //}
                //else
                {
                    if (e != null)
                        currow = e.Cell.Row.Index;
                    v_txtplantcode.Text = grid1.ActiveRow.Cells["PLANTCODE"].Value.ToString();
                    v_txtWorkCenterCode.Text = grid1.ActiveRow.Cells["WorkCenterCode"].Value.ToString();
                    v_txtWorkCenterName.Text = grid1.ActiveRow.Cells["WorkCenterName"].Value.ToString();
                    v_txtMachCode.Text = grid1.ActiveRow.Cells["MachCode"].Value.ToString();
                    v_txtMachName.Text = grid1.ActiveRow.Cells["MachName"].Value.ToString();
                    v_txtFaultDate.Text = grid1.ActiveRow.Cells["FaultDate"].Value.ToString();
                    v_txtStartDate.Text = grid1.ActiveRow.Cells["StartDate"].Value.ToString();
                    txtFaultstartDate.Text = grid1.ActiveRow.Cells["StartDate"].Value.ToString();
                    txtEndDate.Text = grid1.ActiveRow.Cells["EndDate"].Value.ToString();
                    txtFaulttime.Text = grid1.ActiveRow.Cells["FaultTime"].Value.ToString();
                    txtFaultcode.Text = grid1.ActiveRow.Cells["FaultCode"].Value.ToString();
                    v_txtArrivalTime.Text = grid1.ActiveRow.Cells["ArrivalTime"].Value.ToString();
                    txtMaStartTime.Text = grid1.ActiveRow.Cells["MAStartTime"].Value.ToString();
                    txtMaEndtime.Text = grid1.ActiveRow.Cells["MAEndTime"].Value.ToString();
                    txtMaDesc.Text = grid1.ActiveRow.Cells["MaDesc"].Value.ToString();
                    txtWorkerID.Text = grid1.ActiveRow.Cells["WORKER"].Value.ToString();
                    txtWorkName.Text = grid1.ActiveRow.Cells["WorkerName"].Value.ToString();
                    txtFaultname.Text = grid1.ActiveRow.Cells["FAULTNAME"].Value.ToString();
                    //cboFAULTTYPE.SelectedValue = grid1.ActiveRow.Cells["FaultType"].Value.ToString();
                    txtRepaircode.Text = grid1.ActiveRow.Cells["MACODE"].Value.ToString();
                    txtRepairname.Text = grid1.ActiveRow.Cells["REPAIRNAME"].Value.ToString();
                    txtMCType.Text = grid1.ActiveRow.Cells["MachFaultCode"].Value.ToString();
                    txtMCTypeName.Text = grid1.ActiveRow.Cells["MachFaultNAME"].Value.ToString();
                    txtReason.Text = grid1.ActiveRow.Cells["ReasonCode"].Value.ToString();
                    txtReasonName.Text = grid1.ActiveRow.Cells["Reasonname"].Value.ToString();
                    txtRemark.Text = grid1.ActiveRow.Cells["Remark"].Value.ToString();
                    txtMatime.Text = grid1.ActiveRow.Cells["maTime"].Value.ToString();
                    //if (grid1.ActiveRow.Cells["crud"].Value.ToString() == "U")
                    //{
                    //    v_txtWorkCenterCode.Enabled = false;
                    //    v_txtWorkCenterName.Enabled = false;
                    //    v_txtMachCode.Enabled = false;
                    //    v_txtMachName.Enabled = false;
                    //    v_txtFaultDate1.Enabled = false;
                    //    txtFaultstartDate.Enabled = false;
                    //}
                    //else
                    //{
                    //    v_txtWorkCenterCode.Enabled = true;
                    //    v_txtWorkCenterName.Enabled = true;
                    //    v_txtMachCode.Enabled = true;
                    //    v_txtMachName.Enabled = true;
                    //    v_txtFaultDate1.Enabled = true;
                    //    txtFaultstartDate.Enabled = true;

                    //}
                }


            }

        }
        // Form에서 사용할 함수나 메소드를 정의
        #endregion

        #endregion
    }
}
