using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SAMMI.Common;
using SAMMI.PopUp;



namespace SAMMI.PopUp
{
    public partial class POP_TMO0000 : Form
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        ////비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        #endregion

        public POP_TMO0000(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];
            }
        }

        private void POP_TMO0000_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);
            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ImageSeq", "순번", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ImageName", "파일명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "FileSize", "파일크기", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "Descript", "설명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(Grid1);

            search();
         
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            search();
        }
        private void search()
        {
            string RS_CODE = string.Empty, RS_MSG = string.Empty;
            string sImageSeq = txtImageSeq.Text;          // 공장(사업장)
            string sImageName = txtImageName.Text; // cboOPCode_H.Value.ToString() == "ALL" ? "" : cboOPCode_H.Value.ToString();            // 공정코드

            _DtTemp = _biz.SEL_TMO0000(sImageSeq, sImageName);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
 
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            //DataTable TmpDt = new DataTable();
            //TmpDt.Columns.Add("ImageSeq", typeof(string));
            //TmpDt.Columns.Add("ImageName", typeof(string));

            //if (Grid1.Selected.Rows.Count == 1)
            //{
            //    TmpDt.Rows.Add(new object[] { e.Row.Cells["ImageSeq"].Value, e.Row.Cells["ImageName"].Value });
            //}
            //else
            //{
 
            //    foreach(Infragistics.Win.UltraWinGrid.UltraGridRow dr in Grid1.Selected.Rows)
            //        TmpDt.Rows.Add(new object[] { dr.Cells["ImageSeq"].Value, dr.Cells["ImageName"].Value });

            //}
            //this.Tag = TmpDt;
            //this.Close();

            DataTable TmpDt = new DataTable();

            //TmpDt.Columns.Add("InspCode", typeof(string));
            //TmpDt.Columns.Add("InspName", typeof(string));

            foreach (DataColumn dc in ((DataTable)Grid1.DataSource).Columns)
            {
                TmpDt.Columns.Add(dc.ColumnName, dc.DataType);

            }

            DataRow dr = TmpDt.NewRow();
            foreach (DataColumn dc in TmpDt.Columns)
            {
                dr[dc.ColumnName] = e.Row.Cells[dc.ColumnName].Value;
            }

            TmpDt.Rows.Add(dr);

            //TmpDt.Rows.Add(new object[] { e.Row.Cells["InspCode"].Value, e.Row.Cells["InspName"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtWorkerID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtWorkerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void btnSel_Click(object sender, EventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("ImageSeq", typeof(string));
            TmpDt.Columns.Add("ImageName", typeof(string));


            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in Grid1.Selected.Rows)
                TmpDt.Rows.Add(new object[] { dr.Cells["ImageSeq"].Value, dr.Cells["ImageName"].Value });

            this.Tag = TmpDt;
            this.Close();
        }


    }
}
