using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ibaFilesLiteLib;

namespace Alunorf_roh_plugin
{
    public partial class SelectInfoOrChannels : Form
    {
        public SelectInfoOrChannels()
        {
            InitializeComponent();
            AdditionalInfos = new SortedDictionary<string, ExtraData>();
        }

        private bool m_selectChannels;

        public bool SelectChannels
        {
            get { return m_selectChannels; }
            set { m_selectChannels = value; }
        }

        private string m_datFile;

        public string DatFile
        {
            get { return m_datFile; }
            set { m_datFile = value; }
        }

        public SortedDictionary<string, ExtraData> AdditionalInfos;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                IbaFileReader ibaFile = new IbaFileClass();
                ibaFile.Open(m_datFile);
                SortedDictionary<int, ExtraData> vectorExtraDatas = new SortedDictionary<int, ExtraData>();

                if (m_selectChannels)
                {
                    for (int i = 0; true; i++) //get the vectors
                    {
                        String name;
                        String value;
                        ibaFile.QueryInfoByIndex(i, out name, out value);
                        if (String.IsNullOrEmpty(name) && String.IsNullOrEmpty(value)) break; //no more infofields
                        if (name.StartsWith("Vector_name_", StringComparison.InvariantCultureIgnoreCase))
                        {
                            ExtraData ed = new ExtraData();
                            ed.unit = ""; //fill in later
                            ed.dt = "F";
                            ed.description = value.Length > 30 ? value.Substring(0, 30) : value; //might be replaced later
                            ed.kurz = value.Length > 8 ? value.Substring(0, 8) : value;
                            ed.kurz = ed.kurz.Replace(' ', '_');
                        	int vecIndex;
					        if (Int32.TryParse(name.Substring(12),out vecIndex))
					        {
                                vectorExtraDatas.Add(vecIndex,ed);
					        }
                            AdditionalInfos.Add(value, ed);
                            m_lbIba.Items.Add(value);
                        }
                    }
                    //iterate over channels
                    IbaEnumChannelReader enumerator = ibaFile.EnumChannels() as IbaEnumChannelReader;
			        while (enumerator.IsAtEnd()==0)
			        {
				        IbaChannelReader reader = enumerator.Next() as IbaChannelReader;
                        string name = reader.QueryInfoByName("name");
                        string vector = reader.QueryInfoByName("vector");
                        if (!String.IsNullOrEmpty(vector))
                        {
					        int pos = vector.IndexOf(".");
                            int vecIndex;
                            if (pos > 0 && Int32.TryParse(vector.Substring(0, pos), out vecIndex))
                            {
                                if (vectorExtraDatas.ContainsKey(vecIndex))
                                {
                                    ExtraData ed = vectorExtraDatas[vecIndex];
                                    ed.dt = "F";
                                    string description = reader.QueryInfoByName("$PDA_comment1");
                                    if (!string.IsNullOrEmpty(description))
                                        ed.description = description.Length > 30 ? description.Substring(0, 30) : description;
                                    string kurz = reader.QueryInfoByName("$PDA_comment2");
                                    if (!string.IsNullOrEmpty(kurz))
                                    {
                                        ed.kurz = kurz.Length > 8 ? kurz.Substring(0, 8) : kurz;
                                        ed.kurz = ed.kurz.Replace(' ', '_');
                                    }
                                    vectorExtraDatas.Remove(vecIndex); //pass for further vector elements
                                }
                            }
                        }
                        else if (reader.QueryInfoByName("hidden")!="1")
                        {
                            if (String.IsNullOrEmpty(name)) continue;
                            ExtraData ed = new ExtraData();
                            ed.unit = reader.QueryInfoByName("unit");
                            ed.dt = "F";
                            string description = reader.QueryInfoByName("$PDA_comment1");
                            if (!string.IsNullOrEmpty(description))
                                ed.description = description.Length > 30 ? description.Substring(0, 30) : description;
                            else if (String.IsNullOrEmpty(name)) continue;
                            else
                                ed.description = name.Length > 30 ? name.Substring(0, 30) : name;
                            string kurz = reader.QueryInfoByName("$PDA_comment2");
                            if (!string.IsNullOrEmpty(kurz))
                            {
                                ed.kurz = kurz.Length > 8 ? kurz.Substring(0, 8) : kurz;
                            }
                            else if (String.IsNullOrEmpty(name)) continue;
                            else
                                ed.kurz = name.Length > 8 ? name.Substring(0, 8) : name;
                            ed.kurz = ed.kurz.Replace(' ', '_');
                            AdditionalInfos.Add(name, ed);
                            m_lbIba.Items.Add(name);
                        }
                    }
                }
                else
                {
                    for (int i = 0; true; i++)
			        {
				        String name;
				        String value;
				        ibaFile.QueryInfoByIndex(i, out name, out value);
				        if (String.IsNullOrEmpty(name) && String.IsNullOrEmpty(value)) break; //no more infofields
                        m_lbIba.Items.Add(name);
                        ExtraData ed = new ExtraData();
                        ed.unit = "";
                        ed.description = name.Length>30?name.Substring(0,30):name;
                        ed.kurz = name.Length>8?name.Substring(0,8):name;
                        ed.kurz = ed.kurz.Replace(' ','_');
                        int dummyint;
                        double dummyd;
                        if (Int32.TryParse(value, out dummyint))
                            ed.dt = "I";
                        else if (Double.TryParse(value, out dummyd))
                            ed.dt = "F";
                        else
                            ed.dt = "C";
                        AdditionalInfos.Add(name,ed);
			        }
                }
                ibaFile.Close();
            }
            catch
            {
                MessageBox.Show(String.Format(Alunorf_roh_plugin.Properties.Resources.DatFileCouldNotBeOpened, m_datFile), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        public string[] SelectedItems()
        {
            string[] results = new string[m_lbRoh.Items.Count];
            for (int i = 0; i < results.Length; i++)
                results[i] = m_lbRoh.Items[i] as string;
            return results;
        }

        private void m_btAdd_Click(object sender, EventArgs e)
        {
            foreach (string s in m_lbIba.SelectedItems)
            {
                m_lbRoh.Items.Add(s);
            }
            while (m_lbIba.SelectedItems.Count > 0)
            {
                m_lbIba.Items.Remove(m_lbIba.SelectedItem);
            }
        }

        private void m_btRemove_Click(object sender, EventArgs e)
        {
            foreach (string s in m_lbRoh.SelectedItems)
            {
                m_lbIba.Items.Add(s);
            }
            while (m_lbRoh.SelectedItems.Count > 0)
            {
                m_lbRoh.Items.Remove(m_lbRoh.SelectedItem);
            }
        }
    }

    public class ExtraData
    {
        public string kurz;
        public string description;
        public string unit;
        public string dt;
    }

}
