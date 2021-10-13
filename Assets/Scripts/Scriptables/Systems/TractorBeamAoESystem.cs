using Assets.Scripts.Game.Controllers;
using Celeritas.Game;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// This will allow the player's tractor beam to affect an area (scaling with level) of entities rather than just 1
	/// </summary>
	[CreateAssetMenu(fileName = "New Tractor Area Of Effect System", menuName = "Celeritas/Modifiers/Tractor/Area Of Effect")]
	class TractorBeamAoESystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved, IEntityLevelChanged
	{
		public override bool Stacks => false;

		[SerializeField] float initialRangeMultiplier;
		[SerializeField] float extraRangePerLevel;

		public override SystemTargets Targets => SystemTargets.Ship;

		public override string GetTooltip(int level) => $"Tractor beam now affects <color=green>{(initialRangeMultiplier + (extraRangePerLevel * level))*100:0}%</color> more area and can target multiple objects simultaneously.";

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{
			if (!entity.PlayerShip)
				return;

			TractorBeamController.Instance.UseAreaOfEffect(true, calculateRangeMultiplier(wrapper.Level));
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			if (!entity.PlayerShip)
				return;

			TractorBeamController.Instance.UseAreaOfEffect(false, 1/calculateRangeMultiplier(wrapper.Level));
		}

		private float calculateRangeMultiplier(int level)
		{
			return initialRangeMultiplier + (level * extraRangePerLevel);
		}

		public void OnLevelChanged(Entity entity, int previous, int newLevel, EffectWrapper effectWrapper)
		{
			if (!entity.PlayerShip)
				return;

			// revert old level, apply new level (just in case tractor beam controller is doing anything extra)
			TractorBeamController.Instance.UseAreaOfEffect(false, 1 / calculateRangeMultiplier(previous));
			TractorBeamController.Instance.UseAreaOfEffect(true, calculateRangeMultiplier(newLevel));
		}
	}
}
