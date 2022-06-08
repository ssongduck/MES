using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using SAMMI.Common;
using SAMMI.Windows.Forms;
using System.Diagnostics;

namespace SAMMI.PP
{
    /// <summary>
    /// PP0341 class
    /// </summary>
    public partial class PP0341 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _RtnDt = new DataTable();

        /// <summary>
        /// Change grid1 datatable
        /// </summary>
        private DataTable _ChangeDt = null;

        /// <summary>
        /// Header columns
        /// </summary>
        string[] _HeadColumnArray = null;

        /// <summary>
        /// 
        /// </summary>
        string[] _EmptyArray = { "v_txt" };

        /// <summary>
        /// Plant code
        /// </summary>
        private string _sPlantCode = string.Empty;

        /// <summary>
        /// Current row
        /// </summary>
        int _iCurrentRow = -1;

        /// <summary>
        /// Grid util
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        //비지니스 로직 객체 생성
        BizGridManagerEX gridManager; 
        PopUp_Biz _biz = new PopUp_Biz();

        #endregion

        #region Constructor

        /// <summary>
        /// PP0341 constructor
        /// </summary>
        public PP0341()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// Alloy Interface button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAlloyIF_Click(object sender, EventArgs e)
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false, false);
            SqlParameter[] sqlParameters = null;
            int iRtn = 0;

            try
            {
                sqlParameters = new SqlParameter[0];

                iRtn = sqlDBHelper.ExecuteNoneQuery("USP_ERP_IF_PROD_HG", CommandType.StoredProcedure, sqlParameters);
                //sqlDBHelper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                //sqlDBHelper.Transaction.Rollback();
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// PP0341 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP0341_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();
        }

        #endregion

        #region Method

        /// <summary>
        /// Initialize control
        /// </summary>
        private void InitializeControl()
        {
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this._sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this._sPlantCode.Equals("SK"))
            {
                this._sPlantCode = "SK1";
            }
            else if (this._sPlantCode.Equals("EV"))
            {
                this._sPlantCode = "SK2";
            }

            GetAlloyAuth();

            calRegDT.Value = DateTime.Now;

            gridManager = new BizGridManagerEX(grid1);
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            _UltraGridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(grid1, "WK_DTE", "작업일자", false, GridColDataType_emu.VarChar, 110, 110, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INGOT_WGT", "괴(12S) 생산량", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "PROD_WGT", "액상 생산량", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ETC_INGOT_WGT", "괴(ETC) 생산량", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);

            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00016", "칩", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00017", "(서산)게이트", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00017_1", "(평택)게이트", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00017_2", "(실라폰트)게이트", false, GridColDataType_emu.Integer, 130, 130, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00010", "기계철", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00001", "TENSE", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00008", "INGOT", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00015", "주물", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00014", "노베", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00009", "주물괴", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00019", "휠", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00013", "판재", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00007", "실리콘(국내)", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00030", "실리콘(수입)", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00018", "동라지에타", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00025", "다릿발", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00020", "재괴", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00011", "A샤시", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00003", "6063EXTRUSION", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00006", "TALK", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00012", "B샤시", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00002", "TAINT TABOR", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "H13701_00028", "TWITCH", false, GridColDataType_emu.Integer, 110, 110, Infragistics.Win.HAlign.Right, true, true, "#,###", null, null, null, null);
            
            _UltraGridUtil.InitColumnUltraGrid(grid1, "CRE_USER_ID", "등록자", false, GridColDataType_emu.VarChar, 110, 110, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "CRE_DTE", "등록시간", false, GridColDataType_emu.DateTime24, 150, 150, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "UPT_USER_ID", "수정자", false, GridColDataType_emu.VarChar, 110, 110, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "UPT_DTE", "수정시간", false, GridColDataType_emu.DateTime24, 150, 150, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP_IF_DT", "ERP I/F", false, GridColDataType_emu.VarChar, 110, 110, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _UltraGridUtil.SetInitUltraGridBind(grid1);

            grid1.DisplayLayout.UseFixedHeaders = true;

            _ChangeDt = (DataTable)grid1.DataSource;
        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.btnAlloyIF.Click += new EventHandler(btnAlloyIF_Click);
            this.Disposed += new EventHandler(PP0341_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.btnAlloyIF.Click -= new EventHandler(btnAlloyIF_Click);
            this.Disposed -= new EventHandler(PP0341_Disposed);
        }

        /// <summary>
        /// Get alloy auth
        /// </summary>
        private void GetAlloyAuth()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[1];

            try
            {
                if (LoginInfo.UserPlantCode == "SK2")
                {
                    sqlParameters[0] = sqlDBHelper.CreateParameter("@USERID", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                    _RtnDt = sqlDBHelper.FillTable("USP_PP0340_S3", CommandType.StoredProcedure, sqlParameters);

                    if (_RtnDt != null && _RtnDt.Rows.Count > 0)
                    {
                        int iCount = int.Parse(_RtnDt.Rows[0][0].ToString());

                        btnAlloyIF.Visible = (iCount > 0) ? true : false;
                    }
                }
                else
                {
                    btnAlloyIF.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[1];
            ClearAllControl();

            try
            {
                _ChangeDt.Clear();

                base.DoInquire();

                sqlParameters[0] = sqlDBHelper.CreateParameter("@STD_DT", string.Format("{0:yyyy-MM-dd}", calRegDT.Value), SqlDbType.VarChar, ParameterDirection.Input);

                _RtnDt = sqlDBHelper.FillTable("USP_PP0341_S1", CommandType.StoredProcedure, sqlParameters);

                grid1.DataSource = _RtnDt;
                grid1.DataBind();

                _ChangeDt = _RtnDt;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Do new
        /// </summary>
        public override void DoNew()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[1];

            sqlParameters[0] = sqlDBHelper.CreateParameter("@STD_DT", string.Format("{0:yyyy-MM-dd}", calRegDT.Value), SqlDbType.VarChar, ParameterDirection.Input);

            _RtnDt = sqlDBHelper.FillTable("USP_PP0341_S2", CommandType.StoredProcedure, sqlParameters);

            if (_RtnDt != null && _RtnDt.Rows.Count > 0)
            {
                if (int.Parse(_RtnDt.Rows[0]["CNT"].ToString()) > 0)
                {
                    MessageBox.Show(string.Format("[{0}]{1}", DateTime.Now.ToString("yyyy-MM-dd"), " 일자로 등록된 내역이 존재합니다."));
                    return;
                }
            }

            if (_ChangeDt != null && _ChangeDt.Rows.Count > 0)
            {
                foreach (DataRow dr in _ChangeDt.Rows)
                {
                    if (dr["WK_DTE"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        MessageBox.Show(string.Format("[{0}]{1}", DateTime.Now.ToString("yyyy-MM-dd"), " 일자로 등록된 내역이 존재합니다."));
                        return;
                    }
                }
            }

            base.DoNew();

            _iCurrentRow = _UltraGridUtil.AddRow(this.grid1, _ChangeDt);
            grid1.Rows[_iCurrentRow].Cells["WK_DTE"].Value = string.Format("{0:yyyy-MM-dd}", calRegDT.Value);

            grid1.Rows[_iCurrentRow].Cells["INGOT_WGT"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["PROD_WGT"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["ETC_INGOT_WGT"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00009"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00008"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00017"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00017_1"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00017_2"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00020"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00011"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00019"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00013"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00030"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00010"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00018"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00003"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00016"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00006"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00025"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00012"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00001"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00002"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00014"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00015"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00028"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["H13701_00007"].Value = 0;
            grid1.Rows[_iCurrentRow].Cells["CRE_USER_ID"].Value = LoginInfo.UserID;
            grid1.Rows[_iCurrentRow].Cells["CRE_DTE"].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
            bool bOK = true;

            for (int i = 0; i < grid1.Selected.Rows.Count; i++)
            {
                if (grid1.Selected.Rows[i].Cells["UD"].Value.ToString() == "F")
                {
                    grid1.Selected.Rows[i].Selected = false;
                    bOK = false;
                }
            }

            if (bOK == false)
            {
                MessageBox.Show("삭제 할수 없는 행이 포함되었습니다.");
                return;
            }

            base.DoDelete();
            this.grid1.DeleteRow();
        }

        /// <summary>
        /// Do save
        /// </summary>
        public override void DoSave()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(false, false);
            SqlParameter[] sqlParameters = null;

            try
            {
                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow dr in _ChangeDt.Rows)
                {
                    switch (dr.RowState)
                    {
                        //case DataRowState.Deleted:
                            // TODO : 삭제 기능 사용 안함
                            //dr.RejectChanges();
                            //sqlParameters = new SqlParameter[2];

                            //sqlParameters[0] = sqlDBHelper.CreateParameter("@WK_DTE", dr["WK_DTE"].ToString().Replace("-", string.Empty), SqlDbType.NVarChar, ParameterDirection.Input);
                            //sqlParameters[1] = sqlDBHelper.CreateParameter("@RTN_MESSAGE", string.Empty, SqlDbType.NVarChar, ParameterDirection.InputOutput);
                            //sqlDBHelper.ExecuteNoneQuery("USP_PP0341_D1", CommandType.StoredProcedure, sqlParameters);

                            //break;

                        case DataRowState.Added:

                            try
                            {
                                if (string.IsNullOrEmpty(dr["WK_DTE"].ToString()))
                                {
                                    MessageBox.Show("작업일자는 필수정보입니다.");
                                    continue;
                                }
                                sqlParameters = new SqlParameter[29];
                                sqlParameters[0] = sqlDBHelper.CreateParameter("@WK_DTE", dr["WK_DTE"].ToString().Replace("-", string.Empty), SqlDbType.NVarChar, ParameterDirection.Input);
                                sqlParameters[1] = sqlDBHelper.CreateParameter("@INGOT_WGT", dr["INGOT_WGT"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[2] = sqlDBHelper.CreateParameter("@PROD_WGT", dr["PROD_WGT"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[3] = sqlDBHelper.CreateParameter("@ETC_INGOT_WGT", dr["ETC_INGOT_WGT"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[4] = sqlDBHelper.CreateParameter("@H13701_00009", dr["H13701_00009"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[5] = sqlDBHelper.CreateParameter("@H13701_00008", dr["H13701_00008"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[6] = sqlDBHelper.CreateParameter("@H13701_00017", dr["H13701_00017"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[7] = sqlDBHelper.CreateParameter("@H13701_00017_1", dr["H13701_00017_1"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[8] = sqlDBHelper.CreateParameter("@H13701_00017_2", dr["H13701_00017_2"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[9] = sqlDBHelper.CreateParameter("@H13701_00020", dr["H13701_00020"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[10] = sqlDBHelper.CreateParameter("@H13701_00011", dr["H13701_00011"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[11] = sqlDBHelper.CreateParameter("@H13701_00019", dr["H13701_00019"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[12] = sqlDBHelper.CreateParameter("@H13701_00013", dr["H13701_00013"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[13] = sqlDBHelper.CreateParameter("@H13701_00030", dr["H13701_00030"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[14] = sqlDBHelper.CreateParameter("@H13701_00010", dr["H13701_00010"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[15] = sqlDBHelper.CreateParameter("@H13701_00018", dr["H13701_00018"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[16] = sqlDBHelper.CreateParameter("@H13701_00003", dr["H13701_00003"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[17] = sqlDBHelper.CreateParameter("@H13701_00016", dr["H13701_00016"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[18] = sqlDBHelper.CreateParameter("@H13701_00006", dr["H13701_00006"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[19] = sqlDBHelper.CreateParameter("@H13701_00025", dr["H13701_00025"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[20] = sqlDBHelper.CreateParameter("@H13701_00012", dr["H13701_00012"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[21] = sqlDBHelper.CreateParameter("@H13701_00001", dr["H13701_00001"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[22] = sqlDBHelper.CreateParameter("@H13701_00002", dr["H13701_00002"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[23] = sqlDBHelper.CreateParameter("@H13701_00014", dr["H13701_00014"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[24] = sqlDBHelper.CreateParameter("@H13701_00015", dr["H13701_00015"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[25] = sqlDBHelper.CreateParameter("@H13701_00028", dr["H13701_00028"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[26] = sqlDBHelper.CreateParameter("@H13701_00007", dr["H13701_00007"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                                sqlParameters[27] = sqlDBHelper.CreateParameter("@CRE_USER_ID", dr["CRE_USER_ID"].ToString(), SqlDbType.NVarChar, ParameterDirection.Input);
                                sqlParameters[28] = sqlDBHelper.CreateParameter("@RTN_MESSAGE", string.Empty, SqlDbType.NVarChar, ParameterDirection.InputOutput);                                
                                sqlDBHelper.ExecuteNoneQuery("USP_PP0341_I1", CommandType.StoredProcedure, sqlParameters);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                            break;

                        //case DataRowState.Modified:

                            //try
                            //{
                            //    if (string.IsNullOrEmpty(dr["WK_DTE"].ToString()))
                            //    {
                            //        MessageBox.Show("작업일자는 필수정보입니다.");
                            //        continue;
                            //    }

                            //    sqlParameters = new SqlParameter[25];
                            //    sqlParameters[0] = sqlDBHelper.CreateParameter("@WK_DTE", dr["WK_DTE"].ToString().Replace("-", string.Empty), SqlDbType.NVarChar, ParameterDirection.Input);
                            //    sqlParameters[1] = sqlDBHelper.CreateParameter("@INGOT_WGT", dr["INGOT_WGT"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[2] = sqlDBHelper.CreateParameter("@PROD_WGT", dr["PROD_WGT"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[3] = sqlDBHelper.CreateParameter("@H13701_00009", dr["H13701_00009"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[4] = sqlDBHelper.CreateParameter("@H13701_00008", dr["H13701_00008"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[5] = sqlDBHelper.CreateParameter("@H13701_00017", dr["H13701_00017"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[6] = sqlDBHelper.CreateParameter("@H13701_00020", dr["H13701_00020"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[7] = sqlDBHelper.CreateParameter("@H13701_00011", dr["H13701_00011"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[8] = sqlDBHelper.CreateParameter("@H13701_00019", dr["H13701_00019"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[9] = sqlDBHelper.CreateParameter("@H13701_00013", dr["H13701_00013"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[10] = sqlDBHelper.CreateParameter("@H13701_00030", dr["H13701_00030"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[11] = sqlDBHelper.CreateParameter("@H13701_00010", dr["H13701_00010"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[12] = sqlDBHelper.CreateParameter("@H13701_00018", dr["H13701_00018"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[13] = sqlDBHelper.CreateParameter("@H13701_00003", dr["H13701_00003"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[14] = sqlDBHelper.CreateParameter("@H13701_00016", dr["H13701_00016"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[15] = sqlDBHelper.CreateParameter("@H13701_00006", dr["H13701_00006"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[16] = sqlDBHelper.CreateParameter("@H13701_00025", dr["H13701_00025"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[17] = sqlDBHelper.CreateParameter("@H13701_00012", dr["H13701_00012"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[18] = sqlDBHelper.CreateParameter("@H13701_00001", dr["H13701_00001"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[19] = sqlDBHelper.CreateParameter("@H13701_00002", dr["H13701_00002"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[20] = sqlDBHelper.CreateParameter("@H13701_00014", dr["H13701_00014"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[21] = sqlDBHelper.CreateParameter("@H13701_00015", dr["H13701_00015"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[22] = sqlDBHelper.CreateParameter("@H13701_00028", dr["H13701_00028"].ToString(), SqlDbType.Int, ParameterDirection.Input);
                            //    sqlParameters[23] = sqlDBHelper.CreateParameter("@UPT_USER_ID", dr["CRE_USER_ID"].ToString(), SqlDbType.NVarChar, ParameterDirection.Input);
                            //    sqlParameters[24] = sqlDBHelper.CreateParameter("@RTN_MESSAGE", string.Empty, SqlDbType.NVarChar, ParameterDirection.InputOutput);

                            //    sqlDBHelper.ExecuteNoneQuery("USP_PP0341_U1", CommandType.StoredProcedure, sqlParameters);
                            //}
                            //catch (Exception ex)
                            //{
                            //    MessageBox.Show(ex.Message);
                            //}

                            //break;
                    }
                }
                //sqlDBHelper.Transaction.Commit();
                ClearAllControl();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (sqlDBHelper._sConn != null)
                {
                    sqlDBHelper._sConn.Close();
                }

                if (sqlParameters != null)
                {
                    sqlParameters = null;
                }
            }
        }

        /// <summary>
        /// Clear control
        /// </summary>
        /// <param name="control"></param>
        private void ClearControl(System.Windows.Forms.Control control)
        {
            if (control == null)
            {
                return;
            }

            foreach (System.Windows.Forms.Control ctrl in control.Controls)
            {
                ClearControl(ctrl);

                if (ctrl.GetType().Name == "TextBox")
                {
                    TextBox textBox = (TextBox)ctrl;

                    foreach (string sVal in _EmptyArray)
                    {
                        if (textBox.Name.StartsWith(sVal))
                        {
                            textBox.Text = string.Empty;
                        }
                    }
                }

                if (ctrl.GetType().Name == "MaskedTextBox")
                {
                    MaskedTextBox maskedTextBox = (MaskedTextBox)ctrl;

                    foreach (string sVal in _EmptyArray)
                    {
                        if (maskedTextBox.Name.StartsWith(sVal))
                        {
                            maskedTextBox.Text = string.Empty;
                        }
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Clear all control
        /// </summary>
        private void ClearAllControl()
        {
            ClearControl(this);
        }

        #endregion
    }
}
