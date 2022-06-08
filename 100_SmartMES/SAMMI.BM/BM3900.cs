#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : 
//   Form Name    : 
//   Name Space   : 
//   Created Date : 
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAMMI.PopUp;
using SAMMI.Common;
using Infragistics.Win.UltraWinGrid;
#endregion

namespace SAMMI.BM
{
    public partial class BM3900 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        //비지니스 로직 객체 생성
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
        #endregion

        #region < CONSTRUCTOR >
        public BM3900()
        {
            InitializeComponent();

            btbManager = new BizTextBoxManagerEX();
            gridManager = new BizGridManagerEX(grid1);

            GridInit();
        }
        #endregion

        #region BM3900_Load
        private void BM3900_Load(object sender, EventArgs e)
        {
        }
        #endregion BM3900_Load

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[6];

            try
            {
                DtChange.Clear();
                string sPLCNo = this.txtPLCNo_H.Text;
                string sPLCChannel = this.txtPLCChannel_H.Text;
                string sPLCAddr = this.txtPLCAddr_H.Text;
                string sTagName = this.txtTagName_H.Text;
                string sGathringYN = SqlDBHelper.nvlString(cboGathering_H.Value);
                string sUseFlag = SqlDBHelper.nvlString(cboUseFlag_H.Value);
               
                base.DoInquire();

                param[0] = helper.CreateParameter("@PLC_DEVICE_NO", sPLCNo, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@PLC_CH_NO", sPLCChannel, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@PLC_ADDR", sPLCAddr, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@TAG_NAME", sTagName, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@GATHERING_YN", sGathringYN, SqlDbType.VarChar, ParameterDirection.Input);
                param[5] = helper.CreateParameter("@UseFlag", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM3900_S3", CommandType.StoredProcedure, param);
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
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PLC_DEVICE_NO", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PLC_CH_NO", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PLC_ADDR", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PLC_ADDRNM", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "TAG_NAME", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "LEVEL", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "DATATYPE", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Magnification", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "GATHERING_YN", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "TAG_GROUP", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "RW_FLAG", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow); 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "REMARK", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MakeDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Maker", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "EditDate", iRow);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "Editor", iRow);
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
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;

            try
            {
                string sPlantCode = "";
                this.Focus();

                #region [Validate 체크]
                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                        case DataRowState.Modified:
                            if (SqlDBHelper.nvlString(dr["PlantCode"]) == ""
                                || SqlDBHelper.nvlString(dr["WorkCenterCode"]) == ""
                                || SqlDBHelper.nvlString(dr["WorkCenterOPCode"]) == ""
                                || SqlDBHelper.nvlString(dr["PLCRowColumn"]) == ""
                                || SqlDBHelper.nvlString(dr["PLCRowMachNo"]) == "")
                            {
                                ShowDialog("사업장, 작업장코드, 작업장Op코드, PLCRowColumn, PLCRowMachNo는 필수 입력사항입니다. ", Windows.Forms.DialogForm.DialogType.OK);

                                CancelProcess = true;
                                return;
                            }
                            break;
                    }
                }
                #endregion

                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                {
                    CancelProcess = true;
                    return;
                }

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow drRow in ((DataTable)grid1.DataSource).Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[5];

                            sPlantCode = SqlDBHelper.gGetCode(drRow["PlantCode"]);

                            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                            param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[2] = helper.CreateParameter("@WorkCenterOPCode", SqlDBHelper.nvlString(drRow["WorkCenterOPCode"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[3] = helper.CreateParameter("@PLCRowColumn", SqlDBHelper.nvlString(drRow["PLCRowColumn"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드
                            param[4] = helper.CreateParameter("@PLCRowMachNo", SqlDBHelper.nvlString(drRow["PLCRowMachNo"]), SqlDbType.VarChar, ParameterDirection.Input);           // 품목코드

                            helper.ExecuteNoneQuery("USP_BM7500_D1", CommandType.StoredProcedure, param);

                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가
                            param = new SqlParameter[9];

                            sPlantCode = SqlDBHelper.gGetCode(drRow["PlantCode"]);

                            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@WorkCenterOPCode", SqlDBHelper.nvlString(drRow["WorkCenterOPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@PLCRowColumn", SqlDBHelper.nvlString(drRow["PLCRowColumn"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@PLCRowMachNo", SqlDBHelper.nvlString(drRow["PLCRowMachNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@InspCode", SqlDBHelper.nvlString(drRow["InspCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@MachCode", SqlDBHelper.nvlString(drRow["MachCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            helper.ExecuteNoneQuery("USP_BM7500_I1", CommandType.StoredProcedure, param);

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정
                            param = new SqlParameter[9];

                            sPlantCode = SqlDBHelper.gGetCode(drRow["PlantCode"]);

                            param[0] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("@WorkCenterCode", SqlDBHelper.nvlString(drRow["WorkCenterCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("@WorkCenterOPCode", SqlDBHelper.nvlString(drRow["WorkCenterOPCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("@PLCRowColumn", SqlDBHelper.nvlString(drRow["PLCRowColumn"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("@PLCRowMachNo", SqlDBHelper.nvlString(drRow["PLCRowMachNo"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[5] = helper.CreateParameter("@InspCode", SqlDBHelper.nvlString(drRow["InspCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[6] = helper.CreateParameter("@MachCode", SqlDBHelper.nvlString(drRow["MachCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[7] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[8] = helper.CreateParameter("@Editor", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                            helper.ExecuteNoneQuery("USP_BM7500_U1", CommandType.StoredProcedure, param);

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

        #region < EVENT AREA >

        /// <summary>
        /// DATABASE UPDATE전 VALIDATEION CHECK 및 값을 수정한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        void Adapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            if (e.Row.RowState == DataRowState.Modified)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                return;
            }

            if (e.Row.RowState == DataRowState.Added)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
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
                    e.Row.RowError = "창고정보가 있습니다.";
                    throw (new SException("S00099", e.Errors));
                default:
                    break;
            }
        }
        #endregion

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        private void GridInit()
        {
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            // InitColumnUltraGrid
            // 0. gird 명, 1 칼럼명, 2.aption  3. colNotNullable, 4.colDataType
            // 5.columnWidth, 6.maxLength, 7. HAlign, 8. visible, 9. editable, 10. formatString, 
            // 11. editMask, 12. maxValue, 13. minValue, 14. regexPattern

            _GridUtil.InitColumnUltraGrid(grid1, "PLC_CH_NO", "M/PLC Channnel", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PLC_DEVICE_NO", "M/PLC No", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PLC_ADDR", "M/PLC Addr Code", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PLC_ADDRNM", "M/PLC Addr명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "TAG_NAME", "Takebishi Tag", true, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LEVEL", "Takebishi Level", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DATATYPE", "데이터유형", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Magnification", "Magnification", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "GATHERING_YN", "수집유무", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "TAG_GROUP", "Takebishi Grp", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RW_FLAG", "읽기/쓰기", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "REMARK", "비고", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", true, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            //     ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            //grid1.Columns["PLC_CH_NO"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PLC_CH_NO"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PLC_CH_NO"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["PLC_DEVICE_NO"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PLC_DEVICE_NO"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PLC_DEVICE_NO"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["PLC_ADDR"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PLC_ADDR"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PLC_ADDR"].MergedCellStyle = MergedCellStyle.Always;
            //grid1.Columns["PLC_ADDRNM"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PLC_ADDRNM"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PLC_ADDRNM"].MergedCellStyle = MergedCellStyle.Always;

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");


            rtnDtTemp = _Common.GET_TBM0000_CODE("DATATYPE");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "RW_FLAG", rtnDtTemp, "CODE_ID", "CODE_NAME");


            rtnDtTemp = _Common.GET_TBM0000_CODE("PLCDATATYPE");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DATATYPE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "GATHERING_YN", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
        }
        #endregion

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //for (int i = 0; i < this.grid1.DisplayLayout.Bands[0].Columns.Count; i++)
            //{
            //    if (grid1.DisplayLayout.Bands[0].Columns[i].ToString() == "PLC_ADDRNM")
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
