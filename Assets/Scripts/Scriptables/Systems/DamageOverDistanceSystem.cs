using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{

	/// <summary>
	/// System to modify how much damage a projectile does depending on how far the projectile travels.
	/// </summary>
	[CreateAssetMenu(fileName = "New Projectile Damage Over Distance Modifier", menuName = "Celeritas/Modifiers/Projectile Damage Over Distance")]
	public class DamageOverDistanceSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{
		[SerializeField, PropertyRange(-100, 100), Title("Percentage Extra Projectile Damage Per Metre","The bonus damage that will increase per metre until the cap is met.")]
		private int percentage;

		[SerializeField, Title("Maximum Distance For Extra Projectile Damage","The distance in which the bonus damage increase per metre will be capped.")]
		private int maxDistance;

		[SerializeField, PropertyRange(-100, 100), Title("Percentage to increase cap by per module level.","Example: If cap is 5%, a value of 2 will increase the cap to 7%.")]
		private int percentageExtraPerLevel;

		/// <inheritdoc/>
		public override bool Stacks => false;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Projectile;

		/// <inheritdoc/>
		public override string GetTooltip(ushort level) => $"Increases damage by <color=green>{Percentage + (PercentageExtraPerLevel * level)}%</color> per metre up to a maximum of <color=green>{maxDistance}</color> metres.";

		/// <summary>
		/// How much extra percent damage the projectile gets from this modifier per metre.
		/// </summary>
		public int Percentage { get => percentage; }

		/// <summary>
		/// the maximum distance where damage will be increased to.
		/// </summary>
		public int MaxDistance { get => maxDistance; }

		/// <summary>
		/// How much extra percent damage the projectile gets from this modifier, per level
		/// So total extra percent damage == (MaxDamage) + (PercentageExtraLevel * level)
		/// </summary>
		public int PercentageExtraPerLevel { get => percentageExtraPerLevel; }

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			var projectile = entity as ProjectileEntity;
			projectile.MaxDistance = MaxDistance;
			projectile.DamagePercentageOverDistance = Percentage + (PercentageExtraPerLevel * level);
			projectile.damageOverDistance = true;
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var projectile = entity as ProjectileEntity;
			projectile.MaxDistance = projectile.MaxDistance - MaxDistance;
			projectile.DamagePercentageOverDistance = projectile.DamagePercentageOverDistance - (Percentage + (PercentageExtraPerLevel * level));
			projectile.damageOverDistance = false;
		}
	}
}