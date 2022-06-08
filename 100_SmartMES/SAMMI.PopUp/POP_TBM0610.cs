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
    public partial class POP_TBM0610 : Form
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();
        BizTextBoxManagerEX btbManager;
        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        //public string sWorkCenterCode = string.Empty;
        #endregion

        public POP_TBM0610(string[] param)
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
                    case 0:
                        cboPlantCode_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //plant
                        break;

                    case 1:
                        txtWorkCenterCode.Text = argument[1].ToUpper();
                        break;

                    case 2:
                        txtWorkCenterOPCode.Text = argument[2].ToUpper(); //작업장 OP 코드
                        break;

                    case 3:
                        txtWorkCenterOPName.Text = argument[3].ToUpper(); //작업장 OP 명
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
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 250, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterOPCode", "작업장OP코드", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterOPName", "작업장OP명", false, GridColDataType_emu.VarChar, 250, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
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
            string sWorkCenterOPCode = txtWorkCenterOPCode.Text.Trim();
            string sWorkCenterOPName = txtWorkCenterOPName.Text.Trim();
            string sWorkCenterCode = txtWorkCenterCode.Text.Trim();

            sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");  // 사용유무
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");


            _DtTemp = _biz.SEL_TBM0610(sPlantCode, sWorkCenterOPCode, sWorkCenterOPName, sWorkCenterCode, sUseFlag);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }

        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("WorkCenterCode", typeof(string));
            TmpDt.Columns.Add("WorkCenterName", typeof(string));
            TmpDt.Columns.Add("WorkCenterOPCode", typeof(string));
            TmpDt.Columns.Add("WorkCenterOPName", typeof(string));


            TmpDt.Rows.Add(new object[] { e.Row.Cells["WorkCenterCode"].Value, e.Row.Cells["WorkCenterName"].Value, e.Row.Cells["WorkCenterOPCode"].Value, e.Row.Cells["WorkCenterOPName"].Value });

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
