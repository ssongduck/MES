#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM8010
//   Form Name    : 품목별 표준시간 관리
//   Name Space   : SAMMI.BM
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
#endregion

namespace SAMMI.BM
{
    public partial class BM8010 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        #endregion

        #region < CONSTRUCTOR >

        public BM8010()
        {
            InitializeComponent();

            this.txtPlantCode.Text = "[" + LoginInfo.UserPlantCode + "] " + LoginInfo.UserPlantName;
        }
        #endregion

        #region BM8010_Load
        private void BM8010_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);


            // InitColumnUltraGrid 90 162 199 90 135 77 88 83 100 100 100 100 100 100 100 100 100 
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 165, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "작업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "작업장명", false, GridColDataType_emu.VarChar, 135, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StaTime", "표준시간", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CycleTime", "Cycle Time", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SetupTime", "Setup Time", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StartDate", "시작일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EndDate", "종료일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StopMKTime", "정지시간", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "BaseRunTime", "기본사동시간", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Cavity", "Cavity", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            DtChange = (DataTable)grid1.DataSource;

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            #endregion

        }
        #endregion BM8010_Load

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                DtChange.Clear();

                base.DoInquire();

                string sPlantCode = LoginInfo.UserPlantCode;  // 공장코드 
                string sItemCode = txtItemCode.Text.Trim();                                                          // 품목코드                                                          
                string sOPCode = txtOPCode.Text.Trim(); ;                                                          // 공정코드  

                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = SqlDBHelper.FillTable("USP_BM8010_S1", CommandType.StoredProcedure, param);

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
                if (SqlDBHelper._sConn != null) { SqlDBHelper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            try
            {
                base.DoNew();

                int iRow = _GridUtil.AddRow(this.grid1, DtChange);

                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPName", iRow);
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
            //UltraGridUtil.DeleteCurrentRowGrid(this, this.grid1, DtChange);
            base.DoDelete();

            this.grid1.DeleteRow();
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
                this.Focus();

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
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[6];

                            param[0] = helper.CreateParameter("PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[2] = helper.CreateParameter("OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 공장코드
                            param[3] = helper.CreateParameter("StartDate", drRow["StartDate"].ToString(), SqlDbType.DateTime, ParameterDirection.Input);             // 공장코드


                            param[4] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[5] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            SqlDBHelper.ExecuteNoneQuery("USP_BM8010_D1", CommandType.StoredProcedure, param);

                            if (param[4].Value.ToString() == "E") throw new Exception(param[5].Value.ToString());
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가

                            param = new SqlParameter[14];
                            param[0] = helper.CreateParameter("PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);       // 사업장
                            param[1] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 품번
                            param[2] = helper.CreateParameter("OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 공정코드
                            param[3] = helper.CreateParameter("StaTime", drRow["StaTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 표준시간
                            param[4] = helper.CreateParameter("CycleTime", drRow["CycleTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 사이클타임
                            param[5] = helper.CreateParameter("StopMKTime", drRow["StopMKTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);     // 정지시간
                            param[6] = helper.CreateParameter("SetupTime", drRow["SetupTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 셋업시간
                            param[7] = helper.CreateParameter("BaseRunTime", drRow["BaseRunTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);   // 기본사동시간
                            param[8] = helper.CreateParameter("StartDate", drRow["StartDate"].ToString(), SqlDbType.DateTime, ParameterDirection.Input);       // 시작일자
                            param[9] = helper.CreateParameter("EndDate", drRow["EndDate"].ToString(), SqlDbType.DateTime, ParameterDirection.Input);           // 종료일자
                            param[10] = helper.CreateParameter("Cavity", drRow["Cavity"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // Cavity                                                        
                            param[11] = helper.CreateParameter("Maker", drRow["Maker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 등록자    
                            param[12] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[13] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            SqlDBHelper.ExecuteNoneQuery("USP_BM8010_I1", CommandType.StoredProcedure, param);

                            if (param[12].Value.ToString() == "E") throw new Exception(param[13].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:

                            #region 수정
                            param = new SqlParameter[22];

                            param = new SqlParameter[14];
                            param[0] = helper.CreateParameter("PlantCode",LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);      // 사업장
                            param[1] = helper.CreateParameter("ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);        // 품번
                            param[2] = helper.CreateParameter("OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);            // 공정코드
                            param[3] = helper.CreateParameter("StaTime", drRow["StaTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 표준시간
                            param[4] = helper.CreateParameter("CycleTime", drRow["CycleTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);      // 사이클타임
                            param[5] = helper.CreateParameter("StopMKTime", drRow["StopMKTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    // 정지시간
                            param[6] = helper.CreateParameter("SetupTime", drRow["SetupTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);      // 셋업시간
                            param[7] = helper.CreateParameter("BaseRunTime", drRow["BaseRunTime"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);  // 기본사동시간
                            param[8] = helper.CreateParameter("StartDate", drRow["StartDate"].ToString(), SqlDbType.DateTime, ParameterDirection.Input);      // 시작일자
                            param[9] = helper.CreateParameter("EndDate", drRow["EndDate"].ToString(), SqlDbType.DateTime, ParameterDirection.Input);          // 종료일자
                            param[10] = helper.CreateParameter("Cavity", drRow["Cavity"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);            // Cavity                                                        
                            param[11] = helper.CreateParameter("Editor", drRow["Maker"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);            // 등록자    
                            param[12] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[13] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            SqlDBHelper.ExecuteNoneQuery("USP_BM8010_U1", CommandType.StoredProcedure, param);

                            if (param[12].Value.ToString() == "E") throw new Exception(param[13].Value.ToString());

                            #endregion

                            break;
                    }
                }

                helper.Transaction.Commit();

            }
            catch (Exception ex)
            {
                helper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (SqlDBHelper._sConn != null) { SqlDBHelper._sConn.Close(); }
                if (param != null) { param = null; }
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

        }
        /// <summary>
        /// DATABASE UPDATE전 VALIDATEION CHECK 및 값을 수정한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            if (e.Row.RowState == DataRowState.Modified)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                return;
            }

            if (e.Row.RowState == DataRowState.Added)
            {
                //e.Command.Parameters["@Editor"].Value = this.WorkerID;
                e.Command.Parameters["@Maker"].Value = this.WorkerID;
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
                    e.Row.RowError = "데이터가 중복입니다.";
                    throw (new SException("S00099", e.Errors));
                default:
                    break;
            }
        }
        #endregion


        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        #region 텍스트 박스에서 팝업창에서 값 가져오기

        private void Search_Pop_Item()
        {
            string sitem_cd = this.txtItemCode.Text.Trim();    // 품목코드
            string sitem_name = this.txtItemName.Text.Trim();  // 품목명
            string sPlantCode = LoginInfo.UserPlantCode;
           // string splantcd = "820";
            string sitemtype = "";


            try
            {

                _DtTemp = _biz.SEL_TBM0100(sPlantCode, sitem_cd, sitem_name, sitemtype);

                if (_DtTemp.Rows.Count > 1)
                {
                    // 품목 POP-UP 창 처리
                    PopUPManager pu = new PopUPManager();
                    _DtTemp = pu.OpenPopUp("Item", new string[] { sPlantCode, sitemtype, sitem_cd, sitem_name }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (_DtTemp != null && _DtTemp.Rows.Count > 0)
                    {
                        txtItemCode.Text = Convert.ToString(_DtTemp.Rows[0]["ItemCode"]);
                        txtItemName.Text = Convert.ToString(_DtTemp.Rows[0]["Itemname"]);
                    }
                }
                else
                {
                    if (_DtTemp.Rows.Count == 1)
                    {
                        txtItemCode.Text = Convert.ToString(_DtTemp.Rows[0]["ItemCode"]);
                        txtItemName.Text = Convert.ToString(_DtTemp.Rows[0]["Itemname"]);
                    }
                    else
                    {
                        MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                        txtItemCode.Text = string.Empty;
                        txtItemName.Text = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }
        #endregion
        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtItemName.Text = string.Empty;
        }

        private void txtItemCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_Item();
            }
        }

        private void txtItemCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_Item();
        }

        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtItemCode.Text = string.Empty;
        }

        private void txtItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_Item();
            }
        }

        private void txtItemName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_Item();
        }

        #endregion
        #region 공정 작업장 
        //////////////////     
        private void Search_Pop_TBM0400()
        {
            SqlDBHelper.nvlString(cboUseFlag_H.SelectedValue);
            string sPlantCode = LoginInfo.UserPlantCode;          //사업장코드
            string sOPCode = txtOPCode.Text.Trim();       //공정코드
            string sOPName = txtOPName.Text.Trim();      //공정명 
            string sUseFlag =  SqlDBHelper.nvlString(cboUseFlag_H.SelectedValue);;             //사용여부         

            try
            {
                _biz.TBM0400_POP(sPlantCode, sOPCode, sOPName, sUseFlag, txtOPCode, txtOPName);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }
        #endregion        //공정(작업장)
        private void txtOPCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPName.Text = string.Empty;
        }

         private void txtOPNAME_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPCode.Text = string.Empty;
        }

        private void txtOPCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0400();
            }
        }

        private void txtOPCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0400();
        }

   

        private void txtOPName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0400();
            }
        }

        private void txtOPName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0400();
        }
    
        #region grid POP UP 처리
        private void grid_POP_UP()
        {
            int iRow = this.grid1.ActiveRow.Index;
            string sPlantCode = LoginInfo.UserPlantCode;// 사업부
            string sItemCode = this.grid1.Rows[iRow].Cells["ItemCode"].Text.Trim();  // 품목코드
            string sItemname = this.grid1.Rows[iRow].Cells["Itemname"].Text.Trim();  // 품목명
       
            string sOPCode = this.grid1.Rows[iRow].Cells["OPCode"].Text.Trim();  // 작업장(공정)코드
            string sOPName = this.grid1.Rows[iRow].Cells["OPName"].Text.Trim();  // 공정명
        
            if (this.grid1.ActiveCell.Column.ToString() == "ItemCode" || this.grid1.ActiveCell.Column.ToString() == "Itemname")
            {

                _biz.TBM0100_POP_Grid(sItemCode, sItemname, sPlantCode, "", grid1, "ItemCode", "Itemname");
            }
            if (this.grid1.ActiveCell.Column.ToString() == "OPCode" || this.grid1.ActiveCell.Column.ToString() == "OPName")
            {

                _biz.TBM0400_POP_Grid(sPlantCode, sOPCode, sOPName, "", grid1, "OPCode", "OPName");
            }
 
        }
        private void grid1_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            grid_POP_UP();
        }

        private void grid1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                grid_POP_UP();
            }

        }


        #endregion  //grid POP-UP 처리

  

    


    }
}
