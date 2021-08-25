using Sirenix.OdinInspector;
using System.Collections;
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
	public class ModifyProjectileCount : ModifierSystem, IEntityFired, IEntityEffectAdded
	{
		[SerializeField, Title("Extra Projectile Count")]
		private uint extraProjectileCount;

		[SerializeField]
		private uint extraProjectilesPerLevel;

		[SerializeField]
		private float spreadScale = 0.5f; // default

		[SerializeField]
		private bool randomSpread;

		[SerializeField]
		private bool customProjectile;

		[SerializeField, ShowIf(nameof(customProjectile))]
		ProjectileData customProjectileData;

		/// <summary>
		/// How many extra projectiles will be fired per 'fire' command
		/// when system is at level 0
		/// </summary>
		public uint ExtraProjectileCount { get => extraProjectileCount; }

		/// <summary>
		/// How many extra projectiles will be added per level
		/// </summary>
		public uint ExtraProjectileCountPerLevel { get => extraProjectilesPerLevel; }

		/// <inheritdoc/>
		public override bool Stacks => true;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Weapon;

		/// <inheritdoc/>
		public override string GetTooltip(ushort level) => $"Fire <color=green>{ExtraProjectileCount + (ExtraProjectileCountPerLevel * level)}</color> extra projectiles.";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			Debug.Log("added");
		}

		public void OnEntityFired(WeaponEntity entity, ProjectileEntity projectile, ushort level)
		{
			Debug.Log("ping");
			uint numberOfExtraProjectiles = extraProjectileCount + (level * extraProjectilesPerLevel);
			ProjectileData projectileData = projectile.ProjectileData;
			if (customProjectile)
				projectileData = customProjectileData;
			Debug.Log(projectileData);
			Vector3 bulletAlignment = new Vector3(spreadScale, spreadScale, 0);
			for (int i = 0; i < numberOfExtraProjectiles; i++)
			{
				float position = i - (numberOfExtraProjectiles / 2);
				// to address the '0' position projectile lying directly on top of originally shot projectile:
				if (position < 0.00001 && position > -0.00001)
				{
					position = numberOfExtraProjectiles / 2;
				}
					
				var toFire = EntityDataManager.InstantiateEntity<ProjectileEntity>(projectileData, entity.ProjectileSpawn.position, entity.ProjectileSpawn.rotation, entity, projectile.EntityEffects.EffectWrapperCopy);
				toFire.transform.localScale = entity.ProjectileSpawn.localScale;
				Debug.Log(toFire);

				if (randomSpread) { 
					bulletAlignment = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
					position += Random.Range(-1f, 1f);
				}
				toFire.transform.Translate(bulletAlignment.normalized * position * spreadScale);
				toFire.transform.position = toFire.transform.position.RemoveAxes(z: true, normalize: false);
			}
			
		}
	}
}
