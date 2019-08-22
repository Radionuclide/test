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
            m_cbMonths.Init((new string[] { iba.Properties.Resources.SelectAllMonths }).Concat(System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.MonthNames.Take(12)), true);
            m_cbDays.min = 1;
            m_cbDays.max = 31;
            m_cbDays.Init(Enumerable.Range(1, 31).Select(n => n.ToString()).Concat(new string[] { iba.Properties.Resources.Last }), false);
            m_cbOnPart1.Init(new string[] { iba.Properties.Resources.First, iba.Properties.Resources.Second, iba.Properties.Resources.Third, iba.Properties.Resources.Fourth, iba.Properties.Resources.Last },false);
            m_cbOnPartWeekday.Init((new string[] { iba.Properties.Resources.SelectAllDays }).Concat(System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.DayNames.Take(7)), true);
            m_cbOnPartWeekday.DropDownHeigth = 100;
        }

        private void m_rbDays_CheckedChanged(object sender, EventArgs e)
        {
            m_cbDays.Enabled = m_rbDays.Checked;
        }

        private void m_rbOn_CheckedChanged(object sender, EventArgs e)
        {
            m_cbOnPart1.Enabled = m_cbOnPartWeekday.Enabled = m_rbOn.Checked;
        }
    }
}
