#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : PP3820
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
    public partial class PP3820 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string PlantCode = string.Empty;

        public PP3820()
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
        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                      // 사업장(공장)
                string sWorkCenterCode = this.txtWorkCenterCode.Text.Trim();                                        // 작업장 코드
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);                          // 생산시작일자
                string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);                              // 생산  끝일자


                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_PP3820_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP3820_S1_UNION", CommandType.StoredProcedure, param);
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
        private void PP3820_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, false, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "REQUESTDATE", "발생일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STARTDATE", "보급요청시간", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MITEMCODE", "품목", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "소재", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "REQUESTSTARTDATE", "보급확인시간", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ENDDATE", "보급완료시간", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "TIME", "소요시간(분)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DISSSTATUS", "보급상태", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "REQUESTWORKERID", "보급확인자", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKERID", "보급작업자", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            
            
            
            _GridUtil.SetInitUltraGridBind(grid1);
            grid1.DisplayLayout.UseFixedHeaders = true;
            for (int i = 0; i < 3; i++)
                grid1.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;
            #endregion
            //grid1.Columns["REQUESTDATE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["REQUESTDATE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["REQUESTDATE"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["WORKCENTERCODE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WORKCENTERCODE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WORKCENTERCODE"].MergedCellStyle = MergedCellStyle.Always;

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            //cboPlantCode_H.SelectedValue = "SK1";
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("ItemType");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "InItemType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            #endregion
        }
        #endregion

        #region <METHOD AREA>
 

        #region 작업장(TBM0600) 팝업창에서 값 가져오기
        //////////////////     
        private void Search_Pop_TBM0600()
        {

            string sPlantCode = string.Empty;                             //사업장코드
            string sOPCode = "";                       //공정코드
            string sOPName = "";                       //공정명 
            string sLineCode = string.Empty;                              //라인코드
            string sWORKCENTERCODE = txtWorkCenterCode.Text.Trim();       //작업호기(라인)코드
            string sWorkCenterName = txtWorkCenterName.Text.Trim();       //작업호기(라인)명 
            string sUseFlag = string.Empty;                               //사용여부         


            if (this.cboPlantCode_H.Value != null)
                sPlantCode = cboPlantCode_H.Value.ToString() == "ALL" ? "" : cboPlantCode_H.Value.ToString();         ///사업장코드 

            //if (this.cboUseFlag_H.SelectedValue != null)
            //    sUseFlag = cboUseFlag_H.SelectedValue.ToString() == "ALL" ? "" : cboUseFlag_H.SelectedValue.ToString();                 // 사용여부
            sUseFlag = "";                 // 사용여부: 전체


            try
            {
                _biz.TBM0600_POP(sPlantCode, sWORKCENTERCODE, sWorkCenterName, sOPCode, sLineCode, sUseFlag, txtWorkCenterCode, txtWorkCenterName);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }
        #endregion

        private void txtWorkCenterCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkCenterName.Text = string.Empty;
        }

        private void txtWorkCenterCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0600();
            }
        }

        private void txtWorkCenterCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0600();
        }



        private void txtWorkCenterName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0600();
            }
        }

        private void txtWorkCenterName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0600();
        }


        #endregion

    

   

        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "InQty", "InUseQty", "StockQty" });

        }

    }
}
