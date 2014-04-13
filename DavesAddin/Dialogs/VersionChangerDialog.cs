using System;
using DavesAddin.Data;
using MonoDevelop.Projects;
using MonoDevelop.Core.ProgressMonitoring;
using System.Threading.Tasks;
using DavesAddin.Processors;

namespace DavesAddin.Dialogs
{
	public partial class VersionChangerDialog : Gtk.Dialog
	{
		private SolutionVersion mVersionData;
		private Solution mSolution;

		public VersionChangerDialog ()
		{
			this.Build ();
		}

		public VersionChangerDialog (SolutionVersion VersionData, Solution MainSolution) 
			: this ()
		{
			mVersionData = VersionData;
			mSolution = MainSolution;

			edtSolVersion.Text = VersionData.Version.ToString ();

			if (VersionData.HasIOSProjects)
			{
				edtiOSShort.Text = iOSAppVersion.ToShortVersion (VersionData.Version);
				edtiOSShort.Sensitive = true;
				lblIOS.Sensitive = true;

			}

			if (VersionData.HasAndroidProjects)
			{
				edtAndroidBuild.Text = AndroidAppVersion.ToBuild (VersionData.Version);
				edtAndroidBuild.Sensitive = true;
				lblAndroid.Sensitive = true;
			}

			btnOk.Clicked += (object sender, EventArgs e) => {

				SolutionProcessor.UpdateVersions (edtSolVersion.Text, null, mVersionData, mSolution);

			};
		}
	}
}

