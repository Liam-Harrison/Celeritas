using Celeritas.Extensions;
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
	public class ChaseTargetSystem : ModifierSystem, IEntityUpdated, ILevelChanged
	{
		[SerializeField, Title("Chase Settings")]
		private float angPerSec;

		[SerializeField]
		private float angPerLevel;

		[SerializeField, PropertyRange(0, 180)]
		private float maximumAngle = 180;

		/// <summary>
		/// The change in angle per second this modifier applies.
		/// </summary>
		public float AngPerSec { get => angPerSec; }

		/// <summary>
		/// How much the angle per second increases per level.
		/// </summary>
		public float AngPerLevel { get => angPerLevel; }

		/// <summary>
		/// The maximum angle this chase can move relative to world.
		/// </summary>
		public float MaximumAngle { get => maximumAngle; }

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

			var dir = (target - entity.Position).normalized;

			if (Vector3.Dot(entity.Forward, dir) >= 0.975)
				return;

			var angle = (AngPerSec + AngPerLevel * level) * Time.smoothDeltaTime;
			if (Vector3.Dot(entity.Right, dir) > 0)
			{
				angle = -angle;
			}

			var remainder = Mathf.Abs(entity.transform.rotation.eulerAngles.z) - MaximumAngle;

			var rotation = entity.transform.localRotation * Quaternion.Euler(0, 0, angle);

			if (entity is WeaponEntity && Mathf.Abs(Mathf.DeltaAngle(0, rotation.eulerAngles.z)) > MaximumAngle)
			{
				if (rotation.eulerAngles.z < 180)
					rotation = Quaternion.Euler(0, 0, MaximumAngle);
				else
					rotation = Quaternion.Euler(0, 0, -MaximumAngle);
			}

			entity.transform.localRotation = rotation;
		}

		public void OnLevelChanged(Entity entity, ushort previous, ushort level)
		{
			Debug.Log($"system changed from level {previous} to level {level}");
		}
	}
}
