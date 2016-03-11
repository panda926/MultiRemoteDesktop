using System;
using System.Collections.Generic;
using System.Windows.Forms;

using ESPlus.Rapid;
using ESPlus.Application.CustomizeInfo;
using CCWin;
using System.Drawing;
using JustLib.NetworkDisk.Passive;
using OMCS.Passive;
using System.Configuration;

using GG2014;

namespace MultiRemotePC
{
    static class Program
    {
        public static IMultimediaManager MultimediaManager;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //try
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                GlobalResourceManager.Initialize();

                ESPlus.GlobalUtil.SetMaxLengthOfUserID(20);
                ESPlus.GlobalUtil.SetMaxLengthOfMessage(1024 * 1024 * 10);
                OMCS.GlobalUtil.SetMaxLengthOfUserID(20);
                MainForm mainForm = new MainForm();
                IRapidPassiveEngine passiveEngine = RapidEngineFactory.CreatePassiveEngine();                

                NDiskPassiveHandler nDiskPassiveHandler = new NDiskPassiveHandler(); //V 2.0
                ComplexCustomizeHandler complexHandler = new ComplexCustomizeHandler(nDiskPassiveHandler, mainForm);//V 2.0
                LogIn loginForm = new LogIn(passiveEngine, complexHandler);
                loginForm.FormBorderStyle = FormBorderStyle.FixedDialog;

                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                #region 初始化OMCS
                Program.MultimediaManager = MultimediaManagerFactory.GetSingleton();
                Program.MultimediaManager.ChannelMode = ChannelMode.P2PChannelFirst;
                Program.MultimediaManager.CameraDeviceIndex = SystemSettings.Singleton.WebcamIndex;
                Program.MultimediaManager.MicrophoneDeviceIndex = SystemSettings.Singleton.MicrophoneIndex;
                Size? okSize = OMCS.Tools.Camera.MatchCameraVideoSize(SystemSettings.Singleton.WebcamIndex, GlobalConsts.CommonQualityVideo);
                Program.MultimediaManager.CameraVideoSize = okSize == null ? new Size(320, 240) : okSize.Value;
                Program.MultimediaManager.OmcsLogger = GlobalResourceManager.Logger;
                Program.MultimediaManager.Initialize(passiveEngine.CurrentUserID, "", ConfigurationManager.AppSettings["ServerIP"], int.Parse(ConfigurationManager.AppSettings["OmcsServerPort"]));
                #endregion

                nDiskPassiveHandler.Initialize(passiveEngine.FileOutter, null);
                mainForm.Initialize(passiveEngine);
                Application.Run(mainForm);
            }
            //catch (Exception ee)
            //{
            //    MessageBoxEx.Show(ee.ToString());
            //}
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //try
            //{
            //    Exception ex = (Exception)e.ExceptionObject;
            //    DevExpress.XtraEditors.XtraMessageBox.Show(string.Format("프로그램에 오류가 발생하여 죄송합니다. 프로그래머에게 문의하세요. \r\n\r\n{0}", ex.Message + ex.StackTrace), "오류", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //}
            //finally
            //{
            //    Application.Exit();
            //}
        }

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //DialogResult result = DialogResult.Abort;
            //try
            //{
            //    result = DevExpress.XtraEditors.XtraMessageBox.Show(string.Format("프로그램에 오류가 발생하여 죄송합니다. 프로그래머에게 문의하세요. \r\n\r\n{0}", e.Exception.Message + e.Exception.StackTrace), "오류", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
            //}
            //finally
            //{
            //    if (result == DialogResult.Abort)
            //    {
            //        Application.Exit();
            //    }
            //}
        }
    }
}
