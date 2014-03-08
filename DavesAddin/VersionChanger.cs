using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using Mono.TextEditor;

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
			//var item = IdeApp.
			var item = IdeApp.ProjectOperations.CurrentSelectedSolution;

			if (item != null)
			{
				//
			}

		}

		protected override void Update (CommandInfo info)
		{  
			info.Enabled = true; 
		}
	}
}

