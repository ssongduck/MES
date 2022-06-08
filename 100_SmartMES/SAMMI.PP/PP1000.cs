#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  PP1000
//   Form Name    : 금형별 생산실적 조회
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
    public partial class PP1000 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        BizTextBoxManagerEX btbManager;

        private string PlantCode = string.Empty;

        public PP1000()
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
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                       , new string[] { "", "" }, new object[] { "", "" });
                btbManager.PopUpAdd(txtMoldCode, txtMoldName, "TBM1600", new object[] { this.cboPlantCode_H, "", "" }
                        , new string[] { "", "" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                       , new string[] { "", "" }, new object[] { "", "" });
                btbManager.PopUpAdd(txtMoldCode, txtMoldName, "TBM1600", new object[] { LoginInfo.PlantAuth, "", "" }
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
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                     // 사업장(공장)
                string sWorkCenterCode = this.txtWorkCenterCode.Text.Trim();                                             // 작업장 코드
                string sStartDate      = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);                          // 생산시작일자
                string sEndDate        = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);                            // 생산  끝일자

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);        // 사업장(공장)    
                param[1] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);         // 작업자          
                param[2] = helper.CreateParameter("@LineCode", txtLineCode.Text, SqlDbType.VarChar, ParameterDirection.Input);        // 생산시작일자    
                param[3] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);          // 생산  끝일자    
                param[4] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param[5] = helper.CreateParameter("@MoldCode", txtMoldCode.Text, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param[6] = helper.CreateParameter("@MoldName", txtMoldName.Text, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                
                rtnDtTemp = helper.FillTable("USP_PP1000_S1N_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

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
        private void PP1000_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅

            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "div", "", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MOLDCODE", "금형코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "cartype", "차종", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품명", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MOLDNAME", "형번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "cavity", "Cavity", false, GridColDataType_emu.Integer, 60, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAPPINGQTY", "Shot수", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PRODQTY", "생산수량", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKTIME", "작업시간(분)", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "공정명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
           
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");


            #endregion

            #region Grid MERGE                     
            
            //grid1.Columns["MOLDCODE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["MOLDCODE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["MOLDCODE"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["MOLDNAME"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["MOLDNAME"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["MOLDNAME"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["ITEMCODE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ITEMCODE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ITEMCODE"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["ITEMNAME"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ITEMNAME"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ITEMNAME"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["LineName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineName"].MergedCellStyle = MergedCellStyle.Always;


            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["WORKCENTERNAME"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WORKCENTERNAME"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WORKCENTERNAME"].MergedCellStyle = MergedCellStyle.Always;

            //cboPlantCode_H.SelectedValue = this.PlantCode;
            #endregion Grid MERGE
        }
        #endregion
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "MAPPINGQTY", "PRODQTY", "WORKTIME" });

        }
 
        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["MOLDNAME"].Value.ToString() == "계")
                e.Row.Appearance.BackColor = Color.LightCyan;

        }

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
       

     

        #endregion

 
    }
}