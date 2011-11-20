using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Threading;

namespace Minecraft_Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Properties.Load();
            if (!Properties.console)
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Window());
                }
                catch (System.FormatException e) { MessageBox.Show(e.ToString()); Server.ErrorLog(e); }
                catch (Exception e){MessageBox.Show(e.ToString());}
            }
            else
                new Server(null);
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Server.ErrorLog(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Server.ErrorLog((Exception)e.ExceptionObject);
        }
    }
}
