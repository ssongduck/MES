#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : 
//   Form Name    : 
//   Name Space   : 
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
using SAMMI.PopUp;
using SAMMI.PopManager;
using SAMMI.Common;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.CM
{
    public partial class CM0710 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
      
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
        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >

        public CM0710()
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
                btbManager.PopUpAdd(txtMachCode, txtMachName, "TBM0700", new object[] { this.cboPlantCode_H, "", "", "", "" });
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
            }
            else
            {
                btbManager.PopUpAdd(txtMachCode, txtMachName, "TBM0700", new object[] { LoginInfo.PlantAuth, "", "", "", "" });
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
            }

        }

        private void CM0710_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "div", "구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M01", "01월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M02", "02월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M03", "03월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M04", "04월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M05", "05월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M06", "06월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M07", "07월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M08", "08월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M09", "09월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M10", "10월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M11", "11월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "M12", "12월", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.#0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "gu", "gu", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
              
            
            _GridUtil.SetInitUltraGridBind(grid1);
            
            //그리드 라인 색깔 해제
            //grid1.UseAppStyling = false;
            grid1.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;
            grid1.DisplayLayout.Override.SelectTypeCell = SelectType.None;

            DtChange = (DataTable)grid1.DataSource;



            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            //SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, null, "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion

            #endregion Grid 셋팅
        }

       
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
              //SqlDBHelper helper = new SqlDBHelper(false,"Data Source=192.168.100.20;Initial Catalog=MTMES;User ID=sa;Password=qwer1234!~");
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[8];

            try
            {
                DtChange.Clear();
               
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sMachCode   = txtMachCode.Text.Trim();  
                string sWMachName  = txtMachName.Text.Trim();  
                string sdate       = cbo_date.Text.Trim().ToString();
                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);           
                param[1] = helper.CreateParameter("@MACHCODE", sMachCode, SqlDbType.VarChar, ParameterDirection.Input);            
                param[2] = helper.CreateParameter("@MACHNAME", sWMachName, SqlDbType.VarChar, ParameterDirection.Input);       
                param[3] = helper.CreateParameter("@DATE", sdate, SqlDbType.VarChar, ParameterDirection.Input);       
                param[4] = helper.CreateParameter("@OPCode",txtOPCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@WorkCenterCode", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
 
                param[6] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                param[7] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                rtnDtTemp = helper.FillTable("USP_CM0710_S1_UNION", CommandType.StoredProcedure, param);

               
                _GridUtil.SetInitUltraGridBind(grid1);

                grid1.DataSource = rtnDtTemp;
   
                grid1.DataBind();

           


                DtChange = rtnDtTemp;
                //_Common.Grid_Column_Width(this.grid1); //grid 정리용
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
            }
        }
        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
         
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
           
        }
        #endregion

        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ugr = grid1.DoSummaries(new string[] { "macnt", "maTime", "FaultTime", "worktime", "nonworktime" });

        }
        #region <METHOD AREA>

         
        #endregion
    }
}
