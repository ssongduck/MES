using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SAMMI.Common;
using SAMMI.PopManager;


namespace SAMMI.PopUp
{
    public partial class POP_TBM0401 : Form
    {
        string[] argument;
        BizTextBoxManagerEX btbManager;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        #endregion

        public POP_TBM0401(string[] param)
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
                        cboPlantCode_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //plant
                        break;
                    case 1:
                        txtOpCode.Text = argument[1].ToUpper(); //작업장코드
                        break;
                    case 2:
                        txtOpName.Text = argument[2].ToUpper(); //작업장명
                        break;
                    case 3:
                        txtItemCode.Text = argument[3].ToUpper(); //작업장코드
                        break;
                    case 4:
                        txtItemName.Text = argument[4].ToUpper(); //작업장명
                        break;
                    case 5:
                        cboUseFlag_H.Value = argument[5].ToUpper() == "" ? "ALL" : argument[5].ToUpper(); //사용여부
                        break;
                }
                #endregion
            }

            btbManager = new BizTextBoxManagerEX();
            btbManager.PopUpAdd(txtItemCode, txtItemName, "TBM0100", new object[] { cboPlantCode_H, ""});
        }

        private void POP_TBM0401_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "OpCode",  "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "OpName",  "작업장명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag",  "사용유무", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

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
            string sPlantCode = string.Empty;
            string sUseFlag = string.Empty;
            string sOpCode = txtOpCode.Text.Trim();
            string sOpName = txtOpName.Text.Trim();

            if (this.cboPlantCode_H.Value != null)
                sPlantCode = cboPlantCode_H.Value.ToString() == "ALL" ? "" : cboPlantCode_H.Value.ToString();

            if (this.cboUseFlag_H.Value != null)
                sUseFlag = cboUseFlag_H.Value.ToString() == "ALL" ? "" : cboUseFlag_H.Value.ToString();

            _DtTemp = _biz.SEL_TBM0401(sPlantCode, sOpCode, sOpName, txtItemCode.Text, txtItemName.Text, sUseFlag);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }

        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("OpCode", typeof(string));
            TmpDt.Columns.Add("OpName", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["OpCode"].Value, e.Row.Cells["OpName"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtOpCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtOpName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }
    }
}
