using UnityEngine;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a projectile.
	/// </summary>
	[CreateAssetMenu(fileName = "New Projectile", menuName = "Celeritas/New Projectile", order = 40)]
	public class ProjectileData : EntityData
	{
		[SerializeField, TitleGroup("Projectile")]
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
		public float Lifetime { get => lifetime; set => lifetime = value; }

		/// <summary>
		/// How much damage this projectile does when it hits another entity
		/// </summary>
		public int Damage { get => damage; }

		/// <summary>
		/// Whether the projectile is destroyed when hitting something else
		/// (true = yes it will be destroyed)
		/// </summary>
		public bool DestroyedOnHit { get => destroyedOnHit; }

		public override string Tooltip => $"A projectile that travels at <color=\"orange\">{speed}m/s</color> and deals <color=\"orange\">{damage}</color> on hit.";

		protected virtual void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<ProjectileEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(ProjectileEntity)} attatched to it.", this);
			}
		}
	}
}
