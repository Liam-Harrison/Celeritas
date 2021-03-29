using UnityEngine;
using Celeritas.Extensions;
using Celeritas.Game.Entities;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a projectile.
	/// </summary>
	[CreateAssetMenu(fileName = "New Projectile", menuName = "Celeritas/New Projectile", order = 40)]
	public class ProjectileData : EntityData
	{
		[SerializeField]
		private float speed;

		[SerializeField]
		private bool moveToTarget;

		/// <summary>
		/// The speed of this projectile.
		/// </summary>
		public float Speed { get => speed; }

		/// <summary>
		/// Does this projectile move towards the weapon target.
		/// </summary>
		public bool MoveToTarget { get => moveToTarget; }

		protected virtual void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<ProjectileEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(ProjectileEntity)} attatched to it.", this);
			}
		}
	}
}
