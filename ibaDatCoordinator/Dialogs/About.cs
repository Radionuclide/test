using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

namespace iba.Dialogs
{
	public partial class About : Form
	{
		public About()
        {
            InitializeComponent();

            //build the version string your way.
            var versionString = DatCoVersion.GetVersion();
            //var versionString = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            //var versionString = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

            picIcon.Image = iba.Properties.Resources.About_Image_80x80;
            lblProduct.Text = String.Format("ibaDatCoordinator v{0}", versionString);
            //adjust copyright label position for multiline ProductText 
            lblCopyRight.Top = lblProduct.Top + lblProduct.Height;

            //custom detail label text
            //change visisbility for lblCaptionX and lblDetailX in designer/code where needed
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                lblDetail1.Visible = false;
                lblCaption1.Visible = false;
            }
            else
            {
                lblDetail1.Text = "?";
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && Program.CommunicationObject != null && Program.CommunicationObject.TestConnection())
                {
                    lblDetail1.Text = Program.CommunicationObject.GetVersion();
                }
            }

            try
            {
                IbaAnalyzer.IbaAnalysis MyIbaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                lblDetail2.Text = MyIbaAnalyzer.GetVersion().Remove(0, 12);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(MyIbaAnalyzer);
            }
            catch
            {
                lblDetail2.Text = "?";
            }

            try
            {
                ibaFilesLiteDotNet.IbaFileReader myIbaFile = new ibaFilesLiteDotNet.IbaFileReader();
                lblDetail3.Text= myIbaFile.GetVersion();
                myIbaFile.Dispose();
            }
            catch
            {
                lblDetail3.Text = "?";
            }


            //lblDetail4.Text = "?";
            //lblDetail5.Text = "?";

            linkLicenses.LinkArea = new LinkArea(0, linkLicenses.Text.Length);
            linkAgreement.LinkArea = new LinkArea(0, linkAgreement.Text.Length);
        }

        private void btOk_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void linkLicenses_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
                Process.Start(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "LicenseInformation.txt"));
			}
			catch (Exception)
			{
			}
		}

		private void linkAgreement_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
                Process.Start(Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), "License_ibaDatCoordinator.pdf"));
			}
			catch (Exception)
			{
			}
		}

    }
}
