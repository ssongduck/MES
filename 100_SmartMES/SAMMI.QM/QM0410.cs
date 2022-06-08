#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : QM0410
//   Form Name    : 카파개선 실린더블록 합불판정
//   Name Space   : SAMMI.QM
//   Created Date : 2020-10-28
//   Made By      : 정용석
//   Description  : 공정의 합불내역 및 검사데이타 조회 
// *---------------------------------------------------------------------------------------------*
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using SAMMI.Common;
using SAMMI.Windows.Forms;
using System.Diagnostics;
using Infragistics.UltraChart.Resources.Appearance;
using System.Linq;
using DevExpress.XtraCharts;

namespace SAMMI.QM
{
    /// <summary>
    /// QM2710 class
    /// </summary>
    public partial class QM0410 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        #region Variable

        /// <summary>
        /// Return common datatable
        /// </summary>
        DataTable _RtnDt = new DataTable();

        /// <summary>
        /// 
        /// </summary>
        string[] _EmptyArray = { "v_txt" };

        /// <summary>
        /// Plant code
        /// </summary>
        private string _sPlantCode = string.Empty;

        /// <summary>
        /// Grid util
        /// </summary>
        UltraGridUtil _UltraGridUtil = new UltraGridUtil();

        /// <summary>
        /// Common
        /// </summary>
        Common.Common _Common = new Common.Common();

        BizTextBoxManagerEX btbManager;

        #endregion

        #region Constructor

        /// <summary>
        /// QM0410 constructor
        /// </summary>
        public QM0410()
        {
            InitializeComponent();
            InitializeControl();
            InitializeGridControl();

            AttachEventHandlers();            
        }

        #endregion

        #region Event

        /// <summary>
        /// QM2710 Disposed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QM0410_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlers();
        }

        #endregion

        #region Method

        /// <summary>
        /// Initialize control
        /// </summary>
        private void InitializeControl()
        {
            _Common.SetPlantAuth(cboPlantCode_H, LoginInfo.PlantAuth);

            this._sPlantCode = SqlDBHelper.nvlString(cboPlantCode_H.Value);

            if (this._sPlantCode.Equals("SK"))
            {
                this._sPlantCode = "SK1";
            }
            else if (this._sPlantCode.Equals("EC"))
            {
                this._sPlantCode = "SK2";
            }
            else
            {
                if (cboPlantCode_H.Value == null)
                {
                    cboPlantCode_H.Value = "ALL";
                    this._sPlantCode = "ALL";
                }
            }
            calFromDt.Value = DateTime.Now;
            calToDt.Value = DateTime.Now;
        }

        /// <summary>
        /// Initialize grid control
        /// </summary>
        private void InitializeGridControl()
        {
            _UltraGridUtil.InitializeGrid(this.gridItemList, true, false, false, "", false);

            //_UltraGridUtil.InitColumnUltraGrid(gridItemList, "PLANT_CODE", "사업장", false, GridColDataType_emu.VarChar, 60, 60, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            //_UltraGridUtil.InitColumnUltraGrid(gridItemList, "WORKCENTER_CODE", "작업장코드", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "LINE",    "구분", false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);            
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "J_LOTNO", "주조로트", false, GridColDataType_emu.VarChar,     150, 200, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "G_LOTNO", "가공로트", false, GridColDataType_emu.VarChar,     150, 200, Infragistics.Win.HAlign.Left, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "ITEM_CODE", "품목코드", false, GridColDataType_emu.VarChar,   150, 150, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);            
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "F_JUDGE", "최종판정", false, GridColDataType_emu.VarChar,     80, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE01", "크랭크판정", false, GridColDataType_emu.VarChar,   80, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "DATA01",  "크랭크누설량", false, GridColDataType_emu.VarChar, 80, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE02", "워터판정", false, GridColDataType_emu.VarChar,     80, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "DATA02",  "워터누설량", false, GridColDataType_emu.VarChar,   80, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE03", "오일판정", false, GridColDataType_emu.VarChar,     80, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "DATA03",  "오일누설량", false, GridColDataType_emu.VarChar,   80, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE04", "관통#1", false, GridColDataType_emu.VarChar, 60, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE05", "관통#2", false, GridColDataType_emu.VarChar, 60, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE06", "관통#3", false, GridColDataType_emu.VarChar, 60, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE07", "관통#4", false, GridColDataType_emu.VarChar, 60, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE08", "관통#5", false, GridColDataType_emu.VarChar, 60, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE09", "관통#6", false, GridColDataType_emu.VarChar, 60, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE10", "관통#7", false, GridColDataType_emu.VarChar, 60, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE11", "높이#1판정",   false, GridColDataType_emu.VarChar, 100, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "DATA11",  "높이#1측정값", false, GridColDataType_emu.VarChar, 100, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE12", "높이#2판정",   false, GridColDataType_emu.VarChar, 100, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "DATA12",  "높이#2측정값", false, GridColDataType_emu.VarChar, 100, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE13", "T45관통판정",  false, GridColDataType_emu.VarChar, 100, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "JUDGE14", "T46관통판정",  false, GridColDataType_emu.VarChar, 100, 250, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "LASTEVENT_TIME01", "#410공정 작업시간", false, GridColDataType_emu.DateTime, 150, 200, Infragistics.Win.HAlign.Center, true, false, null, "yyyy-MM-dd hh:mm:ss", null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "LASTEVENT_TIME02", "#420공정 작업시간", false, GridColDataType_emu.DateTime, 150, 200, Infragistics.Win.HAlign.Center, true, false, null, "yyyy-MM-dd hh:mm:ss", null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "LASTEVENT_TIME03", "#430공정 작업시간", false, GridColDataType_emu.DateTime, 150, 200, Infragistics.Win.HAlign.Center, true, false, null, "yyyy-MM-dd hh:mm:ss", null, null, null);
            _UltraGridUtil.InitColumnUltraGrid(gridItemList, "LASTEVENT_TIME04", "#500공정 작업시간", false, GridColDataType_emu.DateTime, 150, 200, Infragistics.Win.HAlign.Center, true, false, null, "yyyy-MM-dd hh:mm:ss", null, null, null);           
            //_UltraGridUtil.InitColumnUltraGrid(gridItemList, "CREATE_DATE", "생성일자", false, GridColDataType_emu.DateTime, 200, 200, Infragistics.Win.HAlign.Center, true, false, null, "yyyy-MM-dd hh:mm:ss", null, null, null);           

            //this.gridItemList.DisplayLayout.Bands[0].Columns[1].MergedCellEvaluationType = MergedCellEvaluationType.MergeSameText;

        }

        /// <summary>
        /// Attach event handlers
        /// </summary>
        private void AttachEventHandlers()
        {
            this.Disposed += new EventHandler(QM0410_Disposed);
        }

        /// <summary>
        /// Detach event handlers
        /// </summary>
        private void DetachEventHandlers()
        {
            this.Disposed -= new EventHandler(QM0410_Disposed);
        }

        /// <summary>
        /// Do inquire
        /// </summary>
        public override void DoInquire()
        {
            DataTable dt = new DataTable();

            SqlDBHelper sqlDBHelper = new SqlDBHelper(false);
            SqlParameter[] sqlParameters = new SqlParameter[8];

            ClearAllControl();

            try
            {
                base.DoInquire();

                string sPlantCode = SqlDBHelper.nvlString(this.cboPlantCode_H.Value);
                string sLine = string.Empty;

                if (cboLine.Value.ToString() == "M") { sLine = "B"; }
                else { sLine = cboLine.Value.ToString(); }

                base.DoInquire();

                sqlParameters[0] = sqlDBHelper.CreateParameter("@AS_PLANTCODE", sPlantCode,        SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[1] = sqlDBHelper.CreateParameter("@AS_JLOT",      txtJLot.Text,      SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[2] = sqlDBHelper.CreateParameter("@AS_GLOT",      txtGLot.Text,      SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[3] = sqlDBHelper.CreateParameter("@AS_JUDGE",     cboOKNG.Value,     SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[4] = sqlDBHelper.CreateParameter("@AS_ITEMCODE",  cboItemcode.Value, SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[5] = sqlDBHelper.CreateParameter("@AS_LINE",      sLine,             SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[6] = sqlDBHelper.CreateParameter("@AS_FROM_DT",   string.Format("{0:yyyy-MM-dd 08:00:00}", calFromDt.Value), SqlDbType.VarChar, ParameterDirection.Input);
                sqlParameters[7] = sqlDBHelper.CreateParameter("@AS_TO_DT",     string.Format("{0:yyyy-MM-dd 07:59:59}", DateTime.Parse(calToDt.Value.ToString()).AddDays(1)), SqlDbType.VarChar, ParameterDirection.Input);


                //AND PLANT_CODE = @AS_PLANTCODE
                string sqlQuery = @"SELECT   CASE WHEN LINE = 'B' THEN '수동' ELSE '자동' END AS 'LINE'
                                            ,J_LOTNO
                                            ,G_LOTNO
                                            ,ITEM_CODE
                                            ,F_JUDGE
                                            ,CASE WHEN JUDGE01 = '5'   THEN '소리크'
                                                  WHEN JUDGE01 = '6'   THEN '대리크'
			                                      WHEN JUDGE01 IS NULL THEN '' ELSE 'OK' END AS 'JUDGE01'
                                            ,DATA01
                                            ,CASE WHEN JUDGE02 = '5'   THEN '소리크'
                                                  WHEN JUDGE02 = '6'   THEN '대리크'
			                                      WHEN JUDGE02 IS NULL THEN '' ELSE 'OK' END AS 'JUDGE02'
                                            ,DATA02
                                            ,CASE WHEN JUDGE03 = '5'   THEN '소리크'
                                                  WHEN JUDGE03 = '6'   THEN '대리크'
			                                      WHEN JUDGE03 IS NULL THEN '' ELSE 'OK' END AS 'JUDGE03'
                                            ,DATA03
                                            ,JUDGE04
                                            ,JUDGE05
                                            ,JUDGE06
                                            ,JUDGE07
                                            ,JUDGE08
                                            ,JUDGE09
                                            ,JUDGE10
                                            ,JUDGE11
                                            ,DATA11
                                            ,JUDGE12
                                            ,DATA12
                                            ,JUDGE13
                                            ,JUDGE14
                                            ,LASTEVENT_TIME01
                                            ,LASTEVENT_TIME02
                                            ,LASTEVENT_TIME03
                                            ,LASTEVENT_TIME04
                                      FROM TDS3001_WIA WITH (NOLOCK)
                                     WHERE 1 = 1                     
                                       AND PLANT_CODE LIKE CASE WHEN @AS_PLANTCODE = 'ALL' THEN '' ELSE @AS_PLANTCODE END + '%' 

			                           AND LTRIM(RTRIM(ISNULL(J_LOTNO,''))) LIKE '%' + @AS_JLOT + '%' 
			                           AND LTRIM(RTRIM(ISNULL(G_LOTNO,''))) LIKE '%' + @AS_GLOT + '%'   
                                    
                                       AND ISNULL(F_JUDGE, 'NG') LIKE CASE WHEN @AS_JUDGE = 'ALL' THEN '' ELSE @AS_JUDGE END + '%'
                                       AND ITEM_CODE LIKE CASE WHEN @AS_ITEMCODE = 'ALL' THEN '' ELSE @AS_ITEMCODE END + '%'
                                       AND ISNULL(LINE, 'A') LIKE CASE WHEN @AS_LINE = 'ALL' THEN '' ELSE @AS_LINE END + '%'
                                       AND LASTEVENT_TIME01 > @AS_FROM_DT
                                       AND LASTEVENT_TIME01 < @AS_TO_DT
                                     ORDER BY LASTEVENT_TIME01 DESC";


                                       //AND RTRIM(LTRIM(J_LOTNO)) LIKE '%' + @AS_JLOT + '%'
                                       //AND RTRIM(LTRIM(G_LOTNO)) LIKE '%' + @AS_GLOT + '%'

                dt = sqlDBHelper.FillTable(sqlQuery, CommandType.Text, sqlParameters);

                gridItemList.DataSource = dt;
                gridItemList.DataBind();

            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Do new
        /// </summary>
        public override void DoNew()
        {
        }

        /// <summary>
        /// Do delete
        /// </summary>
        public override void DoDelete()
        {
        }

        /// <summary>
        /// Do save
        /// </summary>
        public override void DoSave()
        {
        }

        /// <summary>
        /// Clear control
        /// </summary>
        /// <param name="control"></param>
        private void ClearControl(System.Windows.Forms.Control control)
        {
            if (control == null)
            {
                return;
            }

            foreach (System.Windows.Forms.Control ctrl in control.Controls)
            {
                ClearControl(ctrl);

                if (ctrl.GetType().Name == "TextBox")
                {
                    TextBox textBox = (TextBox)ctrl;

                    foreach (string sVal in _EmptyArray)
                    {
                        if (textBox.Name.StartsWith(sVal))
                        {
                            textBox.Text = string.Empty;
                        }
                    }
                }

                if (ctrl.GetType().Name == "MaskedTextBox")
                {
                    MaskedTextBox maskedTextBox = (MaskedTextBox)ctrl;

                    foreach (string sVal in _EmptyArray)
                    {
                        if (maskedTextBox.Name.StartsWith(sVal))
                        {
                            maskedTextBox.Text = string.Empty;
                        }
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Clear all control
        /// </summary>
        private void ClearAllControl()
        {
            ClearControl(this);
        }

        #endregion

        private void gridItemList_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["F_JUDGE"].Value.ToString() == "NG" || 
                e.Row.Cells["JUDGE01"].Value.ToString() == "소리크" || e.Row.Cells["JUDGE01"].Value.ToString() == "대리크" ||
                e.Row.Cells["JUDGE02"].Value.ToString() == "소리크" || e.Row.Cells["JUDGE02"].Value.ToString() == "대리크" ||
                e.Row.Cells["JUDGE03"].Value.ToString() == "소리크" || e.Row.Cells["JUDGE03"].Value.ToString() == "대리크" ||
                e.Row.Cells["JUDGE04"].Value.ToString() == "NG" || e.Row.Cells["JUDGE05"].Value.ToString() == "NG" ||
                e.Row.Cells["JUDGE06"].Value.ToString() == "NG" || e.Row.Cells["JUDGE07"].Value.ToString() == "NG" ||
                e.Row.Cells["JUDGE08"].Value.ToString() == "NG" || e.Row.Cells["JUDGE09"].Value.ToString() == "NG" ||
                e.Row.Cells["JUDGE10"].Value.ToString() == "NG" || e.Row.Cells["JUDGE11"].Value.ToString() == "NG" ||
                e.Row.Cells["JUDGE12"].Value.ToString() == "NG" || e.Row.Cells["JUDGE13"].Value.ToString() == "NG" ||
                e.Row.Cells["JUDGE14"].Value.ToString() == "NG")
            {
                e.Row.Cells["F_JUDGE"].Appearance.BackColor = Color.Orange;
                e.Row.Cells["F_JUDGE"].SetValue("NG", false);
                e.Row.Cells["F_JUDGE"].Appearance.ForeColor = Color.Black;
            }
        }

    }
}
