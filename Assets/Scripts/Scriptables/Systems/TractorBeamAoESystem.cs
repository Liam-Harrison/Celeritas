using Assets.Scripts.Game.Controllers;
using Celeritas.Game;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		public override string GetTooltip(ushort level) => $"Tractor beam now affects {10}% more area and can target {4} objects simultaneously.";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			if (!entity.PlayerShip)
				return;

			TractorBeamController.Instance.UseAreaOfEffect(true, calculateRangeMultiplier(level));
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			if (!entity.PlayerShip)
				return;

			TractorBeamController.Instance.UseAreaOfEffect(false, 1/calculateRangeMultiplier(level));
		}

		private float calculateRangeMultiplier(ushort level)
		{
			return initialRangeMultiplier + (level * extraRangePerLevel);
		}
	}
}
