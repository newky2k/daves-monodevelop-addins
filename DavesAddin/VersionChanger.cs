using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using Mono.TextEditor;
using DavesAddin.Processors;
using System.Diagnostics;
using DavesAddin.Dialogs;
using Xwt;

namespace DavesAddin
{
	public enum VersionChangerCommands
	{
		UpdateVersions,
	}

	public class VersionChangerHandler : CommandHandler
	{
		protected override void Run ()
		{

			Xwt.Application.Initialize(Xwt.ToolkitType.Gtk);

			var aItem = Xwt.Toolkit.CurrentEngine.WrapWindow(IdeApp.Workbench.RootWindow);

			//var item = IdeApp.
			var item = IdeApp.ProjectOperations.CurrentSelectedSolution;

			if (item != null)
			{
				//
				var results = SolutionProcessor.BuildVersions (item);


				try
				{
					var dialog = new VersionChangerDialog (results, item);
					MessageService.ShowCustomDialog (dialog);
				}
				catch (Exception ex)
				{
					MessageService.ShowException (ex);
				}



			}
				

		}

		protected override void Update (CommandInfo info)
		{  
			info.Enabled = true; 
		}
	}
}

