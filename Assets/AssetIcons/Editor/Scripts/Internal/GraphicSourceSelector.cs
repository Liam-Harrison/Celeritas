//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Internal;
using AssetIcons.Editors.Internal.Drawing;
using AssetIcons.Editors.Internal.Product;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AssetIcons.Editors.Internal
{
	internal class GraphicSourceSelector
	{
		public int Priority
		{
			get
			{
				return 100;
			}
		}

		public void CollectGraphicsFromMember(MemberInfo memberInfo, List<GraphicSource> sources)
		{
			if (memberInfo is FieldInfo)
			{
				var field = memberInfo as FieldInfo;

				object[] attributeObjects = field.GetCustomAttributes(typeof(AssetIconAttribute), true);

				foreach (object attributeObject in attributeObjects)
				{
					var attribute = (AssetIconAttribute)attributeObject;

					sources.Add(new FieldGraphicSource(new AssetIconsCompiledStyle(attribute.Style), field));
				}
			}
			else if (memberInfo is PropertyInfo)
			{
				var property = memberInfo as PropertyInfo;

				object[] attributeObjects = property.GetCustomAttributes(typeof(AssetIconAttribute), true);

				if (attributeObjects != null && attributeObjects.Length != 0)
				{
					var getMethod = property.GetGetMethod(true);
					bool isSupportedProperty = getMethod != null;

					foreach (object attributeObject in attributeObjects)
					{
						var attribute = (AssetIconAttribute)attributeObject;

						if (!isSupportedProperty)
						{
							AssetIconsLogger.UnsupportedSetOnlyProperties(attribute, property);
							continue;
						}

						sources.Add(new MethodGraphicProvider(new AssetIconsCompiledStyle(attribute.Style), getMethod));
					}
				}
			}
			else if (memberInfo is MethodInfo)
			{
				var method = memberInfo as MethodInfo;

				object[] attributeObjects = method.GetCustomAttributes(typeof(AssetIconAttribute), true);

				if (attributeObjects != null && attributeObjects.Length != 0)
				{
					bool isSupportedParameters = IsSupportedParameters(method.GetParameters());

					foreach (object attributeObject in attributeObjects)
					{
						var attribute = (AssetIconAttribute)attributeObject;

						if (!isSupportedParameters)
						{
							AssetIconsLogger.UnsupportedParametersError(attribute, method);
							continue;
						}

						sources.Add(new MethodGraphicProvider(new AssetIconsCompiledStyle(attribute.Style), method));
					}
				}
			}
		}

		private static bool IsSupportedParameters(ParameterInfo[] parameterInfos)
		{
			var parameterTypes = new Type[parameterInfos.Length];

			for (int k = 0; k < parameterInfos.Length; k++)
			{
				parameterTypes[k] = parameterInfos[k].ParameterType;
			}

			return IsSupportedParameters(parameterTypes);
		}

		private static bool IsSupportedParameters(Type[] parameterTypes)
		{
			return parameterTypes.Length != 0;
		}
	}
}

#pragma warning restore
#endif
