using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace DavesAddin.Data
{
	public enum AppType
	{
		iOS,
		Android,
		Mac,
	}

	public abstract class AppVersion
	{
		public AppType ApplicationType {
			get;
			set;
		}

		public abstract string VersionOne { get; }

		public abstract string VersionTwo { get; }

		public abstract String FilePath { get; set; }
	}

	public class iOSAppVersion : AppVersion
	{
		public iOSAppVersion ()
		{
			ApplicationType = AppType.iOS;
		}

		public override String FilePath { get; set; }

		public override string VersionOne {
			get
			{
				return GetVersion ("CFBundleShortVersionString");
			}
		}

		public override string VersionTwo {
			get
			{
				return GetVersion ("CFBundleVersion");
			}
		}

		public string GetVersion (String Key)
		{
			try
			{
				Dictionary<string, object> dict = (Dictionary<string, object>)Plist.readPlist (FilePath);

				return dict [Key].ToString ();
			}
			catch
			{
				return String.Empty;
			}


		}
	}

	public class AndroidAppVersion : AppVersion
	{
		#region Properties

		public override String FilePath { get; set; }

		public override string VersionOne {
			get
			{
				return GetVersion ("android:versionCode");
			}
		}

		public override string VersionTwo {
			get
			{
				return GetVersion ("android:versionName");
			}
		}

		#endregion

		public AndroidAppVersion ()
		{
			ApplicationType = AppType.Android;
		}

		public string GetVersion (String Key)
		{
			var xmlDoc = new XmlDocument ();

			xmlDoc.Load (FilePath);

			foreach (XmlNode aChild in xmlDoc.ChildNodes)
			{
				if (aChild.Name.ToLower ().Equals ("manifest"))
				{
					//do something here
					var atr = aChild.Attributes [Key];

					return (atr != null) ? atr.Value.ToString () : String.Empty;
				}
			}
			return string.Empty;
		}
	}
}

