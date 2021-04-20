//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Pipeline;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>Manages all AssetIcon Attributes across the project and uses <see cref="Editors.AssetIconsGUI"/> to draw them.</para>
	/// </summary>
	internal class AssetDrawerFactoryLibrary
	{
		/// <summary>
		/// <para>A mapping of a type to a <see cref="AssetDrawerFactory"/> for that type.</para>
		/// </summary>
		private readonly Dictionary<Type, AssetDrawerFactory> factories;
		private readonly AssetIconPipeline pipeline;

		/// <summary>
		/// <para>Initializes the <see cref="AssetIconsMain"/> class.</para>
		/// </summary>
		internal AssetDrawerFactoryLibrary(AssetIconPipeline pipeline)
		{
			this.pipeline = pipeline;

			factories = BuildAssetDrawers(pipeline);
		}

		/// <summary>
		/// <para>Gets an <see cref="AssetDrawerFactory"/> for the <see cref="Type"/>.</para>
		/// </summary>
		/// <param name="type">The type to get an <see cref="AssetDrawerFactory"/> for.</param>
		/// <param name="assetDrawerFactory">The <see cref="AssetDrawerFactory"/> for the <see cref="Type"/>.</param>
		/// <returns>
		/// <para><c>true</c> if an <see cref="AssetDrawerFactory"/> was found; otherwise <c>false</c>.</para>
		/// </returns>
		public bool TryGetAssetDrawerFactory(Type type, out AssetDrawerFactory assetDrawerFactory)
		{
			return factories.TryGetValue(type, out assetDrawerFactory);
		}

		private static IEnumerable<Assembly> GetDependentAssemblies(AppDomain appDomain, Assembly analyzedAssembly)
		{
			foreach (var assembly in appDomain.GetAssemblies())
			{
				if (assembly == analyzedAssembly)
				{
					yield return assembly;
				}
				else
				{
					var referencedAssemblies = assembly.GetReferencedAssemblies();

					foreach (var referencedAssembly in referencedAssemblies)
					{
						if (referencedAssembly.FullName == analyzedAssembly.FullName)
						{
							yield return assembly;
							break;
						}
					}
				}
			}
		}

		private static Dictionary<Type, AssetDrawerFactory> BuildAssetDrawers(AssetIconPipeline pipeline)
		{
			var assetDrawerFactories = new Dictionary<Type, AssetDrawerFactory>();
			var graphicSourceSelection = new List<GraphicSource>();

			var checkAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in GetDependentAssemblies(AppDomain.CurrentDomain, typeof(AssetIconAttribute).Assembly))
			{
				Type[] assemblyTypes;
				try
				{
					assemblyTypes = assembly.GetTypes();
				}
				catch
				{
					continue;
				}

#if !UNITY_2017_1_OR_NEWER
				string assemblyName = assembly.FullName.Substring (0, assembly.FullName.IndexOf (','));

				if (assemblyName != "Assembly-UnityScript" &&
					assemblyName != "Assembly-CSharp" &&
					assemblyName != "Assembly-CSharp-Editor")
					continue;
#endif

				foreach (var type in assemblyTypes)
				{
					if (!typeof(ScriptableObject).IsAssignableFrom(type))
					{
						continue;
					}

					graphicSourceSelection.Clear();

					var typeMembers = type.GetMembers();
					foreach (var member in typeMembers)
					{
						var selector = new GraphicSourceSelector();
						selector.CollectGraphicsFromMember(member, graphicSourceSelection);
					}

					if (graphicSourceSelection.Count != 0)
					{
						graphicSourceSelection.Sort();
						assetDrawerFactories.Add(type, new AssetDrawerFactory(pipeline, graphicSourceSelection.ToArray()));
					}
				}
			}

			return assetDrawerFactories;
		}
	}
}

#pragma warning restore
#endif
