﻿#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      :  PP1200
//   Form Name    : 작업자실 공수 투입 이력 정보 조회
//   Name Space   : SAMMI.MM
//   Created Date : 
//   Made By      : SAMMI INFORMATION SYSTEM CO.,LTD
//   Description  : 
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
#endregion

namespace SAMMI.PP
{
    public partial class PP1200 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통
        DataTable rtnDtTemp = new DataTable(); // return DataTable 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();
        Common.Common _Common = new Common.Common();

        private DataTable DtChange = null;
        BizTextBoxManagerEX btbManager;

        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();

        private string PlantCode = string.Empty;

        public PP1200()
        {
            InitializeComponent();

            // 사업장 사용권한 설정
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this.PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this.PlantCode.Equals("SK"))
                this.PlantCode = "SK1";
            else if (this.PlantCode.Equals("EC"))
                this.PlantCode = "SK2";
            if (!(this.PlantCode.Equals("SK1") || this.PlantCode.Equals("SK2")))
                this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;
        }

        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[7];

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                    // 사업장(공장)
                string sWorkerID =  this.txtWorkerID.Text.Trim();                                                   // 작업자
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);                          // 생산시작일자
                string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);                              // 생산  끝일자
                string sWorkCenterCode = this.txtWorkCenterCode.Text.Trim();                                        // 작업장 코드
                string sOPCode = this.txtOPCode.Text.Trim();                                                        // 공정 코드
                string sLineCode = this.txtLineCode.Text.Trim();                                                    // 라인 코드


                param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);             // 사업장(공장)    
                param[1] = helper.CreateParameter("WorkerID", sWorkerID, SqlDbType.VarChar, ParameterDirection.Input);               // 작업자          
                param[2] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);             // 생산시작일자    
                param[3] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);                 // 생산  끝일자    
                param[4] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
                param[5] = helper.CreateParameter("OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);                   // 공정 코드       
                param[6] = helper.CreateParameter("LineCode", sLineCode, SqlDbType.VarChar, ParameterDirection.Input);               // 라인 코드       

                //rtnDtTemp = helper.FillTable("USP_PP1200_S1N", CommandType.StoredProcedure, param);
                rtnDtTemp = helper.FillTable("USP_PP1200_S1N_UNION", CommandType.StoredProcedure, param);
                grid1.DataSource = rtnDtTemp;
                grid1.DataBind();

                _Common.Grid_Column_Width(this.grid1); //grid 정리용   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (helper._sConn != null) { helper._sConn.Close(); }
                if (param != null) { param = null; }
            }
        }


        #region 폼 로더
        private void PP1200_Load(object sender, EventArgs e)
        {
            #region Grid 셋팅
            _GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitColumnUltraGrid(grid1, "PlantCode", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, (this.PlantCode == "") ? true : false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerID", "사번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerName", "이름", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "RecDate", "작업일자", false, GridColDataType_emu.VarChar, 95, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPCode", "공정", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OPName", "공정명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineCode", "작업라인", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "LineName", "라인명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterCode", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkCenterName", "작업장명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "OrderNo", "지시번호", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "CarType", "차종", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemCode", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName", "품명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "StartDate", "시작시간", false, GridColDataType_emu.DateTime24, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "EndDate", "종료시간", false, GridColDataType_emu.DateTime24, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "WorkerSec", "작업(분)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);


              _GridUtil.SetInitUltraGridBind(grid1);
              grid1.DisplayLayout.UseFixedHeaders = true;
              for (int i = 0; i < 3; i++)
                  grid1.DisplayLayout.Bands[0].Columns[i].Header.Fixed = true;

            #endregion

            #region 콤보박스
            rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
            SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");

            #endregion

            #region Grid MERGE


            //grid1.Columns["PlantCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["PlantCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["PlantCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkerID"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkerID"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkerID"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkerName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkerName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkerName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["RecDate"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["RecDate"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["RecDate"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["OPCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["OPName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["OPName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["OPName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["LineCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["LineName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["LineName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["LineName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterCode"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterCode"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterCode"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["WorkCenterName"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["WorkCenterName"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["WorkCenterName"].MergedCellStyle = MergedCellStyle.Always;

            //grid1.Columns["DayNight"].MergedCellContentArea = MergedCellContentArea.VisibleRect;
            //grid1.Columns["DayNight"].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameValue;
            //grid1.Columns["DayNight"].MergedCellStyle = MergedCellStyle.Always;


            #endregion Grid MERGE
        }
        #endregion

        #region <METHOD AREA>
        // Form에서 사용할 함수나 메소드를 정의
        #region 텍스트 박스에서 팝업창에서 값 가져오기

        private void Search_Pop_Item()
        {
            string sitem_cd = "";// this.txtItemCode.Text.Trim();    // 품목코드
            string sitem_name = ""; // this.txtItemName.Text.Trim();  // 품목명
            string sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value.ToString());
            // string splantcd = "820";
            string sitemtype = "";


            try
            {

                _DtTemp = _biz.SEL_TBM0100(sPlantCode, sitem_cd, sitem_name, sitemtype);

                if (_DtTemp.Rows.Count > 1)
                {
                    // 품목 POP-UP 창 처리
                    PopUPManager pu = new PopUPManager();
                    _DtTemp = pu.OpenPopUp("Item", new string[] { sPlantCode, sitemtype, sitem_cd, sitem_name }); // 품목 조회 POP-UP창 Parameter(비가동코드, 비가동명, 비가동그룹)

                    if (_DtTemp != null && _DtTemp.Rows.Count > 0)
                    {
                        //txtItemCode.Text = Convert.ToString(_DtTemp.Rows[0]["ItemCode"]);
                        //txtItemName.Text = Convert.ToString(_DtTemp.Rows[0]["Itemname"]);
                    }
                }
                else
                {
                    if (_DtTemp.Rows.Count == 1)
                    {
                        //txtItemCode.Text = Convert.ToString(_DtTemp.Rows[0]["ItemCode"]);
                        //txtItemName.Text = Convert.ToString(_DtTemp.Rows[0]["Itemname"]);
                    }
                    else
                    {
                        MessageBox.Show("입력하신 정보는 없는 정보입니다.", "ERROR");
                        // txtItemCode.Text = string.Empty;
                        //txtItemName.Text = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }
        #endregion
        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            //  this.txtItemName.Text = string.Empty;
        }

        private void txtItemCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_Item();
            }
        }

        private void txtItemCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_Item();
        }

        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
            //this.txtItemCode.Text = string.Empty;
        }

        private void txtItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_Item();
            }
        }

        private void txtItemName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_Item();
        }

        #region 텍스트 박스에서 팝업창에서 값 가져오기
        //////////////////     
        private void Search_Pop_TBM0400()
        {

            string sPlantCode = string.Empty;             //사업장코드
            string sOPCode = txtOPCode.Text.Trim();       //공정코드
            string sOPName = txtOPName.Text.Trim();       //공정명 
            string sUseFlag = string.Empty;               //사용여부         
           

            if (SqlDBHelper.nvlString(this.cboPlantCode_H.Value) != null)
                sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value) == "ALL" ? "" : SqlDBHelper.nvlString(this.cboPlantCode_H.Value);         ///사업장코드 

            //            if (this.cboUseFlag_H.SelectedValue != null)
            //                sUseFlag = cboUseFlag_H.SelectedValue.ToString() == "ALL" ? "" : cboUseFlag_H.SelectedValue.ToString();                 // 사용여부

            sUseFlag = "";                 // 사용여부

            try
            {
                _biz.TBM0400_POP(sPlantCode, sOPCode, sOPName, sUseFlag, txtOPCode, txtOPName);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }
        #endregion        //공정(작업장)
        private void txtOPCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPName.Text = string.Empty;
        }

        private void txtOPNAME_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtOPCode.Text = string.Empty;
        }

        private void txtOPCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0400();
            }
        }



        private void txtOPName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0400();
            }

        }

        #region 라인 (TBM0500) 팝업창에서 값 가져오기
        private void Search_Pop_TBM0500()
        {

            string sPlantCode = string.Empty;                             //사업장코드
            string sLineCode = txtLineCode.Text.Trim();                   //라인코드
            string sLineName = txtLineName.Text.Trim();                   //라인명명 
            string sUseFlag = string.Empty;                              //사용여부      


            if (SqlDBHelper.nvlString(this.cboPlantCode_H.Value) != null)
                sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value) == "ALL" ? "" : SqlDBHelper.nvlString(this.cboPlantCode_H.Value);         ///사업장코드 

            //            if (this.cboUseFlag_H.SelectedValue != null)
            //                sUseFlag = cboUseFlag_H.SelectedValue.ToString() == "ALL" ? "" : cboUseFlag_H.SelectedValue.ToString();                 // 사용여부

            sUseFlag = "";                 // 사용여부

            try
            {
                //_biz.TBM0500_POP(sPlantCode, sLineCode, sLineName, sUseFlag, txtLineCode, txtLineName);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }
        #endregion 라인 (TBM0500) 팝업창에서 값 가져오기

        private void txtLineName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtLineCode.Text = string.Empty;
        }


        private void txtLineCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0500();
            }
        }

        private void txtLineCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0500();
        }



        private void txtLineName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0500();
            }
        }

        private void txtLineName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0500();
        }



        #region 작업장(TBM0600) 팝업창에서 값 가져오기
        //////////////////     
        private void Search_Pop_TBM0600()
        {

            string sPlantCode = string.Empty;                             //사업장코드
            string sOPCode = txtOPCode.Text.Trim();                       //공정코드
            string sOPName = txtOPName.Text.Trim();                       //공정명 
            string sLineCode = string.Empty;                              //라인코드
            string sWORKCENTERCODE = txtWorkCenterCode.Text.Trim();       //작업호기(라인)코드
            string sWorkCenterName = txtWorkCenterName.Text.Trim();       //작업호기(라인)명 
            string sUseFlag = string.Empty;                               //사용여부         


            if (SqlDBHelper.nvlString(this.cboPlantCode_H.Value) != null)
                sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value) == "ALL" ? "" : SqlDBHelper.nvlString(this.cboPlantCode_H.Value);         ///사업장코드 

            //if (this.cboUseFlag_H.SelectedValue != null)
            //    sUseFlag = cboUseFlag_H.SelectedValue.ToString() == "ALL" ? "" : cboUseFlag_H.SelectedValue.ToString();                 // 사용여부
            sUseFlag = "";                 // 사용여부: 전체


            try
            {
                _biz.TBM0600_POP(sPlantCode, sWORKCENTERCODE, sWorkCenterName, sOPCode, sLineCode, sUseFlag, txtWorkCenterCode, txtWorkCenterName);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }
        #endregion

        private void txtWorkCenterCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkCenterName.Text = string.Empty;
        }

        private void txtWorkCenterName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkCenterCode.Text = string.Empty;
        }

        private void txtWorkCenterCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0600();
            }
        }

        private void txtWorkCenterCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0600();
        }



        private void txtWorkCenterName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0600();
            }
        }

        private void txtWorkCenterName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0600();
        }


        #endregion

        private void txtOPCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0400();
        }

        private void txtOPName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0400();
        }

        private void txtLineCode_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtLineName.Text = string.Empty;
        }

        #region 작업자 Pop UP
        private void Search_Pop_TBM0200()
        {

            string sPlantCode = string.Empty;             //사업장코드
            string sOPCode = txtOPCode.Text.Trim();      //공정코드
            string sLineCode = txtLineCode.Text.Trim();   //라인코드 
            string sWorkCenterCode = txtWorkCenterCode.Text.Trim();
            string sWorkerID = txtWorkerID.Text.Trim();
            string sWorkerName = txtWorkerName.Text.Trim();
            string sUseFlag = string.Empty;               //사용여부         


            if (SqlDBHelper.nvlString(this.cboPlantCode_H.Value) != null)
                sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value) == "ALL" ? "" : SqlDBHelper.nvlString(this.cboPlantCode_H.Value);         ///사업장코드 

            //            if (this.cboUseFlag_H.SelectedValue != null)
            //                sUseFlag = cboUseFlag_H.SelectedValue.ToString() == "ALL" ? "" : cboUseFlag_H.SelectedValue.ToString();                 // 사용여부

            sUseFlag = "";                 // 사용여부

            try
            {

                _biz.TBM0200_POP(sPlantCode, sOPCode, sLineCode, sWorkCenterCode, sWorkerID, sWorkerName, sUseFlag,"0", txtWorkerID, txtWorkerName);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }

        }
        #endregion        / 작업자 Pop UP


        private void txtsWorkerID_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkerName.Text = string.Empty;
        }

        private void txtsWorkerID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0200();
            }
        }

        private void txtsWorkerID_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0200();
        }

        private void txtWorkerName_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtWorkerID.Text = string.Empty;
        }

        private void txtWorkerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search_Pop_TBM0200();
            }
        }

        private void txtWorkerName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Search_Pop_TBM0200();
        }

        private void grid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["RecDate"].Value.ToString() == "계")
                e.Row.Appearance.BackColor = Color.LightCyan;

        }

    }
}
