using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	[CreateAssetMenu(fileName = "New Nuke System", menuName = "Celeritas/Modifiers/Nuke Modifier")]
	public class NukeSystem : ModifierSystem
	{
		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Ship;

		public override string GetTooltip(int level) => $"Adds ability to convert a large amount of Scrap Metal into unstable quantum explosion.";
	}
}