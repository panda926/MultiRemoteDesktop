using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using ESPlus.Application.CustomizeInfo;
using ESBasic;
using ESBasic.Security;
using ESPlus.Rapid;
using ESBasic.ObjectManagement.Forms;
using ESPlus.Application.FileTransfering;
using ESPlus.FileTransceiver;
using ESPlus.Application.Basic;
using OMCS.Passive;
using System.Configuration;
using ESBasic.ObjectManagement.Managers;
using OMCS.Passive.MicroMessages;

using JustLib;
using JustLib.UnitViews;
using JustLib.NetworkDisk.Passive;
using JustLib.NetworkDisk;
using GG2014;

using Microsoft.Win32;

namespace Client
{
    public partial class LogIn : Form
    {
        private static bool cls = false;

        private IMultimediaManager multimediaManager = MultimediaManagerFactory.GetSingleton();
        private IRapidPassiveEngine rapidPassiveEngine;
        private ICustomizeHandler customizeHandler;
        private NDiskPassiveHandler nDiskPassiveHandler;

        private INDiskOutter nDiskOutter; // V2.0    

        public LogIn(IRapidPassiveEngine engine, ICustomizeHandler handler, NDiskPassiveHandler diskPassiveHandler)
        {
            this.rapidPassiveEngine = engine;
            this.customizeHandler = handler;
            this.nDiskPassiveHandler = diskPassiveHandler;

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
                string pwdMD5 = pwd;
                pwdMD5 = SecurityHelper.MD5String(pwd);

                //id = "CLIENT:" + id;
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
            }
            catch (Exception ee)
            {                
                return;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.button2.Enabled = true;
            }

            nDiskPassiveHandler.Initialize(rapidPassiveEngine.FileOutter, null);
            Initialize(rapidPassiveEngine);

            this.rapidPassiveEngine.CustomizeOutter.Send(InformationTypes.ChangeStatus, BitConverter.GetBytes((int)UserStatus.Online));

            cls = false;

            notifyIcon1.Icon = this.Icon;
            notifyIcon1.Text = "'" + id + "'사용자";
            notifyIcon1.ContextMenu = this.contextMenu1;
            notifyIcon1.Visible = true;


            string strID = string.Empty;
            string strPW = string.Empty;

            if (checkBox1.Checked)
            {
                strID = this.textBoxId.Text;
                strPW = this.textBoxPwd.Text;
            }

            ModifyAppConfigValue("userID", strID);
            ModifyAppConfigValue("userPW", strPW);
            
            this.Close();
        }

        public void Initialize(IRapidPassiveEngine engine)
        {
            this.rapidPassiveEngine = engine;            
            
            this.nDiskOutter = new NDiskOutter(this.rapidPassiveEngine.FileOutter, this.rapidPassiveEngine.CustomizeOutter);
            
            this.multimediaManager.SecurityLogEnabled = false;
            this.multimediaManager.CameraDeviceIndex = 0;
            this.multimediaManager.MicrophoneDeviceIndex = 0;
            this.multimediaManager.SpeakerIndex = 0;            
            this.multimediaManager.CameraVideoSize = new Size(640, 480);
            this.multimediaManager.Initialize(this.rapidPassiveEngine.CurrentUserID, "", ConfigurationManager.AppSettings["OmcsServerIP"], int.Parse(ConfigurationManager.AppSettings["OmcsServerPort"]));
        }

        private void cxMenuItemExit_Click(object sender, EventArgs e)
        {
            cls = true;
            this.Close();
        }

        private void LogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                e.Cancel = false;
            }
            else
            {
                if (cls)
                {
                    try
                    {
                        e.Cancel = false;
                    }
                    catch { }
                }
                else
                {
                    e.Cancel = true;
                    this.Visible = false;
                }
            }
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            string strID = string.Empty;
            string strPW = string.Empty;
            string strAutoRun = string.Empty;

            strID = ConfigurationManager.AppSettings["userID"];
            strPW = ConfigurationManager.AppSettings["userPW"];
            strAutoRun = ConfigurationManager.AppSettings["AutoRun"];

            if (strAutoRun == "true")
                this.checkBox2.Checked = true;

            if(strID != string.Empty)
            {                
                this.textBoxId.Text = strID;
                this.textBoxPwd.Text = strPW;

                this.checkBox1.Checked = true;                
                this.timer1.Enabled = true;
            }

            cls = true;
        }

        /// <summary>
        /// 자동실행버튼 클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            RunWhenStart(this.checkBox2.Checked, "SNEED", Application.ExecutablePath);
        }

        public void RunWhenStart(bool started, string name, string path)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            try
            {
                RegistryKey run = HKLM.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\");                
                if (started)
                {
                    run.SetValue(name, path);
                    ModifyAppConfigValue("AutoRun", "true");
                }
                else
                {
                    run.DeleteValue(name);
                    ModifyAppConfigValue("AutoRun", "false");
                }
            }
            finally
            {
                HKLM.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string strID = string.Empty;
            string strPW = string.Empty;

            if (checkBox1.Checked)
            {
                strID = this.textBoxId.Text;
                strPW = this.textBoxPwd.Text;
            }

            ModifyAppConfigValue("userID", strID);
            ModifyAppConfigValue("userPW", strPW);
        }

        private void ModifyAppConfigValue(string strKey, string strValue)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = @"Client.exe.config";

            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            config.AppSettings.Settings[strKey].Value = strValue;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            button2_Click(null, null);            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
