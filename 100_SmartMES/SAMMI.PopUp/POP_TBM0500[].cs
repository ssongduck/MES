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
    public partial class POP_TBM0500 : Form
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        DataTable _rtnDtTemp = new DataTable();
        Common.Common _Common = new Common.Common();
        #endregion

        public POP_TBM0500(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [라인 정보]
                switch (i)
                {
                    case 0:
                        cboPlantCode_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //plant
                        break;
                        
                    case 1:
                        txtOPCode.Text = argument[1].ToUpper();
                        break;

                    case 2:
                        //txtLineName.Text = argument[2].ToUpper(); //라인명
                        txtLineCode.Text = argument[2].ToUpper(); //라인 코드
                        break;
                    
                    case 3:
                        txtLineName.Text  = argument[3].ToUpper() == "" ? "ALL" : argument[3].ToUpper(); //사용여부
                        break;

                    case 4: 
                        cboUseFlag_H.Value = argument[4].ToUpper() == "" ? "ALL" : argument[4].ToUpper(); //사용여부
                        break;
                }
                #endregion
            }
        }

        private void POP_TBM0500_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장코드", false, GridColDataType_emu.VarChar, 150, 50, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCodeNm", "사업장명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "OPCode", "공정코드", false, GridColDataType_emu.VarChar, 80, 50, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 100, 50, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "LineCode", "라인코드", false, GridColDataType_emu.VarChar, 80, 50, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 50, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.SetInitUltraGridBind(Grid1);

            _rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", _rtnDtTemp, "CODE_ID", "CODE_NAME");

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
            string sOPCode = txtOPCode.Text.Trim();
            string sLineCode = txtLineCode.Text.Trim();
            string sLineName = txtLineName.Text.Trim();

            sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");  // 사용유무
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");

            if (this.cboPlantCode_H.Value != null)
                sPlantCode = cboPlantCode_H.Value.ToString() == "ALL" ? "" : cboPlantCode_H.Value.ToString();

            if (this.cboUseFlag_H.Value != null)
                sUseFlag = cboUseFlag_H.Value.ToString() == "ALL" ? "" : cboUseFlag_H.Value.ToString();

            _DtTemp = _biz.SEL_TBM0500(sPlantCode, sOPCode, sLineCode, sUseFlag);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("LineCode", typeof(string));
            TmpDt.Columns.Add("LineName", typeof(string));
            TmpDt.Columns.Add("OPCode", typeof(string));
            TmpDt.Columns.Add("OPName", typeof(string));

            string sOPCode = SqlDBHelper.nvlString(e.Row.Cells["OPCode"].Value);

            string sOPName = SqlDBHelper.nvlString(e.Row.Cells["OPName"].Value);
            //sOPCode = SqlDBHelper.gGetCode(e.Row.Cells["OPCode"].Value);

          //  TmpDt.Rows.Add(new object[] { e.Row.Cells["LineCode"].Value, e.Row.Cells["LineName"].Value });
            TmpDt.Rows.Add(new object[] { SqlDBHelper.nvlString(e.Row.Cells["LineCode"].Value)
                    , SqlDBHelper.nvlString(e.Row.Cells["LineName"].Value), sOPCode, sOPName});

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtLineCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtLineName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
