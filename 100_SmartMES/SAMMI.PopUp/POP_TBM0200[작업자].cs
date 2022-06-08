using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SAMMI.Common;
using SAMMI.PopUp;



namespace SAMMI.PopUp
{
    public partial class POP_TBM0200 : Form
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        ////비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        string div = "0";
        #endregion

        public POP_TBM0200(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                argument[i] = param[i];

                #region [작업자 및 명 Parameter Show 7=0 사업장, 1, 공정코드, 2,라인코드, 3,작업장, 4, 작업자ID, 5. 작업자, 6.사용여부]
                switch (i)
                {
                    case 0: //사업장
                        cboPlantCode_H.Value = argument[0].ToUpper() == "" ? "ALL" : argument[0].ToUpper(); //plant
                        break;

                    //case 1: //공정코드
                    //    cboOPCode_H.Value = argument[1].ToUpper() == "" ? "ALL" : argument[1].ToUpper(); //OPCode
                    //    break;
                    //case 2: //라인코드
                    //    cboLineCode_H.Value = argument[2].ToUpper() == "" ? "ALL" : argument[2].ToUpper(); //LineCode
                    //    break;
                    //case 3:  //작업장
                    //    cboWorkCenterCode_H.Value = argument[3].ToUpper() == "" ? "ALL" : argument[3].ToUpper(); //WorkCenterCode
                    //    break;
                    case 4: //작업자 ID
                        txtWorkerID.Text = argument[4].ToUpper(); //작업장명
                        break;
                    case 5: //작업자
                        txtWorkerName.Text = argument[5].ToUpper(); //작업장명
                        break;
                    case 6: //사용여부
                        cboUseFlag_H.Value = argument[6].ToUpper() == "" ? "ALL" : argument[6].ToUpper(); //사용여부
                        break;
                    case 7: //사용여부
                        div= argument[7]; // 0=ALL,1=공무,2=금형
                        break;
                }
                #endregion
            }
        }

        private void POP_TBM0200_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1,"PlantCodeNm",   "공장명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "OPCode", "공정", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1,"LineCode",      "라인", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1,"WorkerID",      "작업자ID", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WorkerName",   "작업자명", false, GridColDataType_emu.VarChar, 200, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);

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
            string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);
            string sOPCode = ""; // cboOPCode_H.Value.ToString() == "ALL" ? "" : cboOPCode_H.Value.ToString();            // 공정코드
            string sLineCode = ""; // cboLineCode_H.Value.ToString() == "ALL" ? "" : cboLineCode_H.Value.ToString();            // 라인코드
            string sWorkCenterCode = ""; // cboWorkCenterCode_H.Value.ToString() == "ALL" ? "" : cboWorkCenterCode_H.Value.ToString();  // 작업장코드
            string sWorkerID = txtWorkerID.Text.Trim();                                                                  // 작업자 ID
            string sWorkerName = txtWorkerName.Text.Trim();                                                                // 작업자명
            string sUseFlag = SqlDBHelper.nvlString(cboUseFlag_H.Value);

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
          

            _DtTemp = _biz.SEL_TBM0200(sPlantCode, sOPCode, sLineCode, sWorkCenterCode, sWorkerID, sWorkerName, sUseFlag,div);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }
 
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("WorkerID", typeof(string));
            TmpDt.Columns.Add("WorkerName", typeof(string));

            if (ModifierKeys == Keys.Control)
            {
                string wid = "";
                string wname = "";
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ur in Grid1.Rows)
                {
                    if (ur.Selected)
                    {
                        wid += ur.Cells["WorkerID"].Value.ToString() + ",";
                        wname += ur.Cells["WorkerName"].Value.ToString() + ",";
                    }
                }
                if (wid != "")
                {
                    wid = wid.Substring(0, wid.Length - 1);
                    wname = wname.Substring(0, wname.Length - 1);
                }
                TmpDt.Rows.Add(new object[] { wid, wname });


            }
            else
            {
                TmpDt.Rows.Add(new object[] { e.Row.Cells["WorkerID"].Value, e.Row.Cells["WorkerName"].Value });
            }
            this.Tag = TmpDt;
            this.Close();
        }

        private void txtWorkerID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }

        private void txtWorkerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                search();
            }
        }


    }
}
