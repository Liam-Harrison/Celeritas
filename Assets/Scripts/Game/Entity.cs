using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// The Entity class provides basic functionality for on-screen objects.
	/// </summary>
	public class Entity : MonoBehaviour
	{
		public Vector3 Up { get => Vector3.ProjectOnPlane(transform.up, Vector3.forward); }

		public Vector3 Right { get => Vector3.ProjectOnPlane(transform.right, Vector3.forward); }
	}
}
