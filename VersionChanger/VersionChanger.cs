using System;
using Xwt;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using VersionChanger.Processors;


namespace VersionChanger
{
    public class VersionChanger : CommandHandler
    {
		protected override void Run()
		{

			Application.Initialize(Xwt.ToolkitType.Gtk);

			//var aItem = Xwt.Toolkit.CurrentEngine.WrapWindow(IdeApp.Workbench.RootWindow);

			//var item = IdeApp.
			var item = IdeApp.ProjectOperations.CurrentSelectedSolution;

			if (item != null)
			{
				//
				var results = SolutionProcessor.BuildVersions(item);


				try
				{
					var dialog = new VersionChangerDialog(results, item);
					MessageService.ShowCustomDialog(dialog);
				}
				catch (Exception ex)
				{
					MessageService.ShowError("", ex);
				}



			}


		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = true;
		}
    }
}
