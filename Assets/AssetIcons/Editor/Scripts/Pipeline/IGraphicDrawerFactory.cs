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
	/// <para>A factory for <see cref="IGraphicDrawer"/>s used to extend AssetIcons with additional support..</para>
	/// </summary>
	public interface IGraphicDrawerFactory
	{
		/// <summary>
		/// <para>Determines the importance of this <see cref="IGraphicDrawerFactory"/>.</para>
		/// <para>A higher <c>Priority</c> means that this <see cref="IGraphicDrawerFactory"/> will be used over other <see cref="IGraphicDrawerFactory"/>.</para>
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// <para>Whether this <see cref="IGraphicDrawerFactory"/> is valid for drawing objects of specified <see cref="Type"/>.</para>
		/// </summary>
		/// <param name="type">A <see cref="Type"/> to query.</param>
		/// <returns>
		/// <para><c>true</c> if the specified <see cref="Type"/> is valid for drawing by this <see cref="IGraphicDrawerFactory"/>; otherwise <c>false</c>.</para>
		/// </returns>
		bool IsValidFor(Type type);

		/// <summary>
		/// <para>Creates an <see cref="IGraphicDrawer"/> from this factory.</para>
		/// </summary>
		/// <returns>
		/// <para>A new <see cref="IGraphicDrawer"/>.</para>
		/// </returns>
		IGraphicDrawer CreateDrawer();
	}
}

#pragma warning restore
#endif
