using Celeritas.Game;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	/// <summary>
	/// Destroys an entity after a given time delay (seconds)
	/// counter starts after system is applied.
	/// </summary>
	[CreateAssetMenu(fileName = "New DeathAfterDelay System", menuName = "Celeritas/Modifiers/Self Destruct/Death After Delay")]
	public class DieAfterDelaySystem : ModifierSystem, IEntityUpdated
	{
		[SerializeField, TitleGroup("Time to Live")]
		private float timeToLive;

		[SerializeField]
		private bool autoRemove = false;

		/// <inheritdoc/>
		public override bool Stacks => false;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Projectile | SystemTargets.Ship | SystemTargets.Loot | SystemTargets.Asteroid;

		public override string GetTooltip(ushort level) => $"Self destruct after {timeToLive} seconds";

		public void OnEntityUpdated(Entity entity, EffectWrapper wrapper)
		{
			// if entity has been alive for longer than TimeToLive and it isn't already dying, kill it
			if (!entity.Dying && entity.TimeAlive >= timeToLive) { 
				entity.KillEntity();

				if (autoRemove)
					entity.EntityEffects.RemoveEffect(wrapper);
			}
		}
	}
}
