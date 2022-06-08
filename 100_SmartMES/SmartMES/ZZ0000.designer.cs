namespace SmartMES
{
    partial class ZZ0000
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZZ0000));
            this.gbxLogin = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnChange = new Infragistics.Win.Misc.UltraButton();
            this.cboSite = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblSite = new Infragistics.Win.Misc.UltraLabel();
            this.btnConfig = new Infragistics.Win.Misc.UltraButton();
            this.lblPassword = new Infragistics.Win.Misc.UltraLabel();
            this.btnConfirm = new Infragistics.Win.Misc.UltraButton();
            this.lblLogonWorkerID = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.txtWorkerID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.pnlLogin = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.gbxLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWorkerID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            this.pnlLogin.ClientArea.SuspendLayout();
            this.pnlLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxLogin
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.ImageBackground = global::SmartMES.Properties.Resources.SK_logo;
            this.gbxLogin.Appearance = appearance1;
            this.gbxLogin.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.RectangularSolid;
            this.gbxLogin.Location = new System.Drawing.Point(0, 0);
            this.gbxLogin.Name = "gbxLogin";
            this.gbxLogin.Size = new System.Drawing.Size(552, 342);
            this.gbxLogin.TabIndex = 0;
            this.gbxLogin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gbxLogin_MouseDown);
            this.gbxLogin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gbxLogin_MouseMove);
            this.gbxLogin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gbxLogin_MouseUp);
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Location = new System.Drawing.Point(14, 110);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(471, 54);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "(VER 1.0.0.002)";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblVersion.Visible = false;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.RectangularSolid;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BorderColor = System.Drawing.Color.Red;
            this.ultraGroupBox1.ContentAreaAppearance = appearance2;
            this.ultraGroupBox1.Controls.Add(this.btnChange);
            this.ultraGroupBox1.Controls.Add(this.lblVersion);
            this.ultraGroupBox1.Controls.Add(this.cboSite);
            this.ultraGroupBox1.Controls.Add(this.lblSite);
            this.ultraGroupBox1.Controls.Add(this.btnConfig);
            this.ultraGroupBox1.Controls.Add(this.lblPassword);
            this.ultraGroupBox1.Controls.Add(this.btnConfirm);
            this.ultraGroupBox1.Controls.Add(this.lblLogonWorkerID);
            this.ultraGroupBox1.Controls.Add(this.btnClose);
            this.ultraGroupBox1.Controls.Add(this.txtWorkerID);
            this.ultraGroupBox1.Controls.Add(this.txtPassword);
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 341);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(552, 119);
            this.ultraGroupBox1.TabIndex = 2;
            // 
            // btnChange
            // 
            appearance3.BackColor = System.Drawing.Color.PaleGreen;
            appearance3.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "맑은 고딕";
            appearance3.FontData.SizeInPoints = 10F;
            this.btnChange.Appearance = appearance3;
            this.btnChange.ButtonStyle = Infragistics.Win.UIElementButtonStyle.ButtonSoft;
            this.btnChange.Location = new System.Drawing.Point(397, 39);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(72, 68);
            this.btnChange.TabIndex = 7;
            this.btnChange.Text = "비밀번호\r\n변경";
            this.btnChange.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // cboSite
            // 
            this.cboSite.AutoSize = false;
            appearance4.BackColor = System.Drawing.SystemColors.Window;
            appearance4.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cboSite.DisplayLayout.Appearance = appearance4;
            this.cboSite.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.cboSite.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboSite.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance5.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance5.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.BorderColor = System.Drawing.SystemColors.Window;
            this.cboSite.DisplayLayout.GroupByBox.Appearance = appearance5;
            appearance6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cboSite.DisplayLayout.GroupByBox.BandLabelAppearance = appearance6;
            this.cboSite.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance7.BackColor2 = System.Drawing.SystemColors.Control;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cboSite.DisplayLayout.GroupByBox.PromptAppearance = appearance7;
            this.cboSite.DisplayLayout.MaxColScrollRegions = 1;
            this.cboSite.DisplayLayout.MaxRowScrollRegions = 1;
            this.cboSite.DisplayLayout.NewBandLoadStyle = Infragistics.Win.UltraWinGrid.NewBandLoadStyle.Hide;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            appearance8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboSite.DisplayLayout.Override.ActiveCellAppearance = appearance8;
            appearance9.BackColor = System.Drawing.SystemColors.Highlight;
            appearance9.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cboSite.DisplayLayout.Override.ActiveRowAppearance = appearance9;
            this.cboSite.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cboSite.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            this.cboSite.DisplayLayout.Override.CardAreaAppearance = appearance10;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance11.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cboSite.DisplayLayout.Override.CellAppearance = appearance11;
            this.cboSite.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cboSite.DisplayLayout.Override.CellPadding = 0;
            appearance12.BackColor = System.Drawing.SystemColors.Control;
            appearance12.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance12.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance12.BorderColor = System.Drawing.SystemColors.Window;
            this.cboSite.DisplayLayout.Override.GroupByRowAppearance = appearance12;
            appearance13.TextHAlignAsString = "Left";
            this.cboSite.DisplayLayout.Override.HeaderAppearance = appearance13;
            this.cboSite.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.Color.Silver;
            this.cboSite.DisplayLayout.Override.RowAppearance = appearance14;
            this.cboSite.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cboSite.DisplayLayout.Override.TemplateAddRowAppearance = appearance15;
            this.cboSite.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cboSite.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cboSite.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cboSite.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cboSite.Location = new System.Drawing.Point(84, 4);
            this.cboSite.Name = "cboSite";
            this.cboSite.Size = new System.Drawing.Size(155, 30);
            this.cboSite.TabIndex = 0;
            this.cboSite.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboSite.ValueChanged += new System.EventHandler(this.cboSite_ValueChanged);
            this.cboSite.TextChanged += new System.EventHandler(this.cboSite_TextChanged);
            // 
            // lblSite
            // 
            appearance16.FontData.BoldAsString = "True";
            appearance16.FontData.Name = "맑은 고딕";
            appearance16.FontData.SizeInPoints = 10F;
            appearance16.TextHAlignAsString = "Right";
            appearance16.TextVAlignAsString = "Middle";
            this.lblSite.Appearance = appearance16;
            this.lblSite.Location = new System.Drawing.Point(5, 3);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(73, 30);
            this.lblSite.TabIndex = 5;
            this.lblSite.Text = "회사";
            // 
            // btnConfig
            // 
            appearance17.BackColor = System.Drawing.Color.PaleGreen;
            appearance17.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance17.FontData.BoldAsString = "True";
            appearance17.FontData.Name = "맑은 고딕";
            appearance17.FontData.SizeInPoints = 10F;
            this.btnConfig.Appearance = appearance17;
            this.btnConfig.ButtonStyle = Infragistics.Win.UIElementButtonStyle.ButtonSoft;
            this.btnConfig.Location = new System.Drawing.Point(473, 39);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(72, 68);
            this.btnConfig.TabIndex = 5;
            this.btnConfig.Text = "연결상태";
            this.btnConfig.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // lblPassword
            // 
            appearance18.FontData.BoldAsString = "True";
            appearance18.FontData.Name = "맑은 고딕";
            appearance18.FontData.SizeInPoints = 10F;
            appearance18.TextHAlignAsString = "Right";
            appearance18.TextVAlignAsString = "Middle";
            this.lblPassword.Appearance = appearance18;
            this.lblPassword.Location = new System.Drawing.Point(7, 79);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(73, 30);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "비밀번호";
            // 
            // btnConfirm
            // 
            appearance19.BackColor = System.Drawing.Color.LightCyan;
            appearance19.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance19.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            appearance19.FontData.BoldAsString = "True";
            appearance19.FontData.Name = "맑은 고딕";
            appearance19.FontData.SizeInPoints = 10F;
            this.btnConfirm.Appearance = appearance19;
            this.btnConfirm.ButtonStyle = Infragistics.Win.UIElementButtonStyle.ButtonSoft;
            this.btnConfirm.Location = new System.Drawing.Point(245, 39);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(72, 68);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "LOGIN";
            this.btnConfirm.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lblLogonWorkerID
            // 
            appearance20.FontData.BoldAsString = "True";
            appearance20.FontData.Name = "맑은 고딕";
            appearance20.FontData.SizeInPoints = 10F;
            appearance20.TextHAlignAsString = "Right";
            appearance20.TextVAlignAsString = "Middle";
            this.lblLogonWorkerID.Appearance = appearance20;
            this.lblLogonWorkerID.Location = new System.Drawing.Point(8, 41);
            this.lblLogonWorkerID.Name = "lblLogonWorkerID";
            this.lblLogonWorkerID.Size = new System.Drawing.Size(73, 30);
            this.lblLogonWorkerID.TabIndex = 2;
            this.lblLogonWorkerID.Text = "아이디";
            // 
            // btnClose
            // 
            appearance21.BackColor = System.Drawing.Color.Plum;
            appearance21.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance21.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            appearance21.FontData.BoldAsString = "True";
            appearance21.FontData.Name = "맑은 고딕";
            appearance21.FontData.SizeInPoints = 10F;
            this.btnClose.Appearance = appearance21;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.ButtonSoft;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(321, 39);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 68);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtWorkerID
            // 
            appearance22.FontData.BoldAsString = "True";
            appearance22.FontData.Name = "맑은 고딕";
            appearance22.FontData.SizeInPoints = 10F;
            this.txtWorkerID.Appearance = appearance22;
            this.txtWorkerID.AutoSize = false;
            this.txtWorkerID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.txtWorkerID.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2000;
            this.txtWorkerID.Location = new System.Drawing.Point(84, 41);
            this.txtWorkerID.Name = "txtWorkerID";
            this.txtWorkerID.Size = new System.Drawing.Size(155, 30);
            this.txtWorkerID.TabIndex = 1;
            // 
            // txtPassword
            // 
            appearance23.FontData.BoldAsString = "True";
            appearance23.FontData.Name = "Tahoma";
            appearance23.FontData.SizeInPoints = 10F;
            this.txtPassword.Appearance = appearance23;
            this.txtPassword.AutoSize = false;
            this.txtPassword.BorderStyle = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.txtPassword.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2000;
            this.txtPassword.Location = new System.Drawing.Point(84, 79);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(155, 30);
            this.txtPassword.TabIndex = 2;
            // 
            // pnlLogin
            // 
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BorderColor = System.Drawing.Color.LightGray;
            this.pnlLogin.Appearance = appearance24;
            this.pnlLogin.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            // 
            // pnlLogin.ClientArea
            // 
            this.pnlLogin.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.pnlLogin.ClientArea.Controls.Add(this.gbxLogin);
            this.pnlLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogin.Location = new System.Drawing.Point(0, 0);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(555, 462);
            this.pnlLogin.TabIndex = 0;
            // 
            // ZZ0000
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(555, 462);
            this.Controls.Add(this.pnlLogin);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ZZ0000";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LOGIN";
            this.Load += new System.EventHandler(this.ZZ0000_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gbxLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboSite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWorkerID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            this.pnlLogin.ClientArea.ResumeLayout(false);
            this.pnlLogin.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox gbxLogin;
        private Infragistics.Win.Misc.UltraButton btnConfirm;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnConfig;
        private Infragistics.Win.Misc.UltraPanel pnlLogin;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtPassword;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtWorkerID;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraLabel lblPassword;
        private Infragistics.Win.Misc.UltraLabel lblLogonWorkerID;
        private System.Windows.Forms.Label lblVersion;
        private Infragistics.Win.UltraWinGrid.UltraCombo cboSite;
        private Infragistics.Win.Misc.UltraLabel lblSite;
        private Infragistics.Win.Misc.UltraButton btnChange;
    }
}

