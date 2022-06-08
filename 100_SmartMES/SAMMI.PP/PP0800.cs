#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : PP0800
//   Form Name    : 투입 반제품 Lot 현황(재공) 조회
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
    public partial class PP0800 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();
        BizTextBoxManagerEX btbManager;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string PlantCode = string.Empty;

        public PP0800()
        {
            InitializeComponent();

            btbManager = new BizTextBoxManagerEX();
            
            btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { "SK1", "" });
            btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { "SK1", "" });
            btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { "SK1", txtOPCode, "", "" }
                              , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper   = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[7];            

            try
            {
                base.DoInquire();

                string sPlantCode = "SK1";                                                                          // 사업장(공장)
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);                          // 생산시작일자
                string sEndDate   = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);                            // 생산  끝일자
                string sOPCode    = this.txtOPCode.Text.Trim();                                                     // 공정 코드
                string sLineCode  = this.txtLineCode.Text.Trim();                                                   // 라인 코드
                string sWorkCenterCode = this.txtWorkCenterCode.Text.Trim();                                        // 작업장 코드
                string sItemCode = this.txtItemCode.Text.Trim();                                                    // 품목코드
                
                param[0] = helper.CreateParameter("PlantCode",  sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("StartDate",  sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("EndDate",    sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("OPCode",     sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("LineCode",   sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_PP0800_S1", CommandType.StoredProcedure, param);
                //rtnDtTemp = helper.FillTable("USP_PP0800_S1_UNION", CommandType.StoredProcedure, param);
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

        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "ProdQty" });

        }
        #region 폼 로더
        private void PP0800_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, false, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 95, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "투입일시", false, GridColDataType_emu.DateTime24, 170, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
     //       _GridUtil.InitColumnUltraGrid(grid1, "SeqNo", "순번", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
     //       _GridUtil.InitColumnUltraGrid(grid1, "ItemType", "제품군", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            
            _GridUtil.InitColumnUltraGrid(grid1, "LotNo", "투입 LOTNO", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
       //     _GridUtil.InitColumnUltraGrid(grid1, "Status", "구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQty", "투입량", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "투입자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
      //      _GridUtil.InitColumnUltraGrid(grid1, "InUseQty", "사용량", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
      //      _GridUtil.InitColumnUltraGrid(grid1, "StockQty", "잔량", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
      //      _GridUtil.InitColumnUltraGrid(grid1, "LastUseDate", "최종갱신일", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
      //      _GridUtil.InitColumnUltraGrid(grid1, "StockDate", "잔량보관일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
     //       _GridUtil.InitColumnUltraGrid(grid1, "UnitCode", "단위", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
     //       _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "수불일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
     //       _GridUtil.InitColumnUltraGrid(grid1, "OrderNo", "지시번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
     //       _GridUtil.InitColumnUltraGrid(grid1, "PlanNo", "계획지시번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);


            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
           
            //rtnDtTemp = _Common.GET_TBM0000_CODE("ItemType");
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ItemType", rtnDtTemp, "CODE_ID", "CODE_NAME");
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "InItemType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            #endregion
        }
        #endregion

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        
        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtItemName.Text = string.Empty;
        }

        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtItemCode.Text = string.Empty;
        }


        private void txtOPCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPName.Text = string.Empty;
        }

        private void txtOPNAME_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPCode.Text = string.Empty;
        }

        private void txtWorkCenterCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkCenterName.Text = string.Empty;
        }

        private void txtWorkCenterName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkCenterCode.Text = string.Empty;
        }

        private void txtLineCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtLineName.Text = string.Empty;
        }

        private void txtLineName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtLineCode.Text = string.Empty;
        }
        #endregion
    }
}
