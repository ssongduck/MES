namespace SAMMI.PP
{
    partial class PP0342
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton3 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton4 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance108 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance109 = new Infragistics.Win.Appearance();
            this.sLabel1 = new SAMMI.Control.SLabel();
            this.calFromDt = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.sLabel3 = new SAMMI.Control.SLabel();
            this.cboPlantCode_H = new SAMMI.Control.SCodeNMComboBox();
            this.txtWorkCenterCode = new System.Windows.Forms.TextBox();
            this.txtWorkCenterName = new System.Windows.Forms.TextBox();
            this.lblWorkCenterCode = new SAMMI.Control.SLabel();
            this.calToDt = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.sLabel2 = new SAMMI.Control.SLabel();
            this.txtOPCode = new System.Windows.Forms.TextBox();
            this.txtOPName = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageChart = new System.Windows.Forms.TabPage();
            this.tabPageGrid = new System.Windows.Forms.TabPage();
            this.grid1 = new SAMMI.Control.Grid(this.components);
            this.chartCastTemp = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).BeginInit();
            this.gbxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).BeginInit();
            this.gbxBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.calFromDt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlantCode_H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.calToDt)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageChart.SuspendLayout();
            this.tabPageGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCastTemp)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxHeader
            // 
            this.gbxHeader.ContentPadding.Bottom = 2;
            this.gbxHeader.ContentPadding.Left = 2;
            this.gbxHeader.ContentPadding.Right = 2;
            this.gbxHeader.ContentPadding.Top = 4;
            this.gbxHeader.Controls.Add(this.sLabel2);
            this.gbxHeader.Controls.Add(this.calToDt);
            this.gbxHeader.Controls.Add(this.txtWorkCenterCode);
            this.gbxHeader.Controls.Add(this.txtWorkCenterName);
            this.gbxHeader.Controls.Add(this.lblWorkCenterCode);
            this.gbxHeader.Controls.Add(this.cboPlantCode_H);
            this.gbxHeader.Controls.Add(this.sLabel3);
            this.gbxHeader.Controls.Add(this.calFromDt);
            this.gbxHeader.Controls.Add(this.sLabel1);
            this.gbxHeader.Size = new System.Drawing.Size(1020, 62);
            // 
            // gbxBody
            // 
            this.gbxBody.ContentPadding.Bottom = 4;
            this.gbxBody.ContentPadding.Left = 4;
            this.gbxBody.ContentPadding.Right = 4;
            this.gbxBody.ContentPadding.Top = 6;
            this.gbxBody.Controls.Add(this.tabControl1);
            this.gbxBody.Controls.Add(this.txtOPCode);
            this.gbxBody.Controls.Add(this.txtOPName);
            this.gbxBody.Location = new System.Drawing.Point(0, 62);
            this.gbxBody.Size = new System.Drawing.Size(1020, 680);
            // 
            // sLabel1
            // 
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Middle";
            this.sLabel1.Appearance = appearance5;
            this.sLabel1.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.sLabel1.DbField = null;
            this.sLabel1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sLabel1.Location = new System.Drawing.Point(211, 18);
            this.sLabel1.Name = "sLabel1";
            this.sLabel1.Size = new System.Drawing.Size(68, 25);
            this.sLabel1.TabIndex = 245;
            this.sLabel1.Text = "기준일자";
            // 
            // calFromDt
            // 
            appearance1.FontData.SizeInPoints = 10F;
            this.calFromDt.Appearance = appearance1;
            dateButton3.Date = new System.DateTime(2017, 3, 7, 0, 0, 0, 0);
            this.calFromDt.DateButtons.Add(dateButton3);
            this.calFromDt.Location = new System.Drawing.Point(297, 19);
            this.calFromDt.Name = "calFromDt";
            this.calFromDt.NonAutoSizeHeight = 26;
            this.calFromDt.Size = new System.Drawing.Size(109, 24);
            this.calFromDt.TabIndex = 2;
            this.calFromDt.Value = new System.DateTime(2017, 3, 7, 0, 0, 0, 0);
            // 
            // sLabel3
            // 
            appearance22.TextHAlignAsString = "Right";
            appearance22.TextVAlignAsString = "Middle";
            this.sLabel3.Appearance = appearance22;
            this.sLabel3.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.sLabel3.DbField = "cboUseFlag";
            this.sLabel3.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.sLabel3.Location = new System.Drawing.Point(15, 18);
            this.sLabel3.Name = "sLabel3";
            this.sLabel3.Size = new System.Drawing.Size(52, 26);
            this.sLabel3.TabIndex = 530;
            this.sLabel3.Text = "사업장";
            // 
            // cboPlantCode_H
            // 
            appearance2.FontData.Name = "맑은 고딕";
            appearance2.FontData.SizeInPoints = 9F;
            this.cboPlantCode_H.Appearance = appearance2;
            this.cboPlantCode_H.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboPlantCode_H.ComboDataType = SAMMI.Control.ComboDataType.All;
            this.cboPlantCode_H.DbConfig = null;
            this.cboPlantCode_H.DefaultValue = "ALL";
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cboPlantCode_H.DisplayLayout.Appearance = appearance35;
            this.cboPlantCode_H.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cboPlantCode_H.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboPlantCode_H.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance36.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance36.BorderColor = System.Drawing.SystemColors.Window;
            this.cboPlantCode_H.DisplayLayout.GroupByBox.Appearance = appearance36;
            appearance37.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cboPlantCode_H.DisplayLayout.GroupByBox.BandLabelAppearance = appearance37;
            this.cboPlantCode_H.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance6.BackColor2 = System.Drawing.SystemColors.Control;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cboPlantCode_H.DisplayLayout.GroupByBox.PromptAppearance = appearance6;
            this.cboPlantCode_H.DisplayLayout.MaxColScrollRegions = 1;
            this.cboPlantCode_H.DisplayLayout.MaxRowScrollRegions = 1;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            appearance7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboPlantCode_H.DisplayLayout.Override.ActiveCellAppearance = appearance7;
            appearance8.BackColor = System.Drawing.SystemColors.Highlight;
            appearance8.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cboPlantCode_H.DisplayLayout.Override.ActiveRowAppearance = appearance8;
            appearance9.FontData.BoldAsString = "True";
            this.cboPlantCode_H.DisplayLayout.Override.ActiveRowCellAppearance = appearance9;
            this.cboPlantCode_H.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cboPlantCode_H.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cboPlantCode_H.DisplayLayout.Override.BorderStyleSpecialRowSeparator = Infragistics.Win.UIElementBorderStyle.None;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            this.cboPlantCode_H.DisplayLayout.Override.CardAreaAppearance = appearance10;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance11.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cboPlantCode_H.DisplayLayout.Override.CellAppearance = appearance11;
            this.cboPlantCode_H.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cboPlantCode_H.DisplayLayout.Override.CellPadding = 0;
            appearance12.BackColor = System.Drawing.SystemColors.Control;
            appearance12.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance12.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance12.BorderColor = System.Drawing.SystemColors.Window;
            this.cboPlantCode_H.DisplayLayout.Override.GroupByRowAppearance = appearance12;
            appearance13.TextHAlignAsString = "Left";
            this.cboPlantCode_H.DisplayLayout.Override.HeaderAppearance = appearance13;
            this.cboPlantCode_H.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cboPlantCode_H.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.Color.Silver;
            this.cboPlantCode_H.DisplayLayout.Override.RowAppearance = appearance14;
            this.cboPlantCode_H.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(133)))), ((int)(((byte)(188)))));
            appearance15.FontData.BoldAsString = "True";
            this.cboPlantCode_H.DisplayLayout.Override.SelectedRowAppearance = appearance15;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cboPlantCode_H.DisplayLayout.Override.TemplateAddRowAppearance = appearance16;
            this.cboPlantCode_H.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cboPlantCode_H.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cboPlantCode_H.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.cboPlantCode_H.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cboPlantCode_H.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboPlantCode_H.Location = new System.Drawing.Point(73, 19);
            this.cboPlantCode_H.MajorCode = "PLANTCODE";
            this.cboPlantCode_H.Name = "cboPlantCode_H";
            this.cboPlantCode_H.ShowDefaultValue = false;
            this.cboPlantCode_H.Size = new System.Drawing.Size(119, 24);
            this.cboPlantCode_H.TabIndex = 548;
            this.cboPlantCode_H.Text = "ALL";
            // 
            // txtWorkCenterCode
            // 
            this.txtWorkCenterCode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWorkCenterCode.Location = new System.Drawing.Point(616, 19);
            this.txtWorkCenterCode.Name = "txtWorkCenterCode";
            this.txtWorkCenterCode.Size = new System.Drawing.Size(119, 25);
            this.txtWorkCenterCode.TabIndex = 549;
            // 
            // txtWorkCenterName
            // 
            this.txtWorkCenterName.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtWorkCenterName.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWorkCenterName.Location = new System.Drawing.Point(741, 19);
            this.txtWorkCenterName.MaxLength = 30;
            this.txtWorkCenterName.Name = "txtWorkCenterName";
            this.txtWorkCenterName.Size = new System.Drawing.Size(199, 25);
            this.txtWorkCenterName.TabIndex = 550;
            // 
            // lblWorkCenterCode
            // 
            appearance21.FontData.Name = "맑은 고딕";
            appearance21.FontData.SizeInPoints = 9F;
            appearance21.TextHAlignAsString = "Right";
            appearance21.TextVAlignAsString = "Middle";
            this.lblWorkCenterCode.Appearance = appearance21;
            this.lblWorkCenterCode.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblWorkCenterCode.DbField = null;
            this.lblWorkCenterCode.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWorkCenterCode.Location = new System.Drawing.Point(545, 18);
            this.lblWorkCenterCode.Name = "lblWorkCenterCode";
            this.lblWorkCenterCode.Size = new System.Drawing.Size(65, 29);
            this.lblWorkCenterCode.TabIndex = 551;
            this.lblWorkCenterCode.Text = "작업장";
            // 
            // calToDt
            // 
            appearance3.FontData.SizeInPoints = 10F;
            this.calToDt.Appearance = appearance3;
            dateButton4.Date = new System.DateTime(2017, 3, 7, 0, 0, 0, 0);
            this.calToDt.DateButtons.Add(dateButton4);
            this.calToDt.Location = new System.Drawing.Point(427, 19);
            this.calToDt.Name = "calToDt";
            this.calToDt.NonAutoSizeHeight = 26;
            this.calToDt.Size = new System.Drawing.Size(109, 24);
            this.calToDt.TabIndex = 552;
            this.calToDt.Value = new System.DateTime(2017, 3, 7, 0, 0, 0, 0);
            // 
            // sLabel2
            // 
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.sLabel2.Appearance = appearance4;
            this.sLabel2.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.sLabel2.DbField = null;
            this.sLabel2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sLabel2.Location = new System.Drawing.Point(411, 19);
            this.sLabel2.Name = "sLabel2";
            this.sLabel2.Size = new System.Drawing.Size(12, 25);
            this.sLabel2.TabIndex = 553;
            this.sLabel2.Text = "~";
            // 
            // txtOPCode
            // 
            this.txtOPCode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtOPCode.Location = new System.Drawing.Point(348, 328);
            this.txtOPCode.Name = "txtOPCode";
            this.txtOPCode.Size = new System.Drawing.Size(119, 25);
            this.txtOPCode.TabIndex = 6;
            this.txtOPCode.Visible = false;
            // 
            // txtOPName
            // 
            this.txtOPName.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtOPName.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtOPName.Location = new System.Drawing.Point(473, 328);
            this.txtOPName.MaxLength = 30;
            this.txtOPName.Name = "txtOPName";
            this.txtOPName.Size = new System.Drawing.Size(199, 25);
            this.txtOPName.TabIndex = 7;
            this.txtOPName.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageChart);
            this.tabControl1.Controls.Add(this.tabPageGrid);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1008, 668);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPageChart
            // 
            this.tabPageChart.Controls.Add(this.chartCastTemp);
            this.tabPageChart.Location = new System.Drawing.Point(4, 29);
            this.tabPageChart.Name = "tabPageChart";
            this.tabPageChart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChart.Size = new System.Drawing.Size(1000, 635);
            this.tabPageChart.TabIndex = 0;
            this.tabPageChart.Text = "[ 차트 ]";
            this.tabPageChart.UseVisualStyleBackColor = true;
            // 
            // tabPageGrid
            // 
            this.tabPageGrid.Controls.Add(this.grid1);
            this.tabPageGrid.Location = new System.Drawing.Point(4, 29);
            this.tabPageGrid.Name = "tabPageGrid";
            this.tabPageGrid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGrid.Size = new System.Drawing.Size(1000, 635);
            this.tabPageGrid.TabIndex = 1;
            this.tabPageGrid.Text = "[ 현황 ]";
            this.tabPageGrid.UseVisualStyleBackColor = true;
            // 
            // grid1
            // 
            this.grid1.ContextMenuCopyEnabled = true;
            this.grid1.ContextMenuDeleteEnabled = true;
            this.grid1.ContextMenuExcelEnabled = true;
            this.grid1.ContextMenuInsertEnabled = true;
            this.grid1.ContextMenuPasteEnabled = true;
            appearance98.BackColor = System.Drawing.SystemColors.Window;
            appearance98.BorderColor = System.Drawing.Color.CornflowerBlue;
            appearance98.BorderColor2 = System.Drawing.Color.CornflowerBlue;
            this.grid1.DisplayLayout.Appearance = appearance98;
            this.grid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.TwoColor;
            this.grid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grid1.DisplayLayout.DefaultSelectedBackColor = System.Drawing.Color.Empty;
            appearance99.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance99.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance99.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance99.BorderColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.GroupByBox.Appearance = appearance99;
            appearance100.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grid1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance100;
            this.grid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.GroupByBox.Hidden = true;
            appearance101.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance101.BackColor2 = System.Drawing.SystemColors.Control;
            appearance101.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance101.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grid1.DisplayLayout.GroupByBox.PromptAppearance = appearance101;
            this.grid1.DisplayLayout.MaxColScrollRegions = 1;
            this.grid1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance102.BackColor = System.Drawing.SystemColors.Window;
            appearance102.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grid1.DisplayLayout.Override.ActiveCellAppearance = appearance102;
            appearance103.BackColor = System.Drawing.SystemColors.Highlight;
            appearance103.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grid1.DisplayLayout.Override.ActiveRowAppearance = appearance103;
            this.grid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.DisplayLayout.Override.AllowMultiCellOperations = ((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation)((((((((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Copy | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.CopyWithHeaders) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Cut) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Delete) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Paste) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Undo) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Redo) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Reserved)));
            this.grid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance104.BackColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.Override.CardAreaAppearance = appearance104;
            appearance105.BorderColor = System.Drawing.Color.Silver;
            appearance105.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grid1.DisplayLayout.Override.CellAppearance = appearance105;
            this.grid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grid1.DisplayLayout.Override.CellPadding = 0;
            appearance106.BackColor = System.Drawing.SystemColors.Control;
            appearance106.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance106.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance106.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance106.BorderColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.Override.GroupByRowAppearance = appearance106;
            appearance107.TextHAlignAsString = "Left";
            this.grid1.DisplayLayout.Override.HeaderAppearance = appearance107;
            this.grid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance108.BackColor = System.Drawing.SystemColors.Window;
            appearance108.BorderColor = System.Drawing.Color.Silver;
            this.grid1.DisplayLayout.Override.RowAppearance = appearance108;
            this.grid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance109.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grid1.DisplayLayout.Override.TemplateAddRowAppearance = appearance109;
            this.grid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grid1.DisplayLayout.SelectionOverlayBorderThickness = 2;
            this.grid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grid1.Location = new System.Drawing.Point(3, 3);
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(994, 629);
            this.grid1.TabIndex = 138;
            this.grid1.Text = "grid1";
            this.grid1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.grid1.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grid1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chartCastTemp
            // 
            this.chartCastTemp.DataBindings = null;
            this.chartCastTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartCastTemp.Legend.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Solid;
            this.chartCastTemp.Legend.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartCastTemp.Legend.HorizontalIndent = 20;
            this.chartCastTemp.Legend.MarkerSize = new System.Drawing.Size(30, 20);
            this.chartCastTemp.Legend.Name = "Default Legend";
            this.chartCastTemp.Legend.TextOffset = 8;
            this.chartCastTemp.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            this.chartCastTemp.Location = new System.Drawing.Point(3, 3);
            this.chartCastTemp.LookAndFeel.SkinName = "Visual Studio 2013 Blue";
            this.chartCastTemp.LookAndFeel.UseDefaultLookAndFeel = false;
            this.chartCastTemp.Name = "chartCastTemp";
            this.chartCastTemp.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartCastTemp.Size = new System.Drawing.Size(994, 629);
            this.chartCastTemp.TabIndex = 0;
            // 
            // PP0342
            // 
            this.ClientSize = new System.Drawing.Size(1020, 742);
            this.Name = "PP0342";
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).EndInit();
            this.gbxHeader.ResumeLayout(false);
            this.gbxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).EndInit();
            this.gbxBody.ResumeLayout(false);
            this.gbxBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.calFromDt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlantCode_H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.calToDt)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageChart.ResumeLayout(false);
            this.tabPageGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCastTemp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Control.SLabel sLabel1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo calFromDt;
        private Control.SLabel sLabel3;
        private Control.SCodeNMComboBox cboPlantCode_H;
        private System.Windows.Forms.TextBox txtWorkCenterCode;
        private System.Windows.Forms.TextBox txtWorkCenterName;
        private Control.SLabel lblWorkCenterCode;
        private Control.SLabel sLabel2;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo calToDt;
        private System.Windows.Forms.TextBox txtOPCode;
        private System.Windows.Forms.TextBox txtOPName;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageChart;
        private System.Windows.Forms.TabPage tabPageGrid;
        private Control.Grid grid1;
        private DevExpress.XtraCharts.ChartControl chartCastTemp;
    }
}
