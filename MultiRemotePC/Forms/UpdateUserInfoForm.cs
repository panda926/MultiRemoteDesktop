using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.Configuration;
using ESBasic.Security;
using ESPlus.Rapid;
using ESBasic;

using ESPlus.Application.CustomizeInfo;
using ESPlus.Application;
using GG2014;

namespace MultiRemotePC
{
    public partial class UpdateUserInfoForm : Form
    {
        private int headImageIndex = 0;
        private IRemotingService ggService;
        private User currentUser;
        private IRapidPassiveEngine rapidPassiveEngine;
        public event CbGeneric<User> UserInfoChanged;

        public UpdateUserInfoForm(IRapidPassiveEngine engine, GlobalUserCache globalUserCache, User user)
        {
            InitializeComponent();

            this.rapidPassiveEngine = engine;
            this.currentUser = user;
            int registerPort = int.Parse(ConfigurationManager.AppSettings["RemotingPort"]);
            this.ggService = (IRemotingService)Activator.GetObject(typeof(IRemotingService), string.Format("tcp://{0}:{1}/RemotingService", ConfigurationManager.AppSettings["ServerIP"], registerPort)); ;

            this.textBox1.Text = user.UserID;
            this.textBox4.Text = user.Name;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //0923
                if (!this.rapidPassiveEngine.Connected)
                {                    
                    MessageBox.Show("오프라인 상태입니다. 변경을 할수 없습니다.");
                    return;
                }
                
                this.currentUser.Name = this.textBox4.Text;
                
                //0923
                this.Cursor = Cursors.WaitCursor;
                UIResultHandler handler = new UIResultHandler(this, this.UpdateCallback);
                byte[] data = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(this.currentUser);
                //回复异步调用，避免阻塞UI线程
                this.rapidPassiveEngine.SendMessage(null, InformationTypes.UpdateUserInfo, data, null, 2048, handler.Create(), null); //0924               
            }
            catch (Exception)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("변경실패.");
            }
        }

        private void UpdateCallback(bool acked, object tag)
        {
            this.Cursor = Cursors.Default;
            if (acked)
            {
                //MessageBox.Show("변경완료.");
                if (this.UserInfoChanged != null)
                {
                    this.UserInfoChanged(this.currentUser);
                }
                this.Close();
            }
            else
            {                
                MessageBox.Show("변경실패.");
            }
        }
    }
}
