#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  QM1200
//   Form Name    : 공정검사 실적 내역
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


namespace SAMMI.QM
{
    public partial class QM1200 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        private string PlantCode = string.Empty;

        public QM1200()
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
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
            else
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                        , new string[] { "", "" }, new object[] { });
            
        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                         
                string sWorCenterCode = this.txtWorkCenterCode.Text.Trim();                                                                                                   
                string sShift = SqlDBHelper.nvlString(this.cboDayNight.Value);
                string sJudge = SqlDBHelper.nvlString(this.cboJudge.Value);
                string sStartDate = string.Format("{0:yyyy-MM-dd}", calRegDT_FRH.Value);                           
                string sEndDate = string.Format("{0:yyyy-MM-dd}", calRegDT_TOH.Value);                             

                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);                        
                param[1] = helper.CreateParameter("WorkCenterCode", sWorCenterCode, SqlDbType.VarChar, ParameterDirection.Input);  
                param[2] = helper.CreateParameter("Shift", sShift, SqlDbType.VarChar, ParameterDirection.Input);  
                param[3] = helper.CreateParameter("Judge", sJudge, SqlDbType.VarChar, ParameterDirection.Input); 
                param[4] = helper.CreateParameter("FRDT", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("TODT", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);     
                
                rtnDtTemp = helper.FillTable("USP_QM1200_S3_UNION", CommandType.StoredProcedure, param);

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


        #region 폼 로더
        private void QM1200_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 180, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCode", "감사코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "감사대상", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Spec", "감사내용", false, GridColDataType_emu.VarChar, 350, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Etc", "조치사항", false, GridColDataType_emu.VarChar, 250, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Judge", "판정", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Shift", "주야", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "감사자", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkDT", "감사일시", false, GridColDataType_emu.DateTime24, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            

            _GridUtil.SetInitUltraGridBind(grid1);

            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            #endregion

            grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;
            grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["Shift"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["Shift"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["Shift"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["Maker"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["Maker"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["Maker"].MergedCellStyle = MergedCellStyle.Always;

            grid1.Columns["WorkDT"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["WorkDT"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["WorkDT"].MergedCellStyle = MergedCellStyle.Always;

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("DAYNIGHT");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Shift", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("Judge1");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Judge", rtnDtTemp, "CODE_ID", "CODE_NAME");

        }
        #endregion

        







    }
}

