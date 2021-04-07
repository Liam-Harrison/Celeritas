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
		private float lifetime;

		[SerializeField]
		private int damage;

		[SerializeField]
		private bool destroyedOnHit;

		/// <summary>
		/// The speed of this projectile.
		/// </summary>
		public float Speed { get => speed; }

		/// <summary>
		/// The lifetime of this projectile.
		/// </summary>
		public float Lifetime { get => lifetime; }

		/// <summary>
		/// How much damage this projectile does when it hits another entity
		/// </summary>
		public int Damage { get => damage; }

		/// <summary>
		/// Whether the projectile is destroyed when hitting something else
		/// (true = yes it will be destroyed)
		/// </summary>
		public bool DestroyedOnHit { get => destroyedOnHit; }

		protected virtual void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<ProjectileEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(ProjectileEntity)} attatched to it.", this);
			}
		}
	}
}
