using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace DavesAddin.Data
{
	public enum AppType
	{
		Cocoa,
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

	public class CocoaAppVersion : AppVersion
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DavesAddin.Data.CocoaAppVersion"/> class.
		/// </summary>
		public CocoaAppVersion ()
		{
			ApplicationType = AppType.Cocoa;
		}
		#endregion

		/// <summary>
		/// Gets or sets the file path.
		/// </summary>
		/// <value>The file path.</value>
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

		/// <summary>
		/// Gets the version.
		/// </summary>
		/// <returns>The version.</returns>
		/// <param name="Key">Key.</param>
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

		/// <summary>
		/// To short version.
		/// </summary>
		/// <returns>The short version.</returns>
		/// <param name="CurrentVersion">Current version.</param>
		public static string ToShortVersion (Version CurrentVersion)
		{
			var qa = CurrentVersion.Revision.ToString ("D2");

			var verString = CurrentVersion.ToString();
			var its = verString.Split('.');

			if (its.Length > 3)
			{
				//remove the last dot
				var index = verString.LastIndexOf('.');
				verString = verString.Remove(index);
				verString = verString.Insert(index, qa);
			}
			return verString;
		}

		public override void Update ()
		{
			try
			{
				Dictionary<string, object> dict = (Dictionary<string, object>)Plist.readPlist (FilePath);
				bool fileChanged = false;

				if (!String.IsNullOrWhiteSpace (mVersionOne))
				{
					dict ["CFBundleShortVersionString"] = mVersionOne;

					fileChanged = true;
				}


				if (!String.IsNullOrWhiteSpace (mVersionTwo))
				{
					dict ["CFBundleVersion"] = mVersionTwo;

					fileChanged = true;
				}

				if (fileChanged)
				{
					Plist.writeXml (dict, FilePath);
				}


			}
			catch
			{

			}
		}
	}

	public class iOSAppVersion : CocoaAppVersion
	{
		public iOSAppVersion () : base ()
		{
			ApplicationType = AppType.iOS;
		}
	}

	public class MacAppVersion : CocoaAppVersion
	{
		public MacAppVersion () : base ()
		{
			ApplicationType = AppType.Mac;
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

		#region Xonstructors

		public AndroidAppVersion ()
		{
			ApplicationType = AppType.Android;
		}

		#endregion

		#region Methods

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
			int[] ints = new int[]
			{
				CurrentVersion.Major,
				CurrentVersion.Minor,
				CurrentVersion.Build,
				CurrentVersion.Revision,
			};
//			var fir = CurrentVersion.Major.ToString ();
//			var th = CurrentVersion.Minor.ToString ();
//			var ap = CurrentVersion.Build.ToString ("D2");
//			var qa = CurrentVersion.Revision.ToString ("D2");
//
//			return String.Format ("{0}{1}{2}{3}", fir, th, ap, qa);

			var verString = String.Empty;

			foreach (var aInt in ints)
			{
				if (aInt > 0)
				{
					verString += aInt.ToString("D2");
				}
			}

			return verString;
//			var qa = CurrentVersion.Revision.ToString ("D2");
//
//			var verString = CurrentVersion.ToString();
//			var its = verString.Split('.');
//
//			if (its.Length > 3)
//			{
//				//remove the last dot
//				var index = verString.LastIndexOf('.');
//				verString = verString.Remove(index);
//				verString = verString.Insert(index, qa);
//			}
//			return verString;
		}

		public override void Update ()
		{
			var xmlDoc = new XmlDocument ();

			xmlDoc.Load (FilePath);

			bool fileChanged = false;

			foreach (XmlNode aChild in xmlDoc.ChildNodes)
			{
				if (aChild.Name.ToLower ().Equals ("manifest"))
				{
					//do something here
					if (!String.IsNullOrWhiteSpace (mVersionOne))
					{
						var buildAtr = aChild.Attributes ["android:versionCode"];

						buildAtr.Value = mVersionOne;

						fileChanged = true;
					}


					if (!String.IsNullOrWhiteSpace (mVersionTwo))
					{
						var versionAtr = aChild.Attributes ["android:versionName"];

						versionAtr.Value = mVersionTwo;

						fileChanged = true;
					}

				}
			}

			if (fileChanged)
			{
				xmlDoc.Save (FilePath);
			}

		}

		#endregion
	}
}

