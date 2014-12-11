using System;
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
            if(System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek != DayOfWeek.Sunday)
            {
                var OrigLocs = cbList.Select(x => x.Location).ToArray();
                var OrigTabs = cbList.Select(x => x.TabIndex).ToArray();
                Point tpoint = cbList[6].Location;
                int shift = (int)System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;
                for(int i = 0; i < 7; i++)
                {
                    cbList[(i + shift) % 7].Location = OrigLocs.ElementAt(i);
                    cbList[(i + shift) % 7].TabIndex = OrigTabs.ElementAt(i);
                }
            }
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
