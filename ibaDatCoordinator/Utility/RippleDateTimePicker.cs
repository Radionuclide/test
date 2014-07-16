using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace iba.Utility
{
    /// <summary>
    /// DateTimePicker that ripples through: increasing by 1
    /// minute in '10h59' becomes '11h00' instead of '10h00'.
    /// </summary>
    public class RippleDateTimePicker : DateTimePicker
    {
        int changeMode;
        DateTime prevTime;
        bool internalChange;
        bool bSwapDelta;

        public RippleDateTimePicker()
        {
            changeMode = 0;
            prevTime = Value;
            internalChange = false;
            bSwapDelta = !IsActiveX && (Environment.OSVersion.Version.Major >= 6);
        }

        public static bool IsActiveX = false;

        #region win32

        [StructLayout(LayoutKind.Sequential)]
        struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int code;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct NMUPDOWN
        {
            public NMHDR hdr;
            public int pos;
            public int delta;
        }

        #endregion

        #region ripple

        void CheckRippleThrough()
        {
            internalChange = true;

            if (changeMode == 1 && Value < prevTime)
                RippleThrough(Value - prevTime, true);

            else if (changeMode == -1 && Value > prevTime)
               RippleThrough(Value - prevTime, false);

            internalChange = false;
        }


        void RippleThrough(TimeSpan span, bool increase)
        {
            DateTime newValue = Value;

            if (span < TimeSpan.FromTicks(0))
                span = -span;

            if (span < TimeSpan.FromMinutes(1))
                newValue += increase ? TimeSpan.FromMinutes(1) : -TimeSpan.FromMinutes(1);
            else if (span < TimeSpan.FromHours(1))
                newValue += increase ? TimeSpan.FromHours(1) : -TimeSpan.FromHours(1);
            else if (span < TimeSpan.FromDays(1))
                newValue += increase ? TimeSpan.FromDays(1) : -TimeSpan.FromDays(1);
            else if (span < TimeSpan.FromDays(31))
            {
                span = TimeSpan.FromDays(DateTime.DaysInMonth(Value.Year, Value.Month));
                newValue += increase ? span : -span;
            }
            else if (span < TimeSpan.FromDays(366))
            {
                span = TimeSpan.FromDays(DateTime.IsLeapYear(Value.Year) ? 366 : 365);
                newValue += increase ? span : -span;
            }

            if (newValue < MinDate)
                newValue = MinDate;
            else if (newValue > MaxDate)
                newValue = MaxDate;

            Value = newValue;
        }

        #endregion

        #region overrides

        /// <summary>
        /// Catch up/down button pressed message
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x204e || m.Msg == 0x4e)
            {
                NMHDR nfy = (NMHDR)Marshal.PtrToStructure(m.LParam, typeof(NMHDR));

                if (nfy.code == -722)
                {
                    NMUPDOWN ud = (NMUPDOWN)Marshal.PtrToStructure(m.LParam, typeof(NMUPDOWN));
                    if(bSwapDelta)
                        changeMode = ud.delta < 0 ? -1 : 1;
                    else
                        changeMode = ud.delta < 0 ? 1 : -1;
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            changeMode = 0;
            base.OnMouseUp(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                changeMode = 1;
            else if (e.KeyCode == Keys.Down)
                changeMode = -1;

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            changeMode = 0;
            base.OnKeyUp(e);
        }

        protected override void OnValueChanged(EventArgs eventargs)
        {
            if (!internalChange && changeMode != 0)
                CheckRippleThrough();

            prevTime = Value;

            base.OnValueChanged(eventargs);
        }

        #endregion
    }
}
