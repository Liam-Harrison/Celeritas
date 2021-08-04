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

		[SerializeField, Title("Shrapnel projectile data (not recursive)")]
		ProjectileData shrapnelData;

		[SerializeField, Title("Shrapnel spacing (how wide the spread of shrapnel is) (smaller = closer together)")]
		int spacing; // ie, the spacing between bullets
		[SerializeField, Title("Number Of Shrapnel Bullets")]
		int numberToSpawn;

		public override string GetTooltip(ushort level) 
		{
			throw new NotImplementedException();
		}

		public void OnEntityKilled(Entity entity, ushort level)
		{
			// note: level could make child projectiles shrapnel. Level number of layers.
			// TODO: shrapnel slows down gradually and is destroyed when speed is 0

			ProjectileEntity projectile = entity as ProjectileEntity;

			entity.transform.eulerAngles += new Vector3(0, 0, - spacing * numberToSpawn / 2);

			for (int i = 0; i < numberToSpawn; i++) {
				entity.transform.eulerAngles = entity.transform.rotation.eulerAngles + new Vector3(0, 0, spacing);
				var toFire2 = EntityDataManager.InstantiateEntity<ProjectileEntity>(shrapnelData, entity.Position, entity.transform.rotation, projectile.Weapon);
			}
		}
	}
}
