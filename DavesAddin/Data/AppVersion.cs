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
		#region Fields

		protected string mVersionOne;
		protected string mVersionTwo;

		#endregion

		public AppType ApplicationType {
			get;
			set;
		}

		public abstract string VersionOne { get; set; }

		public abstract string VersionTwo { get; set; }

		public abstract String FilePath { get; set; }

		public abstract void Update ();
	}

	public class iOSAppVersion : AppVersion
	{
		public iOSAppVersion ()
		{
			ApplicationType = AppType.iOS;
		}

		public override String FilePath { get; set; }

		/// <summary>
		/// Short version
		/// </summary>
		/// <value>The version one.</value>
		public override string VersionOne {
			get
			{
				return GetVersion ("CFBundleShortVersionString");
			}
			set
			{
				mVersionOne = value;
			}
		}

		/// <summary>
		/// Bundle Version
		/// </summary>
		/// <value>The version two.</value>
		public override string VersionTwo {
			get
			{
				return GetVersion ("CFBundleVersion");
			}
			set
			{
				mVersionTwo = value;
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

		public static string ToShortVersion (Version CurrentVersion)
		{
			return CurrentVersion.ToString (2);
		}

		public override void Update ()
		{
			try
			{
				Dictionary<string, object> dict = (Dictionary<string, object>)Plist.readPlist (FilePath);

				if (!String.IsNullOrWhiteSpace (mVersionOne))
					dict ["CFBundleShortVersionString"] = mVersionOne;

				if (!String.IsNullOrWhiteSpace (mVersionTwo))
					dict ["CFBundleVersion"] = mVersionTwo;

				Plist.writeXml (dict, FilePath);

			}
			catch
			{

			}
		}
	}

	public class AndroidAppVersion : AppVersion
	{
		#region Properties

		public override String FilePath { get; set; }

		/// <summary>
		/// Build Version
		/// </summary>
		/// <value>The version one.</value>
		public override string VersionOne {
			get
			{
				return GetVersion ("android:versionCode");
			}
			set
			{
				mVersionOne = value;
			}
		}

		/// <summary>
		/// Verion
		/// </summary>
		/// <value>The version two.</value>
		public override string VersionTwo {
			get
			{
				return GetVersion ("android:versionName");
			}
			set
			{
				mVersionTwo = value;
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

		public static string ToBuild (Version CurrentVersion)
		{
			return CurrentVersion.ToString ().Replace (".", "");
		}

		public override void Update ()
		{

		}
	}
}

