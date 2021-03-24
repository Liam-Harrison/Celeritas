using Celeritas.Scriptables;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a projectile.
	/// </summary>
	public class ProjectileEntity : Entity
	{
		/// <summary>
		/// The attatched projectile data.
		/// </summary>
		public ProjectileData ProjectileData { get; private set; }

		public WeaponEntity Weapon { get; private set; }

		public override void Initalize(ScriptableObject data)
		{
			ProjectileData = data as ProjectileData;
			base.Initalize(data);
		}

		/// <summary>
		/// Set the owner of this projectile.
		/// </summary>
		/// <param name="entity">The weapon entity who fired this projectile.</param>
		public void SetOwner(WeaponEntity entity)
		{
			Weapon = entity;
		}

		protected virtual void Update()
		{
			if (ProjectileData.MoveToTarget)
			{
				transform.forward = Vector3.Lerp(transform.forward, (Weapon.AttatchedModule.Ship.Target - transform.position).normalized, 6f * Time.smoothDeltaTime);
			}

			transform.position += transform.forward * ProjectileData.Speed * Time.smoothDeltaTime;

			if (TimeAlive >= 2) Destroy(gameObject);
		}
	}
}
