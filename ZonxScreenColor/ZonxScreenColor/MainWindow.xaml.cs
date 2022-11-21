using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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

            SolidColorBrush color = (SolidColorBrush)Background;
            Background = new SolidColorBrush(Color.FromArgb(0, color.Color.R, color.Color.G, color.Color.B));
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
