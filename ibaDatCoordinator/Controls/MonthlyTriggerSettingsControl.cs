using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using iba.Utility;

namespace iba.Controls
{
    public partial class MonthlyTriggerSettingsControl : UserControl
    {
        public MonthlyTriggerSettingsControl()
        {
            InitializeComponent();
        }
    }

    public class CustomComboMonths : CustomCombo
    {
        protected override void OnPrevScrollItems()
        {
            return;
        }

        protected override void OnNextScrollItems()
        {
            return;
        }

        protected override void OnDropDownControlBinding(EventArgsBindDropDownControl e)
        {
            FlowLayoutPanel fp = new FlowLayoutPanel();
            fp.AutoSize = true;
            fp.FlowDirection = FlowDirection.TopDown;
            CheckBox cb = new CheckBox();
            cb.Text = iba.Properties.Resources.SelectAllMonths;
            fp.Controls.Add(cb);
            for(int i = 0; i < 12; i++)
            {
                cb = new CheckBox();
                cb.Text = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames[i];
                fp.Controls.Add(cb);
            }
            e.BindedControl = fp;
        }
    }

    public class CustomComboDays : CustomCombo
    {
        protected override void OnPrevScrollItems()
        {
            return;
        }

        protected override void OnNextScrollItems()
        {
            return;
        }

        protected override void OnDropDownControlBinding(EventArgsBindDropDownControl e)
        {
            FlowLayoutPanel fp = new FlowLayoutPanel();
            fp.AutoSize = true;
            fp.FlowDirection = FlowDirection.TopDown;
            fp.MaximumSize = new Size(1000, 100);
            for(int i = 1; i <= 31; i++)
            {
                CheckBox cb = new CheckBox();
                cb.Text = i.ToString();
                fp.Controls.Add(cb);
            }
            CheckBox cbLast = new CheckBox();
            cbLast.Text = iba.Properties.Resources.Last;
            fp.Controls.Add(cbLast);
            e.BindedControl = fp;
        }
    }

    public class CustomComboOnPart1 : CustomCombo
    {
        protected override void OnPrevScrollItems()
        {
            return;
        }

        protected override void OnNextScrollItems()
        {
            return;
        }

        protected override void OnDropDownControlBinding(EventArgsBindDropDownControl e)
        {
            FlowLayoutPanel fp = new FlowLayoutPanel();
            fp.AutoSize = true;
            fp.FlowDirection = FlowDirection.TopDown;
            CheckBox cb = new CheckBox();
            cb.Text = iba.Properties.Resources.First;
            fp.Controls.Add(cb);   
            cb = new CheckBox();
            cb.Text = iba.Properties.Resources.Second;
            fp.Controls.Add(cb);
            cb = new CheckBox();
            cb.Text = iba.Properties.Resources.Third;
            fp.Controls.Add(cb);
            cb = new CheckBox();
            cb.Text = iba.Properties.Resources.Fourth;
            fp.Controls.Add(cb);
            cb = new CheckBox();
            cb.Text = iba.Properties.Resources.Last;
            fp.Controls.Add(cb);
            e.BindedControl = fp;
        }
    }

    public class CustomComboOnWeekdays : CustomCombo
    {
        protected override void OnPrevScrollItems()
        {
            return;
        }

        protected override void OnNextScrollItems()
        {
            return;
        }

        protected override void OnDropDownControlBinding(EventArgsBindDropDownControl e)
        {
            FlowLayoutPanel fp = new FlowLayoutPanel();
            fp.AutoSize = true;
            fp.FlowDirection = FlowDirection.TopDown;
            CheckBox cb = new CheckBox();
            cb.Text = iba.Properties.Resources.SelectAllDays;
            fp.Controls.Add(cb);
            for(int i = 0; i < 7; i++)
            {
                cb = new CheckBox();
                cb.Text = System.Globalization.DateTimeFormatInfo.CurrentInfo.DayNames[i];
                fp.Controls.Add(cb);
            }
            e.BindedControl = fp;
        }
    }

}
