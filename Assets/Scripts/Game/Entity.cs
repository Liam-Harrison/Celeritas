using Celeritas.Extensions;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// The Entity class provides basic functionality for on-screen objects.
	/// </summary>
	public class Entity : MonoBehaviour
	{
		/// <summary>
		/// Get the entities game up direction vector.
		/// </summary>
		public Vector3 Up { get => transform.up.RemoveAxes(z: true); }

		/// <summary>
		/// Get the entities game right direction vector.
		/// </summary>
		public Vector3 Right { get => transform.right.RemoveAxes(z: true); }

		/// <summary>
		/// Is this entity initalized.
		/// </summary>
		public bool IsInitalized { get; protected set; } = false;

		/// <summary>
		/// The data associated with this entity.
		/// </summary>
		public virtual ScriptableObject Data { get; protected set; }

		/// <summary>
		/// Called to initalize this entity with its appropriate data.
		/// </summary>
		/// <param name="data">The data to associate this entity with.</param>
		public virtual void Initalize(ScriptableObject data)
		{
			Data = data;
			IsInitalized = true;
		}
	}
}
