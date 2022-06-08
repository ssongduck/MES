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
    public partial class PP0370 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _RtnComDt = new DataTable();

        /// <summary>
        /// Change grid1 datatable 
        /// </summary>

        private DataTable _ChangeDt = new DataTable();

        /// <summary>
        /// Grid object
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        /// <summary>
        /// Check
        /// </summary>
        string _Chk = string.Empty;

        /// <summary>
        /// Current row
        /// </summary>
        int _Currow = -1;

        /// <summary>
        /// Header column arras
        /// </summary>
        string[] _HeadColumnArrs = null;

        /// <summary>
        /// Empty arrays
        /// </summary>
        string[] _EmptyArrs = { "v_txt" };

        /// <summary>
        /// PlantCode
        /// </summary>
        private string _PlantCode = string.Empty;

        #endregion

        #region Constructor

        public PP0370()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// Grid1 initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            for (int i = 1; i <= e.Row.Cells.Count; i++)
            {

                try
                {
                    object oInterfaceDate = e.Row.Cells["DATE_CREATED"].Value;
                    object oMesData = e.Row.Cells[string.Format("MES{0}", i.ToString("00"))].Value;
                    object oErpData = e.Row.Cells[string.Format("ERP{0}", i.ToString("00"))].Value;

                    if (!DiffERPNMESData(oInterfaceDate, oMesData, oErpData))
                    {
                        e.Row.Cells[string.Format("MES{0}", i.ToString("00"))].Appearance.BackColor = Color.Orange;
                        e.Row.Cells[string.Format("ERP{0}", i.ToString("00"))].Appearance.BackColor = Color.Orange;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    continue;
                }
            }

            if (e.Row.Cells["SDATE"].Value.ToString() == "합 계")
                e.Row.Appearance.BackColor = Color.LightCyan;

        }

        /// <summary>
        /// Grid1 before row activate event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_BeforeRowActivate(object sender, RowEventArgs e)
        {
            //           if (grid1.Rows[e.Row.Index].Cells["UD"].Value.ToString() == "F")
            //         {
            //            grid1.Rows[e.Row.Index].Activation = Activation.ActivateOnly;
            //       }
        }

        /// <summary>
        /// Grid1 before cell activate event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_BeforeCellActivate(object sender, CancelableCellEventArgs e)
        {
            //if (!string.IsNullOrEmpty(e.Cell.Row.Cells["UD"].Value.ToString()) && (e.Cell.Column.Key == "iDate" || e.Cell.Column.Key == "LotNo"))
            //{
            //    e.Cell.Activation = Activation.ActivateOnly;
            //}
        }

        /// <summary>
        /// Grid1 after cell update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key == "WorkCenterCode" || e.Cell.Column.Key == "iDate")
            {
                string sDate = e.Cell.Row.Cells["iDate"].Text.ToString();
                string sWorkCenterCode = e.Cell.Row.Cells["WorkCenterCode"].Value.ToString();
                string sOrderNo = e.Cell.Row.Cells["OrderNo"].Text.ToString();

                if (sOrderNo == string.Empty || ("20" + sOrderNo).StartsWith(sDate.Replace("-", "") + sWorkCenterCode) == false)
                {
                    SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
                    DataTable dt = sqlDBHelper.FillTable("select top 1 PlanNo,dbo.FN_ItemName(ItemCode,'SK1') as  ItemName  from tap0100 where workcentercode='" +
                        sWorkCenterCode + "' and recdate='" + sDate + "'", CommandType.Text);

                    if (dt != null && dt.Rows.Count == 1)
                    {
                        e.Cell.Row.Cells["OrderNo"].Value = dt.Rows[0][0].ToString();
                        e.Cell.Row.Cells["ItemName"].Value = dt.Rows[0][1].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// PP0370 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP0370_Disposed(object sender, EventArgs e)
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

            this._PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this._PlantCode.Equals("SK"))
            {
                this._PlantCode = "SK1";
            }
            else if (this._PlantCode.Equals("EC"))
            {
                this._PlantCode = "SK2";
            }

            calRegDT_FRH.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            // 1. Initialize grid1
            _UltraGridUtil.InitializeGrid(this.grid1);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "SDATE", "작업일자_시작", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "EDATE", "작업일자_마지막", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "DATE_CREATED", "Interface_date", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES01", "Tense_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP01", "Tense_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES02", "TAINTTABOR_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP02", "TAINTTABOR_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES03", "6063EXTRUSION_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP03", "6063EXTRUSION_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES04", "HGPUCK_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP04", "HGPUCK_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES05", "TELIC_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP05", "TELIC_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES06", "TALK_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP06", "TALK_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES07", "SI441_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP07", "SI441_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES08", "INGOT_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP08", "INGOT_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES09", "주물괴_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP09", "주물괴_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES10", "기계철_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP10", "기계철_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES11", "A샤시_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP11", "A샤시_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES12", "B샤시_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP12", "B샤시_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES13", "판재_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP13", "판재_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES14", "노베_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP14", "노베_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES15", "주물_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP15", "주물_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES16", "칩_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP16", "칩_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES17", "게이트_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP17", "게이트_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES18", "동라지에타_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP18", "동라지에타_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES19", "휠_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP19", "휠_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES20", "재괴_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP20", "재괴_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES25", "다릿발_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP25", "다릿발_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES30", "실리콘(수입)_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP30", "실리콘(수입)_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES31", "괴_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP31", "괴_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MES32", "액상_MES", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ERP32", "액상_ERP", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.grid1.InitializeRow += new InitializeRowEventHandler(grid1_InitializeRow);
            this.grid1.BeforeRowActivate += new RowEventHandler(grid1_BeforeRowActivate);
            this.grid1.BeforeCellActivate += new CancelableCellEventHandler(grid1_BeforeCellActivate);
            this.grid1.AfterCellUpdate += new CellEventHandler(grid1_AfterCellUpdate);

            this.Disposed += new EventHandler(PP0370_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.grid1.InitializeRow -= new InitializeRowEventHandler(grid1_InitializeRow);
            this.grid1.BeforeRowActivate -= new RowEventHandler(grid1_BeforeRowActivate);
            this.grid1.BeforeCellActivate -= new CancelableCellEventHandler(grid1_BeforeCellActivate);
            this.grid1.AfterCellUpdate -= new CellEventHandler(grid1_AfterCellUpdate);

            this.Disposed -= new EventHandler(PP0370_Disposed);
        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            //SqlDBHelper helper = new SqlDBHelper(false,"Data Source=192.168.100.20;Initial Catalog=MTMES;User ID=sa;Password=qwer1234!~");
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = new SqlParameter[2];
            ClearAllControl();

            try
            {
                _ChangeDt.Clear();

                base.DoInquire();

                string sDtp_date = string.Format("{0:yyyy-MM-dd}", calRegDT_FRH.Value);
                string sDtp_date_to = string.Format("{0:yyyy-MM-dd}", calRegDT_TOH.Value);

                param[0] = helper.CreateParameter("@StartDate", sDtp_date, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@EndDate", sDtp_date_to, SqlDbType.VarChar, ParameterDirection.Input);


                _RtnComDt = helper.FillTable("USP_PP0370_S1", CommandType.StoredProcedure, param);



                grid1.DataSource = _RtnComDt;
                grid1.DataBind();

                _ChangeDt = _RtnComDt;

            }

            catch (SqlException ex)
            {

            }
            finally
            {
                _Chk = "";
            }

        }

        /// <summary>
        /// Do new
        /// </summary>
        public override void DoNew()
        {
        }

        /// <summary>
        /// Do save
        /// </summary>
        public override void DoSave()
        {
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
        }

        /// <summary>
        /// Clear all control
        /// </summary>
        private void ClearAllControl()
        {
            InitializeControl(this);
        }

        /// <summary>
        /// Initialize control
        /// </summary>
        /// <param name="control"></param>
        private void InitializeControl(System.Windows.Forms.Control control)
        {
            if (control == null)
            {
                return;
            }

            foreach (System.Windows.Forms.Control ctrl in control.Controls)
            {
                InitializeControl(ctrl);

                if (ctrl.GetType().Name == "TextBox")
                {
                    TextBox textBox = (TextBox)ctrl;

                    foreach (string s in _EmptyArrs)
                    {
                        if (textBox.Name.StartsWith(s))
                        {
                            textBox.Text = string.Empty;
                        }
                    }
                }

                if (ctrl.GetType().Name == "MaskedTextBox")
                {
                    MaskedTextBox maskedTextBox = (MaskedTextBox)ctrl;

                    foreach (string sVal in _EmptyArrs)
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
        /// Diff ERP & MES
        /// </summary>
        /// <param name="oInterfaceDate"></param>
        /// <param name="oErpData"></param>
        /// <param name="oMesData"></param>
        /// <returns></returns>
        private bool DiffERPNMESData(object oInterfaceDate, object oErpData, object oMesData)
        {
            int iErpData = 0;
            int iMesData = 0;

            if (oInterfaceDate != null && !string.IsNullOrEmpty(oInterfaceDate.ToString()))
            {
                if (oErpData == null && oMesData == null)
                {
                    return true;
                }

                if (oErpData != null)
                {
                    iErpData = int.Parse(oErpData.ToString());
                }
                else
                {
                    return false;
                }

                if (oMesData != null)
                {
                    iMesData = int.Parse(oMesData.ToString());
                }
                else
                {
                    return false;
                }

                if (iErpData != iMesData)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
