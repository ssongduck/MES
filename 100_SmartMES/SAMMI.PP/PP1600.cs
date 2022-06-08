using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SAMMI.PopUp;
using SAMMI.Common;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.Win.UltraWinGrid;

namespace SAMMI.PP
{
    public partial class PP1600 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable rtnDtChart = new DataTable(); // return DataTable 공통
         
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        BizGridManagerEX BIZPOP;
        Common.Common _Common = new Common.Common();
        PopUp_Biz _biz = new PopUp_Biz();
        private string PlantCode = string.Empty;

        public PP1600()
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
        }

        #region PP1600_Load
        private void PP1600_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 180, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STATUSTIME", "비가동시간(분)", false, GridColDataType_emu.Integer, 120, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
         
            
            _GridUtil.SetInitUltraGridBind(grid1);
            //grid1.Columns["OPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPName"].MergedCellStyle = MergedCellStyle.Always;

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            #endregion
            this.ultraChart1.EmptyChartText = string.Empty;
            this.ultraChart2.EmptyChartText = string.Empty;

        }
        #endregion PP1600_Load

        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);          //사업장코드
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);
                string sOPCode = txtOPCode.Text.Trim();
                string stxtWorkCenterCode = txtWorkCenterCode.Text.Trim();

                param[0] = helper.CreateParameter("@STARTDATE", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@ENDDATE", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@OPCODE", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@LINECODE", stxtWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                
                rtnDtTemp = helper.FillTable("USP_PP1600_S1_UNION", CommandType.StoredProcedure, param);

                DataView dvChart = rtnDtTemp.DefaultView;
                dvChart.Sort = "STATUSTIME DESC";
                rtnDtTemp = dvChart.ToTable();
                
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                rtnDtChart.Clear();
                rtnDtChart = rtnDtTemp.Clone();
                
                
                if (rtnDtTemp.Rows.Count > 0)
                {
                    ultraChart1.Series.Clear();
                    ultraChart2.Series.Clear();
                    ultraChart1.Visible = true;

                    int cnt = 0;
                    foreach (DataRow dr in rtnDtTemp.Rows)
                    {
                        if (cnt < 10)
                        {

                            rtnDtChart.Rows.Add(dr.ItemArray);

                        }
                        cnt++;
                    }

                    double sum = 0.0;
                    for (int i = 10; i < rtnDtTemp.Rows.Count; i++)
                    {
                        if (i >= 10)
                        {
                            sum += Convert.ToDouble(rtnDtTemp.Rows[i]["STATUSTIME"]);

                        }
                    }

                    DataRow drOrders = rtnDtChart.NewRow();

                    drOrders["PlantCode"] = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                    drOrders["OPName"] = "Orders";
                    drOrders["WORKCENTERCODE"] = "기타";
                    drOrders["WORKCENTERNAME"] = "기타";
                    drOrders["STATUSTIME"] = sum.ToString();
                    rtnDtChart.Rows.Add(drOrders);

                    NumericSeries series = new NumericSeries();
                    series.Data.DataSource = rtnDtChart;
                    series.Data.LabelColumn = "WORKCENTERNAME";
                    series.Data.ValueColumn = "STATUSTIME";
                    ultraChart1.Axis.X.TimeAxisStyle.TimeAxisStyle = Infragistics.UltraChart.Shared.Styles.RulerGenre.Continuous;
                    ultraChart1.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
                    series.DataBind();

                    ultraChart1.Series.Add(series);
                    ultraChart1.Data.DataBind();

                    ultraChart2.Series.Add(series);
                    ultraChart2.Data.DataBind();

                    ultraChart1.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
                    ultraChart1.Tooltips.Font = new Font("맑은 고딕", 12);

                    ultraChart2.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
                    ultraChart2.Tooltips.Font = new Font("맑은 고딕", 12);
                }
                else
                {
                    ultraChart1.Visible = false;
                }
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

        #region 텍스트 박스에서 팝업창에서 값 가져오기 (작업장)
        //////////////////     
        private void Search_Pop_TBM0400()
        {

            string sPlantCode =  SqlDBHelper.nvlString(cboPlantCode_H.Value);          //사업장코드
            string sOPCode = txtOPCode.Text.Trim();       //공정코드
            string sOPName = txtOPName.Text.Trim();      //공정명 
            string sUseFlag =  "Y";            //사용여부         

            try
            {
                _biz.TBM0400_POP(sPlantCode, sOPCode, sOPName, sUseFlag, txtOPCode, txtOPName);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }
       
        private void txtOPCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPName.Text = string.Empty;
        }

         private void txtOPNAME_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPCode.Text = string.Empty;
        }

        private void txtOPCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0400();
            }
        }

        private void txtOPCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0400();
        }

   

        private void txtOPName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0400();
            }
        }

        private void txtOPName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0400();
        }

         #endregion        //공정(작업장)

        #region 텍스트 박스에서 팝업창에서 값 가져오기 (라인)
         private void Search_Pop_TBM0600()
        {

            string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);                            //사업장코드
            string sOPCode = txtOPCode.Text.Trim();                       //공정코드
            string sOPName = txtOPName.Text.Trim();                       //공정명 
            string sLineCode = string.Empty;                              //라인코드
            string sWorkcenterCode = txtWorkCenterCode.Text.Trim();       //작업호기(라인)코드
            string sWorkCenterName = txtWorkCenterName.Text.Trim();       //작업호기(라인)명 
            string sUseFlag = "Y";                               //사용여부         

            try
            {
                _biz.TBM0600_POP(sPlantCode, sWorkcenterCode, sWorkCenterName, sOPCode, sLineCode, sUseFlag, txtWorkCenterCode, txtWorkCenterName);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }

         private void txtWorkCenterCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0600();
        }

        private void txtWorkCenterName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0600();
        }

        private void txtWorkCenterCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkCenterName.Text = string.Empty;
        }

        private void txtWorkCenterCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                Search_Pop_TBM0600();
            }
        }

        private void txtWorkCenterName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkCenterCode.Text = string.Empty;
        }

        private void txtWorkCenterName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                Search_Pop_TBM0600();
            }
        }
         #endregion        //라인

        private void ultraChart2_DataItemOver(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            if (ultraChart2.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                ultraChart2.Tooltips.FormatString = e.ColumnLabel + " : <DATA_VALUE:#,#>";
            }
        }

        private void ultraChart1_DataItemOver(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            if (ultraChart1.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                ultraChart1.Tooltips.FormatString = e.ColumnLabel + " : <DATA_VALUE:#,#>";
            }
        }

        
    }
}
