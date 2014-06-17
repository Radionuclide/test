using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

//using iba.Utility;
//using iba.Utility.Win32;

namespace iba.Utility
{

    public class ShadowForm : Form
    {
        // Define the CS_DROPSHADOW constant
        private const int CS_DROPSHADOW = 0x00020000;

        // Override the CreateParams property
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (OSFeature.IsPresent(SystemParameter.DropShadow))
                    cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int borderWidth = 2;
            Color borderColor = SystemColors.ControlLight;
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor,
              borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth,
              ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid,
              borderColor, borderWidth, ButtonBorderStyle.Solid);
        }
    }

	[ToolboxItem(false)]
	public abstract class CustomCombo :  System.Windows.Forms.Control
	{
		#region Class members
        protected ShadowForm m_dropDownForm;
		protected bool      m_bFirstShow = true;
		protected bool      m_bControlBinded;
		protected Control   m_ctrlBinded;
		protected bool      m_bCanInvalidate = true;

		private bool        m_bOwnValuePaint;
		protected TextBox   m_editbox;
		private Rectangle   m_clickButton;
		private bool        m_bDropDown;
		private int         m_iDropDownHeight = 120;

		private bool        m_bSkipClick;

		private IntPtr        m_mouseHookHandle = IntPtr.Zero;
		private GCHandle      m_mouseProcHandle;
		//private SolidBrush    m_brushSelected = new SolidBrush(ControlPaint.LightLight(SystemColors.Highlight) );
		private StringFormat  m_strFormat;
        
        protected string errorString;

		#endregion

		#region Class Properties
		[DefaultValue("")]
		public string Value
		{
			get
			{
				return m_editbox.Text;
			}
			set
			{
				if( value != m_editbox.Text )
				{
					if( OnValueValidate( value ) == true )
					{
                        string oldVal = m_editbox.Text;
						m_editbox.Text = value;
						OnValueChanged(oldVal, value);
            
						if( m_bCanInvalidate == true )
							Invalidate();
					}
					else
					{
						throw new ApplicationException( "You try to assign incorect data to Value property." );
					}
				} 
			}
		}
    
		[Browsable(false)]
		[DefaultValue(false)]
		public bool DroppedDown
		{
			get
			{
				return m_bDropDown;
			}
			set
			{
				if( value != m_bDropDown )
				{
					m_bDropDown = value;
          
					if( m_bDropDown == true )
					{
						ShowDropDownForm();
					}
					else
					{
						HideDropDownForm();
					}
				} 
			}
		}
    
		[DefaultValue(120)]
		public int DropDownHeigth
		{
			get
			{
				return m_iDropDownHeight;
			}
			set
			{
				if( value != m_iDropDownHeight )
				{
					m_iDropDownHeight = value;
					OnDropDownSizeChanged();
				}
			}
		}
    
		[DefaultValue(false)]
		public bool ReadOnly
		{
			get
			{
				return m_editbox.ReadOnly;
			}
			set
			{
				if( value != m_editbox.ReadOnly )
				{
					m_editbox.ReadOnly = value;
					OnReadOnlyStateChanged();

					if( m_bCanInvalidate == true )
						Invalidate();
				}
			}
		}
		#endregion

		#region Internal Classes
		public class EventArgsBindDropDownControl : EventArgs
		{
			#region Class members
			private CustomCombo m_parent;
			private Form    m_frm;
			private Control m_ctrl;
			#endregion

			#region Class Properties
			public Form DropDownForm
			{
				get{ return m_frm; }
			}

			public Control BindedControl
			{
				get
				{
					return m_ctrl;
				}
				set
				{
					m_ctrl = value;
				}
			}

			public CustomCombo  Combo
			{
				get{ return m_parent; }
			}
			#endregion

			#region Class constructor
			private EventArgsBindDropDownControl(){}

			public EventArgsBindDropDownControl( CustomCombo parent, Form form )
			{
				m_parent = parent;
				m_frm = form;
			}

			public EventArgsBindDropDownControl( CustomCombo parent, Form form, Control ctrl )
			{
				m_parent = parent;
				m_frm  = form;
				m_ctrl = ctrl;
			}
			#endregion
		}

		public class EventArgsCloseDropDown : EventArgs
		{
			#region Class members
			private bool m_bClose;
			private Keys m_KeyCode;
			#endregion

			#region Class Properties
			public bool Close
			{
				get
				{
					return m_bClose;
				}
				set
				{
					m_bClose = value;
				}
			}
			public Keys KeyCode
			{
				get
				{
					return m_KeyCode;
				}
			}
			#endregion

			#region Class constructor
			private EventArgsCloseDropDown()
			{
      
			}

			public EventArgsCloseDropDown( bool close )
			{
				m_bClose = close;
			}
      
			public EventArgsCloseDropDown( bool close, Keys keycode )
			{
				m_bClose = close;
				m_KeyCode = keycode;
			}
			#endregion
		}

		public class EventArgsEditCustomSize : EventArgs
		{
			#region Class members
			private int iWidth;
			private int iHeight;
			private int iXPos;
			private int iYPos;
			#endregion

			#region Class Properties
			public int xPos
			{
				get
				{
					return iXPos;
				}
				set
				{
					iXPos = value;
				}
			}

			public int yPos
			{
				get
				{
					return iYPos;
				}
				set
				{
					iYPos = value;
				}
			}

			public int Width
			{
				get
				{
					return iWidth;
				}
				set
				{
					iWidth = value;
				}
			}

			public int Height
			{
				get
				{
					return iHeight;
				}
				set
				{
					iHeight = value;
				}
			}
			#endregion

			#region Class Constructors
			private EventArgsEditCustomSize(){}
			public EventArgsEditCustomSize( int x, int y, int width, int height )
			{
				iXPos = x;
				iYPos = y;
				iWidth = width;
				iHeight = height;
			}
      
			public EventArgsEditCustomSize( Rectangle rc )
			{
				iXPos = rc.X;
				iYPos = rc.Y;
				iWidth = rc.Width;
				iHeight = rc.Height;
			}
			#endregion
		}

        public class EventArgsValueChanged : EventArgs
        {
            #region Class members
            private string oldValue;
            private string newValue;
            #endregion

            #region Class Properties
            public string OldValue
            {
                get
                {
                    return oldValue;
                }
                set
                {
                    oldValue = value;
                }
            }

            public string NewValue
            {
                get
                {
                    return newValue;
                }
                set
                {
                    newValue = value;
                }
            }
            #endregion

            #region Class Constructors
            private EventArgsValueChanged(){}
            public EventArgsValueChanged(string oldVal, string newVal)
            {
                oldValue = oldVal;
                newValue = newVal;
            }
            #endregion
        }

 		public delegate void CloseDropDownHandler( object sender, EventArgsCloseDropDown e );
		public delegate void EditControlResizeHandler( object sender, EventArgsEditCustomSize e );
        public delegate void ValueChangedHandler( object sender, EventArgsValueChanged e);

		#endregion

		#region Class Events
		protected event EditControlResizeHandler CustomEditSize;
    
		public event CloseDropDownHandler     CloseDropDown;
		public event EventHandler             DropDownShown;
		public event EventHandler             DropDownHided;
		public event ValueChangedHandler      ValueChanged;
		#endregion

		#region Class Constructor
		public CustomCombo()
		{
			ControlStyles styleTrue = ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer |
				ControlStyles.FixedHeight |
				ControlStyles.ResizeRedraw |
				ControlStyles.UserPaint;

			SetStyle( styleTrue, true );
			SetStyle( ControlStyles.Selectable, false ); 

			m_editbox = new TextBox();
 			m_editbox.BorderStyle = BorderStyle.None;
			m_editbox.KeyDown += new KeyEventHandler( OnEditKeyDown );
			m_editbox.MouseWheel += new MouseEventHandler( OnEditMouseWheel );

			m_dropDownForm = new ShadowForm();
			m_dropDownForm.FormBorderStyle = FormBorderStyle.None;
			m_dropDownForm.StartPosition = FormStartPosition.Manual;
			m_dropDownForm.TopMost = true;
			m_dropDownForm.ShowInTaskbar = false;
			m_dropDownForm.BackColor = SystemColors.ControlLightLight;
			m_dropDownForm.Deactivate += new EventHandler( OnDropDownLostFocus );
			m_dropDownForm.Load += new EventHandler(OnDropFormLoad);
		 
			m_strFormat = new StringFormat();
			m_strFormat.Alignment = StringAlignment.Near;
			m_strFormat.LineAlignment = StringAlignment.Center;
			m_strFormat.FormatFlags = StringFormatFlags.LineLimit;
		}
    
		protected override void OnHandleCreated(System.EventArgs e)
		{
			base.OnHandleCreated( e );

			m_editbox.Parent = this;
            m_editbox.Size = new Size(ClientRectangle.Width - 20, ClientRectangle.Height - 4);
            int yPos = (ClientRectangle.Height - m_editbox.Height) / 2;
			m_editbox.Location = new Point( ClientRectangle.X + 4, ClientRectangle.Y + yPos );
		}

    	#endregion

		#region Class Mouse Hook Methods

        #region Interop
        delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        enum WindowsHookCodes
        {
            WH_MSGFILTER = (-1),
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SetWindowsHookEx(WindowsHookCodes hookid, HookProc pfnhook, IntPtr hinst, int threadid);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern bool UnhookWindowsHookEx(IntPtr hhook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        static extern int GetCurrentThreadId();

        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int x;
            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEHOOKSTRUCT
        {
            public POINT pt;
            public IntPtr hwnd;
            public int wHitTestCode;
            public IntPtr dwExtraInfo;
        }

        #endregion

        private void StartHook()
		{     
			// Mouse hook
			HookProc mouseHookProc = new HookProc(MouseHook);
			m_mouseProcHandle = GCHandle.Alloc(mouseHookProc);
			m_mouseHookHandle = SetWindowsHookEx(WindowsHookCodes.WH_MOUSE, 
				mouseHookProc, IntPtr.Zero, GetCurrentThreadId());
		}

		private void EndHook()
		{
			// Unhook   
			UnhookWindowsHookEx( m_mouseHookHandle );
			m_mouseProcHandle.Free();
			m_mouseHookHandle = IntPtr.Zero;
		}

		private IntPtr MouseHook(int code, IntPtr wparam, IntPtr lparam) 
		{
			MOUSEHOOKSTRUCT mh = (MOUSEHOOKSTRUCT )Marshal.PtrToStructure(lparam, typeof(MOUSEHOOKSTRUCT));
      
			// if user set focus on edit control embedded by us then do not open DropDown box
			if( mh.hwnd == Handle &&
                wparam == (IntPtr)0x0201 &&                     //WM_LBUTTONDOWN
				m_bDropDown == false && ReadOnly == false )
			{
				m_bSkipClick = true;
			}

			return CallNextHookEx( m_mouseHookHandle, code, wparam, lparam );
		}
		#endregion

		#region Class Overrides
		protected abstract void OnPrevScrollItems();
		protected abstract void OnNextScrollItems();
		protected abstract void OnDropDownControlBinding( EventArgsBindDropDownControl e );
		protected virtual  void OnValueChanged(string oldVal, string newVal)
		{
			RaiseValueChanged(oldVal, newVal);
		}

		protected virtual void OnDropDownSizeChanged()
		{
			if( m_dropDownForm != null )
			{
				m_dropDownForm.Size = new Size( ClientRectangle.Width, m_iDropDownHeight );
			}
		}

		protected virtual void OnDropDownFormLocation()
		{
			if( m_dropDownForm != null )
			{
				Rectangle pos = RectangleToScreen( ClientRectangle );
                Screen screen = Screen.FromControl(this);
                if(pos.Bottom + m_dropDownForm.Height > screen.Bounds.Bottom)
                    m_dropDownForm.Location = new Point(pos.X, pos.Top - m_dropDownForm.Height - 1);
                else
                    m_dropDownForm.Location = new Point( pos.X, pos.Bottom + 1 );
			}
		}
		protected virtual bool OnValueValidate( string value )
		{
			// TODO: implement values validation
			return true;
		}

		protected virtual void OnReadOnlyStateChanged()
		{
			if( m_editbox.ReadOnly == true )
			{
				m_bOwnValuePaint = true;
				m_editbox.Visible = false;
				SetStyle( ControlStyles.Selectable, true ); 
			}
			else
			{
				m_bOwnValuePaint = false;
				m_editbox.Visible = true;
				SetStyle( ControlStyles.Selectable, false ); 
			}
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_editbox.Dispose();
                m_dropDownForm.Dispose();
                if ((m_ctrlBinded != null) && !m_ctrlBinded.IsDisposed)
                {
                    m_ctrlBinded.Dispose();
                    m_ctrlBinded = null;
                }
            }

            base.Dispose(disposing);
        }
		#endregion
    
		#region Control Paint methods
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
            bool bVisualStyle = VisualStyleRenderer.IsSupported;
            OnPaintBackground(e, bVisualStyle);
            OnPaintComboButton(e, bVisualStyle);
			OnPaintEditItem( e );
			OnPaintCustomData( e );
		}

        protected virtual void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent, bool bVisualStyle)
        {
            Graphics g = pevent.Graphics;
            Rectangle rc = ClientRectangle;

            g.FillRectangle((Enabled == true) ? SystemBrushes.Window : SystemBrushes.Control, rc);

            if (bVisualStyle)
            {
                rc.Width -= 1;
                rc.Height -= 1;
                ControlPaint.DrawVisualStyleBorder(g, rc);
            }
            else
                ControlPaint.DrawBorder3D(g, rc, Border3DStyle.Sunken);
        }

        protected virtual void OnPaintComboButton(System.Windows.Forms.PaintEventArgs pevent, bool bVisualStyle)
        {
            Graphics g = pevent.Graphics;
            Rectangle rc = ClientRectangle;         

            bool bHot = m_clickButton.Contains(PointToClient(Cursor.Position)) && !DroppedDown;
            bool bPushed = bHot && ((int)MouseButtons != 0);

            if (bVisualStyle && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ComboBox.DropDownButton.Normal))
            {
                m_clickButton = new Rectangle(rc.Right - 18, rc.Y + 1, 17, rc.Height - 2);
                VisualStyleElement el = !Enabled ? VisualStyleElement.ComboBox.DropDownButton.Disabled :
                    bPushed ? VisualStyleElement.ComboBox.DropDownButton.Pressed :
                    bHot ? VisualStyleElement.ComboBox.DropDownButton.Hot :
                    VisualStyleElement.ComboBox.DropDownButton.Normal;
                VisualStyleRenderer renderer = new VisualStyleRenderer(el);
                renderer.DrawBackground(g, m_clickButton);
            }
            else
            {
                m_clickButton = new Rectangle(rc.Right - 18, rc.Y + 2, 16, rc.Height - 4);
                ControlPaint.DrawComboButton(g, m_clickButton, !Enabled ? ButtonState.Inactive : 
                    (bPushed ? ButtonState.Pushed : ButtonState.Normal));
            }
        }


		protected virtual void OnPaintEditItem( System.Windows.Forms.PaintEventArgs pevent )
		{
			Graphics g = pevent.Graphics;
			Rectangle rc = ClientRectangle;
      
			// if control not shown to user then change text centering
			int yPos = ClientRectangle.Y + ( ClientRectangle.Height - m_editbox.Height ) / 2;
			if( m_bOwnValuePaint == true ) yPos = ClientRectangle.Y + 2;
      
			int xPos = ClientRectangle.X + 2;
			int iWidth = ClientRectangle.Width - 20;
			int iHeight = ClientRectangle.Height - 4;
      
			EventArgsEditCustomSize ev = new EventArgsEditCustomSize( xPos, yPos, iWidth, iHeight );
			OnCustomEditSize( ev );

			m_editbox.Location = new Point( ev.xPos, ev.yPos );
			m_editbox.Size = new Size( ev.Width, ev.Height );

			if( m_bOwnValuePaint == true )
			{
				Rectangle rcOut = new Rectangle( ev.xPos, ev.yPos, ev.Width, ev.Height );
				SolidBrush brush = new SolidBrush( ForeColor );
				g.DrawString( Value, Font, brush, rcOut, m_strFormat );
				brush.Dispose();
			}
		}

		protected virtual void OnPaintCustomData( System.Windows.Forms.PaintEventArgs pevent )
		{
			// do nothing here by default
		}
		#endregion

		#region User input catchers methods

		protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
		{
			OnEditKeyDown( this, e );
		}

		protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
		{
			if( e.Delta < 0 )
			{
				OnPrevScrollItems();
			}
			else
			{
				OnNextScrollItems();
			}
		}

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            
            base.OnMouseDown(e);

            if (m_bSkipClick == false)
            {
                Point pnt = MousePosition;
                pnt = PointToClient(pnt);

                if ((m_clickButton.Contains(pnt) == true) ||
                    (m_editbox.ReadOnly == true && ClientRectangle.Contains(pnt))
                    )
                {
                    DroppedDown = true;
                }
            }

            m_bSkipClick = false;


            Invalidate();
        }
    
        //protected override void OnClick( System.EventArgs e )
        //{
        //    Focus();

        //    base.OnClick( e );

        //    if( m_bSkipClick == false )
        //    {
        //        Point pnt = MousePosition;
        //        pnt = PointToClient( pnt );

        //        if( ( m_clickButton.Contains( pnt ) == true ) || 
        //            ( m_editbox.ReadOnly == true && ClientRectangle.Contains( pnt ) )
        //            )
        //        {
        //            DroppedDown = true;
        //        }
        //    }

        //    m_bSkipClick = false;
        //}

		private void OnEditKeyDown( object sender, KeyEventArgs e )
		{
			if( e.Alt == true && e.KeyCode == Keys.Down && DroppedDown == false )
			{
				DroppedDown = true;
			}
			else if( (e.Modifiers & (Keys.Control | Keys.Shift | Keys.Alt)) == 0 )
			{
				if( e.KeyCode == Keys.F4 )
				{
					DroppedDown = !DroppedDown;
				}
				else if( e.KeyCode == Keys.Down )
				{
					OnNextScrollItems();
					e.Handled = true;
					m_editbox.SelectAll();
				}
				else if( e.KeyCode == Keys.Up )
				{
					OnPrevScrollItems();
					e.Handled = true;
					m_editbox.SelectAll();
				} 
			}
		}
		private void OnEditMouseWheel( object sender, MouseEventArgs e )
		{
			if( e.Delta < 0 )
			{
				OnPrevScrollItems();
			}
			else
			{
				OnNextScrollItems();
			}
		}

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Invalidate();
        }

        bool bOverButton = false;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (bOverButton != m_clickButton.Contains(e.X, e.Y))
            {
                bOverButton = !bOverButton;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (bOverButton)
            {
                bOverButton = false;
                Invalidate();
            }
            base.OnMouseLeave(e);
        }

		#endregion
    
		#region Show/Hide dropdown window methods
		protected virtual void ShowDropDownForm()
		{
			// create form on first click
			if( m_bFirstShow == true )
			{
				OnDropDownSizeChanged();
        
				if( m_bControlBinded == false )
				{
					EventArgsBindDropDownControl ev = new EventArgsBindDropDownControl( this, m_dropDownForm );
					OnDropDownControlBinding( ev );
					m_ctrlBinded = ev.BindedControl;
					m_bControlBinded = true;
				}

				m_ctrlBinded.Size = new Size( m_dropDownForm.Width-2, m_dropDownForm.Height-2 );
				m_ctrlBinded.Location = new Point( 1, 1 );
				m_ctrlBinded.Anchor = AnchorStyles.Bottom | 
					AnchorStyles.Left | 
					AnchorStyles.Right | 
					AnchorStyles.Top;
                
        
				m_ctrlBinded.Parent = m_dropDownForm;
				m_ctrlBinded.KeyDown += new KeyEventHandler( OnDropDownControlKeyDown );
				m_ctrlBinded.DoubleClick += new EventHandler( OnDropDownControlDoubleClick );
				m_bFirstShow = false;
			}

			OnDropDownFormLocation();
      
			// if control has smaller size and cannot be resized then resize form
			if( (m_ctrlBinded.Height + 2) < m_dropDownForm.Height )
				m_dropDownForm.Height = m_ctrlBinded.Height + 2;

			m_ctrlBinded.Focus();
			StartHook();

            m_dropDownForm.Show();
      
			RaiseDropDownShown();
		}

		protected virtual void OnDropFormLoad(object sender, EventArgs e)
		{
			m_ctrlBinded.Parent = m_dropDownForm;
		}

		protected virtual void HideDropDownForm()
		{
			if( m_dropDownForm != null )
			{
				m_dropDownForm.Hide();
				EndHook();

				RaiseDropDownHided();

				if( m_bCanInvalidate == true )
					Invalidate();
			}
		}
		#endregion

		#region Class Helper methods
		protected void RaiseCloseDropDown( EventArgsCloseDropDown e )
		{
			if( CloseDropDown != null )
			{
				CloseDropDown( this, e ); 
			}
		}

		protected virtual void OnCustomEditSize( EventArgsEditCustomSize e )
		{
			if( CustomEditSize != null )
			{
				CustomEditSize( this, e );
			}
		}
		protected void RaiseDropDownShown()
		{
			if( DropDownShown != null )
			{
				DropDownShown( this, EventArgs.Empty );
			}
		}

		protected void RaiseDropDownHided()
		{
			if( DropDownHided != null )
			{
				DropDownHided( this, EventArgs.Empty );
			}
		}
		protected void RaiseValueChanged(string oldVal, string newVal)
		{
			if( ValueChanged != null )
			{
				ValueChanged( this, new EventArgsValueChanged(oldVal, newVal) );
			}
		}
		#endregion

		#region Drop Down Event handlers
		private void OnDropDownLostFocus( object sender, EventArgs e )
		{
			DroppedDown = false;
		}

		private void OnDropDownControlKeyDown( object sender, KeyEventArgs e )
		{
			if( e.Alt == true && ( e.KeyCode == Keys.Down || e.KeyCode == Keys.Up ) )
			{
				DroppedDown = false;
			}
			else if( ( e.Modifiers & (Keys.Shift | Keys.Alt | Keys.Control) ) == 0 )
			{
				EventArgsCloseDropDown ev = new EventArgsCloseDropDown( true, e.KeyCode );

				if( e.KeyCode == Keys.Escape )
				{
					DroppedDown = false;
				} 
				else if( e.KeyCode == Keys.F4 )
				{
					RaiseCloseDropDown( ev );
					DroppedDown = !ev.Close;
				}
				else if( e.KeyCode == Keys.Enter )
				{
					RaiseCloseDropDown( ev );
					DroppedDown = !ev.Close;
				}
			}
		}

		private void OnDropDownControlDoubleClick( object sender, EventArgs e )
		{
			if( ( ModifierKeys & (Keys.Shift | Keys.Alt | Keys.Control) ) == 0 )
			{
				EventArgsCloseDropDown ev = new EventArgsCloseDropDown( true );
				RaiseCloseDropDown( ev );
				DroppedDown = !ev.Close;
			}
		}
		#endregion

		#region Class Public methods
		public void BeginUpdate()
		{
			m_bCanInvalidate = false;
		}

		public void EndUpdate()
		{
			m_bCanInvalidate = true;
		}
		#endregion
	}
}
