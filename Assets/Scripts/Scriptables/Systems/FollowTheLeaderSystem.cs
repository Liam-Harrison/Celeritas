using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Scriptables.Systems
{
	/// <summary>
	/// System that causes a chain of entities to follow one-another
	/// Original intended use: to make many laser projectiles form a continuous 'line'.
	/// Note: could be used to make cool enemy behaviour too
	/// </summary>
	[CreateAssetMenu(fileName = "New FollowLeader Modifier", menuName = "Celeritas/Modifiers/FollowLeader(projectile chain)")]
	class FollowTheLeaderSystem : ModifierSystem, IEntityEffectAdded, IEntityEffectRemoved, IEntityUpdated
	{
		public class FollowLeaderData
		{
			public readonly List<Entity> projectiles = new List<Entity>();
		}

		public override bool Stacks => false;

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

		public override string GetTooltip(ushort level) => $"Chase first fired projectile.";

		public void OnEntityEffectAdded(Entity entity, ushort level)
		{
			ProjectileEntity projectile = entity as ProjectileEntity;

			if (projectile.Weapon.Components.TryGetComponent(this, out FollowLeaderData data))
			{
				projectile.Following = data.projectiles[data.projectiles.Count - 1];
				data.projectiles.Add(projectile);
			}
			else
			{
				data = new FollowLeaderData();
				data.projectiles.Add(projectile);
				projectile.Weapon.Components.RegisterComponent(this, data);
			}
		}

		public void OnEntityEffectRemoved(Entity entity, ushort level)
		{
			ProjectileEntity projectile = entity as ProjectileEntity;
			var data = projectile.Weapon.Components.GetComponent<FollowLeaderData>(this);

			data.projectiles.Remove(entity);

			if (data.projectiles.Count == 0)
			{
				projectile.Weapon.Components.ReleaseComponent<FollowLeaderData>(this);
			}
		}

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			ProjectileEntity projectile = entity as ProjectileEntity;
			var data = projectile.Weapon.Components.GetComponent<FollowLeaderData>(this);

			if (projectile.Following != null)
			{
				if (projectile == data.projectiles[0])
				{
					projectile.Following = null;
					return;
				}

				if (!data.projectiles.Contains(projectile.Following))
				{
					projectile.Following = data.projectiles[data.projectiles.IndexOf(projectile) - 1];
				}

				// follow your leader if one exists
				// chase logic copied from ChaseTargetSystem
				Vector3 target = projectile.Following.Position;

				var dir = (target - entity.Position).normalized;

				if (Vector3.Dot(entity.Forward, dir) >= 0.995)
					return;

				var angle = AngPerSec * Time.smoothDeltaTime;
				if (Vector3.Dot(entity.Right, dir) > 0)
				{
					angle = -angle;
				}

				var rotation = entity.transform.localRotation * Quaternion.Euler(0, 0, angle);
				entity.transform.localRotation = rotation;
			}
		}
	}
}
