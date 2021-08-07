using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	/// <summary>
	/// System that causes a chain of entities to follow one-another
	/// Original intended use: to make many laser projectiles form a continuous 'line'.
	/// Note: could be used to make cool enemy behaviour too
	/// </summary>
	[CreateAssetMenu(fileName = "New FollowLeader Modifier", menuName = "Celeritas/Modifiers/FollowLeader(projectile chain)")]
	class FollowTheLeaderSystem : ModifierSystem, IEntityEffectAdded, IEntityUpdated
	{
		public override bool Stacks => false;

		private Entity leader = null;

		public override SystemTargets Targets => SystemTargets.Projectile;



		[SerializeField, Title("Chase Settings")]
		private float angPerSec;

		[SerializeField, PropertyRange(0, 180)]
		private float maximumAngle = 180;

		/// <summary>
		/// The change in angle per second this modifier applies.
		/// </summary>
		public float AngPerSec { get => angPerSec; }

		/// <summary>
		/// The maximum angle this chase can move relative to world.
		/// </summary>
		public float MaximumAngle { get => maximumAngle; }

		public override string GetTooltip(ushort level)
		{
			return "placeholder";
		}

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			ProjectileEntity projectile = entity as ProjectileEntity;
			// if a leader exists, follow it
			if (leader != null && !leader.Dying && leader.IsInitalized && leader.gameObject.activeInHierarchy)
			{
				projectile.Following = leader;
			}
			else
				projectile.Following = null;

			leader = entity;
		}

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			
			ProjectileEntity projectile = entity as ProjectileEntity;
			if (projectile.Following != null)
			{
				// follow your leader if one exists
				// chase logic copied from ChaseTargetSystem
				Vector3 target = projectile.Following.Position;

				var dir = (target - entity.Position).normalized;

				//if (Vector3.Dot(entity.Forward, dir) >= 0.975)
				//	return;

				var angle = (AngPerSec) * Time.smoothDeltaTime;
				if (Vector3.Dot(entity.Right, dir) > 0)
				{
					angle = -angle;
				}

				var remainder = Mathf.Abs(entity.transform.rotation.eulerAngles.z) - MaximumAngle;

				var rotation = entity.transform.localRotation * Quaternion.Euler(0, 0, angle);

				entity.transform.localRotation = rotation;
			}
		}
	}
}
