using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace iba.Controls
{
	public class GradientWaitingBar : System.Windows.Forms.Control
	{
		public GradientWaitingBar()
		{
			// Set the control styles
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			// Set the default size of this control
			this.Left = 0;
			this.Top = 0;
			this.Width = DEF_WIDTH;
			this.Height = DEF_HEIGHT;

			InitializeComponent();

			this.m_timer.Enabled = this.Enabled;
			m_col1 = Color.White;
			m_col2 = Color.Blue;

			this.BackColor = m_col1;

			// Make a gradient Bitmap
			MakeBitmapForWaitingBar();			
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.m_timer = new System.Windows.Forms.Timer(this.components);

			this.m_timer.Tick += new System.EventHandler(this.m_timer_Tick);
			this.m_timer.Interval = 10;
		}

		#region "Variables"
		const int DEF_WIDTH = 50;
		const int DEF_HEIGHT = 18;

		private System.Windows.Forms.Timer m_timer;
		private System.ComponentModel.IContainer components;
		private System.Drawing.Color m_col1, m_col2;
		private System.Drawing.Bitmap m_bitmap = null;
		private int xloc = 0;
		private int m_nSpeed = 5;
		private SCROLLGRADIENTALIGN m_nScrollway = SCROLLGRADIENTALIGN.HORIZONTAL;

		// enum variable of align this control
		public enum SCROLLGRADIENTALIGN
		{
			HORIZONTAL = 0,
			VERTICAL
		};
		
		#endregion

		#region "Properties"

		// property of timer
		[System.ComponentModel.Description("Gets or sets the Interval of timer tick count for Speed of GradientWaitingBar"), System.ComponentModel.Category("Behavior")]
		public int Interval
		{
			get
			{
				return m_timer.Interval;
			}
			set
			{
				m_timer.Interval = value;
			}
		}

		// property of speed in scrolling.
		[System.ComponentModel.Description("Gets or sets the speed of gradient for GradientWaitingBar"), System.ComponentModel.Category("Behavior")]
		public int Speed
		{
			get
			{
				return m_nSpeed;
			}
			set
			{
				m_nSpeed = value;
			}
		}
		// property of begin color in linear gradient
		[System.ComponentModel.Description("Gets or sets the starting color of the gradient for GradientWaitingBar"), System.ComponentModel.Category("Appearance")]
		public Color GradientColor1
		{
			get
			{
				return m_col1;
			}
			set
			{
				m_col1 = value;
				MakeBitmapForWaitingBar();
			}
		}
		
		// property of end color in linear gradient
		[System.ComponentModel.Description("Gets or sets the ending color of the gradient for GradientWaitingBar"), System.ComponentModel.Category("Appearance")]
		public Color GradientColor2
		{
			get
			{
				return m_col2;
			}
			set
			{
				m_col2 = value;
				MakeBitmapForWaitingBar();
			}
		}
		
		// property of scrolling direction in control
		[System.ComponentModel.Description("Gets or sets the direction of scrolling the gradient"), System.ComponentModel.Category("Appearance")]
		public SCROLLGRADIENTALIGN ScrollWAY
		{
			get
			{
				return m_nScrollway;
			}
			set
			{
				m_nScrollway = value;
				MakeBitmapForWaitingBar();
			}
		}		
		
		#endregion
	
		#region "Override Functions"

		// if control is disabled, we do not display the gradient.
		protected override void OnEnabledChanged(EventArgs e)
		{
			m_timer.Enabled = this.Enabled;
			base.OnEnabledChanged (e);
		}

		// On Size changed, we make a new bitmap of current size
		protected override void OnSizeChanged(EventArgs e)
		{	
			MakeBitmapForWaitingBar();
			base.OnSizeChanged (e);
		}

		// scroll the gradient bitmap
		protected override void OnPaint(PaintEventArgs e)
		{
			if(m_bitmap != null)
			{
				Graphics g = e.Graphics;
				Rectangle rt2;

				rt2 = this.ClientRectangle;
				
				if(m_nScrollway == SCROLLGRADIENTALIGN.HORIZONTAL)
				{
					rt2.X = rt2.Width - xloc;
					rt2.Width = xloc;

					g.DrawImage(m_bitmap, xloc, 0, this.ClientRectangle, GraphicsUnit.Pixel);
					g.DrawImage(m_bitmap, 0, 0, rt2, GraphicsUnit.Pixel);
				}
				else
				{
					rt2.Y = rt2.Height - xloc;
					rt2.Height = xloc;

					g.DrawImage(m_bitmap, 0, xloc, this.ClientRectangle, GraphicsUnit.Pixel);
					g.DrawImage(m_bitmap, 0, 0, rt2, GraphicsUnit.Pixel);
				}
			}
			base.OnPaint (e);
		}
		#endregion

		private void m_timer_Tick(object sender, System.EventArgs e)
		{
			xloc += m_nSpeed;
			if(m_nScrollway == SCROLLGRADIENTALIGN.HORIZONTAL)
			{
				if(xloc > this.ClientSize.Width)
					xloc = 0;
			}
			else
			{
				if(xloc > this.ClientSize.Height)
					xloc = 0;
			}
			Invalidate();
		}


		//
		// Make a Gradient Bitmap
		// For performance, on sizing and changed Properties first makes a suitable bitmap image of gradient
		// and then On paint this control uses it.
		//
		private void MakeBitmapForWaitingBar()
		{
			//check the windows size
			if(this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
				return;
			if(m_bitmap != null)
				m_bitmap.Dispose();

			Graphics gimage = null, gWnd = null;
			Brush br1, br2;
			Rectangle rt1, rt2;

			// Get temporary DC and make a compatible bitmap with current Windows.
			gWnd = Graphics.FromHwnd(this.Handle);
			m_bitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height, gWnd);
			gWnd.Dispose();
			
			if(m_nScrollway == SCROLLGRADIENTALIGN.HORIZONTAL)
			{
				rt1 = this.ClientRectangle;
				rt1.Width = this.ClientRectangle.Width/2+1;

				rt2 = rt1;
				rt2.X = this.ClientRectangle.Width/2;
				rt2.Width = this.ClientRectangle.Width/2+1;
				br1 = new LinearGradientBrush(rt1, m_col1, m_col2, LinearGradientMode.Horizontal);
				br2 = new LinearGradientBrush(rt2, m_col2, m_col1, LinearGradientMode.Horizontal);
			}
			else
			{
				rt1 = this.ClientRectangle;
				rt1.Height = this.ClientRectangle.Height/2 + 1;

				rt2 = rt1;
				rt2.Y = this.ClientRectangle.Height / 2;
				rt2.Height = this.ClientRectangle.Height/2 + 1;
				br1 = new LinearGradientBrush(rt1, m_col1, m_col2, LinearGradientMode.Vertical);
				br2 = new LinearGradientBrush(rt2, m_col2, m_col1, LinearGradientMode.Vertical);
			}

			// make a new bitmap
			gimage = Graphics.FromImage(m_bitmap);
			gimage.FillRectangle(br2, rt2);
			gimage.FillRectangle(br1, rt1);
			gimage.Dispose();
		}
	}
}