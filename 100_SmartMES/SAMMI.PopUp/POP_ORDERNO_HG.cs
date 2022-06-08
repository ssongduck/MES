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
   public partial class POP_ORDERNO_HG : Form
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

        public POP_ORDERNO_HG(string[] param)
        {
            InitializeComponent();

            argument = new string[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
               argument[i] = param[i];
            }
        }

        private void POP_ORDERNO_HG_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "RECDATE", "작업일자", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WORKCENTERCODE", "작업장코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "WORKCENTERNAME", "작업장", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "PLANNO", "작업지시번호", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ITEMCODE", "품목코드", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "ITEMNAME", "품목명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(Grid1, "PLANQTY", "계획수량", false, GridColDataType_emu.Double, 100, 100, Infragistics.Win.HAlign.Right, true, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(Grid1);

            _rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PLANTCODE", _rtnDtTemp, "CODE_ID", "CODE_NAME");
           
            Search();
         
        }

        private void Search()
        {
            string RS_CODE = string.Empty, RS_MSG = string.Empty;
            string sPLANTCODE = argument[0];
            string sRECDATE = argument[1];
            string sWORKCENTER = argument[2]; 

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common.Common _Common = new Common.Common();
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");  //사업장
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.Grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            _DtTemp = _biz.SEL_ORDERNO_HG(sPLANTCODE, sRECDATE, sWORKCENTER);

            Grid1.DataSource = _DtTemp;
            Grid1.DataBind();
        }

        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            DataTable TmpDt = new DataTable();
            TmpDt.Columns.Add("PLANNO", typeof(string));
            TmpDt.Columns.Add("ITEMNAME", typeof(string));

            TmpDt.Rows.Add(new object[] { e.Row.Cells["PLANNO"].Value, e.Row.Cells["ITEMNAME"].Value });

            this.Tag = TmpDt;
            this.Close();
        }
    }
}


