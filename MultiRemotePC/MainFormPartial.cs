using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GG2014;
using JustLib;
using JustLib.Controls;
using JustLib.Records;
using ESBasic;
using ESPlus.Serialization;
using System.Runtime.InteropServices;

namespace MultiRemotePC
{
    public partial class MainForm : ESPlus.Application.CustomizeInfo.IIntegratedCustomizeHandler
    {
        void rapidPassiveEngine_MessageReceived(string sourceUserID, int informationType, byte[] info, string tag)
        {
            if (!this.initialized)
            {
                return;
            }

            //if (informationType == InformationTypes.Chat)
            //{
            //    sourceUserID = tag;
            //    byte[] bChatBoxContent = info;
            //    if (bChatBoxContent != null)
            //    {
            //        ChatMessageRecord record = new ChatMessageRecord(sourceUserID, this.rapidPassiveEngine.CurrentUserID, bChatBoxContent, false);
            //        GlobalResourceManager.ChatMessageRecordPersister.InsertChatMessageRecord(record);
            //    }

            //    ChatBoxContent content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(info, 0);
            //    User user = this.globalUserCache.GetUser(sourceUserID);
            //    this.notifyIcon.PushFriendMessage(sourceUserID, informationType, info, content);
            //    return;
            //}
        }

        public void HandleInformation(string sourceUserID, int informationType, byte[] info)
        {
            if (!this.initialized)
            {
                return;
            }

            #region 需要twinkle的消息
            if (informationType == InformationTypes.Chat || informationType == InformationTypes.OfflineMessage || informationType == InformationTypes.OfflineFileResultNotify
                         || informationType == InformationTypes.Vibration || informationType == InformationTypes.VideoRequest || informationType == InformationTypes.AgreeVideo
                         || informationType == InformationTypes.RejectVideo || informationType == InformationTypes.HungUpVideo || informationType == InformationTypes.DiskRequest
                         || informationType == InformationTypes.AgreeDisk || informationType == InformationTypes.RejectDisk || informationType == InformationTypes.RemoteHelpRequest
                         || informationType == InformationTypes.AgreeRemoteHelp || informationType == InformationTypes.RejectRemoteHelp || informationType == InformationTypes.CloseRemoteHelp
                         || informationType == InformationTypes.TerminateRemoteHelp
                         || informationType == InformationTypes.AudioRequest || informationType == InformationTypes.RejectAudio || informationType == InformationTypes.AgreeAudio
                         || informationType == InformationTypes.HungupAudio
                         || informationType == InformationTypes.FriendAddedNotify)
            {
                if (informationType == InformationTypes.FriendAddedNotify)
                {
                    User owner = CompactPropertySerializer.Default.Deserialize<User>(info, 0); // 0922
                    this.globalUserCache.CurrentUser.AddFriend(owner.ID, this.globalUserCache.CurrentUser.DefaultFriendCatalog);
                    this.globalUserCache.OnFriendAdded(owner); //自然会添加 好友条目
                    sourceUserID = owner.UserID;
                }

                object tag = null;
                if (informationType == InformationTypes.OfflineMessage)
                {
                    byte[] bChatBoxContent = null;
                    OfflineMessage msg = CompactPropertySerializer.Default.Deserialize<OfflineMessage>(info, 0);
                    if (msg.InformationType == InformationTypes.Chat) //目前只处理离线的聊天消息
                    {
                        bChatBoxContent = msg.Information;
                        sourceUserID = msg.SourceUserID;
                        ChatMessageRecord record = new ChatMessageRecord(sourceUserID, this.rapidPassiveEngine.CurrentUserID, bChatBoxContent, false);
                        GlobalResourceManager.ChatMessageRecordPersister.InsertChatMessageRecord(record);
                        ChatBoxContent content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(bChatBoxContent, 0);
                        tag = new Parameter<ChatBoxContent, DateTime>(content, msg.Time);
                    }
                }

                if (informationType == InformationTypes.OfflineFileResultNotify)
                {
                    OfflineFileResultNotifyContract contract = CompactPropertySerializer.Default.Deserialize<OfflineFileResultNotifyContract>(info, 0);
                    sourceUserID = contract.AccepterID;
                }

                User user = this.globalUserCache.GetUser(sourceUserID);
                //this.notifyIcon.PushFriendMessage(sourceUserID, informationType, info, tag);
                return;
            }
            #endregion

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string, int, byte[]>(this.HandleInformation), sourceUserID, informationType, info);
            }
            else
            {
                try
                {
                    if (informationType == InformationTypes.InputingNotify)
                    {
                        //ChatForm form = this.chatFormManager.GetForm(sourceUserID);
                        //if (form != null)
                        //{
                        //    form.OnInptingNotify();
                        //}
                        return;
                    }

                    if (informationType == InformationTypes.FriendRemovedNotify)
                    {
                        string friendID = System.Text.Encoding.UTF8.GetString(info);
                        this.globalUserCache.RemovedFriend(friendID);
                        return;
                    }

                    if (informationType == InformationTypes.UserInforChanged)
                    {
                        User user = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<User>(info, 0);
                        this.globalUserCache.AddOrUpdateUser(user);
                        return;
                    }

                    if (informationType == InformationTypes.UserStatusChanged)
                    {
                        UserStatusChangedContract contract = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<UserStatusChangedContract>(info, 0);
                        this.globalUserCache.ChangeUserStatus(contract.UserID, (UserStatus)contract.NewStatus);
                    }
                }
                catch (Exception ee)
                {
                    GlobalResourceManager.Logger.Log(ee, "MainForm.HandleInformation", ESBasic.Loggers.ErrorLevel.Standard);
                    MessageBox.Show(ee.Message);
                }
            }
        }

        public byte[] HandleQuery(string sourceUserID, int informationType, byte[] info)
        {
            return null;
        }

        public bool CanHandle(int informationType)
        {
            return InformationTypes.ContainsInformationType(informationType);
        }
    }
}
