using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Configuration;
using ESPlus.Rapid;
using ESPlus.Serialization;
using GG2014;

namespace MultiRemotePC
{
    public partial class ChangePasswordForm : Form
    {
        private IRapidPassiveEngine rapidPassiveEngine;

        public ChangePasswordForm(IRapidPassiveEngine engine)
        {
            InitializeComponent();
            this.rapidPassiveEngine = engine;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox2.Text != this.textBox3.Text)
            {
                MessageBox.Show("패스워드가 일치하지 않습니다.");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            try
            {
                ChangePasswordContract contract = new ChangePasswordContract(ESBasic.Security.SecurityHelper.MD5String(this.textBox1.Text.Trim()), ESBasic.Security.SecurityHelper.MD5String(this.textBox2.Text));
                byte[] bRes = this.rapidPassiveEngine.CustomizeOutter.Query(InformationTypes.ChangePassword, CompactPropertySerializer.Default.Serialize(contract));
                ChangePasswordResult res = (ChangePasswordResult)BitConverter.ToInt32(bRes, 0);
                if (res == ChangePasswordResult.OldPasswordWrong)
                {
                    MessageBox.Show("이전 패스워드가 정확치 않습니다.");
                    this.textBox1.Focus();
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }

                if (res == ChangePasswordResult.UserNotExist)
                {
                    MessageBox.Show("사용자가 존재하지 않습니다.");
                    this.textBox1.Focus();
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }

                //MessageBox.Show("패스워드 변경완료.");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ee)
            {
                MessageBox.Show("패스워드 변경실패." + ee.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
