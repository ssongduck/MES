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
    public partial class QM9400 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        // private DataTable DtChange = null;

        DataTable DtChange = new DataTable();
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        private string PlantCode = string.Empty;
        #endregion

        public QM9400()
        {
            InitializeComponent();

            GirdInit();

            btbManager = new BizTextBoxManagerEX();

            btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { "SK2", "", "", "" }
                  , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });

            btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { "SK2", txtWorkCenterCode, txtWorkCenterName }
                , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            //btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { "SK2", "", "", "" });
            btbManager.PopUpAdd(txtInspCode, txtInspName, "TBM1500", new object[] { "SK2", txtInspCode, txtInspName, "" }
                , new string[] { "InspCode", "InspName" }, new object[] { });
        }

        private void GirdInit()
        {
            _GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitColumnUltraGrid(grid1, "InspDT", "검사일자", false, GridColDataType_emu.YearMonthDay, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspTime", "검사시각", false, GridColDataType_emu.VarChar, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IFTime", "IF시각", false, GridColDataType_emu.DateTime24, 90, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 120, 30, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CarType", "차종", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SerialNo", "S/N", false, GridColDataType_emu.VarChar, 120, 0, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 140, 10, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 140, 10, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOpCode", "Op코드", false, GridColDataType_emu.VarChar, 90, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterOpName", "OP", false, GridColDataType_emu.VarChar, 150, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCodeSK", "검사항목코드(삼기)", false, GridColDataType_emu.VarChar, 120, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCode", "검사항목코드", false, GridColDataType_emu.VarChar, 120, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "검사항목", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkType", "검사구분", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspVal", "측정치", false, GridColDataType_emu.Double, 70, 0, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Judge", "판정", false, GridColDataType_emu.VarChar, 60, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "USL", "USL", false, GridColDataType_emu.Double, 50, 0, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LSL", "LSL", false, GridColDataType_emu.Double, 50, 0, Infragistics.Win.HAlign.Right, true, false, "#,##0.###", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IFFlag", "전송여부", false, GridColDataType_emu.VarChar, 70, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            rtnDtTemp = _Common.GET_TBM0000_CODE("WorkTypeII");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "WorkType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("DAYNIGHT");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Shift", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "IFFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            DtChange = (DataTable)grid1.DataSource;
            _GridUtil.SetInitUltraGridBind(this.grid1);


            //row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

        }
        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[8];

            try
            {
                base.DoInquire();


                //string sStartDate = SqlDBHelper.nvlDateTime(this.cboStartDate_H.Value).ToString("yyyy-MM-dd");
                string sStartDate = string.Format("{0:yyyy-MM-dd}", cboStartDate_H.Value);
                string sEndDate   = string.Format("{0:yyyy-MM-dd}", cboEndDate_H.Value);
                string sItemCode  = SqlDBHelper.nvlString(this.txtItemCode.Text);
                string sWorkCenterCode = SqlDBHelper.nvlString(this.txtWorkCenterCode.Text);
                string sInspCode = SqlDBHelper.nvlString(this.txtInspCode.Text);
                string sSerialNo = SqlDBHelper.nvlString(this.txtSN.Text);
                string sWorkType = SqlDBHelper.nvlString(this.cboWorkType.Value);
                string sIFFlag   = SqlDBHelper.nvlString(this.cboIFFlag.Value);

                param[0] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@InspCode", sInspCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@SerialNo", sSerialNo, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("@WorkType", sWorkType, SqlDbType.VarChar, ParameterDirection.Input);
                param[7] = helper.CreateParameter("@IFFlag", sIFFlag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_QM9400_S1", CommandType.StoredProcedure, param);

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
            }
        }
        #endregion
    }
}
