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
using Microsoft.Office.Core;
using System.Runtime.InteropServices;

namespace SAMMI.QM
{
    /// <summary>
    /// QM3000
    /// </summary>
    public partial class QM3000 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        private DataTable _RtnComDt = new DataTable();

        /// <summary>
        /// Return grid common datatable
        /// </summary>
        private DataTable _RtnGridDt = new DataTable();

        /// <summary>
        /// Change grid1 datatable
        /// </summary>
        private DataTable _ChangeDt = null;

        /// <summary>
        /// Grid object
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Plant code
        /// </summary>
        private string _PlantCode = string.Empty;

        /// <summary>
        /// Work center code
        /// </summary>
        private string _WorkCenterCode = string.Empty;

        /// <summary>
        /// Item code
        /// </summary>
        private string _ItemCode = string.Empty;

        /// <summary>
        /// Item code
        /// </summary>
        private string _DayNight = string.Empty;

        /// <summary>
        /// Lot no
        /// </summary>
        private string _LotNo = string.Empty;

        /// <summary>
        /// Start datetime
        /// </summary>
        private DateTime _StartDateTime = System.DateTime.Now;

        /// <summary>
        /// End datetime
        /// </summary>
        private DateTime _EndDateTime = System.DateTime.Now;

        /// <summary>
        /// Biz text box manager EX
        /// </summary>
        BizTextBoxManagerEX btbManager;

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        #endregion

        #region Constructor

        /// <summary>
        /// QM3000 constructor
        /// </summary>
        public QM3000()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();
            AttachEventHandlers();
        }

        #endregion

        #region Event

        /// <summary>
        /// Grid1 initialize row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            //if (e.Row.Cells["INSJUDGE2"].Value.ToString() == "NG")
            //{
            //    e.Row.Appearance.ForeColor = Color.Red;
            //}
        }

        /// <summary>
        /// grid1 Cell Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                if (e.Cell.Value.ToString() == "NG" || e.Cell.Value.ToString() == "OK")
                {
                    picImage.Load(@_RtnGridDt.Rows[e.Cell.Row.Index][e.Cell.Column.ToString().Replace("JUDGE", "PATH")].ToString());
                }

                else
                {
                    picImage.Image = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                picImage.Image = Properties.Resources.no_camera;
            }
        }

        /// <summary>
        /// cboitemcode Value Change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboItemCode_ValueChanged(object sender, EventArgs e)
        {
            if (cboItemCode.Value.ToString() == "07000")
            {
                txtItemName.Text = "1.0 MPI CYN/BLCOK";
            }

            else if (cboItemCode.Value.ToString() == "07500")
            {
                txtItemName.Text = "1.0 T-GDI CYN/BLCOK";
            }

            else if (cboItemCode.Value.ToString() == "08000")
            {
                txtItemName.Text = "1.2 MPI CYN/BLCOK";
            }

            else
            {
                txtItemName.Text = "";
            }
        }

        /// <summary>
        /// Excel Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid1.ActiveRow.Cells["ITEMCODE"].Value.ToString().Substring(6) == "07000")
                {
                    InsertExcel(1);
                }

                else if (grid1.ActiveRow.Cells["ITEMCODE"].Value.ToString().Substring(6) == "07500")
                {
                    InsertExcel(2);
                }

                else
                {
                    InsertExcel(3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

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
                this._PlantCode = "SK2";
            }
            else if (this._PlantCode.Equals("EC"))
            {
                this._PlantCode = "SK2";
            }

            if (!(this._PlantCode.Equals("SK1") || this._PlantCode.Equals("SK2")))
            {
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;
            }

            this.cboStartDate_H.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 00:00:00");
            this.cboEndDate_H.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd") + " 23:59:59");

            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, "", "", "" }
                      , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { });
            }

            _RtnComDt = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", _RtnComDt, "CODE_ID", "CODE_NAME");

            _RtnComDt = _Common.GET_TBM0000_CODE("DAYNIGHT");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DAYNIGHT", _RtnComDt, "CODE_ID", "CODE_NAME");

            _RtnComDt = _Common.GET_TBM0000_CODE("ITEMCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "INSITEMNAME", _RtnComDt, "CODE_ID", "CODE_NAME");

            _ChangeDt = (DataTable)grid1.DataSource;
            _UltraGridUtil.SetInitUltraGridBind(this.grid1);
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            // 1. Initialize grid1
            _UltraGridUtil.InitializeGrid(this.grid1);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 55, 80, Infragistics.Win.HAlign.Center, (this._PlantCode == "") ? true : false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INDATE", "작업일자", false, GridColDataType_emu.DateTime24, 180, 30, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "LOTNO", "LOTNO", false, GridColDataType_emu.VarChar, 200, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "DAYNIGHT", "주야", false, GridColDataType_emu.VarChar, 80, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSJUDGE1", "#1\r\n(E1)", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSJUDGE2", "#2\r\n(BT31)", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSJUDGE3", "#3\r\n(T46)", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSJUDGE4", "#4\r\n(BT32)", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSJUDGE5", "#5\r\n(BT33)", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSJUDGE6", "#6\r\n(BT80)", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSJUDGE7", "#7\r\n(T45)", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSJUDGE8", "#8\r\n(BT34)", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSJUDGE9", "#9\r\n(T30/BT35)", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSJUDGE10", "#10\r\n(BT81/T30)", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장코드", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장", false, GridColDataType_emu.VarChar, 150, 10, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품번", false, GridColDataType_emu.VarChar, 120, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품목", false, GridColDataType_emu.VarChar, 150, 30, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKER", "작업자", false, GridColDataType_emu.VarChar, 80, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSPATH1", "경로1", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSPATH2", "경로2", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSPATH3", "경로3", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSPATH4", "경로4", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSPATH5", "경로5", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSPATH6", "경로6", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSPATH7", "경로7", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSPATH8", "경로8", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSPATH9", "경로9", false, GridColDataType_emu.VarChar, 50, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "INSPATH10", "경로10", false, GridColDataType_emu.VarChar, 60, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            grid1.Columns["PLANTCODE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            grid1.Columns["PLANTCODE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            grid1.Columns["PLANTCODE"].MergedCellStyle = MergedCellStyle.Always;

        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            grid1.InitializeRow += new InitializeRowEventHandler(grid1_InitializeRow);
            grid1.ClickCell += new ClickCellEventHandler(grid1_ClickCell);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            grid1.InitializeRow -= new InitializeRowEventHandler(grid1_InitializeRow);
            grid1.ClickCell -= new ClickCellEventHandler(grid1_ClickCell);
        }



        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper sqlDBHelper = new SqlDBHelper(true, false);
            SqlParameter[] sqlParameters = new SqlParameter[7];

            DateTime startInspDateTime = Convert.ToDateTime(((DateTime)this.cboStartDate_H.Value).ToString("yyyy-MM-dd") + " 08:00:00.00");
            DateTime endInspDateTime = Convert.ToDateTime(((DateTime)this.cboEndDate_H.Value).AddDays(1).ToString("yyyy-MM-dd") + " 07:59:59.99");

            try
            {
                base.DoInquire();

                if (Convert.ToInt32(startInspDateTime.ToString("yyyyMMdd")) > Convert.ToInt32(endInspDateTime.ToString("yyyMMdd")))
                {
                    SException ex = new SException("R00200", null);
                    throw ex;
                }

                this._PlantCode = "SK";
                this._WorkCenterCode = txtWorkCenterCode.Text;
                this._ItemCode = SqlDBHelper.nvlString(cboItemCode.Value);
                this._StartDateTime = startInspDateTime;
                this._EndDateTime = endInspDateTime;
                this._DayNight = SqlDBHelper.nvlString(cboDayNight.Value);
                this._LotNo = txtLotNo.Text;

                if (_DayNight == "D")
                {
                    _DayNight = "A";
                }
                else if (_DayNight == "N")
                {
                    _DayNight = "B";
                }
                else
                {
                    _DayNight = "";
                }

                sqlParameters[0] = sqlDBHelper.CreateParameter("@PLANTCODE", this._PlantCode + "%", SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@WORKCENTERCODE", _WorkCenterCode + "%", SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[2] = sqlDBHelper.CreateParameter("@ITEMCODE", "%" + _ItemCode + "%", SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[3] = sqlDBHelper.CreateParameter("@STARTDATE", _StartDateTime.ToString("yyyy-MM-dd"), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[4] = sqlDBHelper.CreateParameter("@ENDDATE", _EndDateTime.ToString("yyyy-MM-dd"), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[5] = sqlDBHelper.CreateParameter("@DAYNIGHT", _DayNight + "%", SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[6] = sqlDBHelper.CreateParameter("@LOTNO", "%" + _LotNo + "%", SqlDbType.VarChar, ParameterDirection.Input);

                _RtnGridDt = sqlDBHelper.FillTable("SP_SELECT_INSRESULT", CommandType.StoredProcedure, sqlParameters);

                if (_RtnGridDt.Rows.Count == 0)
                {
                    grid1.DataSource = _RtnGridDt;
                    grid1.DataBind();

                }
                else
                {
                    grid1.DataSource = _RtnGridDt;
                    grid1.DataBind();
                    //picImage.Load(@_RtnGridDt.Rows[0]["INSPATH1"].ToString());

                    _ChangeDt = _RtnGridDt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (sqlDBHelper._sConn != null) { sqlDBHelper._sConn.Close(); }
                if (sqlParameters != null) { sqlParameters = null; }
                grid1.DisplayLayout.CaptionAppearance.BackColor = Color.White;
            }
        }

        /// <summary>
        /// Do new
        /// </summary>
        public override void DoNew()
        {
            base.DoNew();
        }

        /// <summary>
        /// Do save
        /// </summary>
        public override void DoSave()
        {
            base.DoSave();
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();
        }

        /// <summary>
        /// Do down load excel
        /// </summary>
        public override void DoDownloadExcel()
        {
            base.DoDownloadExcel();
        }

        /// <summary>
        /// 성적서 발급(Excel)
        /// </summary>
        /// <param name="SheetNum"></param>
        private void InsertExcel(int SheetNum)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook _workbook = app.Workbooks.Open(Filename: Application.StartupPath + "\\실린더블럭.xlsx");
            Microsoft.Office.Interop.Excel.Worksheet _worksheet = _workbook.Worksheets[SheetNum];
            Microsoft.Office.Interop.Excel.Range oRng = null;

            try
            {
                oRng = _worksheet.Cells[3, 6];
                oRng.Value2 = grid1.ActiveRow.Cells["INDATE"].Value.ToString().Substring(0, 10);
                oRng = _worksheet.Cells[4, 6];
                oRng.Value2 = grid1.ActiveRow.Cells["INDATE"].Value.ToString().Substring(0, 10);
                oRng = _worksheet.Cells[5, 6];
                oRng.Value2 = grid1.ActiveRow.Cells["LOTNO"].Value.ToString();

                for (int i = 2; i < 7; i++)
                {
                    oRng = _worksheet.Cells[8, i];
                    _worksheet.Shapes.AddPicture(@grid1.ActiveRow.Cells["INSPATH" + (i - 1)].Value.ToString(), MsoTriState.msoFalse, MsoTriState.msoCTrue, oRng.Left, oRng.Top, oRng.Width, oRng.Height);
                    oRng = _worksheet.Cells[13, i];

                    if (SheetNum == 1 && i == 6)
                    {

                    }

                    else
                    {
                        _worksheet.Shapes.AddPicture(@grid1.ActiveRow.Cells["INSPATH" + (i + 4)].Value.ToString(), MsoTriState.msoFalse, MsoTriState.msoCTrue, oRng.Left, oRng.Top, oRng.Width, oRng.Height);
                    }


                }

                //oRng.Copy();
                //oRng.PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteColumnWidths, Microsoft.Office.Interop.Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                //oRng = _worksheet.Cells[18, 2];
                //_worksheet.Paste();

                ((Microsoft.Office.Interop.Excel.Worksheet)app.ActiveWorkbook.Sheets[SheetNum]).Select(Type.Missing);
                app.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ReleaseObject(_worksheet);
                ReleaseObject(_workbook);
                ReleaseObject(app);
            }
        }

        /// <summary>
        /// 액셀 객체 해제 메소드
        /// </summary>
        /// <param name="obj"></param>
        static void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        #endregion
    }
}
