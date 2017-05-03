using System;
using MonoDevelop.Projects;

namespace VersionChanger.Data
{
	public enum ProjectType
	{
		DotNet,
		Portable,
		iOS,
		Android,
		Mac,
		Unknown,
	}

	/// <summary>
	/// Project Version information
	/// </summary>
	public class ProjectVersion
	{
		#region Properties

		public ProjectType ProjType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the source project.
		/// </summary>
		/// <value>The source project.</value>
		public Project SourceProject { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether it should be updated
		/// </summary>
		/// <value>
		///   <c>true</c> if [update]; otherwise, <c>false</c>.
		/// </value>
		public bool Update { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public String Name
		{
			get
			{
				return SourceProject.Name;
			}
		}

		/// <summary>
		/// Gets or sets the assembly version info.
		/// </summary>
		/// <value>The assembly version info.</value>
		public AssemblyVersionInfo AssemblyVersionInfo
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the project version.
		/// </summary>
		/// <value>The project version.</value>
		public Version Version
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the app verison info.
		/// </summary>
		/// <value>The app verison info.</value>
		public AppVersion AppVerisonInfo
		{
			get;
			set;
		}

		#endregion

		#region Constructors

		public ProjectVersion()
		{
			ProjType = ProjectType.Unknown;
		}

		#endregion
	}
}
