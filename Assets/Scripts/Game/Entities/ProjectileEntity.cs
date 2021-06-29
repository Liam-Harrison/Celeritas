using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a projectile.
	/// </summary>
	public class ProjectileEntity : Entity
	{
		[SerializeField, PropertySpace]
		protected int damage;

		[SerializeField, PropertySpace]
		private TrailRenderer[] trails;

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

		/// <inheritdoc/>
		public override SystemTargets TargetType { get => SystemTargets.Projectile; }

		/// <inheritdoc/>
		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			ProjectileData = data as ProjectileData;
			damage = ProjectileData.Damage;
			Weapon = owner as WeaponEntity;

			base.Initalize(data, owner, effects, forceIsPlayer, instanced);
		}

		public override void OnDespawned()
		{
			foreach (var trail in trails)
			{
				trail.Clear();
			}

			base.OnDespawned();
		}

		public override void OnEntityHit(Entity other)
		{
			if (other.IsPlayer == IsPlayer)
				return;

			if (other is ShipEntity ship)
			{
				if (ship.WeaponEntities.Contains(Weapon))
				{
					return;
				}
			}

			other.TakeDamage(this, damage);

			if (ProjectileData.DestroyedOnHit)
				KillEntity();

			base.OnEntityHit(other);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!IsInitalized)
				return;

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
