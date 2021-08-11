using UnityEngine;

namespace Celeritas
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

		/// <summary>
		/// Destroy all the children currently under this transform.
		/// </summary>
		/// <param name="transform">The target transform.</param>
		public static void DestroyAllChildren(this Transform transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.Destroy();
			}
		}

		/// <summary>
		/// Destroy the target unity object.
		/// </summary>
		/// <param name="obj">the object to destroy.</param>
		public static void Destroy(this Object obj)
		{
			if (obj is Transform) obj = (obj as Transform).gameObject;
#if UNITY_EDITOR
			if (Application.isPlaying)
				Object.Destroy(obj);
			else
			{
				UnityEditor.EditorApplication.delayCall += () =>
				{
					Object.DestroyImmediate(obj);
				};
			}
#else
		Object.Destroy(obj);
#endif
		}
	}
}
