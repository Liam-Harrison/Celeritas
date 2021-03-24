using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a ship.
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class ShipEntity : Entity
	{
		[SerializeField, Title("Modules")]
		private Module[] modules;

		/// <summary>
		/// The modules attatched to this ship.
		/// </summary>
		public Module[] Modules { get => modules; }

		/// <summary>
		/// Get all the module entities attatched to this ship.
		/// </summary>
		public List<ModuleEntity> ModuleEntities
		{
			get
			{
				var list = new List<ModuleEntity>();
				foreach (var module in Modules)
				{
					if (module.HasModuleAttatched)
						list.Add(module.AttatchedModule);
				}
				return list;
			}
		}

		/// <summary>
		/// Get all the weapon entities attatched to this ship.
		/// </summary>
		public List<WeaponEntity> WeaponEntities
		{
			get
			{
				var list = new List<WeaponEntity>();
				foreach (var module in Modules)
				{
					if (module.HasModuleAttatched && module.AttatchedModule is WeaponEntity)
						list.Add(module.AttatchedModule as WeaponEntity);
				}
				return list;
			}
		}

		/// <summary>
		/// The attatched ship data.
		/// </summary>
		public ShipData ShipData { get; private set; }

		/// <summary>
		/// The ships Rigidbody.
		/// </summary>
		public Rigidbody2D Rigidbody { get; private set; }

		/// <summary>
		/// The current aim target for this ship.
		/// </summary>
		public Vector3 Target { get; set; }

		/// <summary>
		/// The current translation input for this ship.
		/// </summary>
		public Vector3 Translation { get; set; }

		/// <summary>
		/// The current velocity of this ship.
		/// </summary>
		public Vector3 Velocity { get; private set; }

		/// <summary>
		/// Initalize this entity.
		/// </summary>
		/// <param name="data"></param>
		public override void Initalize(ScriptableObject data)
		{
			Rigidbody = GetComponent<Rigidbody2D>();
			ShipData = data as ShipData;

			ApplyRigidbodySettings();

			foreach (var module in modules)
			{
				module.AttatchTo(this);
			}

			base.Initalize(data);
		}

		protected virtual void Update()
		{
			if (!IsInitalized)
				return;

			TranslationLogic();
			RotationLogic();
			WeaponAim();
		}

		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + Velocity);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position, Target);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, transform.position + Up);
		}

		private void TranslationLogic()
		{
			Velocity = Up * ((Mathf.Max(Translation.y, 0) * ShipData.MovementSettings.forwardForcePerSec) +
							(Mathf.Min(Translation.y, 0) * ShipData.MovementSettings.backForcePerSec)) * Time.smoothDeltaTime;

			Velocity += Right * Translation.x * ShipData.MovementSettings.sideForcePerSec * Time.smoothDeltaTime;

			Rigidbody.AddForce(Velocity, ForceMode2D.Force);
		}

		private void RotationLogic()
		{
			var dir = (Target - transform.position).normalized;
			var dot = Vector3.Dot(Up, dir);

			if (dot < ShipData.MovementSettings.aimDeadzone)
			{
				var torquePerSec = ShipData.MovementSettings.torquePerSec;
				var torque = Mathf.Lerp(torquePerSec.x, torquePerSec.y, ShipData.MovementSettings.rotationCurve.Evaluate(Mathf.InverseLerp(1, -1, dot))) * Time.smoothDeltaTime;

				if (Vector3.Dot(Right, dir) >= 0)
					torque = -torque;

				var absAngular = Mathf.Abs(Rigidbody.angularVelocity);
				if (absAngular < ShipData.MovementSettings.rotationMaximum || (absAngular < 1 && Mathf.Sign(torque) != Mathf.Sign(Rigidbody.angularVelocity)))
				{
					Rigidbody.AddTorque(torque, ForceMode2D.Force);
				}
			}
		}

		private void WeaponAim()
		{
			foreach (var weapon in WeaponEntities)
			{
				if (weapon.WeaponData.Aims)
				{
					var dir = Target - weapon.Position;
					weapon.transform.rotation = Quaternion.LookRotation(dir, Vector3.back);
				}
			}
		}

		protected void ApplyRigidbodySettings()
		{
			Rigidbody.gravityScale = 0;
			Rigidbody.angularDrag = ShipData.MovementSettings.angularDrag;
			Rigidbody.mass = ShipData.MovementSettings.mass;
		}
	}
}
