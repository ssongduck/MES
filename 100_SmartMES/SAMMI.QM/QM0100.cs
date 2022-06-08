#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  QM0100
//   Form Name    : 작업장별 불량현황 정보 조회
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
    public partial class QM0100 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _RtnComDt = new DataTable();

        /// <summary>
        /// Change grid1 Datatable
        /// </summary>
        DataTable _ChangeDt = new DataTable();

        /// <summary>
        /// Biz text box manager EX
        /// </summary>
        BizTextBoxManagerEX btbManager;

        /// <summary>
        /// Grid object
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();
        
        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        /// <summary>
        /// Plant code
        /// </summary>
        private string _PlantCode = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// QM3000 constructor
        /// </summary>
        public QM0100()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();
        }

        #endregion

        #region Event

        #endregion

        #region Method

        /// <summary>
        /// Initialize Control
        /// </summary>
        private void InitializeControl()
        {
            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this._PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this._PlantCode.Equals("SK"))
            {
                this._PlantCode = "SK1";
            }
            else if (this._PlantCode.Equals("EC"))
            {
                this._PlantCode = "SK2";
            }
            if (!(this._PlantCode.Equals("SK1") || this._PlantCode.Equals("SK2")))
            {
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;
            }

            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { this.cboPlantCode_H, txtOpCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOpCode, txtLineCode, "" }
                    , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOpCode, txtOpName, txtLineCode, txtLineName });
                btbManager.PopUpAdd(txtErrorCode, txtErrorName, "TBM1000", new object[] { "", "", "Y" });
            }
            else
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { LoginInfo.PlantAuth, txtOpCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOpCode, txtLineCode, "" }
                    , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOpCode, txtOpName, txtLineCode, txtLineName });
                btbManager.PopUpAdd(txtErrorCode, txtErrorName, "TBM1000", new object[] { "", "", "Y" });
            }

        }

        /// <summary>
        /// Initialize Grild Control
        /// </summary>
        private void InitializeGridControl()
        {
            #region Grid 셋팅
            _UltraGridUtil.InitializeGrid(this.grid1);

            _UltraGridUtil.InitColumnUltraGrid(grid1, "RECDATE", "일자", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this._PlantCode == "") ? true : false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "OPNAME", "공정명", false, GridColDataType_emu.VarChar, 70, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "LINENAME", "라인명", false, GridColDataType_emu.VarChar, 100, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ORDERNO", "지시번호", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품번", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품명", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "LOTNO", "LotNO", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERRORCLASS", "불량유형", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERRORCODE", "불량코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERRORDESC", "불량명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERRORQTY", "불량수량", false, GridColDataType_emu.Double, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "DAYNIGHT", "주야", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "IMPUTE", "귀책처", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MAKEDATE", "등록시각", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MAKER", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid1);
            _ChangeDt = (DataTable)grid1.DataSource;
            #endregion

            #region Grid MERGE


            #endregion Grid MERGE

            #region 콤보박스

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            //SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("ERRORCLASS");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ERRORCLASS", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("ERRORTYPE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ERRORTYPE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "USEFLAG", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("DAYNIGHT");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DAYNIGHT", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("IMPUTE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "IMPUTE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion
        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
            SqlParameter[] sqlParameters = new SqlParameter[10];

            try
            {
                base.DoInquire();

                string _PlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                                                        // 사업장(공장)
                string _StartDate = SqlDBHelper.nvlDateTime(cboStartDate_H.Value).ToString("yyyy-MM-dd");                           // 생산시작일자
                string _EndDate = SqlDBHelper.nvlDateTime(cboEndDate_H.Value).ToString("yyyy-MM-dd");                               // 생산  끝일자
                string _WorkCenterCode = SqlDBHelper.nvlString(this.txtWorkCenterCode.Text.Trim());                                 // 작업장 코드
                string _ErrorCode = SqlDBHelper.nvlString(this.txtErrorCode.Text.Trim());                                           // 불량코드
                string _ErrorClass = SqlDBHelper.nvlString(this.cboErrorClass_H.Value);                                             // 불량유형
                string _DayNight = SqlDBHelper.nvlString(this.cboDayNight_H.Value);                                                 // 주야
                string _Impute = SqlDBHelper.nvlString(this.cboImpute_H.Value);                                                     // 귀책처
                string _OpCode = SqlDBHelper.nvlString(this.txtOpCode.Text.Trim());                                                     // 공정
                string _LineCode = SqlDBHelper.nvlString(this.txtLineCode.Text.Trim());                                                     // 라인


                sqlParameters[0] = sqlDBHelper.CreateParameter("PLANTCODE", _PlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                sqlParameters[1] = sqlDBHelper.CreateParameter("STARTDATE", _StartDate, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                sqlParameters[2] = sqlDBHelper.CreateParameter("ENDDATE", _EndDate, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                sqlParameters[3] = sqlDBHelper.CreateParameter("WORKCENTERCODE", _WorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                sqlParameters[4] = sqlDBHelper.CreateParameter("ERRORCODE", _ErrorCode, SqlDbType.VarChar, ParameterDirection.Input);             // 불량코드       
                sqlParameters[5] = sqlDBHelper.CreateParameter("ERRORCLASS", _ErrorClass, SqlDbType.VarChar, ParameterDirection.Input);           // 불량유형   
                sqlParameters[6] = sqlDBHelper.CreateParameter("DAYNIGHT", _DayNight, SqlDbType.VarChar, ParameterDirection.Input);           // 주야      
                sqlParameters[7] = sqlDBHelper.CreateParameter("IMPUTE", _Impute, SqlDbType.VarChar, ParameterDirection.Input);           // 귀책처
                sqlParameters[8] = sqlDBHelper.CreateParameter("OPCODE", _OpCode, SqlDbType.VarChar, ParameterDirection.Input);           // 공정       
                sqlParameters[9] = sqlDBHelper.CreateParameter("LINECODE", _LineCode, SqlDbType.VarChar, ParameterDirection.Input);           // 라인             

                _RtnComDt = sqlDBHelper.FillTable("USP_QM0100_S2N_UNION", CommandType.StoredProcedure, sqlParameters);

                grid1.DataSource = _RtnComDt;
                grid1.DataBind();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                if (sqlParameters != null) { sqlParameters = null; }
            }
        }

        /// <summary>
        /// Do BaseSum
        /// </summary>
        public override void DoBaseSum()
        {
            base.DoBaseSum();

            UltraGridRow ultraGridRow = grid1.DoSummaries(new string[] { "ERRORQTY" });
        }

        #endregion
    }
}

