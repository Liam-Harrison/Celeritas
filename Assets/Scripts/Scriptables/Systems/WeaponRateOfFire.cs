using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Rate Of Fire Modifier", menuName = "Celeritas/Modifiers/Weapon Rate Of Fire")]
public class WeaponRateOfFire : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved
{
	[SerializeField, Title("Rate Of Fire Percentage (0.5 would add 50%)")]
	private float percentage;

	[SerializeField, Title("Extra Percentage to add per level")]
	private float percentageExtraPerLevel;
	public override bool Stacks => false;

	public override SystemTargets Targets => SystemTargets.Weapon;

	public void OnEntityEffectAdded(Entity entity, ushort level)
	{
		WeaponEntity weapon = (WeaponEntity)entity;
		Debug.Log(weapon.RateOfFire);
		float totalPercent = percentage + (level * percentageExtraPerLevel);
		Debug.Log("adding percent: " + totalPercent);
		float amountToAdd = totalPercent * weapon.RateOfFire;
		weapon.RateOfFire += (uint)(amountToAdd);
		Debug.Log(weapon.RateOfFire);
	}

	public void OnEntityEffectRemoved(Entity entity, ushort level)
	{
		WeaponEntity weapon = (WeaponEntity)entity;
		float totalPercent = percentage + (level * percentageExtraPerLevel);
		float amountToSubtract = totalPercent * weapon.RateOfFire;
		weapon.RateOfFire -= (uint)(amountToSubtract);
	}
}
