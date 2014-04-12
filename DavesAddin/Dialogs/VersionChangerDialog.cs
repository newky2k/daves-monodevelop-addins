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

		public VersionChangerDialog (SolutionVersion VersionDate, Solution MainSolution) 
			: this ()
		{
			
		}
	}
}

