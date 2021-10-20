using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace Celeritas.Scriptables.Systems
{
	[CreateAssetMenu(fileName = "New Teleport Modifier", menuName = "Celeritas/Modifiers/Teleport")]
	public class ShipBlinkSystem : ModifierSystem
	{

		/// <inheritdoc/>
		public override bool Stacks => true;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Ship;

		/// <inheritdoc/>
		public override string GetTooltip(int level) => $"Adds ability to teleport.";
	}
}