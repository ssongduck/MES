#region <USING AREA>
using SAMMI.Common;
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using SAMMI.PopUp;
using SAMMI.PopManager;
#endregion

namespace SAMMI.QM
{
    public partial class QM4700 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        // 변수나 Form에서 사용될 Class를 정의
        DataTable _rtnDtTemp = new DataTable();
        DataTable _rtnDtTemp2 = new DataTable();
        DataSet rtnDsTemp = new DataSet();
        DataTable rtnDtTemp = new DataTable();
        private DataTable DtChange = null;
        BizGridManagerEX gridManager;
        private string PlantCode = string.Empty;
        private string WorkCenterCode = string.Empty;
        private string WorkCenterOPCode = string.Empty;
        private string InspCode = string.Empty;
        private string ItemCode = string.Empty;
        private string MeasureEquID = string.Empty;
        private string MACHCode = string.Empty;
        private string WorkType = string.Empty;
        private DateTime FRDT = System.DateTime.Now;
        private DateTime TODT = System.DateTime.Now;

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private int _Fix_Col = 0;
        #endregion

        public QM4700()
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

            this.calRegDT_FRH.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00");
            this.calRegDT_TOH.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 23:59:59");

            #region <콤보파일 셋팅>
            _rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", _rtnDtTemp, "CODE_ID", "CODE_NAME");
            #endregion

            #region <POPUP>
            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOpCode, "", "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { this.cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOpCode, "", "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
                , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            #endregion

        }

        private void QM4700_Load(object sender, EventArgs e)
        {   
            #region <Grid Setting>

            _GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "등록일자", false, GridColDataType_emu.VarChar, 90, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 70, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 70, 10, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 140, 10, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderNo", "작업지시번호", false, GridColDataType_emu.Double, 120, 0, Infragistics.Win.HAlign.Center, false, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품목코드", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 200, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorClass", "불량유형", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorCode", "불량코드", false, GridColDataType_emu.VarChar, 80, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorDesc", "불량명", false, GridColDataType_emu.VarChar, 120, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQty", "불량수량", false, GridColDataType_emu.VarChar, 70, 30, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SUMQty_D", " 불량수량(주)", false, GridColDataType_emu.VarChar, 70, 30, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SUMQty_N", " 불량수량(야)", false, GridColDataType_emu.VarChar, 70, 30, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SUMQty", " 불량수량(합)", false, GridColDataType_emu.VarChar, 70, 30, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "EmptyQty", "품목별 공타수량", false, GridColDataType_emu.VarChar, 70, 30, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "DayNight", "주/야", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
           
          //  _GridUtil.InitColumnUltraGrid(grid1, "ERP_IF", "ERP_IF", false, GridColDataType_emu.VarChar, 100, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _rtnDtTemp2 = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", _rtnDtTemp2, "CODE_ID", "CODE_NAME");

            _rtnDtTemp2 = _Common.GET_TBM0000_CODE("DAYNIGHT");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DayNight", _rtnDtTemp2, "CODE_ID", "CODE_NAME");

            _rtnDtTemp2 = _Common.GET_TBM0000_CODE("ERRORCLASS");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ErrorClass", _rtnDtTemp2, "CODE_ID", "CODE_NAME");

            _rtnDtTemp2 = _Common.GET_TBM0000_CODE("IF_Flag");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ERP_IF", _rtnDtTemp2, "CODE_ID", "CODE_NAME");

            DtChange = (DataTable)grid1.DataSource;
            _GridUtil.SetInitUltraGridBind(this.grid1);

            #region Grid MERGE
            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["OPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["LineName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["OrderNo"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OrderNo"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OrderNo"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["ItemName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemName"].MergedCellStyle = MergedCellStyle.Always;
            
            //grid1.Columns["DayNight"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["DayNight"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["DayNight"].MergedCellStyle = MergedCellStyle.Always;
            #endregion Grid MERGE

            #endregion
        }

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            base.DoInquire();

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[8];

            DateTime planstartdt = Convert.ToDateTime(((DateTime)this.calRegDT_FRH.Value).ToString("yyyy-MM-dd") + " 08:00:00.00");
            DateTime planenddt = Convert.ToDateTime(((DateTime)this.calRegDT_TOH.Value).AddDays(1).ToString("yyyy-MM-dd") + " 07:59:59.99");
            if (Convert.ToInt32(planstartdt.ToString("yyyyMMdd")) > Convert.ToInt32(planenddt.ToString("yyyMMdd")))
            {
                SException ex = new SException("R00200", null);
                throw ex;
            }

            string sWorkCenterCode = txtWorkCenterCode.Text.Trim();
            string sItemCode = this.txtItemCode.Text.Trim();
            string sOrderNo = SqlDBHelper.nvlString(txtOrderNo.Text);
            string sDayNight = SqlDBHelper.nvlString(cboDayNight_H.Value);

            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            this.ItemCode = sItemCode;
            this.FRDT = planstartdt;
            this.TODT = planenddt;

            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
            param[1] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
            param[2] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
            param[3] = helper.CreateParameter("@OrderNo", sOrderNo, SqlDbType.VarChar, ParameterDirection.Input);
            param[4] = helper.CreateParameter("@Shift", sDayNight, SqlDbType.VarChar, ParameterDirection.Input);
            param[5] = helper.CreateParameter("@FRDT", FRDT, SqlDbType.DateTime, ParameterDirection.Input);
            param[6] = helper.CreateParameter("@TODT", TODT, SqlDbType.DateTime, ParameterDirection.Input);
            param[7] = helper.CreateParameter("@OpCode", txtOpCode.Text, SqlDbType.VarChar, ParameterDirection.Input);

            //rtnDtTemp = helper.FillTable("USP_QM4700_S1N", CommandType.StoredProcedure, param);
            rtnDtTemp = helper.FillTable("USP_QM4700_S1N_UNION", CommandType.StoredProcedure, param);
            if (rtnDtTemp.Rows.Count == 0)
            {
                //MessageBox.Show("DATA가 없습니다.");
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
        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
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
                            if (SqlDBHelper.nvlString(dr["PlantCode"]).Equals(String.Empty) ||
                                SqlDBHelper.nvlString(dr["WorkCenterCode"]).Equals(String.Empty) ||
                                SqlDBHelper.nvlString(dr["OrderNo"]).Equals(String.Empty) ||
                                SqlDBHelper.nvlString(dr["ItemCode"]).Equals(String.Empty))
                            {
                                ShowDialog("작업장코드, 지시번호, 품목코드, 불량코드는 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);

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
                            #endregion

                            break;
                        case DataRowState.Added:
                            #region [추가]
                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region [수정]
                            param = new SqlParameter[9];

                            param[0] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"].ToString()), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[1] = helper.CreateParameter("WorkCenterCode", drRow["WorkCenterCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[2] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[3] = helper.CreateParameter("OrderNo", drRow["OrderNo"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                            param[4] = helper.CreateParameter("ErrorCode", drRow["ErrorCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[5] = helper.CreateParameter("RecDate", drRow["RecDate"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("DayNight", drRow["DayNight"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[7] = helper.CreateParameter("ErrorQty", drRow["ErrorQty"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            helper.ExecuteNoneQuery("USP_QM4700_I1", CommandType.StoredProcedure, param);
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

        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "SUMQty_D", "SUMQty_N", "SUMQty" });

        }
        #endregion

    }
}
