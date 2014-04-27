using System;
using DavesAddin.Data;
using MonoDevelop.Projects;
using MonoDevelop.Core.ProgressMonitoring;
using System.Threading.Tasks;
using DavesAddin.Processors;
using System.Collections.Generic;
using Gdk;

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

				var aDict = new Dictionary<String,String> ();

				if (edtAndroidBuild.Sensitive)
				{
					aDict.Add ("android", edtAndroidBuild.Text);
				}

				if (edtiOSShort.Sensitive)
				{
					aDict.Add ("cocoa", edtiOSShort.Text);
				}

				SolutionProcessor.UpdateVersions (edtSolVersion.Text, aDict, mVersionData, mSolution);

			};
				
				
		}

		protected void OnEdtSolVersionTextInserted (object o, Gtk.TextInsertedArgs args)
		{
			Version outVersion;
			if (!Version.TryParse (edtSolVersion.Text, out outVersion))
			{
				Console.WriteLine ("");
			}
			else
			{
				if (mVersionData.HasIOSProjects)
				{
					edtiOSShort.Text = CocoaAppVersion.ToShortVersion (outVersion);
				}

				if (mVersionData.HasAndroidProjects)
				{
					edtAndroidBuild.Text = AndroidAppVersion.ToBuild (outVersion);
				}


			}
		}
	}
}

