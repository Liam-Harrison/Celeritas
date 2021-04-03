using Celeritas.Extensions;
using Celeritas.Scriptables;
using System.Collections.Generic;
using UnityEngine;


namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a weapon.
	/// </summary>
	public class WeaponEntity : ModuleEntity
	{
		[SerializeField]
		private Transform projectileSpawn;

		/// <summary>
		/// Get the effect manager for the weapons on this entity.
		/// </summary>
		public EffectManager WeaponEffects { get; private set; }

		/// <summary>
		/// The attatched weapon data.
		/// </summary>
		public WeaponData WeaponData { get; private set; }

		/// <summary>
		/// Where the weapon's projectiles will spawn
		/// </summary>
		public Transform ProjectileSpawn { get => projectileSpawn; }

		/// <inheritdoc/>
		public override SystemTargets TargetType { get => SystemTargets.Weapon; }

		public override void Initalize(ScriptableObject data, Entity owner = null, IList<EffectWrapper> effects = null)
		{
			WeaponData = data as WeaponData;

			WeaponEffects = new EffectManager(SystemTargets.Projectile);

			base.Initalize(data, owner, effects);
		}

		/// <summary>
		/// Is the weapon firing?
		/// </summary>
		public bool Firing { get; set; }

		protected override void Update()
		{
			if (!IsInitalized)
				return;

			base.Update();

			if (Firing)
			{
				TryToFire();
			}
		}

		private float lastFired = 0.0f;

		private void TryToFire()
		{
			if (Time.time >= lastFired + (1f / WeaponData.RateOfFire))
			{
				Fire();
				lastFired = Time.time;
			}
		}

		private void Fire()
		{
			var projectile = EntityDataManager.InstantiateEntity<ProjectileEntity>(WeaponData.Projectile, this, WeaponEffects.EffectWrapperCopy);
			projectile.transform.CopyTransform(projectileSpawn);
			projectile.transform.position = projectile.transform.position.RemoveAxes(z: true, normalize: false);
			OnWeaponFired(projectile);
		}

		/// <summary>
		/// Fire events for systems.
		/// </summary>
		private void OnWeaponFired(ProjectileEntity projectile)
		{
			foreach (var wrapper in EffectWrappers)
			{
				wrapper.EffectCollection.OnFired(this, projectile, wrapper.Level);
			}
		}
	}
}
