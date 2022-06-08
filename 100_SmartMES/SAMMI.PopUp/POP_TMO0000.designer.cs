namespace SAMMI.PopUp
{
    partial class POP_TMO0000
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSel = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.lblImageName = new SAMMI.Control.SLabel();
            this.txtImageName = new System.Windows.Forms.TextBox();
            this.lblImageSeq = new SAMMI.Control.SLabel();
            this.txtImageSeq = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Grid1 = new SAMMI.Control.Grid(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSel);
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Controls.Add(this.lblImageName);
            this.panel1.Controls.Add(this.txtImageName);
            this.panel1.Controls.Add(this.lblImageSeq);
            this.panel1.Controls.Add(this.txtImageSeq);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(771, 86);
            this.panel1.TabIndex = 0;
            // 
            // btnSel
            // 
            this.btnSel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSel.Location = new System.Drawing.Point(615, 0);
            this.btnSel.Name = "btnSel";
            this.btnSel.Size = new System.Drawing.Size(78, 86);
            this.btnSel.TabIndex = 36;
            this.btnSel.Text = "선택";
            this.btnSel.UseVisualStyleBackColor = true;
            this.btnSel.Click += new System.EventHandler(this.btnSel_Click);
            // 
            // btnFind
            // 
            this.btnFind.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFind.Location = new System.Drawing.Point(693, 0);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(78, 86);
            this.btnFind.TabIndex = 33;
            this.btnFind.Text = "조회";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lblImageName
            // 
            appearance1.TextHAlignAsString = "Right";
            appearance1.TextVAlignAsString = "Middle";
            this.lblImageName.Appearance = appearance1;
            this.lblImageName.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblImageName.DbField = null;
            this.lblImageName.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblImageName.Location = new System.Drawing.Point(295, 30);
            this.lblImageName.Name = "lblImageName";
            this.lblImageName.Size = new System.Drawing.Size(124, 25);
            this.lblImageName.TabIndex = 32;
            this.lblImageName.Text = "이미지 명";
            // 
            // txtImageName
            // 
            this.txtImageName.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtImageName.Location = new System.Drawing.Point(425, 30);
            this.txtImageName.Name = "txtImageName";
            this.txtImageName.Size = new System.Drawing.Size(154, 25);
            this.txtImageName.TabIndex = 31;
            this.txtImageName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWorkerName_KeyPress);
            // 
            // lblImageSeq
            // 
            appearance16.TextHAlignAsString = "Right";
            appearance16.TextVAlignAsString = "Middle";
            this.lblImageSeq.Appearance = appearance16;
            this.lblImageSeq.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblImageSeq.DbField = null;
            this.lblImageSeq.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblImageSeq.Location = new System.Drawing.Point(24, 30);
            this.lblImageSeq.Name = "lblImageSeq";
            this.lblImageSeq.Size = new System.Drawing.Size(104, 25);
            this.lblImageSeq.TabIndex = 28;
            this.lblImageSeq.Text = "이미지 순번";
            // 
            // txtImageSeq
            // 
            this.txtImageSeq.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtImageSeq.Location = new System.Drawing.Point(134, 32);
            this.txtImageSeq.Name = "txtImageSeq";
            this.txtImageSeq.Size = new System.Drawing.Size(154, 25);
            this.txtImageSeq.TabIndex = 25;
            this.txtImageSeq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWorkerID_KeyPress);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Grid1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 86);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(771, 424);
            this.panel2.TabIndex = 1;
            // 
            // Grid1
            // 
            this.Grid1.ContextMenuCopyEnabled = true;
            this.Grid1.ContextMenuDeleteEnabled = true;
            this.Grid1.ContextMenuExcelEnabled = true;
            this.Grid1.ContextMenuInsertEnabled = true;
            this.Grid1.ContextMenuPasteEnabled = true;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.Grid1.DisplayLayout.Appearance = appearance14;
            this.Grid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.Grid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.Grid1.DisplayLayout.DefaultSelectedBackColor = System.Drawing.Color.Empty;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.Grid1.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Grid1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.Grid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.Grid1.DisplayLayout.GroupByBox.Hidden = true;
            appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Grid1.DisplayLayout.GroupByBox.PromptAppearance = appearance3;
            this.Grid1.DisplayLayout.MaxColScrollRegions = 1;
            this.Grid1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance9.BackColor = System.Drawing.SystemColors.Window;
            appearance9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Grid1.DisplayLayout.Override.ActiveCellAppearance = appearance9;
            appearance5.BackColor = System.Drawing.SystemColors.Highlight;
            appearance5.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.Grid1.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.Grid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.Grid1.DisplayLayout.Override.AllowMultiCellOperations = ((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation)((((((((Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Copy | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.CopyWithHeaders) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Cut) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Delete) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Paste) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Undo) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Redo) 
            | Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.Reserved)));
            this.Grid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.Grid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            this.Grid1.DisplayLayout.Override.CardAreaAppearance = appearance12;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.Grid1.DisplayLayout.Override.CellAppearance = appearance8;
            this.Grid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.Grid1.DisplayLayout.Override.CellPadding = 0;
            appearance6.BackColor = System.Drawing.SystemColors.Control;
            appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            this.Grid1.DisplayLayout.Override.GroupByRowAppearance = appearance6;
            appearance7.TextHAlignAsString = "Left";
            this.Grid1.DisplayLayout.Override.HeaderAppearance = appearance7;
            this.Grid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.Grid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.BorderColor = System.Drawing.Color.Silver;
            this.Grid1.DisplayLayout.Override.RowAppearance = appearance10;
            this.Grid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Grid1.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
            this.Grid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.Grid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.Grid1.DisplayLayout.SelectionOverlayBorderThickness = 2;
            this.Grid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.Grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Grid1.Location = new System.Drawing.Point(0, 0);
            this.Grid1.Name = "Grid1";
            this.Grid1.Size = new System.Drawing.Size(771, 424);
            this.Grid1.TabIndex = 0;
            this.Grid1.Text = "grid1";
            this.Grid1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.Grid1.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.Grid1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.Grid1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.Grid1.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.Grid1_DoubleClickRow);
            // 
            // POP_TMO0000
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 510);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "POP_TMO0000";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "모니터링 이미지 검색";
            this.Load += new System.EventHandler(this.POP_TMO0000_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Control.Grid Grid1;
        private Control.SLabel lblImageName;
        private System.Windows.Forms.TextBox txtImageName;
        private Control.SLabel lblImageSeq;
        private System.Windows.Forms.TextBox txtImageSeq;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnSel;
    }
}