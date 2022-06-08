using SAMMI.Common;
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using Infragistics.Win.UltraWinGrid;
using SAMMI.PopUp;
using SAMMI.PopManager;
using System.Data.Common;

namespace SAMMI.PP
{
    public partial class PP9600 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>

        // 변수나 Form에서 사용될 Class를 정의
        private string PlantCode = string.Empty;
        private string WorkCenterCode = string.Empty;

        DataTable _rtnDtTemp = new DataTable();

        DataTable _GridTable = new DataTable();     //그리드 컬럼 리네임에 사용할 데이터테이블

        BizTextBoxManagerEX btbManager;
        UltraGridUtil _GridUtil = new UltraGridUtil();
        UltraGridUtil _GridUtil2 = new UltraGridUtil();
        private DateTime FRDT = System.DateTime.Now;
        private DateTime TODT = System.DateTime.Now;
        Common.Common _Common = new Common.Common();

        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        private DataTable DtChange = null;

        private int _Fix_Col = 0;
        private int data01 = 0;

        #endregion

        public PP9600()
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

            //this.db = DatabaseFactory.CreateDatabase();
            //this.conn = (SqlConnection)this.db.CreateConnection();
            //this.daTable1.Connection = conn;
            this.calRegDT_FRH.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00");
            this.calRegDT_TOH.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 23:59:59");
            //this.daTable1.Adapter.RowUpdating += new SqlRowUpdatingEventHandler(Adapter_RowUpdating);
            //this.daTable1.Adapter.RowUpdated += new SqlRowUpdatedEventHandler(Adapter_RowUpdated);
           
            // 사업장 사용권한 설정
            //_Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            GridIni();

            #region <POPUP>
            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "4", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "4", "", "" }
                   , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
            }
            #endregion

        }

        private void GridIni()
        {
            #region <Grid1 Setting>
            _GridUtil.InitializeGrid(this.grid1);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "작업일자", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerName", "작업자", false, GridColDataType_emu.VarChar, 70, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkName", "작업명", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProcessName", "공정명", false, GridColDataType_emu.VarChar, 70, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StartTime", "시작시각", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EndTime", "종료시각", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "TotalTime", "전체시간", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlanQty", "계획수량", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQty", "생산수량", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Note", "비고", false, GridColDataType_emu.VarChar, 200, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;


            //grid1.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.White;
            //grid1.UseAppStyling = false;
            _GridUtil.SetInitUltraGridBind(this.grid1);

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("DAYNIGHT");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DayNight", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            #endregion
        }


        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];
           
            try
            {
                base.DoInquire();

                DateTime planstartdt = Convert.ToDateTime(((DateTime)this.calRegDT_FRH.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");
                DateTime planenddt = Convert.ToDateTime(((DateTime)this.calRegDT_TOH.Value).ToString("yyyy-MM-dd") + " 23:59:59.99");
                string DayNight = SqlDBHelper.nvlString(cboDaynight_H.Value, "");

                if (Convert.ToInt32(planstartdt.ToString("yyyyMMdd")) > Convert.ToInt32(planenddt.ToString("yyyMMdd")))
                {
                    SException ex = new SException("R00200", null);
                    throw ex;
                }

                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();

                this.PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                this.WorkCenterCode = sWorkCenterCode;
                this.FRDT = planstartdt;
                this.TODT = planenddt;
                string sFRDT = FRDT.ToString("yyyy-MM-dd");
                string sTODT = TODT.ToString("yyyy-MM-dd");
                //string OKNG = SqlDBHelper.nvlString(cboJudge_H.Value);


                param[0] = helper.CreateParameter("@PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@WorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@FrDate", sFRDT, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@ToDate", sTODT, SqlDbType.VarChar, ParameterDirection.Input);
               
                //rtnDtTemp = helper.FillTable("USP_PP9600_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP9600_S1_UNION", CommandType.StoredProcedure, param);
                if (rtnDtTemp.Rows.Count == 0)
                {
                    // MessageBox.Show("DATA가 없습니다.");
                    grid1.DataSource = rtnDtTemp;
                    grid1.DataBind();
                }
                else
                {
                    grid1.DataSource = rtnDtTemp;
                    grid1.DataBind();

                    DtChange = rtnDtTemp;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
                grid1.DisplayLayout.CaptionAppearance.BackColor = Color.White;
            }
        }
    }
}
