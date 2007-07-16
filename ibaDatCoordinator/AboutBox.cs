using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using iba.Processing;
using iba.Utility;
using IBAFILESLib;

namespace iba
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            //set various versions
            m_vDATC.Text = "v" + GetType().Assembly.GetName().Version.ToString(3);
            string fileName = PathUtil.GetAbsolutePath("ibaLogger.dll");
            System.Reflection.AssemblyName asName = System.Reflection.AssemblyName.GetAssemblyName(fileName);
            m_vLogger.Text = "v" + asName.Version.ToString();
            try
            {
                m_vFILES.Text = "v" + (new IbaFileClass()).GetType().Assembly.GetName().Version.ToString();
            }
            catch
            {
                m_vFILES.Text = "?";
            }
            try
            {
                m_vANAL.Text = "v" + (new IbaAnalyzer.IbaAnalysisClass()).GetVersion().Remove(0, 12);
            }
            catch
            {
                m_vANAL.Text = "?";
            }
            try
            {
                m_vICSharpTextEditor.Text = "v" + typeof(ICSharpCode.TextEditor.TextEditorControl).Assembly.GetName().Version.ToString();;
            }
            catch
            {
                m_vICSharpTextEditor.Text = "?";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void m_textEditorUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(iba.Properties.Resources.ICSharpTextEditorUrl);
            }
            catch
            {
            }
        }
    }
}