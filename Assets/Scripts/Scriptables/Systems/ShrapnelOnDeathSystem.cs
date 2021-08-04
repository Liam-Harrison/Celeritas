using Celeritas.Game;
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
	[CreateAssetMenu(fileName = "New ShrapnelOnDeath Modifier", menuName = "Celeritas/Modifiers/Shrapnel Explosion on death")]
	class ShrapnelOnDeathSystem : ModifierSystem, IEntityKilled
	{
		[SerializeField, TitleGroup("Shrapnel Settings")]
		private ProjectileData shrapnel;

		[SerializeField, TitleGroup("Shrapnel Settings")]
		private float spacing;

		[SerializeField, TitleGroup("Shrapnel Settings")]
		private int numberToSpawn;

		[SerializeField, TitleGroup("Recursion Settings")]
		private EffectCollection[] effectsToExcludeCopying;

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level) => $"<color=green>▲</color> Explodes into {numberToSpawn} subprojectiles when destroyed";

		public void OnEntityKilled(Entity entity, ushort level)
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

				EntityDataManager.InstantiateEntity<ProjectileEntity>(shrapnel, entity.Position, q, projectile.Weapon, effects);
			}
		}
	}
}
