using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
    "VersionChanger",
    Namespace = "VersionChanger",
    Version = "2.0"
)]

[assembly: AddinName("VersionChanger")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("Version Changer to make it easier to change version numbers in projects")]
[assembly: AddinAuthor("Dave Humphreys")]

[assembly: AddinDependency("::MonoDevelop.Core", MonoDevelop.BuildInfo.Version)]
[assembly: AddinDependency("::MonoDevelop.Ide", MonoDevelop.BuildInfo.Version)]
