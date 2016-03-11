using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using CCWin;
using CCWin.SkinControl;
using System.Runtime.InteropServices;
using CCWin.Win32;
using ESPlus.Application.CustomizeInfo;
using ESBasic;
using ESPlus.Rapid;
using ESBasic.ObjectManagement.Forms;
using ESPlus.Application.FileTransfering;
using ESPlus.FileTransceiver;
using ESPlus.Application.Basic;
using OMCS.Passive;
using System.Configuration;
using ESBasic.ObjectManagement.Managers;
using ESPlus.Serialization;
using GG2014.Core;
using ESBasic.Helpers;
using JustLib;
using JustLib.UnitViews;
using JustLib.NetworkDisk.Passive;
using JustLib.NetworkDisk;
using GG2014;

namespace MultiRemotePC
{
    public partial class MainForm : Form, IHeadImageGetter
    {
        private bool initialized = false;
        private UserStatus myStatus = UserStatus.OffLine; //用于断线重连       
        private IRapidPassiveEngine rapidPassiveEngine;
        private GlobalUserCache globalUserCache; //缓存用户资料
        
        private INDiskOutter nDiskOutter; // V2.0            
        private List<Panel> listPanel = new List<Panel>();

        public MainForm()
        {
            InitializeComponent();
            
            UiSafeInvoker invoker = new UiSafeInvoker(this, true, true, GlobalResourceManager.Logger);
            GlobalResourceManager.SetUiSafeInvoker(invoker);

            this.friendListBox1.AddCatalogClicked += new CbGeneric(friendListBox1_AddCatalogClicked);
            this.friendListBox1.ChangeCatalogNameClicked += new CbGeneric<string>(friendListBox1_ChangeCatalogNameClicked);
            this.friendListBox1.UserDoubleClicked += new CbGeneric<IUser>(friendListBox1_UserDoubleClicked);
            this.friendListBox1.RemoveUserClicked += new CbGeneric<IUser>(friendListBox1_RemoveUserClicked);
            this.friendListBox1.ChatRecordClicked += new CbGeneric<IUser>(friendListBox1_ChatRecordClicked);
            this.friendListBox1.CatalogAdded += new CbGeneric<string>(friendListBox1_CatalogAdded);
            this.friendListBox1.CatalogNameChanged += new CbGeneric<string, string, bool>(friendListBox1_CatalogNameChanged);
            this.friendListBox1.CatalogRemoved += new CbGeneric<string>(friendListBox1_CatalogRemoved);
            this.friendListBox1.FriendCatalogMoved += new CbGeneric<string, string, string>(friendListBox1_FriendCatalogMoved);

            listPanel.Add(this.panel1);
            listPanel.Add(this.panel2);
            listPanel.Add(this.panel3);
            listPanel.Add(this.panel4);
        }

        void friendListBox1_AddCatalogClicked()
        {
            EditCatelogNameForm form = new EditCatelogNameForm();
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.friendListBox1.AddCatalog(form.NewName);
            }
        }

        void friendListBox1_ChangeCatalogNameClicked(string catalogName)
        {
            EditCatelogNameForm form = new EditCatelogNameForm(catalogName);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.friendListBox1.ChangeCatelogName(catalogName, form.NewName);
            }
        }

        void friendListBox1_FriendCatalogMoved(string friendID, string oldCatalog, string newCatalog)
        {
            this.globalUserCache.CurrentUser.MoveFriend(friendID, oldCatalog, newCatalog);
            MoveFriendToOtherCatalogContract contract = new MoveFriendToOtherCatalogContract(friendID, oldCatalog, newCatalog);
            byte[] info = CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.CustomizeOutter.Send(InformationTypes.MoveFriendToOtherCatalog, info);
        }

        void friendListBox1_CatalogRemoved(string catalog)
        {
            this.globalUserCache.CurrentUser.RemvoeFriendCatalog(catalog);
            this.rapidPassiveEngine.CustomizeOutter.Send(InformationTypes.RemoveFriendCatalog, System.Text.Encoding.UTF8.GetBytes(catalog));
        }

        void friendListBox1_CatalogNameChanged(string oldName, string newName, bool isMerge)
        {
            this.globalUserCache.CurrentUser.ChangeFriendCatalogName(oldName, newName);

            ChangeCatalogContract contract = new ChangeCatalogContract(oldName, newName);
            byte[] info = CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.CustomizeOutter.Send(InformationTypes.ChangeFriendCatalogName, info);
        }

        void friendListBox1_CatalogAdded(string catalog)
        {
            this.globalUserCache.CurrentUser.AddFriendCatalog(catalog);
            this.rapidPassiveEngine.CustomizeOutter.Send(InformationTypes.AddFriendCatalog, System.Text.Encoding.UTF8.GetBytes(catalog));
        }

        void friendListBox1_ChatRecordClicked(IUser friend)
        {
            //ChatRecordForm form = new ChatRecordForm(GlobalResourceManager.RemotingService, GlobalResourceManager.ChatMessageRecordPersister, this.globalUserCache.CurrentUser.GetIDName(), friend.GetIDName());
            //form.Show();
        }

        void friendListBox1_UserDoubleClicked(IUser friend)
        {
            ShowRemotePC(friend);
        }

        private void ShowRemotePC(IUser friend)
        {
            if(CheckIfFormIsOpen(friend.ID))
            {
                MessageBox.Show("이미 현시되어있습니다.", "메시지");
                return;
            }

            if (friend.UserStatus == UserStatus.OffLine)
            {
                MessageBox.Show("오프라인상태입니다.", "메시지");
                return;
            }

            if (CheckExist(friend) == false)
            {
                MessageBox.Show("이미 현시되어있습니다.", "메시지");
                return;
            }

            foreach (Panel panel in this.listPanel)
            {
                if (panel.Controls.Count == 0)
                {
                    RemotePC remotePC = new RemotePC(friend.ID);

                    remotePC.fileOuter = this.rapidPassiveEngine.FileOutter;
                    remotePC.nDiskOutter = this.nDiskOutter;
                    remotePC.rapidPassiveEngine = this.rapidPassiveEngine;

                    remotePC.RemoteHelpEnded += new CbGeneric<string>(remoteHelpForm_RemoteHelpEnded);
                    remotePC.Dock = DockStyle.Fill;
                    remotePC._strUserID = friend.ID;

                    panel.Controls.Add(remotePC);
                    return;
                }
            }

            MessageBox.Show("방이 꽉 차있습니다.");
        }

        public bool CheckIfFormIsOpen(string formname)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Name == formname)
                {
                    return true;
                }
            }
            return false;
        }

        void remoteHelpForm_RemoteHelpEnded(string strID)
        {
            foreach (Panel panel in listPanel)
            {
                if (panel.Controls.Count > 0)
                {
                    if (((RemotePC)panel.Controls[0])._strUserID == strID)
                    {
                        panel.Controls.Clear();
                        return;
                    }
                }
            }
        }

        private bool CheckExist(IUser friend)
        {
            foreach (Panel panel in this.listPanel)
            {
                if (panel.Controls.Count > 0)
                {
                    if (((RemotePC)panel.Controls[0])._strUserID == friend.ID)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        void friendListBox1_RemoveUserClicked(IUser friend)
        {
            if (this.globalUserCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            try
            {
                if (friend.ID == this.rapidPassiveEngine.CurrentUserID)
                {
                    return;
                }

                if (MessageBox.Show(string.Format("정말 {0}를 삭제하겠습니까?", friend.ID), "메시지", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }

                //SendCertainly 发送请求，并等待Ack回复
                this.rapidPassiveEngine.CustomizeOutter.SendCertainly(null, InformationTypes.RemoveFriend, System.Text.Encoding.UTF8.GetBytes(friend.ID));
                this.globalUserCache.CurrentUser.RemoveFriend(friend.ID);
                this.friendListBox1.RemoveUser(friend.ID);

                // 从recent中删除
                //this.recentListBox1.RemoveUnit(friend);
                //ChatForm chatForm = this.chatFormManager.GetForm(friend.ID);
                //if (chatForm != null)
                //{
                //    chatForm.Close();
                //}

                this.globalUserCache.RemovedFriend(friend.ID);
            }
            catch (Exception ee)
            {
                MessageBox.Show("Request TimeOut: " + ee.Message, GlobalResourceManager.SoftwareName);
            }
        }

        public void Initialize(IRapidPassiveEngine engine)//, Image headImage, string nickName, ChatListSubItem.UserStatus userStatus, Image stateImage)
        {
            GlobalResourceManager.Initialize(engine.CurrentUserID);
            this.Cursor = Cursors.WaitCursor;

            this.rapidPassiveEngine = engine;

            this.globalUserCache = new GlobalUserCache(this.rapidPassiveEngine);
            this.globalUserCache.FriendInfoChanged += new CbGeneric<User>(globalUserCache_FriendInfoChanged);
            this.globalUserCache.FriendStatusChanged += new CbGeneric<User>(globalUserCache_FriendStatusChanged);
            this.globalUserCache.GroupChanged += new CbGeneric<Group, GroupChangedType, string>(globalUserCache_GroupInfoChanged);
            this.globalUserCache.FriendRTDataRefreshCompleted += new CbGeneric(globalUserCache_FriendRTDataRefreshCompleted);
            this.globalUserCache.FriendRemoved += new CbGeneric<string>(globalUserCache_FriendRemoved);
            this.globalUserCache.FriendAdded += new CbGeneric<User>(globalUserCache_FriendAdded);

            this.globalUserCache.CurrentUser.UserStatus = UserStatus.Online;
            this.myStatus = this.globalUserCache.CurrentUser.UserStatus;
            
            this.friendListBox1.Initialize(this.globalUserCache.CurrentUser, this, null);

            this.rapidPassiveEngine.GroupOutter.GroupmateOffline += new CbGeneric<string>(GroupOutter_GroupmateOffline); //所有联系人的下线事件            
            
            //文件传送
            
            this.rapidPassiveEngine.ConnectionInterrupted += new CbGeneric(rapidPassiveEngine_ConnectionInterrupted);//预订断线事件
            this.rapidPassiveEngine.BasicOutter.BeingPushedOut += new CbGeneric(BasicOutter_BeingPushedOut);
            this.rapidPassiveEngine.RelogonCompleted += new CbGeneric<LogonResponse>(rapidPassiveEngine_RelogonCompleted);//预订重连成功事件  
            this.rapidPassiveEngine.MessageReceived += new CbGeneric<string, int, byte[], string>(rapidPassiveEngine_MessageReceived);
                       

            //网盘访问器 V2.0
            this.nDiskOutter = new NDiskOutter(this.rapidPassiveEngine.FileOutter, this.rapidPassiveEngine.CustomizeOutter);            
        }

        void GroupOutter_GroupmateOffline(string userID)
        {
            this.globalUserCache.ChangeUserStatus(userID, UserStatus.OffLine);
        }       

        void BasicOutter_BeingPushedOut()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric(this.BasicOutter_BeingPushedOut));
            }
            else
            {
                MessageBox.Show("이미 로그인되어있습니다.", GlobalResourceManager.SoftwareName);
            }
        }        

        void globalUserCache_FriendAdded(User friend)
        {
            this.friendListBox1.AddUser(friend);
        }

        void globalUserCache_FriendRemoved(string friendID)
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI<string>(this.do_globalUserCache_FriendRemoved, friendID);
        }

        private void do_globalUserCache_FriendRemoved(string friendID)
        {
            this.friendListBox1.RemoveUser(friendID);
        }

        void globalUserCache_GroupInfoChanged(Group group, GroupChangedType type, string userID)
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI<Group, GroupChangedType, string>(this.do_globalUserCache_GroupInfoChanged, group, type, userID);
        }

        void do_globalUserCache_GroupInfoChanged(Group group, GroupChangedType type, string userID)
        {
            
        }

        void globalUserCache_FriendRTDataRefreshCompleted()
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI(this.do_globalUserCache_FriendRTDataRefreshCompleted);
        }

        void do_globalUserCache_FriendRTDataRefreshCompleted()
        {
            ////请求离线消息 
            //this.rapidPassiveEngine.CustomizeOutter.Send(InformationTypes.GetOfflineMessage, null);
            ////请求离线文件
            //this.rapidPassiveEngine.CustomizeOutter.Send(InformationTypes.GetOfflineFile, null);

            //正式通知好友，自己上线
            this.rapidPassiveEngine.CustomizeOutter.Send(InformationTypes.ChangeStatus, BitConverter.GetBytes((int)this.globalUserCache.CurrentUser.UserStatus));

            User mine = this.globalUserCache.GetUser(this.rapidPassiveEngine.CurrentUserID);
            this.InitializeFinished();
        }

        private void InitializeFinished()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric(this.InitializeFinished));
            }
            else
            {
                this.friendListBox1.SortAllUser();
                this.Cursor = Cursors.Default;
            }
        }

        void globalUserCache_FriendStatusChanged(User friend)
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI<User>(this.do_globalUserCache_FriendStatusChanged, friend);            
        }

        private void do_globalUserCache_FriendStatusChanged(User friend)
        {
            this.friendListBox1.UserStatusChanged(friend);            
        }

        void globalUserCache_FriendInfoChanged(User user)
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI<User>(this.do_globalUserCache_FriendInfoChanged, user);
        }

        void do_globalUserCache_FriendInfoChanged(User user)
        {
            this.friendListBox1.UserInfoChanged(user);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    this.notifyIcon1.ContextMenu = this.contextMenu1;
                    this.notifyIcon1.Text = this.rapidPassiveEngine.CurrentUserID + "관리자";
                    this.notifyIcon1.Visible = true;
                }

                //我的好友
                foreach (string friendID in this.globalUserCache.CurrentUser.GetAllFriendList())
                {
                    if (friendID == this.globalUserCache.CurrentUser.UserID)
                    {
                        continue;
                    }

                    User friend = this.globalUserCache.GetUser(friendID);
                    if (friend != null)
                    {
                        this.friendListBox1.AddUser(friend);
                    }
                }
                this.friendListBox1.SortAllUser();
                this.friendListBox1.ExpandRoot();


                this.initialized = true;

                //this.checkBox1.Enabled = false;
                //this.comboBox1.Enabled = false;

                //this.gotoExit = true;

                //this.toolBarLogIn.Enabled = true;
                //this.toolBarNewReg.Enabled = true;
                //this.toolBarAddPC.Enabled = false;
                //this.toolBarUserInfo.Enabled = false;
                //this.toolBarChangePass.Enabled = false;
                //this.toolBarRemoteControl.Enabled = false;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if(globalUserCache != null)
                this.globalUserCache.StartRefreshFriendInfo();
            //InformationForm frm = new InformationForm(this.rapidPassiveEngine.CurrentUserID, this.globalUserCache.CurrentUser.Name, GlobalResourceManager.GetHeadImage(this.globalUserCache.CurrentUser));
            //frm.Show();
        }

        public bool IsFriend(string friendID)
        {
            return this.friendListBox1.ContainsUser(friendID);
        }

        void rapidPassiveEngine_RelogonCompleted(LogonResponse logonResponse)
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI<LogonResponse>(this.do_rapidPassiveEngine_RelogonCompleted, logonResponse);
        }

        void do_rapidPassiveEngine_RelogonCompleted(LogonResponse logonResponse)
        {
            if (logonResponse.LogonResult != LogonResult.Succeed)
            {
                //this.notifyIcon1.ChangeText(String.Format("{0}：{1}（{2}）\n状态：离线，请重新登录。", GlobalResourceManager.SoftwareName, this.globalUserCache.CurrentUser.Name, this.globalUserCache.CurrentUser.UserID));
                MessageBox.Show("자동로그인이 실패하였습니다. 프로그램을 다시 실행하여주세요.", GlobalResourceManager.SoftwareName);
                return;
            }

            this.globalUserCache.CurrentUser.UserStatus = this.myStatus;
            
            this.globalUserCache.StartRefreshFriendInfo();
            //this.notifyIcon1.ChangeText(String.Format("{0}：{1}（{2}）\n状态：{3}", GlobalResourceManager.SoftwareName, this.globalUserCache.CurrentUser.Name, this.globalUserCache.CurrentUser.UserID, GlobalResourceManager.GetUserStatusName(this.globalUserCache.CurrentUser.UserStatus)));
            //this.notifyIcon1.ChangeMyStatus(this.globalUserCache.CurrentUser.UserStatus);
        }

        void rapidPassiveEngine_ConnectionInterrupted()
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI(this.do_rapidPassiveEngine_ConnectionInterrupted);
        }

        void do_rapidPassiveEngine_ConnectionInterrupted()
        {
            this.globalUserCache.CurrentUser.UserStatus = UserStatus.OffLine;
            //this.skinButton_headImage.Image = GlobalResourceManager.GetHeadImage(this.globalUserCache.CurrentUser);
            //this.skinButton_State.Image = Properties.Resources.imoffline__2_;
            //this.skinButton_State.Tag = ChatListSubItem.UserStatus.OffLine;
            //this.skinButton_State.Enabled = false;
            //this.notifyIcon.ChangeText(String.Format("{0}：{1}（{2}）\n状态：离线，正在重连 . . .", GlobalResourceManager.SoftwareName, this.globalUserCache.CurrentUser.Name, this.globalUserCache.CurrentUser.UserID));
            //this.notifyIcon.ChangeMyStatus(UserStatus.OffLine);

            foreach (User friend in this.globalUserCache.GetAllUser())
            {
                friend.UserStatus = UserStatus.OffLine;
            }
            this.friendListBox1.SetAllUserOffline();            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                e.Cancel = false;
                return;
            }

            if (!SystemSettings.Singleton.ExitWhenCloseMainForm && !this.gotoExit)
            {
                this.Visible = false;
                e.Cancel = true;
                return;
            }

            this.Cursor = Cursors.WaitCursor;            

            SystemSettings.Singleton.MainFormSize = this.Size;
            SystemSettings.Singleton.MainFormLocation = this.Location;
            SystemSettings.Singleton.Save();


            this.Visible = false;
            this.notifyIcon1.Visible = false;

            if (this.initialized)
            {
                this.rapidPassiveEngine.Close();
                Program.MultimediaManager.Dispose();
            }

            this.Cursor = Cursors.Default;
        }

        private bool gotoExit = false;
        private void cxMenuItemExit_Click(object sender, EventArgs e)
        {
            this.gotoExit = true;
            this.Close();
        }

        private void cxMenuItemOpen_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.Focus();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.initialized)
            {
                return;
            }

            this.Visible = true;
            this.Focus();
        }

        public Image GetHeadImage(IUser user)
        {
            return GlobalResourceManager.GetHeadImage((User)user);
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button == toolBarButton1)         // 로그인버튼 클릭.
            {
                //ESPlus.GlobalUtil.SetMaxLengthOfUserID(20);
                //ESPlus.GlobalUtil.SetMaxLengthOfMessage(1024 * 1024 * 10);
                //OMCS.GlobalUtil.SetMaxLengthOfUserID(20);
                //MainForm mainForm = new MainForm();
                //IRapidPassiveEngine passiveEngine = RapidEngineFactory.CreatePassiveEngine();

                //NDiskPassiveHandler nDiskPassiveHandler = new NDiskPassiveHandler(); //V 2.0
                //ComplexCustomizeHandler complexHandler = new ComplexCustomizeHandler(nDiskPassiveHandler, this);//V 2.0                
                
                //using (LogIn f = new LogIn(passiveEngine, complexHandler))
                //{
                //    f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

                //    if (f.ShowDialog() != DialogResult.OK)
                //    {
                //        return;
                //    }

                //    //cls = false;

                //    notifyIcon1.Icon = this.Icon;
                //    notifyIcon1.Text = "SNEED원격제어";
                //    notifyIcon1.ContextMenu = this.contextMenu1;
                //    notifyIcon1.Visible = true;

                //    nDiskPassiveHandler.Initialize(passiveEngine.FileOutter, null);
                //    Initialize(passiveEngine);
                //    ShowUserInfos();

                //    this.initialized = true;

                //    this.gotoExit = false;

                //    this.toolBarLogIn.Enabled = false;
                //    this.toolBarNewReg.Enabled = true;
                //    this.toolBarAddPC.Enabled = true;
                //    this.toolBarUserInfo.Enabled = true;
                //    this.toolBarChangePass.Enabled = true;
                //    this.toolBarRemoteControl.Enabled = true;

                //    this.Cursor = Cursors.Default;
                //}
            }
            else if (e.Button == toolBarButton2)    // 새로등록버튼 클릭.
            {
                using (NewUser f = new NewUser())
                {
                    f.currentUser = globalUserCache.CurrentUser;
                    f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                    f.ShowDialog();
                }
            }
            else if (e.Button == toolBarButton7)    // 유저등록버튼 클릭.
            {
                using (AddUser f = new AddUser(this.rapidPassiveEngine, globalUserCache.CurrentUser))
                {
                    if (!this.rapidPassiveEngine.Connected)
                    {
                        return;
                    }

                    f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

                    if (f.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }


                    if (f.bAddList)
                    {
                        foreach (string strId in f._listFriend)
                        {
                            this.globalUserCache.CurrentUser.AddFriend(strId, f.CatalogName);
                            User user = this.globalUserCache.GetUser(strId);
                            this.friendListBox1.AddUser(user);
                        }
                    }
                    else
                    {
                        //byte[] bUser = this.rapidPassiveEngine.CustomizeOutter.Query(InformationTypes.GetUserInfo, System.Text.Encoding.UTF8.GetBytes(f.FriendID));
                        //User friend = (User)GGHelper.DeserializeBytes(bUser, 0, bUser.Length);
                        this.globalUserCache.CurrentUser.AddFriend(f.FriendID, f.CatalogName);
                        User user = this.globalUserCache.GetUser(f.FriendID);
                        this.friendListBox1.AddUser(user);
                    }
                    //ShowUserInfos();
                }

                //using (UserList f = new UserList())
                //{
                //    f.currentUser = this.globalUserCache.CurrentUser;
                //    f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                //    if (f.ShowDialog() != DialogResult.OK)
                //    {
                //        return;
                //    }

                //    foreach (string strId in f.listFriends)
                //    {
                //        this.globalUserCache.CurrentUser.AddFriend(strId, f.strCatalog);
                //        User user = this.globalUserCache.GetUser(strId);
                //        this.friendListBox1.AddUser(user);
                //    }
                //}
            }
            else if (e.Button == toolBarButton3)    // 원격제어버튼 클릭.
            {
                if (toolBarButton3.Text == "원격제어")
                {
                    if (this.friendListBox1._ChatListBox.SelectSubItem == null)
                    {
                        MessageBox.Show("원격제어를 하시려는 컴퓨터를 선택하세요.");
                        return;
                    }

                    IUser friend = (IUser)this.friendListBox1._ChatListBox.SelectSubItem.Tag;                    
                    if (friend.ID == this.globalUserCache.CurrentUser.ID)
                    {
                        return;
                    }

                    if(friend.UserStatus == UserStatus.OffLine)
                    {
                        MessageBox.Show("원격컴퓨터가 로그오프되어있습니다.");
                        return;
                    }

                    ShowRemotePC(friend);

                    //this.tabControl1.SelectedTab = this.tabPage1;

                    //Program.MultimediaManager.DesktopRegion = null;                    
                    //this.rapidPassiveEngine.P2PController.P2PConnectAsyn(friend.ID);

                    //ConnectRemotePC(friend);                    

                    //this.lblWait.Visible = true;
                    //this.nDiskBrowser1.Visible = true;
                    //this.nDiskBrowser1.Initialize(friend.ID, this.rapidPassiveEngine.FileOutter, this.nDiskOutter);

                    //this.toolBarButton3.Text = "접속해제";
                }
                else
                {
                    //this.desktopConnector1.Disconnect();
                    //this.desktopConnector1.Visible = false;
                    //this.desktopConnector1.WatchingOnly = false;

                    //this.nDiskBrowser1.Visible = false;

                    //this.lblWait.Visible = false;

                    //this.checkBox1.Enabled = false;
                    //this.comboBox1.Enabled = false;

                    //toolBarButton3.Text = "원격제어";
                }
            }
            else if (e.Button == toolBarButton4)    // 원격디스크 클릭.
            {
            }
            else if (e.Button == toolBarButton10)    // 유저정보변경.
            {
                if (this.globalUserCache.CurrentUser.UserStatus == UserStatus.OffLine)
                {
                    return;
                }

                this.UpdateMyInfo();
            }
            else if (e.Button == toolBarButton12)   // 비번변경버튼.
            {
                if (this.globalUserCache.CurrentUser.UserStatus == UserStatus.OffLine)
                {
                    return;
                }

                ChangePasswordForm form = new ChangePasswordForm(this.rapidPassiveEngine);
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                form.ShowDialog();
            }
        }

        private void UpdateMyInfo()
        {
            UpdateUserInfoForm form = new UpdateUserInfoForm(this.rapidPassiveEngine, this.globalUserCache, this.globalUserCache.CurrentUser);
            form.UserInfoChanged += new CbGeneric<User>(form_UserInfoChanged);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            form.ShowDialog();
        }

        void form_UserInfoChanged(User user)
        {
            //this.labelSignature.Text = this.globalUserCache.CurrentUser.Signature;
            //this.skinButton_headImage.Image = GlobalResourceManager.GetHeadImage(this.globalUserCache.CurrentUser);
            //this.labelName.Text = this.globalUserCache.CurrentUser.Name;
            this.globalUserCache.AddOrUpdateUser(this.globalUserCache.CurrentUser);

            //foreach (ChatForm chatForm in this.chatFormManager.GetAllForms())
            //{
            //    chatForm.OnMyInfoChanged(this.globalUserCache.CurrentUser);
            //}            
        }

        private void ConnectRemotePC(IUser destUser)
        {
            //if (this.desktopConnector1.Visible == true)
            //{
            //    this.desktopConnector1.Disconnect();
            //}
            //else
            //{
            //    this.desktopConnector1.ConnectEnded += new CbGeneric<ConnectResult>(desktopConnector1_ConnectEnded);
            //    this.desktopConnector1.Disconnected += new CbGeneric<ConnectorDisconnectedType>(desktopConnector1_Disconnected);
            //}
            
            //this.desktopConnector1.Visible = false;
            //this.desktopConnector1.WatchingOnly = false;

            //this.desktopConnector1.BeginConnect(destUser.ID);
        }

        void desktopConnector1_Disconnected(ConnectorDisconnectedType disconnectedType)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<ConnectorDisconnectedType>(this.desktopConnector1_Disconnected), disconnectedType);
            }
            else
            {
                if (disconnectedType == ConnectorDisconnectedType.GuestActiveDisconnect)
                {
                    return;
                }

                //MessageBox.Show(string.Format("현재 원격접속이 끊어졌습니다. 이유 {1}", disconnectedType));

                //this.desktopConnector1.Disconnect();
                //this.desktopConnector1.Visible = false;
                //this.desktopConnector1.WatchingOnly = false;

                //this.nDiskBrowser1.Visible = false;

                //this.lblWait.Visible = false;

                //toolBarButton3.Text = "원격제어";

                //this.Close();                
            }
        }

        void desktopConnector1_ConnectEnded(ConnectResult res)
        {
            //if (this.InvokeRequired)
            //{
            //    this.BeginInvoke(new CbGeneric<ConnectResult>(this.desktopConnector1_ConnectEnded), res);
            //}
            //else
            //{
            //    this.Cursor = Cursors.Default;
            //    if (res == ConnectResult.Succeed)
            //    {
            //        this.desktopConnector1.Visible = true;
            //        this.lblWait.Visible = false;

            //        this.checkBox1.Enabled = true;
            //        this.comboBox1.Enabled = true;

            //        int quality = this.desktopConnector1.GetVideoQuality();
            //        int index = (quality - 1) / 5;
            //        if (index < 0)
            //        {
            //            index = 0;
            //        }
            //        if (index > 3)
            //        {
            //            index = 3;
            //        }

            //        this.comboBox1.SelectedIndex = index;

            //        return;
            //    }

            //    MessageBox.Show(string.Format("원격접속 실패. 이유 {0}", res));
            //    //this.Close();
            //}
        }

        private void ShowUserInfos()
        {
            //foreach (string friendID in this.globalUserCache.CurrentUser.GetAllFriendList())
            //{
            //    if (friendID == this.globalUserCache.CurrentUser.UserID)
            //    {
            //        continue;
            //    }

            //    User friend = this.globalUserCache.GetUser(friendID);
            //    if (friend != null)
            //    {
            //        this.friendListBox1.AddUser(friend);
            //    }
            //}

            //this.friendListBox1.SortAllUser();
            //this.friendListBox1.ExpandRoot();

            //我的好友
            foreach (string friendID in this.globalUserCache.CurrentUser.GetAllFriendList())
            {
                if (friendID == this.globalUserCache.CurrentUser.UserID)
                {
                    continue;
                }

                User friend = this.globalUserCache.GetUser(friendID);
                if (friend != null)
                {
                    this.friendListBox1.AddUser(friend);
                }
            }
            this.friendListBox1.SortAllUser();
            this.friendListBox1.ExpandRoot();


            this.initialized = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int quality = this.comboBox1.SelectedIndex * 5 + 1;
            //this.desktopConnector1.ChangeOwnerDesktopEncodeQuality(quality);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //this.desktopConnector1.AdaptiveToViewerSize = this.checkBox1.Checked;
        }
        
        private void textBoxId_SkinTxt_TextChanged(object sender, EventArgs e)
        {
            string strID = this.textBoxId.SkinTxt.Text;
            if (strID.Trim() == string.Empty)
            {
                foreach (ChatListItem chatItem in this.friendListBox1._ChatListBox.Items)
                {
                    if (chatItem.Text == "검색결과")
                    {
                        friendListBox1._ChatListBox.Items.Remove(chatItem);
                        this.friendListBox1.ExpandRoot();
                        return;
                    }
                }
            }

            List<ChatListSubItem> listChatListSubItem = this.friendListBox1.SearchChatListSubItem(strID);
            //if (listChatListSubItem.Count == 0 || strID.Trim() == string.Empty)
            //    return;

            bool bExist = false;
            foreach (ChatListItem chatItem in this.friendListBox1._ChatListBox.Items)
            {
                chatItem.IsOpen = false;
                if (chatItem.Text == "검색결과")
                {
                    bExist = true;
                }
            }

            if (!bExist)
            {
                this.friendListBox1._ChatListBox.Items.Insert(0, new ChatListItem("검색결과"));
            }

            this.friendListBox1._ChatListBox.Items[0].SubItems.Clear();
            foreach (ChatListSubItem subItem in listChatListSubItem)
            {
                this.friendListBox1._ChatListBox.Items[0].SubItems.Add(subItem);
            }

            this.friendListBox1.ExpandRoot();
        }
    }
}
