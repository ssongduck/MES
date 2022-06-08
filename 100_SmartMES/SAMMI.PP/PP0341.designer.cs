namespace SAMMI.PP
{
    partial class PP0341
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
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
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
            this.sLabel1 = new SAMMI.Control.SLabel();
            this.calRegDT = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.sLabel3 = new SAMMI.Control.SLabel();
            this.cboPlantCode_H = new SAMMI.Control.SCodeNMComboBox();
            this.btnAlloyIF = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).BeginInit();
            this.gbxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).BeginInit();
            this.gbxBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.calRegDT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlantCode_H)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxHeader
            // 
            this.gbxHeader.ContentPadding.Bottom = 2;
            this.gbxHeader.ContentPadding.Left = 2;
            this.gbxHeader.ContentPadding.Right = 2;
            this.gbxHeader.ContentPadding.Top = 4;
            this.gbxHeader.Controls.Add(this.btnAlloyIF);
            this.gbxHeader.Controls.Add(this.cboPlantCode_H);
            this.gbxHeader.Controls.Add(this.sLabel3);
            this.gbxHeader.Controls.Add(this.calRegDT);
            this.gbxHeader.Controls.Add(this.sLabel1);
            this.gbxHeader.Size = new System.Drawing.Size(1020, 62);
            // 
            // gbxBody
            // 
            this.gbxBody.ContentPadding.Bottom = 4;
            this.gbxBody.ContentPadding.Left = 4;
            this.gbxBody.ContentPadding.Right = 4;
            this.gbxBody.ContentPadding.Top = 6;
            this.gbxBody.Controls.Add(this.grid1);
            this.gbxBody.Location = new System.Drawing.Point(0, 62);
            this.gbxBody.Size = new System.Drawing.Size(1020, 680);
            // 
            // grid1
            // 
            this.grid1.ContextMenuCopyEnabled = true;
            this.grid1.ContextMenuDeleteEnabled = true;
            this.grid1.ContextMenuExcelEnabled = true;
            this.grid1.ContextMenuInsertEnabled = true;
            this.grid1.ContextMenuPasteEnabled = true;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grid1.DisplayLayout.Appearance = appearance5;
            this.grid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grid1.DisplayLayout.DefaultSelectedBackColor = System.Drawing.Color.Empty;
            appearance18.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance18.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.BorderColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.GroupByBox.Appearance = appearance18;
            appearance19.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grid1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance19;
            this.grid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.GroupByBox.Hidden = true;
            appearance20.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance20.BackColor2 = System.Drawing.SystemColors.Control;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance20.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grid1.DisplayLayout.GroupByBox.PromptAppearance = appearance20;
            this.grid1.DisplayLayout.MaxColScrollRegions = 1;
            this.grid1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance21.BackColor = System.Drawing.SystemColors.Window;
            appearance21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grid1.DisplayLayout.Override.ActiveCellAppearance = appearance21;
            appearance23.BackColor = System.Drawing.SystemColors.Highlight;
            appearance23.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grid1.DisplayLayout.Override.ActiveRowAppearance = appearance23;
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
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.Override.CardAreaAppearance = appearance24;
            appearance25.BorderColor = System.Drawing.Color.Silver;
            appearance25.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grid1.DisplayLayout.Override.CellAppearance = appearance25;
            this.grid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grid1.DisplayLayout.Override.CellPadding = 0;
            appearance26.BackColor = System.Drawing.SystemColors.Control;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.Override.GroupByRowAppearance = appearance26;
            appearance27.TextHAlignAsString = "Left";
            this.grid1.DisplayLayout.Override.HeaderAppearance = appearance27;
            this.grid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance28.BackColor = System.Drawing.SystemColors.Window;
            appearance28.BorderColor = System.Drawing.Color.Silver;
            this.grid1.DisplayLayout.Override.RowAppearance = appearance28;
            this.grid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance29.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grid1.DisplayLayout.Override.TemplateAddRowAppearance = appearance29;
            this.grid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grid1.DisplayLayout.SelectionOverlayBorderThickness = 2;
            this.grid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grid1.Location = new System.Drawing.Point(6, 6);
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(1008, 668);
            this.grid1.TabIndex = 0;
            this.grid1.Text = "grid1";
            this.grid1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.grid1.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grid1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // sLabel1
            // 
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.sLabel1.Appearance = appearance4;
            this.sLabel1.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.sLabel1.DbField = null;
            this.sLabel1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sLabel1.Location = new System.Drawing.Point(12, 18);
            this.sLabel1.Name = "sLabel1";
            this.sLabel1.Size = new System.Drawing.Size(68, 25);
            this.sLabel1.TabIndex = 245;
            this.sLabel1.Text = "기준일자";
            // 
            // calRegDT
            // 
            appearance3.FontData.SizeInPoints = 10F;
            this.calRegDT.Appearance = appearance3;
            dateButton1.Date = new System.DateTime(2017, 3, 7, 0, 0, 0, 0);
            this.calRegDT.DateButtons.Add(dateButton1);
            this.calRegDT.Location = new System.Drawing.Point(98, 19);
            this.calRegDT.Name = "calRegDT";
            this.calRegDT.NonAutoSizeHeight = 26;
            this.calRegDT.Size = new System.Drawing.Size(109, 24);
            this.calRegDT.TabIndex = 2;
            this.calRegDT.Value = new System.DateTime(2017, 3, 7, 0, 0, 0, 0);
            // 
            // sLabel3
            // 
            appearance22.TextHAlignAsString = "Right";
            appearance22.TextVAlignAsString = "Middle";
            this.sLabel3.Appearance = appearance22;
            this.sLabel3.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.sLabel3.DbField = "cboUseFlag";
            this.sLabel3.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.sLabel3.Location = new System.Drawing.Point(576, 18);
            this.sLabel3.Name = "sLabel3";
            this.sLabel3.Size = new System.Drawing.Size(52, 26);
            this.sLabel3.TabIndex = 530;
            this.sLabel3.Text = "사업장";
            this.sLabel3.Visible = false;
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
            this.cboPlantCode_H.Location = new System.Drawing.Point(634, 19);
            this.cboPlantCode_H.MajorCode = "PLANTCODE";
            this.cboPlantCode_H.Name = "cboPlantCode_H";
            this.cboPlantCode_H.ShowDefaultValue = false;
            this.cboPlantCode_H.Size = new System.Drawing.Size(119, 24);
            this.cboPlantCode_H.TabIndex = 548;
            this.cboPlantCode_H.Text = "ALL";
            this.cboPlantCode_H.Visible = false;
            // 
            // btnAlloyIF
            // 
            this.btnAlloyIF.Location = new System.Drawing.Point(455, 16);
            this.btnAlloyIF.Name = "btnAlloyIF";
            this.btnAlloyIF.Size = new System.Drawing.Size(111, 31);
            this.btnAlloyIF.TabIndex = 550;
            this.btnAlloyIF.Text = "합금실적전송";
            this.btnAlloyIF.Visible = false;
            // 
            // PP0341
            // 
            this.ClientSize = new System.Drawing.Size(1020, 742);
            this.Name = "PP0341";
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).EndInit();
            this.gbxHeader.ResumeLayout(false);
            this.gbxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).EndInit();
            this.gbxBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.calRegDT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlantCode_H)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Control.Grid grid1;
        private Control.SLabel sLabel1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo calRegDT;
        private Control.SLabel sLabel3;
        private Control.SCodeNMComboBox cboPlantCode_H;
        private Infragistics.Win.Misc.UltraButton btnAlloyIF;
    }
}
