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

		/// <summary>
		/// for external effects on projectile speed (eg systems, modules)
		/// Multiplier, default = 1.
		/// </summary>
		public float SpeedModifier { get; set; }

		/// <summary>
		/// Used for chains of projectiles (following one-another)
		/// </summary>
		public Entity Following { get; set; }

		/// <inheritdoc/>
		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			ProjectileData = data as ProjectileData;
			damage = ProjectileData.Damage;
			Weapon = owner as WeaponEntity;
			SpeedModifier = 1;
			Following = null;

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

		private void OnDrawGizmos()
		{
			if (Following != null)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, Following.Position);
			}
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

			if (damageOverDistance)
			{
				other.TakeDamage(this, currentDamageOverDistance);
			}
			else
			{
				other.TakeDamage(this, damage);
			}

			if (ProjectileData.DestroyedOnHit)
				KillEntity();

			base.OnEntityHit(other);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (this == null || !IsInitalized)
				return;

			var entity = other.gameObject.GetComponentInParent<Entity>();
			if (entity != null)
			{
				OnEntityHit(entity);
			}
		}

		protected override void Update()
		{
			if (this == null || !IsInitalized)
				return;

			Position += Forward * ProjectileData.Speed * SpeedModifier * Time.smoothDeltaTime;

			if (TimeAlive >= ProjectileData.Lifetime)
			{
				Dying = true; // workaround
				UnloadEntity();
			}

			base.Update();
		}

		/// <summary>
		/// Determines whether to add damageOverDistance
		/// </summary>
		private bool damageOverDistance = false;

		public bool DamageOverDistance { get => damageOverDistance; set => damageOverDistance = value; }

		/// <summary>
		/// Used to record the total distance the projectile has travelled.
		/// </summary>
		private float totalDistanceTravelled;

		public float TotalDistanceTravelled { get => totalDistanceTravelled; set => totalDistanceTravelled = value; }

		/// <summary>
		/// A seperate value for current damage value if damageOverDistance = true
		/// </summary>
		private int currentDamageOverDistance = 0;

		public int CurrentDamageOverDistance { get => currentDamageOverDistance; set => currentDamageOverDistance = value; }
	}
}
