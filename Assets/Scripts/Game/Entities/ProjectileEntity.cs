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
		private bool inheirtVelcoity;

		[SerializeField, PropertySpace]
		protected int damage;

		[SerializeField, PropertySpace]
		private TrailRenderer[] trails;

		[SerializeField]
		private Transform projectileSpawn;

		public Transform ProjectileSpawn { get => projectileSpawn; }

		/// <summary>
		/// How much damage this entity does to another
		/// when it hits
		/// </summary>
		public int Damage { get => damage; set => damage = value; }

		/// <summary>
		/// The lifetime of this projectile
		/// (in seconds)
		/// </summary>
		public float Lifetime { get; set; }

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

		/// <summary>
		/// Used if this projectile was created by another projectile.
		/// </summary>
		public ProjectileEntity ParentProjectile { get; set; }

		public float BaseVelcoity { get; set; }

		/// <summary>
        /// Used to determine how long a project will stun the target for.
        /// </summary>
		public float StunDuration { get; set; }

		/// <inheritdoc/>
		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			ProjectileData = data as ProjectileData;
			damage = ProjectileData.Damage;
			Lifetime = ProjectileData.Lifetime;
			Weapon = owner as WeaponEntity;
			SpeedModifier = 1;
			Following = null;

			if (inheirtVelcoity && owner != null && !instanced)
			{
				if (owner is WeaponEntity weapon)
					BaseVelcoity = Mathf.Clamp01(Vector3.Dot(Forward, weapon.AttatchedModule.Ship.Rigidbody.velocity.normalized)) * weapon.AttatchedModule.Ship.Rigidbody.velocity.magnitude;
				else if (owner is ProjectileEntity projectile)
					BaseVelcoity = projectile.BaseVelcoity;
			}
			StunDuration = 0;

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

			//int calculatedDamage = damage;

			//Tells target not to display damage from projectile
			other.ShowDamageOnEntity = false;
			other.ShowDamageLocation = this.transform.position;

			base.OnEntityHit(other);

			if (damageOverDistance)
			{
				damage = currentDamageOverDistance;
			}

			if (other is ShipEntity ship)
			{
				if (StunDuration != 0)
				{
					if (ship.Stunned == false)
					{
						ship.Stun(StunDuration);
					}
				}

				if (ship.WeaponEntities.Contains(Weapon))
				{
					return;
				}
			}

			other.TakeDamage(this, damage);


			if (ProjectileData.DestroyedOnHit)
				KillEntity();
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
			Position += Forward * BaseVelcoity * Time.smoothDeltaTime;

			if (TimeAlive >= Lifetime)
			{
				Dying = true;
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
