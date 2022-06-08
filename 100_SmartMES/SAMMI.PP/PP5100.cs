using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using SAMMI.PopUp;
using SAMMI.Common;
using Infragistics.Win.UltraWinGrid;

namespace SAMMI.PP
{
    public partial class PP5100 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통                                                                                                                                       
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통                                                                                                                                 

        //그리드 객체 생성                                                                                                                                                                              
        UltraGridUtil _GridUtil = new UltraGridUtil();
        BizGridManagerEX BIZPOP;
        Common.Common _Common = new Common.Common();
        PopUp_Biz _biz = new PopUp_Biz();

        private DataTable DtChange = null; //그리드의 변경전 상태값을 담아두는 DataTable                                                                                                                

        private string PlantCode = string.Empty;

        public PP5100()
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

            BIZPOP = new BizGridManagerEX(grid1);

            BIZPOP.PopUpAdd("STOPCODE", "STOPDESC", "TBM1100", new string[] { "", "", "Y" }, new string[] { "STOPTYPE", "STOPCLASS" }, new string[] { "Y" });

        }

        #region  PP5100_Load
        private void PP5100_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정", false, GridColDataType_emu.VarChar, 95, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "라인", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MachCode", "고장설비", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Machname", "설비명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultDate", "고장발생일", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StartDate", "수리시작시각", false, GridColDataType_emu.DateTime24, 170, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultStartDate", "고장시작시각", false, GridColDataType_emu.DateTime24, 170, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EndDate", "종료시각", false, GridColDataType_emu.DateTime24, 170, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultTime", "고장시간", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultType", "고장분류", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultCode", "고장코드", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultName", "고장명", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerCnt", "작업인원", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Right, false, false, null, null, null, null, null);

            _GridUtil.InitColumnUltraGrid(grid1, "ArrivalTime", "작업자도착시각", false, GridColDataType_emu.DateTime24, 170, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAStartTime", "MA시작시각", false, GridColDataType_emu.DateTime24, 170, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MAEndTime", "MA종료시각", false, GridColDataType_emu.DateTime24, 170, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MADesc", "MA내역", false, GridColDataType_emu.VarChar, 170, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "MA등록자", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "MA등록일시", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);

            #region Grid MERGE
            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;                                                                                                       
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;                                                                                              
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;                                                                                                                        
            //                                                                                                                                                                                            
            //grid1.Columns["RECDATE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;                                                                                                         
            //grid1.Columns["RECDATE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;                                                                                                
            //grid1.Columns["RECDATE"].MergedCellStyle = MergedCellStyle.Always;                                                                                                                          
            //                                                                                                                                                                                            
            //grid1.Columns["WORKCENTERCODE"].MergedCellContentArea = MergedCellContentArea.VisibleRect;                                                                                                  
            //grid1.Columns["WORKCENTERCODE"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;                                                                                         
            //grid1.Columns["WORKCENTERCODE"].MergedCellStyle = MergedCellStyle.Always;                                                                                                                   
            //                                                                                                                                                                                            
            //grid1.Columns["WORKCENTERNAME"].MergedCellContentArea = MergedCellContentArea.VisibleRect;                                                                                                  
            //grid1.Columns["WORKCENTERNAME"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;                                                                                         
            //grid1.Columns["WORKCENTERNAME"].MergedCellStyle = MergedCellStyle.Always;                                                                                                                   

            #endregion Grid MERGE

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장                                                                                                                                
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("FAULTTYPE");  //고장분류                                                                                                                            
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "FaultType", rtnDtTemp, "CODE_ID", "CODE_NAME");


            #endregion
        }
        #endregion  PP5100_Load

        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);          //사업장코드                                                                                          
                string sStartDate = string.Format("{0:yyyy-MM-dd}", cboStartDate.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", cboEndDate.Value);
                string sOpCode = txtOPCode.Text.Trim();

                string sWorkCenterCode = txtWorkCenterCode.Text.Trim();

                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("OpCode", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);

                //rtnDtTemp = helper.FillTable("USP_PP5100_S1N", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP5100_S1N_UNION", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                foreach (UltraGridRow ugr in grid1.Rows)
                {

                }

                DtChange = rtnDtTemp;
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

        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;
            grid1.PerformAction(UltraGridAction.DeactivateCell);

            try
            {
                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                base.DoSave();

                foreach (DataRow drRow in DtChange.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            break;
                        case DataRowState.Added:
                            break;
                        case DataRowState.Modified:

                            #region 수정
                            #endregion

                            break;
                    }
                }

                helper.Transaction.Commit();

            }
            catch (Exception ex)
            {
                helper.Transaction.Rollback();
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

            string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);          //사업장코드                                                                                             
            string sOPCode = txtOPCode.Text.Trim();       //공정코드                                                                                                                                    
            string sOPName = txtOPName.Text.Trim();      //공정명                                                                                                                                       
            string sUseFlag = "Y";            //사용여부                                                                                                                                               

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

    }
}