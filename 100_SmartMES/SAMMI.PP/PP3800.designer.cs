namespace SAMMI.PP
{
    partial class PP3800
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
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
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
            this.grid1 = new SAMMI.Control.Grid(this.components);
            this.txtLineCode = new System.Windows.Forms.TextBox();
            this.txtLineName = new System.Windows.Forms.TextBox();
            this.txtWorkCenterCode = new System.Windows.Forms.TextBox();
            this.txtWorkCenterName = new System.Windows.Forms.TextBox();
            this.lblLineCode = new SAMMI.Control.SLabel();
            this.lblWorkCenterCode = new SAMMI.Control.SLabel();
            this.CboStartdate_H = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.CboEnddate_H = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.lblUseFlag = new SAMMI.Control.SLabel();
            this.sLabel2 = new SAMMI.Control.SLabel();
            this.cboPlantCode_H = new SAMMI.Control.SCodeNMComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).BeginInit();
            this.gbxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).BeginInit();
            this.gbxBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CboStartdate_H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CboEnddate_H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlantCode_H)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxHeader
            // 
            this.gbxHeader.ContentPadding.Bottom = 2;
            this.gbxHeader.ContentPadding.Left = 2;
            this.gbxHeader.ContentPadding.Right = 2;
            this.gbxHeader.ContentPadding.Top = 4;
            this.gbxHeader.Controls.Add(this.cboPlantCode_H);
            this.gbxHeader.Controls.Add(this.sLabel2);
            this.gbxHeader.Controls.Add(this.CboStartdate_H);
            this.gbxHeader.Controls.Add(this.CboEnddate_H);
            this.gbxHeader.Controls.Add(this.ultraLabel4);
            this.gbxHeader.Controls.Add(this.lblUseFlag);
            this.gbxHeader.Controls.Add(this.txtLineCode);
            this.gbxHeader.Controls.Add(this.txtLineName);
            this.gbxHeader.Controls.Add(this.txtWorkCenterCode);
            this.gbxHeader.Controls.Add(this.txtWorkCenterName);
            this.gbxHeader.Controls.Add(this.lblLineCode);
            this.gbxHeader.Controls.Add(this.lblWorkCenterCode);
            this.gbxHeader.Size = new System.Drawing.Size(1184, 77);
            // 
            // gbxBody
            // 
            this.gbxBody.ContentPadding.Bottom = 4;
            this.gbxBody.ContentPadding.Left = 4;
            this.gbxBody.ContentPadding.Right = 4;
            this.gbxBody.ContentPadding.Top = 6;
            this.gbxBody.Controls.Add(this.grid1);
            this.gbxBody.Location = new System.Drawing.Point(0, 77);
            this.gbxBody.Size = new System.Drawing.Size(1184, 748);
            // 
            // grid1
            // 
            this.grid1.ContextMenuCopyEnabled = true;
            this.grid1.ContextMenuDeleteEnabled = true;
            this.grid1.ContextMenuExcelEnabled = true;
            this.grid1.ContextMenuInsertEnabled = true;
            this.grid1.ContextMenuPasteEnabled = true;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grid1.DisplayLayout.Appearance = appearance23;
            this.grid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grid1.DisplayLayout.DefaultSelectedBackColor = System.Drawing.Color.Empty;
            appearance24.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.BorderColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.GroupByBox.Appearance = appearance24;
            appearance25.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grid1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance25;
            this.grid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.GroupByBox.Hidden = true;
            appearance26.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance26.BackColor2 = System.Drawing.SystemColors.Control;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance26.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grid1.DisplayLayout.GroupByBox.PromptAppearance = appearance26;
            this.grid1.DisplayLayout.MaxColScrollRegions = 1;
            this.grid1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance27.BackColor = System.Drawing.SystemColors.Window;
            appearance27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grid1.DisplayLayout.Override.ActiveCellAppearance = appearance27;
            appearance28.BackColor = System.Drawing.SystemColors.Highlight;
            appearance28.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grid1.DisplayLayout.Override.ActiveRowAppearance = appearance28;
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
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.Override.CardAreaAppearance = appearance29;
            appearance30.BorderColor = System.Drawing.Color.Silver;
            appearance30.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grid1.DisplayLayout.Override.CellAppearance = appearance30;
            this.grid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grid1.DisplayLayout.Override.CellPadding = 0;
            appearance31.BackColor = System.Drawing.SystemColors.Control;
            appearance31.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance31.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance31.BorderColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.Override.GroupByRowAppearance = appearance31;
            appearance17.TextHAlignAsString = "Left";
            this.grid1.DisplayLayout.Override.HeaderAppearance = appearance17;
            this.grid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            appearance18.BorderColor = System.Drawing.Color.Silver;
            this.grid1.DisplayLayout.Override.RowAppearance = appearance18;
            this.grid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance19.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grid1.DisplayLayout.Override.TemplateAddRowAppearance = appearance19;
            this.grid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grid1.DisplayLayout.SelectionOverlayBorderThickness = 2;
            this.grid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grid1.Location = new System.Drawing.Point(6, 6);
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(1172, 736);
            this.grid1.TabIndex = 2;
            this.grid1.Text = "grid2";
            this.grid1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.grid1.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grid1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtLineCode
            // 
            this.txtLineCode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtLineCode.Location = new System.Drawing.Point(142, 105);
            this.txtLineCode.Name = "txtLineCode";
            this.txtLineCode.Size = new System.Drawing.Size(119, 25);
            this.txtLineCode.TabIndex = 310;
            this.txtLineCode.Visible = false;
            // 
            // txtLineName
            // 
            this.txtLineName.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLineName.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtLineName.Location = new System.Drawing.Point(267, 105);
            this.txtLineName.MaxLength = 30;
            this.txtLineName.Name = "txtLineName";
            this.txtLineName.Size = new System.Drawing.Size(199, 25);
            this.txtLineName.TabIndex = 311;
            this.txtLineName.Visible = false;
            // 
            // txtWorkCenterCode
            // 
            this.txtWorkCenterCode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWorkCenterCode.Location = new System.Drawing.Point(73, 45);
            this.txtWorkCenterCode.Name = "txtWorkCenterCode";
            this.txtWorkCenterCode.Size = new System.Drawing.Size(119, 25);
            this.txtWorkCenterCode.TabIndex = 2;
            this.txtWorkCenterCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtWorkCenterCode_KeyDown);
            this.txtWorkCenterCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWorkCenterCode_KeyPress);
            this.txtWorkCenterCode.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtWorkCenterCode_MouseDoubleClick);
            // 
            // txtWorkCenterName
            // 
            this.txtWorkCenterName.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtWorkCenterName.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWorkCenterName.Location = new System.Drawing.Point(198, 45);
            this.txtWorkCenterName.MaxLength = 30;
            this.txtWorkCenterName.Name = "txtWorkCenterName";
            this.txtWorkCenterName.Size = new System.Drawing.Size(199, 25);
            this.txtWorkCenterName.TabIndex = 3;
            this.txtWorkCenterName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWorkCenterName_KeyPress);
            this.txtWorkCenterName.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtWorkCenterName_MouseDoubleClick);
            // 
            // lblLineCode
            // 
            appearance20.FontData.Name = "맑은 고딕";
            appearance20.FontData.SizeInPoints = 9F;
            appearance20.TextHAlignAsString = "Right";
            appearance20.TextVAlignAsString = "Middle";
            this.lblLineCode.Appearance = appearance20;
            this.lblLineCode.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblLineCode.DbField = null;
            this.lblLineCode.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLineCode.Location = new System.Drawing.Point(5, 103);
            this.lblLineCode.Name = "lblLineCode";
            this.lblLineCode.Size = new System.Drawing.Size(130, 27);
            this.lblLineCode.TabIndex = 300;
            this.lblLineCode.Text = "라인";
            this.lblLineCode.Visible = false;
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
            this.lblWorkCenterCode.Location = new System.Drawing.Point(1, 41);
            this.lblWorkCenterCode.Name = "lblWorkCenterCode";
            this.lblWorkCenterCode.Size = new System.Drawing.Size(63, 29);
            this.lblWorkCenterCode.TabIndex = 298;
            this.lblWorkCenterCode.Text = "작업장";
            // 
            // CboStartdate_H
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "맑은 고딕";
            appearance1.FontData.SizeInPoints = 9.75F;
            this.CboStartdate_H.Appearance = appearance1;
            this.CboStartdate_H.DateButtons.Add(dateButton1);
            this.CboStartdate_H.Location = new System.Drawing.Point(293, 15);
            this.CboStartdate_H.Name = "CboStartdate_H";
            this.CboStartdate_H.NonAutoSizeHeight = 26;
            this.CboStartdate_H.Size = new System.Drawing.Size(105, 23);
            this.CboStartdate_H.TabIndex = 4;
            // 
            // CboEnddate_H
            // 
            appearance5.FontData.BoldAsString = "False";
            appearance5.FontData.Name = "맑은 고딕";
            appearance5.FontData.SizeInPoints = 9.75F;
            this.CboEnddate_H.Appearance = appearance5;
            this.CboEnddate_H.DateButtons.Add(dateButton2);
            this.CboEnddate_H.Location = new System.Drawing.Point(418, 15);
            this.CboEnddate_H.Name = "CboEnddate_H";
            this.CboEnddate_H.NonAutoSizeHeight = 26;
            this.CboEnddate_H.Size = new System.Drawing.Size(105, 23);
            this.CboEnddate_H.TabIndex = 5;
            // 
            // ultraLabel4
            // 
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance3;
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ultraLabel4.Location = new System.Drawing.Point(401, 17);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(13, 18);
            this.ultraLabel4.TabIndex = 315;
            this.ultraLabel4.Text = "~";
            // 
            // lblUseFlag
            // 
            appearance4.FontData.Name = "맑은 고딕";
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.lblUseFlag.Appearance = appearance4;
            this.lblUseFlag.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblUseFlag.DbField = null;
            this.lblUseFlag.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUseFlag.Location = new System.Drawing.Point(216, 13);
            this.lblUseFlag.Name = "lblUseFlag";
            this.lblUseFlag.Size = new System.Drawing.Size(68, 27);
            this.lblUseFlag.TabIndex = 314;
            this.lblUseFlag.Text = "투입일자";
            // 
            // sLabel2
            // 
            appearance22.TextHAlignAsString = "Right";
            appearance22.TextVAlignAsString = "Middle";
            this.sLabel2.Appearance = appearance22;
            this.sLabel2.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.sLabel2.DbField = "cboUseFlag";
            this.sLabel2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.sLabel2.Location = new System.Drawing.Point(12, 14);
            this.sLabel2.Name = "sLabel2";
            this.sLabel2.Size = new System.Drawing.Size(52, 26);
            this.sLabel2.TabIndex = 550;
            this.sLabel2.Text = "사업장";
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
            this.cboPlantCode_H.Location = new System.Drawing.Point(73, 15);
            this.cboPlantCode_H.MajorCode = "PLANTCODE";
            this.cboPlantCode_H.Name = "cboPlantCode_H";
            this.cboPlantCode_H.ShowDefaultValue = false;
            this.cboPlantCode_H.Size = new System.Drawing.Size(119, 24);
            this.cboPlantCode_H.TabIndex = 558;
            this.cboPlantCode_H.Text = "ALL";
            // 
            // PP3800
            // 
            this.ClientSize = new System.Drawing.Size(1184, 825);
            this.Name = "PP3800";
            this.Text = "투입 반제품 재공현황(품번별)";
            this.Load += new System.EventHandler(this.PP3800_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).EndInit();
            this.gbxHeader.ResumeLayout(false);
            this.gbxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).EndInit();
            this.gbxBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CboStartdate_H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CboEnddate_H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlantCode_H)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Control.Grid grid1;
        private System.Windows.Forms.TextBox txtLineCode;
        private System.Windows.Forms.TextBox txtLineName;
        private System.Windows.Forms.TextBox txtWorkCenterCode;
        private System.Windows.Forms.TextBox txtWorkCenterName;
        private Control.SLabel lblLineCode;
        private Control.SLabel lblWorkCenterCode;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo CboStartdate_H;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo CboEnddate_H;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Control.SLabel lblUseFlag;
        private Control.SLabel sLabel2;
        private Control.SCodeNMComboBox cboPlantCode_H;
    }
}
