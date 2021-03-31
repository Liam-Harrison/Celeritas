using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Celeritas.Scriptables.Interfaces;
using UnityEngine;
using Celeritas.Game;
using Celeritas.Game.Entities;
using System.Linq;

namespace Celeritas.Scriptables.Systems
{
	/// <summary>
	/// contains infomration for a modifier that will modify a weapon's projectile count
	/// ie. how many projectiles fire per 'fire' command
	/// </summary>
	[CreateAssetMenu(fileName = "Projectile Count Modifier", menuName= "Celeritas/Modifiers/Projectile Count")]
	public class ModifyProjectileCount : ModifierSystem, IEntityCreated
	{
		[SerializeField, Title("Extra Projectile Count")]
		private uint extraProjectileCount;

		[SerializeField]
		private uint extraProjectilesPerLevel;

		/// <summary>
		/// How many extra projectiles will be fired per 'fire' command
		/// when system is at level 0
		/// </summary>
		public uint ExtraProjectileCount { get => extraProjectileCount; }

		/// <summary>
		/// How many extra projectiles will be added per level
		/// </summary>
		public uint ExtraProjectileCountPerLevel { get => extraProjectilesPerLevel; }

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public void OnEntityCreated(Entity entity, ushort level)
		{
			Debug.Log("woof");

			// when one bullet is instantiated
			// instantiate X others, where X = extraProjectileCount + level * countPerLevel

			ProjectileEntity projectile = (ProjectileEntity)entity;
			WeaponEntity weapon = projectile.Weapon;
			

			// copy projectile effects, remove the projectile count effect

			/*EffectWrapper[] desiredEffects = weapon.WeaponEffects.EffectWrapperCopy;
			foreach(EffectWrapper w in desiredEffects)
			{
				if (w.EffectCollection.Systems.Contains(this))
				{
					w.EffectCollection.Systems.Remove(this);
				}
			}*/

			// projectile count is a weapon effect, does not seem to move into projectile?
			// So this should theoretically avoid recursion
			// just do 1 extra projectile for now, for testing.
			// copying weapon's fire method.

			//weapon.Fire();
		}
	}
}
