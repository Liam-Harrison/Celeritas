//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Drawing
{
	/// <summary>
	/// <para>Used for converting expressions to values for styles.</para>
	/// </summary>
	internal static class ExpressionParser
	{
		public static Color32 ParseHexColor(string data)
		{
			data = data.Replace(" ", "");

			if (data[0] != '#')
			{
				throw new InvalidOperationException(string.Format("The hex color \"{0}\" must start with a \"#\".", data));
			}

			byte r;
			byte g;
			byte b;
			byte a = byte.MaxValue;

			if (data.Length == 3 || data.Length == 4)
			{
				r = DoubleHex(data[1]);
				g = DoubleHex(data[2]);
				b = DoubleHex(data[3]);

				if (data.Length == 4)
				{
					a = DoubleHex(data[4]);
				}
				return new Color32(r, g, b, a);
			}
			else if (data.Length == 7 || data.Length == 9)
			{
				r = ParseHex(data[1], data[2]);
				g = ParseHex(data[3], data[4]);
				b = ParseHex(data[5], data[6]);

				if (data.Length == 9)
				{
					a = ParseHex(data[7], data[8]);
				}
				return new Color32(r, g, b, a);
			}
			else
			{
				throw new InvalidOperationException(string.Format("The hex color \"{0}\" is an invalid length.", data));
			}
		}

		/* private static Color32 Color32RGBFormatter (string data)
		{
			data = data.Replace (" ", "");

			string[] cols = data.Split (',');

			if (cols.Length != 3 && cols.Length != 4)
			{
				Debug.LogError ("Only two values passed to create number where 3 or 4 is expected");
				return new Color();
			}

			bool result;
			byte valueA, valueB, valueC, valueD = 255;

			result = byte.TryParse (cols[0], out valueA);
			if (!result)
			{
				return new Color();
			}

			result = byte.TryParse (cols[1], out valueB);
			if (!result)
			{
				return new Color();
			}

			result = byte.TryParse (cols[2], out valueC);
			if (!result)
			{
				return new Color();
			}

			if (cols.Length == 4)
			{
				result = byte.TryParse (cols[3], out valueD);
				if (!result)
				{
					return new Color();
				}
			}

			return new Color32 (valueA, valueB, valueC, valueD);
		}

		private static Color ColorHexFormatter (string data)
		{
			return Color32HEXFormatter (data);
		}

		private static Color ColorRGBFormatter (string data)
		{
			data = data.Replace (" ", "");

			string[] cols = data.Split (',');

			if (cols.Length != 3 && cols.Length != 4)
			{
				// Debug.LogError ("Only two values passed to create number where 3 or 4 is expected");
				return new Color();
			}

			bool result;
			float valueA, valueB, valueC, valueD = 1.0f;

			result = float.TryParse (cols[0], out valueA);
			if (!result)
			{
				return new Color();
			}

			result = float.TryParse (cols[1], out valueB);
			if (!result)
			{
				return new Color();
			}

			result = float.TryParse (cols[2], out valueC);
			if (!result)
			{
				return new Color();
			}

			if (cols.Length == 4)
			{
				result = float.TryParse (cols[3], out valueD);
				if (!result)
				{
					return new Color();
				}
			}

			return new Color (valueA, valueB, valueC, valueD);
		}*/

		private static byte DoubleHex(char c)
		{
			byte value = PhraseHex(c);

			return (byte)((value << 4) + value);
		}

		private static byte PhraseHex(char c)
		{
			switch (c)
			{
				case '0':
					return 0;
				case '1':
					return 1;
				case '2':
					return 2;
				case '3':
					return 3;
				case '4':
					return 4;
				case '5':
					return 5;
				case '6':
					return 6;
				case '7':
					return 7;
				case '8':
					return 8;
				case '9':
					return 9;
				case 'a':
				case 'A':
					return 10;
				case 'b':
				case 'B':
					return 11;
				case 'c':
				case 'C':
					return 12;
				case 'd':
				case 'D':
					return 13;
				case 'e':
				case 'E':
					return 14;
				case 'f':
				case 'F':
					return 15;

				default:
					throw new InvalidOperationException(string.Format("The character \"{0}\" is not a valid hexadecimal character", c));
			}
		}

		private static byte ParseHex(char a, char b)
		{
			return (byte)((PhraseHex(a) << 4) + PhraseHex(b));
		}
	}
}

#pragma warning restore
#endif
