using Celeritas.Game;
using Celeritas.Game.Entities;
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
	/// Sets a projectile's lifetime to be proportional to its weapon's charge level
	/// </summary>
	[CreateAssetMenu(fileName = "New ChargeLifetime Modifier", menuName = "Celeritas/Modifiers/LifetimeProportionalToWeaponCharge")]
	class SetLifetimeProportionalToChargeSystem : ModifierSystem, IEntityEffectAdded
	{
		[SerializeField]
		float multiplier;

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level) => $"";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			ProjectileEntity projectile = entity as ProjectileEntity;
			WeaponEntity weapon = projectile.Weapon;
			projectile.ProjectileData.Lifetime = multiplier * weapon.Charge;
		}
	}
}
