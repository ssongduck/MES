
#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  BM4000
//   Form Name    :  메시지 코드 관리
//   Name Space   : 
//   Created Date : 2012.3.7
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAMMI.Common;
using SAMMI.PopUp;
#endregion

namespace SAMMI.BM
{
    public partial class BM4000 : SAMMI.Windows.Forms.BaseMDIChildForm
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

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        #endregion

        #region < CONSTRUCTOR >
        public BM4000()
        {
            InitializeComponent();
            this.db = DatabaseFactory.CreateDatabase();
            conn = (SqlConnection)this.db.CreateConnection();
            this.daTable1.Connection = conn;
            this.daTable1.Adapter.RowUpdating += new SqlRowUpdatingEventHandler(Adapter_RowUpdating);
            this.daTable1.Adapter.RowUpdated += new SqlRowUpdatedEventHandler(Adapter_RowUpdated);
        }
        #endregion

        #region BM4000_Load
        private void BM4000_Load(object sender, EventArgs e)
        {

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("UseFlag");     //사용여부
            SAMMI.Common.Common.FillComboboxMaster(this.cboUseFlag_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

            rtnDtTemp = _Common.GET_TBM0000_CODE("MESSAGETYPE");     //메세지유형
            SAMMI.Common.Common.FillComboboxMaster(this.cboMessageType_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            #endregion
        }
        #endregion BM4000_Load

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            base.DoInquire();
            

            string messagetype = SqlDBHelper.nvlString(cboMessageType_H.SelectedValue);

            string useflag = SqlDBHelper.nvlString(cboUseFlag_H.SelectedValue); 
            
            // ERROR일때는 throw(new SAMMI.SException("Q......1");
            this.daTable1.Fill(ds.dTable1, messagetype, useflag);
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
                    e.Row.RowError = "창고정보가 있습니다.";
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