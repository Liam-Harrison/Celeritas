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

		/// <summary>
		/// Get a point on a circle with the specificed radius of the provided vector.
		/// </summary>
		/// <param name="vector">The provided vector.</param>
		/// <param name="radius">The radius of the circle.</param>
		/// <returns>Returns a random point on the circle around the vector.</returns>
		public static Vector3 RandomPointOnCircle(this Vector3 vector, float radius)
		{
			return vector + (Quaternion.Euler(0, 0, Random.Range(0, 360)) * new Vector3(radius, 0, 0));
		}
	}
}
