using System;
using System.IO;

namespace DavesAddin.Data
{
	public class AssemblyVersionInfo
	{
		#region Fields

		private String mFilePath;
		private Version mAssemblyVersion = null;
		private Version mFileVersion = null;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the assembly version.
		/// </summary>
		/// <value>The assembly version.</value>
		public Version AssemblyVersion {
			get
			{
				if (mAssemblyVersion == null)
				{
					Parse ();
				}
				return mAssemblyVersion; 
			}
			set{ mAssemblyVersion = value; }
		}

		public Version FileVersion {
			get { return mFileVersion; }
			set{ mFileVersion = value; }
		}

		#endregion

		#region Constructors

		public AssemblyVersionInfo (String Path)
		{
			mFilePath = Path;

			//Parse ();
		}

		#endregion

		private void Parse ()
		{
			var version = String.Empty;

			if (mFilePath != String.Empty)
			{
				//MessageBox.Show(fullPath);
				TextReader tr = new StreamReader (mFilePath);

				String line;
				while ((line = tr.ReadLine ()) != null)
				{
					//strip out white spaces
					line = line.Replace (" ", "");

					if (!line.StartsWith ("//") && !line.StartsWith ("'"))
					{
						string searchText = "AssemblyVersion(\"";

						if (line.Contains (searchText))
						{
							int locationStart = line.IndexOf (searchText);
							string remaining = line.Substring ((locationStart + searchText.Length));
							int locationEnd = remaining.IndexOf ("\"");
							version = remaining.Substring (0, locationEnd);
							continue;
						}
					}
				}

				//line.Close();
				tr.Close ();
				if (version != String.Empty)
				{
					//MessageBox.Show(String.Format("{0} \n {1}", proj.Name, version));

					//remove any .* as they don't work
					version = version.Replace (".*", "");

					try
					{
						this.AssemblyVersion = new Version (version);
					}
					catch
					{
						this.AssemblyVersion = new Version ("0.0");
					}

				}

			}
			else
			{
				this.AssemblyVersion = new Version ("0.0");
			}
		}

		public void Update ()
		{

		}
	}
}

