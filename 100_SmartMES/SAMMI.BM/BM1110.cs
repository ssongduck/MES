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

using Infragistics.Win.UltraWinGrid;

namespace SAMMI.BM
{
    /// <summary>
    /// BM1110 class
    /// </summary>
    public partial class BM1110 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variables

        /// <summary>
        /// Plant code
        /// </summary>
        private string _PlantCode = string.Empty;

        /// <summary>
        /// Ultra grid util
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        /// <summary>
        /// Biz text box manager
        /// </summary>
        BizTextBoxManagerEX _BizTextBoxManagerEX;

        /// <summary>
        /// Biz grid manager
        /// </summary>
        BizGridManagerEX _BizGridManagerEX;

        DataTable rtnDtTemp = new DataTable();

        private DataTable DtChange = null;

        #endregion

        #region Constructors

        /// <summary>
        /// BM1110 constructor
        /// </summary>
        public BM1110()
        {
            InitializeComponent();

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

            _BizTextBoxManagerEX = new BizTextBoxManagerEX();
            _BizGridManagerEX = new BizGridManagerEX(grid1);

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                _BizTextBoxManagerEX.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                _BizTextBoxManagerEX.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                    , new string[] { "", "" }, new object[] { });
            }
            else
            {
                _BizTextBoxManagerEX.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                _BizTextBoxManagerEX.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                    , new string[] { "", "" }, new object[] { });
            }

            _UltraGridUtil.InitializeGrid(this.grid1, false, true, false, string.Empty, false);

            _UltraGridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this._PlantCode == "") ? true : false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "OPCODE", "공정코드", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "OPNAME", "공정", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장코드", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "STOPCODE", "비가동코드", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "STOPDESC", "비가동명", true, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "START_TIME", "비가동구분", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "END_TIME", "비가동유형", true, GridColDataType_emu.VarChar, 120, 255, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ISUSABLE", "사용유무", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "CREATOR", "등록자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "CREATETIME", "등록일자", false, GridColDataType_emu.DateTime, 150, 10, Infragistics.Win.HAlign.Center, true, true, "yyyy-MM-dd HH:mm:ss", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MODIFIER", "수정자", true, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MODIFYTIME", "수정일자", false, GridColDataType_emu.DateTime, 150, 10, Infragistics.Win.HAlign.Center, true, true, "yyyy-MM-dd HH:mm:ss", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "LASTEVENTTIME", "마지막이벤트일자", false, GridColDataType_emu.DateTime, 150, 10, Infragistics.Win.HAlign.Center, true, true, "yyyy-MM-dd HH:mm:ss", null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid1);

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            DtChange = (DataTable)grid1.DataSource;

            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ISUSABLE", _Common.GET_TBM0000_CODE("USEFLAG"), "CODE_ID", "CODE_NAME");

            _BizGridManagerEX.PopUpAdd("WORKCENTERCODE", "WORKCENTERNAME", "TBM0600", new string[] { "PLANTCODE", "", "", "" });
        }

        #endregion

        #region Events

        /// <summary>
        /// BM1110 load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BM1110_Load(object sender, EventArgs e)
        {
            //rtnDtTemp = _Common.GET_TBM0000_CODE("STOPTYPE");
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "StopType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            //rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            //rtnDtTemp = _Common.GET_TBM0000_CODE("STOPCLASS");
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "StopClass", rtnDtTemp, "CODE_ID", "CODE_NAME");
        }

        /// <summary>
        /// Adapter row updating event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Adapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            if (e.Row.RowState == DataRowState.Modified)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                return;
            }

            if (e.Row.RowState == DataRowState.Added)
            {
                e.Command.Parameters["@Maker"].Value = this.WorkerID;
                return;
            }
        }

        /// <summary>
        /// Adapter row updated event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void Adapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Errors == null) return;

            switch (((SqlException)e.Errors).Number)
            {
                // 중복
                case 2627:
                    e.Row.RowError = "비가동 정보가 있습니다.";
                    throw (new SException("S00099", e.Errors));
                default:
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
            SqlParameter[] sqlParameters = new SqlParameter[6];

            try
            {
                DtChange.Clear();
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sOpCode = txtOPCode.Text.Trim();
                string sOpName = txtOPName.Text.Trim();
                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();
                string sWorkCenterName = txtWorkCenterName.Text.Trim();
                string isUseFlag = SqlDBHelper.nvlString(this.cboisUseFlag_H.Value);

                sqlParameters[0] = sqlDBHelper.CreateParameter("@PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@OPCODE", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[2] = sqlDBHelper.CreateParameter("@OPNAME", sOpName, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[3] = sqlDBHelper.CreateParameter("@WORKCENTERCODE", sOpName, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[4] = sqlDBHelper.CreateParameter("@WORKCENTERNAME", sOpName, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[5] = sqlDBHelper.CreateParameter("@ISUSABLE", isUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = sqlDBHelper.FillTable("USP_BM1100_S1", CommandType.StoredProcedure, sqlParameters);

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
                if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                if (sqlParameters != null) { sqlParameters = null; }
            }
        }

        /// <summary>
        /// Do new
        /// </summary>
        public override void DoNew()
        {
            try
            {
                base.DoNew();

                int iRow = _UltraGridUtil.AddRow(this.grid1, DtChange);

                UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "OPName", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StopCode", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StopDesc", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StopType", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StopClass", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Remark", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "StopSMS", iRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();

            this.grid1.DeleteRow();
        }

        /// <summary>
        /// Do save
        /// </summary>
        public override void DoSave()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = null;

            try
            {
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            // Validate 체크
                            if (SqlDBHelper.nvlString(dr["PlantCOde"]) == "" || SqlDBHelper.nvlString(dr["StopCode"]) == "")
                            {
                                ShowDialog("사업장, 비가동코드는 필수 입력항목 입니다", Windows.Forms.DialogForm.DialogType.OK);

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

                foreach (DataRow drRow in ((DataTable)grid1.DataSource).Rows)
                {
                    lblUseFlag.Focus();
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제
                            drRow.RejectChanges();

                            sqlParameters = new SqlParameter[3];

                            sqlParameters[0] = sqlDBHelper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            sqlParameters[1] = sqlDBHelper.CreateParameter("@OPCode", SqlDBHelper.nvlString(drRow["OPCode"]), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            sqlParameters[2] = sqlDBHelper.CreateParameter("@StopCode", SqlDBHelper.nvlString(drRow["StopCode"]), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드

                            sqlDBHelper.ExecuteNoneQuery("USP_BM1100_D1", CommandType.StoredProcedure, sqlParameters);
                            #endregion

                            break;
                        case DataRowState.Added:
                            #region 추가
                            sqlParameters = new SqlParameter[13];


                            sqlParameters[0] = sqlDBHelper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[1] = sqlDBHelper.CreateParameter("@OPCode", SqlDBHelper.nvlString(drRow["OPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[2] = sqlDBHelper.CreateParameter("@StopCode", SqlDBHelper.nvlString(drRow["StopCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[3] = sqlDBHelper.CreateParameter("@StopDesc", SqlDBHelper.nvlString(drRow["StopDesc"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[4] = sqlDBHelper.CreateParameter("@StopType", SqlDBHelper.nvlString(drRow["StopType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[5] = sqlDBHelper.CreateParameter("@StopClass", SqlDBHelper.nvlString(drRow["StopClass"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[6] = sqlDBHelper.CreateParameter("@StopMH", SqlDBHelper.nvlString(drRow["StopMH"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[7] = sqlDBHelper.CreateParameter("@StopMCH", SqlDBHelper.nvlString(drRow["StopMCH"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[8] = sqlDBHelper.CreateParameter("@StopCL", SqlDBHelper.nvlString(drRow["StopCL"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[9] = sqlDBHelper.CreateParameter("@Remark", SqlDBHelper.nvlString(drRow["Remark"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[10] = sqlDBHelper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[11] = sqlDBHelper.CreateParameter("@Maker", SqlDBHelper.nvlString(drRow["Maker"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[12] = sqlDBHelper.CreateParameter("@StopSMS", SqlDBHelper.nvlString(drRow["StopSMS"]), SqlDbType.VarChar, ParameterDirection.Input);

                            sqlDBHelper.ExecuteNoneQuery("USP_BM1100_I1", CommandType.StoredProcedure, sqlParameters);

                            //if (param[15].Value.ToString() == "E") 
                            //    throw new Exception(param[16].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정
                            sqlParameters = new SqlParameter[13];

                            sqlParameters[0] = sqlDBHelper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[1] = sqlDBHelper.CreateParameter("@OPCode", SqlDBHelper.nvlString(drRow["OPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[2] = sqlDBHelper.CreateParameter("@StopCode", SqlDBHelper.nvlString(drRow["StopCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[3] = sqlDBHelper.CreateParameter("@StopDesc", SqlDBHelper.nvlString(drRow["StopDesc"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[4] = sqlDBHelper.CreateParameter("@StopType", SqlDBHelper.nvlString(drRow["StopType"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[5] = sqlDBHelper.CreateParameter("@StopClass", SqlDBHelper.nvlString(drRow["StopClass"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[6] = sqlDBHelper.CreateParameter("@StopMH", SqlDBHelper.nvlString(drRow["StopMH"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[7] = sqlDBHelper.CreateParameter("@StopMCH", SqlDBHelper.nvlString(drRow["StopMCH"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[8] = sqlDBHelper.CreateParameter("@StopCL", SqlDBHelper.nvlString(drRow["StopCL"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[9] = sqlDBHelper.CreateParameter("@Remark", SqlDBHelper.nvlString(drRow["Remark"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[10] = sqlDBHelper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[11] = sqlDBHelper.CreateParameter("@Editor", SqlDBHelper.nvlString(drRow["Editor"]), SqlDbType.VarChar, ParameterDirection.Input);
                            sqlParameters[12] = sqlDBHelper.CreateParameter("@StopSMS", SqlDBHelper.nvlString(drRow["StopSMS"]), SqlDbType.VarChar, ParameterDirection.Input);

                            sqlDBHelper.ExecuteNoneQuery("USP_BM1100_U1", CommandType.StoredProcedure, sqlParameters);
                            #endregion
                            break;
                    }
                }
                sqlDBHelper.Transaction.Commit();

            }
            catch (Exception ex)
            {
                sqlDBHelper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                if (sqlParameters != null) { sqlParameters = null; }
            }
        }

        #endregion
    }
}