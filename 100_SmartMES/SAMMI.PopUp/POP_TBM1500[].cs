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
using SAMMI.Windows.Forms;
using Infragistics.Win.UltraWinGrid;


namespace SAMMI.PopUp
{
    public partial class POP_TBM1500 : SAMMI.Windows.Forms.BaseForm
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
        Common.Common _Common = new Common.Common();
        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        #endregion

        public POP_TBM1500(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];
                // PlantCode, inspCase, inspType, inspCode, inspName, UseFlag, TextBox1, TextBox2
                //_biz.TBM1500_POP(arr[0], arr[1], arr[2], sValueCode, sValueName, arr[3], tCodeBox, tNameBox);

                #region [검사항목 코드 및 명 Parameter Show]
                switch (i)
                {
                    case 0: //공정코드
                        //cboPlantCode_H.Value = argument[5].ToUpper() == "" ? "ALL" : argument[5].ToUpper(); //사용여부
                        cboPlantCode_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //사용여부
                        break;

                    case 1:  //검사구분
                        //cboInspCase_H.Value = argument[1].ToUpper() == "" ? "ALL" : argument[1].ToUpper(); //검사구분
                        //cbo_savecode.Value = argument[1].ToUpper() == "" ? "ALL" : argument[1].ToUpper(); //검사구분
                        break;
                        
                    case 2: //검사대상
                       txtInspCode.Text = argument[2].ToUpper(); //검사대상
                        break;

                    case 3: //검사항목코드
                        txtInspName.Text = argument[3].ToUpper(); //검사항목코드
                        break;
                    
                    case 4: //검사항목
                        //txtInspName.Text = argument[4].ToUpper(); //검사항목
                        break;

                    case 5: //사용여부
                        //cboUseFlag_H.Value = argument[5].ToUpper() == "" ? "ALL" : argument[5].ToUpper(); //사용여부
                        break;

                }
                #endregion
            }
        }

        private void POP_TBM1500_Load(object sender, EventArgs e)
        {
           // MessageBox.Show("폼로드");

           // _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitializeGrid(this.Grid1, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "InspCase", "검사구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "InspCaseNm", "구분명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "InspCode",   "검사항목코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "InspName",   "검사항목", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UnitCode",   "단위", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "InspDesc", "검사항목상세", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "InspType", "검사대상", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "InspTypeNm", "유형명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag", "사용유무", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            

            _GridUtil.SetInitUltraGridBind(Grid1);


            Grid1.DisplayLayout.Override.RowSelectorWidth = 1;
            if (argument[0].ToUpper() == "")
            {
                cboInspCase_H.Enabled = true;
                groupBox2.Visible = false;
            }

            rtnDtTemp = _Common.GET_TBM1300_CODE("Y");  //단위
            SAMMI.Common.Common.FillComboboxMaster(this.cbo_unittype, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "", "");

            rtnDtTemp = _Common.GET_TBM0000_CODE("INSPTYPE");     //PCTYPE
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "InspType", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");     //PCTYPE
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");     //PCTYPE
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");
            cbo_unittype.SelectedValue = 0;


            

            search();
         
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            search();
        }

        private void search()
        {
            string RS_CODE = string.Empty, RS_MSG = string.Empty;
            string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            string sInspCase = string.Empty;               // 검사구분(수입, 자주, 공정,…..)
            string sInspType = string.Empty;               // 검사대상(원자재, 자재, 반제품,…. )
            string sInspCode = txtInspCode.Text.Trim();    // 검사항목코드   
            string sInspName = txtInspName.Text.Trim();    // 검사항목 
            string sUseFlag = string.Empty;               // 사용여부 


            sInspCase = SqlDBHelper.nvlString(this.cboInspCase_H.Value);
            sInspType = SqlDBHelper.nvlString(this.cboInspType_H.Value);
            sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("INSPCASE");  //
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "InspCase", rtnDtTemp, "CODE_ID", "CODE_NAME");
          
            _DtTemp = _biz.SEL_TBM1500(sPlantCode, sInspCase, sInspType, sInspCode, sInspName, sUseFlag);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            
            //TmpDt.Columns.Add("InspCode", typeof(string));
            //TmpDt.Columns.Add("InspName", typeof(string));

            foreach (DataColumn dc in ((DataTable)Grid1.DataSource).Columns)
            {
                TmpDt.Columns.Add(dc.ColumnName, dc.DataType);
                
            }

            DataRow dr = TmpDt.NewRow();
            foreach (DataColumn dc in TmpDt.Columns)
            {
                dr[dc.ColumnName] = e.Row.Cells[dc.ColumnName].Value;
            }

            TmpDt.Rows.Add(dr);

            //TmpDt.Rows.Add(new object[] { e.Row.Cells["InspCode"].Value, e.Row.Cells["InspName"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txt_savetext.Text == string.Empty )
            {
                DialogForm dialogform;
                dialogform = new DialogForm("검사항목명을 등록하여 주십시오.",DialogForm.DialogType.OK);
                dialogform.ShowDialog();

                txt_savetext.Focus();
                return;
            }
            insert();
        }

        private void insert()
        {
           
     
            //텍스트 박스
            string I_InspCode   = SqlDBHelper.nvlString(cbo_savecode.Value);
            string I_Unit       = SqlDBHelper.nvlString(cbo_unittype.SelectedValue);
            string I_InspName   = string.Empty;
            string I_Maker      = string.Empty;
            string RS_CODE = string.Empty;
            string RS_MSG = string.Empty;
       
            if (this.txtInspName.Text != null)
                I_InspName = txt_savetext.Text.ToString();

            I_Maker = SAMMI.Common.LoginInfo.UserID;

             _biz.INS_TBM1500(I_InspCode, I_InspName,I_Maker,I_Unit,ref RS_CODE,ref RS_MSG);

            if (RS_CODE == "S")
            {
                 DialogForm dialogform;
                 dialogform = new DialogForm("C:R00005",DialogForm.DialogType.OK);
                 dialogform.ShowDialog();
                 txt_savetext.Text = string.Empty;
                 search();
                 cbo_unittype.SelectedValue = 0;

             }

        }

        private void txtInspCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtInspName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
