//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using UnityEngine;

namespace AssetIcons.Editors.Pipeline
{
	/// <summary>
	/// <para>A singular element that is drawn as apart of an assets icon.</para>
	/// </summary>
	internal interface IAssetGraphic
	{
		Type GraphicType { get; }

		object Graphic { get; }

		AssetIconsCompiledStyle Style { get; }
	}
}

#pragma warning restore
#endif
