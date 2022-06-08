#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : PP8000
//   Form Name    : 생산현황관리
//   Name Space   : SAMMI.PP
//   Created Date : 2020-11-20
//   Made By      : ysJung
//   Description  : 서산 가공생산팀 요청 
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

namespace SAMMI.PP
{
    public partial class PP8000 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        // 변수나 Form에서 사용될 Class를 정의
        //DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        DataTable DtChange = new DataTable();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();        
        private string PlantCode = string.Empty;
        #endregion

        #region < CONSTRUCTOR >
        public PP8000()
        {
            InitializeComponent();

            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this.PlantCode.Equals("SK")) {
                this.PlantCode = "SK1";
            }
            else if (this.PlantCode.Equals("EC")) {
                this.PlantCode = "SK2";
            }

            if (!(this.PlantCode.Equals("SK1") || this.PlantCode.Equals("SK2")))
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;

            btbManager = new BizTextBoxManagerEX();                        
            GridInit();

            if (LoginInfo.PlantAuth.Equals(string.Empty)) 
            {                
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0600", new object[] { this.cboPlantCode_H, "", "" }, new string[] { "", "" }, new object[] { });                                
            }
            else 
            {                
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "" }, new string[] { "", "" }, new object[] { });                
            }            
        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                DtChange.Clear();
                base.DoInquire();
                
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string itemcode = SqlDBHelper.nvlString(this.txtItemCode.Text);
                string sYmon = CboStartdate_H.Value.ToString().Substring(0, 7).Replace("-","");

                param[0] = helper.CreateParameter("@AS_PlantCode",      sPlantCode,  SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@AS_WorkCenterCode", sPlantCode,  SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@AS_ItemCode",       itemcode,    SqlDbType.VarChar, ParameterDirection.Input);                
                param[3] = helper.CreateParameter("@AS_Ymon",           sYmon,       SqlDbType.VarChar, ParameterDirection.Input);                
                
                rtnDtTemp = helper.FillTable("USP_PP8000_S2", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();
                
                DtChange = rtnDtTemp;                
                //for (int i = 0; i < grid1.Rows.Count; i++)
                //{
                //    for (int j = 0; j < grid1.Columns.Count; j++)
                //    {
                //        if (grid1.Rows[i].Cells[j].Value == "")
                //        {
                //            grid1.Rows[i].Cells["FinalWeight"].Value = grid1.Rows[i].Cells[j-1].Value;
                //            break;
                //        }
                //    }
                //}
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
        {
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
                        case DataRowState.Modified:
                            #region [수정]
                            param = new SqlParameter[37];

                            param[0]   = helper.CreateParameter("AS_WorkCetnerCode",   drRow["WorkCenterCode"].ToString(),  SqlDbType.VarChar,   ParameterDirection.Input);
                            param[1]   = helper.CreateParameter("AS_Ymon",             CboStartdate_H.Value,                SqlDbType.VarChar,   ParameterDirection.Input);
                            param[2]   = helper.CreateParameter("AS_ItemCode",         drRow["ItemCode"].ToString(),        SqlDbType.VarChar,   ParameterDirection.Input);
                            param[3]   = helper.CreateParameter("AS_Type",             drRow["Type"].ToString(),            SqlDbType.VarChar,   ParameterDirection.Input);      
                      
                            param[4]   = helper.CreateParameter("AS_date01",           drRow["date01"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input); 
                            param[5]   = helper.CreateParameter("AS_date02",           drRow["date02"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[6]   = helper.CreateParameter("AS_date03",           drRow["date03"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[7]   = helper.CreateParameter("AS_date04",           drRow["date04"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[8]   = helper.CreateParameter("AS_date05",           drRow["date05"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[9]   = helper.CreateParameter("AS_date06",           drRow["date06"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[10]  = helper.CreateParameter("AS_date07",           drRow["date07"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[11]  = helper.CreateParameter("AS_date08",           drRow["date08"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[12]  = helper.CreateParameter("AS_date09",           drRow["date09"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[13]  = helper.CreateParameter("AS_date10",           drRow["date10"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);

                            param[14]  = helper.CreateParameter("AS_date11",           drRow["date11"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[15]  = helper.CreateParameter("AS_date12",           drRow["date12"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input); 
                            param[16]  = helper.CreateParameter("AS_date13",           drRow["date13"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[17]  = helper.CreateParameter("AS_date14",           drRow["date14"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[18]  = helper.CreateParameter("AS_date15",           drRow["date15"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[19]  = helper.CreateParameter("AS_date16",           drRow["date16"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[20]  = helper.CreateParameter("AS_date17",           drRow["date17"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[21]  = helper.CreateParameter("AS_date18",           drRow["date18"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[22]  = helper.CreateParameter("AS_date19",           drRow["date19"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[23]  = helper.CreateParameter("AS_date20",           drRow["date20"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);

                            param[24]  = helper.CreateParameter("AS_date21",           drRow["date21"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[25]  = helper.CreateParameter("AS_date22",           drRow["date22"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[26]  = helper.CreateParameter("AS_date23",           drRow["date23"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input); 
                            param[27]  = helper.CreateParameter("AS_date24",           drRow["date24"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[28]  = helper.CreateParameter("AS_date25",           drRow["date25"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[29]  = helper.CreateParameter("AS_date26",           drRow["date26"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[30]  = helper.CreateParameter("AS_date27",           drRow["date27"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[31]  = helper.CreateParameter("AS_date28",           drRow["date28"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[32]  = helper.CreateParameter("AS_date29",           drRow["date29"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[33]  = helper.CreateParameter("AS_date30",           drRow["date30"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            param[34]  = helper.CreateParameter("AS_date31",           drRow["date31"].ToString() ,         SqlDbType.VarChar,   ParameterDirection.Input);
                            
                            helper.ExecuteNoneQuery("USP_PP8000_U1", CommandType.StoredProcedure, param);
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
            //e.Row.Cells["UseFlag"].Value = "Y";
        }
        
        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의

        private void GridInit()
        {
            //_GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "월별 생산 현황판",   false);            
            //_GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode",  "작업장", false, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "G1",              "품목",   false, GridColDataType_emu.VarChar, 100, 200, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "G2",              "구분",   false, GridColDataType_emu.VarChar, 100, 200, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "01",              "1일",    false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "02",              "2일",    false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "03",              "3일",    false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "04",              "4일",    false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "05",              "5일",    false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "06",              "6일",    false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "07",              "7일",    false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "08",              "8일",    false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "09",              "9일",    false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "10",              "10일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "11",              "11일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "12",              "12일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "13",              "13일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "14",              "14일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "15",              "15일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "16",              "16일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "17",              "17일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "18",              "18일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "19",              "19일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "20",              "20일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "21",              "21일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "22",              "22일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "23",              "23일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "24",              "24일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "25",              "25일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "26",              "26일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "27",              "27일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "28",              "28일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "29",              "29일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "30",              "30일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "31",              "31일",   false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
                                                            
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

        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["G2"].Value.ToString() == "")
            {
                e.Row.Appearance.BackColor = Color.MistyRose;                            
            }
            if (e.Row.Cells["G1"].Value.ToString() == "")
            {
                e.Row.Appearance.BackColor = Color.Orange;
            }
        }
    }
}
