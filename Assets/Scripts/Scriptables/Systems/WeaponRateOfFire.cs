using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Rate Of Fire Modifier", menuName = "Celeritas/Modifiers/Weapon Rate Of Fire")]
public class WeaponRateOfFire : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved, IEntityLevelChanged
{
	[SerializeField, Title("Rate Of Fire Percentage (0.5 would add 50%)")]
	private float percentage;

	[SerializeField, Title("Extra Percentage to add per level")]
	private float percentageExtraPerLevel;

	/// <inheritdoc/>
	public override bool Stacks => false;

	/// <inheritdoc/>
	public override SystemTargets Targets => SystemTargets.Weapon;

	/// <inheritdoc/>
	public override string GetTooltip(int level) => $"Increase fire rate by <color=green>{(percentage + (percentageExtraPerLevel * level)) * 100:0}%</color>.";

	public void OnEntityEffectAdded(Entity entity, EffectWrapper wrapper)
	{
		Debug.Log("added");
		WeaponEntity weapon = (WeaponEntity)entity;
		float totalPercent = percentage + (wrapper.Level * percentageExtraPerLevel);
		weapon.RateOfFire *= (totalPercent + 1);
	}

	public void OnEntityEffectRemoved(Entity entity, EffectWrapper wrapper)
	{
		Debug.Log("removed");
		WeaponEntity weapon = (WeaponEntity)entity;
		float totalPercent = percentage + (wrapper.Level * percentageExtraPerLevel);
		weapon.RateOfFire /= (totalPercent + 1);
	}

	public void OnLevelChanged(Entity entity, int previous, int newLevel, EffectWrapper effectWrapper)
	{
		Debug.Log("levelchanged");
		// revert rate of fire
		WeaponEntity weapon = (WeaponEntity)entity;
		float oldPercent = percentage + (previous * percentageExtraPerLevel);
		weapon.RateOfFire /= (oldPercent + 1);

		// apply new rate of fire
		float newPercent = percentage + (newLevel * percentageExtraPerLevel);
		weapon.RateOfFire *= (newPercent + 1);
	}
}
