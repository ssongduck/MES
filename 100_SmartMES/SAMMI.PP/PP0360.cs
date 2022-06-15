#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  PP0360
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
    //추가됨
    public partial class PP0360 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        DataTable DtChange = new DataTable();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        public PP0360()
        {
            InitializeComponent();

            this.txtPlantCode.Text = "[" + LoginInfo.UserPlantCode + "] " + LoginInfo.UserPlantName;
            if (LoginInfo.UserID.StartsWith("31") == false)
            {
                btbManager = new BizTextBoxManagerEX();

                //            btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.UserPlantCode, "" });
                //btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.UserPlantCode, txtOPCode, "", "" }
                //         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.UserPlantCode, "", "" }
                         , new string[] { "", "" }, new object[] { });
            }

        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                DtChange.Clear();

                string sPlantCode = LoginInfo.UserPlantCode;                                        // 사업장(공장)
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);          // 생산시작일자
                string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);              // 생산  끝일자
                string sWorkCenterCode = this.txtWorkCenterCode.Text.Trim();                        // 작업장 코드
                 string sItemCode = this.txtItemCode.Text.Trim();                                   // 품목코드

                base.DoInquire();

                param[0] = helper.CreateParameter("@as_plantcode",  sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@as_workcenter", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@as_begin",      sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@as_end",        sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@as_itemcode",   sItemCode, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_PP0360_S1N", CommandType.StoredProcedure, param);
   
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


        #region 폼 로더
        private void PP0360_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, false, false, "", false);
            
            //90 95 160 70 170 100 170 100 100 140 200 80 50 100 130 130 80 100 100 100 100 140 100 
            //_GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "OutDate", "작업일시", false, GridColDataType_emu.DateTime24, 130, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "CarType", "차종", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ItemCode_Out", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ItemNm_Out", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "LotNO_Out", "LOT번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "InQty", "투입수량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "OutQty", "생산수량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,###", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ItemCode_In", "투입품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ItemNm_In", "투입품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "LotNO_In", "투입LOT번호", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "InDate", "투입일시", false, GridColDataType_emu.DateTime24, 130, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);


            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode",      "사업장",     false, GridColDataType_emu.VarChar,    90, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장",     false, GridColDataType_emu.VarChar,    70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldNo",         "금형",       false, GridColDataType_emu.VarChar,    70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);                        
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode",       "품번",       false, GridColDataType_emu.VarChar,    100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName",       "품명",       false, GridColDataType_emu.VarChar,    150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkNo",         "작업번호",   false, GridColDataType_emu.VarChar,    130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkQty",        "작업수량",   false, GridColDataType_emu.VarChar,    130, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkStatus",     "작업상태",   false, GridColDataType_emu.VarChar,    130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);                        
            _GridUtil.InitColumnUltraGrid(grid1, "InDate",         "투입일시",   false, GridColDataType_emu.DateTime24, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OutDate",        "완료일시",   false, GridColDataType_emu.DateTime24, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);            
 
              
            #endregion

            _GridUtil.SetInitUltraGridBind(grid1);

 
        }

       #endregion

        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["WorkStatus"].Value.ToString() == "완료")
            {
                e.Row.Cells["WorkStatus"].Appearance.BackColor = Color.Lime;
                e.Row.Cells["WorkStatus"].Appearance.ForeColor = Color.FromArgb(70, 70, 70);
            }
            else
            {
                e.Row.Cells["WorkStatus"].Appearance.BackColor = Color.OrangeRed;                
            }
        }

  
    }
}
