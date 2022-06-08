#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM1800
//   Form Name    : 출하품목번호 관리
//   Name Space   : SAMMI.BM
//   Created Date : 2012-03-31
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 
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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.PopUp;
using SAMMI.Common;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{
    public partial class BM1800 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        PopUp_Biz _biz = new PopUp_Biz();
        BizGridManagerEX bizGrid;
        BizTextBoxManagerEX btbManager;
        DataTable DtChange = null;

        Common.Common _Common = new Common.Common();

        UltraGridUtil _GridUtil = new UltraGridUtil();
        // 변수나 Form에서 사용될 Class를 정의
        #endregion

        #region < CONSTRUCTOR >
        public BM1800()
        {
            InitializeComponent();

            GridInit();

            //cboUseFlag_H.Rows[0][0] = "Y";
            //rtnDtTemp.Rows[0][1] = "용해";
            //rtnDtTemp.Rows[1][0] = "H";
            //rtnDtTemp.Rows[1][1] = "합금";

        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[1];

            try
            {
                base.DoInquire();

                param[0] = helper.CreateParameter("@Div", SqlDBHelper.nvlString(this.cboUseFlag_H.Value), SqlDbType.VarChar, ParameterDirection.Input);
                //param[1] = helper.CreateParameter("@CustCode", this.txtCustCode.Text.ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                //param[2] = helper.CreateParameter("@ItemCode", this.txtItemName.Text.ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                //param[3] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(this.cboUseFlag_H.Value), SqlDbType.VarChar, ParameterDirection.Input);
                
                grid1.DataSource = helper.FillTable("USP_BM1800_S1", CommandType.StoredProcedure, param);
                grid1.DataBind();

                DtChange = (DataTable)grid1.DataSource;
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
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {

            base.DoNew();
            this.grid1.InsertRow();
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
           // base.DoDelete();
           // this.grid1.DeleteRow();
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
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            // Validate 체크
                            //if (SqlDBHelper.nvlString(dr["PlantCode"]) == ""
                            //    || SqlDBHelper.nvlString(dr["CustCode"]) == ""
                            //    || SqlDBHelper.nvlString(dr["ItemCode"]) == "")
                            //{
                            //    ShowDialog("C:I00004", Windows.Forms.DialogForm.DialogType.OK);

                            //    CancelProcess = true;
                            //    return;
                            //}

                            break;
                    }
                }

                //if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                //    return;

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow drRow in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (drRow.RowState)
                    {

                        case DataRowState.Deleted:
                            #region 삭제
                            //drRow.RejectChanges();

                            //param = new SqlParameter[3];

                            //param[0] = helper.CreateParameter("@CustCode", drRow["CustCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            //param[1] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            //param[2] = helper.CreateParameter("@ItemCode", drRow["ItemCode"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 관리항목

                            //helper.ExecuteNoneQuery("USP_BM1800_D1", CommandType.StoredProcedure, param);
                            
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region [추가]
                            param = new SqlParameter[5];
                            param[0] = helper.CreateParameter("@Div", SqlDBHelper.nvlString(drRow["Div"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@ItemCode", SqlDBHelper.nvlString(drRow["ItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@ItemName", SqlDBHelper.nvlString(drRow["ItemName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@UseYN", SqlDBHelper.nvlString(drRow["UseYN"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@Sno", SqlDBHelper.nvlString(drRow["Sno"]), SqlDbType.VarChar, ParameterDirection.Input);
                            
                            helper.ExecuteNoneQuery("USP_BM1800_I1", CommandType.StoredProcedure, param);
                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 추가/수정
                             param = new SqlParameter[5];
                            param[0] = helper.CreateParameter("@Div", SqlDBHelper.nvlString(drRow["Div"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@ItemCode", SqlDBHelper.nvlString(drRow["ItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@ItemName", SqlDBHelper.nvlString(drRow["ItemName"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@UseYN", SqlDBHelper.nvlString(drRow["UseYN"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@Sno", SqlDBHelper.nvlString(drRow["Sno"]), SqlDbType.VarChar, ParameterDirection.Input);
                            
                             helper.ExecuteNoneQuery("USP_BM1800_U1", CommandType.StoredProcedure, param);

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
        //void Adapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        //{
        //    if (e.Row.RowState == DataRowState.Modified)
        //    {
        //        e.Command.Parameters["@Editor"].Value = this.WorkerID;
        //        return;
        //    }

        //    if (e.Row.RowState == DataRowState.Added)
        //    {
        //        e.Command.Parameters["@Editor"].Value = this.WorkerID;
        //        e.Command.Parameters["@Maker"].Value = this.WorkerID;
        //        return;
        //    }
        //}

        /// <summary>
        /// 저장처리시 오류가 발생한 경우 오류 메세지에 대한 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void Adapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        //{
        //    if (e.Errors == null) return;

        //    switch (((SqlException)e.Errors).Number)
        //    {
        //        // 중복
        //        case 2627:
        //            e.Row.RowError = "검사항목이 있습니다.";
        //            throw (new SException("S00099", e.Errors));
        //        default:
        //            break;
        //    }
        //}
        #endregion

        private void grid1_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            e.Row.Cells["UseYN"].Value = "Y";
        }

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의

        #region [Grid Setting]
        private void GridInit()
        {
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid1, "Div", "구분", true, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Default, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Default, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품목명", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Default, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseYN", "사용여부", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Default, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Sno", "출력순번", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Default, true, true, "##0", null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);

            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseYN", rtnDtTemp, "CODE_ID", "CODE_NAME");
            //rtnDtTemp.Rows[0][0] = "Y";
            //rtnDtTemp.Rows[0][1] = "용해";
            //rtnDtTemp.Rows[1][0] = "H";
            //rtnDtTemp.Rows[1][1] = "합금";
            rtnDtTemp = _Common.GET_TBM0000_CODE("MeltDivision");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Div", rtnDtTemp, "CODE_ID", "CODE_NAME");

        }
        #endregion

        private void BM1800_Load(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
