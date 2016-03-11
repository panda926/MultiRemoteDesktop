using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using GG2014;
using JustLib;
using JustLib.NetworkDisk.Passive;
using OMCS.Passive;
using ESBasic;
using ESPlus.Application.FileTransfering;
using ESPlus.Rapid;
using ESBasic.ObjectManagement.Forms;

namespace MultiRemotePC
{
    public partial class RemotePC : UserControl
    {
        public event CbGeneric<string> RemoteHelpEnded;
        private string strUserID = string.Empty;
        private RemoteControl frmRemoteControl = null;
        public ESPlus.Application.FileTransfering.Passive.IFileOutter fileOuter = null;
        public INDiskOutter nDiskOutter; // V2.0    
        public IRapidPassiveEngine rapidPassiveEngine;

        public RemotePC(string ownerID)
        {
            InitializeComponent();

            this.RemoteHelpEnded += delegate { };

            //this.frmRemoteControl = new RemoteControl(this.desktopConnector1);

            this.lblWait.Visible = true;
            this.desktopConnector1.Visible = false;
            this.desktopConnector1.WatchingOnly = false; //可以操控桌面
            this.desktopConnector1.ConnectEnded += new CbGeneric<ConnectResult>(desktopConnector1_ConnectEnded);
            this.desktopConnector1.Disconnected += new CbGeneric<ConnectorDisconnectedType>(desktopConnector1_Disconnected);

            this.desktopConnector1.BeginConnect(ownerID);
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

                this.button1_Click(null, null);
                //MessageBox.Show(string.Format("到{0}的桌面连接断开。原因：{1}", this.ownerName, disconnectedType));
                //this.Close();
            }
        }

        void desktopConnector1_ConnectEnded(ConnectResult res)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<ConnectResult>(this.desktopConnector1_ConnectEnded), res);
            }
            else
            {
                this.Cursor = Cursors.Default;
                if (res == ConnectResult.Succeed)
                {
                    this.lblWait.Visible = false;
                    this.desktopConnector1.Visible = true;
                    //this.skinComboBox_quality.Visible = true;
                    //this.skinLabel_quality.Visible = true;

                    int quality = this.desktopConnector1.GetVideoQuality();
                    int index = (quality - 1) / 5;
                    if (index < 0)
                    {
                        index = 0;
                    }
                    if (index > 3)
                    {
                        index = 3;
                    }

                    this.comboBox1.SelectedIndex = index;

                    return;
                }

                //MessageBoxEx.Show(string.Format("连接{0}的桌面失败。原因：{1}", this.ownerName, res));
                //this.Close();
            }
        }

        public string _strUserID
        {
            get
            {
                return strUserID;
            }

            set
            {
                strUserID = value;
            }
        }

        // 클로즈버튼 클릭사건.
        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.desktopConnector1.Connected)
            {
                if (frmRemoteControl != null)
                {
                    this.frmRemoteControl.Close();
                    this.frmRemoteControl = null;
                }

                this.RemoteHelpEnded(this.strUserID);

                return;
            }

            this.desktopConnector1.Disconnect();

            if (frmRemoteControl != null)
            {
                this.frmRemoteControl.Close();
                this.frmRemoteControl = null;
            }

            this.RemoteHelpEnded(this.strUserID);
        }

        private void RemoteControl_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.frmRemoteControl = null;
        }

        // 화면확대버튼 클릭사건.
        private void button2_Click(object sender, EventArgs e)
        {
            if (frmRemoteControl == null)
            {
                frmRemoteControl = new RemoteControl();
                frmRemoteControl.Name = this.strUserID;

                this.button1.Visible = false;
                this.button2.Visible = false;
                this.label2.Visible = false;

                frmRemoteControl.fileOuter = this.fileOuter;
                frmRemoteControl.nDiskOutter = this.nDiskOutter;
                frmRemoteControl.strUserID = this.strUserID;
                frmRemoteControl.rapidPassiveEngine = this.rapidPassiveEngine;

                frmRemoteControl.panel.Controls.Add(this);
                frmRemoteControl.FormClosed += new FormClosedEventHandler(RemoteControl_FormClosed);                
                frmRemoteControl.Text = "'" + this.strUserID + "' 원격제어";
                frmRemoteControl.Show();
            }
            else
            {
                frmRemoteControl.Activate();
            }
        }

        // 꽉찬화면체크박스 클릭사건.
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.desktopConnector1.AdaptiveToViewerSize = this.checkBox1.Checked;
        }

        // 화질선택콤보박스 체인지사건.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int quality = this.comboBox1.SelectedIndex * 5 + 1;
            this.desktopConnector1.ChangeOwnerDesktopEncodeQuality(quality);
        }

        private void RemotePC_Load(object sender, EventArgs e)
        {
            this.label2.Text = "'" + this.strUserID + "'" + "원격제어";
            this.lblWait.Location = new Point((this.Width - this.lblWait.Width) / 2, this.lblWait.Location.Y);
        }
    }
}
