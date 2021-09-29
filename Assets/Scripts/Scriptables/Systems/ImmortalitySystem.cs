using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{

	/// <summary>
	/// Grants temporary immortality.
	/// </summary>

	[CreateAssetMenu(fileName = "New Immortality Modifier", menuName = "Celeritas/Modifiers/Immortality")]

	public class ImmortalitySystem : ModifierSystem
	{
		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Ship;

		public override string GetTooltip(ushort level) => $"Grants temporary immortality.";
	}
}