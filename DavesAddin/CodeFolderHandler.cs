using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using Mono.TextEditor;
using MonoDevelop.Ide.Gui.Content;

namespace DavesAddin
{
	public enum CodeFoldingCommands
	{
		CollapseToDefs,
	}

	public class CodeFoldingHandler : CommandHandler
	{
		protected override void Run ()
		{
			//var item = IdeApp.
			var item = IdeApp.Workbench.ActiveDocument;

			if (item != null)
			{
				var editor = item.ActiveView.GetContent <IFoldable> () as MonoDevelop.SourceEditor.SourceEditorView;
				var Document = item.Editor.Document;

				foreach (FoldSegment segment in Document.FoldSegments)
				{
					if (segment.FoldingType == FoldingType.TypeDefinition)
					{
						segment.IsFolded = false;
					}
					if (segment.FoldingType == FoldingType.TypeMember
					    || segment.FoldingType == FoldingType.Comment
					    || segment.FoldingType == FoldingType.Region)
						segment.IsFolded = true;
				}

				editor.TextEditor.Caret.MoveCaretBeforeFoldings ();
				Document.RequestUpdate (new UpdateAll ());
				Document.CommitDocumentUpdate ();
				editor.TextEditor.GetTextEditorData ().RaiseUpdateAdjustmentsRequested ();
				editor.TextEditor.ScrollToCaret ();
			}

		}

		protected override void Update (CommandInfo info)
		{  
			var doc = IdeApp.Workbench.ActiveDocument; 
			info.Enabled = doc != null && doc.GetContent<ITextEditorDataProvider> () != null; 
		}
	}
}

