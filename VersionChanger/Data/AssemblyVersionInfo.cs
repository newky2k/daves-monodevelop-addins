using System;
using System.IO;
using System.Text;

namespace VersionChanger.Data
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
		public Version AssemblyVersion
		{
			get
			{
				if (mAssemblyVersion == null)
				{
					Parse();
				}
				return mAssemblyVersion;
			}
			set { mAssemblyVersion = value; }
		}

		public Version FileVersion
		{
			get
			{
				if (mFileVersion == null)
				{
					return AssemblyVersion;
				}
				return mFileVersion;
			}
			set
			{
				mFileVersion = value;
			}
		}

		#endregion

		#region Constructors

		public AssemblyVersionInfo(String Path)
		{
			mFilePath = Path;

			//Parse ();
		}

		#endregion

		#region Methods

		private void Parse()
		{
			var version = String.Empty;

			if (mFilePath != String.Empty)
			{
				//MessageBox.Show(fullPath);
				TextReader tr = new StreamReader(mFilePath);

				String line;
				while ((line = tr.ReadLine()) != null)
				{
					//strip out white spaces
					line = line.Replace(" ", "");

					if (!line.StartsWith("//") && !line.StartsWith("'"))
					{
						string searchText = "AssemblyVersion(\"";

						if (line.Contains(searchText))
						{
							int locationStart = line.IndexOf(searchText);
							string remaining = line.Substring((locationStart + searchText.Length));
							int locationEnd = remaining.IndexOf("\"");
							version = remaining.Substring(0, locationEnd);
							continue;
						}
					}
				}

				//line.Close();
				tr.Close();
				if (version != String.Empty)
				{
					//MessageBox.Show(String.Format("{0} \n {1}", proj.Name, version));

					//remove any .* as they don't work
					version = version.Replace(".*", "");

					try
					{
						this.AssemblyVersion = new Version(version);
					}
					catch
					{
						this.AssemblyVersion = new Version("0.0");
					}

				}

			}
			else
			{
				this.AssemblyVersion = new Version("0.0");
			}
		}

		public void Update()
		{
			string[] file = File.ReadAllLines(mFilePath);
			var newLines = new StringBuilder();

			//String line;
			foreach (string aLine in file)
			{
				//strip out white spaces
				var line = aLine;

				String newLine = line;
				if (!line.StartsWith("//") && !line.StartsWith("'"))
				{
					if (line.Contains("assembly:"))
					{
						line = line.Replace(" ", "");
					}
					string searchText = "AssemblyVersion(\"";
					string searchText2 = "AssemblyFileVersion(\"";

					if (line.Contains(searchText))
					{
						int locationStart = line.IndexOf(searchText);
						string firstBit = line.Substring(0, (locationStart + searchText.Length));
						string remaining = line.Substring((locationStart + searchText.Length));
						int locationEnd = remaining.IndexOf("\"");
						string end = remaining.Substring(locationEnd);

						//MessageBox.Show(String.Format("{0}{1}{2}", firstBit, newVersion.ToString(), end));
						newLine = String.Format("{0}{1}{2}", firstBit, AssemblyVersion.ToString(), end);
					}

					if (line.Contains(searchText2))
					{
						int locationStart = line.IndexOf(searchText2);
						string firstBit = line.Substring(0, (locationStart + searchText2.Length));
						string remaining = line.Substring((locationStart + searchText2.Length));
						int locationEnd = remaining.IndexOf("\"");
						string end = remaining.Substring(locationEnd);

						newLine = String.Format("{0}{1}{2}", firstBit, FileVersion.ToString(), end);
					}
				}

				newLines.Append(newLine + "\r\n");
			}

			File.WriteAllText(mFilePath, newLines.ToString());
		}

		#endregion
	}
}
