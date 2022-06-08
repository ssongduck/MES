using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace SAMMI.SY
{
    public partial class SY0000 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region <MEMBER AREA>
        private Database       db;
        private SqlConnection  conn;
        private SqlTransaction trans;
        #endregion

        #region < CONSTRUCTOR >
        public SY0000()
        {
            InitializeComponent();
            this.db = DatabaseFactory.CreateDatabase();
            conn = (SqlConnection)this.db.CreateConnection();
            this.daTable1.Adapter.RowUpdating += new SqlRowUpdatingEventHandler(Adapter_RowUpdating);
            this.daTable1.Adapter.RowUpdated  += new SqlRowUpdatedEventHandler(Adapter_RowUpdated);

            this.daTable1.Connection = conn;
        }
        #endregion

        #region <TOOL BAR AREA >
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            // 조회전 변경된 사항을 확인

            if ((this.ds.dTable1.HasErrors) || (this.ds.dTable1.GetChanges() != null))
            {
                if (this.ShowDialog("C:Q00010") == System.Windows.Forms.DialogResult.OK)
                    this.DoSave();
            };
            this.grid1.BeginUpdate();
            
            base.DoInquire();

            this.daTable1.Fill(this.ds.dTable1, 
                               this.txtProgramID_H.Text, 
                               this.cbxUseFlag_H.Value == null ? "" : this.cbxUseFlag_H.Value.ToString());
            this.grid1.EndUpdate();
        }

        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            base.DoNew();

            this.grid1.InsertRow();
            this.grid1.ActiveRow.Cells["UseFlag"].Value = "Y";
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
                //this.daTable1.Adapter.Update(this.ds.dTable1);
                daTable1.Update(this.ds.dTable1);
                trans.Commit();
                conn.Close();
            }
            catch(Exception ex)
            {
                trans.Rollback();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw(ex);
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
                        e.Row.RowError = "중복데이터 입니다.";
                        throw (new SException("C:S00099", e.Errors));
                   default:
                        break;
            }
        }
        #endregion

        private void cbxUseFlag_H_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        #endregion

    }
}
