using CrashReporterDotNET;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AzureKinectRecorder
{
    static class Program
    {
        private static ReportCrash _reportCrash;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += (sender, args) =>
            {
                GlobalThreadExceptionHandler(sender, args);
                SendReport(args.Exception);
            };
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                GlobalUnhandledExceptionHandler(sender, args);
                SendReport((Exception)args.ExceptionObject);
            };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _reportCrash = new ReportCrash("tristan_wyl@hotmail.com")
            {
                Silent = true,
                CaptureScreen = true,
                IncludeScreenshot = true,
                #region Optional Configuration
                // WebProxy = new WebProxy("Web proxy address, if needed"),
                //AnalyzeWithDoctorDump = true,
                //DoctorDumpSettings = new DoctorDumpSettings
                //{
                //    ApplicationID = new Guid("AzureKinectRecorder"),
                //    OpenReportInBrowser = true
                //}
                #endregion
            };
            //_reportCrash.RetryFailedReports();
            Application.Run(new Control());
        }

        public static void SendReport(Exception exception, string developerMessage = "")
        {
            _reportCrash.DeveloperMessage = developerMessage;
            _reportCrash.Silent = false;
            _reportCrash.Send(exception);
        }

        public static void SendReportSilently(Exception exception, string developerMessage = "")
        {
            _reportCrash.DeveloperMessage = developerMessage;
            _reportCrash.Silent = true;
            _reportCrash.Send(exception);
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            ILog log = LogManager.GetLogger(typeof(Program));
            log.Error(ex.Message + "\n" + ex.StackTrace);
        }

        private static void GlobalThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            ILog log = LogManager.GetLogger(typeof(Program)); //Log4NET
            log.Error(ex.Message + "\n" + ex.StackTrace);
        }
    }
}
