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
    public partial class POP_TBM1600 : Form
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

        public POP_TBM1600(string[] param)
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
                        cboMoldType_H.Value = argument[1].ToUpper() == "" ? "ALL" : argument[1].ToUpper(); //제품타입
                    //    cboItemType_H.Value   = argument[1].ToUpper(); //타입
                        break;

                    case 2:
                        txtMoldCode.Text   = argument[2].ToUpper(); //품목코드
                        break;
                    
                    case 3:
                        txtMoldName.Text   = argument[3].ToUpper(); //품목명
                    break;
                }
                #endregion
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            search();
        }

        private void search()
        {
            string RS_CODE = string.Empty, RS_MSG = string.Empty;
            string sPlantCode = string.Empty; //= cboPlantCode_H.Value.ToString();
            string sMoldCode = txtMoldCode.Text.Trim();
            string sMoldName = txtMoldName.Text.Trim();

       //     sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);
       //     sMoldType = SqlDBHelper.nvlString(cboMoldType_H.Value);

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("MOLDTYPE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "MoldType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            _DtTemp = _biz.SEL_TBM1600(sPlantCode, sMoldCode ,sMoldName , txtCarType.Text,txtItemName.Text);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
            //_Common.Grid_Column_Width(this.Grid1); //grid 정리용
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("MoldCode", typeof(string));
            TmpDt.Columns.Add("MoldName", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["MoldCode"].Value, e.Row.Cells["MoldName"].Value });

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


        private void POP_TBM1600_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MoldCode", "금형코드", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MoldName", "형번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "CarType", "차종", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MoldType", "금형타입", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(Grid1);

            search();
        }
    }
}
