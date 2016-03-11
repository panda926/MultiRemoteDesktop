using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GG2014;
using OMCS.Passive;
using ESBasic;
using JustLib;
using JustLib.NetworkDisk.Passive;
using ESPlus.Rapid;

namespace MultiRemotePC
{
    public partial class RemoteControl : Form
    {
        public ESPlus.Application.FileTransfering.Passive.IFileOutter fileOuter = null;
        public INDiskOutter nDiskOutter; // V2.0    
        public string strUserID;
        public IRapidPassiveEngine rapidPassiveEngine;

        public RemoteControl()
        {
            InitializeComponent();
        }

        public Panel panel
        {
            get { return this.panel1; }
        }

        private void RemoteControl_Load(object sender, EventArgs e)
        {
            this.rapidPassiveEngine.P2PController.P2PConnectAsyn(strUserID);
            this.nDiskBrowser1.Initialize(strUserID, this.fileOuter, this.nDiskOutter);
        }

        private void RemoteControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.nDiskBrowser1.fileTransferViewer.FileTransferingViewer_Disposed();
        }
    }
}
