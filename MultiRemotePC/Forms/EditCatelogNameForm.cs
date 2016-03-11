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
    public partial class EditCatelogNameForm : Form
    {
        //private IRapidPassiveEngine rapidPassiveEngine;
        private bool isNew = true;
        private string oldName;

        public EditCatelogNameForm(string _oldName)
        {
            InitializeComponent();
            this.isNew = false;
            this.oldName = _oldName;
            this.textBox1.Text = oldName;
            this.textBox1.Focus();
        }

        public EditCatelogNameForm()
            : this("")
        {
        }

        private string newName;
        public string NewName
        {
            get
            {
                return this.newName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.newName = this.textBox1.Text.Trim();
            if (string.IsNullOrEmpty(this.newName))
            {
                MessageBox.Show("그룹명이 비어있습니다.");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }


            if (this.newName == this.oldName)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                return;
            }



            if (this.newName.Contains(":") || this.newName.Contains(";"))
            {
                MessageBox.Show("그룹명에 기호가 들어갈수가 없습니다.");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
