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
    public partial class POP_TBM0101 : Form
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

        public POP_TBM0101(string[] param)
        {
            InitializeComponent();


            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [품목코드 및 명 Parameter Show]
                switch (i)
                {
                    case 0:     //PlantCode
                        this.cboPlantCode_H.Text = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //plant
                        break;
                    
                    case 1:     //WorkCenterCode
                        txtWorkCenterCode.Text = argument[1].ToUpper(); //작업장코드
                        break;

                    case 2:     //WorkCenterName
                        txtWorkCenterName.Text = argument[2].ToUpper(); //작업장명
                        break;

                    case 3:     //ItemCode
                        txtItemCode.Text = argument[3].ToUpper(); //품목코드
                        break;

                    case 4:     //ItemName
                        txtItemName.Text = argument[4].ToUpper(); //품목명
                        break;

                    case 5:     //Item_type
                        cboItemType_H.Text = argument[5].ToUpper(); //품목유형
                        break;
                }
                #endregion
            }

            GridInit();
            search();
            
        }

        private void GridInit()
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterName", "작업장", false, GridColDataType_emu.VarChar, 160, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "itemname", "품명", false, GridColDataType_emu.VarChar, 300, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ItemType", "품목유형", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "CarType", "차종", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(Grid1);

            _rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", _rtnDtTemp, "CODE_ID", "CODE_NAME");


        }

        private void search()
        {
            string RS_CODE = string.Empty, RS_MSG = string.Empty;
            string splantcd = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            string sitemtype = SqlDBHelper.nvlString(cboItemType_H.Value);
            string sWorkCenterCode = txtWorkCenterCode.Text.Trim();
            string sWorkCenterName = txtWorkCenterName.Text.Trim();
            string sitem_cd = txtItemCode.Text.Trim();
            string sitem_name = txtItemName.Text.Trim();

            //splantcd = LoginInfo.UserPlantCode;
            sitemtype = SqlDBHelper.nvlString(cboItemType_H.Value);

            _DtTemp = _biz.SEL_TBM0101(splantcd, sitem_cd, sitem_name, sWorkCenterCode, sWorkCenterName, sitemtype);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            search();
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
    }
}