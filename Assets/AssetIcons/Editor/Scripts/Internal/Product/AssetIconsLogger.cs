//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Product
{
	internal static class AssetIconsLogger
	{
		private static MethodInfo unityErrorInvoker;

		public static void UnsupportedParametersError(AssetIconAttribute attribute, MethodInfo method)
		{
			string message = "The AssetIcon attribute cannot be used on the type \"" + method.DeclaringType.Name + "\"s method \"" + method.Name + "\" with parameters that is not supported by the AssetIcon attribute.\n" +
				"Please use the AssetIcon attribute on a method with no parameters";

			AttributeError(attribute, message);
		}

		public static void UnsupportedSetOnlyProperties(AssetIconAttribute attribute, PropertyInfo property)
		{
			string message = "The AssetIcon attribute cannot be used on the type \"" + property.DeclaringType.Name + "\"s property \"" + property.Name + "\" with no get implementation.";

			AttributeError(attribute, message);
		}

		public static void UnsupportedTypeError(AssetIconAttribute attribute, MethodInfo method)
		{
			string message = "The AssetIcon attribute cannot be used on the type \"" + method.DeclaringType.Name + "\"s method \"" + method.Name + "\" of type \"" + method.ReturnType + "\".";

			AttributeError(attribute, message);
		}

		public static void UnsupportedTypeError(AssetIconAttribute attribute, FieldInfo field)
		{
			string message = "The AssetIcon attribute cannot be used on the type \"" + field.DeclaringType.Name + "\"s field \"" + field.Name + "\" of type \"" + field.FieldType + "\".";

			AttributeError(attribute, message);
		}

		public static void UnsupportedTypeError(AssetIconAttribute attribute, PropertyInfo property)
		{
			string message = "The AssetIcon attribute cannot be used on the type \"" + property.DeclaringType.Name + "\"s property \"" + property.Name + "\" of type \"" + property.PropertyType + "\".";

			AttributeError(attribute, message);
		}

		internal static void LogError(string message)
		{
			Debug.LogError(message);
		}

		internal static void LogException(Exception exception)
		{
			Debug.LogException(exception);
		}

		private static void AttributeError(AssetIconAttribute attribute, string message)
		{
			try
			{
				string errorPath = attribute.FilePath;
				int errorLine = attribute.LineNumber;

				if (unityErrorInvoker == null)
				{
					unityErrorInvoker = typeof(Debug).GetMethod("LogPlayerBuildError", BindingFlags.NonPublic | BindingFlags.Static);
				}

				var errorStringBuilder = new StringBuilder();

				errorStringBuilder.Append(message);
				errorStringBuilder.Append('\n');
				errorStringBuilder.Append(errorPath);
				errorStringBuilder.Append(':');
				errorStringBuilder.Append(errorLine.ToString());

				unityErrorInvoker.Invoke(null, new object[] { errorStringBuilder.ToString(), errorPath, errorLine, 0 });
			}
			catch
			{
				Debug.LogError(message);
			}
		}
	}
}

#pragma warning restore
#endif
