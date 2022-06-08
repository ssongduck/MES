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
    public partial class POP_TBM4100 : Form
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        #endregion

        public POP_TBM4100(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [사유코드 및 명 Parameter Show]
                switch (i)
                {
                    case 0:
                        cboResType_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //plant
                        break;
                        
                    case 1:
                        txtResCode.Text = argument[1].ToUpper(); //사유코드
                        break;

                    case 2:
                        txtResName.Text = argument[2].ToUpper(); //사유명
                        break;
                    
                    case 3:
                        cboUseFlag_H.Value = argument[3].ToUpper() == "" ? "ALL" : argument[3].ToUpper(); //사용여부
                        break;
                }
                #endregion
            }
        }

        private void POP_TBM4100_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);
            _GridUtil.InitColumnUltraGrid(Grid1, "ResType", "사유유형", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ResTypeNm", "유형명", false, GridColDataType_emu.VarChar,100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);            
            _GridUtil.InitColumnUltraGrid(Grid1, "ResCode",  "사유코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ResName",  "사유명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag",  "사용유무", false, GridColDataType_emu.VarChar, 180, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

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
            string sResType = string.Empty;
            string sUseFlag = string.Empty;
            string sResCode = txtResCode.Text.Trim();
            string sResName = txtResName.Text.Trim();

            if (this.cboResType_H.Value != null)
                sResType = cboResType_H.Value.ToString() == "ALL" ? "" : cboResType_H.Value.ToString();

            if (this.cboUseFlag_H.Value != null)
                sUseFlag = cboUseFlag_H.Value.ToString() == "ALL" ? "" : cboUseFlag_H.Value.ToString();

               _DtTemp = _biz.SEL_TBM4100(sResType, sResCode, sResName, sUseFlag);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("ResCode", typeof(string));
            TmpDt.Columns.Add("ResName", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["ResCode"].Value, e.Row.Cells["ResName"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtResCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtResName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
