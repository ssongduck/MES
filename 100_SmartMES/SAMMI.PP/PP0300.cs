#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  PP0300
//   Form Name    : 작업호기(WorkCenter)별  정보 조회
//   Name Space   : SAMMI.MM
//   Created Date : 
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
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
#endregion

namespace SAMMI.PP
{
    public partial class PP0300 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
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

        public PP0300()
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
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }

        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[10];

            try
            {
                DtChange.Clear();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                       // 사업장(공장)
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);                          // 생산시작일자
                string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);                              // 생산  끝일자
                string sWorkCenterCode = this.txtWorkCenterCode.Text.Trim();                                        // 작업장 코드
                string sOPCode = this.txtOPCode.Text.Trim();                                                        // 공정 코드
                string sLineCode = this.txtLineCode.Text.Trim();                                                    // 라인 코드
                string sItemCode = this.txtItemCode.Text.Trim();                                                    // 품목코드
                string sDayNight = SqlDBHelper.nvlString(cboDaynight_H.Value, "");

                base.DoInquire();

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[7] = helper.CreateParameter("@DayNight", sDayNight, SqlDbType.VarChar, ParameterDirection.Input);

                param[8] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                param[9] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                //rtnDtTemp = helper.FillTable("USP_PP0300_S2N", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP0300_S2N_UNION", CommandType.StoredProcedure, param);
                
                if (param[8].Value.ToString() == "E") throw new Exception(param[9].Value.ToString());
   
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


        #region 폼 로더
        private void PP0300_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, false, false, "", false);
            
            //90 95 160 70 170 100 170 100 100 140 200 80 50 100 130 130 80 100 100 100 100 140 100 

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "일자", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "라인코드", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CarType", "차종", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderNo", "지시번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQty_D", "양품수량(주)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrQty_D", "불량수량(주)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ratio_D", "양품율(주)", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);

            _GridUtil.InitColumnUltraGrid(grid1, "ProdQty_N", "양품수량(야)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrQty_N", "불량수량(야)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ratio_N", "양품율(야)", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);

            _GridUtil.InitColumnUltraGrid(grid1, "ProdQty_T", "양품수량(계)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrQty_T", "불량수량(계)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ratio_T", "양품율(계)", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "color_d", " ", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "color_n", " ", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "color_t", " ", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);

            #endregion

            #region Grid MERGE
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

            //grid1.Columns["RecDate"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["RecDate"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["RecDate"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["itemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["itemCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["ItemName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["OrderNo"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OrderNo"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OrderNo"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["DayNight"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["DayNight"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["DayNight"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["DayNightNM"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["DayNightNM"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["DayNightNM"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["ShiftGb"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ShiftGb"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ShiftGb"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.DisplayLayout.UseFixedHeaders = true;
            //for (int i = 0; i < 10; i++)
            //    grid1.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;

            #endregion Grid MERGE

            _GridUtil.SetInitUltraGridBind(grid1);

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            DtChange = (DataTable)grid1.DataSource;
            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
        }
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "ProdQty_D", "ErrQty_D", "ProdQty_N", "ErrQty_N", "ProdQty_T", "ErrQty_T" });

        }


        #endregion

        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["color_d"].Value.ToString() == "RED")
                e.Row.Cells["ratio_d"].Appearance.ForeColor = Color.Red;
            if (e.Row.Cells["color_n"].Value.ToString() == "RED")
                e.Row.Cells["ratio_n"].Appearance.ForeColor = Color.Red;
            if (e.Row.Cells["color_t"].Value.ToString() == "RED")
                e.Row.Cells["ratio_t"].Appearance.ForeColor = Color.Red;
        }

  
    }
}
