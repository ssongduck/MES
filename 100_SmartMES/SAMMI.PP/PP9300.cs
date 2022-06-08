
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

namespace SAMMI.PP
{
    using SAMMI.Windows.Forms;

    public partial class PP9300 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;
        BizTextBoxManagerEX btbManager;
        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        private string PlantCode = string.Empty;

        public PP9300()
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
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
            }
            else
            {
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
            }
        }

        #region PP9300_Load
        private void PP9300_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPTYPE", "비가동유형", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
         //   _GridUtil.InitColumnUltraGrid(grid1, "STOPTYPENAME", "유형명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPCODE", "비가동코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPCODENAME", "비가동명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCODE", "공정", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "작업라인", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STATUSTIME", "시간(분)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
             
            _GridUtil.SetInitUltraGridBind(grid1);

            DtChange = (DataTable)grid1.DataSource;
            #endregion

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

         
            #endregion

            #region Grid MERGE

            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            ////grid1.Columns["OPCODE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            ////grid1.Columns["OPCODE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            ////grid1.Columns["OPCODE"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["STOPTYPE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["STOPTYPE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["STOPTYPE"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["STOPCODE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["STOPCODE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["STOPCODE"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["STOPCODENAME"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["STOPCODENAME"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["STOPCODENAME"].MergedCellStyle = MergedCellStyle.Always;

            #endregion Grid MERGE

   
        }
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "STATUSTIME" });

        }
        #endregion PP9300_Load

        #region 조회
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];

            grid1.DataSource = DtChange;
            grid1.DataBind();


            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                        // 사업장(공장)
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);
                string sOPCode = this.txtOPCode.Text.Trim();



                param[0] = helper.CreateParameter("@PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[1] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param[2] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                param[3] = helper.CreateParameter("@WorkCenterCode", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);               // 라인 코드       
                param[4] = helper.CreateParameter("@OPCODE", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드       
 

                //rtnDtTemp = helper.FillTable("USP_PP9300_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP9300_S1_UNION", CommandType.StoredProcedure, param);
                
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();
          
            
                //_Common.Grid_Column_Width(this.grid1); //grid 정리용   

                


            }
            catch (Exception ex)
            {
                this.ShowDialog(ex.ToString(), DialogForm.DialogType.OK);
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion 조회
        #region <METHOD AREA>
      


        #endregion

     

        


    }
}
