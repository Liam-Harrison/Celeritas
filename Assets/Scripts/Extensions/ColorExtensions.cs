using UnityEngine;

namespace Celeritas.Extensions
{
	/// <summary>
	/// A collection of extensions for the color class.
	/// </summary>
	public static class ColorExtensions
	{
		/// <summary>
		/// Set the alpha of a color to the specified value.
		/// </summary>
		/// <param name="value">The color to change.</param>
		/// <param name="a">The alpha to set.</param>
		/// <returns>The original color with the specified alpha.</returns>
		public static Color SetAlpha(this Color value, float a)
		{
			value.a = a;
			return value;
		}
	}
}
