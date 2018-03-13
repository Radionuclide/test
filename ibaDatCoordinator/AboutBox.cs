using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using iba.Processing;
using iba.Utility;
using ibaFilesLiteLib;

namespace iba
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            //set various versions
            m_vDATC.Text = "v" + DatCoVersion.GetVersion();
            string fileName = PathUtil.GetAbsolutePath("ibaLogger.dll");
            System.Reflection.AssemblyName asName = System.Reflection.AssemblyName.GetAssemblyName(fileName);
            m_vLogger.Text = "v" + asName.Version.ToString();
            try
            {
                IbaFileClass myIbaFile = new IbaFileClass();
                m_vFILES.Text = "v" + myIbaFile.GetVersion();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myIbaFile);
            }
            catch
            {
                m_vFILES.Text = "?";
            }
            try
            {
                IbaAnalyzer.IbaAnalysis MyIbaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                m_vANAL.Text = "v" + MyIbaAnalyzer.GetVersion().Remove(0, 12);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(MyIbaAnalyzer);
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