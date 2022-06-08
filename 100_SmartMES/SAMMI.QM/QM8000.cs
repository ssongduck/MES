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
using Infragistics.UltraChart.Resources.Appearance;
#endregion

namespace SAMMI.QM
{
    public partial class QM8000 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp2 = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp3 = new DataTable(); // return DataTable 공통

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        // private DataTable DtChange = null;

        DataTable DtChange = new DataTable();
        DataTable DtChange1 = new DataTable();
        DataTable DtChange2 = new DataTable();
        DataTable DtPieChart = new DataTable();
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private ChartLayerAppearance lineLayer = new ChartLayerAppearance();
        private ChartLayerAppearance lineLayer2 = new ChartLayerAppearance();

        private string PlantCode = string.Empty;
        #endregion

        public QM8000()
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

            GridInit();

            btbManager = new BizTextBoxManagerEX();
            btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, "4", "", "" }
                     , new string[] { }, new object[] { });

            this.Pie_Chart_SUM.EmptyChartText = string.Empty;
            this.Pie_Chart_Line.EmptyChartText = string.Empty;
        }

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];
            SqlParameter[] param2 = new SqlParameter[5];
            try
            {
                base.DoInquire();

                //DateTime frRegDT = Convert.ToDateTime(((DateTime)this.calRegDT_FRH.Value).ToString("yyyy-MM-dd") + " 00:00:00.00");

                string frRegDT = SqlDBHelper.nvlDateTime(calRegDT_FRH.Value).ToString("yyyy-MM-dd");

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sWorkCenterCode = SqlDBHelper.nvlString(txtWorkCenterCode.Text);
                string sOpCode = SqlDBHelper.nvlString(cboOpCode_H.Value);
                
                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[1] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[2] = helper.CreateParameter("OpCode", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[3] = helper.CreateParameter("FrDT", frRegDT, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param[4] = helper.CreateParameter("ToDT", frRegDT, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                rtnDtTemp = helper.FillTable("USP_QM2100_S1_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();
                DtChange = rtnDtTemp;

                param2[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param2[1] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param2[2] = helper.CreateParameter("OpCode", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param2[3] = helper.CreateParameter("FrDT", frRegDT, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param2[4] = helper.CreateParameter("ToDT", frRegDT, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                rtnDtTemp2 = helper.FillTable("USP_QM2100_S3_UNION", CommandType.StoredProcedure, param2);

                //this.Pie_Chart_SUM.DataSource = rtnDtTemp2;

                if (rtnDtTemp2.Rows.Count > 0)
                {
                    
                    int A_Value = 0; int B_Value = 0; int C_Value = 0; int D_Value = 0;
                    for (int i = 0; i < rtnDtTemp2.Rows.Count; i++ )
                    {   if (rtnDtTemp2.Rows[i]["CP_Grade"].Equals("A"))
                        {
                            A_Value += Convert.ToInt32(rtnDtTemp2.Rows[i]["CP"]);
                        }
                        else if(rtnDtTemp2.Rows[i]["CP_Grade"].Equals("B"))
                        {
                            B_Value += Convert.ToInt32(rtnDtTemp2.Rows[i]["CP"]);
                        }
                        else if (rtnDtTemp2.Rows[i]["CP_Grade"].Equals("C"))
                        {
                            C_Value += Convert.ToInt32(rtnDtTemp2.Rows[i]["CP"]);
                        }
                        else if (rtnDtTemp2.Rows[i]["CP_Grade"].Equals("D"))
                        {
                            D_Value += Convert.ToInt32(rtnDtTemp2.Rows[i]["CP"]);
                        }
                    }

                    //MessageBox.Show(A_Value + " " + B_Value + " " + C_Value + " " + D_Value);

                    DtPieChart.Clear();
                    DtPieChart.Columns.Clear();
                    DtPieChart.Columns.Add(new DataColumn("CP_Grade", typeof(string)));
                    DtPieChart.Columns.Add(new DataColumn("CP", typeof(decimal)));


                    //DataRow newrow = this.DtTable.NewRow();
                    //newrow["검사코드"] = rtnDtTemp.Rows[i]["InspCode"].ToString();
                    //newrow["검사항목"] = rtnDtTemp.Rows[i]["InspName"].ToString();
                    //newrow["검사구분"] = "CP";
                    //this.DtTable.Rows.Add(newrow);

                    //DataRow newrow1 = this.DtTable.NewRow();
                    //newrow1["검사코드"] = rtnDtTemp.Rows[i]["InspCode"].ToString();
                    //newrow1["검사항목"] = rtnDtTemp.Rows[i]["InspName"].ToString();
                    //newrow1["검사구분"] = "CPK";
                    //this.DtTable.Rows.Add(newrow1);

                    DataRow newrowA = this.DtPieChart.NewRow();
                    newrowA["CP_Grade"] = "A";
                    newrowA["CP"] = Convert.ToDecimal(A_Value);
                    this.DtPieChart.Rows.Add(newrowA);
                    //MessageBox.Show("A성공");
                    
                    DataRow newrowB = this.DtPieChart.NewRow();
                    newrowB["CP_Grade"] = "B";
                    newrowB["CP"] = Convert.ToDecimal(B_Value);
                    this.DtPieChart.Rows.Add(newrowB);
                    //MessageBox.Show("B성공");

                    DataRow newrowC = this.DtPieChart.NewRow();
                    newrowC["CP_Grade"] = "C";
                    newrowC["CP"] = Convert.ToDecimal(C_Value);
                    this.DtPieChart.Rows.Add(newrowC);
                    //MessageBox.Show("C성공");

                    DataRow newrowD = this.DtPieChart.NewRow();
                    newrowD["CP_Grade"] = "D";
                    newrowD["CP"] = Convert.ToDecimal(D_Value);
                    this.DtPieChart.Rows.Add(newrowD);
                    this.Pie_Chart_SUM.DataSource = DtPieChart;
                    //MessageBox.Show("D성공");

                    //rtnDtTemp2.Clear();
                    //rtnDtTemp2 = DtPieChart.Clone();
                    this.Pie_Chart_SUM.DataSource = DtPieChart;

                    Pie_Chart_SUM.Series.Clear();
                   
                    NumericSeries series = new NumericSeries();
                    series.Data.DataSource = DtPieChart;
                    //series.Data.DataSource = DtPieChart;
                    series.Data.LabelColumn = "CP_Grade";
                    series.Data.ValueColumn = "CP";
                    series.DataBind();


                    Pie_Chart_SUM.Series.Add(series);
                    Pie_Chart_SUM.Data.DataBind();

                    
                    //DtChange1 = rtnDtTemp2;
                    DtChange1 = DtPieChart;
                }
                else
                {
                    ShowDialog("조회할 항목이 없습니다.", Windows.Forms.DialogForm.DialogType.OK);
                }


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
                if (param2 != null) { param2 = null; }
            }
        }

        #endregion

        #region [그리드 셋팅]
        private void GridInit()
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkDT", "일자", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SampleQTY", "측정항목수", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SPECIN", "SPEC IN", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SPECOUT", "SPEC OUT", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "A_GRADE", "A등급(CP>1.67)", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "B_GRADE", "B등급(CP>1.33)", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "C_GRADE", "C등급(CP>1.0)", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "D_GRADE", "D등급", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CrtCNT", "이상발생수", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ActCNT", "조치수", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Right, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "NonActCNT", "미조치수", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            
            grid1.Columns[6].CellAppearance.BackColor = Color.LightGreen;
            grid1.Columns[7].CellAppearance.BackColor = Color.LightSkyBlue;
            grid1.Columns[8].CellAppearance.BackColor = Color.Yellow;
            grid1.Columns[9].CellAppearance.BackColor = Color.LightSalmon;


            DataTable rtnGridCombo = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnGridCombo, "CODE_ID", "CODE_NAME");

            #endregion
        }
        #endregion

        private void grid1_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param3 = new SqlParameter[4];

            try
            {

                string frRegDT = SqlDBHelper.nvlDateTime(calRegDT_FRH.Value).ToString("yyyy-MM-dd");
                //string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sPlantCode = SqlDBHelper.nvlString(this.grid1.ActiveRow.Cells["PlantCode"].Value);
                string sWorkCenterCode = this.grid1.ActiveRow.Cells["WorkCenterCode"].Value.ToString();
                string CP_GRADE = string.Empty;

                param3[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param3[1] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param3[2] = helper.CreateParameter("FrDT", frRegDT, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param3[3] = helper.CreateParameter("ToDT", frRegDT, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                rtnDtTemp3 = helper.FillTable("USP_QM2100_S2_UNION", CommandType.StoredProcedure, param3);

                this.Pie_Chart_Line.DataSource = rtnDtTemp3;

                if (rtnDtTemp3.Rows.Count > 0)
                {
                    Pie_Chart_Line.Series.Clear();

                    NumericSeries series = new NumericSeries();
                    series.Data.DataSource = rtnDtTemp3;
                    series.Data.LabelColumn = "CP_Grade";
                    series.Data.ValueColumn = "CP";
                    series.DataBind();

                    Pie_Chart_Line.Series.Add(series);
                    Pie_Chart_Line.Data.DataBind();


                    DtChange2 = rtnDtTemp3;

                }

                switch (e.Cell.Column.Key)
                {
                    case "A_GRADE": CP_GRADE = "A"; break;
                    case "B_GRADE": CP_GRADE = "B"; break;
                    case "C_GRADE": CP_GRADE = "C"; break;
                    case "D_GRADE": CP_GRADE = "D"; break;
                    default: return;
                }

                if (grid1.ActiveRow.Cells[e.Cell.Column.Key].Value.ToString() == "0")
                {
                    MessageBox.Show(CP_GRADE + " 등급의 내역이 없습니다.");
                    return;
                }
                if (CP_GRADE != "")
                {
                    string sWorkCenterName = grid1.ActiveRow.Cells["WorkCenterName"].Value.ToString();
                    QM8020 qm8020 = new QM8020(sPlantCode, frRegDT, sWorkCenterCode, "", CP_GRADE, sWorkCenterName, "");
                    qm8020.ShowDialog();
                }

                //string sWorkCenterName = grid1.ActiveRow.Cells["WorkCenterName"].Value.ToString();
                //QM8010 qm8010 = new QM8010(sPlantCode, sWorkCenterCode, sWorkCenterName, frRegDT);
                //qm8010.ShowDialog();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param3 != null) { param3 = null; }
            }

        }

        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }
    }
}
