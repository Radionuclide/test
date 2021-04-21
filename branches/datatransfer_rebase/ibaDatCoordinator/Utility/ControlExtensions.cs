using System;
using System.Windows.Forms;

namespace iba.Utility
{
    public static class NumericUpDownHelper
    {
        public static int GetIntValue(this NumericUpDown spinner)
        {
            return Convert.ToInt32(spinner.Value);
        }

        public static void SetIntValue(this NumericUpDown spinner, int value)
        {
            spinner.Value = Math.Max(spinner.Minimum, Math.Min(spinner.Maximum, value));
        }
    }
}
