using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace DavesAddin.Data
{
	public class SolutionVersion
	{
		#region Properties

		public Version Version {
			get;
			set;
		}

		public bool HasIOSProjects {
			get
			{
				var items = from e in Projects
				            where e.ProjType.Equals (ProjectType.iOS)
				            select e;

				return (items.ToList ().Count > 0);
			}
		}

		public bool HasAndroidProjects {
			get
			{
				var items = from e in Projects
				            where e.ProjType.Equals (ProjectType.Android)
				            select e;

				return (items.ToList ().Count > 0);
			}
		}

		public Collection<ProjectVersion> Projects {
			get;
			set;
		}

		#endregion

		#region Consructors

		public SolutionVersion ()
		{
			Projects = new Collection<ProjectVersion> ();
		}

		#endregion
	}
}

