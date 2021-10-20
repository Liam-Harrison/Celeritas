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
		/// <inheritdoc/>
		public override bool Stacks => true;

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Ship;

		/// <inheritdoc/>
		public override string GetTooltip(int level) => $"Grants temporary immortality.";
	}
}