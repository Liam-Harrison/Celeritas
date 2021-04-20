//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR && ASSETICONS_ASMDEF
#pragma warning disable

using AssetIcons;
using AssetIcons.Editors;
using System.Diagnostics.CodeAnalysis;

using System.Reflection;

[assembly: AssemblyTitle("AssetIcons.Editors")]
[assembly: AssemblyDescription(ProductInformation.Description)]
[assembly: AssemblyCopyright(ProductInformation.Copyright)]
[assembly: AssemblyCompany(ProductInformation.Author)]
[assembly: AssemblyVersion(ProductInformation.Version)]
[assembly: AssemblyFileVersion(ProductInformation.Version)]
[assembly: AssemblyInformationalVersion(ProductInformation.Version)]

#if !ASSETICONS_DEV

[assembly: SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = JustificationReasons.style)]
[assembly: SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = JustificationReasons.style)]
[assembly: SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = JustificationReasons.style)]

#endif

[assembly: SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = JustificationReasons.unity)]
[assembly: SuppressMessage("Style", "CS0649", Justification = JustificationReasons.unity)]

[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = JustificationReasons.apiCompatibility)]

[assembly: SuppressMessage("Style", "IDE0018:Inline variable declaration", Justification = JustificationReasons.languageCompatibility)]
[assembly: SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = JustificationReasons.languageCompatibility)]
[assembly: SuppressMessage("Style", "IDE0059:Value assigned to symbol is never used", Justification = JustificationReasons.languageCompatibility)]
[assembly: SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = JustificationReasons.languageCompatibility)]
[assembly: SuppressMessage("Style", "IDE1005:Delegate invocation can be simplified.", Justification = JustificationReasons.languageCompatibility)]

[assembly: SuppressMessage("Design", "CA0649", Justification = JustificationReasons.unity)]
[assembly: SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = JustificationReasons.unity)]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = JustificationReasons.apiCompatibility)]
[assembly: SuppressMessage("Microsoft.Interoperability", "CA1405:ComVisibleTypeBaseTypesShouldBeComVisible", Justification = JustificationReasons.unity)]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = JustificationReasons.apiCompatibility)]
[assembly: SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", Justification = JustificationReasons.unity)]
[assembly: SuppressMessage("Microsoft.Usage", "CA2235:MarkAllNonSerializableFields", Justification = JustificationReasons.unity)]

namespace AssetIcons.Editors
{
	internal static class JustificationReasons
	{
		internal const string languageCompatibility = "Ignoring style increases compatibility with C# versions.";
		internal const string apiCompatibility = "Increase API compatbilitiy; past and future.";
		internal const string unity = "Causes incompatibilities with Unity serialization.";
		internal const string style = "Coding style of AssetIcons.";
	}
}

#pragma warning restore
#endif
