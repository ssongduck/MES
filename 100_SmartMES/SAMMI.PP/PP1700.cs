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

namespace SAMMI.PP
{
    public partial class PP1700 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable rtnDtChart = new DataTable();
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();
        PopUp_Biz _biz = new PopUp_Biz();

        BizTextBoxManagerEX btbManager;
        private string PlantCode = string.Empty;
        public PP1700()
        {
            InitializeComponent();

            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this.PlantCode.Equals("SK"))
                this.PlantCode = "SK1";

            else if (this.PlantCode.Equals("EC"))
                this.PlantCode = "SK2";

            if (!(this.PlantCode.Equals("SK1") || this.PlantCode.Equals("SK2")))
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;

            btbManager = new BizTextBoxManagerEX();

            if (LoginInfo.PlantAuth.Equals(string.Empty))
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
            else
                btbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
        }

        #region PP1700_Load
        private void PP1700_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 180, 100, Infragistics.Win.HAlign.Center, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPCLASS", "비가동유형", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPCLASSNM", "비가동\n\r유형명", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPTYPE", "비가동유형", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPTYPENM", "비가동\n\r구분명", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPCODE", "비가동코드", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STOPDESC", "비가동명", false, GridColDataType_emu.VarChar, 140, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STATUSTIME", "비가동시간(분)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion


            #region 콤보박스 셋팅
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("STOPCLASS");  //비가동 유형
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "STOPCLASS", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("STOPTYPE");  //비가동 타입
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "STOPTYPE", rtnDtTemp, "CODE_ID", "CODE_NAME");
            #endregion 콤보박스 셋팅

            this.ultraChart1.EmptyChartText = string.Empty;
            this.ultraChart2.EmptyChartText = string.Empty;

        }
        #endregion PP1700_Load

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
                string sWorkcenterCode = txtWorkCenterCode.Text.Trim();
                string sOPCode = txtOPCode.Text.Trim();
                

                param[0] = helper.CreateParameter("STARTDATE", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("ENDDATE", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("WORKCENTERCODE", sWorkcenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("OPCODE", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                
                rtnDtTemp = helper.FillTable("USP_PP1700_S1_UNION", CommandType.StoredProcedure, param);

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

                    int cnt = 0;
                    foreach(DataRow dr in rtnDtTemp.Rows)
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
                    drOrders["PLANTCODE"] = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                    drOrders["STOPTYPE"] = "기타";
                    drOrders["STOPTYPENM"] = "기타";
                    drOrders["STOPCODE"] = "기타";
                    drOrders["STOPDESC"] = "기타";
                    drOrders["STATUSTIME"] = sum.ToString();
                    rtnDtChart.Rows.Add(drOrders);

                    NumericSeries series = new NumericSeries();
                    series.Data.DataSource = rtnDtChart;
                    series.Data.LabelColumn = "STOPDESC";
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

        private void CboStartdate_H_BeforeDropDown(object sender, CancelEventArgs e)
        {

        }

        
    }
}
