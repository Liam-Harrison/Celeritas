using UnityEngine;

namespace Celeritas.Extensions
{
	/// <summary>
	/// Extensions for Unity transforms.
	/// </summary>
	public static class TransformExtensions
	{
		/// <summary>
		/// Set the provided transform to have the world position, rotation and scale of the other specified transform.
		/// </summary>
		/// <param name="transform">The provided transform.</param>
		/// <param name="other">The transform to copy.</param>
		public static void CopyTransform(this Transform transform, Transform other)
		{
			transform.position = other.position;
			transform.rotation = other.rotation;
			transform.localScale = other.localScale;
		}
	}
}
