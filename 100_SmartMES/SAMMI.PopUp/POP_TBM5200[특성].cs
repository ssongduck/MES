using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SAMMI.Common;
using SAMMI.PopUp;



namespace SAMMI.PopUp
{
    public partial class POP_TBM5200  : Form
    {
        string[] argument;

        private string Major = string.Empty;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        #endregion

        public POP_TBM5200 (string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [품목코드 및 명 Parameter Show]
                switch (i)
                {
                    case 0:
                        txtItemCode.Text   = argument[0].ToUpper(); //품목코드
                        break;
                    
                    case 1:
                        txtItemName.Text   = argument[1].ToUpper(); //품목명
                    break;

                    case 2:
                         Major   = argument[2].ToUpper(); //품목명
                    break;
                }
                #endregion
            }
        }

        private void POP_TBM5200_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);
            
            _GridUtil.InitColumnUltraGrid(Grid1, "CODE_ID",  "규격코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "CODE_NAME",  "규격명", false, GridColDataType_emu.VarChar, 250, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "DISPLAYNO",  "품목타입", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(Grid1);

            search();
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            search();
        }

        private void search()
        {
            string RS_CODE = string.Empty, RS_MSG = string.Empty;
            string sitem_cd = txtItemCode.Text.Trim();
            string sitem_name = txtItemName.Text.Trim();
            
       

            _DtTemp = _biz.SEL_TBM5200(sitem_cd ,sitem_name,Major);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("ItemCode", typeof(string));
            TmpDt.Columns.Add("itemname", typeof(string));
            
            TmpDt.Rows.Add(new object[] { e.Row.Cells["CODE_ID"].Value, e.Row.Cells["CODE_NAME"].Value});

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtItemCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
