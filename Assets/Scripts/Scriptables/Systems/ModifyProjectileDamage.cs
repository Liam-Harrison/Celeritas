using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems {

	/// <summary>
	/// System to modify how much damage a projectile does
	/// </summary>
	[CreateAssetMenu(fileName = "New Projectile Damage Modifier", menuName = "Celeritas/Modifiers/Projectile Damage")]
	public class ModifyProjectileDamage : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{
		[SerializeField, Title("Percentage Extra Projectile Damage"), InfoBox("Percentage to add initially. eg 0.5 would add 50% extra damage.")]
		private float percentage;

		[SerializeField, InfoBox("Percentage extra to add per module level. eg 0.1 would add an extra 20% damage at level 2.")]
		private float percentageExtraPerLevel;

		/// <inheritdoc/>
		public override bool Stacks => false;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Projectile;

		/// <inheritdoc/>
		public override string GetTooltip(ushort level) => $"<i>Missing</i>";

		/// <summary>
		/// How much extra percent damage the projectile gets from this modifier initially
		/// </summary>
		public float Percentage { get => percentage; }

		/// <summary>
		/// How much extra percent damage the projectile gets from this modifier, per level
		/// So total extra percent damage == (Percentage) + (PercentageExtraLevel * level)
		/// </summary>
		public float PercentageExtraPerLevel { get => percentageExtraPerLevel; }

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			var projectile = entity as ProjectileEntity;
			float percentageToAdd = percentage + (level * percentageExtraPerLevel);
			int amountToAdd = Mathf.RoundToInt(projectile.Damage * (percentageToAdd));

			projectile.Damage += amountToAdd;
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			var projectile = entity as ProjectileEntity;
			float percentageToSubtract = percentage + (level * percentageExtraPerLevel);
			int amountToSubtract = Mathf.RoundToInt(projectile.Damage * (percentageToSubtract));

			projectile.Damage -= amountToSubtract;
		}
	}
}
