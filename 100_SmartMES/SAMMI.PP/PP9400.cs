#region < HEADER AREA >

#endregion

#region <USING AREA>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;   
                         

#endregion

namespace SAMMI.PP
{
    public partial class PP9400 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        private string PlantCode = string.Empty;
        #endregion
        
        public PP9400()
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
        private void GridInit()
        {
            //_GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "REQUESTDATE", "요청날짜", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MACHCODE", "설비", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MACHNAME", "설비명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "STARTDATE", "요청시각", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "REQUESTDESC", "요청내역", false, GridColDataType_emu.VarChar, 400, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "REQUESTFLAG", "진행구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;
            ///row number
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
        }

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                DtChange.Clear();
                base.DoInquire();
                string sUseFlag = SqlDBHelper.nvlString(this.cboREQUESTFLAG.Value);
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);          
                string sEndDate   = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);              
                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                //string sOpCode = txtOpCode.Text;
               // string sOpName = txtOpName.Text;


                param[0] = helper.CreateParameter("@WORKDATE", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@WORKDATETO", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@REQUESTFLAG", sUseFlag, SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);
              
                // param[2] = helper.CreateParameter("@OPCode", sOpCode, SqlDbType.VarChar, ParameterDirection.Input);
                //param[3] = helper.CreateParameter("@OPName", sOpName, SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_PP9400_S1_UNION", CommandType.StoredProcedure, param);

                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

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
            SqlDBHelper helper = new SqlDBHelper(false);
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
                      case DataRowState.Modified:
                            #region [수정]
                            param = new SqlParameter[5];

                            param[0] = helper.CreateParameter("@WORKCENTERCODE", drRow["WORKCENTERCODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[1] = helper.CreateParameter("@MACHCODE", drRow["MACHCODE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);             // 작업장(공정)
                            param[2] = helper.CreateParameter("@STARTDATE", drRow["STARTDATE"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[3] = helper.CreateParameter("@REQUESTFLAG", drRow["REQUESTFLAG"].ToString(), SqlDbType.VarChar, ParameterDirection.Input);              // 공정순서
                            param[4] = helper.CreateParameter("@Editor",  SAMMI.Common.LoginInfo.UserID, SqlDbType.VarChar, ParameterDirection.Input);           // 점검항목
                            
                            helper.ExecuteNoneQuery("USP_PP9400_U1", CommandType.StoredProcedure, param);
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
            //if (e.Row.RowState == DataRowState.Modified)
            //{
            //    e.Command.Parameters["@Editor"].Value = this.WorkerID;
            //    return;
            //}

            //if (e.Row.RowState == DataRowState.Added)
            //{
            //    e.Command.Parameters["@Maker"].Value = this.WorkerID;
            //    return;
            //}
        }

        /// <summary>
        /// 저장처리시 오류가 발생한 경우 오류 메세지에 대한 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Adapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            //if (e.Errors == null) return;

            //switch (((SqlException)e.Errors).Number)
            //{
            //    // 중복
            //    case 2627:
            //        e.Row.RowError = "공정코드가 있습니다.";
            //        throw (new SException("S00099", e.Errors));
            //    default:
            //        break;
            //}
        }

      

        #endregion



        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의


        private void PP9400_Load(object sender, EventArgs e)
        {
            GridInit();

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            
            rtnDtTemp = _Common.GET_TBM0000_CODE("REQUESTFLAG");     //사용여부
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "REQUESTFLAG", rtnDtTemp, "CODE_ID", "CODE_NAME");

            SAMMI.Common.UltraGridUtil.SetGridDataCopy(this.grid1);
        }


        #endregion
    }
}
