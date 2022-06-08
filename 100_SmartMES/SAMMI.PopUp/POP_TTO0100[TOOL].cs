using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;

using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.Windows.Forms;

namespace SAMMI.PopUp
{
    public partial class POP_TTO0100 : SAMMI.Windows.Forms.BaseForm
    {
        string[] argument;
        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        public bool bDataRow = false;
        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        #endregion

        public POP_TTO0100(string[] param)
        {
            InitializeComponent();
            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [품목코드 및 명 Parameter Show]
                switch (i)
                {
                    //sCode, sName, sPlantCode, sSeq, sEquip, sUseInfo
                    case 0:
                        txtToolCode.Text = argument[0].ToUpper(); //TOOL코드
                        break;
                    case 1:
                        txtToolName.Text = argument[1].ToUpper(); //TOOL명
                        break;
                    case 2:
                        cboPlantCode_H.SelectedValue = argument[2].ToUpper(); //사업장 코드
                        break;
                    case 3:
                        txtSeq.Text = argument[3].ToUpper(); // 순번
                        break;
                    case 4:
                        cboUse.SelectedValue = argument[4].ToUpper(); // 사용 여부
                        break;
                    case 5:
                        cboEquip.SelectedValue = argument[5].ToUpper(); // 장착 여부
                        break;
                    case 6:
                        bDataRow = argument[6].ToUpper() == "Y";
                        break;
                }
                #endregion
            }
        }

        private void POP_TTO0100_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1, false, false, false, "", false);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ToolCode",  "툴 코드", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ToolName",  "툴명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "Seq",       "순번", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "InDate",    "최초 작창일", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ProdQty",   "사용량", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "Shelflife", "수명", false, GridColDataType_emu.Integer, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseRate",   "Life 잔량", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "EquipDate", "장착일", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "Equip",     "장착", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(Grid1);

            #region 콤보박스
            DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");


            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");  //사용여부
            SAMMI.Common.Common.FillComboboxMaster(this.cboUse, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

            rtnDtTemp = _Common.GET_TBM0000_CODE("EQUIP");  //장착여부
            SAMMI.Common.Common.FillComboboxMaster(this.cboEquip, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

            #endregion

            search();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            search();
        }
        private void search()
        {
            string sPlantCode = SqlDBHelper.gGetCode(cboPlantCode_H.SelectedValue);
            string sUse = SqlDBHelper.gGetCode(cboUse.SelectedValue);
            string sEquip = SqlDBHelper.gGetCode(cboEquip.SelectedValue);

            _DtTemp = _biz.SEL_TTO0100(txtToolCode.Text, txtToolName.Text, sPlantCode, txtSeq.Text, sEquip, sUse);

            //_DtTemp = _biz.SEL_TBM5200(txtToolCode ,txtToolName,Major);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }

        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            if (bDataRow)
            {
                if (SqlDBHelper.nvlString(e.Row.Cells["Equip"].Value) == "Y")
                {
                    DialogForm sd = new DialogForm("C:R00119", DialogForm.DialogType.OK);
                    sd.ShowDialog();
                    return;
                }
            }

            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("PlantCode", typeof(string));
            TmpDt.Columns.Add("ToolCode", typeof(string));
            TmpDt.Columns.Add("ToolName", typeof(string));
            TmpDt.Columns.Add("Seq", typeof(string));
            TmpDt.Columns.Add("InDate", typeof(string));
            TmpDt.Columns.Add("ProdQty", typeof(int));
            TmpDt.Columns.Add("Shelflife", typeof(int));
            TmpDt.Columns.Add("UseRate", typeof(string));
            TmpDt.Columns.Add("EquipDate", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["PlantCode"].Value
                                    , e.Row.Cells["ToolCode"].Value
                                    , e.Row.Cells["ToolName"].Value
                                    , e.Row.Cells["Seq"].Value
                                    , e.Row.Cells["InDate"].Value
                                    , e.Row.Cells["ProdQty"].Value
                                    , e.Row.Cells["Shelflife"].Value
                                    , e.Row.Cells["UseRate"].Value
                                    , e.Row.Cells["EquipDate"].Value});

            this.Tag = TmpDt;
            this.Close();
            
        }

        private void txtToolCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtToolName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtSeq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }
    }
}
