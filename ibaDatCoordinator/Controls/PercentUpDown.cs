
namespace iba.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.Diagnostics;
    using System.Globalization;

    public class PercentUpDown : NumericUpDown
    {

        private static readonly Decimal DefaultValue = 0m;      // 0%
        private static readonly Decimal DefaultMinimum = 0m;    // 0%
        private static readonly Decimal DefaultMaximum = 100m;  // 100%
        private static readonly Decimal DefaultIncrement = 1m;  // 1%

        public PercentUpDown()
        {
            Value = DefaultValue;
            Minimum = DefaultMinimum;
            Maximum = DefaultMaximum;
            Increment = DefaultIncrement;
        }

        protected override void UpdateEditText()
        {
            Text = Value.ToString(String.Format("f{0}", DecimalPlaces)) + "%";
        }

    }
}
