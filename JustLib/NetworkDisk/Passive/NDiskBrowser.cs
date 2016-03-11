using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESBasic.Helpers;
using ESPlus.FileTransceiver;
using ESBasic;
using ESFramework;
using ESPlus.Application.FileTransfering.Passive;
using System.Collections;

namespace JustLib.NetworkDisk.Passive
{   
    /// <summary>
    /// 用于浏览远程的文件目录，并提供下载/上传/删除文件的功能。
    /// 构建在JustLib.NetworkDisk基础上。
    /// 如果是网络硬盘（即访问服务器提供的网盘），则只有一个磁盘，名称为"UserID\\"。
    /// </summary>
    public partial class NDiskBrowser : UserControl, IComparer, INDiskBrowser
    {
        private CutOrCopyAction cutOrCopyAction = null;
        private INDiskOutter fileDirectoryOutter;
        private IFileOutter fileOutter;
        private string ownerID; //NetServer.SystemUserID表示请求对象为网络硬盘。
        /// <summary>
        /// 如果为null，表示获取根目录（如“我的电脑”）下的磁盘或目录
        /// </summary>
        private string currentDirPath;
        private int spaceWidth = 0;
        private bool isLableEditing = false;
        private bool ownerSharedAllDisk = false;

        //说明：listView_fileDirectory的ListViewItem的Tag为FileOrDirectoryTag  -- bool，表示IsFile(是否为文件，还是目录或磁盘).

        #region Ctor
        public NDiskBrowser()
        {
            InitializeComponent();
            this.Disposed += new EventHandler(FileDirectoryBrowser_Disposed);

            this.listView_fileDirectory.ListViewItemSorter = this;

            this.listView_fileDirectory.BeforeLabelEdit += new LabelEditEventHandler(listView_fileDirectory_BeforeLabelEdit);
            this.listView_fileDirectory.AfterLabelEdit += new LabelEditEventHandler(listView_fileDirectory_AfterLabelEdit);

            this.spaceWidth = this.Width - this.toolStripTextBox1.Width;

            this.fileTransferingViewer1.FileTransStarted += new CbGeneric<string, bool>(fileTransferingViewer1_FileTransStarted);
            this.fileTransferingViewer1.AllTaskFinished += new CbSimple(fileTransferingViewer1_AllTaskFinished);
            this.fileTransferingViewer1.FileResumedTransStarted += new CbGeneric<string, bool>(fileTransferingViewer1_FileResumedTransStarted);
            this.fileTransferingViewer1.FileTransCompleted += new CbGeneric<string, bool, string, bool>(fileTransferingViewer1_FileTransCompleted);
            this.fileTransferingViewer1.FileTransDisruptted += new ESBasic.CbGeneric<string, bool, FileTransDisrupttedType>(fileTransferingViewer1_FileTransDisruptted);
        }

        public JustLib.Controls.FileTransferingViewer fileTransferViewer
        {
            get { return this.fileTransferingViewer1; }
        }

        void FileDirectoryBrowser_Disposed(object sender, EventArgs e)
        {
            if (this.fileOutter != null)
            {
                this.fileOutter.CancelAllTransfering();
            }
        }

        void fileTransferingViewer1_FileResumedTransStarted(string fileName, bool isSending)
        {
            this.toolStripButton_state.Image = this.imageList2.Images[0];
            string info = string.Format("{0}: {1} {2}파일전송재개", DateTime.Now.ToString(), fileName, isSending ? "업로드" : "다운로드");
            this.toolStripLabel_msg.Text = info;
        }

        void listView_fileDirectory_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            Cursor old = Cursor.Current;
            try
            {
                //输入为空
                if (e.Label == null || e.Label.Length == 0)
                {
                    e.CancelEdit = true;
                    return;
                }

                ListViewItem item = this.listView_fileDirectory.Items[e.Item];
                string oldName = item.Text;
                string newName = e.Label;

                foreach (ListViewItem target in this.listView_fileDirectory.Items)
                {
                    if (newName == target.Text)
                    {
                        MessageBox.Show(String.Format("{0} 이미 존재, 이름을 바꾸세요.", newName));
                        e.CancelEdit = true;
                        return;
                    }
                }

                Cursor.Current = Cursors.WaitCursor;
                if (this.cutOrCopyAction != null && this.currentDirPath == this.cutOrCopyAction.ParentPathOfCuttedOrCopyed && oldName == this.cutOrCopyAction.ItemNameOfCuttedOrCopyed)
                {
                    this.cutOrCopyAction = null;
                }

                OperationResult result = this.fileDirectoryOutter.Rename(this.ownerID, this.currentDirPath, ((FileOrDirectoryTag)item.Tag).IsFile, oldName, newName);
                if (!result.Succeed)
                {
                    e.CancelEdit = true;
                    MessageBox.Show(result.ErrorMessage);
                }
            }
            catch (Exception ee)
            {
                e.CancelEdit = true;
                MessageBox.Show(ee.Message);
            }
            finally
            {
                this.isLableEditing = false;
                Cursor.Current = old;
            }
        }


        void listView_fileDirectory_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            this.isLableEditing = true;
        }

        void fileTransferingViewer1_FileTransDisruptted(string fileName, bool isSending, FileTransDisrupttedType disrupttedType)
        {
            string info = string.Format("{0} {1}중단.", fileName, isSending ? "업로드" : "다운로드");
            if (disrupttedType == FileTransDisrupttedType.ActiveCancel)
            {
                info += "이유: 파일전송을 취소하였습니다.";
            }
            else if (disrupttedType == FileTransDisrupttedType.DestCancel)
            {
                info += "이유: 상대가 파일전송을 취소하였습니다.";
            }
            else if (disrupttedType == FileTransDisrupttedType.DestOffline)
            {
                info += "이유: 상대가 오프라인상태입니다.";
            }
            else if (disrupttedType == FileTransDisrupttedType.SelfOffline)
            {
                info += "이유: 오프라인상태입니다.";
            }
            else if (disrupttedType == FileTransDisrupttedType.DestInnerError)
            {
                info += "이유: 상대시스템 오류.";
            }
            else if (disrupttedType == FileTransDisrupttedType.InnerError)
            {
                info += "이유: 시스템 오류.";
            }
            else if (disrupttedType == FileTransDisrupttedType.ReliableP2PChannelClosed)
            {
                info += "이유: P2P연결이 끊어졌습니다.";
            }
            else
            {
            }

            this.toolStripButton_state.Image = this.imageList2.Images[1];
            this.toolStripLabel_msg.Text = string.Format("{0} {1}", DateTime.Now.ToString(), info);
        }

        void fileTransferingViewer1_FileTransCompleted(string fileName, bool isSending, string comment, bool isFolder)
        {
            this.toolStripButton_state.Image = this.imageList2.Images[0];
            string info = string.Format("{0} {1}완료.", fileName, isSending ? "업로드" : "다운로드");
            this.toolStripLabel_msg.Text = string.Format("{0} {1}", DateTime.Now.ToString(), info);

            if (isSending)
            {
                System.Threading.Thread.Sleep(500);
                this.LoadDirectory(this.currentDirPath, false);
            }
        }

        void fileTransferingViewer1_AllTaskFinished()
        {
            this.fileTransferingViewer1.Visible = false;
        }

        void fileTransferingViewer1_FileTransStarted(string fileName, bool isSending)
        {
            this.fileTransferingViewer1.Visible = true;

            this.toolStripButton_state.Image = this.imageList2.Images[0];
            string info = string.Format("{0} {1}시작.", fileName, isSending ? "업로드" : "다운로드");
            this.toolStripLabel_msg.Text = string.Format("{0}： {1}", DateTime.Now.ToString(), info);
        }

        //手动触发
        void onListViewMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.ownerIsOnline)
            {
                return;
            }

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            ListViewHitTestInfo info = this.listView_fileDirectory.HitTest(e.Location);
            if (info.Item == null)
            {
                return;
            }

            if (!((FileOrDirectoryTag)info.Item.Tag).IsFile) //目录
            {
                string tempPath = this.currentDirPath;
                if (tempPath == null)
                {
                    tempPath = info.Item.Text;
                }
                else
                {
                    tempPath += info.Item.Text;
                }

                this.LoadDirectory(tempPath, true);

                return;
            }

            this.Download(info.Item.Text, true);
        }

        private void Download(string fileOrDirName, bool isFile)
        {
            string tip = "파일다운로드: 보존할 경로를 선택하세요.";

            if (!isFile)
            {
                tip = "폴더다운로드: 보존할 경로를 선택하세요.";
            }

            string savePath = FileHelper.GetPathToSave(tip, fileOrDirName, null);
            if (savePath == null)
            {
                return;
            }

            if (File.Exists(savePath) || Directory.Exists(savePath))
            {
                if (MessageBox.Show(string.Format("{0}이미 존재합니다. 덮어쓰기 하시겠습니까?", FileHelper.GetFileNameNoPath(savePath)), "메시지", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            OperationResult operationResult = this.fileDirectoryOutter.Download(this.ownerID, this.currentDirPath + fileOrDirName, savePath, isFile);
            if (!operationResult.Succeed)
            {
                MessageBox.Show(operationResult.ErrorMessage);
                this.LoadDirectory(this.currentDirPath, true);
            }
        }
        #endregion

        #region Property
        #region ownerIsOnline
        private bool ownerIsOnline = true;
        /// <summary>
        /// 当前用户与Onwer是否处于连接状态。
        /// </summary>
        public bool Connected
        {
            get { return ownerIsOnline; }
            set
            {
                ownerIsOnline = value;
                this.toolStripButton_parent.Enabled = value;
                this.toolStripButton_root.Enabled = value;
                this.toolStripButton_refresh.Enabled = value;
            }
        }
        #endregion

        #region CurrentDirectoryPath
        public string CurrentDirectoryPath
        {
            get
            {
                return this.currentDirPath;
            }
        }
        #endregion

        #region IsNetworkDisk
        /// <summary>
        /// IsNetworkDisk 是否正在访问网络硬盘。
        /// </summary>
        private bool IsNetworkDisk
        {
            get
            {
                return this.ownerID == NetServer.SystemUserID;
            }
        }
        #endregion

        #region AllowUploadFolder
        private bool allowUploadFolder = false;
        /// <summary>
        /// 是否允许上传文件夹
        /// </summary>
        public bool AllowUploadFolder
        {
            get { return allowUploadFolder; }
            set { allowUploadFolder = value; }
        }
        #endregion

        #region IsFileTransfering
        public bool IsFileTransfering
        {
            get
            {
                return this.fileTransferingViewer1.IsFileTransfering;
            }
        }
        #endregion

        #region OwnerID
        public string OwnerID
        {
            get
            {
                if (this.ownerID == NetServer.SystemUserID)
                {
                    return null;
                }
                return this.ownerID;
            }
        }
        #endregion

        #endregion

        #region LoadDirectory
        private void LoadDirectory(string path, bool tipOnException)
        {
            if (this.ownerID == null)
            {
                return;
            }

            Cursor old = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SharedDirectory sharedDirectory = this.fileDirectoryOutter.GetSharedDirectory(this.ownerID, path);
                if (sharedDirectory == null)
                {
                    MessageBox.Show("네트워크하드디스크 열기오류.");
                }
                else if (!sharedDirectory.Valid)
                {
                    MessageBox.Show(sharedDirectory.Exception);
                }
                else
                {
                    if (path == null)
                    {
                        this.ownerSharedAllDisk = sharedDirectory.DirectoryPath == null;
                    }

                    #region Action
                    this.listView_fileDirectory.Items.Clear();
                    if (sharedDirectory.DirectoryPath == null)
                    {
                        sharedDirectory.DriveList.Sort();
                        foreach (DiskDrive drive in sharedDirectory.DriveList)
                        {
                            int imageIndex = 2;
                            if (drive.DriveType == DriveType.CDRom)
                            {
                                imageIndex = 3;
                            }
                            if (drive.DriveType == DriveType.Removable)
                            {
                                imageIndex = 4;
                            }
                            ListViewItem item = new ListViewItem(new string[] { drive.Name, "", "" }, imageIndex);
                            item.Tag = new FileOrDirectoryTag(drive.Name, 0, DateTime.Now, false);
                            string name = drive.VolumeLabel;
                            if (name == null || name.Length == 0)
                            {
                                name = drive.Name;
                            }
                            item.ToolTipText = string.Format("{0}\n사용공간{1}\n총 크기{2}", name, PublicHelper.GetSizeString(drive.AvailableFreeSpace), PublicHelper.GetSizeString(drive.TotalSize));
                            this.listView_fileDirectory.Items.Add(item);
                        }
                    }
                    else
                    {
                        foreach (DirectoryDetail dirDetail in sharedDirectory.SubDirectorys)
                        {
                            ListViewItem item = new ListViewItem(new string[] { dirDetail.Name, dirDetail.CreateTime.ToString(), "" }, 0);
                            //ListViewItem item = this.listView_fileDirectory.Items.Add(dirName, 0);
                            item.Tag = new FileOrDirectoryTag(dirDetail.Name, 0, dirDetail.CreateTime, false);
                            this.listView_fileDirectory.Items.Add(item);
                        }

                        foreach (FileDetail file in sharedDirectory.FileList)
                        {
                            ListViewItem item = new ListViewItem(new string[] { file.Name, file.CreateTime.ToString(), PublicHelper.GetSizeString((uint)file.Size) }, this.GetIconIndex(file.Name));
                            //ListViewItem item = this.listView_fileDirectory.Items.Add(file.Name, this.GetIconIndex(file.Name));
                            item.Tag = new FileOrDirectoryTag(file.Name, file.Size, file.CreateTime, true);
                            item.ToolTipText = string.Format("크  기{0}\n작성날짜{1}", PublicHelper.GetSizeString((uint)file.Size), file.CreateTime);
                            this.listView_fileDirectory.Items.Add(item);
                        }

                        this.columnIndexToSort = 0;
                        this.asendingOrder = true;
                        this.listView_fileDirectory.Sort();
                    }

                    this.currentDirPath = path;
                    if (this.currentDirPath != null && !this.currentDirPath.EndsWith("\\"))
                    {
                        this.currentDirPath += "\\";
                    }

                    string displayPath = this.IsNetworkDisk ? "네트워크 하드디스크" : "공유 하드디스크" ;
                    if (this.currentDirPath != null && this.currentDirPath != sharedDirectory.DirectoryPath)
                    {
                        displayPath += "\\" + this.currentDirPath; 
                    }
                    this.toolStripTextBox1.Text = displayPath;

                    this.listView_fileDirectory.LabelEdit = (sharedDirectory.DirectoryPath != null);
                    #endregion
                }
            }
            catch (Exception ee)
            {
                if (tipOnException)
                {
                    MessageBox.Show(ee.Message);
                }
            }
            finally
            {
                Cursor.Current = old;
            }
        }

        private Dictionary<string, int> iconIndexDic = new Dictionary<string, int>();// ".txt" - 3
        private int GetIconIndex(string fileName)
        {
            string[] ary = fileName.Split('.');
            if (ary.Length == 1)
            {
                return 1;
            }

            try
            {
                string extendName = "." + ary[ary.Length - 1].ToLower();
                if (!this.iconIndexDic.ContainsKey(extendName))
                {
                    int index = 1;
                    Icon icon = WindowsHelper.GetSystemIconByFileType(extendName, true);
                    if (icon != null)
                    {
                        this.imageList1.Images.Add(icon);
                        index = this.imageList1.Images.Count - 1;
                    }
                    this.iconIndexDic.Add(extendName, index);
                }

                return this.iconIndexDic[extendName];
            }
            catch (Exception ee)
            {
                ee = ee;
                return 1;
            }
        }
        #endregion

        #region Initialize
        public void Initialize(string _ownerID, IFileOutter _fileOutter, INDiskOutter outter)
        {
            this.ownerID = _ownerID ?? NetServer.SystemUserID;
            this.currentDirPath = null;
            this.cutOrCopyAction = null;
            this.fileOutter = _fileOutter;
            this.fileDirectoryOutter = outter;
            this.fileTransferingViewer1.Initialize(this.fileOutter, delegate(TransferingProject pro) { return pro.Comment != null; });
            this.LoadDirectory(null, true);
        }

        /// <summary>
        /// 调用该方法可恢复到未做任何连接的初始状态。
        /// </summary>
        public void Reset()
        {
            this.ownerID = null;
            this.currentDirPath = null;
            this.cutOrCopyAction = null;
            this.fileOutter = null;
            this.fileDirectoryOutter = null;
            this.listView_fileDirectory.Clear();
            this.toolStripButton_state.Image = this.imageList2.Images[0];
            this.toolStripLabel_msg.Text = "준비";
            this.toolStripTextBox1.Text = "";
        }
        #endregion

        #region CancelAllTransfering
        public void CancelAllTransfering()
        {
            this.fileOutter.CancelTransferingAbout(this.ownerID);
        }
        #endregion

        #region event handler
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.GotoParent();
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            this.GotoParent();
        }

        private void GotoParent()
        {
            try
            {
                if ((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null)
                {
                    return;
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(this.currentDirPath);
                string temp = this.currentDirPath.Substring(0, this.currentDirPath.Length - 1);
                if (directoryInfo.Parent == null || !temp.Contains("\\"))
                {
                    this.LoadDirectory(null, true);
                    return;
                }

                //if (this.IsNetworkDisk)
                //{
                //    int pos = temp.LastIndexOf('\\');
                //    string relativeDir = temp.Substring(0, pos) + "\\";
                //    this.LoadDirectory(relativeDir, true);
                //}
                //else
                //{
                //    this.LoadDirectory(directoryInfo.Parent.FullName, true);
                //}

                int pos = temp.LastIndexOf('\\');
                string relativeDir = temp.Substring(0, pos) + "\\";
                this.LoadDirectory(relativeDir, true);
            }
            catch (Exception ee)
            {
                ee = ee;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.LoadDirectory(this.currentDirPath, true);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.图标ToolStripMenuItem.PerformClick();
            this.LoadDirectory(null, true);
        }

        private void DirectoryBrowser_SizeChanged(object sender, EventArgs e)
        {
            int newWidth = this.Width - this.spaceWidth;
            if (newWidth < 10)
            {
                newWidth = 10;
            }

            this.toolStripTextBox1.Width = newWidth;
        }
        #endregion

        #region ContextMenu
        //上传文件夹
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                string dirPath = FileHelper.GetFolderToOpen(false);
                if (dirPath == null)
                {
                    return;
                }

                ulong dirSize = FileHelper.GetDirectorySize(dirPath);
                if (this.IsNetworkDisk)
                {
                    NetworkDiskState state = this.fileDirectoryOutter.GetNetworkDiskState();
                    ulong available = state.TotalSize - state.SizeUsed;
                    if (available < dirSize)
                    {
                        MessageBox.Show(string.Format("공간부족!네트워크디스크 남은공간: {0}, 필요한 공간: {1}", PublicHelper.GetSizeString(available), PublicHelper.GetSizeString(dirSize)));
                        return;
                    }
                }

                string containerPath = this.currentDirPath;
                string[] names = dirPath.Split('\\');
                string dirName = names[names.Length - 1];
                this.fileDirectoryOutter.Upload(this.ownerID, dirPath, containerPath + dirName);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void 上传文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = FileHelper.GetFileToOpen("업로드할 파일을 선택하세요.");
                if (filePath == null)
                {
                    return;
                }

                string fileName = FileHelper.GetFileNameNoPath(filePath);
                foreach (ListViewItem item in this.listView_fileDirectory.Items)
                {
                    if (((FileOrDirectoryTag)item.Tag).IsFile && item.Text.ToLower() == fileName.ToLower())
                    {
                        if (MessageBox.Show(string.Format("{0}이미 존재합니다. 덮어쓰기 하시겠습니까?", fileName), "메시지", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                if (this.IsNetworkDisk)
                {
                    ulong fileSize = FileHelper.GetFileSize(filePath);
                    NetworkDiskState state = this.fileDirectoryOutter.GetNetworkDiskState();
                    ulong available = state.TotalSize - state.SizeUsed;
                    if (available < fileSize)
                    {
                        MessageBox.Show(string.Format("네트워크하드디스크공간: {0}, 필요한 공간: {1}", PublicHelper.GetSizeString(available), PublicHelper.GetSizeString(fileSize)));
                        return;
                    }
                }


                this.fileDirectoryOutter.Upload(this.ownerID, filePath, this.currentDirPath + fileName);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }



        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.DeleteFileOrDir();
        }

        private void DeleteFileOrDir()
        {
            if (((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null) || this.listView_fileDirectory.SelectedItems.Count == 0)
            {
                return;
            }

            ListViewItem item = this.listView_fileDirectory.SelectedItems[0];

            if (MessageBox.Show(string.Format("정말 {0} {1} 를 삭제하시겠습니까?", ((FileOrDirectoryTag)item.Tag).IsFile ? "파일" : "폴더", item.Text), "메시지", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (this.cutOrCopyAction != null && this.currentDirPath == this.cutOrCopyAction.ParentPathOfCuttedOrCopyed && item.Text == this.cutOrCopyAction.ItemNameOfCuttedOrCopyed)
            {
                this.cutOrCopyAction = null;
            }

            List<string> files = new List<string>();
            List<string> dirs = new List<string>();
            if (((FileOrDirectoryTag)item.Tag).IsFile)
            {
                files.Add(item.Text);
            }
            else
            {
                dirs.Add(item.Text);
            }

            OperationResult result = this.fileDirectoryOutter.Delete(this.ownerID, this.currentDirPath, files, dirs);
            if (!result.Succeed)
            {
                MessageBox.Show(result.ErrorMessage);
            }
            this.LoadDirectory(this.currentDirPath, true);
        }
        #endregion

        #region event Handler
        private void listView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (!this.ownerIsOnline)
            {
                return;
            }

            if (this.isLableEditing)
            {
                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                this.DeleteFileOrDir();
            }
        }

        private DateTime lastTimeMouseLeftButtonDown = DateTime.Now;
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.ownerIsOnline)
            {
                this.listView_fileDirectory.ContextMenuStrip = null;
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if ((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null)
                {
                    this.listView_fileDirectory.ContextMenuStrip = null;
                }
                else
                {
                    ListViewHitTestInfo info = this.listView_fileDirectory.HitTest(e.Location);
                    this.listView_fileDirectory.ContextMenuStrip = (info.Item == null ? this.contextMenuStrip_blank : this.contextMenuStrip1);
                    this.toolStripMenuItem_uploadFolder.Visible = this.allowUploadFolder;
                    if (info.Item != null)
                    {
                        this.toolStripMenuItem_downLoad.Visible = ((FileOrDirectoryTag)info.Item.Tag).IsFile || this.allowUploadFolder;
                    }
                    else
                    {
                        this.toolStripMenuItem_paste.Visible = this.cutOrCopyAction != null;
                    }
                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                TimeSpan span = DateTime.Now - this.lastTimeMouseLeftButtonDown;
                this.lastTimeMouseLeftButtonDown = DateTime.Now;
                if (span.TotalMilliseconds < 300)
                {
                    this.onListViewMouseDoubleClick(this.listView_fileDirectory, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 2, e.X, e.Y, 0));
                    return;
                }

                if ((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null)
                {
                    return;
                }

                ListViewHitTestInfo info = this.listView_fileDirectory.HitTest(e.Location);
                if (info.Item == null)
                {
                    return;
                }

                this.DoDragDrop(info.Item, (Control.ModifierKeys & Keys.Control) == Keys.Control ? DragDropEffects.Copy : DragDropEffects.Move);
            }
        }

        private void toolStripMenuItem_downLoad_Click(object sender, EventArgs e)
        {
            if (((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null) || this.listView_fileDirectory.SelectedItems.Count == 0)
            {
                return;
            }

            ListViewItem item = this.listView_fileDirectory.SelectedItems[0];

            this.Download(item.Text, ((FileOrDirectoryTag)item.Tag).IsFile);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.LoadDirectory(this.currentDirPath, true);
        }

        private void 新建文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor old = Cursor.Current;
            try
            {
                string name = "새 폴더";
                bool found = true;
                int i = 1;
                while (found)
                {
                    found = false;
                    foreach (ListViewItem target in this.listView_fileDirectory.Items)
                    {
                        if (!((FileOrDirectoryTag)target.Tag).IsFile && name == target.Text)
                        {
                            found = true;
                        }
                    }

                    if (found)
                    {
                        name = "새 폴더" + i.ToString();
                        i++;
                    }
                }

                Cursor.Current = Cursors.WaitCursor;
                this.fileDirectoryOutter.CreateDirectory(this.ownerID, this.currentDirPath, name);

                ListViewItem item = new ListViewItem(new string[] { name, DateTime.Now.ToString(), "" }, 0);
                item.Tag = new FileOrDirectoryTag(name, 0, DateTime.Now, false);
                this.listView_fileDirectory.Items.Add(item);
                item.BeginEdit();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                this.isLableEditing = false;
                Cursor.Current = old;
            }
        }

        private void toolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            if (this.listView_fileDirectory.SelectedItems.Count == 0)
            {
                return;
            }

            this.listView_fileDirectory.SelectedItems[0].BeginEdit();
        }
        #endregion

        #region 拖拽上传
        private void listView_fileDirectory_DragOver(object sender, DragEventArgs e)
        {
            if ((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null)
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                ListViewItem target = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                ListViewHitTestInfo info = this.listView_fileDirectory.HitTest(this.PointToClient(new Point(e.X, e.Y)));
                if (info.Item != null && !((FileOrDirectoryTag)info.Item.Tag).IsFile)
                {
                    info.Item.Selected = true;
                }
                else
                {
                    foreach (ListViewItem item in this.listView_fileDirectory.Items)
                    {
                        item.Selected = (item == target);
                    }
                }

                if (target == null || (Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void listView_fileDirectory_DragEnter(object sender, DragEventArgs e)
        {
            if ((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null)
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void listView_fileDirectory_DragDrop(object sender, DragEventArgs e)
        {
            if ((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null)
            {
                return;
            }

            try
            {
                string containerPath = this.currentDirPath;
                ListViewHitTestInfo containerInfo = this.listView_fileDirectory.HitTest(this.PointToClient(new Point(e.X, e.Y)));
                if (containerInfo.Item != null && !((FileOrDirectoryTag)containerInfo.Item.Tag).IsFile)
                {
                    containerPath += containerInfo.Item.Text + "\\";
                }

                ListViewItem targetItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                if (targetItem != null)
                {
                    #region 内部拖动
                    if (targetItem == containerInfo.Item)
                    {
                        return;
                    }

                    if (containerPath != this.currentDirPath)
                    {
                        if (this.ExistSameNameItem(containerPath, targetItem.Text))
                        {
                            MessageBox.Show("같은 이름의 파일이나 폴더가 이미 존재합니다. 이름을 변경하세요.");
                            return;
                        }
                    }

                    List<string> fileNames = new List<string>();
                    List<string> dirNames = new List<string>();
                    if (((FileOrDirectoryTag)targetItem.Tag).IsFile)
                    {
                        fileNames.Add(targetItem.Text);
                    }
                    else
                    {
                        dirNames.Add(targetItem.Text);
                    }

                    OperationResult result = null;
                    if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        result = this.fileDirectoryOutter.Copy(this.ownerID, this.currentDirPath, fileNames, dirNames, containerPath);
                    }
                    else
                    {
                        if (this.currentDirPath != containerPath)
                        {
                            result = this.fileDirectoryOutter.Move(this.ownerID, this.currentDirPath, fileNames, dirNames, containerPath);
                        }
                    }

                    if (result != null && !result.Succeed)
                    {
                        MessageBox.Show(result.ErrorMessage);
                    }

                    this.LoadDirectory(this.currentDirPath, true);

                    #endregion
                    return;
                }


                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    #region 从外部拖入
                    string[] fileOrDirs = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (!this.allowUploadFolder)
                    {
                        foreach (string fileOrDirPath in fileOrDirs)
                        {
                            if (Directory.Exists(fileOrDirPath))
                            {
                                MessageBox.Show("등록부업로드를 할수가 없습니다.");
                                return;
                            }
                        }
                    }

                    ulong fileSize = 0;
                    foreach (string fileOrDirPath in fileOrDirs)
                    {
                        if (File.Exists(fileOrDirPath))
                        {
                            fileSize += FileHelper.GetFileSize(fileOrDirPath);
                        }

                        if (Directory.Exists(fileOrDirPath))
                        {
                            fileSize += FileHelper.GetDirectorySize(fileOrDirPath);
                        }

                        string fileOrDirName = FileHelper.GetFileNameNoPath(fileOrDirPath);

                        if (this.ExistSameNameItem(containerPath, fileOrDirName))
                        {
                            MessageBox.Show("같은 이름의 파일이나 폴더가 이미 존재합니다. 이름을 변경하세요.");
                            return;
                        }
                    }

                    if (this.IsNetworkDisk)
                    {
                        NetworkDiskState state = this.fileDirectoryOutter.GetNetworkDiskState();
                        ulong available = state.TotalSize - state.SizeUsed;
                        if (available < fileSize)
                        {
                            MessageBox.Show(string.Format("공간부족!네트워크하드디스크 남은공간: {0}, 필요한 공간: {1}", PublicHelper.GetSizeString(available), PublicHelper.GetSizeString(fileSize)));
                            return;
                        }
                    }

                    foreach (string fileOrDirPath in fileOrDirs)
                    {
                        if (File.Exists(fileOrDirPath))
                        {
                            string fileName = FileHelper.GetFileNameNoPath(fileOrDirPath);
                            this.fileDirectoryOutter.Upload(this.ownerID, fileOrDirPath, containerPath + fileName);
                        }

                        if (Directory.Exists(fileOrDirPath))
                        {
                            string[] names = fileOrDirPath.Split('\\');
                            string dirName = names[names.Length - 1];
                            this.fileDirectoryOutter.Upload(this.ownerID, fileOrDirPath, containerPath + dirName);
                            //this.UploadDirectory(filePath, containerPath);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        #region ExistSameNameItem
        /// <summary>
        /// 目标目录下是否存在同名的项目。
        /// </summary>       
        private bool ExistSameNameItem(string dirPath, string itemName)
        {
            if (this.currentDirPath == dirPath)
            {
                foreach (ListViewItem item in this.listView_fileDirectory.Items)
                {
                    if (item.Text.ToLower() == itemName.ToLower())
                    {
                        return true;
                    }
                }
            }
            else
            {
                SharedDirectory containerDirectory = this.fileDirectoryOutter.GetSharedDirectory(this.ownerID, dirPath);
                foreach (FileDetail fileDetail in containerDirectory.FileList)
                {
                    if (fileDetail.Name.ToLower() == itemName.ToLower())
                    {
                        return true;
                    }
                }

                foreach (DirectoryDetail dirDetail in containerDirectory.SubDirectorys)
                {
                    if (dirDetail.Name.ToLower() == itemName.ToLower())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion

        #endregion

        #region 复制与粘贴
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (this.listView_fileDirectory.SelectedItems.Count == 0)
            {
                return;
            }

            this.cutOrCopyAction = new CutOrCopyAction(this.currentDirPath, this.listView_fileDirectory.SelectedItems[0].Text, ((FileOrDirectoryTag)this.listView_fileDirectory.SelectedItems[0].Tag).IsFile, false);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (this.listView_fileDirectory.SelectedItems.Count == 0)
            {
                return;
            }

            this.cutOrCopyAction = new CutOrCopyAction(this.currentDirPath, this.listView_fileDirectory.SelectedItems[0].Text, ((FileOrDirectoryTag)this.listView_fileDirectory.SelectedItems[0].Tag).IsFile, true);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (this.cutOrCopyAction == null)
            {
                return;
            }


            if (this.currentDirPath.StartsWith(this.cutOrCopyAction.ParentPathOfCuttedOrCopyed + this.cutOrCopyAction.ItemNameOfCuttedOrCopyed))
            {
                MessageBox.Show("대상폴더는 원본폴더의 자식폴더입니다.");
                return;
            }

            List<string> fileNames = new List<string>();
            List<string> dirNames = new List<string>();
            if (this.cutOrCopyAction.IsFile)
            {
                fileNames.Add(this.cutOrCopyAction.ItemNameOfCuttedOrCopyed);
            }
            else
            {
                dirNames.Add(this.cutOrCopyAction.ItemNameOfCuttedOrCopyed);
            }

            OperationResult result = null;
            if (this.cutOrCopyAction.IsCutted)
            {
                result = this.fileDirectoryOutter.Move(this.ownerID, this.cutOrCopyAction.ParentPathOfCuttedOrCopyed, fileNames, dirNames, this.currentDirPath);
                this.cutOrCopyAction = null;
            }
            else
            {
                result = this.fileDirectoryOutter.Copy(this.ownerID, this.cutOrCopyAction.ParentPathOfCuttedOrCopyed, fileNames, dirNames, this.currentDirPath);
            }

            if (result != null && !result.Succeed)
            {
                MessageBox.Show(result.ErrorMessage);
            }

            this.LoadDirectory(this.currentDirPath, true);
        }
        #endregion

        #region 查看 -- 详细、图标
        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView_fileDirectory.View = View.Details;
            this.详细信息ToolStripMenuItem.Checked = true;
            this.图标ToolStripMenuItem.Checked = false;
        }

        private void 图标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView_fileDirectory.View = View.LargeIcon;
            this.详细信息ToolStripMenuItem.Checked = false;
            this.图标ToolStripMenuItem.Checked = true;
        }
        #endregion

        #region 排序
        private bool asendingOrder = true;
        private int columnIndexToSort = 0;
        public int Compare(object x, object y)
        {
            FileOrDirectoryTag xTag = (FileOrDirectoryTag)((ListViewItem)x).Tag;
            FileOrDirectoryTag yTag = (FileOrDirectoryTag)((ListViewItem)y).Tag;

            if (xTag.IsFile == !yTag.IsFile)
            {
                int res = xTag.IsFile ? 1 : -1;
                return asendingOrder ? res : -res;
            }

            if (this.columnIndexToSort == 0)
            {
                int restult = xTag.Name.CompareTo(yTag.Name);
                return asendingOrder ? restult : -restult;
            }

            if (this.columnIndexToSort == 1)
            {
                int restult2 = xTag.CreatTime.CompareTo(yTag.CreatTime);
                return asendingOrder ? restult2 : -restult2;
            }

            int restult1 = (int)(xTag.Size - yTag.Size);
            return asendingOrder ? restult1 : -restult1;
        }

        private void listView_fileDirectory_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (this.columnIndexToSort == e.Column)
            {
                this.asendingOrder = !this.asendingOrder;
            }
            else
            {
                this.columnIndexToSort = e.Column;
                this.asendingOrder = true;
            }

            this.listView_fileDirectory.Sort();
        }
        #endregion
    }

    #region CutOrCopyAction
    internal class CutOrCopyAction
    {
        public CutOrCopyAction() { }
        public CutOrCopyAction(string parentPath, string itemName, bool _isFile, bool _isCutted)
        {
            this.parentPathOfCuttedOrCopyed = parentPath;
            this.itemNameOfCuttedOrCopyed = itemName;
            this.isCutted = _isCutted;
            this.isFile = _isFile;
        }

        #region ParentPathOfCuttedOrCopyed
        private string parentPathOfCuttedOrCopyed = null;
        /// <summary>
        /// 被剪切或复制物品的父目录的的路径
        /// </summary>
        public string ParentPathOfCuttedOrCopyed
        {
            get { return parentPathOfCuttedOrCopyed; }
            set { parentPathOfCuttedOrCopyed = value; }
        }
        #endregion

        #region ItemNameOfCuttedOrCopyed
        private string itemNameOfCuttedOrCopyed = null;
        /// <summary>
        /// 被复制或剪切的文件夹或文件的名称
        /// </summary>
        public string ItemNameOfCuttedOrCopyed
        {
            get { return itemNameOfCuttedOrCopyed; }
            set { itemNameOfCuttedOrCopyed = value; }
        }
        #endregion

        #region IsFile
        private bool isFile = true;
        public bool IsFile
        {
            get { return isFile; }
            set { isFile = value; }
        }
        #endregion

        #region IsCutted
        private bool isCutted = true;
        public bool IsCutted
        {
            get { return isCutted; }
            set { isCutted = value; }
        }
        #endregion
    } 
    #endregion

    #region FileOrDirectoryTag
    internal class FileOrDirectoryTag
    {
        public FileOrDirectoryTag() { }       
        public FileOrDirectoryTag(string _name, long _size, DateTime time ,bool _isFile)
        {
            this.name = _name;
            this.size = _size;
            this.creatTime = time;
            this.isFile = _isFile;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private long size;
        public long Size
        {
            get { return size; }
            set { size = value; }
        }

        private DateTime creatTime;
        public DateTime CreatTime
        {
            get { return creatTime; }
            set { creatTime = value; }
        }

        private bool isFile = true;
        public bool IsFile
        {
            get { return isFile; }
            set { isFile = value; }
        }
    } 
    #endregion
     
}
