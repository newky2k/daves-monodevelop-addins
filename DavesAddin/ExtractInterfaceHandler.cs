using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Core;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.CSharp.Resolver;
using System.Linq;
using MonoDevelop.Ide.Gui.Content;

namespace DavesAddin
{
	public enum ExtractInterfaceCommands
	{
		ExtractInterface,
	}

	public class ExtractInterfaceHandler : CommandHandler
	{
		protected override void Run ()
		{
//			//var item = IdeApp.
//			var item = IdeApp.ProjectOperations.CurrentSelectedSolution;
//
//			if (item != null)
//			{
//				//
//				var results = SolutionProcessor.BuildVersions (item);
//
//
//				try
//				{
//					var dialog = new VersionChangerDialog (results, item);
//					MessageService.ShowCustomDialog (dialog);
//				}
//				catch (Exception ex)
//				{
//					MessageService.ShowException (ex);
//				}
//
//
//
//			}


		}

		protected override void Update (CommandInfo info)
		{  
			var doc = IdeApp.Workbench.ActiveDocument;
			if (doc == null || doc.FileName == FilePath.Null)
				return;
				
//			ResolveResult resolveResoult;
//			System.Object item = GetItem (doc, out resolveResoult);

			info.Enabled = isTypeDef(doc);
			info.Visible = info.Enabled;

		}

		public bool isTypeDef(MonoDevelop.Ide.Gui.Document doc)
		{
			var resolveResult = GetResolveResult (doc);

			if (resolveResult is TypeResolveResult)
				return true;

			return false;
		}
		public object GetItem (MonoDevelop.Ide.Gui.Document doc, out ResolveResult resolveResult)
		{
			resolveResult = GetResolveResult (doc);
			if (resolveResult is LocalResolveResult) 
				return ((LocalResolveResult)resolveResult).Variable;
			if (resolveResult is MemberResolveResult)
				return ((MemberResolveResult)resolveResult).Member;
			if (resolveResult is MethodGroupResolveResult) {
				var mg = ((MethodGroupResolveResult)resolveResult);
				var method = mg.Methods.FirstOrDefault ();
				if (method == null && mg.GetExtensionMethods ().Any ()) 
					method = mg.GetExtensionMethods ().First ().FirstOrDefault ();
				return method;
			}
			if (resolveResult is TypeResolveResult)
				return resolveResult.Type;
			if (resolveResult is NamespaceResolveResult)
				return ((NamespaceResolveResult)resolveResult).Namespace;
			if (resolveResult is OperatorResolveResult)
				return ((OperatorResolveResult)resolveResult).UserDefinedOperatorMethod;
			return null;
		}

		public static ResolveResult GetResolveResult (MonoDevelop.Ide.Gui.Document doc)
		{
			ITextEditorResolver textEditorResolver = doc.GetContent<ITextEditorResolver> ();
			if (textEditorResolver != null)
				return textEditorResolver.GetLanguageItem (doc.Editor.IsSomethingSelected ? doc.Editor.SelectionRange.Offset : doc.Editor.Caret.Offset);
			return null;
		}

	}
}

