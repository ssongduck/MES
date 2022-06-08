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

namespace SAMMI.PP
{
   /// <summary>
   /// PP1100 class
   /// </summary>
    public partial class PP1100 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
       #region Variable
       
       /// <summary>
       /// return DataTable 공통
       /// </summary>
       DataTable rtnDtTemp = new DataTable(); 

       /// <summary>
       /// 그리드 객체 생성
       /// </summary>
       UltraGridUtil _GridUtil = new UltraGridUtil();
       
       /// <summary>
       /// DtChange
       /// </summary>
       private DataTable _DtChange = null;

       /// <summary>
       /// Biz text box manager EX
       /// </summary>
       BizTextBoxManagerEX _BtbManager;

       /// <summary>
       /// 비지니스 로직 객체 생성
       /// </summary>
       PopUp_Biz _biz = new PopUp_Biz();

       /// <summary>
       /// 임시로 사용할 데이터테이블 생성
       /// </summary>
       DataTable _DtTemp = new DataTable();

       /// <summary>
       /// PlantCode
       /// </summary>
       private string _PlantCode = string.Empty;

       /// <summary>
       /// common
       /// </summary>
       Common.Common _Common = new Common.Common();

       #endregion

       #region Constructor

       public PP1100()
       {
          InitializeComponent();
          InitializeControl();
          InitializeGridControl();

       }

       #endregion

       #region Event

       #endregion

       #region Method

       /// <summary>
       /// 폼초기화
       /// </summary>
       private void InitializeControl()
       {
          // 사업장 사용권한 설정
          _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

          this._PlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

          #region 콤보박스
          rtnDtTemp = _Common.GET_TBM0000_CODE("PLANTCODE");
          SAMMI.Common.UltraGridUtil.SetComboUltraGrid(this.grid1, "PlantCode", rtnDtTemp, "CODE_ID", "CODE_NAME");
          #endregion

          if (this._PlantCode.Equals("SK"))
          {
             this._PlantCode = "SK1";
          }
          else if (this._PlantCode.Equals("EC"))
          {
             this._PlantCode = "SK2";
          }

          if (!(this._PlantCode.Equals("SK1") || this._PlantCode.Equals("SK2")))
          {
             this.cboPlantCode_H.Value = this.cboPlantCode_H.DefaultValue;
          }

          _BtbManager = new BizTextBoxManagerEX();

          if (LoginInfo.PlantAuth.Equals(string.Empty))
          {
             _BtbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { this.cboPlantCode_H, "" });
             _BtbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { this.cboPlantCode_H, txtOPCode, "", "" }
                 , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });
          }
          else
          {
             _BtbManager.PopUpAdd(txtOPCode, txtOPName, "TBM0400", new object[] { LoginInfo.PlantAuth, "" });
             _BtbManager.PopUpAdd(txtWorkCenterCode, txtWorkCenterName, "TBM0600", new object[] { LoginInfo.PlantAuth, txtOPCode, "", "" }
                 , new string[] { "OPCode", "OPName" }, new object[] { txtOPCode, txtOPName });

          }
       }

       /// <summary>
       /// 그리드 초기화
       /// </summary>
       private void InitializeGridControl()
       {
          #region Init_Grid
          _GridUtil.InitializeGrid(this.grid1, true, false, false, "", false);
          _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "사업장", false, GridColDataType_emu.VarChar, 90, 100, Infragistics.Win.HAlign.Left, (this._PlantCode == "") ? true : false, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "RECDATE", "작업일자", false, GridColDataType_emu.VarChar, 95, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "OPCODE", "공정", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "OPNAME", "공정명", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "LINECODE", "작업라인", false, GridColDataType_emu.VarChar, 110, 100, Infragistics.Win.HAlign.Left, false, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "LINENAME", "라인명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", false, GridColDataType_emu.VarChar, 70, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "ORDERNO", "지시번호", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "CARTYPE", "차종", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품번", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품명", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "START_DT", "시작시간", false, GridColDataType_emu.DateTime24, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "END_DT", "종료시간", false, GridColDataType_emu.DateTime24, 160, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "WORKSEC", "가동(분)", false, GridColDataType_emu.Integer, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "NONWORKSEC", "비가동(분)", false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "WORKERCNT", "작업자수", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);
          _GridUtil.InitColumnUltraGrid(grid1, "WORKERSEC", "작업자 투입(분)", false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Right, true, false, "#,##0", null, null, null, null);

          _GridUtil.SetInitUltraGridBind(grid1);
          #endregion

       }

       /// <summary>
       /// 조회
       /// </summary>
       public override void DoInquire()
       {
          
          SqlDBHelper helper = new SqlDBHelper(true, false);
          SqlParameter[] param = new SqlParameter[6];

          try
          {
             base.DoInquire();

             string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);                                     // 사업장(공장)
             string sWorkerID = "";//this.txtWorkerID.Text.Trim();                                              // 작업자
             string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);                          // 생산시작일자
             string sEndDate = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);                            // 생산  끝일자
             string sWorkCenterCode = this.txtWorkCenterCode.Text.Trim();                                             // 작업장 코드
             string sOPCode = this.txtOPCode.Text.Trim();                                                     // 공정 코드

             param[0] = helper.CreateParameter("PlantCode", sPlantCode, SqlDbType.VarChar, ParameterDirection.Input);        // 사업장(공장)    
             param[1] = helper.CreateParameter("WorkerID", sWorkerID, SqlDbType.VarChar, ParameterDirection.Input);         // 작업자          
             param[2] = helper.CreateParameter("StartDate", sStartDate, SqlDbType.VarChar, ParameterDirection.Input);        // 생산시작일자    
             param[3] = helper.CreateParameter("EndDate", sEndDate, SqlDbType.VarChar, ParameterDirection.Input);          // 생산  끝일자    
             param[4] = helper.CreateParameter("WorkCenterCode", sWorkCenterCode, SqlDbType.VarChar, ParameterDirection.Input);   // 작업장 코드     
             param[5] = helper.CreateParameter("OPCode", sOPCode, SqlDbType.VarChar, ParameterDirection.Input);           // 공정 코드       

             //rtnDtTemp = helper.FillTable("USP_PP1100_S1N", CommandType.StoredProcedure, param);
             rtnDtTemp = helper.FillTable("USP_PP1100_S1N_UNION", CommandType.StoredProcedure, param);
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

       /// <summary>
       /// Do new
       /// </summary>
       public override void DoNew()
       {
       }

       /// <summary>
       /// Do save
       /// </summary>
       public override void DoSave()
       {
       }

       /// <summary>
       /// Do delete
       /// </summary>
       public override void DoDelete()
       {
       }

       /// <summary>
       /// Do down load excel
       /// </summary>
       public override void DoDownloadExcel()
       {
       }

       /// <summary>
       /// 합계
       /// </summary>
       public override void DoBaseSum()
       {
          base.DoBaseSum();

          UltraGridRow ugr = grid1.DoSummaries(new string[] { "workSec", "nonworkSec", "WorkerCnt", "workerSec" });

       } 

       #endregion
       
    }
}