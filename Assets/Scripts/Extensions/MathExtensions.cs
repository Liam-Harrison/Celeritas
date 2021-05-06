using UnityEngine;

namespace Celeritas
{
	/// <summary>
	/// Contains a collection of math functions specific to Celeritas.
	/// </summary>
	public static class CMathf
	{
		/// <summary>
		/// Lper a value between a and b according to t. Wraps the value to the value to be between 0 and l.
		/// </summary>
		/// <param name="a">The start value.</param>
		/// <param name="b">The end value.</param>
		/// <param name="l">The length to wrap around.</param>
		/// <param name="t">The percentage between a and b.</param>
		/// <returns>The value at the specified percentage between a and b.</returns>
		public static float LerpWrap(float a, float b, float l, float t)
		{
			float delta = Mathf.Repeat(b - a, l);
			if (delta > l / 2)
				delta -= l;
			return a + delta * Mathf.Clamp01(t);
		}
	}
}
