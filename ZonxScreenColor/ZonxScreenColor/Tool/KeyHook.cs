using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        private const int WM_KEYDOWN = 0x100;
        private const int WM_SYSKEYDOWN = 0x104;
        public const int WH_KEYBOARD_LL = 13;

        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode; 
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        string str = string.Empty;

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
