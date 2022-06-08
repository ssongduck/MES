#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : PP8100
//   Form Name    : PDA 재고실사 로그현황
//   Name Space   : SAMMI.PP
//   Created Date : 
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD, 2014.09.29 YMC
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
   public partial class PP8100 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        public PP8100()
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
               btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { cboPlantCode_H, "", "", }
                      , new string[] { "", "" }, new object[] { });
            }
            else
            {
               btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "" , "" , }
                      , new string[] { "", "" }, new object[] { });
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

              string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                               // 사업장(공장)
              string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);                          // from 작업일자
              string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);                              // to 작업일자
              string sItemCode = this.txtItemCode.Text.Trim();                                                    // 품목코드
              string sLotNo = this.txtLotNo.Text.Trim();                                                       // LotNo, 재고기록표 

              
              base.DoInquire();

              param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
              param[1] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
              param[2] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
              param[3] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
              param[4] = helper.CreateParameter("@LotNo", sLotNo, SqlDbType.VarChar, ParameterDirection.Input);
              
              param[5] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
              param[6] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

              rtnDtTemp = helper.FillTable("USP_PDA_Log_S1", CommandType.StoredProcedure, param);

              if (param[5].Value.ToString() == "E") throw new Exception(param[6].Value.ToString());

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
      private void PP8100_Load(object sender, EventArgs e)
      {
         #region Grid 셋팅
         _GridUtil.InitializeGrid(this.grid1, true, false, false, "", false);

         //90 95 160 70 170 100 170 100 100 140 200 80 50 100 130 130 80 100 100 100 100 140 100 

         _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 70, 70, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "TagNo", "Tag번호", false, GridColDataType_emu.YearMonthDay, 70, 70, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "Resource_no", "품번", false, GridColDataType_emu.VarChar, 150, 150, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "MoveTicket", "재고기록표", false, GridColDataType_emu.VarChar, 50, 50, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "LotNo", "LotNo", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "TagQty", "Tag수량", false, GridColDataType_emu.Integer, 90, 90, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "Qty", "실사수량", false, GridColDataType_emu.Integer, 90, 90, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "Location", "저장소", false, GridColDataType_emu.VarChar, 50, 50, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "ChkYN", "스캔", false, GridColDataType_emu.VarChar, 40, 40, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "TransYN", "전송", false, GridColDataType_emu.VarChar, 40, 40, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "Chkdate", "스캔일자", false, GridColDataType_emu.DateTime24, 180, 180, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "Transdate", "전송일자", false, GridColDataType_emu.DateTime24, 180, 180, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
         _GridUtil.InitColumnUltraGrid(grid1, "User_id", "등록자", false, GridColDataType_emu.VarChar, 80, 80, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);


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

      #endregion
   }
}
