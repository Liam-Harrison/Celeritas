using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a weapon.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class WeaponEntity : ModuleEntity
	{
		[SerializeField, TitleGroup("Weapon Settings")]
		private Transform projectileSpawn;

		[SerializeField, TitleGroup("Weapon Settings")]
		private AudioClip fired;

		[SerializeField, TitleGroup("Weapon Settings")]
		private bool hasDefaultProjectileEffects;

		[SerializeField, TitleGroup("Weapon Settings"), ShowIf(nameof(hasDefaultProjectileEffects))]
		private EffectWrapper[] projectileEffects;

		private AudioSource source;

		private float rateOfFire;
		private float maxCharge = 10.0f;

		/// <summary>
		/// Get the effect manager for the projectiles on this entity.
		/// </summary>
		public EffectManager ProjectileEffects { get; private set; }

		/// <summary>
		/// The attatched weapon data.
		/// </summary>
		public WeaponData WeaponData { get; private set; }

		/// <summary>
		/// The rate of fire of this weapon.
		/// </summary>
		public float RateOfFire { get=> rateOfFire; set => rateOfFire = value; }

		/// <summary>
		/// Where the weapon's projectiles will spawn
		/// </summary>
		public Transform ProjectileSpawn { get => projectileSpawn; }

		/// <inheritdoc/>
		public override SystemTargets TargetType { get => SystemTargets.Weapon; }

		private void Awake()
		{
			source = GetComponent<AudioSource>();
		}

		/// <inheritdoc/>
		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			WeaponData = data as WeaponData;
			rateOfFire = WeaponData.RateOfFire;
			maxCharge = WeaponData.MaxCharge;

			ProjectileEffects = new EffectManager(this, SystemTargets.Projectile);

			if (hasDefaultProjectileEffects)
				ProjectileEffects.AddEffectRange(projectileEffects);

			base.Initalize(data, owner, effects, forceIsPlayer, instanced);
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
			if (Time.deltaTime == 0) // if paused
				return;

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
				Charge += (1f * rateOfFire * Time.deltaTime); // high rate of fire should make weapon fire faster
				if (Charge > maxCharge)
				{
					if (WeaponData.Autofire)
					{
						Fire();
						//Debug.Log($"Weapon Fired. Time between fires: {TimeAlive - lastFired }");
						lastFired = TimeAlive;
						Charge = 0;
					}
					else
						Charge = maxCharge;
				}
				//TODO: add animation to show weapon is charging (see: git issue #35)
			}
			//else if (Time.time >= lastFired + (1f / rateOfFire))
			else if (TimeAlive >= lastFired + (1f / rateOfFire))
			{
				Fire();
				//lastFired = Time.time;
				//Debug.Log($"Weapon Fired. Time between fires: {TimeAlive - lastFired }");
				lastFired = TimeAlive;
			}
		}

		protected virtual void Fire()
		{
			var projectile = EntityDataManager.InstantiateEntity<ProjectileEntity>(WeaponData.Projectile, projectileSpawn.position, projectileSpawn.rotation, this, ProjectileEffects.EffectWrapperCopy);
			projectile.transform.localScale = projectileSpawn.localScale;
			EntityEffects.EntityFired(projectile);

			if (fired != null)
				source.PlayOneShot(fired);
		}
	}
}
