#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  PP0500
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
    public partial class PP0500 : SAMMI.Windows.Forms.BaseMDIChildForm
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
        private int _Fix_Col = 0;
        private string PlantCode = string.Empty;

        public PP0500()
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
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                       // 사업장(공장)
                string sInMonth = string.Format("{0:yyyy-MM}", CboStartdate_H.Value);
                string sOPCode = this.txtOPCode.Text.Trim();
                string sWorkCenterCode = this.txtWorkCenterCode.Text.Trim();
                string sItemCode = this.txtItemCode.Text.Trim();                                                    // 품목코드
                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("InMonth", sInMonth, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_PP0500_S1N", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP0500_S1N_UNION", CommandType.StoredProcedure, param);
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
        private void PP0500_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);


            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
        //    _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "일자", false, GridColDataType_emu.VarChar, 95, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 95, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CarType", "차종", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Gubun", "구분", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _GridUtil.InitColumnUltraGrid(grid1, "D01", "1 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D02", "2 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D03", "3 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D04", "4 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D05", "5 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D06", "6 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D07", "7 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D08", "8 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D09", "9 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D10", "10 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D11", "11 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D12", "12 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D13", "13 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D14", "14 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D15", "15 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D16", "16 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D17", "17 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D18", "18 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D19", "19 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D20", "20 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D21", "21 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D22", "22 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D23", "23 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D24", "24 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D25", "25 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D26", "26 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D27", "27 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D28", "28 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D29", "29 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D30", "30 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D31", "31 일", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "TotQty", "합계", false, GridColDataType_emu.Integer, 200, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);

             _GridUtil.SetInitUltraGridBind(grid1);
            
            DtChange = (DataTable)grid1.DataSource;

            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            //grid1.DisplayLayout.UseFixedHeaders = true;
            //for (int i = 0; i < 10; i++)
            //    grid1.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;
            #endregion

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");


            #endregion

            #region Grid MERGE
            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["LineName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineName"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["CarType"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["CarType"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["CarType"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["ItemName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["OpCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OpCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OpCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["OPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;


            #endregion Grid MERGE
        }
         public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "D01", "D02", "D03", "D04", "D05","D06","D07","D08","D09","D10","D11", "D12", "D13", "D14", "D15","D16","D17","D18","D19","D20","D21", "D22", "D23", "D24", "D25","D26","D27","D28","D29","D30","D31", "TotQty" });

        }  
        #endregion

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        #region 텍스트 박스에서 팝업창에서 값 가져오기

        //private void Search_Pop_Item()
        //{
        //    string sitem_cd = this.txtItemCode.Text.Trim();    // 품목코드
        //    string sitem_name = this.txtItemName.Text.Trim();  // 품목명
        //    string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.SelectedValue.ToString());
        //    // string splantcd = "820";
        //    string sitemtype = "";


        //    try
        //    {

        //        _DtTemp = _biz.SEL_TBM0100(sPlantCode, sitem_cd, sitem_name, sitemtype);

        //        if (_DtTemp.Rows.Count > 1)
        //        {
        //            // 품목 POP-UP 창 처리
        //            PopUPManager pu = new PopUPManager();
        //            _DtTemp = pu.OpenPopUp("Item", new string[] { sPlantCode, sitemtype, sitem_cd, sitem_name }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

        //            if (_DtTemp != null && _DtTemp.Rows.Count > 0)
        //            {
        //                txtItemCode.Text = Convert.ToString(_DtTemp.Rows[0]["ItemCode"]);
        //                txtItemName.Text = Convert.ToString(_DtTemp.Rows[0]["Itemname"]);
        //            }
        //        }
        //        else
        //        {
        //            if (_DtTemp.Rows.Count == 1)
        //            {
        //                txtItemCode.Text = Convert.ToString(_DtTemp.Rows[0]["ItemCode"]);
        //                txtItemName.Text = Convert.ToString(_DtTemp.Rows[0]["Itemname"]);
        //            }
        //            else
        //            {
        //                MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
        //                txtItemCode.Text = string.Empty;
        //                txtItemName.Text = string.Empty;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("ERROR", ex.Message);
        //    }

        //}
        #endregion
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

        #region 텍스트 박스에서 팝업창에서 값 가져오기
        //////////////////     
        //private void Search_Pop_TBM0400()
        //{

        //    string sPlantCode = string.Empty;             //사업장코드
        //    string sOPCode = txtOPCode.Text.Trim();       //공정코드
        //    string sOPName = txtOPName.Text.Trim();       //공정명 
        //    string sUseFlag = string.Empty;               //사용여부         


        //    if (this.cboPlantCode_H.SelectedValue != null)
        //        sPlantCode = cboPlantCode_H.SelectedValue.ToString() == "ALL" ? "" : cboPlantCode_H.SelectedValue.ToString();         ///사업장코드 

        //    //            if (this.cboUseFlag_H.SelectedValue != null)
        //    //                sUseFlag = cboUseFlag_H.SelectedValue.ToString() == "ALL" ? "" : cboUseFlag_H.SelectedValue.ToString();                 // 사용여부

        //    sUseFlag = "";                 // 사용여부

        //    try
        //    {
        //        _biz.TBM0400_POP(sPlantCode, sOPCode, sOPName, sUseFlag, txtOPCode, txtOPName);

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("ERROR", ex.Message);
        //    }

        //}
        #endregion        //공정(작업장)
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



        private void txtOPName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    Search_Pop_TBM0400();
            //}

        }

        #region 라인 (TBM0500) 팝업창에서 값 가져오기
        //private void Search_Pop_TBM0500()
        //{

        //    string sPlantCode = string.Empty;                             //사업장코드
        //    string sLineCode = ""; // txtLineCode.Text.Trim();                   //라인코드
        //    string sLineName = ""; // txtLineName.Text.Trim();                   //라인명명 
        //    string sUseFlag = string.Empty;                              //사용여부      


        //    if (this.cboPlantCode_H.SelectedValue != null)
        //        sPlantCode = cboPlantCode_H.SelectedValue.ToString() == "ALL" ? "" : cboPlantCode_H.SelectedValue.ToString();         ///사업장코드 

        //    //            if (this.cboUseFlag_H.SelectedValue != null)
        //    //                sUseFlag = cboUseFlag_H.SelectedValue.ToString() == "ALL" ? "" : cboUseFlag_H.SelectedValue.ToString();                 // 사용여부

        //    sUseFlag = "";                 // 사용여부

        //    try
        //    {
        //       // _biz.TBM0500_POP(sPlantCode, sLineCode, sLineName, sUseFlag, txtLineCode, txtLineName);

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("ERROR", ex.Message);
        //    }

        //}
        #endregion 라인 (TBM0500) 팝업창에서 값 가져오기

        //private void txtLineName_KeyDown(object sender, KeyEventArgs e)
        //{
        //    this.txtLineCode.Text = string.Empty;
        //}


        private void txtLineCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    Search_Pop_TBM0500();
            //}
        }

        private void txtLineCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_TBM0500();
        }



        private void txtLineName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    Search_Pop_TBM0500();
            //}
        }

        private void txtLineName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_TBM0500();
        }



        #region 작업장(TBM0600) 팝업창에서 값 가져오기
        //////////////////     
        //private void Search_Pop_TBM0600()
        //{

        //    string sPlantCode = string.Empty;                             //사업장코드
        //    string sOPCode = txtOPCode.Text.Trim();                       //공정코드
        //    string sOPName = txtOPName.Text.Trim();                       //공정명 
        //    string sLineCode = string.Empty;                              //라인코드
        //    string sWORKCENTERCODE = ""; //txtWorkCenterCode.Text.Trim();       //작업호기(라인)코드
        //    string sWorkCenterName = ""; //txtWorkCenterName.Text.Trim();       //작업호기(라인)명 
        //    string sUseFlag = string.Empty;                               //사용여부         


        //    if (this.cboPlantCode_H.SelectedValue != null)
        //        sPlantCode = cboPlantCode_H.SelectedValue.ToString() == "ALL" ? "" : cboPlantCode_H.SelectedValue.ToString();         ///사업장코드 

        //    //if (this.cboUseFlag_H.SelectedValue != null)
        //    //    sUseFlag = cboUseFlag_H.SelectedValue.ToString() == "ALL" ? "" : cboUseFlag_H.SelectedValue.ToString();                 // 사용여부
        //    sUseFlag = "";                 // 사용여부: 전체


        //    try
        //    {
        //      //  _biz.TBM0600_POP(sPlantCode, sWORKCENTERCODE, sWorkCenterName, sOPCode, sLineCode, sUseFlag, txtWorkCenterCode, txtWorkCenterName);

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("ERROR", ex.Message);
        //    }

        //}
        #endregion

        private void txtWorkCenterCode_KeyDown(object sender, KeyEventArgs e)
        {
        //   this.txtWorkCenterName.Text = string.Empty;
        }

        private void txtWorkCenterName_KeyDown(object sender, KeyEventArgs e)
        {
        //    this.txtWorkCenterCode.Text = string.Empty;
        }

        private void txtWorkCenterCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    Search_Pop_TBM0600();
            //}
        }

        private void txtWorkCenterCode_MouseDoubleClick(object sender, MouseEventArgs e)
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


        #endregion

        private void txtOPCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_TBM0400();
        }

        private void txtOPName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Search_Pop_TBM0400();
        }

        private void txtLineCode_KeyDown(object sender, KeyEventArgs e)
        {
           // this.txtLineName.Text = string.Empty;
        }

        private void txtLineName_KeyDown(object sender, KeyEventArgs e)
        {
            //this.txtLineCode.Text = string.Empty;
        }

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "ItemName")
            //    {
            //        _Fix_Col = i;
            //    }
            //}

            //for (int i = 0; i < _Fix_Col + 1; i++)
            //{
            //    e.Layout.UseFixedHeaders = true;
            //    e.Layout.Bands[0].Columns[i].Header.Fixed = true;
            //}
        }


    }
}