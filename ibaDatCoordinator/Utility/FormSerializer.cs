using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace iba.Utility
{
    public class FormStateSerializer
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Point
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct Rect
        {
            [FieldOffset(0)]
            public int left;
            [FieldOffset(4)]
            public int top;
            [FieldOffset(8)]
            public int right;
            [FieldOffset(12)]
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowPlacement
        {
            public int length;
            public int flags;
            public int showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rect rcNormalPosition;
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement wndP);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPlacement(IntPtr hWnd, ref WindowPlacement wndP);

        public static void SaveSettings(Form form, string name)
        {
            WindowPlacement wndP = new WindowPlacement();
            wndP.length = Marshal.SizeOf(wndP);
            GetWindowPlacement(form.Handle, ref wndP);
            Profiler.ProfileInt(false, name, "Flags", ref wndP.flags, wndP.flags);
            Profiler.ProfileInt(false, name, "ShowCmd", ref wndP.showCmd, wndP.showCmd);
            Profiler.ProfileInt(false, name, "MinPositionX", ref wndP.ptMinPosition.x, wndP.ptMinPosition.x);
            Profiler.ProfileInt(false, name, "MinPositionY", ref wndP.ptMinPosition.y, wndP.ptMinPosition.x);
            Profiler.ProfileInt(false, name, "MaxPositionX", ref wndP.ptMaxPosition.x, wndP.ptMinPosition.x);
            Profiler.ProfileInt(false, name, "MaxPositionY", ref wndP.ptMaxPosition.y, wndP.ptMinPosition.x);
            Profiler.ProfileInt(false, name, "NormalPositionLeft", ref wndP.rcNormalPosition.left, wndP.ptMinPosition.x);
            Profiler.ProfileInt(false, name, "NormalPositionRight", ref wndP.rcNormalPosition.right, wndP.ptMinPosition.x);
            Profiler.ProfileInt(false, name, "NormalPositionTop", ref wndP.rcNormalPosition.top, wndP.ptMinPosition.x);
            Profiler.ProfileInt(false, name, "NormalPositionBottom", ref wndP.rcNormalPosition.bottom, wndP.ptMinPosition.x);
        }

        public static void LoadSettings(Form form, string name)
        {
            if (!Profiler.KeyExists(name)) return;

            WindowPlacement wndP = new WindowPlacement();
            wndP.length = Marshal.SizeOf(wndP);
            GetWindowPlacement(form.Handle, ref wndP);
            Profiler.ProfileInt(true, name, "Flags", ref wndP.flags, wndP.flags);
            Profiler.ProfileInt(true, name, "ShowCmd", ref wndP.showCmd, wndP.showCmd);
            Profiler.ProfileInt(true, name, "MinPositionX", ref wndP.ptMinPosition.x, wndP.ptMinPosition.x);
            Profiler.ProfileInt(true, name, "MinPositionY", ref wndP.ptMinPosition.y, wndP.ptMinPosition.x);
            Profiler.ProfileInt(true, name, "MaxPositionX", ref wndP.ptMaxPosition.x, wndP.ptMinPosition.x);
            Profiler.ProfileInt(true, name, "MaxPositionY", ref wndP.ptMaxPosition.y, wndP.ptMinPosition.x);
            Profiler.ProfileInt(true, name, "NormalPositionLeft", ref wndP.rcNormalPosition.left, wndP.ptMinPosition.x);
            Profiler.ProfileInt(true, name, "NormalPositionRight", ref wndP.rcNormalPosition.right, wndP.ptMinPosition.x);
            Profiler.ProfileInt(true, name, "NormalPositionTop", ref wndP.rcNormalPosition.top, wndP.ptMinPosition.x);
            Profiler.ProfileInt(true, name, "NormalPositionBottom", ref wndP.rcNormalPosition.bottom, wndP.ptMinPosition.x);
            SetWindowPlacement(form.Handle, ref wndP);
        }

        public static string SaveSettingsToString(Form form)
        {
            WindowPlacement wndP = new WindowPlacement();
            wndP.length = Marshal.SizeOf(wndP);
            GetWindowPlacement(form.Handle, ref wndP);

            string settings = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}",
                wndP.flags,
                wndP.showCmd,
                wndP.ptMinPosition.x,
                wndP.ptMinPosition.y,
                wndP.ptMaxPosition.x,
                wndP.ptMaxPosition.y,
                wndP.rcNormalPosition.left,
                wndP.rcNormalPosition.right,
                wndP.rcNormalPosition.top,
                wndP.rcNormalPosition.bottom
            );

            return settings;
        }

        public static void LoadSettingsFromString(Form form, string settings)
        {
            WindowPlacement wndP = new WindowPlacement();
            wndP.length = Marshal.SizeOf(wndP);
            GetWindowPlacement(form.Handle, ref wndP);

            string[] parts = settings.Split(';');
            if (parts.Length < 10)
                return;

            wndP.flags = Int32.Parse(parts[0]);
            wndP.showCmd = Int32.Parse(parts[1]);
            wndP.ptMinPosition.x = Int32.Parse(parts[2]);
            wndP.ptMinPosition.y = Int32.Parse(parts[3]);
            wndP.ptMaxPosition.x = Int32.Parse(parts[4]);
            wndP.ptMaxPosition.y = Int32.Parse(parts[5]);
            wndP.rcNormalPosition.left = Int32.Parse(parts[6]);
            wndP.rcNormalPosition.right = Int32.Parse(parts[7]);
            wndP.rcNormalPosition.top = Int32.Parse(parts[8]);
            wndP.rcNormalPosition.bottom = Int32.Parse(parts[9]);

            SetWindowPlacement(form.Handle, ref wndP);
        }
    }
}