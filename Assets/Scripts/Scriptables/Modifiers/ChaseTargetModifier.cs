using Celeritas.Game;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a chase modifier.
	/// </summary>
	[CreateAssetMenu(fileName = "New Chase Modifier", menuName = "Celeritas/Modifiers/Chase")]
	public class ChaseTargetModifier : ModifierData
	{
		[SerializeField, Title("Chase Settings")]
		private float angPerSec;

		/// <summary>
		/// The change in angle per second this modifier applies.
		/// </summary>
		public float AngPerSec { get => angPerSec; }

		public override void OnEntityCreated(Entity target)
		{

		}

		public override void OnEntityDestroyed(Entity target)
		{

		}

		public override void OnEntityHit(Entity target, Entity other)
		{

		}

		public override void OnEntityUpdated(Entity target)
		{
			var projectile = target as ProjectileEntity;
			var d = projectile.Weapon.AttatchedModule.Ship.Target - target.transform.position;
			d = d.normalized;

			if (Vector3.Dot(target.transform.forward, d) >= 0.95)
				return;

			var angle = AngPerSec * Time.smoothDeltaTime;
			if (Vector3.Dot(target.transform.right, d) < 0)
			{
				angle = -angle;
			}

			target.transform.rotation *= Quaternion.Euler(0, angle, 0);
		}
	}
}
