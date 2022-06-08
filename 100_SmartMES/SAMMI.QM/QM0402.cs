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
    public partial class QM0402 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        public QM0402()
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
                btbManager.PopUpAdd(txtOpCode, txtOpName, "TBM0400", new object[] { cboPlantCode_H, "" });
                btbManager.PopUpAdd(txtLineCode, txtLineName, "TBM0501", new object[] { cboPlantCode_H, txtOpCode, "" }
                        , new string[] { "OPCode", "OPName" }, new object[] { txtOpCode, txtOpName });
                btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { cboPlantCode_H, txtOpCode, txtLineCode, "" }
                        , new string[] { "OPCode", "OPName", "LineCode", "LineName" }, new object[] { txtOpCode, txtOpName, txtLineCode, txtLineName });
                btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0101", new object[] { cboPlantCode_H, txtWorkCenterCode, txtWorkCenterName, }
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
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                       // 사업장(공장)
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);
                string sWorkCenterCode = SqlDBHelper.nvlString(this.txtWorkCenterCode.Text);
                string sOpCode = SqlDBHelper.nvlString(this.txtOpCode.Text);
                string sLineCode = SqlDBHelper.nvlString(this.txtLineCode.Text);
                string sItemCode = SqlDBHelper.nvlString(this.txtItemCode.Text);

                param1[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param1[1] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param1[2] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                param1[3] = helper.CreateParameter("OpCode", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param1[4] = helper.CreateParameter("LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param1[5] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param1[6] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드

                //DataSet ds = helper.FillDataSet("USP_QM0402_S1_UNION", CommandType.StoredProcedure, param1);
                //rtnDtTemp1 = ds.Tables[0];
                /*작업장별 불량 TOP 10*/
                rtnDtTemp1 = helper.FillTable("USP_QM0402_S1_UNION", CommandType.StoredProcedure, param1);

               
                DataView dvChartItem = rtnDtTemp1.DefaultView;
                dvChartItem.Sort = "ratio DESC";
                rtnDtTemp1 = dvChartItem.ToTable();

                int fqty=Convert.ToInt32(uneOKStart.Value);
                int sqty=Convert.ToInt32(uneOKEnd.Value);


                for (int i = rtnDtTemp1.Rows.Count - 1; i >= 0;i-- )
                {
                    try{
                        int cqty = Convert.ToInt32(rtnDtTemp1.Rows[i]["okqty"]);
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
                            QtySum += Convert.ToInt64(dr["okqty"]);
                            ErrSum += Convert.ToInt64(dr["ErrorQty"]);
                            RatioSum += Convert.ToDouble(dr["ratio"]);

                        }
                        cnt++;
                    }

                    DataRow drOrders1 = rtnDtChart1.NewRow();
                    drOrders1["ItemCode"] = "--";
                    drOrders1["ItemName"] = "기타";
                    drOrders1["okqty"] = QtySum.ToString();
                    drOrders1["ErrorQty"] = ErrSum.ToString();
                    drOrders1["ratio"] = RatioSum.ToString();
                    rtnDtChart1.Rows.Add(drOrders1);

                    NumericSeries series = new NumericSeries();
                    series.Data.DataSource = rtnDtChart1;
                    series.Data.LabelColumn = "ItemName";
                    series.Data.ValueColumn = "ratio";
                    series.DataBind();

                    ultraChart2.Series.Add(series);
                    ultraChart2.Data.DataBind();

                    ultraChart2.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
                    ultraChart2.Tooltips.Font = new Font("맑은 고딕", 12);

                }
                #endregion

                //#region [유형별 불량 조회]

                //rtnDtTemp2 = ds.Tables[1];

                param2[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param2[1] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param2[2] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                param2[3] = helper.CreateParameter("OpCode", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param2[4] = helper.CreateParameter("LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param2[5] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드
                param2[6] = helper.CreateParameter("ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드

                rtnDtTemp2 = helper.FillTable("USP_QM0402_S2_UNION", CommandType.StoredProcedure, param2);

                DataView dvChartEC = rtnDtTemp2.DefaultView;
                dvChartEC.Sort = "ItemCode asc, WorkCenterCode asc";
                //dvChartEC.Sort = "WorkCenterCode";
                rtnDtTemp2 = dvChartEC.ToTable();
                
                grid2.DataSource = rtnDtTemp2;
                grid2.DataBind();
                _Common.Grid_Column_Width(this.grid2); //grid 정리용   

                grid1_AfterRowActivate(null,null);

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

         //    _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
        //    _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null); 
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "okqty", "양품수량", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ErrorQty", "불량수량", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ratio", "불량율", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, "0.00", null, null, null, null);
 
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            #region Grid2 셋팅
            _GridUtil.InitializeGrid(this.grid2, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid2, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ItemName", "품명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
             _GridUtil.InitColumnUltraGrid(grid2, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
             _GridUtil.InitColumnUltraGrid(grid2, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "okqty", "양품수량", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ErrorQty", "불량수량", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ratio", "불량율", false, GridColDataType_emu.Integer, 90, 100, Infragistics.Win.HAlign.Right, true, false, "0.00", null, null, null, null);
            _GridUtil.SetInitUltraGridBind(grid2);
            #endregion

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME"); 
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid2, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
        }

        private void QM0402_Load(object sender, EventArgs e)
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

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                string itemcode = grid1.ActiveRow.Cells["ItemCode"].Value.ToString();

                ((DataTable)grid2.DataSource).DefaultView.RowFilter = "ItemCode='" + itemcode + "'";
            }
            catch { }
        }
    }
}
