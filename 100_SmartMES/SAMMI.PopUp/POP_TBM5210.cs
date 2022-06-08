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
    public partial class POP_TBM5210 : Form
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        BizTextBoxManagerEX btbManager;
        //public string sWorkCenterCode = string.Empty;
        #endregion

        public POP_TBM5210(string[] param)
        {
            InitializeComponent();

            btbManager = new BizTextBoxManagerEX();
            btbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { cboPlantCode_H, "", "", "" }
                     , new string[] { }, new object[] { });

            
            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [작업장 OP코드 및 명 Parameter Show]
                switch (i)
                {
                    //_biz.TBM5210_POP_Grid(aParam1[0], sValueCode, sValueName, aParam1[1], aParam1[2], grid, sCode, sName);
                    //gridManager.PopUpAdd("WorkCenterLineCode", "WorkCenterLineName", "TBM5210", new string[] { "PlantCode", "", "" });
                    case 0:
                        cboPlantCode_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //plant
                        break;

                    case 1:
                        txtWorkCenterCode.Text = argument[1].ToUpper();
                        break;

                    case 2:
                        txtWorkCenterLineCode.Text = argument[2].ToUpper(); //작업장 OP 코드
                        break;

                    case 3:
                        txtWorkCenterLineName.Text = argument[3].ToUpper(); //작업장 OP 명
                        break;
                    case 4:
                        
                        cboUseFlag_H.Value = argument[4].ToUpper() == "" ? "ALL" : argument[4].ToUpper(); //사용여부break;
                        break;
                }
                #endregion
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void POP_TBM0610_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterLineCode", "가공라인코드", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterLineName", "가공라인", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
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
            string sWorkCenterLineCode = txtWorkCenterLineCode.Text.Trim();
            string sWorkCenterLineName = txtWorkCenterLineName.Text.Trim();
            string sWorkCenterCode = txtWorkCenterCode.Text.Trim();

            sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");  // 사용유무
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");


            _DtTemp = _biz.SEL_TBM5210(sPlantCode, sWorkCenterLineCode, sWorkCenterLineName, sWorkCenterCode, sUseFlag);
            //_DtTemp = _biz.SEL_TBM5210(sPlantCode, sWorkCenterLineCode, sWorkCenterLineName, sWorkCenterCode, sUseFlag);
            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }

        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("WorkCenterCode", typeof(string));
            TmpDt.Columns.Add("WorkCenterName", typeof(string));
            TmpDt.Columns.Add("WorkCenterLineCode", typeof(string));
            TmpDt.Columns.Add("WorkCenterLineName", typeof(string));


            TmpDt.Rows.Add(new object[] { e.Row.Cells["WorkCenterCode"].Value, e.Row.Cells["WorkCenterName"].Value, e.Row.Cells["WorkCenterLineCode"].Value, e.Row.Cells["WorkCenterLineName"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtWorkCenterOPCode_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtWorkCenterOPName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }
    }
}
