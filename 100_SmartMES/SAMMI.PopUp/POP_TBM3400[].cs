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
    public partial class POP_TBM3400 : Form
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

        public POP_TBM3400(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [고장항목코드 및 명 Parameter Show]
                switch (i)
                {
                    case 0:
                        cboFaultType_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //고장유형
                        break;
                        
                    case 1:
                        txtFaultCode.Text = argument[1].ToUpper(); //고장항목코드
                        break;

                    case 2:
                        txtFaultName.Text = argument[2].ToUpper(); //고장명
                        break;
                    
                    case 3:
                        cboUseFlag_H.Value = argument[3].ToUpper() == "" ? "ALL" : argument[3].ToUpper(); //사용여부
                        break;
                }
                #endregion
            }
        }

        private void POP_TBM3400_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "FaultType", "유형코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "FaultTypeNm", "유형명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "FaultCode", "항목코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "FaultName", "고장명", false, GridColDataType_emu.VarChar, 250, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag", "사용유무", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);

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
            string sFaultType = string.Empty;              // 고장유형
            string sFaultCode = txtFaultCode.Text.Trim();  // 고장코드
            string sFaultName = txtFaultName.Text.Trim();  // 고장명
            string sUseFlag = string.Empty;              // 사용여부  

            if (this.cboFaultType_H.Value != null)
                sFaultType = cboFaultType_H.Value.ToString() == "ALL" ? "" : cboFaultType_H.Value.ToString();

            if (this.cboUseFlag_H.Value != null)
                sUseFlag = cboUseFlag_H.Value.ToString() == "ALL" ? "" : cboUseFlag_H.Value.ToString();

            _DtTemp = _biz.SEL_TBM3400(sFaultType, sFaultCode, sFaultName, sUseFlag);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("FaultCode", typeof(string));
            TmpDt.Columns.Add("FaultName", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["FaultCode"].Value, e.Row.Cells["FaultName"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtFaultCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtFaultName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
