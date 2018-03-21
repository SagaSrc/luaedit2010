using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LuaEdit
{
    static class Program
    {
        private static SplashScreen _splashScreenDlg;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _splashScreenDlg = new SplashScreen();
            _splashScreenDlg.Show();
            Application.DoEvents();
            MainForm mainDlg = new MainForm();
            AppDomain.CurrentDomain.AssemblyLoad -= OnAssemblyLoad;
            _splashScreenDlg.Close();
            Application.Run(mainDlg);
        }

        private static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs e)
        {
            _splashScreenDlg.LoadingMsg = string.Format("Loading {0}...", e.LoadedAssembly.ManifestModule.Name);
        }
    }
}