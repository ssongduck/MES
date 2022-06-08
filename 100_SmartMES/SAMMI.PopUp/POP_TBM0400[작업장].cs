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
    public partial class POP_TBM0400 : Form
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        Common.Common _Common = new Common.Common();
        private string PlantCode = string.Empty;
        #endregion

        public POP_TBM0400(string[] param)
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
                        cboUseFlag_H.Value = argument[3].ToUpper() == "" ? "ALL" : argument[3].ToUpper(); //사용여부
                        break;
                }
                #endregion
            }
        }

        private void POP_TBM0400_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "OpCode", "공정코드", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "OpName", "공정명", false, GridColDataType_emu.VarChar, 250, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag", "사용유무", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

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


            sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
          
            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");  // 사용유무
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");


            _DtTemp = _biz.SEL_TBM0400(sPlantCode, sOpCode, sOpName, sUseFlag);

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

        private void lblOpCode_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblPlantCode_Click(object sender, EventArgs e)
        {

        }

        private void lblOpName_Click(object sender, EventArgs e)
        {

        }

        private void lblUseFlag_Click(object sender, EventArgs e)
        {

        }

        private void cboUseFlag_H_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void cboPlantCode_H_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void txtOpCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtOpName_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
