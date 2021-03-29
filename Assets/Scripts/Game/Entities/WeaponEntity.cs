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

		private List<EffectCollection> projectileEffects = new List<EffectCollection>();

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
		public IReadOnlyList<EffectCollection> ProjectileEffects { get => projectileEffects.AsReadOnly(); }

		/// <inheritdoc/>
		public override SystemTargets TargetType { get => SystemTargets.Weapon; }

		public override void Initalize(ScriptableObject data, Entity owner = null, IList<EffectCollection> effects = null)
		{
			WeaponData = data as WeaponData;

			WeaponEffects = new EffectManager(SystemTargets.Projectile);

			base.Initalize(data, owner, effects);
		}

		protected override void Update()
		{
			if (!IsInitalized)
				return;

			if (WeaponData.Aims)
			{
				var ship = AttatchedModule.Ship;
				var dir = (ship.Target - transform.position).normalized;

				if (Vector3.Dot(transform.forward, dir) >= 0.95)
					return;

				var angle = WeaponData.AimSpeed * Time.smoothDeltaTime;
				if (Vector3.Dot(transform.right, dir) < 0)
				{
					angle = -angle;
				}

				transform.rotation *= Quaternion.Euler(0, angle, 0);
			}
			base.Update();
		}

		/// <summary>
		/// Fire the provided projectile from this weapon.
		/// </summary>
		public void Fire()
		{
			var projectile = EntityDataManager.InstantiateEntity<ProjectileEntity>(WeaponData.Projectile, this, WeaponEffects.EffectsCopy);
			projectile.transform.CopyTransform(projectileSpawn);
			projectile.transform.position = projectile.transform.position.RemoveAxes(z: true, normalize: false);
		}
	}
}
