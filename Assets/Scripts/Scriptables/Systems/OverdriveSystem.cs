using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Overdrive Modifier", menuName = "Celeritas/Modifiers/Overdrive")]
public class OverdriveSystem : ModifierSystem
{
	/// <inheritdoc/>
	public override bool Stacks => true;

	/// <inheritdoc/>
	public override SystemTargets Targets => SystemTargets.Weapon;

	/// <inheritdoc/>
	public override string GetTooltip(int level) => $"Adds ability to temporarily increase rate of fire.";
}