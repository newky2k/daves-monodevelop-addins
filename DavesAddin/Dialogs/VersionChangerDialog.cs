using System;
using DavesAddin.Data;
using MonoDevelop.Projects;
using MonoDevelop.Core.ProgressMonitoring;

namespace DavesAddin.Dialogs
{
	public partial class VersionChangerDialog : Gtk.Dialog
	{
		public VersionChangerDialog ()
		{
			this.Build ();
		}

		public VersionChangerDialog (SolutionVersion VersionData, Solution MainSolution) 
			: this ()
		{
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

				MainSolution.Version = edtSolVersion.Text;


				MainSolution.Save (new NullProgressMonitor ());
			};
		}
	}
}

