#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  QM4400
//   Form Name    : 성분 분석 실적 등록
//   Name Space   : SAMMI.QM
//   Created Date : 
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 
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
    public partial class QM4400 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region Variable
        
        /// <summary>
        /// Return common dataset
        /// </summary>
        DataSet rtnDsTemp = new DataSet(); 
        
        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable rtnDtTemp = new DataTable(); 

        /// <summary>
        /// Grid Util
        /// </summary>
        UltraGridUtil _GridUtil = new UltraGridUtil();

        /// <summary>
        /// common
        /// </summary>
        Common.Common _Common = new Common.Common();

        /// <summary>
        /// Biz Logic
        /// </summary>
        BizGridManagerEX gridManager;

        /// <summary>
        /// Create Temp DataTable
        /// </summary>
        DataTable _DtTemp = new DataTable();
        
        /// <summary>
        /// Plantcode
        /// </summary>
        private string PlantCode = string.Empty;

        #endregion

        #region Constructor

        public QM4400()
        {
           InitializeComponent();
           InitializeControl();
           InitializeGridControl();

        }

        #endregion

        #region Event

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {

            SqlDBHelper helper = new SqlDBHelper(false, false);
            SqlParameter[] param = new SqlParameter[8];

            try
            {
                _DtTemp.Clear();

                base.DoInquire();


                string sStartDate = string.Format("{0:yyyy-MM-dd}", this.CboStartDate_H.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", this.CboEndDate_H.Value);
                string sPlantCode = LoginInfo.UserPlantCode;
                string sHeatNo = txtHeatNo.Text.ToString().Trim();
                string ItemCode = SqlDBHelper.nvlString(cboCompoItem.Value);
                param[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@HeatNo ", sHeatNo, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@ItemCode", ItemCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@REMARK", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                param[6] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                param[7] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);
                
                rtnDtTemp = helper.FillTable("USP_QM3100_S3NEW_UNION", CommandType.StoredProcedure, param);

                if (param[6].Value.ToString() == "E") throw new Exception(param[7].Value.ToString());
                grid1.DataSource = rtnDtTemp;
                rtnDtTemp.AcceptChanges();
                grid1.DataBind();

                _DtTemp = rtnDtTemp;
                //_Common.Grid_Column_Width(this.grid1); //grid 정리용


                //그리드 헤더 조회

                if (!ItemCode.Equals(string.Empty))
                { SetGrid(ItemCode); }
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
            string ItemCode = SqlDBHelper.nvlString(cboCompoItem.Value);
            if (ItemCode.Equals(string.Empty))
            {
                ShowDialog("품목을 선택하세요.", Windows.Forms.DialogForm.DialogType.OK);

                CancelProcess = true;
                return;
            }

            base.DoNew();

            int iRow = _GridUtil.AddRow(this.grid1, _DtTemp);
            this.grid1.Rows[iRow].Cells["PlantCode"].Value = LoginInfo.UserPlantCode;
            this.grid1.Rows[iRow].Cells["ItemCode"].Value = ItemCode;
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "HeatNo", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "LadelNo", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "ItemCode", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Element_Cu", iRow);
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Element_Si", iRow);// NO
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Element_Mg", iRow);    // 측정구
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Element_Zn", iRow);       // 규격하한치 
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Element_Fe", iRow);       // 규격상한치
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Element_Mn", iRow);    // 검사주기(일/주/월)
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Element_Ni", iRow);    // 검사수집장비  
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Element_Sn", iRow);     // 검사횟수
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);      // 표시순서
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);      // 표시순서
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);      // 표시순서
            //UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);      // 표시순서

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
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;

            try
            {
                string sPlantCode = "";
                this.Focus();

                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            // Validate 체크
                            //SqlDBHelper.gGetCode(drRow["PlantCode"]);
                            //if (SqlDBHelper.nvlString(dr["PlantCode"]) == "")
                            if (SqlDBHelper.nvlString(dr["HeatNo"]) == "" || SqlDBHelper.nvlString(dr["LadelNo"]) == "" || SqlDBHelper.nvlString(dr["ItemCode"]) == "")
                            {
                                ShowDialog("Heat No, 래들번호는 필수 입력항목입니다.", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }

                            break;
                    }
                }

                //if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                //    return;

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow drRow in _DtTemp.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[5];
                            param[0] = helper.CreateParameter("@HeatNo", SqlDBHelper.nvlString(drRow["HeatNo"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[1] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@LadelNo", SqlDBHelper.nvlString(drRow["LadelNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[4] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_QM3100_D1", CommandType.StoredProcedure, param);

                            if (param[3].Value.ToString() == "E") throw new Exception(param[4].Value.ToString());
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가
                            param = new SqlParameter[16];

                            param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@HeatNo", SqlDBHelper.nvlString(drRow["HeatNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@LadelNo", SqlDBHelper.nvlString(drRow["LadelNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@ItemCode", SqlDBHelper.nvlString(drRow["ItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@Element_Cu", SqlDBHelper.nvlString(drRow["Element_Cu"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@Element_Si", SqlDBHelper.nvlString(drRow["Element_SI"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@Element_Mg", SqlDBHelper.nvlString(drRow["Element_Mg"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@Element_Zn", SqlDBHelper.nvlString(drRow["Element_Zn"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@Element_Fe", SqlDBHelper.nvlString(drRow["Element_Fe"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@Element_Mn", SqlDBHelper.nvlString(drRow["Element_Mn"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@Element_Ni", SqlDBHelper.nvlString(drRow["Element_Ni"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@Element_Sn", SqlDBHelper.nvlString(drRow["Element_Sn"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                           param[13] = helper.CreateParameter("@Judge", SqlDBHelper.nvlString(drRow["Judge"]), SqlDbType.VarChar, ParameterDirection.Input);

                            param[14] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[15] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_QM3100_I1", CommandType.StoredProcedure, param);

                            if (param[13].Value.ToString() == "E") throw new Exception(param[14].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:

                            #region 수정
                            param = new SqlParameter[16];
                            
                            param[0] = helper.CreateParameter("@PlantCode", LoginInfo.UserPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@HeatNo", SqlDBHelper.nvlString(drRow["HeatNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@LadelNo", SqlDBHelper.nvlString(drRow["LadelNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@ItemCode", SqlDBHelper.nvlString(drRow["ItemCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@Element_Cu", SqlDBHelper.nvlString(drRow["Element_Cu"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@Element_Si", SqlDBHelper.nvlString(drRow["Element_SI"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@Element_Mg", SqlDBHelper.nvlString(drRow["Element_Mg"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@Element_Zn", SqlDBHelper.nvlString(drRow["Element_Zn"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@Element_Fe", SqlDBHelper.nvlString(drRow["Element_Fe"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[9] = helper.CreateParameter("@Element_Mn", SqlDBHelper.nvlString(drRow["Element_Mn"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[10] = helper.CreateParameter("@Element_Ni", SqlDBHelper.nvlString(drRow["Element_Ni"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[11] = helper.CreateParameter("@Element_Sn", SqlDBHelper.nvlString(drRow["Element_Sn"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[12] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);
                            param[13] = helper.CreateParameter("@Judge", SqlDBHelper.nvlString(drRow["Judge"]), SqlDbType.VarChar, ParameterDirection.Input);

                            param[14] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[15] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_QM3100_U1", CommandType.StoredProcedure, param);

                            if (param[13].Value.ToString() == "E") throw new Exception(param[14].Value.ToString());

                            #endregion

                            break;
                    }
                }

                helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                CancelProcess = true;
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

        #region Method

        /// <summary>
        /// InitializeControl
        /// </summary>
        private void InitializeControl()
        {
           // 사업장 사용권한 설정
           _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

           this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

           if (this.PlantCode.Equals("SK"))
              this.PlantCode = "SK1";
           else if (this.PlantCode.Equals("EC"))
              this.PlantCode = "SK2";

           if (!(this.PlantCode.Equals("SK1") || this.PlantCode.Equals("SK2")))
              this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;


           gridManager = new BizGridManagerEX(grid1);

           gridManager.PopUpAdd("ItemCode", "ItemName", "TBM0100", new string[] { "PlantCode", "", "" });

        }
        /// <summary>
        /// InitializeGridControl
        /// </summary>
        private void InitializeGridControl()
        {
           _GridUtil.InitializeGrid(this.grid1);

           _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "HeatNo", "LotNo", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           //_GridUtil.InitColumnUltraGrid(grid1, "LadelNo", "LadelNo", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Element_Cu", "Cu", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Element_Si", "Si", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Element_Mg", "Mg", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Element_Zn", "Zn", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Element_Fe", "Fe", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Element_Mn", "Mn", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Element_Ni", "Ni", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Element_Sn", "Sn", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Judge", "판정", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
           _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

           _GridUtil.SetInitUltraGridBind(grid1);
           _DtTemp = (DataTable)grid1.DataSource;

           grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
           //SetGrid();
           DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("JUDGE");  //사업장
           SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "Judge", rtnDtTemp, "CODE_ID", "CODE_NAME");
           rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
           SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
        }
        /// <summary>
        /// Grid Header Set
        /// </summary>
        /// <param name="sItemCode"></param>
        private void SetGrid(string sItemCode)
        {
           SqlDBHelper helper = new SqlDBHelper(false);
           SqlParameter[] param = new SqlParameter[1];
           string[] Data = new string[8];
           try
           {

              DataTable _GridTable = new DataTable();
              param[0] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);
              _GridTable = helper.FillTable("USP_QM3100_S3GirdNEW", CommandType.StoredProcedure, param);

              if (_GridTable.Rows.Count > 0)
              {

                 for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
                 {
                    // MessageBox.Show(grid1.DisplayLayout.Bands[0].Columns[i].ToString() + " " + _GridTable.Rows[i]["InspName"].ToString());
                    for (int j = 0; j < _GridTable.Rows.Count; j++)
                    {
                       // MessageBox.Show(grid1.DisplayLayout.Bands[0].Columns[i].ToString() + " " + _GridTable.Rows[j]["InspName"].ToString());
                       if (grid1.DisplayLayout.Bands[0].Columns[i].Key.ToString().ToUpper().EndsWith(_GridTable.Rows[j]["InspName"].ToString().ToUpper()))
                       {
                          //MessageBox.Show(_GridTable.Rows[j]["Header"].ToString());
                          grid1.DisplayLayout.Bands[0].Columns[i].Header.Caption = _GridTable.Rows[j]["Header"].ToString();
                       }
                    }

                 }

              }

           }
           catch (Exception ex)
           {
              MessageBox.Show(ex.ToString());
           }
        }
        /// <summary>
        /// Key Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_O_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
           { //키를 조사해서 숫자만입력 !을 없애만 문자만 입력 받게된다.
              e.Handled = true;
           }
        }

        private void Element_Si_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
           { //키를 조사해서 숫자만입력 !을 없애만 문자만 입력 받게된다.
              e.Handled = true;
           }
        }

        private void Element_Al_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
           { //키를 조사해서 숫자만입력 !을 없애만 문자만 입력 받게된다.
              e.Handled = true;
           }
        }

        private void Element_Fe_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
           { //키를 조사해서 숫자만입력 !을 없애만 문자만 입력 받게된다.
              e.Handled = true;
           }
        }

        private void Element_Ca_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
           { //키를 조사해서 숫자만입력 !을 없애만 문자만 입력 받게된다.
              e.Handled = true;
           }
        }

        private void Element_Na_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
           { //키를 조사해서 숫자만입력 !을 없애만 문자만 입력 받게된다.
              e.Handled = true;
           }
        }

        private void Element_K_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
           { //키를 조사해서 숫자만입력 !을 없애만 문자만 입력 받게된다.
              e.Handled = true;
           }
        }

        private void Element_Mg_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
           { //키를 조사해서 숫자만입력 !을 없애만 문자만 입력 받게된다.
              e.Handled = true;
           }
        }

        #endregion
    }
}

