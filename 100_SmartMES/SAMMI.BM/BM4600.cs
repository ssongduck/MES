#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*                                                                                                                              
//   Form ID      : BM4600                                                                                                                                                                                                      
//   Form Name    : 설비보전 SMS 알림 관리                                                                                                                                                                                       
//   Name Space   : SAMMI.BM                                                                                                                                                                                                    
//   Created Date :                                                                                                                                                                                                             
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD                                                                                                                                                                            
//   Description  :                                                                                                                                                                                                             
// *---------------------------------------------------------------------------------------------*                                                                                                                              
#endregion

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
#endregion

namespace SAMMI.BM
{
    public partial class BM4600 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통                                                                                                                                                               
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통                                                                                                                                                         

        //그리드 객체 생성                                                                                                                                                                                                      
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;

        //비지니스 로직 객체 생성                                                                                                                                                                                               
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성                                                                                                                                                                                       
        DataTable _DtTemp = new DataTable();
        private string PlantCode = string.Empty;
        
        #endregion

        #region < CONSTRUCTOR >

        public BM4600()
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
        #endregion

        #region BM4600_Load
        private void BM4600_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            // InitColumnUltraGrid  90 113 105 112 147 80 80 163 98 121 98 121 

            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (LoginInfo.PlantAuth == "") ? true : false, true, null, null, null, null, null); 
            _GridUtil.InitColumnUltraGrid(grid1, "MSGKEY", "구분", false, GridColDataType_emu.VarChar, 180, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DAYNIGHT", "주야", false, GridColDataType_emu.VarChar, 105, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MESSAGE", "메세지", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "USETYPE", "용도", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "presentYN", "출근확인", false, GridColDataType_emu.VarChar, 80, 10, Infragistics.Win.HAlign.Center, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RCVTELNO", "수신번호", false, GridColDataType_emu.VarChar, 300, 300, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SENDTELNO", "송신번호", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            
            _GridUtil.SetInitUltraGridBind(grid1);

            #endregion

            DtChange = (DataTable)grid1.DataSource;

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장                                                                                                                                                        
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");
          
            rtnDtTemp = _Common.GET_TBM0000_CODE("DayNight");  //주야                                                                                                                                                   
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DayNight", rtnDtTemp, "CODE_ID", "CODE_NAME");  //주야                                                                                                                
           // SAMMI.Common.Common.FillComboboxMaster(this.cboDayNight_H0, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            rtnDtTemp = _Common.GET_TBM0000_CODE("YESNO");     //Lot관리여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "presentYN", rtnDtTemp, "CODE_ID", "CODE_NAME");

            //rtnDtTemp = _Common.GET_TBM0000_CODE("SMSFLAG");  //수신                                                                                                                                                 
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MSGKEY", rtnDtTemp, "CODE_ID", "CODE_NAME");  //주야                                                                                                                
            //SAMMI.Common.Common.FillComboboxMaster(this.cboMSG, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

            //rtnDtTemp = _Common.GET_TBM0000_CODE("MSGKEY");                                                                                                                                            
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "msgkey", rtnDtTemp, "CODE_ID", "CODE_NAME");  //주야                                                                                                                
            //SAMMI.Common.Common.FillComboboxMaster(this.cboMSG, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
 
         

         
            #endregion

        }
        #endregion BM4600_Load

        #region <TOOL BAR AREA >
        /// <summary>                                                                                                                                                                                                           
        /// ToolBar의 조회 버튼 클릭                                                                                                                                                                                            
        /// </summary>                                                                                                                                                                                                          
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                DtChange.Clear();

                base.DoInquire();

                string sPlantCode =  SqlDBHelper.nvlString(cboPlantCode_H.Value);  // 사업장 공장코드  
                string sMSG = SqlDBHelper.nvlString(txtKey.Text);  //   
                string sDayNight = SqlDBHelper.nvlString(cboDayNight_H.Value);     // 주.야 구분  

                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("Msg", sMSG, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("DayNight", sDayNight, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM4600_S1", CommandType.StoredProcedure, param);
                rtnDtTemp.AcceptChanges();
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
            }
        }
        /// <summary>                                                                                                                                                                                                           
        /// ToolBar의 신규 버튼 클릭                                                                                                                                                                                            
        /// </summary>                                                                                                                                                                                                          
        public override void DoNew()
        {
            try
            {
                base.DoNew();

                int iRow = _GridUtil.AddRow(this.grid1, DtChange);
                UltraGridUtil.ActivationAllowEdit(this.grid1, "PLANTCODE", iRow);     // 사업장
                UltraGridUtil.ActivationAllowEdit(this.grid1, "MSGKEY", iRow);      // 
                UltraGridUtil.ActivationAllowEdit(this.grid1, "DAYNIGHT", iRow);      // 주야
                                                                                                                                                                                                                                    

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>                                                                                                                                                                                                           
        /// ToolBar의 삭제 버튼 Click                                                                                                                                                                                           
        /// </summary>                                                                                                                                                                                                          
        public override void DoDelete()
        {
            //UltraGridUtil.DeleteCurrentRowGrid(this, this.grid1, DtChange);
            base.DoDelete();

            this.grid1.DeleteRow();
        }
        /// <summary>                                                                                                                                                                                                           
        /// ToolBar의 저장 버튼 Click                                                                                                                                                                                           
        /// </summary>                                                                                                                                                                                                          
        public override void DoSave()
        {
            SqlDBHelper helper = new SqlDBHelper(false, false);
            SqlParameter[] param = null;

            try
            {
                this.Focus();

                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                base.DoSave();

                UltraGridUtil.DataRowDelete(this.grid1);
                this.grid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.DeactivateCell);

                foreach (DataRow drRow in DtChange.Rows)
                {
                    switch (drRow.RowState)
                    {
                        case DataRowState.Deleted:
                            #region 삭제
                            drRow.RejectChanges();

                            param = new SqlParameter[5];

                            param[0] = helper.CreateParameter("PLANTCODE", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PLANTCODE"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("MSGKEY", drRow["MSGKEY"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);           // 일자                                                     
                            param[2] = helper.CreateParameter("DAYNIGHT", drRow["DAYNIGHT"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 주야                                                        
                            
                            param[3] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[4] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM4600_D1", CommandType.StoredProcedure, param);

                            if (param[3].Value.ToString() == "E") throw new Exception(param[4].Value.ToString());
                            #endregion
                            break;
                        case DataRowState.Added:
                            #region 추가
                            param = new SqlParameter[10];
                            param[0] = helper.CreateParameter("PLANTCODE", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PLANTCODE"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("MSGKEY", drRow["MSGKEY"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 일자                                        
                            param[2] = helper.CreateParameter("DAYNIGHT", drRow["DAYNIGHT"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 주야                                          
                            param[3] = helper.CreateParameter("MESSAGE ", drRow["MESSAGE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 작업자ID                                    
                            param[4] = helper.CreateParameter("USETYPE ", drRow["USETYPE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    // 생산담당자                                    
                            param[5] = helper.CreateParameter("RCVTELNO", drRow["RCVTELNO"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    // 보전수신자                                    
                            param[6] = helper.CreateParameter("SENDTELNO", drRow["SENDTELNO"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 연락처
                            param[7] = helper.CreateParameter("presentYN", drRow["presentYN"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 연락처
                            
                            param[8] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[9] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM4600_I2", CommandType.StoredProcedure, param);

                            if (param[7].Value.ToString() == "E") throw new Exception(param[8].Value.ToString());

                            #endregion
                            break;
                        case DataRowState.Modified:
                            #region 수정

                            param = new SqlParameter[10];
                            param[0] = helper.CreateParameter("PLANTCODE", (LoginInfo.PlantAuth.Equals("")) ? SqlDBHelper.nvlString(drRow["PLANTCODE"]) : LoginInfo.PlantAuth, SqlDbType.VarChar, ParameterDirection.Input);
                            param[1] = helper.CreateParameter("MSGKEY", drRow["MSGKEY"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 일자                                        
                            param[2] = helper.CreateParameter("DAYNIGHT", drRow["DAYNIGHT"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 주야                                          
                            param[3] = helper.CreateParameter("MESSAGE ", drRow["MESSAGE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 작업자ID                                    
                            param[4] = helper.CreateParameter("USETYPE ", drRow["USETYPE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    // 생산담당자                                    
                            param[5] = helper.CreateParameter("RCVTELNO", drRow["RCVTELNO"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);    // 보전수신자                                    
                            param[6] = helper.CreateParameter("SENDTELNO", drRow["SENDTELNO"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 연락처
                             param[7] = helper.CreateParameter("presentYN", drRow["presentYN"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);       // 연락처
                           
                            param[8] = helper.CreateParameter("RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
                            param[9] = helper.CreateParameter("RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);

                            helper.ExecuteNoneQuery("USP_BM4600_U1", CommandType.StoredProcedure, param);

                            if (param[7].Value.ToString() == "E") throw new Exception(param[8].Value.ToString());

                            #endregion
                            break;
                    }
                }

                //helper.Transaction.Commit();

            }
            catch (Exception ex)
            {
               // helper.Transaction.Rollback();
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }
        #endregion

        #region < EVENT AREA >
        /// <summary>                                                                                                                                                                                                           
        /// Form이 Close 되기전에 발생                                                                                                                                                                                          
        /// e.Cancel을 true로 설정 하면, Form이 close되지 않음                                                                                                                                                                  
        /// 수정 내역이 있는지를 확인 후 저장여부를 물어보고 저장, 저장하지 않기, 또는 화면 닫기를 Cancel 함                                                                                                                    
        /// </summary>                                                                                                                                                                                                          
        /// <param name="sender"></param>                                                                                                                                                                                       
        /// <param name="e"></param>                                                                                                                                                                                            
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {

        }
        /// <summary>                                                                                                                                                                                                           
        /// DATABASE UPDATE전 VALIDATEION CHECK 및 값을 수정한다.                                                                                                                                                               
        /// </summary>                                                                                                                                                                                                          
        /// <param name="sender"></param>                                                                                                                                                                                       
        /// <param name="e"></param>                                                                                                                                                                                            
        void Adapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            if (e.Row.RowState == DataRowState.Modified)
            {
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
                return;
            }

            if (e.Row.RowState == DataRowState.Added)
            {
                //e.Command.Parameters["@Editor"].Value = this.WorkerID;                                                                                                                                                        
                e.Command.Parameters["@Maker"].Value = this.WorkerID;
                return;
            }
        }

        /// <summary>                                                                                                                                                                                                           
        /// 저장처리시 오류가 발생한 경우 오류 메세지에 대한 처리                                                                                                                                                               
        /// </summary>                                                                                                                                                                                                          
        /// <param name="sender"></param>                                                                                                                                                                                       
        /// <param name="e"></param>                                                                                                                                                                                            
        void Adapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Errors == null) return;

            switch (((SqlException)e.Errors).Number)
            {
                // 중복                                                                                                                                                                                                         
                case 2627:
                    e.Row.RowError = "데이터가 중복입니다.";
                    throw (new SException("C:S00099", e.Errors));
                default:
                    break;
            }
        }
        #endregion

        private void grid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        

   
    }
}