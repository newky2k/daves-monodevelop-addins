using System;
using DavesAddin.Data;
using MonoDevelop.Projects;

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


		}
	}
}

