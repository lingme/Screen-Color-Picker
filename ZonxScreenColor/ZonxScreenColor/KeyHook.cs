using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZonxScreenColor
{
    public class KeyHook
    {
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        HookProc KeyboardHookProcedure;

        public Action<Key> VM_ActionEvent; 

        static int hKeyboardHook = 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        // 装载键盘钩子
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        // 卸载键盘钩子
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        // 获取进程句柄
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // 普通按键消息  
        private const int WM_KEYDOWN = 0x100;

        // 系统按键消息  
        private const int WM_SYSKEYDOWN = 0x104;

        //鼠标常量   
        public const int WH_KEYBOARD_LL = 13;

        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;               //表示一个在1到254间的虚似键盘码   
            public int scanCode;             //表示硬件扫描码   
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        string str = string.Empty;

        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                Key keyData = KeyInterop.KeyFromVirtualKey(MyKeyboardHookStruct.vkCode);
                VM_ActionEvent?.Invoke(keyData);
            }
            return 0;
        }

        /// <summary>
        /// 装载钩子
        /// </summary>
        public void LoadHook()
        {
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                Process curProcess = Process.GetCurrentProcess();
                ProcessModule curModule = curProcess.MainModule;
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        /// <summary>
        /// 卸载钩子
        /// </summary>
        public void CloseHook()
        {
            if (hKeyboardHook != 0)
            {
                UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }
        }
    }
}
