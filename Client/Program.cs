using System;
using System.Collections.Generic;
using System.Windows.Forms;

using ESPlus.Rapid;
using JustLib.NetworkDisk.Passive;
using ESPlus.Application.CustomizeInfo;

using GG2014;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ESPlus.GlobalUtil.SetMaxLengthOfMessage(10 * 1024 * 1024);
            ESPlus.GlobalUtil.SetMaxLengthOfUserID(20);
            OMCS.GlobalUtil.SetMaxLengthOfUserID(20);
            IRapidPassiveEngine passiveEngine = RapidEngineFactory.CreatePassiveEngine();

            NDiskPassiveHandler nDiskPassiveHandler = new NDiskPassiveHandler(); //V 2.0
            ComplexCustomizeHandler complexHandler = new ComplexCustomizeHandler(nDiskPassiveHandler);//V 2.0

            Application.Run(new LogIn(passiveEngine, complexHandler, nDiskPassiveHandler));
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
