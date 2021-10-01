using System.Collections;
using System.Collections.Generic;
using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	[CreateAssetMenu(fileName = "New Homing Modifier", menuName = "Celeritas/Modifiers/Homing")]
	public class HomingSystem : ModifierSystem, IEntityUpdated
	{
		[SerializeField, Title("Homing Settings")]
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

		public override bool Stacks => true;

		public override SystemTargets Targets => SystemTargets.Projectile | SystemTargets.Weapon;

		public override string GetTooltip(int level) => $"Homes in on target at <color=green>{AngPerSec + (AngPerLevel * level)}°</color> per second.";

		public void OnEntityUpdated(Entity entity, EffectWrapper wrapper)
		{
			ShipEntity closest = null;
			float distance = 0;
			var ships = EntityDataManager.GetEntities<ShipEntity>();
			foreach (var ship in ships)
			{
				if (!ship.IsPlayer)
				{
					var d = ship.Position - entity.Position;

					if (closest == null || d.sqrMagnitude < distance)
					{
						closest = ship;
						distance = d.sqrMagnitude;
					}
				}
			}

			if (closest == null) return;
			var dir = (closest.Position - entity.Position).normalized;

			if (Vector3.Dot(entity.Forward, dir) >= 0.975)
				return;

			var angle = (AngPerSec + AngPerLevel * wrapper.Level) * Time.smoothDeltaTime;
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
	}
}
