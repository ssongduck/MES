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
    public partial class POP_TBM0600 : Form
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

        public POP_TBM0600(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [WorkCenter코드 및 명 Parameter Show]
                switch (i)
                {
                    case 0:
                        cboPlantCode_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //plant
                        break;
                        
                    case 1:
                        txtWorkCenterCode.Text = argument[1].ToUpper(); //작업장코드
                        break;

                    case 2:
                        txtWorkCenterName.Text = argument[2].ToUpper(); //작업장명
                        break;
                    
                    case 3:
                        cboOPCode_H.Value = argument[3].ToUpper() == "" ? "ALL" : argument[3].ToUpper(); //공정
                        break;

                    case 4:
                        cboLineCode_H.Value = argument[4].ToUpper() == "" ? "ALL" : argument[4].ToUpper(); //라인
                        break;

                    case 5:
                        cboUseFlag_H.Value = argument[5].ToUpper() == "" ? "ALL" : argument[5].ToUpper(); //사용여부
                        break;
                }
                #endregion
            }
        }

        private void POP_TBM0600_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "공장코드", false, GridColDataType_emu.VarChar, 80, 50, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCodeNm", "공장명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "OPCode", "공정명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "LineCode", "라인코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterCode", "작업장코드", false, GridColDataType_emu.VarChar, 100, 50, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 220, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "OutProcFlag", "외주구분", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "UseFlag", "사용여부", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);      

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
            string sPlantCode = string.Empty;                            // 사업장(공장)코드  
            string sWorkCenterCode = txtWorkCenterCode.Text.Trim();      // WorkCenter코드
            string sWorkCenterName = txtWorkCenterName.Text.Trim();      // WorkCenter명
            string sOPCode = string.Empty;                               // 공정코드
            string sLineCode = string.Empty;                             // 라인  
            string sUseFlag = string.Empty;                              // 사용여부

            sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
            sOPCode = SqlDBHelper.nvlString(this.cboOPCode_H.Value);
            sLineCode = SqlDBHelper.nvlString(this.cboLineCode_H.Value);
            sUseFlag = SqlDBHelper.nvlString(this.cboUseFlag_H.Value);
            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_TBM0000_CODE("USEFLAG");  // 사용유무
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "UseFlag", rtnDtTemp, "CODE_ID", "CODE_NAME");


               
            _DtTemp = _biz.SEL_TBM0600(sPlantCode,sWorkCenterCode,sWorkCenterName, sOPCode, sLineCode, sUseFlag);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }

        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            try
            {
                DataTable TmpDt = new DataTable();
                TmpDt.Columns.Add("WorkCenterCode", typeof(string));
                TmpDt.Columns.Add("WorkCenterName", typeof(string));
                TmpDt.Columns.Add("OPCode", typeof(string));
                TmpDt.Columns.Add("OPName", typeof(string));
                TmpDt.Columns.Add("LineCode", typeof(string));
                TmpDt.Columns.Add("LineName", typeof(string));

                string sOPCode = SqlDBHelper.nvlString(e.Row.Cells["OPCode"].Value);

                string sOPName = SqlDBHelper.gGetName(e.Row.Cells["OPCode"].Value);
                sOPCode = SqlDBHelper.gGetCode(e.Row.Cells["OPCode"].Value);

                string sLineName = SqlDBHelper.gGetName(e.Row.Cells["LineName"].Value);
                string sLineCode = SqlDBHelper.gGetName(e.Row.Cells["LineCode"].Value);

                TmpDt.Rows.Add(new object[] { SqlDBHelper.nvlString(e.Row.Cells["WorkCenterCode"].Value)
                    , SqlDBHelper.nvlString(e.Row.Cells["WorkCenterName"].Value), sOPCode, sOPName, sLineCode, sLineName });

                this.Tag = TmpDt;
                this.Close();
            }
            catch(Exception ex)
            {

            }
        }

        private void txtWorkCenterCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtWorkCenterName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }
    }
}
