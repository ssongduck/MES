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
    public partial class POP_TBM0800 : Form
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

        public POP_TBM0800(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [창고 및 명 Parameter Show]
                switch (i)
                {
                    case 0:
                        cboPlantCode_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //plant
                        break;
                        
                    case 1:
                        txtWHCode.Text = argument[1].ToUpper(); //창고코드
                        break;

                    case 2:
                        txtWHName.Text = argument[2].ToUpper(); //창고명
                        break;

                    
                    case 3:
                        cboBaseWHFlag_H.Value = argument[3].ToUpper() == "" ? "ALL" : argument[3].ToUpper(); //기본창고여부
                        break;

                    
                    case 4:
                        cboProdWHFlag_H.Value = argument[4].ToUpper() == "" ? "ALL" : argument[4].ToUpper(); //제품창고여부
                        break;

                    
                    case 5:
                        cboMetWHFlag_H.Value = argument[5].ToUpper() == "" ? "ALL" : argument[5].ToUpper(); //자재창고여부
                        break;

                    case 6:
                        cboUseFlag_H.Value = argument[6].ToUpper() == "" ? "ALL" : argument[6].ToUpper(); //사용여부
                        break;
                }
                #endregion
            }
        }

        private void POP_TBM0400_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "공장코드", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCodeNm", "공장명", false, GridColDataType_emu.VarChar,100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WHCode", "창고코드", false, GridColDataType_emu.VarChar, 50,100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WHName", "창고명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "BaseWHFlag", "기본창고여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ProdWHFlag", "제품창고여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MetWHFlag", "자재창고여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null); 

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
            string sPlantCode = string.Empty;               // 공장코드
            string sWHCode = txtWHCode.Text.Trim();      // 창고코드  
            string sWHName = txtWHName.Text.Trim();      // 창고명   
            string sBaseWHFlag = string.Empty;               // 기본창고여부
            string sProdWHFlag = string.Empty;               // 제품창고여부
            string sMetWHFlag = string.Empty;               // 자재창고여부
            string sUseFlag = string.Empty;               // 사용여부


            if (this.cboPlantCode_H.Value != null)
                sPlantCode = cboPlantCode_H.Value.ToString() == "ALL" ? "" : cboPlantCode_H.Value.ToString();

            if (this.cboBaseWHFlag_H.Value != null)
                sBaseWHFlag = cboBaseWHFlag_H.Value.ToString() == "ALL" ? "" : cboBaseWHFlag_H.Value.ToString();

            if (this.cboProdWHFlag_H.Value != null)
                sProdWHFlag = cboProdWHFlag_H.Value.ToString() == "ALL" ? "" : cboProdWHFlag_H.Value.ToString();

            if (this.cboMetWHFlag_H.Value != null)
                sMetWHFlag = cboMetWHFlag_H.Value.ToString() == "ALL" ? "" : cboMetWHFlag_H.Value.ToString();

            if (this.cboUseFlag_H.Value != null)
                sUseFlag = cboUseFlag_H.Value.ToString() == "ALL" ? "" : cboUseFlag_H.Value.ToString();

            _DtTemp = _biz.SEL_TBM0800(sPlantCode, sWHCode, sWHName, sBaseWHFlag, sProdWHFlag, sMetWHFlag, sUseFlag);


            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("WHCode", typeof(string));
            TmpDt.Columns.Add("WHName", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["WHCode"].Value, e.Row.Cells["WHName"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtWHCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtWHName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
