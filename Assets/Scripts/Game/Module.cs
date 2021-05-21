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
		[InfoBox("Provided default module type does not match designed type.", InfoMessageType.Error,
			VisibleIf = "@this.module != null && this.module is WeaponData != isWeapon")]
		private bool isWeapon;

		[SerializeField, Title("Module"), DisableInPlayMode, InfoBox("Cannot modify in playmode.", VisibleIf = "@UnityEngine.Application.isPlaying")]
		private bool hasDefaultModule;

		[SerializeField, ShowIf(nameof(hasDefaultModule)), DisableInPlayMode]
		private ModuleData module;

		[SerializeField, ShowIf(nameof(hasDefaultModule)), Title("Module Effects"), DisableInPlayMode]
		private EffectWrapper[] moduleEffects;

		[SerializeField, ShowIf("@this.module != null && this.module is WeaponData && this.hasDefaultModule && this.isWeapon"), Title("Projectile Effects"), DisableInPlayMode]
		private EffectWrapper[] projectileEffects;

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

		/// <summary>
		/// Initalize this module and attach it to a ship.
		/// </summary>
		/// <param name="ship">The ship to attach to.</param>
		public void Initalize(ShipEntity ship)
		{
			Ship = ship;
			
			if (hasDefaultModule)
			{
				SetModule(module, moduleEffects);
			}
		}

		/// <summary>
		/// Set the current module to be of the new provided data.
		/// </summary>
		/// <param name="module">The module data to set.</param>
		public void SetModule(ModuleData module, EffectWrapper[] effects = null)
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

			RemoveModule();

			AttatchedModule = EntityDataManager.InstantiateEntity<ModuleEntity>(module, Ship, effects);
			AttatchedModule.AttatchTo(this);

			if (AttatchedModule is WeaponEntity weapon)
			{
				weapon.ProjectileEffects.AddEffectRange(projectileEffects);
			}

			if (module.HasShipEffects)
			{
				Ship.EntityEffects.AddEffectRange(module.ShipEffects);
			}

			if (module.HasWeaponEffects || module.HasProjectileEffects)
			{
				foreach (var entity in Ship.WeaponEntities)
				{
					if (module.HasWeaponEffects)
						entity.EntityEffects.AddEffectRange(module.WeaponEffects);

					if (module.HasProjectileEffects)
						entity.ProjectileEffects.AddEffectRange(module.ProjectileEffects);
				}
			}
		}

		/// <summary>
		/// Remove the currently attatched module entity from this module.
		/// </summary>
		public void RemoveModule()
		{
			if (AttatchedModule != null)
			{
				if (AttatchedModule.ModuleData.HasShipEffects)
				{
					Ship.EntityEffects.RemoveEffectRange(AttatchedModule.ModuleData.ShipEffects);
				}

				if (AttatchedModule.ModuleData.HasWeaponEffects || AttatchedModule.ModuleData.HasProjectileEffects)
				{
					foreach (var entity in Ship.WeaponEntities)
					{
						if (AttatchedModule.ModuleData.HasWeaponEffects)
							entity.EntityEffects.RemoveEffectRange(AttatchedModule.ModuleData.WeaponEffects);

						if (AttatchedModule.ModuleData.HasProjectileEffects)
							entity.ProjectileEffects.RemoveEffectRange(AttatchedModule.ModuleData.ProjectileEffects);
					}
				}

				Destroy(AttatchedModule.gameObject);

				AttatchedModule = null;
			}
		}

		public void IncreaseEffectLevel()
		{
			if (AttatchedModule != null)
			{
				if (AttatchedModule.ModuleData.HasShipEffects)
				{
					Ship.EntityEffects.IncreaseEffectLevel();
				}

				if (AttatchedModule.ModuleData.HasWeaponEffects || AttatchedModule.ModuleData.HasProjectileEffects)
				{
					foreach (var entity in Ship.WeaponEntities)
					{
						if (AttatchedModule.ModuleData.HasWeaponEffects)
							entity.EntityEffects.IncreaseEffectLevel();

						if (AttatchedModule.ModuleData.HasProjectileEffects)
							entity.ProjectileEffects.IncreaseEffectLevel();
					}
				}
			}
		}

		public void DecreaseEffectLevel()
		{
			if (AttatchedModule != null)
			{
				if (AttatchedModule.ModuleData.HasShipEffects)
				{
					Ship.EntityEffects.DecreaseEffectLevel();
				}

				if (AttatchedModule.ModuleData.HasWeaponEffects || AttatchedModule.ModuleData.HasProjectileEffects)
				{
					foreach (var entity in Ship.WeaponEntities)
					{
						if (AttatchedModule.ModuleData.HasWeaponEffects)
							entity.EntityEffects.DecreaseEffectLevel();

						if (AttatchedModule.ModuleData.HasProjectileEffects)
							entity.ProjectileEffects.DecreaseEffectLevel();
					}
				}
			}
		}
	}
}
