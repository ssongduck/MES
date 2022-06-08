namespace SAMMI.PP
{
    partial class PP0355
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
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            this.grid1 = new SAMMI.Control.Grid(this.components);
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.grid2 = new SAMMI.Control.Grid(this.components);
            this.sLabel11 = new SAMMI.Control.SLabel();
            this.calRegDT_TOH = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.calRegDT_FRH = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblDate = new SAMMI.Control.SLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).BeginInit();
            this.gbxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).BeginInit();
            this.gbxBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.calRegDT_TOH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.calRegDT_FRH)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxHeader
            // 
            this.gbxHeader.ContentPadding.Bottom = 2;
            this.gbxHeader.ContentPadding.Left = 2;
            this.gbxHeader.ContentPadding.Right = 2;
            this.gbxHeader.ContentPadding.Top = 4;
            this.gbxHeader.Controls.Add(this.sLabel11);
            this.gbxHeader.Controls.Add(this.calRegDT_TOH);
            this.gbxHeader.Controls.Add(this.calRegDT_FRH);
            this.gbxHeader.Controls.Add(this.lblDate);
            this.gbxHeader.Size = new System.Drawing.Size(1028, 51);
            // 
            // gbxBody
            // 
            this.gbxBody.ContentPadding.Bottom = 4;
            this.gbxBody.ContentPadding.Left = 4;
            this.gbxBody.ContentPadding.Right = 4;
            this.gbxBody.ContentPadding.Top = 6;
            this.gbxBody.Controls.Add(this.grid2);
            this.gbxBody.Controls.Add(this.ultraSplitter1);
            this.gbxBody.Controls.Add(this.grid1);
            this.gbxBody.Location = new System.Drawing.Point(0, 51);
            this.gbxBody.Size = new System.Drawing.Size(1028, 695);
            // 
            // grid1
            // 
            this.grid1.ContextMenuCopyEnabled = true;
            this.grid1.ContextMenuDeleteEnabled = false;
            this.grid1.ContextMenuExcelEnabled = true;
            this.grid1.ContextMenuInsertEnabled = false;
            this.grid1.ContextMenuPasteEnabled = true;
            appearance8.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grid1.DisplayLayout.Appearance = appearance8;
            this.grid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.grid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.BorderStyleCaption = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance9.BackColor = System.Drawing.Color.Gray;
            this.grid1.DisplayLayout.CaptionAppearance = appearance9;
            this.grid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grid1.DisplayLayout.DefaultSelectedBackColor = System.Drawing.Color.RoyalBlue;
            this.grid1.DisplayLayout.GroupByBox.Hidden = true;
            this.grid1.DisplayLayout.InterBandSpacing = 2;
            this.grid1.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.True;
            appearance10.BackColor = System.Drawing.Color.RoyalBlue;
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            this.grid1.DisplayLayout.Override.ActiveRowAppearance = appearance10;
            this.grid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.DisplayLayout.Override.AllowMultiCellOperations = ((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation)((((((((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Copy | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.CopyWithHeaders) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Cut) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Delete) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Paste) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Undo) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Redo) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Reserved)));
            this.grid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2010ScrollbarButton;
            appearance11.BackColor = System.Drawing.Color.DimGray;
            appearance11.BackColor2 = System.Drawing.Color.Silver;
            appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance11.BorderColor = System.Drawing.Color.White;
            appearance11.FontData.BoldAsString = "True";
            appearance11.ForeColor = System.Drawing.Color.White;
            this.grid1.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.grid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            this.grid1.DisplayLayout.Override.RowEditTemplateUIType = Infragistics.Win.UltraWinGrid.RowEditTemplateUIType.None;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance12.BackColor2 = System.Drawing.Color.Gray;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grid1.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance12;
            this.grid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.grid1.DisplayLayout.Override.RowSelectorNumberStyle = Infragistics.Win.UltraWinGrid.RowSelectorNumberStyle.RowIndex;
            this.grid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grid1.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.DisplayLayout.RowConnectorColor = System.Drawing.Color.Silver;
            this.grid1.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grid1.DisplayLayout.SelectionOverlayBorderThickness = 2;
            this.grid1.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Top;
            this.grid1.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.grid1.Location = new System.Drawing.Point(6, 6);
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(1016, 306);
            this.grid1.TabIndex = 0;
            this.grid1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.grid1.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grid1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.Color.White;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(6, 312);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 473;
            this.ultraSplitter1.Size = new System.Drawing.Size(1016, 10);
            this.ultraSplitter1.TabIndex = 1;
            // 
            // grid2
            // 
            this.grid2.ContextMenuCopyEnabled = true;
            this.grid2.ContextMenuDeleteEnabled = true;
            this.grid2.ContextMenuExcelEnabled = true;
            this.grid2.ContextMenuInsertEnabled = true;
            this.grid2.ContextMenuPasteEnabled = true;
            appearance3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grid2.DisplayLayout.Appearance = appearance3;
            this.grid2.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.grid2.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid2.DisplayLayout.BorderStyleCaption = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.Color.Gray;
            this.grid2.DisplayLayout.CaptionAppearance = appearance4;
            this.grid2.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grid2.DisplayLayout.DefaultSelectedBackColor = System.Drawing.Color.RoyalBlue;
            this.grid2.DisplayLayout.GroupByBox.Hidden = true;
            this.grid2.DisplayLayout.InterBandSpacing = 2;
            this.grid2.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.True;
            appearance5.BackColor = System.Drawing.Color.RoyalBlue;
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.White;
            this.grid2.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.grid2.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grid2.DisplayLayout.Override.AllowMultiCellOperations = ((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation)((((((((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Copy | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.CopyWithHeaders) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Cut) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Delete) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Paste) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Undo) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Redo) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Reserved)));
            this.grid2.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid2.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid2.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2010ScrollbarButton;
            appearance6.BackColor = System.Drawing.Color.DimGray;
            appearance6.BackColor2 = System.Drawing.Color.Silver;
            appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.BorderColor = System.Drawing.Color.White;
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            this.grid2.DisplayLayout.Override.HeaderAppearance = appearance6;
            this.grid2.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grid2.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance7.BackColor2 = System.Drawing.Color.Gray;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grid2.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance7;
            this.grid2.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.grid2.DisplayLayout.Override.RowSelectorNumberStyle = Infragistics.Win.UltraWinGrid.RowSelectorNumberStyle.RowIndex;
            this.grid2.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grid2.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grid2.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.True;
            this.grid2.DisplayLayout.RowConnectorColor = System.Drawing.Color.Silver;
            this.grid2.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grid2.DisplayLayout.SelectionOverlayBorderThickness = 2;
            this.grid2.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid2.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.grid2.Location = new System.Drawing.Point(6, 322);
            this.grid2.Name = "grid2";
            this.grid2.Size = new System.Drawing.Size(1016, 367);
            this.grid2.TabIndex = 1;
            this.grid2.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.grid2.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grid2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grid2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // sLabel11
            // 
            appearance17.TextHAlignAsString = "Right";
            appearance17.TextVAlignAsString = "Middle";
            this.sLabel11.Appearance = appearance17;
            this.sLabel11.DbField = null;
            this.sLabel11.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sLabel11.Location = new System.Drawing.Point(213, 13);
            this.sLabel11.Name = "sLabel11";
            this.sLabel11.Size = new System.Drawing.Size(12, 25);
            this.sLabel11.TabIndex = 256;
            this.sLabel11.Text = "~";
            // 
            // calRegDT_TOH
            // 
            appearance18.FontData.SizeInPoints = 10F;
            this.calRegDT_TOH.Appearance = appearance18;
            this.calRegDT_TOH.DateButtons.Add(dateButton1);
            this.calRegDT_TOH.Location = new System.Drawing.Point(231, 13);
            this.calRegDT_TOH.Name = "calRegDT_TOH";
            this.calRegDT_TOH.NonAutoSizeHeight = 26;
            this.calRegDT_TOH.Size = new System.Drawing.Size(109, 24);
            this.calRegDT_TOH.TabIndex = 254;
            this.calRegDT_TOH.Value = new System.DateTime(2022, 5, 24, 0, 0, 0, 0);
            // 
            // calRegDT_FRH
            // 
            appearance19.FontData.SizeInPoints = 10F;
            this.calRegDT_FRH.Appearance = appearance19;
            this.calRegDT_FRH.DateButtons.Add(dateButton2);
            this.calRegDT_FRH.Location = new System.Drawing.Point(98, 13);
            this.calRegDT_FRH.Name = "calRegDT_FRH";
            this.calRegDT_FRH.NonAutoSizeHeight = 26;
            this.calRegDT_FRH.Size = new System.Drawing.Size(109, 24);
            this.calRegDT_FRH.TabIndex = 253;
            this.calRegDT_FRH.Value = new System.DateTime(2022, 5, 24, 0, 0, 0, 0);
            // 
            // lblDate
            // 
            appearance20.TextHAlignAsString = "Right";
            appearance20.TextVAlignAsString = "Middle";
            this.lblDate.Appearance = appearance20;
            this.lblDate.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblDate.DbField = null;
            this.lblDate.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDate.Location = new System.Drawing.Point(12, 12);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(68, 25);
            this.lblDate.TabIndex = 255;
            this.lblDate.Text = "조회기간";
            // 
            // PP0355
            // 
            this.ClientSize = new System.Drawing.Size(1028, 746);
            this.Name = "PP0355";
            this.Text = "기준정보관리";
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).EndInit();
            this.gbxHeader.ResumeLayout(false);
            this.gbxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).EndInit();
            this.gbxBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.calRegDT_TOH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.calRegDT_FRH)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Control.Grid grid2;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Control.Grid grid1;
        private Control.SLabel sLabel11;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo calRegDT_TOH;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo calRegDT_FRH;
        private Control.SLabel lblDate;
    }
}
