using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{

	/// <summary>
	/// System to modify how much damage a projectile does
	/// </summary>
	[CreateAssetMenu(fileName = "New Knockback Modifier", menuName = "Celeritas/Modifiers/Knockback")]
	public class KnockbackSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{
		[SerializeField, Title("Knockback strength", "Determines how powerful the knockback effect is.")]
		private float percentage;

		[SerializeField, Title("Knockback strength added per Level", "Determines how much the knockback effect is increased per level.")]
		private float percentageExtraPerLevel;

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level) => $"Projectiles knock the target by <color=green>{(Percentage + (PercentageExtraPerLevel * level)) * 100:0}%</color>.";

		/// <summary>
		/// How much the target will get knocked back by initially.
		/// </summary>
		public float Percentage { get => percentage; }

		/// <summary>
		/// How much extra knockback the target gets from this modifier, per level
		/// So total extra knockback == (Percentage) + (PercentageExtraLevel * level)
		/// </summary>
		public float PercentageExtraPerLevel { get => percentageExtraPerLevel; }

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			var projectile = entity as ProjectileEntity;
			projectile.KnockBack = projectile.KnockBack + (percentage + percentageExtraPerLevel);
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var projectile = entity as ProjectileEntity;
			projectile.KnockBack = projectile.KnockBack - (percentage + percentageExtraPerLevel);
		}
	}
}
