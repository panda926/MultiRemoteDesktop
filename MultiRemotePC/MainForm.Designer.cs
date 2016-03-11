namespace MultiRemotePC
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.imageListImg = new System.Windows.Forms.ImageList(this.components);
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.cxMenuItemOpen = new System.Windows.Forms.MenuItem();
            this.cxMenuItemZhuYe = new System.Windows.Forms.MenuItem();
            this.cxMenuItemExit = new System.Windows.Forms.MenuItem();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton10 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton11 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton12 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton13 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.friendListBox1 = new JustLib.UnitViews.UserListBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textBoxId = new CCWin.SkinControl.SkinTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.textBoxId.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListImg
            // 
            this.imageListImg.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListImg.ImageStream")));
            this.imageListImg.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListImg.Images.SetKeyName(0, "login-32.png");
            this.imageListImg.Images.SetKeyName(1, "add-user-3-32.png");
            this.imageListImg.Images.SetKeyName(2, "desktop-2-32.png");
            this.imageListImg.Images.SetKeyName(3, "hdd-32.gif");
            this.imageListImg.Images.SetKeyName(4, "add-32.png");
            this.imageListImg.Images.SetKeyName(5, "edit-user-32.png");
            this.imageListImg.Images.SetKeyName(6, "key-3-32.png");
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cxMenuItemOpen,
            this.cxMenuItemZhuYe,
            this.cxMenuItemExit});
            // 
            // cxMenuItemOpen
            // 
            this.cxMenuItemOpen.Index = 0;
            this.cxMenuItemOpen.Text = "SNEED원격제어";
            this.cxMenuItemOpen.Click += new System.EventHandler(this.cxMenuItemOpen_Click);
            // 
            // cxMenuItemZhuYe
            // 
            this.cxMenuItemZhuYe.Index = 1;
            this.cxMenuItemZhuYe.Text = "홈페이지";
            // 
            // cxMenuItemExit
            // 
            this.cxMenuItemExit.Index = 2;
            this.cxMenuItemExit.Text = "종료(&E)";
            this.cxMenuItemExit.Click += new System.EventHandler(this.cxMenuItemExit_Click);
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton1,
            this.toolBarButton5,
            this.toolBarButton2,
            this.toolBarButton6,
            this.toolBarButton7,
            this.toolBarButton9,
            this.toolBarButton10,
            this.toolBarButton11,
            this.toolBarButton12,
            this.toolBarButton13,
            this.toolBarButton3,
            this.toolBarButton8,
            this.toolBarButton4});
            this.toolBar1.ButtonSize = new System.Drawing.Size(60, 55);
            this.toolBar1.CausesValidation = false;
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolBar1.ImageList = this.imageListImg;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(1008, 60);
            this.toolBar1.TabIndex = 2;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.ImageIndex = 0;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Tag = "";
            this.toolBarButton1.Text = "로그인";
            this.toolBarButton1.Visible = false;
            // 
            // toolBarButton5
            // 
            this.toolBarButton5.Name = "toolBarButton5";
            this.toolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            this.toolBarButton5.Visible = false;
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.ImageIndex = 1;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Text = "새로 등록";
            // 
            // toolBarButton6
            // 
            this.toolBarButton6.Name = "toolBarButton6";
            this.toolBarButton6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton7
            // 
            this.toolBarButton7.ImageIndex = 4;
            this.toolBarButton7.Name = "toolBarButton7";
            this.toolBarButton7.Text = "PC추가";
            // 
            // toolBarButton9
            // 
            this.toolBarButton9.Name = "toolBarButton9";
            this.toolBarButton9.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton10
            // 
            this.toolBarButton10.ImageIndex = 5;
            this.toolBarButton10.Name = "toolBarButton10";
            this.toolBarButton10.Text = "유저정보";
            // 
            // toolBarButton11
            // 
            this.toolBarButton11.Name = "toolBarButton11";
            this.toolBarButton11.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton12
            // 
            this.toolBarButton12.ImageIndex = 6;
            this.toolBarButton12.Name = "toolBarButton12";
            this.toolBarButton12.Text = "비번변경";
            // 
            // toolBarButton13
            // 
            this.toolBarButton13.Name = "toolBarButton13";
            this.toolBarButton13.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.ImageIndex = 2;
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Text = "원격제어";
            // 
            // toolBarButton8
            // 
            this.toolBarButton8.Name = "toolBarButton8";
            this.toolBarButton8.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton4
            // 
            this.toolBarButton4.ImageIndex = 3;
            this.toolBarButton4.Name = "toolBarButton4";
            this.toolBarButton4.Text = "원격디스크";
            this.toolBarButton4.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 60);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 561);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.friendListBox1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(196, 559);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // friendListBox1
            // 
            this.friendListBox1.BackColor = System.Drawing.Color.White;
            this.friendListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.friendListBox1.IconSizeMode = CCWin.SkinControl.ChatListItemIcon.Large;
            this.friendListBox1.Location = new System.Drawing.Point(1, 37);
            this.friendListBox1.Margin = new System.Windows.Forms.Padding(1);
            this.friendListBox1.Name = "friendListBox1";
            this.friendListBox1.Size = new System.Drawing.Size(194, 521);
            this.friendListBox1.TabIndex = 135;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.textBoxId);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(190, 30);
            this.panel5.TabIndex = 136;
            // 
            // textBoxId
            // 
            this.textBoxId.BackColor = System.Drawing.Color.Transparent;
            this.textBoxId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxId.Icon = null;
            this.textBoxId.IconIsButton = false;
            this.textBoxId.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.textBoxId.Location = new System.Drawing.Point(0, 0);
            this.textBoxId.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxId.MinimumSize = new System.Drawing.Size(28, 30);
            this.textBoxId.MouseBack = null;
            this.textBoxId.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.NormlBack = null;
            this.textBoxId.Padding = new System.Windows.Forms.Padding(5, 5, 28, 5);
            this.textBoxId.Size = new System.Drawing.Size(190, 30);
            // 
            // textBoxId.BaseText
            // 
            this.textBoxId.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxId.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxId.SkinTxt.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F);
            this.textBoxId.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.textBoxId.SkinTxt.Name = "BaseText";
            this.textBoxId.SkinTxt.Size = new System.Drawing.Size(157, 18);
            this.textBoxId.SkinTxt.TabIndex = 0;
            this.textBoxId.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.textBoxId.SkinTxt.WaterText = "PC검색";
            this.textBoxId.SkinTxt.TextChanged += new System.EventHandler(this.textBoxId_SkinTxt_TextChanged);
            this.textBoxId.TabIndex = 36;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 559F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(806, 559);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.panel4, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(800, 553);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = global::MultiRemotePC.Properties.Resources._4_filled_128;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(403, 279);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(394, 271);
            this.panel4.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::MultiRemotePC.Properties.Resources._3_filled_128;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 279);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(394, 271);
            this.panel3.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::MultiRemotePC.Properties.Resources._2_filled_128;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(403, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(394, 270);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::MultiRemotePC.Properties.Resources._1_filled_128;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(394, 270);
            this.panel1.TabIndex = 0;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 621);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "SNEED";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.textBoxId.ResumeLayout(false);
            this.textBoxId.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageListImg;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem cxMenuItemOpen;
        private System.Windows.Forms.MenuItem cxMenuItemZhuYe;
        private System.Windows.Forms.MenuItem cxMenuItemExit;
        private System.Windows.Forms.ToolBar toolBar1;        
        private System.Windows.Forms.ToolBarButton toolBarButton5;        
        private System.Windows.Forms.ToolBarButton toolBarButton6;        
        private System.Windows.Forms.ToolBarButton toolBarButton9;        
        private System.Windows.Forms.ToolBarButton toolBarButton8;
        private System.Windows.Forms.ToolBarButton toolBarButton4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private JustLib.UnitViews.UserListBox friendListBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;        
        private System.Windows.Forms.ToolBarButton toolBarButton11;
        private System.Windows.Forms.ToolBarButton toolBarButton13;
        //private System.Windows.Forms.ToolBarButton toolBarLogIn;
        //private System.Windows.Forms.ToolBarButton toolBarNewReg;
        //private System.Windows.Forms.ToolBarButton toolBarAddPC;
        //private System.Windows.Forms.ToolBarButton toolBarUserInfo;
        //private System.Windows.Forms.ToolBarButton toolBarChangePass;
        //private System.Windows.Forms.ToolBarButton toolBarRemoteControl;
        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.ToolBarButton toolBarButton2;
        private System.Windows.Forms.ToolBarButton toolBarButton7;
        private System.Windows.Forms.ToolBarButton toolBarButton10;
        private System.Windows.Forms.ToolBarButton toolBarButton12;
        private System.Windows.Forms.ToolBarButton toolBarButton3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private CCWin.SkinControl.SkinTextBox textBoxId;
    }
}

