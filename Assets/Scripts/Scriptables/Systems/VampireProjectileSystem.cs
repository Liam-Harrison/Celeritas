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
	public class VampireProjectileSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{
		[SerializeField, Title("Percentage of damage dealt healed.", "Percentage a projectile will heal the player if it hits an enemy ship.")]
		private float percentage;

		[SerializeField, Title("Percentage of damage sacrificed", "Percentage of damage that will be lost in exchange for the heal.")]
		private float damageReducedBy;

		[SerializeField, Title("Percentage of bonus heal per level.", "Example: If original heal is 5%, a value of 2 will increase the heal to 7%.")]
		private float percentageExtraPerLevel;

		[SerializeField, Title("Percentage of damage sacrificed", "Percentage of damage that will be lost in exchange for the heal.")]
		private float damageReducedByPerLevel;

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Projectile | SystemTargets.Ship;

		public override string GetTooltip(ushort level) => $"Sacrifice <color=red>{damageReducedBy + (damageReducedByPerLevel * level)}%</color> damage dealt but projectiles will now restore <color=green>{Percentage + (PercentageExtraPerLevel * level)}%</color> health proportional to the amount of damage done";

		/// <summary>
		/// How much is healed.
		/// </summary>
		public float Percentage { get => percentage; }

		/// <summary>
		/// How much projectile damage is reduced by.
		/// </summary>
		public float DamageReducedBy { get => damageReducedBy; }

		/// <summary>
		/// How much extra is healed per level
		/// So total heal = (Percentage) + (PercentageExtraLevel * level)
		/// </summary>
		public float PercentageExtraPerLevel { get => percentageExtraPerLevel; }

		/// <summary>
		/// How much projectile damage penalty is reduced by per level
		/// So total projectile damage reduction = (DamageReducedBy) - (DamageReducedByPerLevel * level)
		/// </summary>
		public float DamageReducedByPerLevel { get => damageReducedByPerLevel; }

		private ShipEntity targetShip = null;

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			if (entity is ShipEntity ship)
			{
				targetShip = ship;
			}

			if (entity is ProjectileEntity projectile)
			{
				float baseDamage = (float)projectile.Damage;
				projectile.Damage = projectile.Damage - Mathf.RoundToInt((baseDamage / 100.0f) * (DamageReducedBy - (DamageReducedByPerLevel * level)));
				float amountToHeal = ((baseDamage / 100.0f) * (Percentage + (PercentageExtraPerLevel * level)));
				if (targetShip != null)
				{
					if (targetShip.Health.CurrentValue < targetShip.Health.MaxValue)
					{
					projectile.HealOnHit = (Mathf.RoundToInt(amountToHeal) * -1);
					projectile.HealShip = targetShip;
					}
				}
			}
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			entity.Components.ReleaseComponent<VampireProjectileSystem>(this);
		}
	}
}