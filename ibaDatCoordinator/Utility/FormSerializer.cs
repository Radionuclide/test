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
            DatCoProfiler.ProfileInt(false, name, "Flags", ref wndP.flags, wndP.flags);
            DatCoProfiler.ProfileInt(false, name, "ShowCmd", ref wndP.showCmd, wndP.showCmd);
            DatCoProfiler.ProfileInt(false, name, "MinPositionX", ref wndP.ptMinPosition.x, wndP.ptMinPosition.x);
            DatCoProfiler.ProfileInt(false, name, "MinPositionY", ref wndP.ptMinPosition.y, wndP.ptMinPosition.y);
            DatCoProfiler.ProfileInt(false, name, "MaxPositionX", ref wndP.ptMaxPosition.x, wndP.ptMaxPosition.x);
            DatCoProfiler.ProfileInt(false, name, "MaxPositionY", ref wndP.ptMaxPosition.y, wndP.ptMaxPosition.y);
            DatCoProfiler.ProfileInt(false, name, "NormalPositionLeft", ref wndP.rcNormalPosition.left, wndP.rcNormalPosition.left);
            DatCoProfiler.ProfileInt(false, name, "NormalPositionRight", ref wndP.rcNormalPosition.right, wndP.rcNormalPosition.right);
            DatCoProfiler.ProfileInt(false, name, "NormalPositionTop", ref wndP.rcNormalPosition.top, wndP.rcNormalPosition.top);
            DatCoProfiler.ProfileInt(false, name, "NormalPositionBottom", ref wndP.rcNormalPosition.bottom, wndP.rcNormalPosition.bottom);
        }

        public static void LoadSettings(Form form, string name, bool onlyNormalPosition=false)
        {
            if (!DatCoProfiler.KeyExists(name)) return;

            WindowPlacement wndP = new WindowPlacement();
            wndP.length = Marshal.SizeOf(wndP);
            GetWindowPlacement(form.Handle, ref wndP);
            if(!onlyNormalPosition)
            {
                DatCoProfiler.ProfileInt(true, name, "Flags", ref wndP.flags, wndP.flags);
                DatCoProfiler.ProfileInt(true, name, "ShowCmd", ref wndP.showCmd, wndP.showCmd);
                DatCoProfiler.ProfileInt(true, name, "MinPositionX", ref wndP.ptMinPosition.x, wndP.ptMinPosition.x);
                DatCoProfiler.ProfileInt(true, name, "MinPositionY", ref wndP.ptMinPosition.y, wndP.ptMinPosition.y);
                DatCoProfiler.ProfileInt(true, name, "MaxPositionX", ref wndP.ptMaxPosition.x, wndP.ptMaxPosition.x);
                DatCoProfiler.ProfileInt(true, name, "MaxPositionY", ref wndP.ptMaxPosition.y, wndP.ptMaxPosition.y);
            }
            DatCoProfiler.ProfileInt(true, name, "NormalPositionLeft", ref wndP.rcNormalPosition.left, wndP.rcNormalPosition.left);
            DatCoProfiler.ProfileInt(true, name, "NormalPositionRight", ref wndP.rcNormalPosition.right, wndP.rcNormalPosition.right);
            DatCoProfiler.ProfileInt(true, name, "NormalPositionTop", ref wndP.rcNormalPosition.top, wndP.rcNormalPosition.top);
            DatCoProfiler.ProfileInt(true, name, "NormalPositionBottom", ref wndP.rcNormalPosition.bottom, wndP.rcNormalPosition.bottom);
            SetWindowPlacement(form.Handle, ref wndP);
            if(form.WindowState == FormWindowState.Maximized)
                form.Bounds = new System.Drawing.Rectangle(wndP.rcNormalPosition.left, wndP.rcNormalPosition.top,
                    wndP.rcNormalPosition.right - wndP.rcNormalPosition.left, wndP.rcNormalPosition.bottom - wndP.rcNormalPosition.top);
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

            if(form.WindowState == FormWindowState.Maximized)
                form.Bounds = new System.Drawing.Rectangle(wndP.rcNormalPosition.left, wndP.rcNormalPosition.top,
                    wndP.rcNormalPosition.right - wndP.rcNormalPosition.left, wndP.rcNormalPosition.bottom - wndP.rcNormalPosition.top);
        }
    }
}