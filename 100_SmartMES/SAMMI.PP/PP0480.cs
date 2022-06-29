#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID        : PP0480
//   Form Name      : 가동율현황
//   Name Space     : SAMMI.PP
//   Created Date   : 2022.02.21
//   Made By        : 정용석
//   Description    : 작업일보 활용#4 (가동율현황)
// *---------------------------------------------------------------------------------------------*
#endregion

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

using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.Data;
using DevExpress.Export;
using DevExpress.XtraPrinting;

namespace SAMMI.PP
{
    public partial class PP0480 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _dt = new DataTable();

        DataSet   _ds = new DataSet();


        BizTextBoxManagerEX btbManager = new BizTextBoxManagerEX();
        /// <summary>
        /// Change grid1 datatable 
        /// </summary>

        private DataTable _ChangeDt;
        private DataTable _TotalDt;

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();
        
        /// <summary>
        /// PlantCode
        /// </summary>
        private string _PlantCode = string.Empty;

        #endregion

        public PP0480()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();
            AttachEventHandlers();
        }        
        #region Event

        /// <summary>
        /// PP0370 disposed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP0480_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();
        }

        /// <summary>
        /// Excel Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            ExportToExcel();
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
            btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "2000", "", "" }, new string[] { }, new object[] { });

            deSdate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {

        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.Disposed += new EventHandler(PP0480_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(PP0480_Disposed);
        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            if (_dt != null)
            {
                _dt.Clear();                
            }
            if (_ChangeDt != null)
            {
                _ChangeDt.Clear();
            }
            if (_TotalDt != null)
            {
                _TotalDt.Clear();                
            }

            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[3];
            ClearAllControl();

            try
            {
                base.DoInquire();

                string sdate = string.Format("{0:yyyy-MM-dd}", deSdate.Value);
                string edate = string.Format("{0:yyyy-MM-dd}", deEdate.Value);

                param[0] = helper.CreateParameter("@AS_PLANTCODE",      "SK2", SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@AS_SDATE",          sdate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@AS_EDATE",          edate, SqlDbType.VarChar, ParameterDirection.Input);

                _dt = helper.FillTable("USP_PP0480_S1", CommandType.StoredProcedure, param);

                GridColumnInitialize(_dt);

                gridControl1.DataSource = _dt;
                gridControl2.DataSource = _TotalDt;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }

        private void GridColumnInitialize(DataTable dt)
        {
            gridView1.Columns.Clear();
            gridView2.Columns.Clear();

            _ChangeDt = dt.Clone();
            _TotalDt  = dt.Clone();

            if (dt != null)
            {

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    gridView1.Columns.AddField(dt.Columns[i].ColumnName);
                    gridView2.Columns.AddField(dt.Columns[i].ColumnName);

                    gridView1.Columns[i].Caption = dt.Columns[i].ColumnName == "workcentercode"  ? "작업장"   :
                                                   dt.Columns[i].ColumnName == "itemcode" ? "품목명" :
                                                   dt.Columns[i].ColumnName == "ct" ? "C/T" :
                                                   dt.Columns[i].ColumnName == "subsum" ? "합계" :
                                                   dt.Columns[i].ColumnName == "div" ? "구분" : dt.Columns[i].ColumnName;

                    gridView2.Columns[i].Caption = dt.Columns[i].ColumnName == "workcentercode" ? "작업장" :
                                                   dt.Columns[i].ColumnName == "itemcode" ? "품목명" :
                                                   dt.Columns[i].ColumnName == "ct" ? "C/T" :
                                                   dt.Columns[i].ColumnName == "subsum" ? "합계" :
                                                   dt.Columns[i].ColumnName == "div" ? "구분" : dt.Columns[i].ColumnName;

                    gridView1.Columns[i].Width = 60;
                    gridView1.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridView1.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gridView1.Columns[i].AppearanceHeader.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


                    gridView2.Columns[i].Width = 60;
                    gridView2.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridView2.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gridView2.Columns[i].AppearanceHeader.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


                    if (dt.Columns[i].ColumnName == "workcentercode")
                    {
                        gridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                        gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridView1.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridView1.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                        gridView1.Columns[i].AppearanceCell.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        gridView2.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                        gridView2.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridView2.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridView2.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                        gridView2.Columns[i].AppearanceCell.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    }
                    else
                    {
                        gridView1.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                        gridView2.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                    }

                    if (dt.Columns[i].ColumnName == "itemcode")
                    {
                        gridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                        gridView1.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                        gridView1.Columns[i].Width = 360;
                        gridView1.Columns[i].AppearanceCell.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        gridView2.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                        gridView2.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridView2.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridView2.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                        gridView2.Columns[i].Width = 360;
                        gridView2.Columns[i].AppearanceCell.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    }

                    if (dt.Columns[i].ColumnName == "div")
                    {
                        //gridView1.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                        gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridView1.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridView1.Columns[i].AppearanceCell.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        gridView2.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridView2.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridView2.Columns[i].AppearanceCell.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    }
                    else
                    {
                        gridView1.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        gridView1.Columns[i].DisplayFormat.FormatString = "#,0";

                        gridView2.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        gridView2.Columns[i].DisplayFormat.FormatString = "#,0";
                    }
                    //if ( !(dt.Columns[i].ColumnName == "workcentercode" || dt.Columns[i].ColumnName == "itemcode" || dt.Columns[i].ColumnName == "div" ))
                    //{
                    //    gridView1.Columns[i].Visible = true;
                    //}                    
                    gridView1.Columns[i].Visible = true;
                    gridView2.Columns[i].Visible = true;
                }

                int[] goalQty = new int[dt.Columns.Count - 4];
                int[] prodQty = new int[dt.Columns.Count - 4];
                int[] runRate = new int[dt.Columns.Count - 4]; 

                for (int i = 0; i < dt.Rows.Count; i++)
                {                   
                    for (int j = 4; j < dt.Columns.Count; j++)
                    {                        
                        if (dt.Rows[i]["div"].ToString() == "목표수량")
                        {
                            goalQty[j-4] = dt.Rows[i][j] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i][j]);
                        }
                        if (dt.Rows[i]["div"].ToString() == "생산수량")
                        {
                            prodQty[j-4] = dt.Rows[i][j] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i][j]);

                            runRate[j-4] = goalQty[j-4] == 0 ? 0 : prodQty[j-4] * 100 / goalQty[j-4];
                        }
                    }
                    if (dt.Rows[i]["div"].ToString() != "목표수량")
                    {
                        DataRow dr = _ChangeDt.NewRow();

                        dr["workcentercode"]   = dt.Rows[i]["workcentercode"].ToString();
                        dr["itemcode"]         = dt.Rows[i]["itemcode"].ToString();
                        dr["div"]              = "가동율";
                        //dr["ct"]             = dt.Rows[i]["ct"].ToString();

                        for (int k = 0; k < dt.Columns.Count - 4; k++)
                        {
                            dr[k+4] = runRate[k];
                        }
                        _ChangeDt.Rows.Add(dr);                    
                    }
                }
                foreach (DataRow temp in _ChangeDt.Rows)
                {
                    dt.ImportRow(temp);
                }
                dt.DefaultView.Sort = "workcentercode ASC, itemcode ASC";

                int[] totalgoalQty = new int[dt.Columns.Count - 4];
                int[] totalprodQty = new int[dt.Columns.Count - 4];
                int[] totalrunRate = new int[dt.Columns.Count - 4];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["div"].ToString() == "목표수량")
                    {
                        for (int j = 4; j < dt.Columns.Count; j++)
                        {                            
                            totalgoalQty[j-4] += dt.Rows[i][j] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i][j]);
                        }
                    }
                    if (dt.Rows[i]["div"].ToString() == "생산수량")
                    {
                        for (int j = 4; j < dt.Columns.Count; j++)
                        {
                            if (dt.Rows[i]["ct"] != DBNull.Value)
                            {
                                totalprodQty[j - 4] += dt.Rows[i][j] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i][j]);
                                totalrunRate[j - 4] = totalgoalQty[j - 4] == 0 ? 0 : totalprodQty[j - 4] * 100 / totalgoalQty[j - 4];
                            }
                        }
                    }
                }
                DataRow drG = _TotalDt.NewRow();
                drG["workcentercode"] = "종합";
                drG["itemcode"]       = "종합";
                drG["div"]            = "목표수량";                

                for (int i = 0; i < _TotalDt.Columns.Count - 4; i++)
                {
                    drG[i + 4] = totalgoalQty[i];
                }
                _TotalDt.Rows.Add(drG);

                DataRow drP = _TotalDt.NewRow();
                drP["workcentercode"] = "종합";
                drP["itemcode"]       = "종합";
                drP["div"]            = "생산수량";

                for (int i = 0; i < _TotalDt.Columns.Count - 4; i++)
                {
                    drP[i + 4] = totalprodQty[i];
                }
                _TotalDt.Rows.Add(drP);

                DataRow drR = _TotalDt.NewRow();
                drR["workcentercode"] = "종합";
                drR["itemcode"]       = "종합";
                drR["div"]            = "가동율";

                for (int i = 0; i < _TotalDt.Columns.Count - 4; i++)
                {
                    drR[i + 4] = totalrunRate[i];
                }
                _TotalDt.Rows.Add(drR);   
            }
        }
        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {            
            GridView gView = sender as GridView;

            if (gView.GetRowCellValue(e.RowHandle, "div").ToString() == "가동율")
            {
                int runRate = 0;
                try
                {
                    runRate = e.CellValue == DBNull.Value ? 0 : Convert.ToInt32(e.CellValue);
                }
                catch
                {
                    runRate = 0;
                }

                if (runRate >= 100)
                {
                    e.Appearance.BackColor = Color.PowderBlue; // Color.FromArgb(255, 192, 192);
                }  
            }          
        }
        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView gView = sender as GridView;

            if (gView.GetRowCellValue(e.RowHandle, "div").ToString() == "가동율")
            {
                int runRate = 0;
                try
                {
                    runRate = e.CellValue == DBNull.Value ? 0 : Convert.ToInt32(e.CellValue);
                }
                catch
                {
                    runRate = 0;
                }

                if (runRate >= 100)
                {
                    e.Appearance.BackColor = Color.PowderBlue; // Color.FromArgb(255, 192, 192);
                }
            }
            //if (gView.GetRowCellValue(e.RowHandle, "workcentercode").ToString() == "종합" || gView.GetRowCellValue(e.RowHandle, "itemcode").ToString() == "종합")
            //{
            //    e.Appearance.BackColor = Color.LightYellow; 
            //}
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView gView = sender as GridView;

            if (e.RowHandle > 0)
            {
                if (gView.GetRowCellValue(e.RowHandle, "div").ToString() == "가동율")
                {
                    e.Appearance.BackColor = Color.FromArgb(192, 255, 192);
                }
            }
        }
        private void gridView2_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView gView = sender as GridView;

            if (e.RowHandle > 0)
            {
                if (gView.GetRowCellValue(e.RowHandle, "div").ToString() == "가동율")
                {
                    e.Appearance.BackColor = Color.FromArgb(192, 255, 192);
                }
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

                    //foreach (string s in _EmptyArrs)
                    //{
                    //    if (textBox.Name.StartsWith(s))
                    //    {
                    //        textBox.Text = string.Empty;
                    //    }
                    //}
                }

                if (ctrl.GetType().Name == "MaskedTextBox")
                {
                    MaskedTextBox maskedTextBox = (MaskedTextBox)ctrl;

                    //foreach (string sVal in _EmptyArrs)
                    //{
                    //    if (maskedTextBox.Name.StartsWith(sVal))
                    //    {
                    //        maskedTextBox.Text = string.Empty;
                    //    }
                    //}
                }
            }
            return;
        }

        /// <summary>
        /// Export To Excel
        /// </summary>
        private void ExportToExcel()
        {
            XlsxExportOptionsEx xlsxOptions = new XlsxExportOptionsEx();
            xlsxOptions.ExportType = DevExpress.Export.ExportType.WYSIWYG;
            string path = "가동율현황_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            gridControl1.ExportToXlsx(path);
            Process.Start(path);


        }

        #endregion

    }
}
