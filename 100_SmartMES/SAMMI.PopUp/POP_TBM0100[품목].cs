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
    public partial class POP_TBM0100 : Form
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

        public POP_TBM0100(string[] param)
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
                        cboItemType_H.Value = argument[1].ToUpper() == "" ? "ALL" : argument[1].ToUpper(); //제품타입
                    //    cboItemType_H.Value   = argument[1].ToUpper(); //타입
                        break;

                    case 2:
                        txtItemCode.Text   = argument[2].ToUpper(); //품목코드
                        break;
                    
                    case 3:
                        txtItemName.Text   = argument[3].ToUpper(); //품목명
                    break;
                }
                #endregion
            }
        }

        private void POP_TBM0100_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ItemCode",  "품번", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "itemname",  "품명", false, GridColDataType_emu.VarChar, 300, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ItemType",  "품목유형", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(Grid1);

            _rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", _rtnDtTemp, "CODE_ID", "CODE_NAME");

            search();
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            search();
        }

        private void search()
        {
            string RS_CODE = string.Empty, RS_MSG = string.Empty;
            string splantcd = string.Empty; //= cboPlantCode_H.Value.ToString();
            string sitemtype = string.Empty; // cboItemType_H.Value.ToString();
            string sitem_cd = txtItemCode.Text.Trim();
            string sitem_name = txtItemName.Text.Trim();

            splantcd = SqlDBHelper.nvlString(cboPlantCode_H.Value);
            sitemtype = SqlDBHelper.nvlString(cboItemType_H.Value);

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
          
            _DtTemp = _biz.SEL_TBM0100(splantcd, sitem_cd ,sitem_name , sitemtype);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("ItemCode", typeof(string));
            TmpDt.Columns.Add("itemname", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["ItemCode"].Value, e.Row.Cells["itemname"].Value });

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
