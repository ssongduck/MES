#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*                                                                                                                              
//   Form ID      : BM4610                                                                                                                                                                                                      
//   Form Name    : 설비보전 SMS 이력                                                                                                                                                                               
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
    public partial class BM4610 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        public BM4610()
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

        #region BM4610_Load
        private void BM4610_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            // InitColumnUltraGrid  90 113 105 112 147 80 80 163 98 121 98 121 
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SEQNO", "순서", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MSGKEY", "구분", false, GridColDataType_emu.VarChar, 180, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "DAYNIGHT", "주야", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MESSAGE", "메세지", false, GridColDataType_emu.VarChar, 300, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "USETYPE", "용도", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RCVTELNO", "수신번호", false, GridColDataType_emu.VarChar, 300, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SENDTELNO", "송신번호", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "SENDDATE", "수신날짜", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            
            _GridUtil.SetInitUltraGridBind(grid1);

            #endregion

            DtChange = (DataTable)grid1.DataSource;

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장                                                                                                                                                        
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("DayNight");  //주야                                                                                                                                                   
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "DayNight", rtnDtTemp, "CODE_ID", "CODE_NAME");  //주야                                                                                                                

            //rtnDtTemp = _Common.GET_TBM0000_CODE("SMSFLAG");  //수신                                                                                                                                                 
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "MSGKEY", rtnDtTemp, "CODE_ID", "CODE_NAME");  //주야                                                                                                                
            //SAMMI.Common.Common.FillComboboxMaster(this.cboMSG, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");


            //rtnDtTemp = _Common.GET_TBM0000_CODE("MSGKEY");                                                                                                                                            
            //SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "msgkey", rtnDtTemp, "CODE_ID", "CODE_NAME");  //주야                                                                                                                
            //SAMMI.Common.Common.FillComboboxMaster(this.cboMSG, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
 
         

         
            #endregion

        }
        #endregion BM4610_Load

        #region <TOOL BAR AREA >
        /// <summary>                                                                                                                                                                                                           
        /// ToolBar의 조회 버튼 클릭                                                                                                                                                                                            
        /// </summary>                                                                                                                                                                                                          
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];

            try
            {
                DtChange.Clear();

                base.DoInquire();

                string sPlantCode =  SqlDBHelper.nvlString(cboPlantCode_H.Value);  // 사업장 공장코드  
                string sMSG = SqlDBHelper.nvlString(txtKey.Text);  // 사업장 공장코드  
                string sDayNight = SqlDBHelper.nvlString(cboDayNight_H.Value);     // 주.야 구분  
                string sdate = SqlDBHelper.nvlDateTime(cboStartDate_H.Value).ToString("yyyy-MM-dd");
                string sdate_to = SqlDBHelper.nvlDateTime(cboEndDate_H.Value).ToString("yyyy-MM-dd"); 

                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("Msg", sMSG, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("DayNight", sDayNight, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("date", sdate, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("date_to", sdate_to, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM4610_S1", CommandType.StoredProcedure, param);

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
         
        }
        /// <summary>                                                                                                                                                                                                           
        /// ToolBar의 삭제 버튼 Click                                                                                                                                                                                           
        /// </summary>                                                                                                                                                                                                          
        public override void DoDelete()
        {
            
        }
        /// <summary>                                                                                                                                                                                                           
        /// ToolBar의 저장 버튼 Click                                                                                                                                                                                           
        /// </summary>                                                                                                                                                                                                          
        public override void DoSave()
        {
           
        }
        #endregion

        
    }
}