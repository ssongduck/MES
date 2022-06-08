#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM9400
//   Form Name    : 금형별 단중 이력관리
//   Name Space   : SAMMI.BM
//   Created Date : 2020-04-03
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
using SAMMI.PopUp;
using SAMMI.Common;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{
    public partial class BM9400 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        // 변수나 Form에서 사용될 Class를 정의
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        DataTable DtChange = new DataTable();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private int _Fix_Col = 0;
        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >
        public BM9400()
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
            gridManager = new BizGridManagerEX(grid1);

            GridInit();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                //품목 팝업
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { this.cboPlantCode_H, "", "" }, new string[] { "", "" }, new object[] { });
                //금형 팝업 추가 필요.
                btbManager.PopUpAdd(txtMoldCode, txtMoldName, "TBM1600", new object[] { this.cboPlantCode_H, "", "" }, new string[] { "", "" }, new object[] { });
            }
            else
            {
                //품목 팝업
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { LoginInfo.PlantAuth, "", "" }, new string[] { "", "" }, new object[] { });
                //금형 팝업 추가 필요.
                btbManager.PopUpAdd(txtMoldCode, txtMoldName, "TBM1600", new object[] { LoginInfo.PlantAuth, "", "" }, new string[] { "", "" }, new object[] { });
            }
            gridManager.PopUpAdd("ItemCode", "ItemName", "TBM0100", new string[] { "PlantCode", "", "" }); // 그리드 품목컬럼 팝업
            gridManager.PopUpAdd("MoldCode", "MoldName", "TBM1600", new string[] { "PlantCode", "", "" }); // 그리드 금형컬럼 팝업
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");            
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
                        
        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                DtChange.Clear();
                base.DoInquire();

                string sYear  = CboStartdate_H.Value.ToString().Substring(0, 4);
                string sMonth = CboStartdate_H.Value.ToString().Substring(5, 2);
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                string itemcode = SqlDBHelper.nvlString(this.txtItemCode.Text);
                string Moldcode = SqlDBHelper.nvlString(this.txtMoldCode.Text);
                string useflag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

                param[0] = helper.CreateParameter("@AS_PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@AS_ItemCode",  itemcode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@AS_MoldCode",  Moldcode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@AS_Gyear",     sYear, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@AS_Gmonth",    sMonth, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@AS_UseFlag",   useflag, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_BM1600_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM9400_S1", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();
                
                DtChange = rtnDtTemp;

                for (int i = 0; i < grid1.Rows.Count; i++)
                {
                    for (int j = 0; j < grid1.Columns.Count; j++)
                    {
                        if (grid1.Rows[i].Cells[j].Value == "")
                        {
                            grid1.Rows[i].Cells["FinalWeight"].Value = grid1.Rows[i].Cells[j-1].Value;
                            break;
                        }
                    }
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
        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        public override void DoNew()
        /// </summary>
        {
            try
            {
                base.DoNew();

                int iRow = _GridUtil.AddRow(this.grid1, DtChange);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MoldCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "InsertQty", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "BaseWeight",  iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ScrapWeight", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Gyear", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Gmonth", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date01", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date02", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date03", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date04", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date05", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date06", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date07", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date08", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date09", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date10", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date11", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date12", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date13", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date14", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date15", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date16", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date17", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date18", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date19", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date20", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date21", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date22", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date23", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date24", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date25", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date26", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date27", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date28", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date29", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date30", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "date31", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "FinalWeight", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Remark", iRow);

                // 기본정보 자동입력 
                //grid1.Rows[iRow].Cells["MoldCode"].Value = "SK-1";
                grid1.Rows[iRow].Cells["Gyear"].Value = System.DateTime.Now.ToString("yyyy");
                grid1.Rows[iRow].Cells["Gmonth"].Value = System.DateTime.Now.ToString("MM");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();
            this.grid1.DeleteRow();
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false, false);
            SqlParameter[] param = null;

            try
            {
                this.Focus();

                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            // Validate 체크
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" || SqlDBHelper.nvlString(dr["MoldCode"]) == "")
                            {
                                ShowDialog("금형코드는 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            break;
                    }
                }

                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow drRow in DtChange.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:

                            #region [삭제]
                            drRow.RejectChanges();
                            param = new SqlParameter[5];
                            param[0] = helper.CreateParameter("AS_PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("AS_ItemCode",  drRow["ItemCode"].ToString(),  SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[2] = helper.CreateParameter("AS_MoldCode",  drRow["MoldCode"].ToString(),  SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[3] = helper.CreateParameter("AS_Gyear",     drRow["Gyear"].ToString(),     SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[4] = helper.CreateParameter("AS_Gmonth",    drRow["Gmonth"].ToString(),    SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            helper.ExecuteNoneQuery("USP_BM9400_D1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region [추가]
                            param = new SqlParameter[44];
                            param[0]  = helper.CreateParameter("AS_PlantCode",   drRow["PlantCode"].ToString(),    SqlDbType.VarChar,   ParameterDirection.Input);
                            param[1]  = helper.CreateParameter("AS_ItemCode",    drRow["ItemCode"].ToString(),     SqlDbType.VarChar,   ParameterDirection.Input);
                            param[2]  = helper.CreateParameter("AS_ItemName",    drRow["ItemName"].ToString(),     SqlDbType.VarChar,   ParameterDirection.Input);
                            param[3]  = helper.CreateParameter("AS_MoldCode",    drRow["MoldCode"].ToString(),     SqlDbType.VarChar,   ParameterDirection.Input);
                            param[4]  = helper.CreateParameter("AS_MoldName",    drRow["MoldName"].ToString(),     SqlDbType.VarChar,   ParameterDirection.Input);
                            param[5]  = helper.CreateParameter("AS_Gyear",       drRow["Gyear"].ToString(),        SqlDbType.VarChar,   ParameterDirection.Input);
                            param[6]  = helper.CreateParameter("AS_Gmonth",      drRow["Gmonth"].ToString(),       SqlDbType.VarChar,   ParameterDirection.Input);
                            param[7]  = helper.CreateParameter("AS_InsertQty",   drRow["InsertQty"].ToString()   , SqlDbType.VarChar,   ParameterDirection.Input);
                            param[8]  = helper.CreateParameter("AS_BaseWeight",  drRow["BaseWeight"].ToString()  , SqlDbType.VarChar,   ParameterDirection.Input);                            
                            param[9]  = helper.CreateParameter("AS_date01",      drRow["date01"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[10] = helper.CreateParameter("AS_date02",      drRow["date02"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[11] = helper.CreateParameter("AS_date03",      drRow["date03"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[12] = helper.CreateParameter("AS_date04",      drRow["date04"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[13] = helper.CreateParameter("AS_date05",      drRow["date05"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[14] = helper.CreateParameter("AS_date06",      drRow["date06"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[15] = helper.CreateParameter("AS_date07",      drRow["date07"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[16] = helper.CreateParameter("AS_date08",      drRow["date08"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[17] = helper.CreateParameter("AS_date09",      drRow["date09"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[18] = helper.CreateParameter("AS_date10",      drRow["date10"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[19] = helper.CreateParameter("AS_date11",      drRow["date11"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[20] = helper.CreateParameter("AS_date12",      drRow["date12"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[21] = helper.CreateParameter("AS_date13",      drRow["date13"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[22] = helper.CreateParameter("AS_date14",      drRow["date14"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[23] = helper.CreateParameter("AS_date15",      drRow["date15"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[24] = helper.CreateParameter("AS_date16",      drRow["date16"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[25] = helper.CreateParameter("AS_date17",      drRow["date17"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[26] = helper.CreateParameter("AS_date18",      drRow["date18"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[27] = helper.CreateParameter("AS_date19",      drRow["date19"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[28] = helper.CreateParameter("AS_date20",      drRow["date20"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[29] = helper.CreateParameter("AS_date21",      drRow["date21"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[30] = helper.CreateParameter("AS_date22",      drRow["date22"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[31] = helper.CreateParameter("AS_date23",      drRow["date23"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[32] = helper.CreateParameter("AS_date24",      drRow["date24"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[33] = helper.CreateParameter("AS_date25",      drRow["date25"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[34] = helper.CreateParameter("AS_date26",      drRow["date26"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[35] = helper.CreateParameter("AS_date27",      drRow["date27"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[36] = helper.CreateParameter("AS_date28",      drRow["date28"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[37] = helper.CreateParameter("AS_date29",      drRow["date29"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[38] = helper.CreateParameter("AS_date30",      drRow["date30"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[39] = helper.CreateParameter("AS_date31",      drRow["date31"].ToString() ,      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[40] = helper.CreateParameter("AS_FinalWeight", drRow["FinalWeight"].ToString(),  SqlDbType.VarChar,   ParameterDirection.Input);
                            param[41] = helper.CreateParameter("AS_Maker",       LoginInfo.UserID,                 SqlDbType.VarChar,   ParameterDirection.Input);
                            param[42] = helper.CreateParameter("AS_UseFlag",     drRow["UseFlag"].ToString(),      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[43] = helper.CreateParameter("AS_Remark",      drRow["Remark"].ToString(),       SqlDbType.VarChar,   ParameterDirection.Input);
                                                        
                            helper.ExecuteNoneQuery("USP_BM9400_I1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region [수정]
                            param = new SqlParameter[44];

                            param[0]  = helper.CreateParameter("AS_PlantCode",   drRow["PlantCode"].ToString(),         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[1]  = helper.CreateParameter("AS_ItemCode",    drRow["ItemCode"].ToString(),          SqlDbType.VarChar,   ParameterDirection.Input);
                            param[2]  = helper.CreateParameter("AS_ItemName",    drRow["ItemName"].ToString(),          SqlDbType.VarChar,   ParameterDirection.Input);
                            param[3]  = helper.CreateParameter("AS_MoldCode",    drRow["MoldCode"].ToString(),          SqlDbType.VarChar,   ParameterDirection.Input);
                            param[4]  = helper.CreateParameter("AS_MoldName",    drRow["MoldName"].ToString(),          SqlDbType.VarChar,   ParameterDirection.Input);
                            param[5]  = helper.CreateParameter("AS_Gyear",       drRow["Gyear"].ToString(),             SqlDbType.VarChar,   ParameterDirection.Input);
                            param[6]  = helper.CreateParameter("AS_Gmonth",      drRow["Gmonth"].ToString(),            SqlDbType.VarChar,   ParameterDirection.Input);
                            param[7]  = helper.CreateParameter("AS_InsertQty",   drRow["InsertQty"].ToString(),         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[8]  = helper.CreateParameter("AS_BaseWeight",  drRow["BaseWeight"].ToString(),        SqlDbType.VarChar,   ParameterDirection.Input);                            
                            param[9]  = helper.CreateParameter("AS_date01",      drRow["date01"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[10] = helper.CreateParameter("AS_date02",      drRow["date02"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[11] = helper.CreateParameter("AS_date03",      drRow["date03"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[12] = helper.CreateParameter("AS_date04",      drRow["date04"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[13] = helper.CreateParameter("AS_date05",      drRow["date05"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[14] = helper.CreateParameter("AS_date06",      drRow["date06"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[15] = helper.CreateParameter("AS_date07",      drRow["date07"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[16] = helper.CreateParameter("AS_date08",      drRow["date08"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[17] = helper.CreateParameter("AS_date09",      drRow["date09"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[18] = helper.CreateParameter("AS_date10",      drRow["date10"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[19] = helper.CreateParameter("AS_date11",      drRow["date11"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[20] = helper.CreateParameter("AS_date12",      drRow["date12"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[21] = helper.CreateParameter("AS_date13",      drRow["date13"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[22] = helper.CreateParameter("AS_date14",      drRow["date14"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[23] = helper.CreateParameter("AS_date15",      drRow["date15"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[24] = helper.CreateParameter("AS_date16",      drRow["date16"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[25] = helper.CreateParameter("AS_date17",      drRow["date17"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[26] = helper.CreateParameter("AS_date18",      drRow["date18"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[27] = helper.CreateParameter("AS_date19",      drRow["date19"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[28] = helper.CreateParameter("AS_date20",      drRow["date20"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[29] = helper.CreateParameter("AS_date21",      drRow["date21"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[30] = helper.CreateParameter("AS_date22",      drRow["date22"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[31] = helper.CreateParameter("AS_date23",      drRow["date23"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[32] = helper.CreateParameter("AS_date24",      drRow["date24"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[33] = helper.CreateParameter("AS_date25",      drRow["date25"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[34] = helper.CreateParameter("AS_date26",      drRow["date26"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[35] = helper.CreateParameter("AS_date27",      drRow["date27"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[36] = helper.CreateParameter("AS_date28",      drRow["date28"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[37] = helper.CreateParameter("AS_date29",      drRow["date29"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[38] = helper.CreateParameter("AS_date30",      drRow["date30"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[39] = helper.CreateParameter("AS_date31",      drRow["date31"].ToString() ,           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[40] = helper.CreateParameter("AS_FinalWeight", drRow["FinalWeight"].ToString(),       SqlDbType.VarChar,   ParameterDirection.Input);
                            param[41] = helper.CreateParameter("AS_Editor",      LoginInfo.UserID,                      SqlDbType.VarChar,   ParameterDirection.Input);
                            param[42] = helper.CreateParameter("AS_UseFlag",     drRow["UseFlag"].ToString(),           SqlDbType.VarChar,   ParameterDirection.Input);
                            param[43] = helper.CreateParameter("AS_Remark",      drRow["Remark"].ToString(),            SqlDbType.VarChar,   ParameterDirection.Input);             

                            helper.ExecuteNoneQuery("USP_BM9400_U1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                    }
                }
                //helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                //helper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion

        private void btnTemplateOk_Click(object sender, EventArgs e)
        {
            // This code was automatically generated by the RowEditTemplate Wizard
            // 
            // Close the template and save any pending changes.
            //   this.ultraGridRowEditTemplate1.Close(true);

        }

        private void btnTemplateCancel_Click(object sender, EventArgs e)
        {
            // This code was automatically generated by the RowEditTemplate Wizard
            // 
            // Close the template and discard any pending changes.
            // this.ultraGridRowEditTemplate1.Close(false);

        }

        private void grid1_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            e.Row.Cells["UseFlag"].Value = "Y";
        }
        
        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의

        private void GridInit()
        {
            //_GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode",   "사업장", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode",    "품목코드", false, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName",    "품목명", false, GridColDataType_emu.VarChar, 100, 200, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldCode",    "금형코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MoldName",    "금형명", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InsertQty",   "주입중량", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BaseWeight",  "기초단중", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ScrapWeight", "스크랩", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Gyear",       "년도", false, GridColDataType_emu.VarChar, 40, 4, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Gmonth",      "월",  false, GridColDataType_emu.VarChar, 40, 2, Infragistics.Win.HAlign.Left, false, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date01",      "1일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date02",      "2일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date03",      "3일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date04",      "4일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date05",      "5일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date06",      "6일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date07",      "7일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date08",      "8일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date09",      "9일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date10",     "10일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date11",     "11일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date12",     "12일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date13",     "13일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date14",     "14일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date15",     "15일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date16",     "16일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date17",     "17일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date18",     "18일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date19",     "19일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date20",     "20일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date21",     "21일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date22",     "22일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date23",     "23일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date24",     "24일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date25",     "25일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date26",     "26일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date27",     "27일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date28",     "28일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date29",     "29일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date30",     "30일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "date31",     "31일", false, GridColDataType_emu.VarChar, 40, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);

            _GridUtil.InitColumnUltraGrid(grid1, "FinalWeight", "최적중량", false, GridColDataType_emu.VarChar, 50, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
                                                            
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
        }

        #endregion

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "MoldName")
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
