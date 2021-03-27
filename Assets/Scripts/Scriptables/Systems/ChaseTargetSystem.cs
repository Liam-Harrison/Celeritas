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

		/// <summary>
		/// The change in angle per second this modifier applies.
		/// </summary>
		public float AngPerSec { get => angPerSec; }

		public void OnEntityUpdated(Entity target)
		{
			var projectile = target as ProjectileEntity;
			var dir = (projectile.Weapon.AttatchedModule.Ship.Target - target.transform.position).normalized;

			if (Vector3.Dot(target.transform.forward, dir) >= 0.95)
				return;

			var angle = AngPerSec * Time.smoothDeltaTime;
			if (Vector3.Dot(target.transform.right, dir) < 0)
			{
				angle = -angle;
			}

			target.transform.rotation *= Quaternion.Euler(0, angle, 0);
		}
	}
}
