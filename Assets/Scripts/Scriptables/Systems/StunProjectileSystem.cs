using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{

	/// <summary>
	/// Module to apply stun on projectiles.
	/// </summary>
	[CreateAssetMenu(fileName = "New Stun Projectile Modifier", menuName = "Celeritas/Modifiers/Stun Projectiles")]
	public class StunProjectileSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{
		[SerializeField, Title("Stun duration", "Determines how long the stun effect will last.")]
		private float duration;

		[SerializeField, Title("Added duration per level", "Determines how long the stun duration is increased per level.")]
		private float durationExtraPerLevel;

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level) => $"Projectiles stun the target by <color=green>{(Duration + (DurationExtraPerLevel * level)):0}</color> seconds.";

		/// <summary>
		/// How long the target will get stunned by initially.
		/// </summary>
		public float Duration { get => duration; }

		/// <summary>
		/// How much longer stun will last per level.
		/// So total duration == (Percentage) + (PercentageExtraLevel * level)
		/// </summary>
		public float DurationExtraPerLevel { get => durationExtraPerLevel; }

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			var projectile = entity as ProjectileEntity;
			projectile.StunDuration = projectile.StunDuration + (duration + durationExtraPerLevel);
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var projectile = entity as ProjectileEntity;
			projectile.StunDuration = projectile.StunDuration - (duration + durationExtraPerLevel);
		}
	}
}
