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
    public partial class POP_TBM2500 : Form
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string _sPlantCode = string.Empty;

        private string _sItemCode = string.Empty;

        private string _sComponent = string.Empty;
        #endregion

        public POP_TBM2500(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [불량코드 및 명 Parameter Show]
                switch (i)
                {
                    case 0:  //불량구분
                        cboErrorType_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //불량구분
                        break;

                    case 1:  //불량유형
                        cboErrorClass_H.Value = argument[1].ToUpper() == "" ? "ALL" : argument[1].ToUpper(); //불량유형
                        break;
                        
                    case 2: //불량코드
                        txtErrorCode.Text = argument[2].ToUpper(); //불량코드
                        break;

                    case 3: //불량명
                        txtErrorDesc.Text = argument[3].ToUpper(); //불량명
                        break;
                    
                    case 4:  //사용여부
                        cboUseFlag_H.Value = argument[4].ToUpper() == "" ? "ALL" : argument[4].ToUpper(); //사용여부
                        break;

                    case 5:  //사업장
                        _sPlantCode = argument[5].ToUpper();
                        cboPlantCode_H.Value = argument[5].ToUpper() == "" ? "ALL" : argument[5].ToUpper();
                        break;

                    case 6:  //제품번
                        _sItemCode = argument[6].ToUpper();
                        break;

                    case 7:  //단품번
                        _sComponent = argument[7].ToUpper();
                        if (_sComponent == string.Empty) txtItemCode.Text = _sItemCode;
                        else txtItemCode.Text = _sComponent;
                        break;

                    case 8: // 사업장 Enable (단순히 사업장별 불량 유형 정보를 보고자 할경우 "TRUE", 고정하여 검색시 "FALSE")
                        try
                        {
                            cboPlantCode_H.Enabled = Convert.ToBoolean(argument[8]);
                            txtItemCode.Enabled = Convert.ToBoolean(argument[8]);
                        }
                        catch
                        {
                            cboPlantCode_H.Enabled = true;
                            txtItemCode.Enabled = true;
                        }
                        break;
                }
                #endregion
            }
        }

        private void POP_TBM2500_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "ErrorType", "불량구분", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ErrorTypeNm", "구분명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ErrorClass", "불량유형", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ErrorClassNm", "유형명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ErrorCode", "불량코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ErrorDesc", "불량명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);   

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
            string sErrorType = string.Empty;              // 불량구분
            string sErrorClass = string.Empty;              // 불량유형
            string sErrorCode = txtErrorCode.Text.Trim();  // 불량코드  
            string sErrorDesc = txtErrorDesc.Text.Trim();  // 불량명 
            string sUseFlag = string.Empty;              // 사용여부  

            if (this.cboErrorType_H.Value != null)
                sErrorType = cboErrorType_H.Value.ToString() == "ALL" ? "" : cboErrorType_H.Value.ToString();      // 불량구분 

            if (this.cboErrorClass_H.Value != null)
                sErrorClass = cboErrorClass_H.Value.ToString() == "ALL" ? "" : cboErrorClass_H.Value.ToString();   // 불량유형

            if (this.cboUseFlag_H.Value != null)
                sUseFlag = cboUseFlag_H.Value.ToString() == "ALL" ? "" : cboUseFlag_H.Value.ToString();            // 사용여부 

            _DtTemp = _biz.SEL_TBM2500(sErrorType, sErrorClass, sErrorCode, sErrorDesc, sUseFlag, _sPlantCode, _sItemCode, _sComponent);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("ErrorCode", typeof(string));
            TmpDt.Columns.Add("ErrorDesc", typeof(string));
            TmpDt.Columns.Add("ErrorClass", typeof(string));
            TmpDt.Columns.Add("ErrorType", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["ErrorCode"].Value, e.Row.Cells["ErrorDesc"].Value
                , e.Row.Cells["ErrorClass"].Value, e.Row.Cells["ErrorType"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtErrorCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtErrorDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
