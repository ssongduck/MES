#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM0400
//   Form Name    : 공정코드 마스터
//   Name Space   : SAMMI.BM
//   Created Date : 2012-03-02
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 기준정보 ( 공정코드 마스터 ) 정보 관리 폼 
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;   
                         

#endregion

namespace SAMMI.BM
{
    public partial class BM0400 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        private string PlantCode = string.Empty;
        #endregion
        
        public BM0400()
        {
            InitializeComponent();

            //this.txtPlantCode.Text = "[" + LoginInfo.UserPlantCode + "] " + LoginInfo.UserPlantName;
           // GridInit();
            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this.PlantCode.Equals("SK"))
                this.PlantCode = "SK1";
            else if (this.PlantCode.Equals("EC"))
                this.PlantCode = "SK2";

            if (!(this.PlantCode.Equals("SK1") || this.PlantCode.Equals("SK2")))
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;

        }
        private void GridInit()
        {
            //_GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, true, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "PlantCodeNM", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자ID", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "MakerNM", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자ID", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);
            //_GridUtil.InitColumnUltraGrid(grid1, "EditorNM", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            ///row number
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
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                DtChange.Clear();
                base.DoInquire();
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);
                string sOpCode = txtOpCode.Text;
                string sOpName = txtOpName.Text;

                param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@OPCode", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);
                //param[3] = helper.CreateParameter("@OPName", sOpName, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_BM0400_S1", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_BM0400_S1_UNION", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;
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
        /// </summary>
        public override void DoNew()
        {
            try
            {
                base.DoNew();
                int iRow = _GridUtil.AddRow(this.grid1, DtChange);

                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
                grid1.Rows[iRow].Cells["PlantCode"].Value = LoginInfo.PlantAuth.Equals("ALL") ? "" : LoginInfo.PlantAuth; 

            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
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
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" || SqlDBHelper.nvlString(dr["OPCode"]) == "")
                            {
                                ShowDialog("사업장코드와 공정코드는 필수입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }

                            if (!LoginInfo.PlantAuth.Equals("ALL") &&
                                !LoginInfo.PlantAuth.Equals(SqlDBHelper.nvlString(dr["PlantCode"])))
                            {
                                ShowDialog("[" + SqlDBHelper.nvlString(dr["PlantCode"]) + "] 등록권한이 없습니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            break;
                        case DataRowState.Modified:
                            // Validate 체크
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == "" || SqlDBHelper.nvlString(dr["OPCode"]) == "")
                            {
                                ShowDialog("사업장코드와 공정코드는 필수입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            
                            if (!LoginInfo.PlantAuth.Equals("ALL") &&
                                !LoginInfo.PlantAuth.Equals(SqlDBHelper.nvlString(dr["PlantCode"])))
                            {
                                ShowDialog("[" + SqlDBHelper.nvlString(dr["PlantCode"]) + "] 등록권한이 없습니다.", Windows.Forms.DialogForm.DialogType.OK);

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

                            #region [삭제 - 통합 완료]
                            drRow.RejectChanges();
                            param = new SqlParameter[2];
                            param[0] = helper.CreateParameter("@PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                          
                            helper.ExecuteNoneQuery("USP_BM0400_D1", CommandType.StoredProcedure, param);
                            #endregion

                            break;
                        case DataRowState.Added:
                            #region [추가 - 통합완료]
                            param = new SqlParameter[5];

                            param[0] = helper.CreateParameter("@PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[1] = helper.CreateParameter("@OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[2] = helper.CreateParameter("@OPName", drRow["OPName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@Maker",  SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[4] = helper.CreateParameter("@UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서

                            helper.ExecuteNoneQuery("USP_BM0400_I1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region [수정 - 통합완료]
                            param = new SqlParameter[5];

                            param[0] = helper.CreateParameter("@PlantCode", drRow["PlantCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);          // 공장코드
                            param[1] = helper.CreateParameter("@OPCode", drRow["OPCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[2] = helper.CreateParameter("@OPName", drRow["OPName"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[3] = helper.CreateParameter("@Editor",  SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            param[4] = helper.CreateParameter("@UseFlag", drRow["UseFlag"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서

                            helper.ExecuteNoneQuery("USP_BM0400_U1", CommandType.StoredProcedure, param);
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
                if (helper._sConn != null) { helper._sConn.Close(); }
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
            //if (e.Row.RowState == DataRowState.Modified)
            //{
            //    e.Command.Parameters["@Editor"].Value = this.WorkerID;
            //    return;
            //}

            //if (e.Row.RowState == DataRowState.Added)
            //{
            //    e.Command.Parameters["@Maker"].Value = this.WorkerID;
            //    return;
            //}
        }

        /// <summary>
        /// 저장처리시 오류가 발생한 경우 오류 메세지에 대한 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            //if (e.Errors == null) return;

            //switch (((SqlException)e.Errors).Number)
            //{
            //    // 중복
            //    case 2627:
            //        e.Row.RowError = "공정코드가 있습니다.";
            //        throw (new SException("S00099", e.Errors));
            //    default:
            //        break;
            //}
        }

        private void txtOpCode_KeyDown(object sender, KeyEventArgs e)
        {
          //  this.txtOpName.Text = string.Empty;
        }

        private void txtOpCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_OP();
            }
        }

        private void txtOpCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_OP();
        }

        private void txtOpName_KeyDown(object sender, KeyEventArgs e)
        {
         //   this.txtOpCode.Text = string.Empty;
        }

        private void txtOpName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_OP();
            }
        }

        private void txtOpName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_OP();
        }

        #endregion



        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의


        #region 텍스트 박스에서 팝업창에서 값 가져오기
        private void Search_Pop_OP()
        {
            return;

            string sOpCode = this.txtOpCode.Text.Trim();  // 작업장코드
            string sOpName = this.txtOpName.Text.Trim();  // 작업장명
            string sPlantCode = string.Empty;


            try
            {
                sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                _DtTemp = _biz.SEL_TBM0400(sPlantCode, sOpCode, sOpName, "Y");

                if (_DtTemp.Rows.Count > 1)
                {
                    // 품목 POP-UP 창 처리
                    PopUPManager pu = new PopUPManager();
                    _DtTemp = pu.OpenPopUp("OP", new string[] { sPlantCode, sOpCode, sOpName, "Y" }); // 작업장 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (_DtTemp != null && _DtTemp.Rows.Count > 0)
                    {
                        txtOpCode.Text = Convert.ToString(_DtTemp.Rows[0]["OpCode"]);
                        txtOpName.Text = Convert.ToString(_DtTemp.Rows[0]["OpName"]);
                    }
                }
                else
                {
                    if (_DtTemp.Rows.Count == 1)
                    {
                        txtOpCode.Text = Convert.ToString(_DtTemp.Rows[0]["OpCode"]);
                        txtOpName.Text = Convert.ToString(_DtTemp.Rows[0]["OpName"]);
                    }
                    else
                    {
                        this.IsShowDialog = false;
                        this.ShowDialog("C:R00114");

                        txtOpCode.Text = string.Empty;
                        txtOpName.Text = string.Empty;
                    }

                }
            }
            catch (Exception )
            {
                this.IsShowDialog = false;
                this.ShowDialog("C:S00001");
            }

        }
        #endregion

        private void BM0400_Load(object sender, EventArgs e)
        {
            GridInit();

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            //SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
        }


        #endregion
    }
}
