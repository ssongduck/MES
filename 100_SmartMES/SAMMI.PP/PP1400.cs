using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using SAMMI.PopUp;
using SAMMI.Common;
using Infragistics.Win.UltraWinGrid;

namespace SAMMI.PP
{
    public partial class PP1400 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        //SAMMI.PopUp;.BizGridManagerEX BIZPOP;
        Common.Common _Common = new Common.Common();

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        private DataTable DtChange = null; //그리드의 변경전 상태값을 담아두는 DataTable
        private string PlantCode = string.Empty;
        public PP1400()
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
            gridManager = new BizGridManagerEX(grid1);

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOpCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOpCode, txtOpName, txtLineCode, txtLineName });

                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { this.cboPlantCode_H, txtOpCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
            }
            else
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOpCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOpCode, txtOpName, txtLineCode, txtLineName });

                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { LoginInfo.PlantAuth, txtOpCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
            }
            
            gridManager.PopUpAdd("OPCode", "OPName", "TBM0400", new string[] { "PlantCode", "" });
            gridManager.PopUpAdd("LineCode", "LineName", "TBM0500", new string[] { "PlantCode", "OPCode" });
            gridManager.PopUpAdd("STOPCODE", "StopDesc", "TBM1100", new string[] { "WORKCENTERCODE", "", "", "" });
            
        }

        #region PP1400_Load
        private void PP1400_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1);

            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RECDATE", "일자", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCODE", "공정", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPTYPE",  "비가동유형", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPTYPENM","비가동유형명", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPCODE",  "비가동", false, GridColDataType_emu.VarChar, 90, 60, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StopDesc",  "비가동명", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "REMARK",    "비가동사유", false, GridColDataType_emu.VarChar, 200, 500, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STARTDATE", "시작시간", false, GridColDataType_emu.DateTime24, 180, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ENDDATE",   "종료시간", false, GridColDataType_emu.DateTime24, 180, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STATUSTIME","소요시간(분)", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKERCNT", "작업자수", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LOTNO",     "Lot No", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE",  "품목", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME",  "품목명", false, GridColDataType_emu.VarChar, 180, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PRODQTY",   "설비카운트", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RESULTQTY", "입고량", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKER",     "작성자", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            
            _GridUtil.SetInitUltraGridBind(grid1);

            #region Grid MERGE                   
            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["RECDATE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["RECDATE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["RECDATE"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["OPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WORKCENTERCODE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WORKCENTERCODE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WORKCENTERCODE"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WORKCENTERNAME"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WORKCENTERNAME"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WORKCENTERNAME"].MergedCellStyle = MergedCellStyle.Always;
         
            #endregion Grid MERGE

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("STOPCLASS");  //비가동 유형
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "STOPCLASS", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("STOPTYPE");  //비가동 타입
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "STOPTYPE", rtnDtTemp, "CODE_ID", "CODE_NAME");
            #endregion
        }
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "STATUSTIME" });

        }
        #endregion PP1400_Load

        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);        //사업장코드
                string sStartDate = string.Format("{0:yyyy-MM-dd}", cboStartDate.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", cboEndDate.Value);
                string sOpCode = txtOpCode.Text.Trim();
                
                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();

                param[0] = helper.CreateParameter("PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("STARTDATE", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("ENDDATE", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("WORKCENTERCODE", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("OPCODE", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);
                
                //rtnDtTemp = helper.FillTable("USP_PP1400_S1N", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP1400_S1N_UNION", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                foreach (UltraGridRow ugr in grid1.Rows)
                {

                }

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

        public override void DoSave()
        {
         
            
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;
            grid1.PerformAction(UltraGridAction.DeactivateCell);

            try
            {
                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                base.DoSave();

                foreach (DataRow drRow in DtChange.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            break;
                        case DataRowState.Added:
                            break;
                        case DataRowState.Modified:

                            #region 수정
                            param = new SqlParameter[9];
                            
                            param[0] = helper.CreateParameter("@PLANTCODE",      drRow["PLANTCODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@RECDATE",        drRow["RECDATE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@WORKCENTERCODE", drRow["WORKCENTERCODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@STARTDATE",      drRow["STARTDATE"].ToString(), SqlDbType.DateTime, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@STOPCODE",       drRow["STOPCODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@REMARKS",       drRow["REMARK"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@MAKER",         SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            param[7] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[8] = helper.CreateParameter("@RS_MSG",  SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_PP1400_U1", CommandType.StoredProcedure, param);

                            if (param[7].Value.ToString() == "E") throw new Exception(param[8].Value.ToString());

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
         
    }
}
