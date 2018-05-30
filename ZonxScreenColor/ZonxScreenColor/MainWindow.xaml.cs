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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZonxScreenColor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SolidColorBrush b = (SolidColorBrush)Background;
            Background = new SolidColorBrush(Color.FromArgb(0, b.Color.R, b.Color.G, b.Color.B));
            RenderOptions.SetBitmapScalingMode(pickImage, BitmapScalingMode.NearestNeighbor);

            pickImage.MouseMove += PickImage_MouseMove;
        }

        private void PickImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
