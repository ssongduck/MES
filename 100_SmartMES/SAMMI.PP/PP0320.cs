#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  PP0320
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
    public partial class PP0320 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        public PP0320()
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
                //btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.PlantCode, "", "" }
                //         , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                //btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.PlantCode, "", "" }
                //         , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }


        }

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

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                        // 사업장(공장)
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

                //rtnDtTemp = helper.FillTable("USP_PP0320_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP0320_S1_UNION", CommandType.StoredProcedure, param);
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
        private void PP0320_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, false, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "recdate", "일자", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "opcode", "공정코드", false, GridColDataType_emu.VarChar, 105, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "linecode", "라인코드", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "linename", "라인명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "workcentercode", "작업장코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "workcentername", "작업장명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "itemcode", "품번", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "itemname", "품명", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "orderno", "지시번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "planQty", "계획수량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "dQty", "Lot(주)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "nQty", "Lot(야)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "tQty", "Lot(계)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "dEqty", "불량(주)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "nEqty", "불량(야)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "tEqty", "불량(계)", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "endDate", "종료시간", false, GridColDataType_emu.DateTime24, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);            

            //_GridUtil.InitColumnUltraGrid(grid1, "seqid", "seqid", false, GridColDataType_emu.VarChar, 0, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "RecDate", "일자", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 105, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "LineCode", "라인코드", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "OrderNo", "지시번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "WorkerCnt", "작업자수", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null); 
            //_GridUtil.InitColumnUltraGrid(grid1, "PlanQty", "계획수량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);            
            //_GridUtil.InitColumnUltraGrid(grid1, "okqty_D", "양품수량(주)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "okqty_N", "양품수량(야)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "okqty_T", "양품수량(계)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQty1_D", "완성불량(주)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQty1_N", "완성불량(야)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQty1_T", "완성불량(계)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQty2_D", "리크물량(주)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQty2_N", "리크물량(야)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQty2_T", "리크물량(계)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQtyt_D", "불량계(주)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQtyt_N", "불량계(야)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQtyt_T", "불량계(계)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ProdQty_D", "생산량(주)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ProdQty_N", "생산량(야)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ProdQty_T", "생산량(계)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQty0_D", "공정불량(주)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQty0_N", "공정불량(야)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ErrorQty0_T", "공정불량(계)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "incnt_D", "투입(주)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, false, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "incnt_N", "투입(야)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, false, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "incnt_T", "투입(계)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, false, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "LotQty_D", "Lot(주)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "LotQty_N", "Lot(야)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "LotQty_T", "Lot(계)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "CAQty", "이월수량", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "NonWork_D", "비가동(주,분)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "NonWork_N", "비가동(야,분)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "NonWork_T", "비가동(계,분)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "start_DT", "시작시간", false, GridColDataType_emu.DateTime24, 130, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "end_DT", "종료시간", false, GridColDataType_emu.DateTime24, 130, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "order_dt", "작지선택시간", false, GridColDataType_emu.DateTime24,130, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            
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

            grid1.InitializeRow += new InitializeRowEventHandler(grid1_InitializeRow);
            grid1.DisplayLayout.UseFixedHeaders = true;
            //for (int i = 0; i < 9; i++)
            //    grid1.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;
        }

        void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["ItemCode"].Value.ToString() == "계")
                e.Row.Appearance.BackColor = Color.LightCyan;
        }
        #endregion

  
    }
}
