#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : PP9500
//   Form Name      : 측정 ROW DATA 
//   Name Space     : SAMMI.QM
//   Created Date   : 2012.03.09
//   Made By        : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description    : 
//   DB Table       : PP9500
//   StoreProcedure : 
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
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
#endregion

namespace SAMMI.PP
{
    public partial class PP9500 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        #region < CONSTRUCTOR >
        public PP9500()
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

            #region <콤보파일 셋팅>
            _rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", _rtnDtTemp, "CODE_ID", "CODE_NAME");

            //this.cboPlantCode_H.SelectedIndex = 1;
            #endregion

            #region <POPUP>
            btbManager = new BizTextBoxManagerEX();

            if(LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { cboPlantCode_H, "2", "", "" }
                       , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                   , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
            
            }
            #endregion

            //grid1.DisplayLayout.CaptionAppearance.BackColor = Color.White;
        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];
            SqlParameter[] param2 = new SqlParameter[2];
            string[] Data = new string[67];

            if(txtWorkCenterCode.Text == string.Empty
                || SqlDBHelper.nvlString(this.cboPlantCode_H.Value).Equals(string.Empty))
            {
                ShowDialog("사업장, 작업장을 입력하세요.", Windows.Forms.DialogForm.DialogType.OK);
                return;
            }

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


                param[0] = helper.CreateParameter("@pPlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@pWorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@FRDT", sFRDT, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@TODT", sTODT, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@DayNight", DayNight, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_PP9500_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP9500_S1_UNION", CommandType.StoredProcedure, param);
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
                //그리드 셋팅
                param2[0] = helper.CreateParameter("@pWorkCenterCode", this.txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                param2[1] = helper.CreateParameter("@pPlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);
                _GridTable = helper.FillTable("USP_PP9500_S0_UNION", CommandType.StoredProcedure, param2);

                if (_GridTable.Rows.Count > 0)
                {
                    //있는 컬럼만큼 채움
                    for (int i = 0; i < _GridTable.Rows.Count; i++)
                    {
                        Data[i] = _GridTable.Rows[i][0].ToString();
                    }

                    //for (int j = _GridTable.Rows.Count; j < 67; j++)
                    //{
                    //    Data[j] = "측정값" + (j + 1).ToString();
                    //}
                    //for (int j = _GridTable.Rows.Count; j < 31; j++)
                    //{
                    //    Data[j] = "측정값" + (j + 1).ToString();
                    //}

                    GridIni2(Data);
                    _GridTable.Clear();
                }

                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
                if (param2 != null) { param2 = null; }
                grid1.DisplayLayout.CaptionAppearance.BackColor = Color.White;
            }
        }

        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            base.DoNew();
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            try
            {
                //this.grid2.UpdateData();

                
                    if (this.ShowDialog("Q00009") == System.Windows.Forms.DialogResult.Cancel)
                        return;

                else return;
            }
            catch (SException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
            }
        }

        
        #endregion

        #region < EVENT AREA >
        /// <summary>
        /// Form이 Close 되기전에 발생
        /// e.Cancel을 true로 설정 하면, Form이 close되지 않음
        /// 수정 내역이 있는지를 확인 후 저장여부를 물어보고 저장, 저장하지 않기, 또는 화면 닫기를 Cancel 함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            this.DoSave();
        }

        /// <summary>
        /// DATABASE UPDATE전 VALIDATEION CHECK 및 값을 수정한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            if ((e.Row.RowState == DataRowState.Added) || (e.Row.RowState == DataRowState.Modified))
            {
                this.DoNewValidate(e.Row);
            }

            // 등록자, 수정자 정보 등록
            if (e.Row.RowState == DataRowState.Modified)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                return;
            }

            if (e.Row.RowState == DataRowState.Added)
            {
                e.Command.Parameters["@Maker"].Value = this.WorkerID;
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                return;
            }
        }

        /// <summary>
        /// 저장처리시 오류가 발생한 경우 오류 메세지에 대한 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Errors == null) return;

            switch (((SqlException)e.Errors).Number)
            {
                // 중복
                case 2627:
                    e.Row.RowError = this.FormInformation.GetMessage("E00004");
                    throw (new SException("E00004", e.Errors));
                // NULL 오류
                case 515:
                    e.Row.RowError = this.FormInformation.GetMessage("E00003");
                    throw (new SException("E00003", e.Errors));
                default:
                    break;
            }
        }

        private void grid1_KeyDown(object sender, KeyEventArgs e)
        {
            string enter = e.KeyCode.ToString();
            if (enter == "Return")
            {
                Infragistics.Win.UltraWinGrid.GridKeyActionMapping KeyMapping1 = new Infragistics.Win.UltraWinGrid.GridKeyActionMapping(Keys.Tab, Infragistics.Win.UltraWinGrid.UltraGridAction.NextCellByTab, 0, 0, 0, 0);
            }
        }
        #endregion

        #region <METHOD AREA>
        /// <summary>
        /// 행의 신규 등록시 오류 CHECK
        /// </summary>
        private void DoNewValidate(DataRow row)
        {
            // 입력항목에 대한 VALIDATION CHECK
            if (row["ItemCode"].ToString() == "")
            {
                row.RowError = this.FormInformation.GetMessage("E00003");
                throw (new SException("E00003", null));
            }

            if (row["PlantCode"].ToString() == "")
            {
                row.RowError = this.FormInformation.GetMessage("E00005");
                throw (new SException("E00005", null));
            }
        }
        #endregion

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            DateTime planstartdt = Convert.ToDateTime(((DateTime)this.calRegDT_FRH.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");
            DateTime planenddt = Convert.ToDateTime(((DateTime)this.calRegDT_TOH.Value).ToString("yyyy-MM-dd") + " 23:59:59.99");

        }

        private void SetGrid2Header(string dapointid)
        {
            string colname = string.Empty;
            int j = 0;
            for (int i = 1; i <= 55; i++)
            {
                colname = "DATA" + i.ToString("00");
            
            }
        }
        private void PP9500_Load(object sender, EventArgs e)
        {
            #region < Grid1 >
            

            #endregion
        }

        private void GridIni()
        {
            #region <Grid1 Setting>
            _GridUtil.InitializeGrid(this.grid1);


            //_GridUtil.InitColumnUltraGrid(grid1, "Spec", "규격", false, GridColDataType_emu.Double, 80, 100, Infragistics.Win.HAlign.Default, true, true, null, "#,##0.###", null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "USL", "규격상한선", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, "#,##0.###", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "LSL", "규격하한선", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "작업일자", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DayNight", "주야", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "1", "1", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "2", "2", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "3", "3", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "4", "4", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "5", "5", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "6", "6", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "7", "7", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "8", "8", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "9", "9", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "10", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "11", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "12", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "13", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "14", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "15", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "16", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "17", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "18", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "19", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "20", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "21", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "22", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "23", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "24", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "25", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "26", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "27", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "28", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "29", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "30", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "31", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "32", "10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
        
            
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

        private void GridIni2(string[] Data)
        {
            for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            {
                if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "1")
                {
                    data01 = i;
                }
            }
            int k = 0; 
            for (int j = data01; j < 33; j++)
            {

                grid1.DisplayLayout.Bands[0].Columns[j].Header.Caption = Data[k];
                k++;
            }

            
        }

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "DayNight")
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
