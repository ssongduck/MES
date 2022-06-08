#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : QM2700
//   Form Name      : 측정 ROW DATA 
//   Name Space     : SAMMI.QM
//   Created Date   : 2012.03.09
//   Made By        : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description    : 
//   DB Table       : QM2700
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

namespace SAMMI.QM
{
    public partial class QM2700 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        private byte[] FileImage;                    //이미지파일 이진데이터
        private string FileImagePath = string.Empty;
        

        #endregion

        #region < CONSTRUCTOR >
        public QM2700()
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

            GridIni();

            #region <콤보파일 셋팅>
            _rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.Common.FillComboboxMaster(this.cboPl, _rtnDtTemp, _rtnDtTemp.Columns["CODE_ID"].ColumnName, _rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", _rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            _rtnDtTemp = _Common.GET_TBM0000_CODE("PRODTYPE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ProdType", _rtnDtTemp, "CODE_ID", "CODE_NAME");

            _rtnDtTemp = _Common.GET_TBM0000_CODE("DOUTTYPE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "OutType", _rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            this.cboPl.SelectedIndex = 1;
            #endregion

            #region <POPUP>
            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { cboPlantCode_H, "", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
           , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
           , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
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
            SqlParameter[] param = new SqlParameter[9];
            SqlParameter[] param2 = new SqlParameter[3];
            string[] Data = new string[67];

            if (txtWorkCenterCode.Text == string.Empty 
                || SqlDBHelper.nvlString(this.cboPlantCode_H.Value).Equals(string.Empty)) //|| txtItemCode.Text == string.Empty)
            {
                ShowDialog("사업장, 작업장은 필수 입력사항입니다.", Windows.Forms.DialogForm.DialogType.OK);
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
                string hipisflag = "p";

                this.PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                this.WorkCenterCode = sWorkCenterCode;
                this.FRDT = planstartdt;
                this.TODT = planenddt;
                string OKNG = SqlDBHelper.nvlString(cboJudge_H.Value);


                param[0] = helper.CreateParameter("@PlantCode", PlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@WorkCenterCode", WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@FRDT", FRDT, SqlDbType.DateTime, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@TODT", TODT, SqlDbType.DateTime, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@DayNight", DayNight, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@OKNG", OKNG, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@SerialNo", txtSerialNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                param[7] = helper.CreateParameter("@LotNo", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                param[8] = helper.CreateParameter("@ItemCode", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);


                //rtnDtTemp = helper.FillTable("USP_QM2700_S1N", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_QM2702_S2N_UNION", CommandType.StoredProcedure, param);
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
                //if (this.txtItemCode.Text != "")
                //{
                //그리드 셋팅
                param2[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);
                param2[1] = helper.CreateParameter("@WorkCenterCode", this.txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                param2[2] = helper.CreateParameter("@ItemCode", this.txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                _GridTable = helper.FillTable("USP_QM2700_S2NGRID_UNION", CommandType.StoredProcedure, param2);

                if (_GridTable.Rows.Count > 0)
                {
                    //있는 컬럼만큼 채움
                    for (int i = 0; i < _GridTable.Rows.Count; i++)
                    {
                        Data[i] = _GridTable.Rows[i][0].ToString();
                    }

                    // for (int j = _GridTable.Rows.Count; j < 67; j++)
                    //{
                    //    Data[j] = "측정값" + (j + 1).ToString();
                    //}

                    GridIni2(Data);
                    _GridTable.Clear();
                }
                else
                {
                    for (int j = 0; j < 67; j++)
                    {
                        Data[j] = "측정값" + (j + 1).ToString();
                    }
                    GridIni2(Data);

                }
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
                if (param2 != null) { param2 = null; }
                grid1.DisplayLayout.CaptionAppearance.BackColor = Color.White;
                //}
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
        private void QM2700_Load(object sender, EventArgs e)
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

            _GridUtil.InitColumnUltraGrid(grid1, "SerialNo", "타각번호(S/N)", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 123, 10, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 140, 40, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품목코드", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목명", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OutLot", "출하Lot", false, GridColDataType_emu.VarChar, 130, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "InPoint", "투입위치", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "InTime", "투입시간", false, GridColDataType_emu.DateTime24, 170, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OutTime", "배출시각", false, GridColDataType_emu.DateTime24, 170, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdType", "생산유형", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "SeqID", "일련번호", false, GridColDataType_emu.Integer, 72, 30, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OKQty", "판정", false, GridColDataType_emu.VarChar, 60, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "NGQty", "불량수량", false, GridColDataType_emu.Integer, 72, 30, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data01", "측정값01", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data02", "측정값02", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data03", "측정값03", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data04", "측정값04", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data05", "측정값05", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data06", "측정값06", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data07", "측정값07", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data08", "측정값08", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data09", "측정값09", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data10", "측정값10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data11", "측정값11", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data12", "측정값12", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data13", "측정값13", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data14", "측정값14", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data15", "측정값15", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data16", "측정값16", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data17", "측정값17", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data18", "측정값18", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data19", "측정값19", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data20", "측정값20", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data21", "측정값21", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data22", "측정값22", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data23", "측정값23", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data24", "측정값24", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data25", "측정값25", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data26", "측정값26", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data27", "측정값27", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data28", "측정값28", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data29", "측정값29", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data30", "측정값30", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data31", "측정값31", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data32", "측정값32", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data33", "측정값33", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data34", "측정값34", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data35", "측정값35", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data36", "측정값36", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data37", "측정값37", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data38", "측정값38", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data39", "측정값39", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data40", "측정값40", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data41", "측정값41", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data42", "측정값42", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data43", "측정값43", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data44", "측정값44", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data45", "측정값45", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data46", "측정값46", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data47", "측정값47", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data48", "측정값48", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data49", "측정값49", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data50", "측정값50", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data51", "측정값51", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data52", "측정값52", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data53", "측정값53", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data54", "측정값54", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data55", "측정값55", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data56", "측정값56", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data57", "측정값57", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data58", "측정값58", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data59", "측정값59", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data60", "측정값60", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data61", "측정값61", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data62", "측정값62", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data63", "측정값63", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data64", "측정값64", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data65", "측정값65", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data66", "측정값66", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Data67", "측정값67", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Data68", "측정값68", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Data69", "측정값69", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Data70", "측정값70", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);

            //_GridUtil.InitColumnUltraGrid(grid1, "Mach01", "설비01", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach02", "설비02", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach03", "설비03", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach04", "설비04", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach05", "설비05", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach06", "설비06", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach07", "설비07", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach08", "설비08", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach09", "설비09", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach10", "설비10", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach11", "설비11", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach12", "설비12", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach13", "설비13", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach14", "설비14", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach15", "설비15", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach16", "설비16", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach17", "설비17", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach18", "설비18", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach19", "설비19", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach20", "설비20", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach21", "설비21", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach22", "설비22", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach23", "설비23", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach24", "설비24", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach25", "설비25", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach26", "설비26", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach27", "설비27", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach28", "설비28", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach29", "설비29", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach30", "설비30", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach31", "설비31", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach32", "설비32", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach33", "설비33", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach34", "설비34", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Mach35", "설비35", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "ReworkFlag", "재작업여부", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "OutType", "배출구분", false, GridColDataType_emu.VarChar,80, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "OutPoint", "배출포인트", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "PLCSerialNo", "PLC번호", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "Blank", "공타", false, GridColDataType_emu.VarChar, 60, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "MES_IFFlag", "MES IF여부", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "MES_IFTime", "MES IF시각", false, GridColDataType_emu.DateTime24, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "SPC_IFFlag", "SPC IF여부", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "SPC_IFTime", "SPC IF시각", false, GridColDataType_emu.DateTime24, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;


            //grid1.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.White;
            //grid1.UseAppStyling = false;
            _GridUtil.SetInitUltraGridBind(this.grid1);
            #endregion
        }

        private void GridIni2(string[] Data)
        {
            for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            {
                if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "Data01")
                {
                    data01 = i;
                }
            }
            int k = 0;
            for (int j = data01; j < this.grid1.DisplayLayout.Bands[0].Columns.Count - 1; j++)
            {
                if (Data[k] == null)
                {
                    grid1.DisplayLayout.Bands[0].Columns[j].Hidden = true;
                }
                else
                {
                    grid1.DisplayLayout.Bands[0].Columns[j].Hidden = false;
                    grid1.DisplayLayout.Bands[0].Columns[j].Header.Caption = Data[k];
                }
                k++;
            }

            
        }

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "ItemName")
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

        private void grid1_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            try
            {
                Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
               
                if (appConfig.AppSettings.Settings["SITE"].Value == "EC" || txtWorkCenterCode.Text != "4401")
                    return;

                _rtnDtTemp = _Common.GET_TBM0000_CODE("FILESVINFO");

                _rtnDtTemp.DefaultView.RowFilter = "CODE_ID='VSSV'";
                string ip = _rtnDtTemp.DefaultView[0]["RelCode1"].ToString();
                string id = _rtnDtTemp.DefaultView[0]["RelCode2"].ToString();
                string pw = _rtnDtTemp.DefaultView[0]["RelCode3"].ToString();
                string folder= _rtnDtTemp.DefaultView[0]["RelCode4"].ToString();

                this.pbDrawing_H.Image = null;

                string FileName = string.Empty;

                if (grid1.ActiveRow.Cells["SerialNo"].Value.ToString().Equals(string.Empty))
                    return;

                FileName = grid1.ActiveRow.Cells["SerialNo"].Value.ToString() + ".jpg";

 
                FileImagePath = @"\\" + ip + @"\"
                                + folder + @"\"
                                + FileName.Substring(0,3) + @"\" + FileName;

                if (System.IO.Directory.Exists("\\\\" + ip + "\\" + folder) == false) // 이미 열려있으면 넘어간다.
                {
                    System.Diagnostics.Process.Start("cmd.exe", "/C Net Use \\\\"+ip+"\\"+folder+" /user:"+id+" "+pw);
                }
                using (Image image = Image.FromFile(FileImagePath))
                {
                    pbDrawing_H.Image = new Bitmap(image);
                    this.pbDrawing_H.Visible = true;
                }
            }
            catch(Exception ex)
            {
                
            }
        }


        private void pbDrawing_H_DoubleClick(object sender, EventArgs e)
        {
            this.pbDrawing_H.Image = null;
            this.pbDrawing_H.Visible = false;
        }

        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["OKQty"].Value.ToString()== "NG")
            {
                e.Row.Appearance.ForeColor = Color.Red;
            }
        }
    }
}
