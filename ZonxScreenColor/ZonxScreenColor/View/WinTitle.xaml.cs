using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ZonxScreenColor.View
{
    /// <summary>
    /// WinTitle.xaml 的交互逻辑
    /// </summary>
    public partial class WinTitle : Window
    {
        private DispatcherTimer timer;
        private int second = 0;
        public WinTitle(Color color , string str)
        {
            InitializeComponent();
            eli.Fill = new SolidColorBrush(color);
            text.Text = $"{str} 已复制";

            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (s,e) => {
                if (++second == 3)
                    Close();
            };
            timer.Start();
        }
    }
}
