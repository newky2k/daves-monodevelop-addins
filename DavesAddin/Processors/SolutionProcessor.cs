using System;
using System.Linq;
using System.Collections.ObjectModel;
using DavesAddin.Data;
using MonoDevelop.Projects;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using MonoDevelop.Core.ProgressMonitoring;

namespace DavesAddin.Processors
{
	public class SolutionProcessor
	{
		/// <summary>
		/// Builds the versions from the projects
		/// </summary>
		/// <param name="MainSolution">The main solution.</param>
		/// <returns></returns>
		/// <exception cref="System.Exception"></exception>
		public static SolutionVersion BuildVersions (Solution MainSolution)
		{
			var solVersion = new SolutionVersion () {
				Version = new Version (MainSolution.Version),
			};


			try
			{
				foreach (Project proj in MainSolution.GetAllProjects())
				{
					String directory = String.Empty;

					if (!String.IsNullOrEmpty (proj.FileName))
					{
						try
						{
							directory = Path.GetDirectoryName (proj.FileName);
						}
						catch (Exception ex)
						{
							throw new Exception (String.Format ("Path {0} is invalid", proj.FileName), ex);
						}

						var newVersion = new ProjectVersion () {
							ProjType = ProjectType.DotNet,
							SourceProject = proj,
							Version = new Version (proj.Version),
						};
							
						var paths = new List<String> ();

						//c# sub folder
						paths.Add (Path.Combine (Path.Combine (directory, @"Properties"), "AssemblyInfo.cs"));
						paths.Add (Path.Combine (directory, "AssemblyInfo.cs"));
						paths.Add (Path.Combine (Path.Combine (directory, @"My Project"), "AssemblyInfo.vb"));
						paths.Add (Path.Combine (directory, "AssemblyInfo.vb"));

						foreach (var aPath in paths)
						{
							//c or vb.net
							if (File.Exists (aPath))
							{
								newVersion.AssemblyVersionInfo = new AssemblyVersionInfo (aPath);

								continue;
							}
						}

						if (proj is MonoDevelop.IPhone.IPhoneProject)
						{
							var apVersion = new iOSAppVersion ();

							//need to load the plist!
							var infoPlistPath = Path.Combine (directory, "Info.Plist");

							if (File.Exists (infoPlistPath))
							{
								apVersion.FilePath = infoPlistPath;

								// only set if the info.plist exists
								newVersion.AppVerisonInfo = apVersion;
								newVersion.ProjType = ProjectType.iOS;
							}


						}
						else if (proj is MonoDevelop.MonoDroid.MonoDroidProject)
						{
							var apVersion = new AndroidAppVersion ();

							var manifestPath = Path.Combine (Path.Combine (directory, @"Properties"), "AndroidManifest.xml");

							if (File.Exists (manifestPath))
							{
								apVersion.FilePath = manifestPath;

								//only set for the apps with a manifest
								newVersion.ProjType = ProjectType.Android;
								newVersion.AppVerisonInfo = apVersion;
							}

						}
							
						solVersion.Projects.Add (newVersion);
					}

				}
			}
			catch
			{

			}

			return solVersion;
		}

		public static void UpdateVersions (String MainVersion, Dictionary<String,String> AdditionaVersions, SolutionVersion Data, Solution MainSolution)
		{
			var aProgess = new SimpleProgressMonitor ();

			if (MainSolution != null)
			{
				MainSolution.Version = MainVersion;
				MainSolution.Save (aProgess);
			}


			if (Data != null)
			{
				///Update the version number of items that aren't synced
				var unsynceditems = from e in Data.Projects
				                    where e.SourceProject.SyncVersionWithSolution.Equals (false)
				                    select e;

				foreach (var aProj in unsynceditems.ToList())
				{
					aProj.SourceProject.Version = MainVersion;
					aProj.SourceProject.Save (aProgess);
				}


				//now update the assmebly data
				var itemsWithAssemInfo = from e in Data.Projects
				                         where e.AssemblyVersionInfo != null
				                         select e;

				foreach (var aProj in itemsWithAssemInfo.ToList())
				{
					aProj.AssemblyVersionInfo.AssemblyVersion = new Version (MainVersion);

					aProj.AssemblyVersionInfo.Update ();
				}

			}

		}
	}
}

