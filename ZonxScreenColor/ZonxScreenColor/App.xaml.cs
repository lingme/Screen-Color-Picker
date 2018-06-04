using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ZonxScreenColor
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            var isRuning = false;
            mutex = new Mutex(true, "ZonxScreenColor", out isRuning);
            if (!isRuning)
            {
                MessageBox.Show("程序已在运行，请勿重复启动！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                Environment.Exit(0);
            }
            base.OnStartup(e);
        }
    }
}
