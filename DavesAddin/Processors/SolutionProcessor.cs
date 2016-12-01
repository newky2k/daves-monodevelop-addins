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
using MonoDevelop.Core;

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


						var iOSTypes = new List<String>{ "{FEACFBD2-3405-455C-9665-78FE426C6842}","{EE2C853D-36AF-4FDB-B1AD-8E90477E2198}"};
						var androidTypes = new List<String> { "{EFBA0AD7-5A72-4C68-AF49-83D382785DCF}","{10368E6C-D01B-4462-8E8B-01FC667A7035}"};
						var macTypes = new List<String> { "{A3F8F2AB-B479-4A4A-A458-A89E7DC349F1}", "{EE2C853D-36AF-4FDB-B1AD-8E90477E2198}" };

						if (iOSTypes.Contains(proj.FlavorGuids.First()))
						{
							var iVersion = new iOSAppVersion();

							var infoPlist = proj.Files.FirstOrDefault(x => x.Name.ToLower().Contains("info.plist"));

							if (infoPlist != null)
							{
								iVersion.FilePath = infoPlist.FilePath;

								// only set if the info.plist exists
								newVersion.AppVerisonInfo = iVersion;
								newVersion.ProjType = ProjectType.iOS;
							}
	

						}
						else if (androidTypes.Contains(proj.FlavorGuids.First()))
						{
							var aVersion = new AndroidAppVersion();

							var manifest = proj.Files.FirstOrDefault(x => x.Name.ToLower().Contains("androidmanifest.xml"));

							if (manifest != null)
							{
								aVersion.FilePath = manifest.FilePath;

								//only set for the apps with a manifest
								newVersion.ProjType = ProjectType.Android;
								newVersion.AppVerisonInfo = aVersion;
							}

						}
						else if (macTypes.Contains(proj.FlavorGuids.First()))
						{
							var apVersion = new MacAppVersion();

							var infoPlist = proj.Files.FirstOrDefault(x => x.Name.ToLower().Contains("info.plist"));

							if (infoPlist != null)
							{
								apVersion.FilePath = infoPlist.FilePath;

								// only set if the info.plist exists
								newVersion.AppVerisonInfo = apVersion;
								newVersion.ProjType = ProjectType.Mac;
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

		/// <summary>
		/// Updates the versions.
		/// </summary>
		/// <param name="MainVersion">Main version.</param>
		/// <param name="AdditionaVersions">Additiona versions.</param>
		/// <param name="Data">Data.</param>
		/// <param name="MainSolution">Main solution.</param>
		public async static void UpdateVersions (String MainVersion, Dictionary<String,String> AdditionaVersions, SolutionVersion Data, Solution MainSolution)
		{
			
			var aProgess = new ProgressMonitor();

			if (MainSolution != null)
			{
				MainSolution.Version = MainVersion;
				await MainSolution.SaveAsync (aProgess);
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
					await aProj.SourceProject.SaveAsync(aProgess);
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


				if (AdditionaVersions != null)
				{
					if (AdditionaVersions.ContainsKey ("cocoa"))
					{
						var shortVersion = AdditionaVersions ["cocoa"];

						//process iOS first
						var iOSItems = from e in Data.Projects
						               where e.AppVerisonInfo is CocoaAppVersion
						               select e;

						foreach (var aProj in iOSItems.ToList())
						{
							aProj.AppVerisonInfo.VersionOne = shortVersion;
							aProj.AppVerisonInfo.VersionTwo = MainVersion.ToString ();
							aProj.AppVerisonInfo.Update ();
						}
					}

					if (AdditionaVersions.ContainsKey ("android"))
					{
						var build = AdditionaVersions ["android"];

						var iOSItems = from e in Data.Projects
						               where e.AppVerisonInfo is AndroidAppVersion
						               select e;

						foreach (var aProj in iOSItems.ToList())
						{
							aProj.AppVerisonInfo.VersionOne = build;
							aProj.AppVerisonInfo.VersionTwo = MainVersion.ToString ();
							aProj.AppVerisonInfo.Update ();
						}
					}
				}
			}



		}
	}
}

