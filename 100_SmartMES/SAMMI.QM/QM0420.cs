#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : QM0420
//   Form Name    : 포르쉐 AI비전검사 불량현황
//   Name Space   : SAMMI.MM
//   Created Date : 2021-04-28
//   Made By      : 정용석
//   Description  : 비전검사 결과 조회
// *---------------------------------------------------------------------------------------------*
#endregion

#region <USING AREA>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SAMMI.Common;
using SAMMI.PopUp;
using SAMMI.PopManager;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Linq;

using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.Data;
#endregion

namespace SAMMI.QM
{
    public partial class QM0420 : SAMMI.Windows.Forms.BaseMDIChildForm
    {
        DataSet rtnDsTemp = new DataSet(); // return DataSet 공통

        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        Common.Common _Common = new Common.Common();

        DataTable DtChangeGrid1 = new DataTable();
        DataTable DtChangeGrid2 = new DataTable();

        //Panel _Canvas = new Panel();
        Graphics _CanvasGraphics = null;
        BufferedGraphics _BufferedGraphics;
        BufferedGraphicsContext _CurrentContext;

        Bitmap bitmap;

        private string PlantCode = string.Empty;

        public QM0420()
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
            
            peErrPos.Width  = 448;
            peErrPos.Height = 254;

            _CanvasGraphics = peErrPos.CreateGraphics();                        
            _CurrentContext = BufferedGraphicsManager.Current;
            _BufferedGraphics = _CurrentContext.Allocate(_CanvasGraphics, new Rectangle(0, 0, peErrPos.Width, peErrPos.Height));                        
            _BufferedGraphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;                    
        }
        /// <summary>
        /// ToolBar의 조회 버튼 클릭
        /// </summary>
        public override void DoInquire()
        {
            SqlDBHelper helper = new SqlDBHelper(true, false);
            SqlParameter[] param = new SqlParameter[5];
            try
            {
                DtChangeGrid1.Clear();
                DtChangeGrid2.Clear();
                
                string sLotNo     = SqlDBHelper.nvlString(this.txtLotNo.Text);
                string sJudge     = SqlDBHelper.nvlString(this.cboJudge.Value);
                string sItemType  = SqlDBHelper.nvlString(this.cboItemType.Value);
                string sStartDate = string.Format("{0:yyyy-MM-dd}", CboStartdate_H.Value);                      
                string sEndDate   = string.Format("{0:yyyy-MM-dd}", CboEnddate_H.Value);                                                                      
                base.DoInquire();

                param[0] = helper.CreateParameter("@as_lotNo",    sLotNo,     SqlDbType.VarChar, ParameterDirection.Input);
                param[1] = helper.CreateParameter("@as_judge",    sJudge,     SqlDbType.VarChar, ParameterDirection.Input);
                param[2] = helper.CreateParameter("@as_itemType", sItemType,  SqlDbType.VarChar, ParameterDirection.Input);
                param[3] = helper.CreateParameter("@as_sDate",    sStartDate, SqlDbType.VarChar, ParameterDirection.Input);
                param[4] = helper.CreateParameter("@as_eDate",    sEndDate,   SqlDbType.VarChar, ParameterDirection.Input);
                

                rtnDsTemp = helper.FillDataSet("USP_QM0420_S1", CommandType.StoredProcedure, param);
                                                
                grid1.DataSource = DtChangeGrid1 = rtnDsTemp.Tables[0];
                grid1.DataBind();
                
                _Common.Grid_Column_Width(this.grid1); //grid 정리용  

                DrawChart(rtnDsTemp.Tables[2]);                
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
        /// 일자별 불량유형 통계차트
        /// </summary>
        /// <param name="dt"></param>
        private void DrawChart(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                chartErrStatic.Series.Clear();

                foreach (string faultName in dt.AsEnumerable().Select(t => t.Field<string>("ErrType")).Distinct())
                {
                    DataTable tempDt = dt.Clone();

                    Series series = new Series(faultName, ViewType.Bar);

                    foreach (DataRow dr in dt.AsEnumerable().Where(t => t.Field<string>("ErrType") == faultName))
                    {
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dr["inspDate"].ToString()), int.Parse(dr["ErrCount"].ToString())));
                    }
                    series.ArgumentScaleType = ScaleType.DateTime;
                    series.ValueScaleType    = ScaleType.Numerical;

                    ((BarSeriesView)series.View).FillStyle.FillMode = FillMode.Solid;
                    series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartErrStatic.Series.Add(series);
                }

                XYDiagram diagram = chartErrStatic.Diagram as XYDiagram;
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                diagram.AxisX.Label.TextPattern = "{V:yyyy-MM-dd}";
                diagram.AxisY.Label.TextPattern = "{V:n}";
                diagram.EnableAxisXScrolling = true;
                diagram.EnableAxisXZooming = true;
                diagram.EnableAxisYScrolling = true;
                diagram.EnableAxisYZooming = true;
            }
            else
            {
                chartErrStatic.Series.Clear();
            }
        }
        private void QM0420_Load(object sender, EventArgs e)
        {
            #region Grid1 셋팅

            _GridUtil.InitializeGrid(grid1);                        
            _GridUtil.InitColumnUltraGrid(grid1, "LotNo",      "로트번호",  false, GridColDataType_emu.VarChar, 120, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "ItemName",   "품명",      false, GridColDataType_emu.VarChar, 150, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);            
            _GridUtil.InitColumnUltraGrid(grid1, "Judge",      "판정",      false, GridColDataType_emu.VarChar, 60,  100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid1, "InspDate",   "검사일",    false, GridColDataType_emu.VarChar, 80,  100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);                                    
            
            _GridUtil.SetInitUltraGridBind(grid1);
            DtChangeGrid1 = (DataTable)grid1.DataSource;
            
            grid1.DisplayLayout.Override.RowSelectorNumberStyle = RowSelectorNumberStyle.VisibleIndex;
            grid1.DisplayLayout.Override.RowSelectorWidth = 40;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grid1.DisplayLayout.Override.RowSelectorAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;            

            #endregion

            #region Grid2 셋팅

            _GridUtil.InitializeGrid(grid2);
            _GridUtil.InitColumnUltraGrid(grid2, "LotNo",    "로트번호",   false, GridColDataType_emu.VarChar, 120, 180, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ErrName",  "불량명",     false, GridColDataType_emu.VarChar, 100, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "InspDate", "검사일시",   false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "ImgPath",  "이미지경로", false, GridColDataType_emu.VarChar, 300, 300, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "x",        "불량좌표X",  false, GridColDataType_emu.VarChar, 80,  100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "y",        "불량좌표Y",  false, GridColDataType_emu.VarChar, 80,  100, Infragistics.Win.HAlign.Center, true, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "Width",    "불량너비",   false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "Height",   "불량높이",   false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);
            _GridUtil.InitColumnUltraGrid(grid2, "Size",     "불량크기",   false, GridColDataType_emu.VarChar, 80, 100, Infragistics.Win.HAlign.Center, false, false, null, null, null, null, null);

            _GridUtil.SetInitUltraGridBind(grid2);
            DtChangeGrid2 = (DataTable)grid2.DataSource;    

            #endregion            
        }
        /// <summary>
        /// 검사로트(헤더)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            string sLotNo = this.grid1.ActiveRow.Cells["LotNo"].Value.ToString();

            if (SqlDBHelper.nvlString(sLotNo) != "")
            {
       
                grid2.DataSource = rtnDsTemp.Tables[1].AsEnumerable().Where(t => t.Field<string>("LotNo") == sLotNo).CopyToDataTable();                
                grid2.DataBind();
                _Common.Grid_Column_Width(this.grid2); //grid 정리용    
            }
            else
            {
                grid2.DataSource = null;
            }
        }
        /// <summary>
        /// 검사로트(상세)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid2_AfterRowActivate(object sender, EventArgs e)
        {
            //_BufferedGraphics.Graphics.Clear(Color.Transparent);            
            string sImgPath = this.grid2.ActiveRow.Cells["ImgPath"].Value.ToString();

            // 이미지에 따라 보정치 조정필요
            int iX = Convert.ToInt32(this.grid2.ActiveRow.Cells["x"].Value) / 50;
            int iY = Convert.ToInt32(this.grid2.ActiveRow.Cells["y"].Value) / 50;

            // 불량위치 이미지 전체샷 분기 로직 필요
            if (SqlDBHelper.nvlString(sImgPath) != "")
            {
                peErrImg.LoadAsync(sImgPath);

                _BufferedGraphics.Graphics.DrawImage(ImageReSize(peErrPos, global::SAMMI.QM.Properties.Resources.endplate), 0, 0);
                _BufferedGraphics.Graphics.DrawRectangle(pen(Color.Yellow), iX, iY, 30, 30);
                _BufferedGraphics.Render();                  
            }
            else
            {                
                peErrImg.LoadAsync(null);
            }
        }

        /// <summary>
        /// 펜
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Pen pen(Color color)
        {
            Pen p = new Pen(color);
            p.Width = 1f;
            return p;
        }
        /// <summary>
        /// 투명도
        /// </summary>
        /// <param name="image"></param>
        /// <param name="opacityvalue"></param>
        /// <returns></returns>
        public Bitmap ChangeOpacity(Image image, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();
            return bmp;
        }
        /// <summary>
        /// 이미지 사이즈 재조정
        /// </summary>
        /// <param name="pnSector"></param>
        /// <param name="tgImg"></param>
        /// <returns></returns>
        Image ImageReSize(Panel pnSector, Image tgImg)
        {
            Bitmap sourceImage = new Bitmap(tgImg);
            Size resize = new Size(pnSector.Size.Width, pnSector.Size.Height);
            sourceImage = new Bitmap(sourceImage, resize);
            return sourceImage;
        }
    }
}
