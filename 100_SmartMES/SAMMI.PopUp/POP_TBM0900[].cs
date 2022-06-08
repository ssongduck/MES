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
    public partial class POP_TBM0900 : Form
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

        public POP_TBM0900(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [저장위치 코드 및 명 Parameter Show]
                switch (i)
                {
                    case 0:
                        cboPlantCode_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); // 사업장(공장) 
                        break;
                    case 1:
                        cboWHCode_H.Value = argument[1].ToUpper() == "" ? "ALL" : argument[1].ToUpper(); // 창고코드
                        break;                        
                    case 2:
                        txtStorageLocCode.Text = argument[2].ToUpper(); //저장위치
                        break;

                    case 3:
                        txtStorageLocName.Text = argument[3].ToUpper(); // 저장위치명
                        break;

                    case 4:
                        cboStorageLocType_H.Value = argument[04].ToUpper() == "" ? "ALL" : argument[4].ToUpper(); //저장위치구분 
                        break;                  
                    
                    case 5:
                        cboUseFlag_H.Value = argument[5].ToUpper() == "" ? "ALL" : argument[5].ToUpper(); //사용여부
                        break;
                }
                #endregion
            }
        }

        private void POP_TBM0900_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장(공장)", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCodeNm", "사업장(공장)명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WHCode", "창고코드", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WHName", "창고", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "StorageLocCode", "저장위치 ", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "StorageLocName", "저장위치명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "StorageLocType", "저장위치구분", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null); 

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
            string sPlantCode = string.Empty;                           // 사업장(공장)
            string sWHCode = string.Empty;                              // 창고코드       
            string sStorageLocCode = txtStorageLocCode.Text.Trim();     // 저장위치       
            string sStorageLocName = txtStorageLocName.Text.Trim();     // 저장위치명     
            string sStorageLocType = string.Empty;                      // 저장위치구분  
            string sUseFlag = string.Empty;                             // 사용여 

            if (this.cboPlantCode_H.Value != null)
                sPlantCode = cboPlantCode_H.Value.ToString() == "ALL" ? "" : cboPlantCode_H.Value.ToString();

            if (this.cboWHCode_H.Value != null)
                sWHCode = cboWHCode_H.Value.ToString() == "ALL" ? "" : cboWHCode_H.Value.ToString();

            if (this.cboStorageLocType_H.Value != null)
                sStorageLocType = cboStorageLocType_H.Value.ToString() == "ALL" ? "" : cboStorageLocType_H.Value.ToString();

            if (this.cboUseFlag_H.Value != null)
                sUseFlag = cboUseFlag_H.Value.ToString() == "ALL" ? "" : cboUseFlag_H.Value.ToString();

            _DtTemp = _biz.SEL_TBM0900(sPlantCode, sWHCode, sStorageLocCode, sStorageLocName, sStorageLocType, sUseFlag); 


            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("StorageLocCode", typeof(string));
            TmpDt.Columns.Add("StorageLocName", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["StorageLocCode"].Value, e.Row.Cells["StorageLocName"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtStorageLocCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtStorageLocName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
