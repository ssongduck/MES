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
    public partial class QM0401 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp1 = new DataTable(); // return DataTable 공통
        DataTable rtnDtTemp2 = new DataTable(); // return DataTable 공통

        DataTable rtnDtChart1 = new DataTable();
        DataTable rtnDtChart2 = new DataTable();
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        // private DataTable DtChange = null;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string PlantCode = string.Empty;

        public QM0401()
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

            if (LoginInfo.PlantAuth.Equals(string.Empty))
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { this.cboPlantCode_H, txtOpCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOpCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOpCode, txtOpName, txtLineCode, txtLineName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { this.cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }
            else
            {
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { LoginInfo.PlantAuth, txtOpCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOpCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOpCode, txtOpName, txtLineCode, txtLineName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { LoginInfo.PlantAuth, txtWorkCenterCode, txtWorkCenterName, }
                       , new string[] { "WorkCenterCode", "WorkCenterName" }, new object[] { });
            }

            ultraChart1.EmptyChartText = string.Empty;
            ultraChart2.EmptyChartText = string.Empty;
        }

        #region 조회
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param1 = new SqlParameter[7];
            SqlParameter[] param2 = new SqlParameter[7];

            try
            {
                base.DoInquire();

                #region [작업장별 불량 조회]
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                     // 사업장(공장)
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);
                string sWorkCenterCode = SqlDBHelper.nvlString(this.txtWorkCenterCode.Text);
                string sOpCode = SqlDBHelper.nvlString(this.txtOpCode.Text);
                string sLineCode = SqlDBHelper.nvlString(this.txtLineCode.Text);
                string sItemCode = SqlDBHelper.nvlString(this.txtItemCode.Text);

                param1[0] = helper.CreateParameter("PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param1[1] = helper.CreateParameter("STARTDATE", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param1[2] = helper.CreateParameter("ENDDATE", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                param1[3] = helper.CreateParameter("OPCODE", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param1[4] = helper.CreateParameter("LINECODE", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param1[5] = helper.CreateParameter("WORKCENTERCODE", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param1[6] = helper.CreateParameter("ITEMCODE", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드

                //DataSet ds = helper.FillDataSet("USP_QM0401_S1", CommandType.StoredProcedure, param1);
                //DataSet ds = helper.FillDataSet("USP_QM0401_S1_UNION", CommandType.StoredProcedure, param1);
                //rtnDtTemp1 = ds.Tables[0];

                /*작업장별 불량 TOP 10*/
                rtnDtTemp1 = helper.FillTable("USP_QM0401_S1_UNION", CommandType.StoredProcedure, param1);

                DataView dvChartWC = rtnDtTemp1.DefaultView;
                dvChartWC.Sort = "RATIO DESC";
                rtnDtTemp1 = dvChartWC.ToTable();

                int fqty=Convert.ToInt32(uneOKStart.Value);
                int sqty=Convert.ToInt32(uneOKEnd.Value);

                for (int i = rtnDtTemp1.Rows.Count - 1; i >= 0;i-- )
                {
                    try{
                        int cqty = Convert.ToInt32(rtnDtTemp1.Rows[i]["OKQTY"]);
                        if (cqty < fqty || cqty > sqty)
                            rtnDtTemp1.Rows.RemoveAt(i);
                    }
                    catch{}
                }

                
                grid1.DataSource = rtnDtTemp1;
                grid1.DataBind();
                //_Common.Grid_Column_Width(this.grid1); //grid 정리용   

                rtnDtChart1.Clear();
                rtnDtChart1 = rtnDtTemp1.Clone();

                if (rtnDtTemp1.Rows.Count > 0)
                {
                    ultraChart2.Series.Clear();

                    Int64 QtySum = 0;
                    Int64 ErrSum = 0;
                    double RatioSum = 0.000000;
                    int cnt = 0;
                    foreach (DataRow dr in rtnDtTemp1.Rows)
                    {
                        if (cnt < 10)
                        {
                            rtnDtChart1.Rows.Add(dr.ItemArray);

                        }
                        else
                        {
                            QtySum += Convert.ToInt64(dr["OKQTY"]);
                            ErrSum += Convert.ToInt64(dr["ERRORQTY"]);
                            RatioSum += Convert.ToDouble(dr["RATIO"]);

                        }
                        cnt++;
                    }

                    DataRow drOrders1 = rtnDtChart1.NewRow();
                    drOrders1["PLANTCODE"] = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                    drOrders1["WORKCENTERCODE"] = "기타";
                    drOrders1["WORKCENTERNAME"] = "기타";
                    drOrders1["OKQTY"] = QtySum.ToString();
                    drOrders1["ERRORQTY"] = ErrSum.ToString();
                    drOrders1["RATIO"] = RatioSum.ToString();
                    rtnDtChart1.Rows.Add(drOrders1);

                    NumericSeries series = new NumericSeries();
                    series.Data.DataSource = rtnDtChart1;
                    series.Data.LabelColumn = "WorkCenterName";
                    series.Data.ValueColumn = "ratio";
                    series.DataBind();

                    ultraChart2.Series.Add(series);
                    ultraChart2.Data.DataBind();

                    ultraChart2.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
                    ultraChart2.Tooltips.Font = new Font("맑은 고딕", 12);

                }
                #endregion

                #region [유형별 불량 조회]

                param2[0] = helper.CreateParameter("PLANTCODE", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param2[1] = helper.CreateParameter("STARTDATE", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param2[2] = helper.CreateParameter("ENDDATE", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                param2[3] = helper.CreateParameter("OPCODE", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param2[4] = helper.CreateParameter("LINECODE", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param2[5] = helper.CreateParameter("WORKCENTERCODE", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param2[6] = helper.CreateParameter("ITEMCODE", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                rtnDtTemp2 = helper.FillTable("USP_QM0401_S2_UNION", CommandType.StoredProcedure, param2);

                DataView dvChartEC = rtnDtTemp2.DefaultView;
                dvChartEC.Sort = "ERRORQTY DESC";
                rtnDtTemp2 = dvChartEC.ToTable();

                grid2.DataSource = rtnDtTemp2;
                grid2.DataBind();
                _Common.Grid_Column_Width(this.grid2); //grid 정리용   


                if (rtnDtTemp2.Rows.Count > 0)
                {
                    ultraChart1.Series.Clear();
                    ultraChart1.Visible = true;
                    rtnDtChart2 = rtnDtTemp2.Clone();

                    int cnt = 0;
                    double sum = 0.0;
                    foreach (DataRow dr in rtnDtTemp2.Rows)
                    {
                        if (cnt < 10)
                        {
                            rtnDtChart2.Rows.Add(dr.ItemArray);

                        }
                        else
                        {
                            sum += Convert.ToDouble(dr["ErrorQty"]);

                        }
                        cnt++;
                    }


                    DataRow drOrders2 = rtnDtChart2.NewRow();
                    drOrders2["PLANTCODE"] = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                    drOrders2["ERRORCLASS"] = "기타";
                    drOrders2["ERRORCODE"] = "기타";
                    drOrders2["ERRORDESC"] = "기타";
                    drOrders2["ERRORQTY"] = sum.ToString();
                    rtnDtChart2.Rows.Add(drOrders2);

                    NumericSeries series = new NumericSeries();
                    series.Data.DataSource = rtnDtChart2;
                    series.Data.LabelColumn = "ERRORDESC";
                    series.Data.ValueColumn = "ERRORQTY";
                    ultraChart1.Axis.X.TimeAxisStyle.TimeAxisStyle = Infragistics.UltraChart.Shared.Styles.RulerGenre.Continuous;
                    ultraChart1.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
                    series.DataBind();

                    ultraChart1.Series.Add(series);
                    ultraChart1.Data.DataBind();

                    ultraChart1.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
                    ultraChart1.Tooltips.Font = new Font("맑은 고딕", 12);

                }
                else
                {
                    ultraChart1.Visible = false;
                }
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param1 != null) { param1 = null; }
                if (param2 != null) { param2 = null; }
            }

        }
        #endregion 조회

        private void GridInit()
        {
            #region Grid1 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OKQTY", "양품수량", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ERRORQTY", "불량수량", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RATIO", "불량율", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "0.00", null, null, null, null);
            
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            #region Grid2 셋팅
            _GridUtil.InitializeGrid(this.grid2, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid2, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ERRORCLASS", "불량유형", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ERRORCODE", "불량코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ERRORDESC", "불량내역", false, GridColDataType_emu.VarChar, 220, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ERRORQTY", "불량수량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid2);
            #endregion

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid2, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");
        }

        private void QM0401_Load(object sender, EventArgs e)
        {
            GridInit();
        }

        private void ultraChart2_DataItemOver(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            if (ultraChart2.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                ultraChart2.Tooltips.FormatString = e.ColumnLabel.Trim() + " : <DATA_VALUE:#,#>";
            }
        }

        private void ultraChart1_DataItemOver(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            if (ultraChart1.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                ultraChart1.Tooltips.FormatString = e.ColumnLabel.Trim() + " : <DATA_VALUE:#,#>";
            }
        }
    }
}
