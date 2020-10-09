using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZonxScreenColor.View;

namespace ZonxScreenColor
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private const int RoiWidth = 11;
        private const int RoiHeight = 11;
        private DispatcherTimer timer;
        private Point position;
        private Color screenPixelColor = Colors.White;
        private BitmapImage imageSource = null;
        private KeyHook keyHook;
        private WinTitle Titlewin;

        public string P => $"{position.X + 1} , {position.Y + 1}";

        public int R => screenPixelColor.R;

        public int G => screenPixelColor.G;

        public int B => screenPixelColor.B;

        public string Hex => $"#{screenPixelColor.A:X2}{screenPixelColor.R:X2}{screenPixelColor.G:X2}{screenPixelColor.B:X2}";

        public SolidColorBrush Brush => new SolidColorBrush(screenPixelColor); 

        public BitmapImage ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                NotifyPropertyChanged(() => ImageSource);
            }
        }

        public DelegateCommand CloseCommand
        {
            get => new DelegateCommand((p) =>
            {
                keyHook.CloseHook();
            });
        }

        public Color ScreenPixelColor
        {
            get => screenPixelColor;
            set
            {
                screenPixelColor = value;

                NotifyPropertyChanged(() => ScreenPixelColor);
                NotifyPropertyChanged(() => Brush);
                NotifyPropertyChanged(() => P);
                NotifyPropertyChanged(() => R);
                NotifyPropertyChanged(() => G);
                NotifyPropertyChanged(() => B);
                NotifyPropertyChanged(() => Hex);
            }
        }

        public MainWindowVM()
        {
            keyHook = new KeyHook();
            keyHook.VM_ActionEvent += KeyHookEvent;
            keyHook.LoadHook();

            timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(50) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void KeyHookEvent(Key key)
        {
            string str = string.Empty;
            if (key == Key.R || key == Key.H)
            {
                Clipboard.SetDataObject(new DataObject(
                    DataFormats.Text,
                    key == Key.R ? $"{screenPixelColor.R},{screenPixelColor.G},{screenPixelColor.B}" : Hex,
                    true), true);
                Titlewin = new WinTitle(screenPixelColor, key == Key.R ? $"RGB（{screenPixelColor.R},{screenPixelColor.G},{screenPixelColor.B}）" : $"HEX（{Hex}）");
                Titlewin.Show();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ScreenPixelColor = ScreenColorGrabberUtil.GetColorUnderMousePointer(out position);
            ImageSource = ScreenColorGrabberUtil.BitmapToBitmapImage(ScreenColorGrabberUtil.GetScreenArea(position, RoiWidth, RoiHeight));
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        private void NotifyPropertyChanged<T>(Expression<Func<T>> expr)
        {
            NotifyPropertyChanged((expr.Body as MemberExpression).Member.Name);
        }
        #endregion
    }
}
