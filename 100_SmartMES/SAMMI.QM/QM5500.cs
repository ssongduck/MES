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
using System.Configuration;

#endregion

namespace SAMMI.QM
{
    public partial class QM5500 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        DataTable _rtnDtTemp = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        BizGridManagerEX gridManager;

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        // private DataTable DtChange = null;

        DataTable DtChange = new DataTable();
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string PlantCode = string.Empty;


        #endregion

        public QM5500()
        {
            InitializeComponent();

            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this.PlantCode.Equals("SK"))
                this.PlantCode = "SK1";
            else if (this.PlantCode.Equals("EC"))
                this.PlantCode = "SK2";
            else
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;

            
            btbManager = new BizTextBoxManagerEX();
            gridManager = new BizGridManagerEX(grid1);


            GridInit();
        }

        #region [그리드 셋팅]
        private void GridInit()
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CompleteFlag", "측정완료", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "req_no", "관리번호", false, GridColDataType_emu.YearMonthDay, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "seq", "SEQ", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "model_car", "차종", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "resource_no", "품목코드", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "description", "품명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "req_user", "의뢰자", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "req_date", "의뢰일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "req_desc", "의뢰내용", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ThreeInspFlag", "3차원측정 완료여부", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ThreePath", "3차원측정 경로", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IlusInspFlag", "조도측정 완료여부", false, GridColDataType_emu.VarChar, 130, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "IluPath", "조도측정 경로", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ERPFlag", "ERP IF", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ThreeInspFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "IlusInspFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
       
            rtnDtTemp = _Common.GET_TBM0000_CODE("COMPLETEFLAG");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "CompleteFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "ERPFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
       
            #endregion

        }
        #endregion


        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];

            string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);
            string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);
            string sItemCode = SqlDBHelper.nvlString(txtItemCode_Reg.Text.Trim());
            string sReqNo = SqlDBHelper.nvlString(txtItemCode_Reg.Text.Trim());
            
            try
            {
                base.DoInquire();

                param[0] = helper.CreateParameter("@StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[1] = helper.CreateParameter("@EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자      
                param[2] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드            // 생산시작일자      
                param[3] = helper.CreateParameter("@Req_No", sReqNo, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);   
                
                rtnDtTemp = helper.FillTable("USP_QM5500_S1_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                DtChange = rtnDtTemp;

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

                ControlInit();
            }
        }

        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param = null;


            try
            {
                this.Focus();


                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);
                
                base.DoSave();

                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                {
                    CancelProcess = true;
                    return;
                }

                foreach (DataRow dr in ((DataTable)grid1.DataSource).Rows)
                {
                    if (SqlDBHelper.nvlString(dr["CompleteFlag"]).Equals("C"))
                    {
                        param = new SqlParameter[2];

                        param[0] = helper.CreateParameter("@Req_No", grid1.ActiveRow.Cells["req_no"].Value.ToString(), SqlDbType.VarChar, ParameterDirection.Input);         // 공장코드
                        param[1] = helper.CreateParameter("@Maker", LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);

                        helper.ExecuteNoneQuery("USP_QM5500_I3", CommandType.StoredProcedure, param);
                    }

                }

                helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                CancelProcess = true;
                helper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }


        private void btnSUM1_Click(object sender, EventArgs e)
        {
            
            SqlDBHelper helper = new SqlDBHelper(false);
            SqlParameter[] param1 = new SqlParameter[11];

            try
       
            {
                if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                string FileExtension = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf('.'), openFileDialog1.FileName.Length - openFileDialog1.FileName.LastIndexOf('.'));


                base.DoSave();

                string sInspDate = string.Format("{0:yyyy-MM-dd}", SqlDBHelper.nvlString(CboIspDate_H.Value));
                string sReqNo = SqlDBHelper.nvlString(txtReqNo.Text.Trim());
                string sItemCode = SqlDBHelper.nvlString(txtItemCode1.Text.Trim());
                string sItemName = SqlDBHelper.nvlString(txtItemName1.Text.Trim());
                string sJudge = SqlDBHelper.nvlString(cboJudge_H.Value);

                param1[0] = helper.CreateParameter("@IspDate", sInspDate, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)      
                param1[1] = helper.CreateParameter("@ItemCode", sItemCode, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드  
                param1[2] = helper.CreateParameter("@ItemName", sItemName, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param1[3] = helper.CreateParameter("@WorkCenterCode", "*", SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드  
                param1[4] = helper.CreateParameter("@WorkCenterName", "*".Trim(), SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param1[5] = helper.CreateParameter("@Judgment", sJudge, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param1[6] = helper.CreateParameter("@IspType", "I", SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param1[7] = helper.CreateParameter("@DayNight", "*", SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param1[8] = helper.CreateParameter("@Req_No", sReqNo, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param1[9] = helper.CreateParameter("@FileExtension", FileExtension, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드
                param1[10] = helper.CreateParameter("@RT_FileDir", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                param1[10].Direction = System.Data.ParameterDirection.Output;

                /*3차원 측정인경우*/
                if (rbtnThree.Checked== true)
                {
                    DataSet ds = helper.FillDataSet("USP_QM5500_I1", CommandType.StoredProcedure, param1);
                }
                /*조도/형상 측정인경우*/
                if (rbtnIlu.Checked == true)
                {
                    DataSet ds = helper.FillDataSet("USP_QM5500_I2", CommandType.StoredProcedure, param1);
                }

                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(param1[10].Value.ToString().Substring(0, param1[10].Value.ToString().LastIndexOf('\\')));

                if (System.IO.Directory.Exists(di.ToString()) == false) // 이미 열려있으면 넘어간다.
                {
                    System.Diagnostics.Process.Start("cmd.exe", "/C Net Use " + di.ToString() + " /user:" + "attach" + " " + "skatt@ssw5rd");
                }

                if (di.Exists == false)
                    di.Create();
                
                System.IO.File.Copy(openFileDialog1.FileName, param1[10].Value.ToString(), true);

                helper.Transaction.Commit();
            }
            catch (Exception ex)
            {
                helper.Transaction.Rollback();
                this.ShowDialog(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param1 != null) { param1 = null; }
            }

            this.ClosePrgForm();
            DoInquire();
            
        }

        private void grid1_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            ControlInit();
            //grid2.ActiveRow.Cells["spec_no"].Value.ToString();
            this.txtItemCode1.Text = grid1.ActiveRow.Cells["resource_no"].Value.ToString();
            this.txtItemName1.Text = grid1.ActiveRow.Cells["description"].Value.ToString();
            this.txtCarType.Text = grid1.ActiveRow.Cells["model_car"].Value.ToString();
            this.txtReqNo.Text = grid1.ActiveRow.Cells["req_no"].Value.ToString();
        }

        private void ControlInit()
        {
            this.txtItemCode1.Text = string.Empty;
            this.txtItemName1.Text = string.Empty;
            this.txtCarType.Text = string.Empty;
            this.txtReqNo.Text = string.Empty;
        }

    }
}
