using Celeritas.Extensions;
using Celeritas.Scriptables;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;


namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a weapon.
	/// </summary>
	public class WeaponEntity : ModuleEntity
	{
		[SerializeField]
		private Transform projectileSpawn;

		private List<EffectWrapper> projectileEffects = new List<EffectWrapper>();

		/// <summary>
		/// Get the effect manager for the weapons on this entity.
		/// </summary>
		public EffectManager WeaponEffects { get; private set; }

		/// <summary>
		/// The attatched weapon data.
		/// </summary>
		public WeaponData WeaponData { get; private set; }

		/// <summary>
		/// The effects on this weapon which will be added to projectiles.
		/// </summary>
		public IReadOnlyList<EffectWrapper> ProjectileEffects { get => projectileEffects.AsReadOnly(); }

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
		public bool firing { get; set; }

		protected override void Update()
		{
			if (!IsInitalized)
				return;

			base.Update();

			if (firing)
			{
				TryToFire();
			}
		}

		private void Fire()
		{
			var projectile = EntityDataManager.InstantiateEntity<ProjectileEntity>(WeaponData.Projectile, this, WeaponEffects.EffectWrapperCopy);
			projectile.transform.CopyTransform(projectileSpawn);
			projectile.transform.position = projectile.transform.position.RemoveAxes(z: true, normalize: false);
		}

		private float lastFired = 0.0f;

		private void TryToFire()
		{
			if (Time.time >= lastFired + WeaponData.RateOfFire)
			{
				Fire();
				lastFired = Time.time;
			}
		}

	}
}
