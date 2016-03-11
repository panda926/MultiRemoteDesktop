
using JustLib.Controls;
namespace JustLib.NetworkDisk.Passive
{
    partial class NDiskBrowser
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NDiskBrowser));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton_parent = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_root = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_refresh = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_downLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel_left = new System.Windows.Forms.Panel();
            this.listView_fileDirectory = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skinToolStrip1 = new CCWin.SkinControl.SkinToolStrip();
            this.toolStripButton_state = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel_msg = new System.Windows.Forms.ToolStripLabel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.fileTransferingViewer1 = new JustLib.Controls.FileTransferingViewer();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip_blank = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_view = new System.Windows.Forms.ToolStripMenuItem();
            this.图标ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.详细信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.上传文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_uploadFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_paste = new System.Windows.Forms.ToolStripMenuItem();
            this.新建文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel_left.SuspendLayout();
            this.skinToolStrip1.SuspendLayout();
            this.contextMenuStrip_blank.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripTextBox1,
            this.toolStripButton_parent,
            this.toolStripButton_root,
            this.toolStripButton_refresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(575, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel1.Text = "경로";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.AutoSize = false;
            this.toolStripTextBox1.BackColor = System.Drawing.Color.White;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.ReadOnly = true;
            this.toolStripTextBox1.Size = new System.Drawing.Size(270, 25);
            // 
            // toolStripButton_parent
            // 
            this.toolStripButton_parent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_parent.Image = global::JustLib.Properties.Resources.arrow_135_32;
            this.toolStripButton_parent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_parent.Name = "toolStripButton_parent";
            this.toolStripButton_parent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_parent.Text = "위로";
            this.toolStripButton_parent.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton_root
            // 
            this.toolStripButton_root.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_root.Image = global::JustLib.Properties.Resources.hdd_32;
            this.toolStripButton_root.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_root.Name = "toolStripButton_root";
            this.toolStripButton_root.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_root.Text = "루트";
            this.toolStripButton_root.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton_refresh
            // 
            this.toolStripButton_refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_refresh.Image = global::JustLib.Properties.Resources.refresh_32;
            this.toolStripButton_refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_refresh.Name = "toolStripButton_refresh";
            this.toolStripButton_refresh.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_refresh.Text = "새로 고침";
            this.toolStripButton_refresh.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_downLoad,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem5,
            this.toolStripMenuItem_Delete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(135, 114);
            // 
            // toolStripMenuItem_downLoad
            // 
            this.toolStripMenuItem_downLoad.Image = global::JustLib.Properties.Resources.download_2_32;
            this.toolStripMenuItem_downLoad.Name = "toolStripMenuItem_downLoad";
            this.toolStripMenuItem_downLoad.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem_downLoad.Text = "다운로드";
            this.toolStripMenuItem_downLoad.Click += new System.EventHandler(this.toolStripMenuItem_downLoad_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem2.Text = "이름바꾸기";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click_1);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Image = global::JustLib.Properties.Resources.copy_32;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem3.Text = "복사";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Image = global::JustLib.Properties.Resources.cut_32;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem5.Text = "자르기";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem_Delete
            // 
            this.toolStripMenuItem_Delete.Image = global::JustLib.Properties.Resources.delete_32;
            this.toolStripMenuItem_Delete.Name = "toolStripMenuItem_Delete";
            this.toolStripMenuItem_Delete.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem_Delete.Text = "삭제";
            this.toolStripMenuItem_Delete.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder-32.png");
            this.imageList1.Images.SetKeyName(1, "document-32.png");
            this.imageList1.Images.SetKeyName(2, "hdd-32.gif");
            this.imageList1.Images.SetKeyName(3, "cd-32.png");
            this.imageList1.Images.SetKeyName(4, "usb-2-32.png");
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.Controls.Add(this.panel_left);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.fileTransferingViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(575, 420);
            this.panel1.TabIndex = 5;
            // 
            // panel_left
            // 
            this.panel_left.AllowDrop = true;
            this.panel_left.Controls.Add(this.listView_fileDirectory);
            this.panel_left.Controls.Add(this.skinToolStrip1);
            this.panel_left.Controls.Add(this.splitter2);
            this.panel_left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_left.Location = new System.Drawing.Point(0, 0);
            this.panel_left.Name = "panel_left";
            this.panel_left.Size = new System.Drawing.Size(371, 420);
            this.panel_left.TabIndex = 7;
            // 
            // listView_fileDirectory
            // 
            this.listView_fileDirectory.AllowDrop = true;
            this.listView_fileDirectory.BackColor = System.Drawing.Color.White;
            this.listView_fileDirectory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView_fileDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_fileDirectory.LargeImageList = this.imageList1;
            this.listView_fileDirectory.Location = new System.Drawing.Point(0, 0);
            this.listView_fileDirectory.MultiSelect = false;
            this.listView_fileDirectory.Name = "listView_fileDirectory";
            this.listView_fileDirectory.ShowItemToolTips = true;
            this.listView_fileDirectory.Size = new System.Drawing.Size(371, 392);
            this.listView_fileDirectory.SmallImageList = this.imageList1;
            this.listView_fileDirectory.TabIndex = 12;
            this.listView_fileDirectory.UseCompatibleStateImageBehavior = false;
            this.listView_fileDirectory.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_fileDirectory_ColumnClick);
            this.listView_fileDirectory.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_fileDirectory_DragDrop);
            this.listView_fileDirectory.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView_fileDirectory_DragEnter);
            this.listView_fileDirectory.DragOver += new System.Windows.Forms.DragEventHandler(this.listView_fileDirectory_DragOver);
            this.listView_fileDirectory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            this.listView_fileDirectory.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.listView1_PreviewKeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "이름";
            this.columnHeader1.Width = 185;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "작성날짜";
            this.columnHeader2.Width = 199;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "크기";
            this.columnHeader3.Width = 130;
            // 
            // skinToolStrip1
            // 
            this.skinToolStrip1.Arrow = System.Drawing.Color.Black;
            this.skinToolStrip1.Back = System.Drawing.Color.White;
            this.skinToolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.skinToolStrip1.BackRadius = 4;
            this.skinToolStrip1.BackRectangle = new System.Drawing.Rectangle(10, 10, 10, 10);
            this.skinToolStrip1.Base = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skinToolStrip1.BaseFore = System.Drawing.Color.Black;
            this.skinToolStrip1.BaseForeAnamorphosis = false;
            this.skinToolStrip1.BaseForeAnamorphosisBorder = 4;
            this.skinToolStrip1.BaseForeAnamorphosisColor = System.Drawing.Color.White;
            this.skinToolStrip1.BaseForeOffset = new System.Drawing.Point(0, 0);
            this.skinToolStrip1.BaseHoverFore = System.Drawing.Color.White;
            this.skinToolStrip1.BaseItemAnamorphosis = true;
            this.skinToolStrip1.BaseItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip1.BaseItemBorderShow = true;
            this.skinToolStrip1.BaseItemDown = ((System.Drawing.Image)(resources.GetObject("skinToolStrip1.BaseItemDown")));
            this.skinToolStrip1.BaseItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip1.BaseItemMouse = ((System.Drawing.Image)(resources.GetObject("skinToolStrip1.BaseItemMouse")));
            this.skinToolStrip1.BaseItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip1.BaseItemRadius = 4;
            this.skinToolStrip1.BaseItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinToolStrip1.BaseItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.skinToolStrip1.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinToolStrip1.Fore = System.Drawing.Color.Black;
            this.skinToolStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.skinToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.skinToolStrip1.HoverFore = System.Drawing.Color.White;
            this.skinToolStrip1.ItemAnamorphosis = true;
            this.skinToolStrip1.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip1.ItemBorderShow = true;
            this.skinToolStrip1.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip1.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip1.ItemRadius = 4;
            this.skinToolStrip1.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_state,
            this.toolStripLabel_msg});
            this.skinToolStrip1.Location = new System.Drawing.Point(0, 392);
            this.skinToolStrip1.Name = "skinToolStrip1";
            this.skinToolStrip1.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinToolStrip1.Size = new System.Drawing.Size(371, 25);
            this.skinToolStrip1.SkinAllColor = true;
            this.skinToolStrip1.TabIndex = 11;
            this.skinToolStrip1.Text = "skinToolStrip1";
            this.skinToolStrip1.TitleAnamorphosis = true;
            this.skinToolStrip1.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.skinToolStrip1.TitleRadius = 4;
            this.skinToolStrip1.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripButton_state
            // 
            this.toolStripButton_state.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_state.Image = global::JustLib.Properties.Resources.ok_32;
            this.toolStripButton_state.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_state.Name = "toolStripButton_state";
            this.toolStripButton_state.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_state.Text = "상태";
            // 
            // toolStripLabel_msg
            // 
            this.toolStripLabel_msg.Name = "toolStripLabel_msg";
            this.toolStripLabel_msg.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel_msg.Text = "정상";
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 417);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(371, 3);
            this.splitter2.TabIndex = 9;
            this.splitter2.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(371, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 420);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // fileTransferingViewer1
            // 
            this.fileTransferingViewer1.AutoScroll = true;
            this.fileTransferingViewer1.BackColor = System.Drawing.Color.Transparent;
            this.fileTransferingViewer1.Dock = System.Windows.Forms.DockStyle.Right;
            this.fileTransferingViewer1.Location = new System.Drawing.Point(374, 0);
            this.fileTransferingViewer1.Name = "fileTransferingViewer1";
            this.fileTransferingViewer1.Size = new System.Drawing.Size(201, 420);
            this.fileTransferingViewer1.TabIndex = 4;
            this.fileTransferingViewer1.Visible = false;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "ok-32.png");
            this.imageList2.Images.SetKeyName(1, "error-7-32.png");
            // 
            // contextMenuStrip_blank
            // 
            this.contextMenuStrip_blank.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_view,
            this.toolStripMenuItem1,
            this.上传文件ToolStripMenuItem,
            this.toolStripMenuItem_uploadFolder,
            this.toolStripMenuItem_paste,
            this.新建文件夹ToolStripMenuItem});
            this.contextMenuStrip_blank.Name = "contextMenuStrip_blank";
            this.contextMenuStrip_blank.Size = new System.Drawing.Size(150, 136);
            // 
            // toolStripMenuItem_view
            // 
            this.toolStripMenuItem_view.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.图标ToolStripMenuItem,
            this.详细信息ToolStripMenuItem});
            this.toolStripMenuItem_view.Name = "toolStripMenuItem_view";
            this.toolStripMenuItem_view.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItem_view.Text = "보기";
            // 
            // 图标ToolStripMenuItem
            // 
            this.图标ToolStripMenuItem.Checked = true;
            this.图标ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.图标ToolStripMenuItem.Name = "图标ToolStripMenuItem";
            this.图标ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.图标ToolStripMenuItem.Text = "이미지형식";
            this.图标ToolStripMenuItem.Click += new System.EventHandler(this.图标ToolStripMenuItem_Click);
            // 
            // 详细信息ToolStripMenuItem
            // 
            this.详细信息ToolStripMenuItem.Name = "详细信息ToolStripMenuItem";
            this.详细信息ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.详细信息ToolStripMenuItem.Text = "상세보기";
            this.详细信息ToolStripMenuItem.Click += new System.EventHandler(this.详细信息ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::JustLib.Properties.Resources.refresh_321;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItem1.Text = "새로 고침";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 上传文件ToolStripMenuItem
            // 
            this.上传文件ToolStripMenuItem.Image = global::JustLib.Properties.Resources.upload_2_32;
            this.上传文件ToolStripMenuItem.Name = "上传文件ToolStripMenuItem";
            this.上传文件ToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.上传文件ToolStripMenuItem.Text = "파일업로드";
            this.上传文件ToolStripMenuItem.Click += new System.EventHandler(this.上传文件ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem_uploadFolder
            // 
            this.toolStripMenuItem_uploadFolder.Name = "toolStripMenuItem_uploadFolder";
            this.toolStripMenuItem_uploadFolder.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItem_uploadFolder.Text = "등록부업로드";
            this.toolStripMenuItem_uploadFolder.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem_paste
            // 
            this.toolStripMenuItem_paste.Image = global::JustLib.Properties.Resources.paper_clip_2_32;
            this.toolStripMenuItem_paste.Name = "toolStripMenuItem_paste";
            this.toolStripMenuItem_paste.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItem_paste.Text = "붙이기";
            this.toolStripMenuItem_paste.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // 新建文件夹ToolStripMenuItem
            // 
            this.新建文件夹ToolStripMenuItem.Image = global::JustLib.Properties.Resources.add_folder_32;
            this.新建文件夹ToolStripMenuItem.Name = "新建文件夹ToolStripMenuItem";
            this.新建文件夹ToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.新建文件夹ToolStripMenuItem.Text = "새폴더 만들기";
            this.新建文件夹ToolStripMenuItem.Click += new System.EventHandler(this.新建文件夹ToolStripMenuItem_Click);
            // 
            // NDiskBrowser
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "NDiskBrowser";
            this.Size = new System.Drawing.Size(575, 445);
            this.SizeChanged += new System.EventHandler(this.DirectoryBrowser_SizeChanged);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel_left.ResumeLayout(false);
            this.panel_left.PerformLayout();
            this.skinToolStrip1.ResumeLayout(false);
            this.skinToolStrip1.PerformLayout();
            this.contextMenuStrip_blank.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_parent;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton_root;
        private System.Windows.Forms.ToolStripButton toolStripButton_refresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private FileTransferingViewer fileTransferingViewer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Delete;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_blank;
        private System.Windows.Forms.ToolStripMenuItem 上传文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_downLoad;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Panel panel_left;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ToolStripMenuItem 新建文件夹ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_paste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_view;
        private System.Windows.Forms.ToolStripMenuItem 图标ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 详细信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_uploadFolder;
        private System.Windows.Forms.ListView listView_fileDirectory;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private CCWin.SkinControl.SkinToolStrip skinToolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_msg;
        private System.Windows.Forms.ToolStripButton toolStripButton_state;
    }
}
