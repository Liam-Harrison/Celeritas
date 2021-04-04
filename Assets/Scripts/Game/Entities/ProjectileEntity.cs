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
		private bool iAmDestroyedOnHit; // default

		private bool damageOwnerShip = false; // default

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
		public bool IAmDestroyedOnHit;

		/// <inheritdoc/>
		public override SystemTargets TargetType { get => SystemTargets.Projectile; }

		public override void Initalize(ScriptableObject data, Entity owner = null, IList<EffectWrapper> effects = null)
		{
			ProjectileData = data as ProjectileData;
			damage = ProjectileData.Damage;
			iAmDestroyedOnHit = ProjectileData.IAmDestroyedOnHit;
			Weapon = owner as WeaponEntity;
			base.Initalize(data, owner, effects);
		}

		protected override void damageEntity(Entity other)
		{

			// check if other is owner ship
			if (damageOwnerShip == false && other is ShipEntity)
			{
				ShipEntity ship = (ShipEntity)other;
				if (ship.WeaponEntities.Contains(this.Weapon))
				{
					// return without doing damage
					return;
				}
			}

			if (iAmDestroyedOnHit)
				this.Dead = true;

			base.damageEntity(other);
		}

		/*
		private void OnTriggerEnter(Collider other)
		{
			var entity = other.GetComponent<Entity>();
			if (entity != null)
			{
				OnEntityHit(entity);
			}
		}*/

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
			transform.position += transform.forward * ProjectileData.Speed * Time.smoothDeltaTime;

			if (TimeAlive >= ProjectileData.Lifetime) Destroy(gameObject);

			base.Update();
		}
	}
}
