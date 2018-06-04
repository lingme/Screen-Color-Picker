using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            Title = "ZonxScreenColor";

            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(++second == 6)
            {
                this.Close();
            }
        }
    }
}
