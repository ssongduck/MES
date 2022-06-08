namespace SAMMI.SY
{
    partial class SY0300
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("dTable1", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WorkerID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WorkerName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SDATE");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("STIME");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SSTAMP");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("EDATE");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ETIME");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ESTAMP");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("STATE");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ProgID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RUNTIME");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RUNSTR");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IPAddress");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("COMNAME");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("REMARK");
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            this.lbl = new SAMMI.Control.SLabel();
            this.lblPlanDT = new SAMMI.Control.SLabel();
            this.cboPlanEndDT_H = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.cboPlanStartDT_H = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblMaker = new SAMMI.Control.SLabel();
            this.txtMaker = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.grid1 = new SAMMI.Control.Grid(this.components);
            this.ds = new SAMMI.SY.SY0300D();
            this.daTable1 = new SAMMI.SY.SY0300DTableAdapters.SY0300DTA1();
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).BeginInit();
            this.gbxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).BeginInit();
            this.gbxBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlanEndDT_H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlanStartDT_H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxHeader
            // 
            this.gbxHeader.ContentPadding.Bottom = 2;
            this.gbxHeader.ContentPadding.Left = 2;
            this.gbxHeader.ContentPadding.Right = 2;
            this.gbxHeader.ContentPadding.Top = 4;
            this.gbxHeader.Controls.Add(this.lblMaker);
            this.gbxHeader.Controls.Add(this.txtMaker);
            this.gbxHeader.Controls.Add(this.lbl);
            this.gbxHeader.Controls.Add(this.cboPlanStartDT_H);
            this.gbxHeader.Controls.Add(this.lblPlanDT);
            this.gbxHeader.Controls.Add(this.cboPlanEndDT_H);
            // 
            // gbxBody
            // 
            this.gbxBody.ContentPadding.Bottom = 4;
            this.gbxBody.ContentPadding.Left = 4;
            this.gbxBody.ContentPadding.Right = 4;
            this.gbxBody.ContentPadding.Top = 6;
            this.gbxBody.Controls.Add(this.grid1);
            // 
            // lbl
            // 
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.lbl.Appearance = appearance3;
            this.lbl.DbField = null;
            this.lbl.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl.Location = new System.Drawing.Point(340, 25);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(46, 23);
            this.lbl.TabIndex = 17;
            this.lbl.Text = "~";
            // 
            // lblPlanDT
            // 
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.lblPlanDT.Appearance = appearance2;
            this.lblPlanDT.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblPlanDT.DbField = null;
            this.lblPlanDT.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPlanDT.Location = new System.Drawing.Point(90, 25);
            this.lblPlanDT.Name = "lblPlanDT";
            this.lblPlanDT.Size = new System.Drawing.Size(100, 25);
            this.lblPlanDT.TabIndex = 16;
            this.lblPlanDT.Text = "조회기간";
            // 
            // cboPlanEndDT_H
            // 
            this.cboPlanEndDT_H.DateButtons.Add(dateButton2);
            this.cboPlanEndDT_H.Location = new System.Drawing.Point(395, 25);
            this.cboPlanEndDT_H.Name = "cboPlanEndDT_H";
            this.cboPlanEndDT_H.NonAutoSizeHeight = 26;
            this.cboPlanEndDT_H.Size = new System.Drawing.Size(135, 26);
            this.cboPlanEndDT_H.TabIndex = 15;
            // 
            // cboPlanStartDT_H
            // 
            this.cboPlanStartDT_H.DateButtons.Add(dateButton1);
            this.cboPlanStartDT_H.Location = new System.Drawing.Point(200, 25);
            this.cboPlanStartDT_H.Name = "cboPlanStartDT_H";
            this.cboPlanStartDT_H.NonAutoSizeHeight = 26;
            this.cboPlanStartDT_H.Size = new System.Drawing.Size(135, 26);
            this.cboPlanStartDT_H.TabIndex = 14;
            // 
            // lblMaker
            // 
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Middle";
            this.lblMaker.Appearance = appearance4;
            this.lblMaker.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblMaker.DbField = null;
            this.lblMaker.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMaker.Location = new System.Drawing.Point(562, 25);
            this.lblMaker.Name = "lblMaker";
            this.lblMaker.Size = new System.Drawing.Size(100, 25);
            this.lblMaker.TabIndex = 19;
            this.lblMaker.Text = "ID";
            // 
            // txtMaker
            // 
            this.txtMaker.AutoSize = false;
            this.txtMaker.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMaker.Location = new System.Drawing.Point(668, 25);
            this.txtMaker.Name = "txtMaker";
            this.txtMaker.Size = new System.Drawing.Size(156, 25);
            this.txtMaker.TabIndex = 18;
            // 
            // grid1
            // 
            this.grid1.ContextMenuCopyEnabled = true;
            this.grid1.ContextMenuDeleteEnabled = false;
            this.grid1.ContextMenuExcelEnabled = true;
            this.grid1.ContextMenuInsertEnabled = false;
            this.grid1.ContextMenuPasteEnabled = true;
            this.grid1.DataMember = "dTable1";
            this.grid1.DataSource = this.ds;
            appearance26.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grid1.DisplayLayout.Appearance = appearance26;
            this.grid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Tag = "M";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Tag = "N";
            ultraGridColumn4.Header.VisiblePosition = 2;
            ultraGridColumn4.Tag = "N";
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Tag = "N";
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Tag = "N";
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Tag = "N";
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn8.Tag = "N";
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn9.Tag = "N";
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn10.Tag = "N";
            ultraGridColumn11.Header.VisiblePosition = 10;
            ultraGridColumn11.Tag = "N";
            ultraGridColumn12.Header.VisiblePosition = 11;
            ultraGridColumn12.Tag = "N";
            ultraGridColumn13.Header.VisiblePosition = 12;
            ultraGridColumn13.Tag = "N";
            ultraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            ultraGridColumn3.CellAppearance = appearance1;
            ultraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridColumn3.Header.VisiblePosition = 3;
            ultraGridColumn3.Tag = "N";
            ultraGridColumn14.Header.VisiblePosition = 13;
            ultraGridColumn14.Tag = "N";
            ultraGridColumn15.Header.VisiblePosition = 14;
            ultraGridColumn15.Tag = "N";
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn3,
            ultraGridColumn14,
            ultraGridColumn15});
            this.grid1.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grid1.DisplayLayout.BorderStyleCaption = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance25.BackColor = System.Drawing.Color.Gray;
            this.grid1.DisplayLayout.CaptionAppearance = appearance25;
            this.grid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grid1.DisplayLayout.DefaultSelectedBackColor = System.Drawing.Color.RoyalBlue;
            this.grid1.DisplayLayout.GroupByBox.Hidden = true;
            this.grid1.DisplayLayout.InterBandSpacing = 2;
            this.grid1.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.True;
            appearance23.BackColor = System.Drawing.Color.RoyalBlue;
            appearance23.FontData.BoldAsString = "True";
            appearance23.ForeColor = System.Drawing.Color.White;
            this.grid1.DisplayLayout.Override.ActiveRowAppearance = appearance23;
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
            appearance22.BackColor = System.Drawing.Color.DimGray;
            appearance22.BackColor2 = System.Drawing.Color.Silver;
            appearance22.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance22.BorderColor = System.Drawing.Color.White;
            appearance22.FontData.BoldAsString = "True";
            appearance22.ForeColor = System.Drawing.Color.White;
            this.grid1.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.grid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance24.BackColor2 = System.Drawing.Color.Gray;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grid1.DisplayLayout.Override.RowSelectorHeaderAppearance = appearance24;
            this.grid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.grid1.DisplayLayout.Override.RowSelectorNumberStyle = Infragistics.Win.UltraWinGrid.RowSelectorNumberStyle.RowIndex;
            this.grid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grid1.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.DisplayLayout.RowConnectorColor = System.Drawing.Color.Silver;
            this.grid1.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grid1.DisplayLayout.SelectionOverlayBorderThickness = 2;
            this.grid1.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.grid1.Location = new System.Drawing.Point(6, 6);
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(1124, 744);
            this.grid1.TabIndex = 3;
            this.grid1.Text = "DE";
            this.grid1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.grid1.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grid1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grid1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ds
            // 
            this.ds.DataSetName = "SY0300D";
            this.ds.EnforceConstraints = false;
            this.ds.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // daTable1
            // 
            this.daTable1.ClearBeforeFill = true;
            // 
            // SY0300
            // 
            this.ClientSize = new System.Drawing.Size(1136, 825);
            this.Name = "SY0300";
            ((System.ComponentModel.ISupportInitialize)(this.gbxHeader)).EndInit();
            this.gbxHeader.ResumeLayout(false);
            this.gbxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBody)).EndInit();
            this.gbxBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboPlanEndDT_H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlanStartDT_H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SY0300D ds;
        private SY0300DTableAdapters.SY0300DTA1 daTable1;
        private Control.Grid grid1;
        private Control.SLabel lbl;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo cboPlanStartDT_H;
        private Control.SLabel lblPlanDT;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo cboPlanEndDT_H;
        private Control.SLabel lblMaker;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtMaker;
    }
}
