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
    public partial class POP_TBM0700 : Form
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

        public POP_TBM0700(string[] param)
        {
            InitializeComponent();
            //cboUseFlag_H.Value = "Y";
            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];
                #region [설비코드 및 명 Parameter Show]
                switch (i)
                {
                    //_biz.TBM0700_POP_Grid(aParam1[0], sValueCode, sValueName, aParam1[1], aParam1[2], aParam1[3], aParam1[4], grid, sCode, sName);
                    //gridManager.PopUpAdd("MachCode", "MachName", "TBM0700", new string[] { "", "PlantCode", "", "", "" });
                    case 1:
                        cboPlantCode_H.Text  = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); ; //설비코드
                        break;
                        
                    case 2:
                        txtMachCode.Text = argument[1].ToUpper(); //설비명
                        break;

                    case 3:
                        txtMachName.Text= argument[2].ToUpper(); // 
                        break;

                    case 4: 
                        cboMachType_H.Value = argument[3].ToUpper() == "" ? "ALL" : argument[3].ToUpper(); //설비타입
                        break;
                    case 5: 
                        cboMachType1_H.Value = argument[4].ToUpper() == "" ? "ALL" : argument[3].ToUpper(); //설비붕류1
                        break;
                    case 6: 
                        cboMachType2_H.Value = argument[5].ToUpper() == "" ? "ALL" : argument[4].ToUpper(); //설비분류2
                        break;
                    case 7:
                        cboUseFlag_H.Value = argument[6].ToUpper() == "" ? "Y" : argument[5].ToUpper(); //사용여부
                        break;
                   // case 8 :
                   //     txtPlantCode.Text = argument[0].ToUpper() ; // 
                    //    break;
                }
                #endregion
            }
        }

        private void POP_TBM0700_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 200, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterCode", "작업장명", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterName", "작업장명", true, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "OPName", "작업장OP", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "Line", "가공라인", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MachType", "설비타입", false, GridColDataType_emu.VarChar, 200, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MachTypeNm", "설비유형", false, GridColDataType_emu.VarChar,100, 40, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MachType1", " 분류1", false, GridColDataType_emu.VarChar, 200, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MachType1Nm", "분류1명", false, GridColDataType_emu.VarChar, 100, 40, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MachType2", "분류2", false, GridColDataType_emu.VarChar, 200, 20, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MachType2Nm", "분류2명", false, GridColDataType_emu.VarChar, 100, 40, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "MachCode", "설비코드", false, GridColDataType_emu.VarChar, 110, 20, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "Machname", "설비명", false, GridColDataType_emu.VarChar, 200, 40, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 80, 20, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(Grid1);
            _rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", _rtnDtTemp, "CODE_ID", "CODE_NAME");

            search();
         
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            search();
        }

        private void search()
        {
            string RS_CODE = string.Empty, RS_MSG = string.Empty;
            string sPlantCode = string.Empty;
            string sMachCode = txtMachCode.Text.Trim();        // 설비(장비)코드 
            string sMachname = txtMachName.Text.Trim();        // 설비명 
            string sMachType = string.Empty;                   // 설비타입 
            string sMachType1 = string.Empty;                   // 분류1
            string sMachType2 = string.Empty;                   // 분류2
            string sUseFlag = string.Empty;                   // 사용여부


            sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sMachType = SqlDBHelper.nvlString(this.cboMachType_H.Value);
            sMachType1 = SqlDBHelper.nvlString(this.cboMachType1_H.Value);
            sMachType2 = SqlDBHelper.nvlString(this.cboMachType2_H.Value);
            sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");  // 사용유무
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");


            _DtTemp = _biz.SEL_TBM0700(sMachCode, sMachname, sPlantCode, sMachType, sMachType1, sMachType2, sUseFlag);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("MachCode", typeof(string));
            TmpDt.Columns.Add("MachName", typeof(string));
            TmpDt.Columns.Add("WorkCenterCode", typeof(string));
            TmpDt.Columns.Add("WorkCenterName", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["MachCode"].Value, e.Row.Cells["MachName"].Value, e.Row.Cells["WorkCenterCode"].Value, e.Row.Cells["WorkCenterName"].Value });

            this.Tag = TmpDt;
            this.Close();
        }

        private void txtMachCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtMachName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
