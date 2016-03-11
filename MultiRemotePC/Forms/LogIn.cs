using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESPlus.Rapid;
using ESPlus.Application.Basic;
using ESPlus.Application.CustomizeInfo;
using System.Configuration;
using ESBasic.Security;

using GG2014;

namespace MultiRemotePC
{
    public partial class LogIn : Form
    {
        private IRapidPassiveEngine rapidPassiveEngine;
        private ICustomizeHandler customizeHandler;

        private string pwdMD5; 

        public LogIn(IRapidPassiveEngine engine, ICustomizeHandler handler)
        {
            this.rapidPassiveEngine = engine;
            this.customizeHandler = handler;
            InitializeComponent();
        }

        // 로그인버튼 클릭사건.
        private void button2_Click(object sender, EventArgs e)
        {
            string id = this.textBoxId.Text;
            string pwd = this.textBoxPwd.Text;
            if (id.Length == 0) { return; }

            try
            {
                this.rapidPassiveEngine.SecurityLogEnabled = false;
                
                if (!this.pwdIsMD5)
                {
                    pwdMD5 = SecurityHelper.MD5String(pwd);                    
                }
                
                LogonResponse response = this.rapidPassiveEngine.Initialize(id, pwdMD5, ConfigurationManager.AppSettings["ServerIP"], int.Parse(ConfigurationManager.AppSettings["ServerPort"]), this.customizeHandler);
                if (response.LogonResult == LogonResult.Failed)
                {
                    MessageBox.Show("아이디나 패스워드가 틀립니다.");
                    return;
                }

                if (response.LogonResult == LogonResult.HadLoggedOn)
                {
                    MessageBox.Show("현재 계정이 이미 로그인되어있습니다.");
                    return;
                }

                SystemSettings.Singleton.LastLoginUserID = id;
                //SystemSettings.Singleton.RememberPwd = this.checkBoxRememberPwd.Checked;
                SystemSettings.Singleton.LastLoginPwdMD5 = pwdMD5;
                //SystemSettings.Singleton.AutoLogin = this.checkBoxAutoLogin.Checked;
                SystemSettings.Singleton.Save();
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.button2.Enabled = true;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private bool pwdIsMD5 = false;
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewUser registerForm = new NewUser();
            DialogResult res = registerForm.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("관리자등록성공! 등록하신 아이디로 자동로그인이 됩니다.", "메시지");
                this.textBoxId.Text = registerForm.RegisteredUser.UserID;
                this.textBoxPwd.Text = "11111111";
                this.pwdMD5 = registerForm.RegisteredUser.PasswordMD5;
                this.pwdIsMD5 = true;
                this.button2.PerformClick();
            }
        }
    }
}
