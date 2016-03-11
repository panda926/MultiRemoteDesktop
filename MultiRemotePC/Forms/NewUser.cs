using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Configuration;
using GG2014;
using ESBasic.Security;

namespace MultiRemotePC
{
    public partial class NewUser : Form
    {
        private IRemotingService ggService;
        public User currentUser = null;
        //public bool bAdminRegister = false;

        public NewUser()
        {
            InitializeComponent();

            int registerPort = int.Parse(ConfigurationManager.AppSettings["RemotingPort"]);
            this.ggService = (IRemotingService)Activator.GetObject(typeof(IRemotingService), string.Format("tcp://{0}:{1}/RemotingService", ConfigurationManager.AppSettings["ServerIP"], registerPort));
        }

        #region RegisteredUser
        private User registeredUser = null;
        public User RegisteredUser
        {
            get
            {
                return this.registeredUser;
            }
        }
        #endregion    

        /// <summary>
        /// 등록버튼 클릭사건.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string userID = this.textBox1.Text.Trim();
            if (userID.Length == 0)
            {
                this.textBox1.Focus();
                MessageBox.Show("아이디를 입력하세요.");
                return;
            }

            string pwd = this.textBox2.Text;
            if (pwd != this.textBox2.Text)
            {
                MessageBox.Show("패스워드가 일치하지 않습니다.");
                this.textBox2.SelectAll();
                this.textBox2.Focus();
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            try
            {
                //string strSignature = string.Empty;
                //if (bAdminRegister)
                //    strSignature = "ADMIN";
                //else
                //    strSignature = "CLIENT";

                string strCreator = "";
                if (currentUser != null)
                    strCreator = currentUser.UserID;

                User user = new User(userID, SecurityHelper.MD5String(pwd), this.textBox4.Text, string.Empty, string.Empty, 0, "", strCreator);

                RegisterResult result = ggService.Register(user);
                if (result == RegisterResult.Existed)
                {
                    this.textBox1.SelectAll();
                    this.textBox1.Focus();
                    MessageBox.Show("아이디가 이미 존재하고있습니다.");
                    return;
                }

                if (result == RegisterResult.Error)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    MessageBox.Show("등록과정에 오류가 발생하였습니다.");
                    return;
                }

                this.registeredUser = user;  
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ee)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                MessageBox.Show("등록오류: " + ee.ToString());
            }

             

            //try
            //{
            //    int registerPort = int.Parse(ConfigurationManager.AppSettings["RemotingPort"]);
            //    IRemotingService registerService = (IRemotingService)Activator.GetObject(typeof(IRemotingService), string.Format("tcp://{0}:{1}/RegisterService", ConfigurationManager.AppSettings["ServerIP"], registerPort)); ;
            //    User user = new User(userID, SecurityHelper.MD5String(this.textBox2.Text), string.Empty, string.Empty, string.Empty, 0, "");

            //    RegisterResult result = registerService.Register(user);
            //    if (result == RegisterResult.Existed)
            //    {
            //        MessageBox.Show("아이디가 이미 존재하고있습니다.");
            //        return;
            //    }

            //    if (result == RegisterResult.Error)
            //    {
            //        MessageBox.Show("아이디등록 실패.");
            //        return;
            //    }

            //    this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //}
            //catch (Exception ee)
            //{
            //    MessageBox.Show("계정등록 실패." + ee.Message);
            //}
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExcelReg frmExcelLog = new ExcelReg();
            frmExcelLog.currentUser = this.currentUser;
            frmExcelLog.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

            frmExcelLog.ShowDialog();
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
