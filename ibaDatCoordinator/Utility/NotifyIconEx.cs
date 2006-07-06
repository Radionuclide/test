using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using iba.Logging;

namespace iba.Utility
{
    public sealed class NotifyIconEx : Component
    {
        // Events
        [Category("Action"), Description("Occurs when the balloon is dismissed because of a mouse click.")]
        public event BalloonEventHandler BalloonClick;
        [Category("Behavior"), Description("Occurs when the balloon disappears\u2014for example, when the icon is deleted. This message is not sent if the balloon is dismissed because of a timeout or a mouse click.")]
        public event BalloonEventHandler BalloonHide;
        [Description("Occurs when the balloon is shown (balloons are queued)."), Category("Behavior")]
        public event BalloonEventHandler BalloonShow;
        [Description("Occurs when the balloon is dismissed because of a timeout."), Category("Behavior")]
        public event BalloonEventHandler BalloonTimeout;
        [Description("Occurs when the user clicks the icon in the status area."), Category("Action")]
        public event EventHandler Click;
        [Description("Occurs when the user double-clicks the icon in the status notification area of the taskbar."), Category("Action")]
        public event EventHandler DoubleClick;
        [Description("Occurs when the user presses the mouse button while the pointer is over the icon in the status notification area of the taskbar."), Category("Mouse")]
        public event MouseEventHandler MouseDown;
        [Description("Occurs when the user moves the mouse while the pointer is over the icon in the status notification area of the taskbar."), Category("Mouse")]
        public event MouseEventHandler MouseMove;
        [Description("Occurs when the user releases the mouse button while the pointer is over the icon in the status notification area of the taskbar."), Category("Mouse")]
        public event MouseEventHandler MouseUp;

        // Methods
        public NotifyIconEx()
        {
            Messages = new MessageHandler();
            InitializeComponent();
            Initialize();
        }

        public NotifyIconEx(IContainer Container) : this()
        {
            Container.Add(this);
            Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Visible = false;
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void Initialize()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Enabled = false;
            refreshTimer.Interval = 1000;
            refreshTimer.Tick += new EventHandler(OnRefreshTimer);

            NID.hwnd = Messages.Handle;
            NID.cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
            NID.uFlags = 7;
            NID.uCallbackMessage = 0x401;
            NID.uVersion = 5;
            NID.szTip = "";
            NID.uID = 0;
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            components = new Container();
        }

        private void Messages_BalloonClick(object sender)
        {
            if (!m_VisibleBeforeBalloon)
            {
                Visible = false;
            }
            if (BalloonClick != null)
            {
                BalloonClick(this);
            }
        }

        private void Messages_BalloonHide(object sender)
        {
            if (BalloonHide != null)
            {
                BalloonHide(this);
            }
        }

        private void Messages_BalloonShow(object sender)
        {
            if (BalloonShow != null)
            {
                BalloonShow(this);
            }
        }

        private void Messages_BalloonTimeout(object sender)
        {
            if (!m_VisibleBeforeBalloon)
            {
                Visible = false;
            }
            if (BalloonTimeout != null)
            {
                BalloonTimeout(this);
            }
        }

        private void Messages_Click(object sender, EventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }

        private void Messages_DoubleClick(object sender, EventArgs e)
        {
            if (DoubleClick != null)
            {
                DoubleClick(this, e);
            }
        }

        private void Messages_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, e);
            }
        }

        private void Messages_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseMove != null)
            {
                MouseMove(this, e);
            }
        }

        private void Messages_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(this, e);
            }
            if (e.Button == MouseButtons.Right)
            {
                Point point2;
                Point point4;
                Point point7;
                Messages.Activate();
                Point point8 = Cursor.Position;
                point7 = new Point(0, 0);
                Point point6 = Messages.PointToScreen(point7);
                Point point5 = Cursor.Position;
                point4 = new Point(0, 0);
                Point point3 = Messages.PointToScreen(point4);
                point2 = new Point(point8.X - point6.X, point5.Y - point3.Y);
                Point point1 = point2;
                if (m_ContextMenu != null)
                {
                    m_ContextMenu.Show(Messages, point1);
                }
            }
        }

        private void Messages_Reload()
        {
            if (Visible)
            {
                Visible = true;
            }
        }

        [DllImport("Shell32", CharSet=CharSet.Auto)]
        private static extern bool Shell_NotifyIcon(int dwMessage, ref NOTIFYICONDATA lpData);

        public void ShowBalloon(EBalloonIcon Icon, string Text, string Title, [Optional] int Timeout /* = 15000 */)
        {
            m_VisibleBeforeBalloon = m_Visible;
            NID.uFlags |= 0x10;
            NID.uVersion = Timeout;
            NID.szInfo = Text;
            NID.szInfoTitle = Title;
            NID.dwInfoFlags = Convert.ToInt32((int) Icon);
            if (!Visible)
            {
                Visible = true;
            }
            else
            {
				if(!Shell_NotifyIcon(1, ref NID))
				{
					ShellError = true;
					ibaLogger.Log(Level.Debug, "Error Shell_NotifyIcon in ShowBalloon");
				}
				else
					ShellError = false;
            }
            NID.uFlags &= -17;
        }


        // Properties
        [DefaultValue(""), Description("The pop-up menu to show when the user right-clicks the icon."), Category("Behavior")]
        public System.Windows.Forms.ContextMenu ContextMenu
        {
            get
            {
                return m_ContextMenu;
            }
            set
            {
                m_ContextMenu = value;
            }
        }

        [Description("The icon to display in the system tray."), DefaultValue(""), Category("Appearance")]
        public System.Drawing.Icon Icon
        {
            get
            {
                return m_Icon;
            }
            set
            {
                m_Icon = value;
                NID.uFlags |= 2;
                NID.hIcon = Icon.Handle;
                if (Visible)
                {
					if(!Shell_NotifyIcon(1, ref NID))
					{
						ShellError = true;
						ibaLogger.Log(Level.Debug, "Error Shell_NotifyIcon in set Icon");
					}
					else
						ShellError = false;
                }
            }
        }

        private MessageHandler Messages
        {
            get
            {
                return _Messages;
            }
            [MethodImpl(MethodImplOptions.Synchronized)] 
			set
            {
                if (_Messages != null)
                {
                    _Messages.BalloonClick -= new BalloonEventHandler(Messages_BalloonClick);
                    _Messages.BalloonTimeout -= new BalloonEventHandler(Messages_BalloonTimeout);
                    _Messages.BalloonHide -= new BalloonEventHandler(Messages_BalloonHide);
                    _Messages.BalloonShow -= new BalloonEventHandler(Messages_BalloonShow);
                    _Messages.Reload -= new MessageHandler.ReloadEventHandler(Messages_Reload);
                    _Messages.MouseUp -= new MouseEventHandler(Messages_MouseUp);
                    _Messages.MouseMove -= new MouseEventHandler(Messages_MouseMove);
                    _Messages.MouseDown -= new MouseEventHandler(Messages_MouseDown);
                    _Messages.DoubleClick -= new EventHandler(Messages_DoubleClick);
                    _Messages.Click -= new EventHandler(Messages_Click);
                }
                _Messages = value;
                if (_Messages != null)
                {
                    _Messages.BalloonClick += new BalloonEventHandler(Messages_BalloonClick);
                    _Messages.BalloonTimeout += new BalloonEventHandler(Messages_BalloonTimeout);
                    _Messages.BalloonHide += new BalloonEventHandler(Messages_BalloonHide);
                    _Messages.BalloonShow += new BalloonEventHandler(Messages_BalloonShow);
                    _Messages.Reload += new MessageHandler.ReloadEventHandler(Messages_Reload);
                    _Messages.MouseUp += new MouseEventHandler(Messages_MouseUp);
                    _Messages.MouseMove += new MouseEventHandler(Messages_MouseMove);
                    _Messages.MouseDown += new MouseEventHandler(Messages_MouseDown);
                    _Messages.DoubleClick += new EventHandler(Messages_DoubleClick);
                    _Messages.Click += new EventHandler(Messages_Click);
                }
            }
        }

        [Description("The text that will be displayed when the mouse hovers over the icon."), Category("Appearance")]
        public string Text
        {
            get
            {
                return NID.szTip;
            }
            set
            {
                NID.szTip = value;
                if (Visible)
                {
                    NID.uFlags |= 4;
					if(!Shell_NotifyIcon(1, ref NID))
					{
						ShellError = true;
						ibaLogger.Log(Level.Debug, "Error Shell_NotifyIcon in set Text");
					}
					else
						ShellError = false;
                }
            }
        }

        [Category("Behavior"), DefaultValue(false), Description("Determines whether the control is visible or hidden.")]
        public bool Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                m_Visible = value;
                if (!DesignMode)
                {
                    if (m_Visible)
                    {
						if(!Shell_NotifyIcon(0, ref NID))
						{
						    ShellError = true;
							ibaLogger.Log(Level.Debug, "Error Shell_NotifyIcon in Visible = true");
						}
						else
							ShellError = false;
                    }
                    else
                    {
						if(!Shell_NotifyIcon(2, ref NID))
						{
							ShellError = true;
							ibaLogger.Log(Level.Debug, "Error Shell_NotifyIcon in Visible = false");
						}
						else
							ShellError = false;
                    }
                }
            }
        }

		public bool ShellError
		{
			get {return refreshTimer.Enabled;}
            set {refreshTimer.Enabled = value;}
		}

        private System.Windows.Forms.Timer refreshTimer;
        private void OnRefreshTimer(object sender, EventArgs e)
        {
            ibaLogger.Log(Level.Debug, "In refresh timer of NotifyIconEx");
            Visible = Visible;
            Text = Text;
            Icon = Icon;
        }

        // Fields
        [AccessedThroughProperty("Messages")]
        private MessageHandler _Messages;
        private IContainer components;
        private System.Windows.Forms.ContextMenu m_ContextMenu;
        private System.Drawing.Icon m_Icon;
        private bool m_Visible;
        private bool m_VisibleBeforeBalloon;
        private NOTIFYICONDATA NID;
        private const int NIF_ICON = 2;
        private const int NIF_INFO = 0x10;
        private const int NIF_MESSAGE = 1;
        private const int NIF_STATE = 8;
        private const int NIF_TIP = 4;
        private const int NIM_ADD = 0;
        private const int NIM_DELETE = 2;
        private const int NIM_MODIFY = 1;
        private const int NIM_SETVERSION = 4;
        private const int NOTIFYICON_VERSION = 5;
        private const int WM_USER = 0x400;
        private const int WM_USER_TRAY = 0x401;

        // Nested Types
        public delegate void BalloonEventHandler(object sender);

        public enum EBalloonIcon
        {
            // Fields
            Error = 3,
            Info = 1,
            None = 0,
            Warning = 2
        }

        private class MessageHandler : Form
        {
            // Events
            public event BalloonEventHandler BalloonClick;
            public event BalloonEventHandler BalloonHide;
            public event BalloonEventHandler BalloonShow;
            public event BalloonEventHandler BalloonTimeout;
            public new event EventHandler Click;
            public new event EventHandler DoubleClick;
            public new event MouseEventHandler MouseDown;
            public new event MouseEventHandler MouseMove;
            public new event MouseEventHandler MouseUp;
            public event ReloadEventHandler Reload;

            // Methods
            public MessageHandler()
            {
                Point point1;
                Size size1;
                WM_TASKBARCREATED = MessageHandler.RegisterWindowMessage("TaskbarCreated");
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.Manual;
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
                size1 = new Size(100, 100);
                Size = size1;
                point1 = new Point(-500, -500);
                Location = point1;
                Show();
            }

            [DllImport("User32", CharSet=CharSet.Auto)]
            private static extern int RegisterWindowMessage(string lpString);

            protected override void WndProc(ref Message m)
            {
                int num2 = m.Msg;
                if (num2 == 0x401)
                {
                    Point point1;
                    Point point2;
                    IntPtr ptr1 = m.LParam;
                    int num1 = ptr1.ToInt32();
                    if (((num1 == 0x203) || (num1 == 0x206)) || (num1 == 0x209))
                    {
                        if (DoubleClick != null)
                        {
                            point2 = Control.MousePosition;
                            point1 = Control.MousePosition;
                            DoubleClick(this, new MouseEventArgs(Control.MouseButtons, 0, point2.X, point1.Y, 0));
                        }
                    }
                    else if (((num1 == 0x201) || (num1 == 0x204)) || (num1 == 0x207))
                    {
                        if (MouseDown != null)
                        {
                            point1 = Control.MousePosition;
                            point2 = Control.MousePosition;
                            MouseDown(this, new MouseEventArgs(Control.MouseButtons, 0, point1.X, point2.Y, 0));
                        }
                    }
                    else if (num1 == 0x200)
                    {
                        if (MouseMove != null)
                        {
                            point1 = Control.MousePosition;
                            point2 = Control.MousePosition;
                            MouseMove(this, new MouseEventArgs(Control.MouseButtons, 0, point1.X, point2.Y, 0));
                        }
                    }
                    else if (num1 == 0x202)
                    {
                        if (MouseUp != null)
                        {
                            point1 = Control.MousePosition;
                            point2 = Control.MousePosition;
                            MouseUp(this, new MouseEventArgs(MouseButtons.Left, 0, point1.X, point2.Y, 0));
                        }
                        if (Click != null)
                        {
                            point1 = Control.MousePosition;
                            point2 = Control.MousePosition;
                            Click(this, new MouseEventArgs(MouseButtons.Left, 0, point1.X, point2.Y, 0));
                        }
                    }
                    else if (num1 == 0x205)
                    {
                        if (MouseUp != null)
                        {
                            point1 = Control.MousePosition;
                            point2 = Control.MousePosition;
                            MouseUp(this, new MouseEventArgs(MouseButtons.Right, 0, point1.X, point2.Y, 0));
                        }
                        if (Click != null)
                        {
                            point1 = Control.MousePosition;
                            point2 = Control.MousePosition;
                            Click(this, new MouseEventArgs(MouseButtons.Right, 0, point1.X, point2.Y, 0));
                        }
                    }
                    else if (num1 == 520)
                    {
                        if (MouseUp != null)
                        {
                            point1 = Control.MousePosition;
                            point2 = Control.MousePosition;
                            MouseUp(this, new MouseEventArgs(MouseButtons.Middle, 0, point1.X, point2.Y, 0));
                        }
                        if (Click != null)
                        {
                            point1 = Control.MousePosition;
                            point2 = Control.MousePosition;
                            Click(this, new MouseEventArgs(MouseButtons.Middle, 0, point1.X, point2.Y, 0));
                        }
                    }
                    else if (num1 == 0x402)
                    {
                        if (BalloonShow != null)
                        {
                            BalloonShow(this);
                        }
                    }
                    else if (num1 == 0x403)
                    {
                        if (BalloonHide != null)
                        {
                            BalloonHide(this);
                        }
                    }
                    else if (num1 == 0x404)
                    {
                        if (BalloonTimeout != null)
                        {
                            BalloonTimeout(this);
                        }
                    }
                    else if (num1 == 0x405)
                    {
                        if (BalloonClick != null)
                        {
                            BalloonClick(this);
                        }
                    }
                    else
                    {
                        ptr1 = m.LParam;
                        Debug.WriteLine(ptr1.ToInt32());
                    }
                }
                else if ((num2 == WM_TASKBARCREATED) && (Reload != null))
                {
					ibaLogger.Log(Level.Debug, "WM_TASKBARCREATED");
                    Reload();
                }
                base.WndProc(ref m);
            }


            // Fields
            private const int NIN_BALLOONHIDE = 0x403;
            private const int NIN_BALLOONSHOW = 0x402;
            private const int NIN_BALLOONTIMEOUT = 0x404;
            private const int NIN_BALLOONUSERCLICK = 0x405;
            private const int WM_LBUTTONDBLCLK = 0x203;
            private const int WM_LBUTTONDOWN = 0x201;
            private const int WM_LBUTTONUP = 0x202;
            private const int WM_MBUTTONDBLCLK = 0x209;
            private const int WM_MBUTTONDOWN = 0x207;
            private const int WM_MBUTTONUP = 520;
            private const int WM_MOUSEMOVE = 0x200;
            private const int WM_RBUTTONDBLCLK = 0x206;
            private const int WM_RBUTTONDOWN = 0x204;
            private const int WM_RBUTTONUP = 0x205;
            private int WM_TASKBARCREATED;
            private const int WM_USER = 0x400;
            private const int WM_USER_TRAY = 0x401;

            // Nested Types
             public delegate void ReloadEventHandler();

        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        private struct NOTIFYICONDATA
        {
            public int cbSize;
            public IntPtr hwnd;
            public int uID;
            public int uFlags;
            public int uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
            public string szTip;
            public int dwState;
            public int dwStateMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x100)]
            public string szInfo;
            public int uVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x40)]
            public string szInfoTitle;
            public int dwInfoFlags;
        }
    }
}

