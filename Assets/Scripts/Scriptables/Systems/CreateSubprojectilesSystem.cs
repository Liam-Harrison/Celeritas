using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// Modifier that will cause an explosion of shrapnel on entity death
	/// </summary>
	[CreateAssetMenu(fileName = "New SpawnProjectiles Modifier", menuName = "Celeritas/Modifiers/Spawn Projectiles On DeathOrCreationOrContinuously")]
	class CreateSubprojectilesSystem : ModifierSystem, IEntityKilled, IEntityEffectAdded, IEntityEffectRemoved, IEntityUpdated
	{
		public class SpawnProjectilesData
		{
			public float lastTimeSpawned;
		}

		[SerializeField, TitleGroup("Shrapnel Settings")]
		private ProjectileData shrapnel;

		[SerializeField, TitleGroup("Shrapnel Settings")]
		private float spacing;

		[SerializeField, TitleGroup("Shrapnel Settings")]
		private int numberToSpawn;

		[SerializeField, TitleGroup("Recursion Settings")]
		private EffectCollection[] effectsToExcludeCopying;

		[SerializeField, DisableInPlayMode]
		private bool spawnOnDeath;

		[SerializeField, DisableInPlayMode]
		private bool spawnContinuouslyThroughoutLife;

		[SerializeField, ShowIf(nameof(spawnContinuouslyThroughoutLife))]
		private float delayBetweenSpawns;

		[SerializeField, DisableInPlayMode]
		private bool spawnOnCreation; // when effect is added

		[SerializeField]
		private bool useCustomProjectileSpawnLocation; // use projectileSpawn specified in ProjectileEntity

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level)
		{
			string toReturn = "";
			if (spawnOnCreation)
				toReturn += $"Creates {numberToSpawn} subprojectile(s).";
			if (spawnOnDeath)
				toReturn += $"Explodes into {numberToSpawn} subprojectiles when destroyed.";
			if (spawnContinuouslyThroughoutLife)
				toReturn += $"Spawns {numberToSpawn} subprojectiles every {delayBetweenSpawns} second(s).";
			return toReturn;
		}

		public void OnEntityKilled(Entity entity, ushort level)
		{
			if (spawnOnDeath)
				SpawnProjectiles(entity);
		}

		private void SpawnProjectiles(Entity entity)
		{
			ProjectileEntity projectile = entity as ProjectileEntity;

			for (int i = 0; i < numberToSpawn; i++)
			{
				var effects = new List<EffectWrapper>(entity.EntityEffects.EffectWrapperCopy);
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

				float d = i - (numberToSpawn / 2f);
				var q = entity.transform.rotation * Quaternion.Euler(0, 0, d * spacing);

				ProjectileEntity proj;
				if (useCustomProjectileSpawnLocation)
					proj = EntityDataManager.InstantiateEntity<ProjectileEntity>(shrapnel, projectile.ProjectileSpawn.position, q, projectile.Weapon, effects);
				else
					proj = EntityDataManager.InstantiateEntity<ProjectileEntity>(shrapnel, entity.Position, q, projectile.Weapon, effects);
				proj.ParentProjectile = projectile;

			}
		}



		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			if (spawnOnCreation)
				SpawnProjectiles(entity);

			if (!spawnContinuouslyThroughoutLife)
				return;

			//ProjectileEntity projectile = entity as ProjectileEntity;
			if (entity.Components.TryGetComponent(this, out SpawnProjectilesData data))
			{
				data.lastTimeSpawned = entity.TimeAlive;
			}
			else
			{
				data = new SpawnProjectilesData();
				entity.Components.RegisterComponent<SpawnProjectilesData>(this, data);
			}
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			if (entity.Components.TryGetComponent(this, out SpawnProjectilesData data))
				entity.Components.ReleaseComponent<SpawnProjectilesData>(this);
		}

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			if (!spawnContinuouslyThroughoutLife)
				return;

			SpawnProjectilesData data = entity.Components.GetComponent<SpawnProjectilesData>(this);
			if (entity.TimeAlive - data.lastTimeSpawned > delayBetweenSpawns)
			{
				data.lastTimeSpawned = entity.TimeAlive;
				SpawnProjectiles(entity);
			}
		}
	}
}
