using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.Scriptables.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Scriptables.Systems
{
	/// <summary>
	/// Makes an entity orbit around another entity
	/// </summary>

	[CreateAssetMenu(fileName = "New OrbitMotion System", menuName = "Celeritas/Modifiers/Entity Movement/Orbit Around Another Entity")]
	class OrbitAroundOwnerSystem : ModifierSystem, IEntityUpdated
	{
		[SerializeField]
		bool orbitAroundParentProjectile; // if false, will orbit around owner.

		[SerializeField]
		float degreesPerSecond;

		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Projectile;

		public override string GetTooltip(ushort level)
		{
			return "Causes an entity to orbit around its creator";
		}

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			ProjectileEntity projectile = entity as ProjectileEntity;
			Entity toOrbitAround = projectile.Owner;
			if (orbitAroundParentProjectile && projectile.ParentProjectile != null)
				toOrbitAround = projectile.ParentProjectile;
			//'forward' appears to work as 'up'.
			entity.transform.RotateAround(toOrbitAround.transform.position, toOrbitAround.transform.forward, degreesPerSecond * Time.deltaTime);
		}
	}
}
