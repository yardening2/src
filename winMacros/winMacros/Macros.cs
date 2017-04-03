using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace winMacros
{
    public static class Macros
    {
        public static void WindowsScreen()
        {
            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.TAB);
        }

        public static void Desktop()
        {
            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_D);
        }

        public static void Taskmgr()
        {
            Process.Start("taskmgr");
        }

        public static void osk()
        {
            Process.Start("osk");
        }

        public static void ShiftLastWindow(int i_slot)
        {
            IntPtr lastWindowHandle = getLastWindow();

            if (lastWindowHandle.Equals(GetDesktopWindow()) == false && IsWindowVisible(lastWindowHandle))
            {
                ShowWindow(lastWindowHandle, ShowWindowCommands.Normal);
                MoveWindow(lastWindowHandle, 600 * i_slot, 0, 600, 600, true);
                SetForegroundWindow(lastWindowHandle);
            }
        }

        public static void LastWindowShow(ShowWindowCommands i_swc)
        {
            IntPtr lastWindowHandle = getLastWindow();

            if (lastWindowHandle.Equals(GetDesktopWindow()) == false)
            {
                ShowWindow(lastWindowHandle, i_swc);
                SetForegroundWindow(lastWindowHandle);
            }
        }

        private static IntPtr getLastWindow()
        {
            IntPtr deskTopPtr = GetDesktopWindow();
            IntPtr lastWindowHandle = GetWindow(Process.GetCurrentProcess().MainWindowHandle, (uint)GetWindow_Cmd.GW_HWNDNEXT);
            while (true)
            {
                IntPtr temp = GetParent(lastWindowHandle);
                if (temp.Equals(IntPtr.Zero)) break;
                lastWindowHandle = temp;
            }
            return lastWindowHandle;
        }





        //user32.dll functions and enums
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        private enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        public enum ShowWindowCommands : uint
        {
            Hide = 0,
            Normal = 1,
            ShowMinimized = 2,
            Maximize = 3,
            ShowMaximized = 3,
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);
    }
}
