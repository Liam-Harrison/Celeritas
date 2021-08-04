using Celeritas.Game;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	/// <summary>
	/// Destroys an entity after a given time delay (seconds)
	/// counter starts after system is applied.
	/// </summary>
	[CreateAssetMenu(fileName = "New DeathAfterDelay System", menuName = "Celeritas/Modifiers/Self Destruct/Death After Delay")]
	public class DieAfterDelaySystem : ModifierSystem, IEntityEffectAdded, IEntityUpdated
	{
		private float startTime;

		/// <inheritdoc/>
		public override bool Stacks => false;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Projectile | SystemTargets.Ship | SystemTargets.Loot | SystemTargets.Asteroid;

		[SerializeField, Title("Time to live (seconds)")]
		private float timeToLive;

		public override string GetTooltip(ushort level) => $"<color=red>▼</color> Self destructs after "+timeToLive+ " seconds";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			startTime = entity.TimeAlive;
		}

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			// if entity has been alive for longer than TimeToLive and it isn't already dying, kill it
			if (!entity.Dying && entity.TimeAlive - startTime >= timeToLive) { 
				//entity.KillEntity();
				entity.Dying = true;
			}
		}
	}
}
