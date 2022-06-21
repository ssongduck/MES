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
using Infragistics.UltraChart.Resources.Appearance;
using System.Linq;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.Data;

namespace SAMMI.QM
{
    /// <summary>
    /// QM0403 class
    /// </summary>
    public partial class QM0403 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _RtnDt = new DataTable();

        /// <summary>
        /// 
        /// </summary>
        string[] _EmptyArray = { "v_txt" };

        /// <summary>
        /// Plant code
        /// </summary>
        private string _sPlantCode = string.Empty;

        /// <summary>
        /// Grid util
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        BizTextBoxManagerEX btbManager;

        #endregion

        #region Constructor

        /// <summary>
        /// QM0403 constructor
        /// </summary>
        public QM0403()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();

            this.layoutControlFront.CanvasWidth = 1000;
            this.layoutControlFront.CanvasHeight = 600;
            this.layoutControlBack.CanvasWidth = 1000;
            this.layoutControlBack.CanvasHeight = 600;
            this.layoutControlFrontBack.CanvasWidth  = 1000;
            this.layoutControlFrontBack.CanvasHeight = 600;
            this.layoutControlFrontBack1.CanvasWidth = 1000;
            this.layoutControlFrontBack1.CanvasHeight = 600;

            txtWorkCenterCode.Focus();
        }

        #endregion

        #region Event

        /// <summary>
        /// PP0341 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QM0403_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();

            this.layoutControlFront.Dispose();
            this.layoutControlBack.Dispose();
            this.layoutControlFrontBack.Dispose();
            this.layoutControlFrontBack1.Dispose();
        }

        /// <summary>
        /// Grid3 click cell event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid3_ClickCell(object sender, ClickCellEventArgs e)
        {
            Grid3SelectRow();
        }

        /// <summary>
        /// Grid6 click cell event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid6_ClickCell(object sender, ClickCellEventArgs e)
        {
            string sMold = string.Empty;
            string sSubWorkCenterCode = string.Empty;
            string sWorkCenterCode = txtWorkCenterCode.Text;

            this.layoutControlFrontBack.faultCodeNodesList.Clear();
            this.layoutControlFrontBack.DrawRectangle();

            this.layoutControlFrontBack1.faultCodeNodesList.Clear();
            this.layoutControlFrontBack1.DrawRectangle();

            if (grid6.ActiveRow != null)
            {
                sMold = grid6.ActiveRow.Cells["MOLD"].Value.ToString();
                if (sSubWorkCenterCode == "ALL")
                {
                    sSubWorkCenterCode = "%";
                }
                else
                {
                    if (!string.IsNullOrEmpty(sWorkCenterCode))
                    {
                        sSubWorkCenterCode = sWorkCenterCode + sSubWorkCenterCode;
                    }
                    else
                    {
                        sSubWorkCenterCode = "%";
                    }
                }

                if (!string.IsNullOrEmpty(sMold))
                {
                    DoInquireChart(sMold);
                }
            }
        }

        /// <summary>
        /// Grid1 initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["CAST_LOTNO"].Value.ToString() == "합 계")
            {
                e.Row.Appearance.BackColor = Color.LightBlue;
            }
        }

        /// <summary>
        /// Grid2 initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid2_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["MOLD"].Value.ToString() == "합 계")
            {
                e.Row.Appearance.BackColor = Color.LightBlue;
            }
        }

        /// <summary>
        /// Grid3 initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid3_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["GBN_NM"].Value.ToString() == "합 계")
            {
                e.Row.Appearance.BackColor = Color.LightBlue;
            }
        }

        /// <summary>
        /// Grid4 initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid4_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["CREATE_DT"].Value.ToString() == "합 계")
            {
                e.Row.Appearance.BackColor = Color.LightBlue;
            }
        }

        /// <summary>
        ///  Grid5 initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid5_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["CREATE_DT"].Value.ToString() == "합 계")
            {
                e.Row.Appearance.BackColor = Color.LightBlue;
            }
        }

        /// <summary>
        ///  Grid6 initialize row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid6_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["MOLD"].Value.ToString() == "합 계")
            {
                e.Row.Appearance.BackColor = Color.LightBlue;
            }
        }

        /// <summary>
        /// Grid3 mouse click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid3_MouseClick(object sender, MouseEventArgs e)
        {
            DataTable dt = gridControlLocation.DataSource as DataTable;

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["FLAG"] = "N";
                }

                dt.AcceptChanges();
            }
        }

        /// <summary>
        /// Grid view location poupmenushowing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewLocation_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.Menu == null) return;

                DXMenuItem dXMenuItem = new DevExpress.Utils.Menu.DXMenuItem("엑셀 내보내기", new EventHandler(this.exportLocationMenu_Click));
                e.Menu.Items.Add(dXMenuItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Location  grid view rowstyle event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewLocation_RowStyle(object sender, RowStyleEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView gridView = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            if (e.RowHandle >= 0)
            {
                if (gridView.GetRowCellDisplayText(e.RowHandle, "FLAG") == "Y")
                {
                    e.Appearance.BackColor = System.Drawing.Color.FromArgb(255, 192, 0);
                }
            }
        }

        /// <summary>
        /// Export location menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportLocationMenu_Click(object sender, EventArgs e)
        {
            ExportExcel(gridViewLocation);
        }

        /// <summary>
        /// Export excel
        /// </summary>
        /// <param name="gridView"></param>
        private void ExportExcel(GridView gridView)
        {
            try
            {
                gridView.OptionsPrint.AutoWidth = false;
                gridView.OptionsPrint.PrintHeader = true;
                gridView.OptionsPrint.PrintSelectedRowsOnly = false;
                gridView.OptionsPrint.ExpandAllDetails = true;
                gridView.OptionsPrint.ExpandAllGroups = true;
                gridView.OptionsPrint.PrintDetails = true;
                gridView.OptionsPrint.UsePrintStyles = true;

                SaveFileDialog dialog = new SaveFileDialog();

                dialog.Filter = "Excel File(*.xlsx)|*.xlsx|All Files(*.*)|*.*";
                dialog.Title = "엑셀로 저장";

                if ((dialog.ShowDialog()) == DialogResult.OK)
                {
                    string filePath = dialog.FileName.ToString();
                    gridView.ExportToXlsx(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Tab control1 selected index changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.TabControl tabControl = (System.Windows.Forms.TabControl)sender;

            if (tabControl.SelectedTab.Text == "[ 위치별 불량 현황 ]")
            {
                DataTable dt = grid3.DataSource as DataTable;

                if (dt != null && dt.Rows.Count > 0)
                {
                    grid3.Rows[0].Activate();
                    Grid3SelectRow();
                }
            }
        }

        /// <summary>
        /// Item code textbox textchanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtItemCode_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemCode.Text))
            {
                ImageChange(string.Empty);
            }
            else
            {
                ImageChange(txtItemCode.Text);
            }
        }

        /// <summary>
        /// Layout control front diagram click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void layoutControlFront_DiagramClick(object sender, DiagramClickEventArgs e)
        {
            SetLocationInfo(GetLocation(e._FaultDetailNode.LOCATION, FrontBack.FRONT, GetFaultCode(e._FaultDetailNode.LOCATION)));
        }

        /// <summary>
        /// Layout control back diagram click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void layoutControlBack_DiagramClick(object sender, DiagramClickEventArgs e)
        {
            SetLocationInfo(GetLocation(e._FaultDetailNode.LOCATION, FrontBack.BACK, GetFaultCode(e._FaultDetailNode.LOCATION)));
        }

        #endregion 

        #region Method

        /// <summary>
        /// Grid3 select row
        /// </summary>
        private void Grid3SelectRow()
        {
            this.layoutControlFront.faultCodeNodesList.Clear();
            this.layoutControlBack.faultCodeNodesList.Clear();
            this.layoutControlFront.DrawRectangle();
            this.layoutControlBack.DrawRectangle();
            List<FaultCodeNodes> frontFaultCodeNodesList = new List<FaultCodeNodes>();
            List<FaultCodeNodes> backFaultCodeNodesList = new List<FaultCodeNodes>();
            string prefix = string.Empty;

            char[] delimiterChars = { '/' };

            if (grid3.ActiveRow != null)
            {
                for (int i = 2; i < grid3.ActiveRow.Cells.Count; i++)
                {
                    if (!string.IsNullOrEmpty(grid3.ActiveRow.Cells[i].Value.ToString()))
                    {
                        if (i >= 3 && i <= 7)
                        {
                            prefix = "A";
                        }
                        else if (i >= 8 && i <= 12)
                        {
                            prefix = "B";
                        }
                        else if (i >= 13 && i <= 17)
                        {
                            prefix = "C";
                        }
                        else if (i >= 18 && i <= 22)
                        {
                            prefix = "D";
                        }
                        else if (i >= 23 && i <= 27)
                        {
                            prefix = "E";
                        }
                        else
                        {
                            continue;
                        }

                        string[] tempArray = grid3.ActiveRow.Cells[i].Value.ToString().Split(delimiterChars);

                        if (tempArray != null)
                        {
                            if (!string.IsNullOrEmpty(tempArray[0].Trim()))
                            {
                                FaultCodeNodes frontFaultCodeNodes = new FaultCodeNodes();
                                frontFaultCodeNodes.LOCATION = string.Format("{0}{1}", prefix, ((i - 2) % 5) == 0 ? 5 : (i - 2) % 5);
                                frontFaultCodeNodes.FAULT_COUNT = int.Parse(tempArray[0].Trim());

                                frontFaultCodeNodesList.Add(frontFaultCodeNodes);
                            }

                            if (!string.IsNullOrEmpty(tempArray[1].Trim()))
                            {
                                FaultCodeNodes backFaultCodeNodes = new FaultCodeNodes();
                                backFaultCodeNodes.LOCATION = string.Format("{0}{1}", prefix, ((i - 2) % 5) == 0 ? 5 : (i - 2) % 5);
                                backFaultCodeNodes.FAULT_COUNT = int.Parse(tempArray[1].Trim());

                                backFaultCodeNodesList.Add(backFaultCodeNodes);
                            }
                        }
                    }
                }
            }

            if (frontFaultCodeNodesList != null && frontFaultCodeNodesList.Count > 0)
            {
                this.layoutControlFront.faultCodeNodesList = frontFaultCodeNodesList;
                this.layoutControlFront.DrawRectangle();
                this.layoutControlFront.SelectNode(frontFaultCodeNodesList);
            }

            if (backFaultCodeNodesList != null && backFaultCodeNodesList.Count > 0)
            {
                this.layoutControlBack.faultCodeNodesList = backFaultCodeNodesList;
                this.layoutControlBack.DrawRectangle();
                this.layoutControlBack.SelectNode(backFaultCodeNodesList);
            }
        }

        /// <summary>
        /// Initialize control
        /// </summary>
        private void InitializeControl()
        {
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this._sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            //if (this._sPlantCode.Equals("SK"))
            //{
            //    this._sPlantCode = "SK1";
            //}
            //else if (this._sPlantCode.Equals("EC"))
            //{
            //    this._sPlantCode = "SK2";
            //}
            //else
            //{
            //    if (cboPlantCode_H.Value == null)
            //    {
            //        cboPlantCode_H.Value = "ALL";
            //        this._sPlantCode = "ALL";
            //    }
            //}

            this._sPlantCode = "SK2";   //사업장 서산 지정
            cboPlantCode_H.Value = "SK2";
            cmbSubWorkCenterCode.SelectedItem = "ALL";
            txtOPCode.Text = "4000";    //가공조립지정

            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, "", "" }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
                btbManager.PopUpAdd(txtItemCode, txtItemCodeName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, "F", }
                       , new string[] { "WorkCenterCode", "WorkCenterName", "ItemType" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, "", "" }
                         , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
                btbManager.PopUpAdd(txtItemCode, txtItemCodeName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, "F",}
                       , new string[] { "WorkCenterCode", "WorkCenterName", "ItemType" }, new object[] { });
            }

            ChartTitle chartTitle1 = new ChartTitle();
            chartTitle1.Text = "[ 일자별 불량 추이도 ]";
            chartTitle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartFaultResult.Titles.Add(chartTitle1);


            ChartTitle chartTitle2 = new ChartTitle();
            chartTitle2.Text = "[ 금형별 불량 수량 ]";
            chartTitle2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartControlPie.Titles.Add(chartTitle2);

            ChartTitle chartTitle3 = new ChartTitle();
            chartTitle3.Text = "[ 금형 일자별 불량 추이도 ]";
            chartTitle3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartControlMold.Titles.Add(chartTitle3);

            ChartTitle chartTitle4 = new ChartTitle();
            chartTitle4.Text = "[ 주조 LotNo 별 불량 수량 ]";
            chartTitle4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            chartControlLot1.Titles.Add(chartTitle4);

            calFromDt.Value = DateTime.Now;
            calToDt.Value = DateTime.Now;
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            _UltraGridUtil.InitializeGrid(this.grid1, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "CAST_LOTNO", "주조LOTNO", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "LOTNO", "가공LOTNO", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "MOLD", "금형차수", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "CREATE_DT", "검사일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "102", "기포", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "208", "변형", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "105", "살떨어짐", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "101", "미성형", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "106", "박리", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "110", "흑피", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "113", "사상", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "104", "소착", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "117", "부풀음", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "103", "크랙", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "109", "변색", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "124", "이물질", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "123", "소재(기타)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "121", "찍힘(소재)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "111", "리크", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "118", "겹주조", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "206", "찍힘(가공)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "209", "칩눌림", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "215", "인선", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "201", "단차떨림", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "205", "치수", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "211", "스크래치", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "210", "세팅TRAIL", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "207", "공구파손", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "216", "백화(발청)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "217", "가공(기타)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "219", "확공", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "222", "흑피", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _UltraGridUtil.InitColumnUltraGrid(grid1, "992", "홀가공버재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "993", "홀박리재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "994", "사상재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "995", "박리재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "996", "결육재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "997", "버재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "998", "찍힘재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "999", "스크래치재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid1, "TOTAL", "합계", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _UltraGridUtil.InitializeGrid(this.grid3, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(grid3, "GBN_NM", "구분", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "FAULT_NAME", "불량명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "TOTAL", "합계", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "A1", "A-1", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "A2", "A-2", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "A3", "A-3", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "A4", "A-4", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "A5", "A-5", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "B1", "B-1", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "B2", "B-2", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "B3", "B-3", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "B4", "B-4", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "B5", "B-5", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "C1", "C-1", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "C2", "C-2", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "C3", "C-3", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "C4", "C-4", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "C5", "C-5", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "D1", "D-1", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "D2", "D-2", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "D3", "D-3", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "D4", "D-4", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "D5", "D-5", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "E1", "E-1", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "E2", "E-2", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "E3", "E-3", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "E4", "E-4", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid3, "E5", "E-5", false, GridColDataType_emu.VarChar, 56, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _UltraGridUtil.InitializeGrid(this.grid4, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(grid4, "CREATE_DT", "검사일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "102", "기포", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "208", "변형", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "105", "살떨어짐", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "101", "미성형", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "106", "박리", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "110", "흑피", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "113", "사상", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "104", "소착", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "117", "부풀음", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "103", "크랙", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "109", "변색", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "124", "이물질", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "123", "소재(기타)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "121", "찍힘(소재)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "111", "리크", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "118", "겹주조", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "206", "찍힘(가공)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "209", "칩눌림", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "215", "인선", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "201", "단차떨림", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "205", "치수", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "211", "스크래치", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "210", "세팅TRAIL", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "207", "공구파손", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "216", "백화(발청)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "217", "가공(기타)", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "219", "확공", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "222", "흑피", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _UltraGridUtil.InitColumnUltraGrid(grid4, "992", "홀가공버재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "993", "홀박리재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "994", "사상재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "995", "박리재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "996", "결육재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "997", "버재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "998", "찍힘재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "999", "스크래치재작업", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid4, "TOTAL", "합계", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _UltraGridUtil.InitializeGrid(this.grid5, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(grid5, "CREATE_DT", "검사일", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid5, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 300, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid5, "CAST_LOTNO", "주조LOTNO", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid5, "MAT_CNT", "소재불량", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid5, "MAT_RECNT", "소재재작업", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid5, "WRK_CNT", "가공불량", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid5, "WRK_RECNT", "가공재작업", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);

            _UltraGridUtil.InitializeGrid(this.grid6, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(grid6, "MOLD", "금형", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid6, "INS_QTY", "검사수량", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid6, "FAULT_COUNT", "수량", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid6, "FAULT_PER", "불량율(%)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

            _UltraGridUtil.InitializeGrid(this.grid8, true, false, false, "", false);

            _UltraGridUtil.InitColumnUltraGrid(grid8, "CREATE_DT", "작업시작일자", false, GridColDataType_emu.VarChar, 80, 110, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid8, "UPDATE_DT", "최근작업일자", false, GridColDataType_emu.VarChar, 80, 110, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid8, "MOLD", "금형차수", false, GridColDataType_emu.VarChar, 50, 70, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid8, "CAST_LOTNO", "소재LOTNO", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid8, "SUBWORKCENTERCODE", "가공라인", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid8, "INS_QTY", "검사수량", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid8, "FAULT_QTY", "불량수량", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid8, "FAULT_PER", "불량율(%)", false, GridColDataType_emu.VarChar, 60, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(grid8, "STATE", "상태", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);


            grid3.DisplayLayout.UseFixedHeaders = true;
            for (int i = 0; i < 3; i++)
                grid3.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;

        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.Disposed += new EventHandler(QM0403_Disposed);

            grid3.ClickCell += new ClickCellEventHandler(grid3_ClickCell);
            grid6.ClickCell += new ClickCellEventHandler(grid6_ClickCell);
            grid1.InitializeRow += new InitializeRowEventHandler(grid1_InitializeRow);

            grid3.InitializeRow += new InitializeRowEventHandler(grid3_InitializeRow);
            grid4.InitializeRow += new InitializeRowEventHandler(grid4_InitializeRow);
            grid5.InitializeRow += new InitializeRowEventHandler(grid5_InitializeRow);
            grid6.InitializeRow += new InitializeRowEventHandler(grid6_InitializeRow);
            grid3.MouseClick += new MouseEventHandler(grid3_MouseClick);

            gridViewLocation.PopupMenuShowing += new PopupMenuShowingEventHandler(gridViewLocation_PopupMenuShowing);
            gridViewLocation.RowStyle += new RowStyleEventHandler(gridViewLocation_RowStyle);

            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            txtItemCode.TextChanged += new EventHandler(txtItemCode_TextChanged);

            layoutControlFront.DiagramClick += new EventHandler<DiagramClickEventArgs>(layoutControlFront_DiagramClick);
            layoutControlBack.DiagramClick += new EventHandler<DiagramClickEventArgs>(layoutControlBack_DiagramClick);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(QM0403_Disposed);

            grid3.ClickCell -= new ClickCellEventHandler(grid3_ClickCell);
            grid6.ClickCell -= new ClickCellEventHandler(grid6_ClickCell);
            grid1.InitializeRow -= new InitializeRowEventHandler(grid1_InitializeRow);
            grid3.InitializeRow -= new InitializeRowEventHandler(grid3_InitializeRow);
            grid4.InitializeRow -= new InitializeRowEventHandler(grid4_InitializeRow);
            grid5.InitializeRow -= new InitializeRowEventHandler(grid5_InitializeRow);
            grid6.InitializeRow -= new InitializeRowEventHandler(grid6_InitializeRow);
            grid3.MouseClick -= new MouseEventHandler(grid3_MouseClick);

            gridViewLocation.PopupMenuShowing -= new PopupMenuShowingEventHandler(gridViewLocation_PopupMenuShowing);
            gridViewLocation.RowStyle -= new RowStyleEventHandler(gridViewLocation_RowStyle);

            tabControl1.SelectedIndexChanged -= new EventHandler(tabControl1_SelectedIndexChanged);
            txtItemCode.TextChanged -= new EventHandler(txtItemCode_TextChanged);

            layoutControlFront.DiagramClick -= new EventHandler<DiagramClickEventArgs>(layoutControlFront_DiagramClick);
            layoutControlBack.DiagramClick -= new EventHandler<DiagramClickEventArgs>(layoutControlBack_DiagramClick);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            this.layoutControlFront.faultCodeNodesList.Clear();
            this.layoutControlBack.faultCodeNodesList.Clear();
            this.layoutControlFront.DrawRectangle();
            this.layoutControlBack.DrawRectangle();
            this.layoutControlFrontBack.faultCodeNodesList.Clear();
            this.layoutControlFrontBack.DrawRectangle();
            this.layoutControlFrontBack1.faultCodeNodesList.Clear();
            this.layoutControlFrontBack1.DrawRectangle();

            chartControlPie.Series.Clear();
            chartControlMold.Series.Clear();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DataTable dt5 = new DataTable();
            DataTable dt6 = new DataTable();
            DataTable dt7 = new DataTable();
            DataTable dt8 = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(false, false);
            SqlParameter[] sqlParameters1 = new SqlParameter[9];
            SqlParameter[] sqlParameters2 = new SqlParameter[9];
            SqlParameter[] sqlParameters3 = new SqlParameter[9];
            SqlParameter[] sqlParameters4 = new SqlParameter[9];
            SqlParameter[] sqlParameters5 = new SqlParameter[10];
            SqlParameter[] sqlParameters6 = new SqlParameter[9];
            SqlParameter[] sqlParameters7 = new SqlParameter[9];
            SqlParameter[] sqlParameters8 = new SqlParameter[8];

            ClearAllControl();

            try
            {
                if (string.IsNullOrEmpty(txtItemCode.Text))
                {
                    ShowDialog("품목코드는 필수 입력사항입니다. ", Windows.Forms.DialogForm.DialogType.OK);
                    txtItemCode.Focus();
                    return;
                }

                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sWorkCenterCode = txtWorkCenterCode.Text;
                string sSubWorkCenterCode = cmbSubWorkCenterCode.Text;

                if (sSubWorkCenterCode == "ALL")
                {
                    sSubWorkCenterCode = "%";
                }
                else
                {
                    if (!string.IsNullOrEmpty(sWorkCenterCode))
                    {
                        sSubWorkCenterCode = sWorkCenterCode + sSubWorkCenterCode;
                    }
                    else
                    {
                        sSubWorkCenterCode = "%";
                    }
                }

                base.DoInquire();

                sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[7] = sqlDBHelper.CreateParameter("@AS_MOLD", txtMold.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[8] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters2[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[7] = sqlDBHelper.CreateParameter("@AS_MOLD", txtMold.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[8] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters3[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[7] = sqlDBHelper.CreateParameter("@AS_MOLD", txtMold.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[8] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters4[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters4[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters4[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters4[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters4[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters4[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters4[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters4[7] = sqlDBHelper.CreateParameter("@AS_MOLD", txtMold.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters4[8] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters5[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters5[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters5[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters5[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters5[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters5[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters5[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters5[7] = sqlDBHelper.CreateParameter("@AS_MOLD", txtMold.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters5[8] = sqlDBHelper.CreateParameter("@AS_TYPE", "Y", SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters5[9] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters6[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters6[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters6[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters6[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters6[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters6[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters6[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters6[7] = sqlDBHelper.CreateParameter("@AS_MOLD", txtMold.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters6[8] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters7[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters7[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters7[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters7[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters7[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters7[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters7[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters7[7] = sqlDBHelper.CreateParameter("@AS_MOLD", txtMold.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters7[8] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters8[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters8[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters8[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters8[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters8[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters8[5] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", "", SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters8[6] = sqlDBHelper.CreateParameter("@AS_MOLD", txtMold.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters8[7] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                dt1 = sqlDBHelper.FillTable("SP_GET_FAULT_RESULT", CommandType.StoredProcedure, sqlParameters1);
                dt2 = sqlDBHelper.FillTable("SP_GET_FAULT_RESULT_LIST", CommandType.StoredProcedure, sqlParameters2);
                dt3 = sqlDBHelper.FillTable("SP_GET_LOCATION_FAULT_RESULT", CommandType.StoredProcedure, sqlParameters3);
                dt4 = sqlDBHelper.FillTable("SP_GET_DATE_FAULT_RESULT", CommandType.StoredProcedure, sqlParameters4);
                dt5 = sqlDBHelper.FillTable("SP_GET_CHART_FAULT_RESULT", CommandType.StoredProcedure, sqlParameters5);
                dt6 = sqlDBHelper.FillTable("SP_GET_FAULT_RESULT_COUNT", CommandType.StoredProcedure, sqlParameters6);
                dt7 = sqlDBHelper.FillTable("SP_GET_MOLD_FAULT_RESULT1", CommandType.StoredProcedure, sqlParameters7);
                dt8 = sqlDBHelper.FillTable("SP_GET_VW065_FAULT_LOT", CommandType.StoredProcedure, sqlParameters8);

                grid1.DataSource = dt1;
                grid1.DataBind();

                if (gridViewLocation.Columns["LOTNO"].Summary.Count > 0) gridViewLocation.Columns["LOTNO"].Summary.Clear();

                gridViewLocation.Columns["LOTNO"].Summary.Add(new GridColumnSummaryItem(SummaryItemType.Count, "LOTNO", "전체 행:{0:#,###}건"));

                gridControlLocation.DataSource = dt2;

                grid3.DataSource = dt3;
                grid3.DataBind();

                grid4.DataSource = dt4;
                grid4.DataBind();

                BindChart(dt5);

                grid5.DataSource = dt6;
                grid5.DataBind();

                grid6.DataSource = dt7;
                grid6.DataBind();

                BindLot1Chart(dt6);

                grid8.DataSource = dt8;
                grid8.DataBind();

                if (dt3 != null && dt3.Rows.Count > 0)
                {
                    grid3.Rows[0].Activate();
                    Grid3SelectRow();
                }

                if (dt7 != null && dt7.Rows.Count > 0)
                {
                    string sMold = string.Empty;

                    if (grid6.Rows[0] != null)
                    {
                        sMold = grid6.Rows[0].Cells["MOLD"].Value.ToString();

                        if (!string.IsNullOrEmpty(sMold))
                        {
                            DoInquireChart(sMold);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Do inquire chart
        /// </summary>
        /// <param name="sMold"></param>
        public void DoInquireChart(string sMold)
        {
            string sWorkCenterCode = txtWorkCenterCode.Text;
            string sSubWorkCenterCode = cmbSubWorkCenterCode.Text;

            if (sSubWorkCenterCode == "ALL")
            {
                sSubWorkCenterCode = "%";
            }
            else
            {
                if (!string.IsNullOrEmpty(sWorkCenterCode))
                {
                    sSubWorkCenterCode = sWorkCenterCode + sSubWorkCenterCode;
                }
                else
                {
                    sSubWorkCenterCode = "%";
                }
            }

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters1 = new SqlParameter[10];
            SqlParameter[] sqlParameters2 = new SqlParameter[10];
            SqlParameter[] sqlParameters3 = new SqlParameter[9];

            ClearAllControl();

            try
            {
                if (sMold == "합 계")
                {
                    sMold = "%";
                }

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);

                sqlParameters1[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[7] = sqlDBHelper.CreateParameter("@AS_MOLD", sMold, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[8] = sqlDBHelper.CreateParameter("@AS_TYPE", "N", SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters1[9] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters2[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[7] = sqlDBHelper.CreateParameter("@AS_MOLD", sMold, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[8] = sqlDBHelper.CreateParameter("@AS_TYPE", "Y", SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters2[9] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                sqlParameters3[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[7] = sqlDBHelper.CreateParameter("@AS_MOLD", sMold, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters3[8] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);

                dt1 = sqlDBHelper.FillTable("SP_GET_CHART_FAULT_RESULT", CommandType.StoredProcedure, sqlParameters1);
                dt2 = sqlDBHelper.FillTable("SP_GET_CHART_FAULT_RESULT", CommandType.StoredProcedure, sqlParameters2);
                dt3 = sqlDBHelper.FillTable("SP_GET_LOCATION_FAULT_RESULT_LIST", CommandType.StoredProcedure, sqlParameters3);

                BindMoldChart1(dt1);
                BindMoldChart2(dt2);

                List<FaultCodeNodes> faultFrontCodeNodesList = new List<FaultCodeNodes>();
                List<FaultCodeNodes> faultBackCodeNodesList = new List<FaultCodeNodes>();

                if (dt3 != null && dt3.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt3.AsEnumerable().Where(t => t.Field<string>("FRONT_BACK") == "FRONT"))
                    {
                        FaultCodeNodes faultCodeNodes = new FaultCodeNodes();
                        faultCodeNodes.LOCATION = dr["LOCATION"].ToString();
                        faultCodeNodes.FAULT_COUNT = int.Parse(dr["FAULT_COUNT"].ToString());

                        faultFrontCodeNodesList.Add(faultCodeNodes);
                    }

                    if (faultFrontCodeNodesList != null && faultFrontCodeNodesList.Count > 0)
                    {
                        this.layoutControlFrontBack.faultCodeNodesList = faultFrontCodeNodesList;
                        this.layoutControlFrontBack.DrawRectangle();
                        this.layoutControlFrontBack.SelectNode(faultFrontCodeNodesList);
                    }

                    foreach (DataRow dr in dt3.AsEnumerable().Where(t => t.Field<string>("FRONT_BACK") == "BACK"))
                    {
                        FaultCodeNodes faultCodeNodes = new FaultCodeNodes();
                        faultCodeNodes.LOCATION = dr["LOCATION"].ToString();
                        faultCodeNodes.FAULT_COUNT = int.Parse(dr["FAULT_COUNT"].ToString());

                        faultBackCodeNodesList.Add(faultCodeNodes);
                    }

                    if (faultBackCodeNodesList != null && faultBackCodeNodesList.Count > 0)
                    {
                        this.layoutControlFrontBack1.faultCodeNodesList = faultBackCodeNodesList;
                        this.layoutControlFrontBack1.DrawRectangle();
                        this.layoutControlFrontBack1.SelectNode(faultBackCodeNodesList);
                    }
                }
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
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
        }

        /// <summary>
        /// Do save
        /// </summary>
        public override void DoSave()
        {
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

        /// <summary>
        /// Bind chart
        /// </summary>
        /// <param name="dt"></param>
        private void BindChart(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                chartFaultResult.Series.Clear();

                foreach (string faultName in dt.AsEnumerable().Select(t => t.Field<string>("FAULT_NAME")).Distinct())
                {
                    DataTable tempDt = dt.Clone();

                    Series series = new Series(faultName, ViewType.Line);

                    foreach (DataRow dr in dt.AsEnumerable().Where(t => t.Field<string>("FAULT_NAME") == faultName))
                    {
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dr["CREATE_DT"].ToString()), int.Parse(dr["FAULT_COUNT"].ToString())));
                    }

                    series.ArgumentScaleType = ScaleType.DateTime;
                    series.ValueScaleType = ScaleType.Numerical;
                    ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Triangle;
                    ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Solid;

                    chartFaultResult.Series.Add(series);
                }

                XYDiagram diagram = chartFaultResult.Diagram as XYDiagram;
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                diagram.AxisX.Label.TextPattern = "{V:yyyy-MM-dd}";
                diagram.AxisY.Label.TextPattern = "{V:n}";
                diagram.EnableAxisXScrolling = true;
                diagram.EnableAxisXZooming = true;
                diagram.EnableAxisYScrolling = true;
                diagram.EnableAxisYZooming = true;
            }
            else
            {
                chartFaultResult.Series.Clear();
            }
        }

        /// <summary>
        /// Bind mold chart1
        /// </summary>
        /// <param name="dt"></param>
        private void BindMoldChart1(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                chartControlPie.Series.Clear();
                Series series = new Series("", ViewType.Pie);

                foreach (DataRow dr in dt.Rows)
                {
                    series.Points.Add(new SeriesPoint(dr["FAULT_NAME"].ToString(), int.Parse(dr["FAULT_COUNT"].ToString())));
                }

                series.ArgumentScaleType = ScaleType.Auto;
                series.ValueScaleType = ScaleType.Numerical;

                series.Label.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

                chartControlPie.Series.Add(series);
                series.Label.TextPattern = "{A}: {V:0ea}";

                ((PieSeriesLabel)series.Label).Position = PieSeriesLabelPosition.TwoColumns;

                ((PieSeriesLabel)series.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;

                PieSeriesView pieSeriesView = (PieSeriesView)series.View;

                pieSeriesView.Titles.Add(new SeriesTitle());
                pieSeriesView.Titles[0].Text = series.Name;

                pieSeriesView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Value_1, DataFilterCondition.GreaterThanOrEqual, 9));
                pieSeriesView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.NotEqual, "Others"));
                pieSeriesView.ExplodeMode = PieExplodeMode.UseFilters;
                pieSeriesView.RuntimeExploding = true;

                chartControlPie.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            }
            else
            {
                chartControlPie.Series.Clear();
            }
        }

        /// <summary>
        /// Bind mold chart2
        /// </summary>
        /// <param name="dt"></param>
        private void BindMoldChart2(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                chartControlMold.Series.Clear();

                foreach (string faultName in dt.AsEnumerable().Select(t => t.Field<string>("FAULT_NAME")).Distinct().OrderBy(t => t[1]))
                {
                    DataTable tempDt = dt.Clone();

                    Series series = new Series(faultName, ViewType.Line);

                    foreach (DataRow dr in dt.AsEnumerable().Where(t => t.Field<string>("FAULT_NAME") == faultName))
                    {
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dr["CREATE_DT"].ToString()), int.Parse(dr["FAULT_COUNT"].ToString())));
                    }

                    series.ArgumentScaleType = ScaleType.DateTime;
                    series.ValueScaleType = ScaleType.Numerical;
                    ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Triangle;
                    ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Solid;

                    chartControlMold.Series.Add(series);
                }

                XYDiagram diagram = chartControlMold.Diagram as XYDiagram;
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                diagram.AxisX.Label.TextPattern = "{V:yyyy-MM-dd}";
                diagram.AxisY.Label.TextPattern = "{V:n}";
                diagram.EnableAxisXScrolling = true;
                diagram.EnableAxisXZooming = true;
                diagram.EnableAxisYScrolling = true;
                diagram.EnableAxisYZooming = true;
            }
            else
            {
                chartControlMold.Series.Clear();
            }
        }

        /// <summary>
        /// Bind lot1 chart
        /// </summary>
        /// <param name="dt"></param>
        private void BindLot1Chart(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                chartControlLot1.Series.Clear();
                Series series = new Series("", ViewType.Pie);
                string sCastLotNo = string.Empty;
                int? iFaultCount = 0;

                foreach (var tempVal in dt.AsEnumerable().GroupBy(t => t.Field<string>("CAST_LOTNO")))
                {
                    if (tempVal != null && !string.IsNullOrEmpty(tempVal.Key))
                    {
                        sCastLotNo = tempVal.Key;
                        sCastLotNo = string.Format("{0}-{1}-{2}", tempVal.Key.Substring(0, 2), tempVal.Key.Substring(2, 2), tempVal.Key.Substring(4, 2));
                        iFaultCount = dt.AsEnumerable().Where(t => t.Field<string>("CAST_LOTNO") == tempVal.Key).Select(t => t.Field<int?>("MAT_CNT")).Sum();
                        iFaultCount += dt.AsEnumerable().Where(t => t.Field<string>("CAST_LOTNO") == tempVal.Key).Select(t => t.Field<int?>("WRK_CNT")).Sum();
                        series.Points.Add(new SeriesPoint(sCastLotNo, iFaultCount));
                    }
                }

                series.ArgumentScaleType = ScaleType.Auto;
                series.ValueScaleType = ScaleType.Numerical;

                series.Label.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

                chartControlLot1.Series.Add(series);
                series.Label.TextPattern = "{A:yy-MM-dd}: {V:0ea}";

                ((PieSeriesLabel)series.Label).Position = PieSeriesLabelPosition.TwoColumns;

                ((PieSeriesLabel)series.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;

                PieSeriesView pieSeriesView = (PieSeriesView)series.View;

                pieSeriesView.Titles.Add(new SeriesTitle());
                pieSeriesView.Titles[0].Text = series.Name;

                pieSeriesView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Value_1, DataFilterCondition.GreaterThanOrEqual, 9));
                pieSeriesView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.NotEqual, "Others"));
                pieSeriesView.ExplodeMode = PieExplodeMode.UseFilters;
                pieSeriesView.RuntimeExploding = true;

                chartControlLot1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            }
            else
            {
                chartControlLot1.Series.Clear();
            }
        }

        /// <summary>
        /// Image change
        /// </summary>
        /// <param name="sItemCode"></param>
        private void ImageChange(string sItemCode)
        {
            if (string.IsNullOrEmpty(sItemCode))
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.EMPTY, string.Empty);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.EMPTY, string.Empty);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.EMPTY, string.Empty);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.EMPTY, string.Empty);
            }
            else if (sItemCode == "0BH325066E")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0BH325066E, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_0BH325066E, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0BH325066E, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_0BH325066E, FrontBack.BACK);
            }
            else if (sItemCode == "0GC325066A" || sItemCode == "0GC325066B")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0GC325066A, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_0GC325066A, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0GC325066A, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_0GC325066A, FrontBack.BACK);
            }

            else if (sItemCode == "0BH325065H")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0BH325065H, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_0BH325065H, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0BH325065H, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_0BH325065H, FrontBack.BACK);
            }
            else if (sItemCode == "0DN325065A")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DN325065A, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DN325065A, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DN325065A, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DN325065A, FrontBack.BACK);
            }
            else if (sItemCode == "0DN325066")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DN325066, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DN325066, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DN325066, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DN325066, FrontBack.BACK);
            }
            else if (sItemCode == "22141-2U000")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_22141_2U000, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_22141_2U000, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_22141_2U000, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_22141_2U000, FrontBack.BACK);
            }
            else if (sItemCode == "0DD325063E")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DD325063E, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DD325063E, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DD325063E, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DD325063E, FrontBack.BACK);
            }
            else if (sItemCode == "0DD325064C")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DD325064C, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DD325064C, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DD325064C, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DD325064C, FrontBack.BACK);
            }
            else if (sItemCode == "0DD325473C")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DD325473C, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DD325473C, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DD325473C, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DD325473C, FrontBack.BACK);
            }
            else if (sItemCode == "0DD325474C")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DD325474C, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DD325474C, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DD325474C, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DD325474C, FrontBack.BACK);
            }
            else if (sItemCode == "0DD325064D")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DD325064D, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DD325064D, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_0DD325064D, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_0DD325064D, FrontBack.BACK);
            }
            else if (sItemCode == "MEK65172301-50" || sItemCode == "MEK65172301")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK65172301, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK65172301, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK65172301, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK65172301, FrontBack.BACK);
            }
            else if (sItemCode == "MEK63867201-50" || sItemCode == "AEN74391701")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK63867201, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK63867201, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK63867201, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK63867201, FrontBack.BACK);
            }

            else if (sItemCode == "MEK63946601-50" || sItemCode == "MEK63946601")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK63946601, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK63946601, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK63946601, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK63946601, FrontBack.BACK);
            }

            else if (sItemCode == "MEK65166901-50" || sItemCode == "AEN75452201")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK65166901, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK65166901, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK65166901, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK65166901, FrontBack.BACK);
            }

            else if (sItemCode == "MEK64466501-50" || sItemCode == "AEN75271601")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK64466501, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK64466501, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK64466501, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK64466501, FrontBack.BACK);
            }

            else if (sItemCode == "MEK64486501-50" || sItemCode == "AEN74852201")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK64486501, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK64486501, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK64486501, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK64486501, FrontBack.BACK);
            }

            else if (sItemCode == "AEN74412101-50" || sItemCode == "AEN74412101")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_AEN74412101, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_AEN74412101, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_AEN74412101, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_AEN74412101, FrontBack.BACK);
            }

            else if (sItemCode == "MEK63887201-50" || sItemCode == "AEN74412301")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK63887201, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK63887201, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK63887201, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK63887201, FrontBack.BACK);
            }

            else if (sItemCode == "MEK63887101-50" || sItemCode == "AEN75452601")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK63887101, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK63887101, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK63887101, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK63887101, FrontBack.BACK);
            }

            else if (sItemCode == "MEK64266601-50" || sItemCode == "AEN74731701")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK64266601, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK64266601, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_MEK64266601, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_MEK64266601, FrontBack.BACK);
            }

            else if (sItemCode == "21111-07000")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_21111_07000, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_21111_07000, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_21111_07000, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_21111_07000, FrontBack.BACK);
            }

            else if (sItemCode == "21111-07500")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_21111_07500, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_21111_07500, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_21111_07500, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_21111_07500, FrontBack.BACK);
            }

            else if (sItemCode == "21111-08000")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_21111_08000, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_21111_08000, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_21111_08000, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_21111_08000, FrontBack.BACK);
            }

            else if (sItemCode == "GS22701")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_GS22701_2, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_GS22701_2, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_GS22701_2, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_GS22701_2, FrontBack.BACK);
            }

            else if (sItemCode == "GS22701.5")
            {
                this.layoutControlFront.Bind(global::SAMMI.QM.Properties.Resources.FRONT_GS22701_5, FrontBack.FRONT);
                this.layoutControlBack.Bind(global::SAMMI.QM.Properties.Resources.BACK_GS22701_5, FrontBack.BACK);
                this.layoutControlFrontBack.Bind(global::SAMMI.QM.Properties.Resources.FRONT_GS22701_5, FrontBack.FRONT);
                this.layoutControlFrontBack1.Bind(global::SAMMI.QM.Properties.Resources.BACK_GS22701_5, FrontBack.BACK);
            }    


            this.layoutControlFront.faultCodeNodesList.Clear();
            this.layoutControlBack.faultCodeNodesList.Clear();
            this.layoutControlFrontBack.faultCodeNodesList.Clear();
            this.layoutControlFrontBack1.faultCodeNodesList.Clear();
            this.layoutControlFront.DrawRectangle();
            this.layoutControlBack.DrawRectangle();
            this.layoutControlFrontBack.DrawRectangle();
            this.layoutControlFrontBack1.DrawRectangle();
        }
        
        /// <summary>
        /// Get fault code
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private string GetFaultCode(string location)
        {
            string type = grid3.ActiveRow.Cells[0].Value.ToString();
            string faultName = grid3.ActiveRow.Cells[1].Value.ToString();
            string faultCode = string.Empty;

            if (!string.IsNullOrEmpty(faultName))
            {
                if (faultName == "미성형")
                {
                    faultCode = "101";
                }
                else if (faultName == "기포")
                {
                    faultCode = "102";
                }
                else if (faultName == "크랙")
                {
                    faultCode = "103";
                }
                else if (faultName == "소착")
                {
                    faultCode = "104";
                }
                else if (faultName == "살떨어짐")
                {
                    faultCode = "105";
                }
                else if (faultName == "박리")
                {
                    faultCode = "106";
                }
                else if (faultName == "변색")
                {
                    faultCode = "109";
                }
                else if (faultName == "흑피")
                {
                    faultCode = "110";
                }
                else if (faultName == "리크")
                {
                    faultCode = "111";
                }
                else if (faultName == "사상")
                {
                    faultCode = "113";
                }
                else if (faultName == "부풀음")
                {
                    faultCode = "117";
                }
                else if (faultName == "찍힘")
                {
                    if (type == "소재")
                    {
                        faultCode = "121";
                    }
                    else
                    {
                        faultCode = "206";
                    }
                }
                else if (faultName == "소재(기타)")
                {
                    faultCode = "123";
                }
                else if (faultName == "이물질")
                {
                    faultCode = "124";
                }
                else if (faultName == "단차떨림")
                {
                    faultCode = "201";
                }
                else if (faultName == "치수")
                {
                    faultCode = "205";
                }
                else if (faultName == "공구파손")
                {
                    faultCode = "207";
                }
                else if (faultName == "변형")
                {
                    faultCode = "208";
                }
                else if (faultName == "칩눌림")
                {
                    faultCode = "209";
                }
                else if (faultName == "세팅Trail")
                {
                    faultCode = "210";
                }
                else if (faultName == "스크레치")
                {
                    faultCode = "211";
                }
                else if (faultName == "인선")
                {
                    faultCode = "215";
                }
                else if (faultName == "백화(발청)")
                {
                    faultCode = "216";
                }
                else if (faultName == "가공(기타)")
                {
                    faultCode = "217";
                }
                else if (faultName == "확공")
                {
                    faultCode = "219";
                }
                else if (faultName == "흑피")
                {
                    faultCode = "222";
                }
                else if (faultName == "홀박리 재작업")
                {
                    faultCode = "992";
                }
                else if (faultName == "홀가공버 재작업")
                {
                    faultCode = "993";
                }
                else if (faultName == "홀박리 재작업")
                {
                    faultCode = "994";
                }
                else if (faultName == "사상 재작업")
                {
                    faultCode = "995";
                }
                else if (faultName == "결육 재작업")
                {
                    faultCode = "996";
                }
                else if (faultName == "버 재작업")
                {
                    faultCode = "997";
                }
                else if (faultName == "찍힘 재작업")
                {
                    faultCode = "998";
                }
                else if (faultName == "스크래치 재작업")
                {
                    faultCode = "999";
                }
            }

            return faultCode;
        }

        /// <summary>
        /// Set locationInfo
        /// </summary>
        /// <param name="dt"></param>
        private void SetLocationInfo(DataTable dt)
        {
            foreach (DataRow dr in (gridControlLocation.DataSource as DataTable).Rows)
            {
                dr["FLAG"] = "N";
            }

            (gridControlLocation.DataSource as DataTable).AcceptChanges();

            if (dt != null && dt.Rows.Count > 0)
            {
                System.Data.DataView dataView = gridViewLocation.DataSource as System.Data.DataView;

                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataRow dr1 in (gridControlLocation.DataSource as DataTable).Rows)
                    {
                        if (dr["CAST_LOTNO"].ToString() == dr1["CAST_LOTNO"].ToString() && dr["LOTNO"].ToString() == dr1["LOTNO"].ToString())
                        {
                            dr1["FLAG"] = "Y";
                        }
                    }
                }

                (gridControlLocation.DataSource as DataTable).AcceptChanges();
            }
        }

        /// <summary>
        /// Get location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="frontBack"></param>
        /// <param name="faultName"></param>
        /// <returns></returns>
        private DataTable GetLocation(string location, string frontBack, string faultName)
        {
            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            string sWorkCenterCode = txtWorkCenterCode.Text;
            string sSubWorkCenterCode = cmbSubWorkCenterCode.Text;

            if (sSubWorkCenterCode == "ALL")
            {
                sSubWorkCenterCode = "%";
            }
            else
            {
                if (!string.IsNullOrEmpty(sWorkCenterCode))
                {
                    sSubWorkCenterCode = sWorkCenterCode + sSubWorkCenterCode;
                }
                else
                {
                    sSubWorkCenterCode = "%";
                }
            }

            DataTable dt = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[12];

            sqlParameters[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[1] = sqlDBHelper.CreateParameter("@AS_WORKCENTERCODE", txtWorkCenterCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[2] = sqlDBHelper.CreateParameter("@AS_FROM_DT", string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[3] = sqlDBHelper.CreateParameter("@AS_TO_DT", string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[4] = sqlDBHelper.CreateParameter("@AS_CAST_LOTNO", txtCastLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[5] = sqlDBHelper.CreateParameter("@AS_LOTNO", txtLotNo.Text, SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[6] = sqlDBHelper.CreateParameter("@AS_ITEMCODE", txtItemCode.Text, SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[7] = sqlDBHelper.CreateParameter("@AS_MOLD", txtMold.Text, SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[8] = sqlDBHelper.CreateParameter("@AS_SUBWORKCENTERCODE", sSubWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[9] = sqlDBHelper.CreateParameter("@AS_LOCATION", location, SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[10] = sqlDBHelper.CreateParameter("@AS_FRONTBACK", frontBack, SqlDbType.VarChar, ParameterDirection.Input);
            sqlParameters[11] = sqlDBHelper.CreateParameter("@AS_FAULTNAME", faultName, SqlDbType.VarChar, ParameterDirection.Input);

            dt = sqlDBHelper.FillTable("SP_GET_FAULT_LOCATION_LIST", CommandType.StoredProcedure, sqlParameters);

            return dt;
        }

        #endregion
    }
}
