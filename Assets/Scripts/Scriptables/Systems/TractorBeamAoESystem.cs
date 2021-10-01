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
	class TractorBeamAoESystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{
		public override bool Stacks => false;

		[SerializeField] float initialRangeMultiplier;
		[SerializeField] float extraRangePerLevel;

		public override SystemTargets Targets => SystemTargets.Ship;

		// PLACEHOLDER, PLEASE UPDATE WHEN WE HAVE DESIRED BEHAVIOUR FINALISED
		public override string GetTooltip(ushort level) => $"Tractor beam now affects <color=\"green\">{10}%</color> more area and can target <color=\"green\">{4}</color> objects simultaneously.";

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

		private float calculateRangeMultiplier(ushort level)
		{
			return initialRangeMultiplier + (level * extraRangePerLevel);
		}
	}
}
