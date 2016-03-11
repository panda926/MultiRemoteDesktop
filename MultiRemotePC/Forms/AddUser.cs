using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GG2014;
using System.Configuration;
using ESPlus.Rapid;

namespace MultiRemotePC
{
    public partial class AddUser : Form
    {
        private IRapidPassiveEngine rapidPassiveEngine;
        private IRemotingService ggService;
        private User curUser;

        public bool bAddList = false;

        public AddUser(IRapidPassiveEngine engine, User currentUser)
        {
            InitializeComponent();
            this.comboBox1.DataSource = currentUser.GetFriendCatalogList();
            this.comboBox1.SelectedIndex = 0;
            this.rapidPassiveEngine = engine;

            this.curUser = currentUser;

            int registerPort = int.Parse(ConfigurationManager.AppSettings["RemotingPort"]);
            this.ggService = (IRemotingService)Activator.GetObject(typeof(IRemotingService), string.Format("tcp://{0}:{1}/RemotingService", ConfigurationManager.AppSettings["ServerIP"], registerPort));
        }

        #region FriendID
        private string friendID = "";
        public string FriendID
        {
            get
            {
                return this.friendID;
            }
        }
        #endregion

        #region CatalogName
        private string catalogName = "";
        public string CatalogName
        {
            get { return catalogName; }
        }
        #endregion

        #region List of FriendID
        private List<string> listFriend = new List<string>();
        public List<string> _listFriend
        {
            get { return this.listFriend; }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            this.friendID = this.textBox1.Text.Trim();
            if (this.friendID.Length == 0)
            {
                MessageBox.Show("아이디가 비어있습니다.");
                return;
            }

            string strPassword = this.textBox2.Text;
            if (strPassword.Length == 0)
            {
                MessageBox.Show("패스워드를 입력하세요.");
                return;
            }

            try
            {
                this.catalogName = this.comboBox1.SelectedItem.ToString();
                //AddFriendContract contract = new AddFriendContract(this.friendID, this.catalogName);
                //byte[] info = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
                //byte[] bRes = this.rapidPassiveEngine.CustomizeOutter.Query(InformationTypes.AddFriend, info);
                //AddFriendResult res = (AddFriendResult)BitConverter.ToInt32(bRes, 0);
                //if (res == AddFriendResult.FriendNotExist)
                //{
                //    MessageBox.Show("아이디가 존재하지 않습니다.");
                //    this.DialogResult = System.Windows.Forms.DialogResult.None;
                //    return;
                //}

                int nResult = this.ggService.AddFriend(curUser.UserID, this.friendID, ESBasic.Security.SecurityHelper.MD5String(strPassword), this.catalogName);
                if (nResult == 1)
                {
                    MessageBox.Show("아이디가 존재하지 않습니다.");
                    return;
                }
                else if (nResult == 2)
                {
                    MessageBox.Show("패스워드가 일치하지 않습니다.");
                    return;
                }
                else if (nResult == 3)
                {
                    MessageBox.Show("오류가 발생하였습니다.");
                    return;
                }


                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ee)
            {
                MessageBox.Show("친구추가 실패" + ee.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserList userList = new UserList();
            userList.currentUser = this.curUser;
            userList.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            if (userList.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.bAddList = true;
                this.listFriend = userList.listFriends;
                this.catalogName = userList.strCatalog;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }            
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (UserListPerManager f = new UserListPerManager())
            {
                f.currentUser = this.curUser;
                f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                this.bAddList = true;
                this.catalogName = f.strCatalog;
                this.listFriend = f.listFriends;

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
    }
}
