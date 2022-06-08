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
using Infragistics.UltraChart.Resources.Appearance;
#endregion

namespace SAMMI.QM
{
    public partial class QM1101 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        // private DataTable DtChange = null;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private DateTime FRDT = System.DateTime.Now;
        private DateTime TODT = System.DateTime.Now;
        private string PlantCode = string.Empty;
        public QM1101()
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


            GridInit();

            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
                //품목 팝업
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { cboPlantCode_H, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
                //품목 팝업
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("DAYNIGHT");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Shift", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("WORKTYPE2");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "WorkType", rtnDtTemp, "CODE_ID", "CODE_NAME");

        }

        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[7];
            DateTime StartInspDT = Convert.ToDateTime(((DateTime)this.calRegDT_FRH.Value).ToString("yyyy-MM-dd") + " 08:00:00.00");
            DateTime EndInspDT = Convert.ToDateTime(((DateTime)this.calRegDT_TOH.Value).AddDays(1).ToString("yyyy-MM-dd") + " 07:59:59.99");
            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                           // 공장 코드
                string sWorCenterCode = this.txtWorkCenterCode.Text.Trim();                                           // 작업라인 코드                                                         
                string sItemCode = SqlDBHelper.nvlString(this.txtItemCode.Text);
                string sDayNight = SqlDBHelper.nvlString(this.cboDayNight.Value);
                string sWorkType = SqlDBHelper.nvlString(this.cboWorkType.Value);
                this.FRDT = StartInspDT;
                this.TODT = EndInspDT;

                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);                  // 사업장(공장)       
                param[1] = helper.CreateParameter("WorkCenterCode", sWorCenterCode, SqlDbType.VarChar, ParameterDirection.Input);  // 작업라인 코드
                param[2] = helper.CreateParameter("StartDate", FRDT, SqlDbType.DateTime, ParameterDirection.Input);                    // 생산시작일자    
                param[3] = helper.CreateParameter("EndDate", TODT, SqlDbType.DateTime, ParameterDirection.Input);                      // 생산  끝일자 
                param[4] = helper.CreateParameter("DayNight", sDayNight, SqlDbType.VarChar, ParameterDirection.Input);                      // 생산  끝일자 
                param[5] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);                      // 생산  끝일자 
                param[6] = helper.CreateParameter("WorkType", sWorkType, SqlDbType.VarChar, ParameterDirection.Input);                      // 생산  끝일자 
               

                rtnDtTemp = helper.FillTable("USP_QM1100_S2N_UNION", CommandType.StoredProcedure, param);

                if (rtnDtTemp.Rows.Count == 0)
                {
                    // MessageBox.Show("DATA가 없습니다.");
                }
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                _Common.Grid_Column_Width(this.grid1); //grid 정리용   

              
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

        private void GridInit()
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 70, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
             _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachCode", "설비코드", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCode", "검사항목코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "검사항목명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
          //  _GridUtil.InitColumnUltraGrid(grid1, "InspDT", "검사시각", false, GridColDataType_emu.DateTime24, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Shift", "주야", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
          //  _GridUtil.InitColumnUltraGrid(grid1, "Inspector", "검사자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkType", "작업구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspVal_D", "건수(주)", false, GridColDataType_emu.Double, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspVal_N", "건수(야)", false, GridColDataType_emu.Double, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspVal", "건수(계)", false, GridColDataType_emu.Double, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
          //  _GridUtil.InitColumnUltraGrid(grid1, "USL", "USL", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
         //   _GridUtil.InitColumnUltraGrid(grid1, "LSL", "LSL", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            

            _GridUtil.SetInitUltraGridBind(grid1);

            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            #endregion

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
           
            //grid1.Columns["ItemCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemCode"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["ItemName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["ItemName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["ItemName"].MergedCellStyle = MergedCellStyle.Always;
           // grid1.DisplayLayout.UseFixedHeaders = true;
           // for (int i = 0; i < 10; i++)
           //     grid1.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;
        }

    }
}
