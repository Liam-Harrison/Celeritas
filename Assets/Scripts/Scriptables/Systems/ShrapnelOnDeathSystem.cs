using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// Modifier that will cause an explosion of shrapnel on entity death
	/// </summary>
	[CreateAssetMenu(fileName = "New ShrapnelOnDeath Modifier", menuName = "Celeritas/Modifiers/Shrapnel Explosion on death")]
	class ShrapnelOnDeathSystem : ModifierSystem, IEntityKilled
	{
		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		//[SerializeField, Title("shrapnel projectile data (not recursive)")]
		//ProjectileData shrapnelData;

		public override string GetTooltip(ushort level) 
		{
			throw new NotImplementedException();
		}

		public void OnEntityKilled(Entity entity, ushort level)
		{
			// TODO: level could make child projectiles shrapnel. Level number of layers.
			// TODO: make number spawned not constant - put into system settings or scale with level.

			EffectWrapper[] manager = entity.EntityEffects.EffectWrapperCopy;
			EffectWrapper toRemove; // containing the shrapnel effect
			foreach (EffectWrapper w in manager)
			{
				if (w.EffectCollection.Systems.Contains<ModifierSystem>(this))
				{
					//manager.RemoveEffect(w);
					entity.EntityEffects.RemoveEffect(w);
					break;
				}
			}

			int numberToSpawn = 10;
			int spread_degrees = 360;

			for (int i = 0; i < numberToSpawn; i++) {
				entity.transform.eulerAngles = entity.transform.rotation.eulerAngles + new Vector3(0, 0, i * (spread_degrees / numberToSpawn));
				var toFire = EntityDataManager.InstantiateEntity<ProjectileEntity>(entity.Data, entity.Position, entity.transform.rotation, entity, entity.EntityEffects.EffectWrapperCopy);
				// manager at end ?

			}
		}
	}
}
