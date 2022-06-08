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
    public partial class POP_TBM3700 : Form
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

        public POP_TBM3700(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [운행치량 코드 및 명 Parameter Show]
                switch (i)
                {
                    case 0:  //차량구분
                        cboCarGubun_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //plant
                        break;
                        
                    case 1: //차량번호
                        txtCarNo.Text = argument[1].ToUpper(); //작업장코드
                        break;

                    case 2:  //차량내역
                        txtCarDesc.Text = argument[2].ToUpper(); //작업장명
                        break;
                    
                    case 3:  //사용여부
                        cboUseFlag_H.Value = argument[3].ToUpper() == "" ? "ALL" : argument[3].ToUpper(); //사용여부
                        break;
                }
                #endregion
            }
        }

        private void POP_TB3700_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);
            _GridUtil.InitColumnUltraGrid(Grid1, "CarGubun", "구분", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "CarGubunNm", "구분명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "CarNo", "차량번호", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "CarDesc", "차량내역", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "CustCode", "용차업체", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag", "사용유무", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

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
            string sCarGubun = string.Empty;            // 구분
            string sCarNo = txtCarNo.Text.Trim();   // 차량번호
            string sCarDesc = txtCarDesc.Text.Trim(); // 용차업체
            string sUseFlag = string.Empty;           // 사용여부  

            if (this.cboCarGubun_H.Value != null)
                sCarGubun = cboCarGubun_H.Value.ToString() == "ALL" ? "" : cboCarGubun_H.Value.ToString();

            if (this.cboUseFlag_H.Value != null)
                sUseFlag = cboUseFlag_H.Value.ToString() == "ALL" ? "" : cboUseFlag_H.Value.ToString();

            _DtTemp = _biz.SEL_TBM3700(sCarGubun, sCarDesc, sCarDesc, sUseFlag);



            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("CarNo", typeof(string));
            TmpDt.Columns.Add("CarDesc", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["CarNo"].Value, e.Row.Cells["CarDesc"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtCarNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtCarDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
