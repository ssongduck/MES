namespace SAMMI.BM
{
    partial class MO0000
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
            Infragistics.Win.Appearance appearance108 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance109 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance110 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance111 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance112 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance113 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance114 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance115 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance116 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance117 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance118 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance119 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance136 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance137 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance138 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance139 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance140 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance141 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance142 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance143 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance144 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance145 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance146 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance147 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance148 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance149 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance150 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance151 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.grid1 = new SAMMI.Control.Grid(this.components);
            this.cboPlantCode_H = new SAMMI.Control.SCodeNMComboBox();
            this.sLabel10 = new SAMMI.Control.SLabel();
            this.txtMonitorIP = new System.Windows.Forms.TextBox();
            this.sLabel1 = new SAMMI.Control.SLabel();
            this.txtMonitorName = new System.Windows.Forms.TextBox();
            this.sLabel2 = new SAMMI.Control.SLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).BeginInit();
            this.gbxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).BeginInit();
            this.gbxBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlantCode_H)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxHeader
            // 
            this.gbxHeader.ContentPadding.Bottom = 2;
            this.gbxHeader.ContentPadding.Left = 2;
            this.gbxHeader.ContentPadding.Right = 2;
            this.gbxHeader.ContentPadding.Top = 4;
            this.gbxHeader.Controls.Add(this.txtMonitorName);
            this.gbxHeader.Controls.Add(this.sLabel2);
            this.gbxHeader.Controls.Add(this.txtMonitorIP);
            this.gbxHeader.Controls.Add(this.sLabel1);
            this.gbxHeader.Controls.Add(this.sLabel10);
            this.gbxHeader.Controls.Add(this.cboPlantCode_H);
            this.gbxHeader.Size = new System.Drawing.Size(1136, 85);
            // 
            // gbxBody
            // 
            this.gbxBody.ContentPadding.Bottom = 4;
            this.gbxBody.ContentPadding.Left = 4;
            this.gbxBody.ContentPadding.Right = 4;
            this.gbxBody.ContentPadding.Top = 6;
            this.gbxBody.Controls.Add(this.grid1);
            this.gbxBody.Location = new System.Drawing.Point(0, 85);
            this.gbxBody.Size = new System.Drawing.Size(1136, 740);
            // 
            // grid1
            // 
            this.grid1.ContextMenuCopyEnabled = true;
            this.grid1.ContextMenuDeleteEnabled = true;
            this.grid1.ContextMenuExcelEnabled = true;
            this.grid1.ContextMenuInsertEnabled = true;
            this.grid1.ContextMenuPasteEnabled = true;
            appearance108.BackColor = System.Drawing.SystemColors.Window;
            appearance108.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grid1.DisplayLayout.Appearance = appearance108;
            this.grid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grid1.DisplayLayout.DefaultSelectedBackColor = System.Drawing.Color.Empty;
            appearance109.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance109.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance109.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance109.BorderColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.GroupByBox.Appearance = appearance109;
            appearance110.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grid1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance110;
            this.grid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.GroupByBox.Hidden = true;
            appearance111.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance111.BackColor2 = System.Drawing.SystemColors.Control;
            appearance111.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance111.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grid1.DisplayLayout.GroupByBox.PromptAppearance = appearance111;
            this.grid1.DisplayLayout.MaxColScrollRegions = 1;
            this.grid1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance112.BackColor = System.Drawing.SystemColors.Window;
            appearance112.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grid1.DisplayLayout.Override.ActiveCellAppearance = appearance112;
            appearance113.BackColor = System.Drawing.SystemColors.Highlight;
            appearance113.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grid1.DisplayLayout.Override.ActiveRowAppearance = appearance113;
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
            appearance114.BackColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.Override.CardAreaAppearance = appearance114;
            appearance115.BorderColor = System.Drawing.Color.Silver;
            appearance115.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grid1.DisplayLayout.Override.CellAppearance = appearance115;
            this.grid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grid1.DisplayLayout.Override.CellPadding = 0;
            appearance116.BackColor = System.Drawing.SystemColors.Control;
            appearance116.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance116.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance116.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance116.BorderColor = System.Drawing.SystemColors.Window;
            this.grid1.DisplayLayout.Override.GroupByRowAppearance = appearance116;
            appearance117.TextHAlignAsString = "Left";
            this.grid1.DisplayLayout.Override.HeaderAppearance = appearance117;
            this.grid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance118.BackColor = System.Drawing.SystemColors.Window;
            appearance118.BorderColor = System.Drawing.Color.Silver;
            this.grid1.DisplayLayout.Override.RowAppearance = appearance118;
            this.grid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance119.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grid1.DisplayLayout.Override.TemplateAddRowAppearance = appearance119;
            this.grid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grid1.DisplayLayout.SelectionOverlayBorderThickness = 2;
            this.grid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grid1.Location = new System.Drawing.Point(6, 6);
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(1124, 728);
            this.grid1.TabIndex = 365;
            this.grid1.Text = "grid1";
            this.grid1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.grid1.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grid1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cboPlantCode_H
            // 
            appearance136.FontData.Name = "맑은 고딕";
            appearance136.FontData.SizeInPoints = 9F;
            this.cboPlantCode_H.Appearance = appearance136;
            this.cboPlantCode_H.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboPlantCode_H.ComboDataType = SAMMI.Control.ComboDataType.All;
            this.cboPlantCode_H.DbConfig = null;
            this.cboPlantCode_H.DefaultValue = "ALL";
            appearance137.BackColor = System.Drawing.SystemColors.Window;
            appearance137.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cboPlantCode_H.DisplayLayout.Appearance = appearance137;
            this.cboPlantCode_H.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.cboPlantCode_H.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboPlantCode_H.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance138.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance138.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance138.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance138.BorderColor = System.Drawing.SystemColors.Window;
            this.cboPlantCode_H.DisplayLayout.GroupByBox.Appearance = appearance138;
            appearance139.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cboPlantCode_H.DisplayLayout.GroupByBox.BandLabelAppearance = appearance139;
            this.cboPlantCode_H.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance140.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance140.BackColor2 = System.Drawing.SystemColors.Control;
            appearance140.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance140.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cboPlantCode_H.DisplayLayout.GroupByBox.PromptAppearance = appearance140;
            this.cboPlantCode_H.DisplayLayout.MaxColScrollRegions = 1;
            this.cboPlantCode_H.DisplayLayout.MaxRowScrollRegions = 1;
            appearance141.BackColor = System.Drawing.SystemColors.Window;
            appearance141.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboPlantCode_H.DisplayLayout.Override.ActiveCellAppearance = appearance141;
            appearance142.BackColor = System.Drawing.SystemColors.Highlight;
            appearance142.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cboPlantCode_H.DisplayLayout.Override.ActiveRowAppearance = appearance142;
            appearance143.FontData.BoldAsString = "True";
            this.cboPlantCode_H.DisplayLayout.Override.ActiveRowCellAppearance = appearance143;
            this.cboPlantCode_H.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cboPlantCode_H.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cboPlantCode_H.DisplayLayout.Override.BorderStyleSpecialRowSeparator = Infragistics.Win.UIElementBorderStyle.None;
            appearance144.BackColor = System.Drawing.SystemColors.Window;
            this.cboPlantCode_H.DisplayLayout.Override.CardAreaAppearance = appearance144;
            appearance145.BorderColor = System.Drawing.Color.Silver;
            appearance145.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cboPlantCode_H.DisplayLayout.Override.CellAppearance = appearance145;
            this.cboPlantCode_H.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cboPlantCode_H.DisplayLayout.Override.CellPadding = 0;
            appearance146.BackColor = System.Drawing.SystemColors.Control;
            appearance146.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance146.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance146.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance146.BorderColor = System.Drawing.SystemColors.Window;
            this.cboPlantCode_H.DisplayLayout.Override.GroupByRowAppearance = appearance146;
            appearance147.TextHAlignAsString = "Left";
            this.cboPlantCode_H.DisplayLayout.Override.HeaderAppearance = appearance147;
            this.cboPlantCode_H.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cboPlantCode_H.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance148.BackColor = System.Drawing.SystemColors.Window;
            appearance148.BorderColor = System.Drawing.Color.Silver;
            this.cboPlantCode_H.DisplayLayout.Override.RowAppearance = appearance148;
            this.cboPlantCode_H.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance149.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(133)))), ((int)(((byte)(188)))));
            appearance149.FontData.BoldAsString = "True";
            this.cboPlantCode_H.DisplayLayout.Override.SelectedRowAppearance = appearance149;
            appearance150.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cboPlantCode_H.DisplayLayout.Override.TemplateAddRowAppearance = appearance150;
            this.cboPlantCode_H.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cboPlantCode_H.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cboPlantCode_H.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.cboPlantCode_H.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cboPlantCode_H.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboPlantCode_H.Location = new System.Drawing.Point(99, 29);
            this.cboPlantCode_H.MajorCode = "PLANTCODE";
            this.cboPlantCode_H.Name = "cboPlantCode_H";
            this.cboPlantCode_H.ShowDefaultValue = false;
            this.cboPlantCode_H.Size = new System.Drawing.Size(119, 24);
            this.cboPlantCode_H.TabIndex = 554;
            this.cboPlantCode_H.Text = "ALL";
            // 
            // sLabel10
            // 
            appearance151.TextHAlignAsString = "Right";
            appearance151.TextVAlignAsString = "Middle";
            this.sLabel10.Appearance = appearance151;
            this.sLabel10.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.sLabel10.DbField = "cboUseFlag";
            this.sLabel10.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.sLabel10.Location = new System.Drawing.Point(40, 29);
            this.sLabel10.Name = "sLabel10";
            this.sLabel10.Size = new System.Drawing.Size(53, 26);
            this.sLabel10.TabIndex = 553;
            this.sLabel10.Text = "사업장";
            // 
            // txtMonitorIP
            // 
            this.txtMonitorIP.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMonitorIP.Location = new System.Drawing.Point(311, 29);
            this.txtMonitorIP.Name = "txtMonitorIP";
            this.txtMonitorIP.Size = new System.Drawing.Size(119, 25);
            this.txtMonitorIP.TabIndex = 557;
            // 
            // sLabel1
            // 
            appearance1.TextHAlignAsString = "Right";
            appearance1.TextVAlignAsString = "Middle";
            this.sLabel1.Appearance = appearance1;
            this.sLabel1.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.sLabel1.DbField = "";
            this.sLabel1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sLabel1.Location = new System.Drawing.Point(224, 29);
            this.sLabel1.Name = "sLabel1";
            this.sLabel1.Size = new System.Drawing.Size(81, 26);
            this.sLabel1.TabIndex = 558;
            this.sLabel1.Text = "현황판IP";
            // 
            // txtMonitorName
            // 
            this.txtMonitorName.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMonitorName.Location = new System.Drawing.Point(527, 29);
            this.txtMonitorName.Name = "txtMonitorName";
            this.txtMonitorName.Size = new System.Drawing.Size(119, 25);
            this.txtMonitorName.TabIndex = 559;
            // 
            // sLabel2
            // 
            appearance47.TextHAlignAsString = "Right";
            appearance47.TextVAlignAsString = "Middle";
            this.sLabel2.Appearance = appearance47;
            this.sLabel2.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.sLabel2.DbField = "";
            this.sLabel2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sLabel2.Location = new System.Drawing.Point(440, 29);
            this.sLabel2.Name = "sLabel2";
            this.sLabel2.Size = new System.Drawing.Size(81, 26);
            this.sLabel2.TabIndex = 560;
            this.sLabel2.Text = "현황판명";
            // 
            // MO0000
            // 
            this.ClientSize = new System.Drawing.Size(1136, 825);
            this.Name = "MO0000";
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).EndInit();
            this.gbxHeader.ResumeLayout(false);
            this.gbxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).EndInit();
            this.gbxBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlantCode_H)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Control.Grid grid1;
        private Control.SCodeNMComboBox cboPlantCode_H;
        private Control.SLabel sLabel10;
        private System.Windows.Forms.TextBox txtMonitorName;
        private Control.SLabel sLabel2;
        private System.Windows.Forms.TextBox txtMonitorIP;
        private Control.SLabel sLabel1;
    }
}
