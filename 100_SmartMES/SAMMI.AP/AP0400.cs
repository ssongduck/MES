#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  AP0400
//   Form Name    : 생산계획 대비 생산현황
//   Name Space   : SAMMI.AP
//   Created Date : 2012-11-28
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 생산계획 대비 생산현황 화면
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

namespace SAMMI.AP
{
    public partial class AP0400 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;
        private string PlantCode = string.Empty;
        //비지니스 로직 객체 생성
        //PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        #endregion

        #region < CONSTRUCTOR >

        public AP0400()
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
                btbManager.PopUpAdd(txtWorkcenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "", "" }
                        , new string[] { "", "" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkcenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "", "" }
                        , new string[] { "", "" }, new object[] { });
            }

        }
        #endregion

        #region  AP0400_Load
        private void AP0400_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);


            // InitColumnUltraGrid  (100 121 121 114 192 100 100 100 100 129 209 139 100 100 100 100 100 136 100 141 100 100 100  )
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Linecode", "라인코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StartPlanDate", "지시일자", false, GridColDataType_emu.YearMonthDay, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);  
            _GridUtil.InitColumnUltraGrid(grid1, "EndPlanDate",          "지시일자(종)",   false, GridColDataType_emu.YearMonthDay, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);  
            _GridUtil.InitColumnUltraGrid(grid1, "PlanNo",               "계획번호",      false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, null , null, null, null, null);  
            _GridUtil.InitColumnUltraGrid(grid1, "PlanQty",              "계획량",       false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###" , null, null, null, null);  
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQty",              "생산실적",      false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);  
            _GridUtil.InitColumnUltraGrid(grid1, "UnitCode",             "단위",         false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Rate",                 "생산률(%)",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderAssignFlag",      "지시할당여부",   false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "AssignWorkCenterCode", "지시할당작업장", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldAssignFlag",       "금형할당여부",   false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerAssignFlag",     "작업자할당여부", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StartWorkDate", "생산시작일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EndWorkDate", "생산완료일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "수불일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "color", " ", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            DtChange = (DataTable)grid1.DataSource;

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

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            //SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            #endregion

        }
        #endregion  AP0400_Load

        #region <TOOL BAR AREA >
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "PlanQty", "ProdQty" });

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

                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);  // 공장코드                                          
                string sStartDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(calRegDT_FRH.Value));
                string sEndDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(calRegDT_TOH.Value));
                string sWorkCenterCode = txtWorkcenterCode.Text.Trim();
                string sItemCode = txtItemCode.Text.Trim();
                string sOPCode = txtOPCode.Text.Trim(); ;
                string sLineCode = "";  //라인코드;                                               


                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("ItemCode ", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("LineCode ", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_AP0400_S1N", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_AP0400_S1N_UNION", CommandType.StoredProcedure, param);

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
        #endregion  <TOOL BAR AREA >

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        
        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtItemName.Text = string.Empty;
        }

        private void txtItemCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    Search_Pop_Item();
            //}
        }

        private void txtItemCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_Item();
        }

        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtItemCode.Text = string.Empty;
        }

        private void txtItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    Search_Pop_Item();
            //}
        }

        private void txtItemName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_Item();
        }

        #endregion <METHOD AREA>

        
        private void txtOPCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPName.Text = string.Empty;
        }

        private void txtOPNAME_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPCode.Text = string.Empty;
        }

        private void txtOPCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    Search_Pop_TBM0400();
            //}
        }

        private void txtOPCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_TBM0400();
        }



        private void txtOPName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    Search_Pop_TBM0400();
            //}
        }

        private void txtOPName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_TBM0400();
        }

        

        private void txtWorkcenterCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkCenterName.Text = string.Empty;
        }

        private void txtWorkCenterName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkcenterCode.Text = string.Empty;
        }

        private void txtWorkcenterCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    Search_Pop_TBM0600();
            //}
        }

        private void txtWorkcenterCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_TBM0600();
        }



        private void txtWorkCenterName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    Search_Pop_TBM0600();
            //}
        }

        private void txtWorkCenterName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_TBM0600();
        }

        private void gbxHeader_Click(object sender, EventArgs e)
        {

        }

        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["color"].Value.ToString() == "RED")
                e.Row.Cells["Rate"].Appearance.ForeColor = Color.Red;
        }

    }
}
