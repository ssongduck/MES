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
    public partial class QM8020 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp2 = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp3 = new DataTable(); // return DataTable 공통

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        // private DataTable DtChange = null;

        DataTable DtChange = new DataTable();
        DataTable DtChange1 = new DataTable();
        DataTable DtChange2 = new DataTable();
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private ChartLayerAppearance lineLayer = new ChartLayerAppearance();
        private ChartLayerAppearance lineLayer2 = new ChartLayerAppearance();
        #endregion

        public QM8020(string PlantCode, string WorkDT, string WorkCenterCode, string ItemCode, string CP_GRADE, string WorkCenterName, string ItemName)
        {
            InitializeComponent();

            GridInit();

            //this.txtItemCode.Text = ItemCode;
            //this.txtItemName.Text = ItemName;
            this.txtWorkCenterCode.Text = WorkCenterCode;
            this.txtWorkCenterName.Text = WorkCenterName;
            WorkDT = (DateTime.Parse(WorkDT)).ToString("yyyy-MM-dd");
            this.txtWorkDT_H.Text = WorkDT;

            DoSearch(PlantCode, WorkDT, WorkCenterCode, ItemCode, CP_GRADE);
        }

        private void GridInit()
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품목코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCode", "검사코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "검사명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Spec", "SPEC", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "USL", "USL", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LSL", "LSL", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CP", "CP", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CP_Grade", "CP등급", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
           
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            #endregion
        }

        private void DoSearch(string sPlantCode, string sWorkDT, string sWorkCenterCode, string sItemCode, string sCP_GRADE)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[1] = helper.CreateParameter("FrDT", sWorkDT, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param[2] = helper.CreateParameter("ToDT", sWorkDT, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                param[3] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[4] = helper.CreateParameter("ItemCode", sItemCode.Trim().ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[5] = helper.CreateParameter("CP_GRADE", sCP_GRADE, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                rtnDtTemp = helper.FillTable("USP_QM2110_S3_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();
                DtChange = rtnDtTemp;


                DataTable rtnGridCombo = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
                SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnGridCombo, "CODE_ID", "CODE_NAME");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
