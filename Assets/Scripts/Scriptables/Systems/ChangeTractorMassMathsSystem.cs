using Assets.Scripts.Game.Controllers;
using Celeritas.Game;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// Changes how 'heavy' the tractor beam considers whatever it is holding.
	/// Note that all values are multipliers. 1 = 100%
	/// Effectively makes the tractor beam stronger
	/// </summary>
	[CreateAssetMenu(fileName = "New Tractor Strength System", menuName = "Celeritas/Modifiers/Tractor/Mass")]
	class ChangeTractorMassMathsSystem: ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
	{

		[SerializeField]
		private float startingMultiplier;

		[SerializeField]
		private float amountToAddPerLevel;
		// also a multiplier. so 0.2 == add 20% each level
		// so if starting multiplier = 0.5, and toAdd is 0.1, level 1 = 0.5, level 2 = 0.6, ect.

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Ship;

		public override string GetTooltip(ushort level) => $"Reduces the mass of tractor-beamed objects by {startingMultiplier + (amountToAddPerLevel * level) * 100}%";

		public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
		{
			// check if entity is player
			if (entity.PlayerShip)
			{
				TractorBeamController.Instance.TargetMassMultiplier += factorToAdd();
			}
		}

		public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
		{
			if (entity.PlayerShip)
			{
				TractorBeamController.Instance.TargetMassMultiplier -= factorToAdd();
			}
		}

		private float factorToAdd()
		{
			return startingMultiplier + (amountToAddPerLevel * amountToAddPerLevel);
		}

	}
}
