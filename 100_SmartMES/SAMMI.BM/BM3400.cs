#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : BM3400
//   Form Name    : 
//   Name Space   : 
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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using SAMMI.Common;
#endregion

namespace SAMMI.BM
{
    public partial class BM3400 : SAMMI.Windows.Forms.BaseMDIChildForm
    {

        #region <MEMBER AREA>
        // 변수나 Form에서 사용될 Class를 정의
        private Database db;
        private SqlConnection conn;
        private SqlTransaction trans;

        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통 
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;
        private int _Fix_Col = 0;



        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        private string PlantCode = string.Empty;

        #endregion

        #region < CONSTRUCTOR >
        public BM3400()
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

            //this.db = DatabaseFactory.CreateDatabase();
            //conn = (SqlConnection)this.db.CreateConnection();
            //this.daTable1.Connection = conn;
            //this.daTable1.Adapter.RowUpdating += new SqlRowUpdatingEventHandler(Adapter_RowUpdating);
            //this.daTable1.Adapter.RowUpdated += new SqlRowUpdatedEventHandler(Adapter_RowUpdated);

            GridInit();


        }
        #endregion

        private void GridInit()
        {

            #region [Grid]
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);

            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultType", "고장유형", false, GridColDataType_emu.VarChar, 100, 30, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultCode", "고장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "FaultName", "고장명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Remark", "비고", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "UseFlag", "사용유무", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, true, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "MakeDate", "등록일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Maker", "등록자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EditDate", "수정일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "Editor", "수정자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);
            DtChange = (DataTable)grid1.DataSource;

            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("FaultType");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "FaultType", rtnDtTemp, "CODE_ID", "CODE_NAME");
            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");


            #endregion
        }

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            base.DoInquire();


            string faulttype =  SqlDBHelper.nvlString(cboFaultTypeCode_H.Value);
            string useflag = SqlDBHelper.nvlString(cboUseFlag_H.Value);


            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[3];

            try
            {
                DtChange.Clear();

                base.DoInquire();

                param[0] = helper.CreateParameter("@FaultType", faulttype, SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@UseFlag", useflag, SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@PlantCode", SqlDBHelper.nvlString(this.cboPlantCode_H.Value), SqlDbType.VarChar, ParameterDirection.Input);

                rtnDtTemp = helper.FillTable("USP_BM3400_S1", CommandType.StoredProcedure, param);
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
            base.DoNew();

            this.grid1.InsertRow();

        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {
            base.DoDelete();

            this.grid1.DeleteRow();
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            try
            {
                if (this.ShowDialog("C:Q00009") == System.Windows.Forms.DialogResult.Cancel)
                    return;

                base.DoSave();

                this.grid1.UpdateGridData();
                conn.Open();
                trans = conn.BeginTransaction();
                this.daTable1.Transaction = trans;
                this.daTable1.Adapter.Update(this.ds.dTable1);

                trans.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw (ex);
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
                e.Command.Parameters["@Editor"].Value = this.WorkerID;
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
                    e.Row.RowError = "고장코드가 등록되어 있습니다.";
                    throw (new SException("S00099", e.Errors));
                default:
                    break;
            }
        }
        #endregion

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        #endregion
    }
}
