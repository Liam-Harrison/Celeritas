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
	[CreateAssetMenu(fileName = "New Vampiric Projectile Modifier", menuName = "Celeritas/Modifiers/Vampiric Projectiles")]
	public class VampireProjectileSystem : ModifierSystem, IEntityHit
	{
		[SerializeField, TitleGroup("Vampire")]
		private float percentage;

		[SerializeField, TitleGroup("Vampire")]
		private float percentageExtraPerLevel;

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level) => $"Restore <color=green>{Percentage + (PercentageExtraPerLevel * level)}%</color> of damage dealt as health.";

		/// <summary>
		/// How much is healed.
		/// </summary>
		public float Percentage { get => percentage; }

		/// <summary>
		/// How much extra is healed per level
		/// So total heal = (Percentage) + (PercentageExtraLevel * level)
		/// </summary>
		public float PercentageExtraPerLevel { get => percentageExtraPerLevel; }

		public void OnEntityHit(Entity entity, Entity other, ushort level)
		{
			if (entity is ProjectileEntity projectile && other is ShipEntity)
			{
				float amountToHeal = projectile.Damage * Mathf.Clamp01(Percentage + (PercentageExtraPerLevel * level));

				var amount = Mathf.RoundToInt(amountToHeal) * -1;
				projectile.Weapon.AttatchedModule.Ship.Health.Damage(amount);
			}
		}
	}
}