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

		/// <summary>
		/// The weapon this projectile was fired from.
		/// </summary>
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
			Weapon.OnEntityCreated(this);
		}

		private void OnTriggerEnter(Collider other)
		{
			var entity = other.GetComponent<Entity>();
			if (entity != null)
			{
				Weapon.OnEntityHit(this, entity);
			}
		}

		protected virtual void OnDestroy()
		{
			if (Weapon != null)
				Weapon.OnEntityDestroyed(this);
		}

		protected virtual void Update()
		{
			Weapon.OnEntityUpdated(this);

			transform.position += transform.forward * ProjectileData.Speed * Time.smoothDeltaTime;

			if (TimeAlive >= ProjectileData.Lifetime) Destroy(gameObject);
		}
	}
}
