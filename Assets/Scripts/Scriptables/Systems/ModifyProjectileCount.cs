using Sirenix.OdinInspector;
using System.Collections.Generic;
using Celeritas.Scriptables.Interfaces;
using UnityEngine;
using Celeritas.Game;
using Celeritas.Game.Entities;

namespace Celeritas.Scriptables.Systems
{
	/// <summary>
	/// contains infomration for a modifier that will modify a weapon's projectile count
	/// ie. how many projectiles fire per 'fire' command
	/// </summary>
	[CreateAssetMenu(fileName = "Projectile Count Modifier", menuName= "Celeritas/Modifiers/Projectile Count")]
	public class ModifyProjectileCount : ModifierSystem, IEntityFired
	{
		[SerializeField, Title("Extra Projectile Count")]
		private int extraProjectileCount;

		[SerializeField]
		private int extraProjectilesPerLevel;

		[SerializeField]
		private float spreadScale = 0.5f; // default

		[SerializeField] // only for linear
		private bool randomSpread;

		[SerializeField]
		private bool customProjectile;

		[SerializeField, ShowIf(nameof(customProjectile))]
		ProjectileData customProjectileData;

		[SerializeField]
		private bool arcLayout;

		[SerializeField, ShowIf(nameof(arcLayout))]
		private float degreesSpread;

		[SerializeField, ShowIf(nameof(arcLayout))]
		private float weaponOriginDisplacement = -5; // default = -5

		//[SerializeField]
		//private EffectCollection[] effectsToExcludeCopying;

		/// <summary>
		/// How many extra projectiles will be fired per 'fire' command
		/// when system is at level 0
		/// </summary>
		public int ExtraProjectileCount { get => extraProjectileCount; }

		/// <summary>
		/// How many extra projectiles will be added per level
		/// </summary>
		public int ExtraProjectileCountPerLevel { get => extraProjectilesPerLevel; }

		/// <inheritdoc/>
		public override bool Stacks => true;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Weapon;

		/// <inheritdoc/>
		public override string GetTooltip(int level) => $"Fire <color=green>{ExtraProjectileCount + (ExtraProjectileCountPerLevel * level)}</color> extra projectiles.";

		public void OnEntityFired(WeaponEntity weapon, ProjectileEntity projectile, EffectWrapper wrapper)
		{
			int numberOfExtraProjectiles = extraProjectileCount + (wrapper.Level * extraProjectilesPerLevel);
			
			// arc layout logic
			if (arcLayout)
			{
				for (int i = -(numberOfExtraProjectiles / 2); i <= numberOfExtraProjectiles / 2; i++)
				{
					// rotate around, using weapon position as origin
					var toFire = EntityDataManager.InstantiateEntity<ProjectileEntity>(projectile.ProjectileData, weapon.ProjectileSpawn.position, weapon.ProjectileSpawn.rotation, weapon);
					toFire.transform.localScale = weapon.ProjectileSpawn.localScale;

					//toFire.transform.RotateAround(weapon.transform.position, weapon.transform.forward , degreesSpread * i);
					toFire.transform.RotateAround(weapon.transform.position + (weapon.transform.up * weaponOriginDisplacement), weapon.transform.forward, degreesSpread * i);
					//

					toFire.EntityEffects.AddEffectRange(projectile.EntityEffects.EffectWrapperCopy); // add effects once position has been finalised
				}
			}
			else
			{ 
				// linear layout logic
				Vector3 bulletAlignment = new Vector3(1, 0, 0);
				for (int i = 0; i < numberOfExtraProjectiles; i++)
				{
					float position = i - (numberOfExtraProjectiles / 2);
					// to address the '0' position projectile lying directly on top of originally shot projectile:
					if (position < 0.00001 && position > -0.00001)
					{
						position = numberOfExtraProjectiles / 2;
					}

					var toFire = EntityDataManager.InstantiateEntity<ProjectileEntity>(projectile.ProjectileData, weapon.ProjectileSpawn.position, weapon.ProjectileSpawn.rotation, weapon);
					toFire.transform.localScale = weapon.ProjectileSpawn.localScale;

					if (randomSpread)
					{
						bulletAlignment = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
						position += Random.Range(-1f, 1f);
					}
					toFire.transform.Translate(bulletAlignment.normalized * position * spreadScale);
					toFire.transform.position = toFire.transform.position.RemoveAxes(z: true, normalize: false);

					toFire.EntityEffects.AddEffectRange(projectile.EntityEffects.EffectWrapperCopy); // add effects once position has been finalised
				}
			}

		}

		// leaving this, we might want it later, not currently needed though.

		/*
		private List<EffectWrapper> getEffectsForSubProjectiles(WeaponEntity entity)
		{
			var effects = new List<EffectWrapper>(entity.ProjectileEffects.EffectWrapperCopy);
			var toRemove = new List<EffectWrapper>();

			foreach (var j in effects)
			{
				foreach (var k in effectsToExcludeCopying)
				{
					if (j.EffectCollection == k)
						toRemove.Add(j);
				}
			}

			foreach (var item in toRemove)
			{
				effects.Remove(item);
			}
			return effects;
		}*/
	}
}
