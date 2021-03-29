using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// A container for module meta-data. Used on ships to setup module locations and settings.
	/// </summary>
	public class Module : MonoBehaviour
	{
		[SerializeField, Title("Settings")]
		private ModuleSize size;

		[SerializeField]
		private bool isWeapon;

		[SerializeField, Title("Module")]
		private bool useDefault;

		[SerializeField, ShowIf(nameof(useDefault))]
		private ModuleData defaultModule;

		/// <summary>
		/// The size of this module.
		/// </summary>
		public ModuleSize ModuleSize { get => size; }

		/// <summary>
		/// Is this a weapons module.
		/// </summary>
		public bool IsWeapon { get => isWeapon; }

		/// <summary>
		/// Get the attatched module entity to this module.
		/// </summary>
		public ModuleEntity AttatchedModule { get; private set; }

		/// <summary>
		/// Get the ship this module is attatched to.
		/// </summary>
		public ShipEntity Ship { get; private set; }

		/// <summary>
		/// Get if this module has an entity attatched or not.
		/// </summary>
		public bool HasModuleAttatched { get => AttatchedModule != null; }

		private void Awake()
		{
			if (useDefault)
			{
				SetModule(defaultModule);
			}
		}

		/// <summary>
		/// Attatch this module to an owner ship.
		/// </summary>
		/// <param name="ship">The ship to attach to.</param>
		public void AttatchTo(ShipEntity ship)
		{
			Ship = ship;
		}

		public void SetModule(ModuleData module)
		{
			if (IsWeapon && ((module is WeaponData) == false))
			{
				Debug.LogError($"Tried to set weapon module to {module.GetType().Name}", this);
				return;
			}
			else if (IsWeapon == false && module is WeaponData)
			{
				Debug.LogError($"Tried to set normal module to {module.GetType().Name}", this);
				return;
			}

			if (AttatchedModule != null)
				Destroy(AttatchedModule);

			AttatchedModule = EntityManager.InstantiateEntity<ModuleEntity>(defaultModule);
			AttatchedModule.AttatchTo(this);
		}
	}
}
