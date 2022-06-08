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
using System.IO;
#endregion

namespace SAMMI.BM
{
    public partial class MO1001 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        private DataTable DtChange = null;
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string PlantCode = string.Empty;
        #endregion

        public MO1001()
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
        }

        #region [그리드 셋팅]
        private void GridInit()
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Seq", "순번", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FileID", "파일명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FileSize", "파일크기", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FileImage", "파일이미지", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Descript", "설명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
          
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            #endregion

            #region 콤보박스

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("MONITORIP");  //현황판IP
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MonitorIP", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
            #endregion

        }
        #endregion

        #region 조회
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[2];

            try
            {
                base.DoInquire();

                param[0] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);                  
                param[1] = helper.CreateParameter("@UseFlag", SqlDBHelper.nvlString(this.cboUseFlag_H.Value), SqlDbType.VarChar, ParameterDirection.Input); 

                //DataSet ds = helper.FillDataSet("USP_MO1001_S1", CommandType.StoredProcedure, param);
                DataTable rtnDtTemp = helper.FillTable("USP_MO1001_S1", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;
            }
            catch (Exception ex)
            {
                this.ShowDialog(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
            this.ClosePrgForm();
        }
        #endregion 조회

        #region 등록
        private void btnSUM1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.InitialDirectory = Application.StartupPath;
          
            openfiledialog.Filter = "ALL|*.*";

            if (openfiledialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    base.DoNew();

                    int iRow = _GridUtil.AddRow(this.grid1, DtChange);

                    UltraGridUtil.ActivationAllowEdit(this.grid1, "PlantCode", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "FileID", iRow);
                    UltraGridUtil.ActivationAllowEdit(this.grid1, "UseFlag", iRow);

                    FileInfo fileinfo = new FileInfo(openfiledialog.FileName);
                    grid1.Rows[iRow].Cells["FileID"].Value = fileinfo.Name;
                    grid1.Rows[iRow].Cells["FileSize"].Value = fileinfo.Length;

                    FileStream fs = new FileStream(fileinfo.FullName, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] fileimage = new byte[fs.Length];
                    fs.Read(fileimage, 0, System.Convert.ToInt32(fs.Length));
                    grid1.Rows[iRow].Cells["FileImage"].Value = fileimage;
                    fs.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            };
           
        }
        #endregion

        #region 삭제
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();

            this.grid1.DeleteRow();
        }
        #endregion

        #region 저장
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

                            param = new SqlParameter[3];

                            param[0] = helper.CreateParameter("Seq", drRow["Seq"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // Plan No

                            param[1] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[2] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_MO1001_D1", CommandType.StoredProcedure, param);

                            if (param[1].Value.ToString() == "E") throw new Exception(param[2].Value.ToString());
                            #endregion
                            break;
                            
                        case DataRowState.Added:
                            #region 추가

                            param = new SqlParameter[8];
                            param[0] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("FileID", drRow["FileID"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);
                            param[2] = helper.CreateParameter("FileSize", Convert.ToDecimal(drRow["FileSize"]), SqlDbType.Decimal, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("FileImage", drRow["FileImage"], SqlDbType.Image, ParameterDirection.Input);
                            param[4] = helper.CreateParameter("Descript", drRow["Descript"].ToString(), SqlDbType.VarChar, ParameterDirection.Input); 
                            param[5] = helper.CreateParameter("UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[6] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[7] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_MO1001_I1", CommandType.StoredProcedure, param);

                            if (param[6].Value.ToString() == "E") throw new Exception(param[7].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:

                            #region 수정
                            
                            param = new SqlParameter[6];
                            param[0] = helper.CreateParameter("Seq", Convert.ToInt32(drRow["Seq"]), SqlDbType.Int, ParameterDirection.Input);  
                            param[1] = helper.CreateParameter("PlantCode", SqlDBHelper.nvlString(drRow["PlantCode"]), SqlDbType.VarChar, ParameterDirection.Input);  
                            param[2] = helper.CreateParameter("UseFlag", SqlDBHelper.nvlString(drRow["UseFlag"]), SqlDbType.VarChar, ParameterDirection.Input);
                            param[3] = helper.CreateParameter("Descript", drRow["Descript"].ToString(), SqlDbType.VarChar, ParameterDirection.Input); 
                            param[4] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[5] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);
          
                            helper.ExecuteNoneQuery("USP_MO1001_U1", CommandType.StoredProcedure, param);

                            if (param[4].Value.ToString() == "E") throw new Exception(param[5].Value.ToString());

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
    }
}
