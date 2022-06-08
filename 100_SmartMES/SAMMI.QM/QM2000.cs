#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : QM2000
//   Form Name      : 이상발생 조치현황 관리
//   Name Space     : SAMMI.QM
//   Created Date   : 2013.08.13
//   Made By        : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description    :
//   DB Table       : 
//   StoreProcedure :    USP_QM2000_S1
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
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using SAMMI.PopUp;
using SAMMI.PopManager;
#endregion

namespace SAMMI.QM
{
    public partial class QM2000 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        // 변수나 Form에서 사용될 Class를 정의

        DataTable _rtnDtTemp = new DataTable();
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        private DataTable DtChange = null;

        private string PlantCode = string.Empty;
        private string WorkCenterCode = string.Empty;
        private string InspCode = string.Empty;
        private string DASType = string.Empty;
        private string SPCJudge = string.Empty;
        private string HipisFlag = string.Empty;
        private DateTime FRDT = System.DateTime.Now;
        private DateTime TODT = System.DateTime.Now;

        BizTextBoxManagerEX btbManager;
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();


        private int _Fix_Col = 0;
        #endregion



        public QM2000()
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

            this.calRegDT_FRH.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00");
            this.calRegDT_TOH.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 23:59:59");

            #region <콤보파일 셋팅>
            _rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            //SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, _rtnDtTemp, _rtnDtTemp.Columns["CODE_ID"].ColumnName, _rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", _rtnDtTemp, "CODE_ID", "CODE_NAME");
            //this.cboPlantCode_H.SelectedIndex = 1;
            #endregion

            #region <POPUP>
            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtLineCode_H, txtLineName_H, "TBM0501", new object[] { this.cboPlantCode_H, "", "" });
                btbManager.PopUpAdd(txtWorkCenterCode_H, txtWorkCenterName_H, "TBM0600", new object[] { this.cboPlantCode_H, "", txtLineCode_H, "" }
                        , new string[] { "LineCode", "LineName" }, new object[] { txtLineCode_H, txtLineName_H });
                btbManager.PopUpAdd(txtItemCode_H, txtItemName_H, "TBM0100", new object[] { this.cboPlantCode_H, "", "" }
                         , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtInspCode_H, txtInspCodeNM_H, "TBM1500", new object[] { this.cboPlantCode_H, "", "", "", "" });
            }
            else
            {
                btbManager.PopUpAdd(txtLineCode_H, txtLineName_H, "TBM0501", new object[] { LoginInfo.PlantAuth, "", "" });
                btbManager.PopUpAdd(txtWorkCenterCode_H, txtWorkCenterName_H, "TBM0600", new object[] { LoginInfo.PlantAuth, "", txtLineCode_H, "" }
                        , new string[] { "LineCode", "LineName" }, new object[] { txtLineCode_H, txtLineName_H });
                btbManager.PopUpAdd(txtItemCode_H, txtItemName_H, "TBM0100", new object[] { LoginInfo.PlantAuth, "", "" }
                         , new string[] { "", "" }, new object[] { });
                btbManager.PopUpAdd(txtInspCode_H, txtInspCodeNM_H, "TBM1500", new object[] { LoginInfo.PlantAuth, "", "", "", "" });
            }

            #endregion

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

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                                                       // 사업장(공장)
                string sLineCode = SqlDBHelper.nvlString(this.txtLineCode_H.Text.Trim());                                           // 라인코드
                string sWorkCenterCode = SqlDBHelper.nvlString(this.txtWorkCenterCode_H.Text.Trim());                                               // 공정 코드
                string sItemCode = SqlDBHelper.nvlString(this.txtItemCode_H.Text.Trim());                                           // 품목 코드
                string sInspCode = SqlDBHelper.nvlString(this.txtInspCode_H.Text.Trim());                                           // 검사할 항목
                string sInspItem = SqlDBHelper.nvlString("");                                                                       // 측정할 항목
                string sfrRegDT = SqlDBHelper.nvlDateTime(calRegDT_FRH.Value).ToString("yyyy-MM-dd");                               // 이상발생 시작일자
                string stoRegDT = SqlDBHelper.nvlDateTime(calRegDT_TOH.Value).ToString("yyyy-MM-dd");                               // 이상발생 종료일자


                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("InspCode", sInspCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("InspItem", sInspItem, SqlDbType.VarChar, ParameterDirection.Input);
                param[6] = helper.CreateParameter("frRegDT", sfrRegDT, SqlDbType.VarChar, ParameterDirection.Input);
                param[7] = helper.CreateParameter("toRegDT", stoRegDT, SqlDbType.VarChar, ParameterDirection.Input);           

                rtnDtTemp = helper.FillTable("USP_QM2000_S1_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

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

        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;
            try
            {
                base.DoSave();

                string sActionDT = SqlDBHelper.nvlDateTime(cboActionDT.Value).ToString("yyyy-MM-dd");                                                                    // 사업장(공장)
                string sActionUserID = SqlDBHelper.nvlString(LoginInfo.UserID);                                           // 라인코드
                string sActionDesc = SqlDBHelper.nvlString(txtActionDesc.Text.Trim());                                               // 공정 코드
                string sCriticalNo = SqlDBHelper.nvlString(this.grid2.ActiveRow.Cells["CriticalNo"].Value);                                           // 품목 코드
                string sCriticalID = SqlDBHelper.nvlString(this.grid2.ActiveRow.Cells["CriticalID"].Value);                                           // 검사할 항목

                param = new SqlParameter[5];

                param[0] = helper.CreateParameter("ActionDT", sActionDT, SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                param[1] = helper.CreateParameter("ActionUserID", sActionUserID, SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                param[2] = helper.CreateParameter("ActionDesc", sActionDesc, SqlDbType.VarChar, ParameterDirection.Input);           // 설비코드
                param[3] = helper.CreateParameter("CriticalNo", sCriticalNo, SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                param[4] = helper.CreateParameter("CriticalID", sCriticalID, SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서

                helper.ExecuteNoneQuery("USP_QM2000_U1", CommandType.StoredProcedure, param);

                helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                helper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }

            DoInquire2();
        }

        #endregion

        private void DoInquire2()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[2];

            try
            {
                base.DoInquire();

                param[0] = helper.CreateParameter("CriticalNo", SqlDBHelper.gGetCode(this.grid1.ActiveRow.Cells["CriticalNo"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("PlantCode", SqlDBHelper.gGetCode(this.grid1.ActiveRow.Cells["PlantCode"].Value), SqlDbType.VarChar, ParameterDirection.Input);
                grid2.DataSource = helper.FillTable("USP_QM2000_S2_UNION", CommandType.StoredProcedure, param);
                grid2.DataBind();

                ClosePrgForm();
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

        private void QM2000_Load(object sender, EventArgs e)
        {
            #region <Grid Setting>
            _GridUtil.InitializeGrid(this.grid1,false,false,false,"",false);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CriticalDate", "이상발생일자", false, GridColDataType_emu.VarChar, 102, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CriticalNo", "이상발생번호", false, GridColDataType_emu.VarChar, 135, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 79, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 131, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 144, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 152, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspCode", "검사코드", false, GridColDataType_emu.VarChar, 66, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspName", "검사명", false, GridColDataType_emu.VarChar, 198, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspNO", "검사번호", false, GridColDataType_emu.VarChar, 135, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspVal", "검사값", false, GridColDataType_emu.Double, 53, 20, Infragistics.Win.HAlign.Right, true, false, "#,##0.######", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspSeqNO", "검사생성순번", false, GridColDataType_emu.VarChar, 92, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "USL", "USL", false, GridColDataType_emu.Double, 41, 20, Infragistics.Win.HAlign.Right, true, false, "#,##0.######", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LSL", "LSL", false, GridColDataType_emu.Double, 35, 20, Infragistics.Win.HAlign.Right, true, false, "#,##0.######", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CL", "CL", false, GridColDataType_emu.Double, 34, 20, Infragistics.Win.HAlign.Right, true, false, "#,##0.######", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SpecType", "관리규격", false, GridColDataType_emu.VarChar, 76, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UCL", "UCL", false, GridColDataType_emu.Double, 49, 20, Infragistics.Win.HAlign.Right, true, false, "#,##0.######", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LCL", "LCL", false, GridColDataType_emu.Double, 49, 20, Infragistics.Win.HAlign.Right, true, false, "#,##0.######", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SPEC", "SPEC", false, GridColDataType_emu.VarChar, 74, 20, Infragistics.Win.HAlign.Right, true, false, "#,##0.######", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remarks", "비고", false, GridColDataType_emu.VarChar, 40, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "생성자", false, GridColDataType_emu.VarChar, 64, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "생성일시", false, GridColDataType_emu.VarChar, 101, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 64, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일시", false, GridColDataType_emu.VarChar, 101, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            DtChange = (DataTable)grid1.DataSource;
            _GridUtil.SetInitUltraGridBind(this.grid1);
            _GridUtil.InitializeGrid(this.grid2, false, false, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid2, "CriticalNo", "이상발생번호", false, GridColDataType_emu.VarChar, 135, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "CriticalID", "이상발생ID", false, GridColDataType_emu.VarChar, 80, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "CriticalDESC", "이상발생내역", false, GridColDataType_emu.VarChar, 92, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "CriticalDT", "이상발생일자", false, GridColDataType_emu.VarChar, 102, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ActionDesc", "조치내역", false, GridColDataType_emu.VarChar, 167, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ActionDT", "조치일시", false, GridColDataType_emu.VarChar, 98, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ActionUserID", "조치자ID", false, GridColDataType_emu.VarChar, 68, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "WorkerName", "조치자", false, GridColDataType_emu.VarChar, 72, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "Remarks", "비고", false, GridColDataType_emu.VarChar, 40, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "Maker", "생성자", false, GridColDataType_emu.VarChar, 64, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "MakeDate", "생성일시", false, GridColDataType_emu.VarChar, 101, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "Editor", "수정자", false, GridColDataType_emu.VarChar, 64, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "EditDate", "수정일시", false, GridColDataType_emu.VarChar, 102, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            DtChange = (DataTable)grid2.DataSource;
            _GridUtil.SetInitUltraGridBind(this.grid2);
            #endregion

            ///row number
            //grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            //grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            //grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            //grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
        }

        private void grid1_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            DoInquire2();

            if (this.grid2.Rows.Count == 0)
            {
                this.txtUSL.Text = (0.0).ToString();
                this.txtLSL.Text = (0.0).ToString();
                this.txtInspVal.Text = (0.0).ToString();
                return;
            }

            double usl = 0.0;
            double lsl = 0.0;
            double cl = 0.0;
            double ucl = 0.0;
            double lcl = 0.0;
            double spec = 0.0;

            if (this.grid1.ActiveRow.Cells["USL"].Value.ToString() != "")
                usl = Convert.ToDouble(this.grid1.ActiveRow.Cells["USL"].Value);
            if (this.grid1.ActiveRow.Cells["LSL"].Value.ToString() != "")
                lsl = Convert.ToDouble(this.grid1.ActiveRow.Cells["LSL"].Value);
            if (this.grid1.ActiveRow.Cells["CL"].Value.ToString() != "")
                cl = Convert.ToDouble(this.grid1.ActiveRow.Cells["CL"].Value);
            if (this.grid1.ActiveRow.Cells["UCL"].Value.ToString() != "")
                ucl = Convert.ToDouble(this.grid1.ActiveRow.Cells["UCL"].Value);
            if (this.grid1.ActiveRow.Cells["LCL"].Value.ToString() != "")
                lcl = Convert.ToDouble(this.grid1.ActiveRow.Cells["LCL"].Value);
            if (this.grid1.ActiveRow.Cells["SPEC"].Value.ToString() != "")
                spec = Convert.ToDouble(this.grid1.ActiveRow.Cells["SPEC"].Value);


            if (this.grid1.ActiveRow.Cells["SpecType"].Value.ToString() == "U")
            {
                this.txtUSL.Text = usl.ToString("0.######");
                this.txtLSL.Text = (0.0).ToString();
            }
            if (this.grid1.ActiveRow.Cells["SpecType"].Value.ToString() == "L")
            {
                this.txtUSL.Text = (0.0).ToString();
                this.txtLSL.Text = lsl.ToString("0.######");
            }
            if (this.grid1.ActiveRow.Cells["InspVal"].Value.ToString() != "")
                this.txtInspVal.Text = Convert.ToDecimal(this.grid1.ActiveRow.Cells["InspVal"].Value).ToString("0.######");
            else
                this.txtInspVal.Text = (0.0).ToString();
            this.txtCL.Text = cl.ToString("0.######");
            this.txtUCL.Text = cl.ToString("0.######");
            this.txtLCL.Text = cl.ToString("0.######");
            this.txtSPEC.Text = cl.ToString("0.######");

            this.txtCriticalDate.Text = this.grid1.ActiveRow.Cells["CriticalDate"].Value.ToString();
            this.txtLineCode.Text = this.grid1.ActiveRow.Cells["WorkCenterCode"].Value.ToString();
            this.txtLineCodeNM.Text = this.grid1.ActiveRow.Cells["WorkCenterName"].Value.ToString();
            this.txtItemCode.Text = this.grid1.ActiveRow.Cells["ItemCode"].Value.ToString();
            this.txtItemCodeNM.Text = this.grid1.ActiveRow.Cells["ItemName"].Value.ToString();

        }

        private void grid2_AfterRowActivate(object sender, EventArgs e)
        {
            this.txtCriticalID.Text = this.grid2.ActiveRow.Cells["CriticalID"].Value.ToString();
            this.txtCriticalDESC.Text = this.grid2.ActiveRow.Cells["CriticalDESC"].Value.ToString();
            this.txtActionDesc.Text = string.Empty;
        }
    }
}
