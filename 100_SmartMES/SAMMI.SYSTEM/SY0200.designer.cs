namespace SAMMI.SY
{
    partial class SY0200
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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            this.ultraTextEditor1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.cboUseFlag = new SAMMI.Control.SCodeNMComboBox();
            this.txtWorkerID_H = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblProgramID_H = new SAMMI.Control.SLabel();
            this.lblUseFlag_H = new SAMMI.Control.SLabel();
            this.btnCopy = new Infragistics.Win.Misc.UltraButton();
            this.sLabel3 = new SAMMI.Control.SLabel();
            this.txtWorkerName_H = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.cboUseFlag_H = new System.Windows.Forms.ComboBox();
            this.grid1 = new SAMMI.Control.Grid(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).BeginInit();
            this.gbxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).BeginInit();
            this.gbxBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUseFlag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWorkerID_H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWorkerName_H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxHeader
            // 
            this.gbxHeader.ContentPadding.Bottom = 2;
            this.gbxHeader.ContentPadding.Left = 2;
            this.gbxHeader.ContentPadding.Right = 2;
            this.gbxHeader.ContentPadding.Top = 4;
            this.gbxHeader.Controls.Add(this.cboUseFlag_H);
            this.gbxHeader.Controls.Add(this.btnCopy);
            this.gbxHeader.Controls.Add(this.sLabel3);
            this.gbxHeader.Controls.Add(this.lblUseFlag_H);
            this.gbxHeader.Controls.Add(this.txtWorkerName_H);
            this.gbxHeader.Controls.Add(this.txtWorkerID_H);
            this.gbxHeader.Controls.Add(this.lblProgramID_H);
            // 
            // gbxBody
            // 
            this.gbxBody.ContentPadding.Bottom = 4;
            this.gbxBody.ContentPadding.Left = 4;
            this.gbxBody.ContentPadding.Right = 4;
            this.gbxBody.ContentPadding.Top = 6;
            this.gbxBody.Controls.Add(this.grid1);
            this.gbxBody.Controls.Add(this.ultraTextEditor1);
            this.gbxBody.Controls.Add(this.cboUseFlag);
            // 
            // ultraTextEditor1
            // 
            this.ultraTextEditor1.Location = new System.Drawing.Point(210, 164);
            this.ultraTextEditor1.Name = "ultraTextEditor1";
            this.ultraTextEditor1.PasswordChar = '*';
            this.ultraTextEditor1.Size = new System.Drawing.Size(152, 29);
            this.ultraTextEditor1.TabIndex = 28;
            this.ultraTextEditor1.Text = "txtPassword";
            // 
            // cboUseFlag
            // 
            this.cboUseFlag.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboUseFlag.ComboDataType = SAMMI.Control.ComboDataType.CodeOnly;
            this.cboUseFlag.DbConfig = null;
            this.cboUseFlag.DefaultValue = "";
            appearance5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cboUseFlag.DisplayLayout.Appearance = appearance5;
            this.cboUseFlag.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cboUseFlag.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboUseFlag.DisplayLayout.BorderStyleCaption = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance6.BackColor = System.Drawing.Color.Gray;
            this.cboUseFlag.DisplayLayout.CaptionAppearance = appearance6;
            this.cboUseFlag.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cboUseFlag.DisplayLayout.DefaultSelectedBackColor = System.Drawing.Color.RoyalBlue;
            this.cboUseFlag.DisplayLayout.InterBandSpacing = 2;
            this.cboUseFlag.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.True;
            appearance7.BackColor = System.Drawing.Color.RoyalBlue;
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.White;
            this.cboUseFlag.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            appearance8.FontData.BoldAsString = "True";
            this.cboUseFlag.DisplayLayout.Override.ActiveRowCellAppearance = appearance8;
            this.cboUseFlag.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
            this.cboUseFlag.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
            this.cboUseFlag.DisplayLayout.Override.BorderStyleSpecialRowSeparator = Infragistics.Win.UIElementBorderStyle.None;
            this.cboUseFlag.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2010ScrollbarButton;
            appearance9.BackColor = System.Drawing.Color.DimGray;
            appearance9.BackColor2 = System.Drawing.Color.Silver;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.BorderColor = System.Drawing.Color.White;
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            this.cboUseFlag.DisplayLayout.Override.HeaderAppearance = appearance9;
            this.cboUseFlag.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance10.BackColor2 = System.Drawing.Color.Gray;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboUseFlag.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance10;
            this.cboUseFlag.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.cboUseFlag.DisplayLayout.Override.RowSelectorNumberStyle = Infragistics.Win.UltraWinGrid.RowSelectorNumberStyle.RowIndex;
            this.cboUseFlag.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.cboUseFlag.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(133)))), ((int)(((byte)(188)))));
            appearance11.FontData.BoldAsString = "True";
            this.cboUseFlag.DisplayLayout.Override.SelectedRowAppearance = appearance11;
            this.cboUseFlag.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.True;
            this.cboUseFlag.DisplayLayout.RowConnectorColor = System.Drawing.Color.Silver;
            this.cboUseFlag.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.cboUseFlag.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.cboUseFlag.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cboUseFlag.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.cboUseFlag.Location = new System.Drawing.Point(322, 164);
            this.cboUseFlag.MajorCode = "USEFLAG";
            this.cboUseFlag.Name = "cboUseFlag";
            this.cboUseFlag.ShowDefaultValue = true;
            this.cboUseFlag.Size = new System.Drawing.Size(169, 25);
            this.cboUseFlag.TabIndex = 27;
            // 
            // txtWorkerID_H
            // 
            this.txtWorkerID_H.AutoSize = false;
            this.txtWorkerID_H.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWorkerID_H.Location = new System.Drawing.Point(149, 24);
            this.txtWorkerID_H.Name = "txtWorkerID_H";
            this.txtWorkerID_H.Size = new System.Drawing.Size(120, 25);
            this.txtWorkerID_H.TabIndex = 3;
            // 
            // lblProgramID_H
            // 
            appearance24.TextHAlignAsString = "Right";
            this.lblProgramID_H.Appearance = appearance24;
            this.lblProgramID_H.DbField = null;
            this.lblProgramID_H.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProgramID_H.Location = new System.Drawing.Point(43, 24);
            this.lblProgramID_H.Name = "lblProgramID_H";
            this.lblProgramID_H.Size = new System.Drawing.Size(100, 25);
            this.lblProgramID_H.TabIndex = 2;
            this.lblProgramID_H.Text = "사용자";
            // 
            // lblUseFlag_H
            // 
            appearance25.TextHAlignAsString = "Right";
            this.lblUseFlag_H.Appearance = appearance25;
            this.lblUseFlag_H.DbField = null;
            this.lblUseFlag_H.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUseFlag_H.Location = new System.Drawing.Point(530, 24);
            this.lblUseFlag_H.Name = "lblUseFlag_H";
            this.lblUseFlag_H.Size = new System.Drawing.Size(100, 25);
            this.lblUseFlag_H.TabIndex = 21;
            this.lblUseFlag_H.Text = "사용여부";
            // 
            // btnCopy
            // 
            appearance26.FontData.Name = "맑은 고딕";
            appearance26.FontData.SizeInPoints = 15F;
            this.btnCopy.Appearance = appearance26;
            this.btnCopy.Location = new System.Drawing.Point(1020, 12);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(108, 45);
            this.btnCopy.TabIndex = 23;
            this.btnCopy.Text = "복 사";
            this.btnCopy.Visible = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // sLabel3
            // 
            appearance27.TextHAlignAsString = "Right";
            this.sLabel3.Appearance = appearance27;
            this.sLabel3.DbField = null;
            this.sLabel3.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sLabel3.Location = new System.Drawing.Point(916, 26);
            this.sLabel3.Name = "sLabel3";
            this.sLabel3.Size = new System.Drawing.Size(98, 23);
            this.sLabel3.TabIndex = 22;
            this.sLabel3.Text = "사용자 복사";
            this.sLabel3.Visible = false;
            // 
            // txtWorkerName_H
            // 
            this.txtWorkerName_H.AutoSize = false;
            this.txtWorkerName_H.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWorkerName_H.Location = new System.Drawing.Point(275, 24);
            this.txtWorkerName_H.Name = "txtWorkerName_H";
            this.txtWorkerName_H.Size = new System.Drawing.Size(170, 25);
            this.txtWorkerName_H.TabIndex = 3;
            // 
            // cboUseFlag_H
            // 
            this.cboUseFlag_H.FormattingEnabled = true;
            this.cboUseFlag_H.Location = new System.Drawing.Point(636, 24);
            this.cboUseFlag_H.Name = "cboUseFlag_H";
            this.cboUseFlag_H.Size = new System.Drawing.Size(119, 28);
            this.cboUseFlag_H.TabIndex = 24;
            // 
            // grid1
            // 
            this.grid1.ContextMenuCopyEnabled = true;
            this.grid1.ContextMenuDeleteEnabled = true;
            this.grid1.ContextMenuExcelEnabled = true;
            this.grid1.ContextMenuInsertEnabled = true;
            this.grid1.ContextMenuPasteEnabled = true;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grid1.DisplayLayout.Appearance = appearance12;
            this.grid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grid1.DisplayLayout.DefaultSelectedBackColor = System.Drawing.Color.Empty;
            appearance13.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.GroupByBox.Appearance = appearance13;
            appearance14.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grid1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance14;
            this.grid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.GroupByBox.Hidden = true;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance15.BackColor2 = System.Drawing.SystemColors.Control;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grid1.DisplayLayout.GroupByBox.PromptAppearance = appearance15;
            this.grid1.DisplayLayout.MaxColScrollRegions = 1;
            this.grid1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            appearance16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grid1.DisplayLayout.Override.ActiveCellAppearance = appearance16;
            appearance17.BackColor = System.Drawing.SystemColors.Highlight;
            appearance17.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grid1.DisplayLayout.Override.ActiveRowAppearance = appearance17;
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
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.Override.CardAreaAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.Silver;
            appearance19.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grid1.DisplayLayout.Override.CellAppearance = appearance19;
            this.grid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grid1.DisplayLayout.Override.CellPadding = 0;
            appearance20.BackColor = System.Drawing.SystemColors.Control;
            appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance20.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance20.BorderColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.Override.GroupByRowAppearance = appearance20;
            appearance21.TextHAlignAsString = "Left";
            this.grid1.DisplayLayout.Override.HeaderAppearance = appearance21;
            this.grid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance22.BackColor = System.Drawing.SystemColors.Window;
            appearance22.BorderColor = System.Drawing.Color.Silver;
            this.grid1.DisplayLayout.Override.RowAppearance = appearance22;
            this.grid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.grid1.DisplayLayout.Override.RowSelectorNumberStyle = Infragistics.Win.UltraWinGrid.RowSelectorNumberStyle.RowIndex;
            this.grid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance23.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grid1.DisplayLayout.Override.TemplateAddRowAppearance = appearance23;
            this.grid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grid1.DisplayLayout.SelectionOverlayBorderThickness = 2;
            this.grid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grid1.Location = new System.Drawing.Point(6, 6);
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(1124, 744);
            this.grid1.TabIndex = 29;
            this.grid1.Text = "grid1";
            this.grid1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.grid1.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grid1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // SY0200
            // 
            this.ClientSize = new System.Drawing.Size(1136, 825);
            this.Name = "SY0200";
            this.Load += new System.EventHandler(this.SY0200_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).EndInit();
            this.gbxHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).EndInit();
            this.gbxBody.ResumeLayout(false);
            this.gbxBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUseFlag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWorkerID_H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWorkerName_H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Control.SLabel lblUseFlag_H;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWorkerID_H;
        private Control.SLabel lblProgramID_H;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditor1;
        private Infragistics.Win.Misc.UltraButton btnCopy;
        private Control.SLabel sLabel3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWorkerName_H;
        private Control.SCodeNMComboBox cboUseFlag;
        private System.Windows.Forms.ComboBox cboUseFlag_H;
        private Control.Grid grid1;
    }
}
