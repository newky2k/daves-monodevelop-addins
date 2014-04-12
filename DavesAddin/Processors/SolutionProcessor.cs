using System;
using System.Collections.ObjectModel;
using DavesAddin.Data;
using MonoDevelop.Projects;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

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
				foreach (Project proj in FindProjects (MainSolution))
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

						var newVersion = new ProjectVersion ();
						newVersion.ProjType = ProjectType.DotNet;
						newVersion.Name = proj.Name;
						newVersion.Path = proj.FileName;
						newVersion.Version = new Version (proj.Version);


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

		/// <summary>
		/// Finds the projects in the solution
		/// </summary>
		/// <param name="solution">The solution.</param>
		/// <returns></returns>
		private static ArrayList FindProjects (Solution solution)
		{
			ArrayList projectst = new ArrayList ();

			var projs = solution.GetAllProjects ();

			foreach (Project proj in projs)
			{
				//MessageBox.Show(proj.FullName);
				Debug.WriteLine (proj.Name);

				if (proj.Name == "")
				{
					//folder
					//int count = proj.ProjectItems.Count;

					AddSubProjects (proj, projectst);
				}
				else
				{
					projectst.Add (proj);
				}
			}


			return projectst;
		}

		/// <summary>
		/// Adds the sub projects.
		/// </summary>
		/// <param name="Proj">The proj.</param>
		/// <param name="Items">The items.</param>
		private static void AddSubProjects (Project Proj, ArrayList Items)
		{
//			//MessageBox.Show(proj.FullName);
//			Debug.WriteLine (Proj.Name);
//
//			if (Proj.Name == "")
//			{
//				//folder
//				int count = Proj.ProjectItems.Count;
//
//				foreach (ProjectItem proj2 in Proj.ProjectItems)
//				{
//					if (proj2.SubProject != null)
//					{
//						AddSubProjects (proj2.SubProject, Items);
//					}
//					//MessageBox.Show(proj2.SubProject.ToString());
//				}
//			}
//			else
//			{
//				Items.Add (Proj);
//			}
		}
	}
}

