using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZonxScreenColor.Tool
{
    public class MouesHook
    {
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public HookProc MouseHookProcedure;

        public static int hMouseHook = 0;
        public const int WH_MOUSE_LL = 14;

        public Action<int,int> VM_ActionEvent;
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        public int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                VM_ActionEvent?.Invoke(MyMouseHookStruct.pt.x,MyMouseHookStruct.pt.y);
            }
            return 0;
        }

        public void LoadHook()
        {
            if(hMouseHook == 0)
            {
                MouseHookProcedure = new HookProc(MouseHookProc);
                Process curProcess = Process.GetCurrentProcess();
                ProcessModule curModule = curProcess.MainModule;
                hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public void CloseHook()
        {
            if(hMouseHook != 0)
            {
                UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
            }
        }
    }
}
