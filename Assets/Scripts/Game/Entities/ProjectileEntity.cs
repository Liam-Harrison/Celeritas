using Celeritas.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a projectile.
	/// </summary>
	public class ProjectileEntity : Entity
	{
		// this is a variable as modifiers may need to change this (eg piercing bullets)
		private bool destroyedOnHit;

		private bool damageOwnerShip = false; // default

		[SerializeField]
		protected int damage;

		/// <summary>
		/// How much damage this entity does to another
		/// when it hits
		/// </summary>
		public int Damage { get => damage; set => damage = value; }

		/// <summary>
		/// The attatched projectile data.
		/// </summary>
		public ProjectileData ProjectileData { get; private set; }

		/// <summary>
		/// The weapon this projectile was fired from.
		/// </summary>
		public WeaponEntity Weapon { get; private set; }

		/// <summary>
		/// Whether the projectile is destroyed on hitting another entity
		/// </summary>
		public bool DestroyedOnHit { get => destroyedOnHit; }

		/// <summary>
		/// Whether the projectile will damage the ship that owns the weapon that shoots it
		/// </summary>
		public bool DamageOwnerShip {get => damageOwnerShip; }

		/// <inheritdoc/>
		public override SystemTargets TargetType { get => SystemTargets.Projectile; }

		/// <inheritdoc/>
		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			ProjectileData = data as ProjectileData;
			damage = ProjectileData.Damage;
			destroyedOnHit = ProjectileData.DestroyedOnHit;
			Weapon = owner as WeaponEntity;

			base.Initalize(data, owner, effects, forceIsPlayer, instanced);
		}

		public override void OnEntityHit(Entity other)
		{
			if (other.IsPlayer == IsPlayer)
				return;

			if (damageOwnerShip == false && other is ShipEntity ship)
			{
				if (ship.WeaponEntities.Contains(Weapon))
				{
					return;
				}
			}

			other.TakeDamage(this, damage);

			if (destroyedOnHit)
				KillEntity();

			base.OnEntityHit(other);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			var entity = other.gameObject.GetComponentInParent<Entity>();
			if (entity != null)
			{
				OnEntityHit(entity);
			}
		}

		protected override void Update()
		{
			Position += Forward * ProjectileData.Speed * Time.smoothDeltaTime;

			if (TimeAlive >= ProjectileData.Lifetime) Destroy(gameObject);

			base.Update();
		}
	}
}
