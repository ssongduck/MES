#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  PP3700
//   Form Name    : 
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
#endregion

namespace SAMMI.PP
{
    public partial class PP3700 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string PlantCode = string.Empty;

        public PP3700()
        {
            InitializeComponent();

            btbManager = new BizTextBoxManagerEX();

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

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                base.DoInquire();


                string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);  // 공장코드                                       // 사업장(공장)
                string sStartDate = SqlDBHelper.nvlDateTime(CboStartdate_H.Value).ToString("yyyy-MM-dd");                           // 생산시작일자
                string sEndDate = SqlDBHelper.nvlDateTime(CboEnddate_H.Value).ToString("yyyy-MM-dd");                             // 생산  끝일자
                string sDayNight = SqlDBHelper.nvlString(cboDaynight_H.Value, "");     // 주야 구분
                string sGubun = SqlDBHelper.nvlString(cboGubun.Value, "");    // 근무 
             
                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("DayNight", sDayNight, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("Gubun", sGubun, SqlDbType.VarChar, ParameterDirection.Input);
                
                //rtnDtTemp = helper.FillTable("USP_PP3700_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP3700_S1_UNION", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

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
        private void PP3700_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);//90 95 160 70 170 100 170 140 200 100 100 121 100 100 80 50 100 100 100 100 100 80 80 100 100 100 100 107 113 89 
                                                 //90 95 160 70 170 100 170 140 200 100 100 121 128 126 80 50 100 100 100 100 100 80 80 100 100 100 100 110 110 110 


            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 95, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "라인코드", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품목", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "prevItemName", "원재료명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LotNO", "Lot No", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "일자", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Status", "상태", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlanNo", "계획지시번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderNo", "지시번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQty", "생산량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UnitCode", "단위", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DayNight", "주야구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ShiftGb", "조구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ReWorkFlag", "재작업여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LotProcType", "완료구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MainWorker", "주작업자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UPH", "UPH", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CycleTime", "사이클타임", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProcFlag", "반영여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkStartDate", "작업일시", false, GridColDataType_emu.VarChar, 180, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkEndDate", "완료일시", false, GridColDataType_emu.VarChar, 180, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkTime", "작업시간(분)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQtyCal", "생산량계산치", false, GridColDataType_emu.Integer, 110, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQtyReal", "생산량실측치", false, GridColDataType_emu.Integer, 110, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQtyMan", "작업자입력치", false, GridColDataType_emu.Integer, 110, 100, Infragistics.Win.HAlign.Right,false, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            
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

            //grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["ItemName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemName"].MergedCellStyle = MergedCellStyle.Always;
            #endregion

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");


            #endregion
        }
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "ProdQty", "WorkTime" });

        }
        #endregion

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
       



        #endregion


    }
}