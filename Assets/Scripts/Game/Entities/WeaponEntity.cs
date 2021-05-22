using Celeritas.Extensions;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a weapon.
	/// </summary>
	public class WeaponEntity : ModuleEntity
	{
		[SerializeField, Title("Weapon Settings")]
		private Transform projectileSpawn;

		private uint rateOfFire;
		private float maxCharge = 10.0f;

		/// <summary>
		/// Get the effect manager for the projectiles on this entity.
		/// </summary>
		public EffectManager ProjectileEffects { get; private set; }

		/// <summary>
		/// The attatched weapon data.
		/// </summary>
		public WeaponData WeaponData { get; private set; }

		public uint RateOfFire { get=> rateOfFire; set => rateOfFire = value; }

		/// <summary>
		/// Where the weapon's projectiles will spawn
		/// </summary>
		public Transform ProjectileSpawn { get => projectileSpawn; }

		/// <inheritdoc/>
		public override SystemTargets TargetType { get => SystemTargets.Weapon; }

		/// <inheritdoc/>
		public override void Initalize(ScriptableObject data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false)
		{
			WeaponData = data as WeaponData;
			rateOfFire = WeaponData.RateOfFire;
			maxCharge = WeaponData.MaxCharge;

			ProjectileEffects = new EffectManager(this, SystemTargets.Projectile);

			base.Initalize(data, owner, effects, forceIsPlayer);
		}

		/// <summary>
		/// Is the weapon firing?
		/// </summary>
		public bool Firing { get; set; }

		/// <summary>
		/// Weapon's current charge level
		/// </summary>
		public float Charge { get; set; } = 0.0f;

		protected override void Update()
		{
			if (!IsInitalized)
				return;

			base.Update();

			if (Firing)
			{
				TryToFire();
			}

			if (Firing == false && Charge > 0)
			{
				Fire();
				Charge = 0.0f;
			}
		}

		private float lastFired = 0.0f;

		protected virtual void TryToFire()
		{
			if (WeaponData.Charge)
			{
				Charge += (1f / rateOfFire);
				if (Charge > maxCharge)
				{
					Charge = maxCharge;
				}
				//TODO: add animation to show weapon is charging (see: git issue #35)
			}
			else if (Time.time >= lastFired + (1f / rateOfFire))
			{
				Fire();
				lastFired = Time.time;
			}
		}

		protected virtual void Fire()
		{
			var projectile = EntityDataManager.InstantiateEntity<ProjectileEntity>(WeaponData.Projectile, this, ProjectileEffects.EffectWrapperCopy);
			projectile.transform.CopyTransform(projectileSpawn);
			EntityEffects.EntityFired(projectile);
		}
	}
}
