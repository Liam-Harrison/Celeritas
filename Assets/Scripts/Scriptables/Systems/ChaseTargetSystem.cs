using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	/// <summary>
	/// Contains the instanced information for a chase modifier.
	/// </summary>
	[CreateAssetMenu(fileName = "New Chase Modifier", menuName = "Celeritas/Modifiers/Chase")]
	public class ChaseTargetSystem : ModifierSystem, IEntityUpdated
	{
		[SerializeField, Title("Chase Settings")]
		private float angPerSec;

		[SerializeField]
		private float angPerLevel;

		/// <summary>
		/// The change in angle per second this modifier applies.
		/// </summary>
		public float AngPerSec { get => angPerSec; }

		/// <summary>
		/// How much the angle per second increases per level.
		/// </summary>
		public float AngPerLevel { get => angPerLevel; }

		/// <inheritdoc/>
		public override SystemTargets Targets => SystemTargets.Projectile | SystemTargets.Weapon;

		/// <inheritdoc/>
		public override bool Stacks => false;

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			Vector3 target;
			
			if (entity is ProjectileEntity projectile)
			{
				target = projectile.Weapon.AttatchedModule.Ship.Target;
			}
			else if (entity is WeaponEntity weapon)
			{
				target = weapon.AttatchedModule.Ship.Target;
			}
			else
			{
				Debug.LogError($"Type {entity.GetType().Name} not supported on {nameof(ChaseTargetSystem)}");
				return;
			}

			var dir = (target - entity.transform.position).normalized;

			if (Vector3.Dot(entity.transform.forward, dir) >= 0.95)
				return;

			var angle = (AngPerSec + AngPerLevel * level) * Time.smoothDeltaTime;
			if (Vector3.Dot(entity.transform.right, dir) < 0)
			{
				angle = -angle;
			}

			entity.transform.rotation *= Quaternion.Euler(0, angle, 0);
		}
	}
}
