using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	[CreateAssetMenu(fileName = "New Damage Modifier System", menuName = "Celeritas/Modifiers/Damage Modifier")]
	public class DamageModifierSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{
		[SerializeField, PropertyRange(-1, 1), TitleGroup("Damage Reduction")]
		private float baseAmount;

		[SerializeField, PropertyRange(-1, 1), TitleGroup("Damage Reduction")]
		private float amountPerLevel;

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level) => $"Damage {(GetReduction(level) >= 0 ? "increased" : "decreased")} by <color={(GetReduction(level) >= 0 ? "green" : "red")}>{GetReduction(level) * 100:0}%</color>.";

		/// <summary>
		/// How much damage is changed.
		/// </summary>
		public float BaseAmount { get => baseAmount; }

		/// <summary>
		/// How much extra is healed per level
		/// So total heal = (Percentage) + (PercentageExtraLevel * level)
		/// </summary>
		public float AmountPerLevel { get => amountPerLevel; }

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{
			if (entity is ProjectileEntity projectile)
			{
				projectile.Damage += Mathf.RoundToInt(projectile.ProjectileData.Damage * GetReduction(wrapper.Level));
			}
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			if (entity is ProjectileEntity projectile)
			{
				projectile.Damage -= Mathf.RoundToInt(projectile.ProjectileData.Damage * GetReduction(wrapper.Level));
			}
		}

		private float GetReduction(ushort level)
		{
			return baseAmount + (amountPerLevel * level);
		}
	}
}