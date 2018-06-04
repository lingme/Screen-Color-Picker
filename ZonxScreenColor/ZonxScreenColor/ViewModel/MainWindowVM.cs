using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZonxScreenColor.Tool;

namespace ZonxScreenColor
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private const int RoiWidth = 11;                                              //区域放大宽比
        private const int RoiHeight = 11;                                             //区域放大高比
        private DispatcherTimer timer = new DispatcherTimer();          //刷新绘制计时器
        private Point origin = new Point(0.0, 0.0);                                
        private Point position = new Point(0.0, 0.0);
        private Color screenPixelColor = Colors.White;
        private BitmapImage imageSource = null;
        private KeyHook keyHook;
        private MouesHook mouseHook;

        /// <summary>
        /// 鼠标坐标
        /// </summary>
        public string P { get { return $"{(position.X - origin.X).ToString()} , {(position.Y - origin.Y).ToString()}"; } }

        /// <summary>
        /// 鼠标所在 RGB - R
        /// </summary>
        public int R { get { return screenPixelColor.R; } }

        /// <summary>
        /// 鼠标所在 RGB - G
        /// </summary>
        public int G { get { return screenPixelColor.G; } }

        /// <summary>
        /// 鼠标所在 RGB - B
        /// </summary>
        public int B { get { return screenPixelColor.B; } }

        /// <summary>
        /// 鼠标所在16进制颜色
        /// </summary>
        public string Hex
        {
            get
            {
                return $"#{screenPixelColor.A.ToString("X2")}{screenPixelColor.R.ToString("X2")}{screenPixelColor.G.ToString("X2")}{screenPixelColor.B.ToString("X2")}";
            }
        }

        /// <summary>
        /// 鼠标所在Brush颜色
        /// </summary>
        public SolidColorBrush Brush { get { return new SolidColorBrush(screenPixelColor); } }

        /// <summary>
        /// 鼠标所在区域放大
        /// </summary>
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                NotifyPropertyChanged(() => ImageSource);
            }
        }

        /// <summary>
        /// 关闭处理
        /// </summary>
        public SimpleDelegateCommand CloseCommand
        {
            get
            {
                return new SimpleDelegateCommand((p) => {
                    keyHook.CloseHook();
                    mouseHook.CloseHook();
                });
            }
        }

        /// <summary>
        /// 获取指针所在位置颜色
        /// </summary>
        public Color ScreenPixelColor
        {
            get { return screenPixelColor; }
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

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindowVM()
        {
            //timer.Interval = TimeSpan.FromMilliseconds(50);
            //timer.Tick += Timer_Tick;
            //timer.Start();

            keyHook = new KeyHook();
            keyHook.VM_ActionEvent += KeyHookControlHandle;
            keyHook.LoadHook();

            mouseHook = new MouesHook();
            mouseHook.VM_ActionEvent += MouseHookControlHandle;
            mouseHook.LoadHook();
        }

        /// <summary>
        /// 鼠标钩子回调
        /// </summary>
        private void MouseHookControlHandle(int mouseX,int mouseY)
        {
            position = new Point(mouseX, mouseY);
            ScreenPixelColor = ScreenColorGrabberUtil.GetColorUnderMousePointer(mouseX , mouseY);
            ImageSource = ScreenColorGrabberUtil.BitmapToBitmapImage(ScreenColorGrabberUtil.GetScreenArea(position, RoiWidth, RoiHeight));
        }

        /// <summary>
        /// 键盘钩子回调
        /// </summary>
        /// <param name="key"></param>
        private void KeyHookControlHandle(Key key)
        {
            if(key == Key.R)
            {
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, $"{screenPixelColor.R},{screenPixelColor.G},{screenPixelColor.B}", true), true);
            }
            if (key == Key.H)
            {
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, Hex, true), true);
            }
        }

        ///// <summary>
        ///// 计时器 - 负责重绘
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    ScreenPixelColor = ScreenColorGrabberUtil.GetColorUnderMousePointer(out position);

        //    ImageSource = ScreenColorGrabberUtil.BitmapToBitmapImage(
        //        ScreenColorGrabberUtil.GetScreenArea(position, RoiWidth, RoiHeight));
        //}

        #region 数据修改通知
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
