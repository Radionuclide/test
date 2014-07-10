﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iba.Controls
{
    public partial class WeeklyTriggerSettingsControl : UserControl
    {
        CheckBox[] cbList;
        public WeeklyTriggerSettingsControl()
        {
            InitializeComponent();
            cbList = new CheckBox[] {m_cbSunday,m_cbMonday, m_cbTuesday, m_cbWednesday, m_cbThursday, m_cbFriday, m_cbSaturday};
            List<string> daynames = new List<String>(System.Globalization.DateTimeFormatInfo.CurrentInfo.DayNames.Take(7));
            for(int i = 0; i < 7; i++)
                cbList[i].Text = daynames[i];
        }

        public void SetDaysFromList(List<int> list)
        {
            foreach (CheckBox cb in cbList) 
                cb.Checked = false;
            foreach (int i in list)
                cbList[i].Checked = true;
        }

        public List<int> GetListFromDays()
        {
            return new List<int>(cbList.Select((cb, index) => new { cb, index }).Where(z => z.cb.Checked).Select(z => z.index));
        }
    }
}
