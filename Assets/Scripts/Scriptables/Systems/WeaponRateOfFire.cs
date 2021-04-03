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
	[SerializeField, Title("Rate Of Fire")]
	private uint rateOfFire;

	[SerializeField, Title("Extra to add per level")]
	private uint rateOfFireToAddPerLevel;
	public override bool Stacks => false;

	public override SystemTargets Targets => SystemTargets.Weapon;

	public void OnEntityEffectAdded(Entity entity, ushort level)
	{
		WeaponEntity weapon = (WeaponEntity)entity;
		weapon.RateOfFire += rateOfFire + (level * rateOfFireToAddPerLevel);
	}

	public void OnEntityEffectRemoved(Entity entity, ushort level)
	{
		WeaponEntity weapon = (WeaponEntity)entity;
		weapon.RateOfFire -= rateOfFire + (level * rateOfFireToAddPerLevel);
	}
}
