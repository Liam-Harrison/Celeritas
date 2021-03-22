using UnityEngine;

namespace Celeritas.Extensions
{
	/// <summary>
	/// Contains a collection of Unity vector extensions.
	/// </summary>
	public static class VectorExtensions
	{
		/// <summary>
		/// Zeroes out any selected axes of the provided vector.
		/// </summary>
		/// <param name="vector">The vector to modify.</param>
		/// <param name="x">Zero out the x axis if true.</param>
		/// <param name="y">Zero out the y axis if true.</param>
		/// <param name="z">Zero out the z axis if true.</param>
		/// <param name="normalize">Normalize the vector if true.</param>
		/// <returns>Returns the modified vector.</returns>
		public static Vector3 RemoveAxes(this Vector3 vector, bool x = false, bool y = false, bool z = false, bool normalize = true)
		{
			if (x) vector.x = 0;
			if (y) vector.y = 0;
			if (z) vector.z = 0;
			return normalize ? vector.normalized : vector;
		}
	}
}
