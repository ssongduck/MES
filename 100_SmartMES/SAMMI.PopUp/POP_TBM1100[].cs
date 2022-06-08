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
    public partial class POP_TBM1100 : Form
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        Common.Common _Common = new Common.Common();
        #endregion

        public POP_TBM1100(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [비가동항목코드 및 명 Parameter Show]
                switch (i)
                {
                    case 0: //비가동구분
                        cboStopType_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //비가동구분
                        break;
                        
                    case 1: //비가동유형
                       cboStopClass_H.Value = argument[1].ToUpper() == "" ? "ALL" : argument[1].ToUpper();  //비가동유형
                        break;
                    
                    case 2: //비가동코드
                        txtPlantCode.Text = argument[2].ToUpper(); //비가동코드
                        break;
                    

                    case 3: //비가동코드
                        txtStopCode.Text = argument[3].ToUpper(); //비가동코드
                        break;
                    
                    case 4: //비가동명
                        txtStopDesc.Text = argument[4].ToUpper(); //작업장명
                        break;

                    case 5: //사용여부
                        cboUseFlag_H.Value = argument[5].ToUpper() == "" ? "ALL" : argument[4].ToUpper(); //사용여부
                        break;

                }
                #endregion
            }
        }

        private void POP_TBM1100_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);    
            _GridUtil.InitColumnUltraGrid(Grid1, "StopType",       "비가동구분", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);  
            _GridUtil.InitColumnUltraGrid(Grid1, "StopTypeNm",     "비가동구분명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);   
            _GridUtil.InitColumnUltraGrid(Grid1, "StopClass",      "비가동유형", false, GridColDataType_emu.VarChar, 50, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);  
            _GridUtil.InitColumnUltraGrid(Grid1, "StopClassNm",    "비가동유형명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);      
            _GridUtil.InitColumnUltraGrid(Grid1, "StopCode",       "비가동코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);  
            _GridUtil.InitColumnUltraGrid(Grid1, "StopDesc",       "비가동명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);    

            _GridUtil.SetInitUltraGridBind(Grid1);

            DataTable rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            search();
         
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            search();
        }

        private void search()
        {
            string RS_CODE = string.Empty, RS_MSG = string.Empty;
            string sPlantCode = txtPlantCode.Text.Trim();
            string sStopType = string.Empty;                // 비가동구분
            string sStopClass = string.Empty;                // 비가동유형
            string sStopCode = txtStopCode.Text.Trim();     // 비가동코드
            string sStopDesc = txtStopDesc.Text.Trim();    // 비가동명
            string sUseFlag = string.Empty;                // 사용여부

            if (this.cboStopType_H.Value != null)
                sStopType = cboStopType_H.Value.ToString() == "ALL" ? "" : cboStopType_H.Value.ToString();

            if (this.cboStopClass_H.Value != null)
                sStopClass = cboStopClass_H.Value.ToString() == "ALL" ? "" : cboStopClass_H.Value.ToString();

            if (this.cboUseFlag_H.Value != null)
                sUseFlag = cboUseFlag_H.Value.ToString() == "ALL" ? "" : cboUseFlag_H.Value.ToString();

            _DtTemp = _biz.SEL_TBM1100(sPlantCode, sStopType, sStopClass, sStopCode, sStopDesc, sUseFlag);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }           
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("StopCode", typeof(string));
            TmpDt.Columns.Add("StopDesc", typeof(string));
            TmpDt.Columns.Add("StopType", typeof(string));
            TmpDt.Columns.Add("StopClass", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["StopCode"].Value, e.Row.Cells["StopDesc"].Value, e.Row.Cells["StopType"].Value, e.Row.Cells["StopClass"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtStopCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtStopDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
