using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SAMMI.PopUp;
using SAMMI.Windows.Forms;
using SAMMI.Common;


namespace SAMMI.SY
{

    public partial class SY0201 : Form
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        //DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        //Common.Common _Common = new Common.Common();
        #endregion

        public SY0201()
        {
            InitializeComponent();
        }

        public SY0201(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [사업장 품번 명 Parameter Show] //사업장, 공정, 설비
                switch (i)
                {
                    case 0:  
                        txtWorkerID.Text = argument[0].ToUpper(); //작업자ID
                        break;

                    case 1:
                        txtWorkerName.Text = argument[1].ToUpper(); //작업자명
                        break;
                }
                #endregion
            }
        }

        private void insert()
        {
            try
            {
                int sGrid2 = grid1.Rows.Count;
                string sCODE = string.Empty;
                string sMSG = string.Empty;

                for (int i = 0; i < sGrid2; i++)
                {
                    string I_WorkerID = this.grid1.Rows[i].Cells["WorkerID"].Value.ToString();

                    INS_TSY0201(I_WorkerID, ref sCODE, ref sMSG);

                }

                if (sCODE == "S")
                {

                    DialogForm dialogform;

                    dialogform = new DialogForm("C:R00005", DialogForm.DialogType.OK);

                    dialogform.ShowDialog();

                }
            }
            catch
            {

            }
            finally
            {

            }

        }


        public void INS_TSY0201(string I_WorkerID, ref string code, ref string mesg)
        {

            SqlDBHelper helper = new SqlDBHelper(false);
            //SqlDBHelper helper = new SqlDBHelper(false,"Data Source=192.168.100.20;Initial Catalog=MTMES;User ID=sa;Password=qwer1234!~");
            SqlParameter[] param = new SqlParameter[2];

            try
            {
                param[0] = helper.CreateParameter("@FromWorkerID", txtWorkerID.Text, SqlDbType.VarChar, ParameterDirection.Input);        // 공장코드
                param[1] = helper.CreateParameter("@ToWorkerID", I_WorkerID, SqlDbType.VarChar, ParameterDirection.Input);        // 항목이름

              //  param[2] = helper.CreateParameter("@RS_CODE", SqlDbType.VarChar, ParameterDirection.Output, null, 1);
              //  param[3] = helper.CreateParameter("@RS_MSG", SqlDbType.VarChar, ParameterDirection.Output, null, 200);


                helper.ExecuteNoneQuery("USP_SY0200_P1", CommandType.StoredProcedure, param);

           //     code = param[2].Value.ToString();
             //   mesg = param[3].Value.ToString();

               // if (code == "S")
             //   {
                    helper.Transaction.Commit();

              //  }
              //  else
             //   {
             //       MessageBox.Show(mesg);
             //   }
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


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void bntAdd_Click(object sender, EventArgs e)
        {
           PopUpManagerEX pu = new PopUpManagerEX();
           DataTable  DtTemp = pu.OpenPopUp("TSY0200", new string[] { "", "", "", "", "", "", "Y" });

           if (DtTemp != null && DtTemp.Rows.Count > 0)
           {
               //Code_ID.Text = Convert.ToString(DtTemp.Rows[0]["WorkerID"]);
               //Code_Name.Text = Convert.ToString(DtTemp.Rows[0]["WorkerName"]);
               _DtTemp = (DataTable)grid1.DataSource;

               int idx = 0;
               if (grid1.ActiveRow != null)
                   idx = grid1.ActiveRow.Index + 1;

               foreach (DataRow dr in DtTemp.Rows)
               {
                   DataRow Row = _DtTemp.NewRow();
                   Row["WorkerID"] = dr["WorkerID"];
                   Row["WorkerName"] = dr["WorkerName"];
                   _DtTemp.Rows.InsertAt(Row,idx);

                   ////추가
                   //int iRow = _GridUtil.AddRow(this.grid1, _DtTemp);
                   //UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkerID", iRow);
                   //UltraGridUtil.ActivationAllowEdit(this.grid1, "WorkerName", iRow);
               }
           }
        }

        private void bntDel_Click(object sender, EventArgs e)
        {
            //삭제
            int idx = this.grid1.ActiveRow == null ? 0 : this.grid1.ActiveRow.Index;
            if (idx >= 0)
                UltraGridUtil.GridRowDelete(grid1, idx);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogForm dialogform;

            dialogform = new DialogForm("C:Q00009", DialogForm.DialogType.YESNO);

            dialogform.ShowDialog();

            if (dialogform.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                insert();
            }
        }

        private void grid1_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            grid_POP_UP();
        }

        private void grid1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                grid_POP_UP();
            }  
        }
        #region grid POP UP 처리
        private void grid_POP_UP()
        {
            int iRow = this.grid1.ActiveRow.Index;

            string sUseFlag = "Y"; //사용여부 

            string sWorkerID = this.grid1.Rows[iRow].Cells["WorkerID"].Text.Trim();  // 
            string sWorkerName = this.grid1.Rows[iRow].Cells["WorkerName"].Text.Trim();  // 

            if (this.grid1.ActiveCell.Column.ToString() == "WorkerID" || this.grid1.ActiveCell.Column.ToString() == "WorkerName")
            {
                _biz.TSY0200_POP_Grid("", "", "", "", sWorkerID, sWorkerName, sUseFlag, grid1, "WorkerID", "WorkerName");
            }
                                                                                                                                     
        }  
        #endregion

        private void SY0201_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.grid1);

            _GridUtil.InitColumnUltraGrid(grid1, "WorkerID", "사용자ID", false, GridColDataType_emu.VarChar, 300, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerName", "사용자명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid1);

        }
    }
}
