using System;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
		private EventInstance fireSoundInstance;

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
			// if (!WeaponData.FireSound.IsNull ) fireSoundInstance = RuntimeManager.CreateInstance(WeaponData.FireSound);
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

			// if (!WeaponData.FireSound.IsNull && WeaponData.IsChargeUpSound)
			// {
			// 	fireSoundInstance.getPlaybackState(out var state);
			// 	switch (state)
			// 	{
			// 		case PLAYBACK_STATE.STOPPED when state == PLAYBACK_STATE.STOPPING && Firing:
			// 			fireSoundInstance.start();
			// 			break;
			// 		case PLAYBACK_STATE.PLAYING when !Firing:
			// 			fireSoundInstance.stop(STOP_MODE.ALLOWFADEOUT);
			// 			break;
			// 	}
			// }
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
						lastFired = TimeAlive;
						Charge = 0;
					}
					else
						Charge = maxCharge;
				}
				//TODO: add animation to show weapon is charging (see: git issue #35)
			}
			else if (TimeAlive >= lastFired + (1f / rateOfFire))
			{
				Fire();
				lastFired = TimeAlive;
			}
		}

		protected virtual void Fire()
		{
			var projectile = EntityDataManager.InstantiateEntity<ProjectileEntity>(WeaponData.Projectile, projectileSpawn.position, projectileSpawn.rotation, this, ProjectileEffects.EffectWrapperCopy);
			projectile.transform.localScale = projectileSpawn.localScale;
			EntityEffects.EntityFired(projectile);

			if (fired != null && !WeaponData.FireSound.IsNull)
				if (!WeaponData.IsChargeUpSound) RuntimeManager.PlayOneShot(WeaponData.FireSound);
		}

		public void OverDrive(float percentageToAdd, float duration)
		{
			StartCoroutine(RunOverdrive(percentageToAdd, duration));
		}

		public IEnumerator RunOverdrive(float percentageToAdd, float duration)
		{
			float originalRateOfFire = RateOfFire;
			RateOfFire = (uint)((float)RateOfFire * percentageToAdd);
			yield return new WaitForSeconds(duration);
			RateOfFire = originalRateOfFire;
		}
	}
}
