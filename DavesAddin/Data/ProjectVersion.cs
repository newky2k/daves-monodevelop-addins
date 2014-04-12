using System;

namespace DavesAddin.Data
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
		#region Fields

		private bool m_Update = true;
		private String m_Name;
		private String m_Path;
		private Version m_Version;

		#endregion

		#region Properties

		public ProjectType ProjType {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether it should be updated
		/// </summary>
		/// <value>
		///   <c>true</c> if [update]; otherwise, <c>false</c>.
		/// </value>
		public bool Update {
			get
			{
				return m_Update;
			}

			set
			{
				m_Update = value;
			}
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public String Name {
			get
			{
				return m_Name;
			}

			set
			{
				m_Name = value;
			}
		}

		/// <summary>
		/// Gets or sets the path.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public String Path {
			get
			{
				return m_Path;
			}

			set
			{
				m_Path = value;
			}
		}

		public AssemblyVersionInfo AssemblyVersionInfo {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the project version.
		/// </summary>
		/// <value>The project version.</value>
		public Version Version {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the app verison info.
		/// </summary>
		/// <value>The app verison info.</value>
		public AppVersion AppVerisonInfo {
			get;
			set;
		}

		#endregion

		#region Constructors

		public ProjectVersion ()
		{
			ProjType = ProjectType.Unknown;
		}

		#endregion
	}
}

