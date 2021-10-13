using Celeritas.Game.Actions;
using Celeritas.Scriptables;
using Celeritas.UI.Tooltips;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a module.
	/// </summary>
	public class ModuleEntity : Entity, ITooltip
	{
		[SerializeField, Title("Module Settings")]
		private int level;

		[SerializeField, TitleGroup("Ship Effects"), InfoBox("Effects on modules use the module level above, the effect level is un-used here. Modules here affect the ship, its weapons and projectiles. Use default effects to give this specific entity effects.", InfoMessageType = InfoMessageType.Info), PropertySpace]
		private bool hasShipEffects;

		[SerializeField, TitleGroup("Ship Effects"), ShowIf(nameof(hasShipEffects))]
		private EffectWrapper[] shipEffects;

		[SerializeField, TitleGroup("Ship Weapon Effects")]
		private bool hasShipWeaponEffects;

		[SerializeField, TitleGroup("Ship Weapon Effects"), ShowIf(nameof(hasShipWeaponEffects))]
		private EffectWrapper[] shipWeaponEffects;

		[SerializeField, TitleGroup("Ship Projectile Effects")]
		private bool hasShipProjectileEffects;

		[SerializeField, TitleGroup("Ship Projectile Effects"), ShowIf(nameof(hasShipProjectileEffects))]
		private EffectWrapper[] shipProjectileEffects;

		[SerializeField, TitleGroup("Ship Actions")]
		private bool hasShipActions;

		[SerializeField, TitleGroup("Ship Actions"), ShowIf(nameof(hasShipActions))]
		private ActionData[] shipActions;

		/// <summary>
		/// Does this module add effects to the ship entity.
		/// </summary>
		public bool HasShipEffects { get => hasShipEffects; }

		/// <summary>
		/// The effects to add to the ship entity, if used.
		/// </summary>
		public EffectWrapper[] ShipEffects { get => shipEffects; }

		/// <summary>
		/// Does this module add effects to the ships weapon entities.
		/// </summary>
		public bool HasShipWeaponEffects { get => hasShipWeaponEffects; }

		/// <summary>
		/// The effects to add to the ships weapon entities, if used.
		/// </summary>
		public EffectWrapper[] ShipWeaponEffects { get => shipWeaponEffects; }

		/// <summary>
		/// Does this module add effects to the ships projectile entities.
		/// </summary>
		public bool HasShipProjectileEffects { get => hasShipProjectileEffects; }

		/// <summary>
		/// The effects to add to the ships projectile entities, if used.
		/// </summary>
		public EffectWrapper[] ShipProjectileEffects { get => shipProjectileEffects; }

		/// <summary>
		/// Does this module add actions to the ship.
		/// </summary>
		public bool HasShipActions { get => hasShipActions; }

		/// <summary>
		/// The actions to attach to the ship.
		/// </summary>
		public ActionData[] ShipActions {  get => shipActions; }

		/// <summary>
		/// The attatched module data.
		/// </summary>
		public ModuleData ModuleData { get; private set; }

		/// <summary>
		/// The module that this entity is attatched to.
		/// </summary>
		public Module AttatchedModule { get; private set; }

		/// <inheritdoc/>
		public override SystemTargets TargetType { get => SystemTargets.Module; }

		/// <inheritdoc/>
		public override string Subheader => $"Level {Level + 1} {ModuleData.ModuleCatagory} {ModuleData.ModuleSize} Module";

		/// <summary>
		/// The level of this module.
		/// </summary>
		public int Level { get => level; private set => level = value; }

		///<inheritdoc/>
		public ModuleEntity TooltipEntity => this;

		///<inheritdoc/>
		public GameAction TooltipAction => null;

		/// <inheritdoc/>
		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			ModuleData = data as ModuleData;
			base.Initalize(data, owner, effects, forceIsPlayer, instanced);
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			if (ModuleData != null)
			{
				for (int x = 0; x < ModuleData.TetrisShape.ModuleShape().GetLength(0); x++)
				{
					for (int y = 0; y < ModuleData.TetrisShape.ModuleShape().GetLength(1); y++)
					{
						if (ModuleData.TetrisShape.ModuleShape()[x,y])
						{
							var pos = new Vector3(x, y, 0);
							Gizmos.DrawWireCube(pos, Vector3.one);
						}
					}
				}
			}
		}

		/// <summary>
		/// Attatch this entity to a module.
		/// </summary>
		/// <param name="module">The module to attatch this entity to.</param>
		public void AttatchTo(Module module)
		{
			AttatchedModule = module;

			transform.parent = module.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;

			if (HasShipEffects)
			{
				AttatchedModule.Ship.EntityEffects.AddEffectRange(ShipEffects);
			}

			if (HasShipWeaponEffects || HasShipProjectileEffects)
			{
				foreach (var entity in AttatchedModule.Ship.WeaponEntities)
				{
					if (HasShipWeaponEffects)
						entity.EntityEffects.AddEffectRange(ShipWeaponEffects);

					if (HasShipProjectileEffects)
						entity.ProjectileEffects.AddEffectRange(ShipProjectileEffects);
				}
			}

			if (HasShipActions)
			{
				foreach (var action in ShipActions)
				{
					AttatchedModule.Ship.AddAction(action);
				}
			}
		}

		/// <summary>
		/// Detatch this entity from the specified module.
		/// </summary>
		/// <param name="module">The module to detatch from.</param>
		public void DetatchFrom(Module module)
		{
			if (HasShipEffects)
			{
				module.Ship.EntityEffects.RemoveEffectRange(ShipEffects);
			}

			if (HasShipWeaponEffects || HasShipProjectileEffects)
			{
				foreach (var entity in module.Ship.WeaponEntities)
				{
					if (HasShipWeaponEffects)
						entity.EntityEffects.RemoveEffectRange(ShipWeaponEffects);

					if (HasShipProjectileEffects)
						entity.ProjectileEffects.RemoveEffectRange(ShipProjectileEffects);
				}
			}

			if (HasShipActions)
			{
				foreach (var action in ShipActions)
				{
					foreach (var a in AttatchedModule.Ship.Actions.ToArray())
					{
						if (a.Data == action)
						{
							AttatchedModule.Ship.RemoveAction(a);
						}
					}
				}
			}
		}

		/// <summary>
		/// Set the level of this module and update any effects.
		/// </summary>
		/// <param name="level">The level to set to.</param>
		public void SetLevel(int level)
		{
			Level = Mathf.Clamp(level, 0, Constants.MAX_EFFECT_LEVEL);

			if (HasShipEffects)
				SetEffectLevel(Level, ShipEffects);

			if (HasShipWeaponEffects)
				SetEffectLevel(Level, ShipWeaponEffects);

			if (HasShipProjectileEffects)
				SetEffectLevel(Level, ShipProjectileEffects);

			if (hasShipActions)
			{
				foreach (var action in ShipActions)
				{
					foreach (var shipaction in AttatchedModule.Ship.Actions)
					{
						if (shipaction.Data == action)
						{
							shipaction.SetLevel(level);
						}
					}
				}
			}
		}

		/// <summary>
		/// Increase the level of the effects of this entity by 1 level.
		/// </summary>
		public void IncreaseLevel()
		{
			SetLevel(Level + 1);
		}

		/// <summary>
		/// Decrease the level of the effects of this entity by 1 level.
		/// </summary>
		public void DecreaseLevel()
		{
			SetLevel(Level - 1);
		}

		private void SetEffectLevel(int level, EffectWrapper[] effects)
		{
			foreach (var wrapper in effects)
			{
				var prev = wrapper.Level;
				wrapper.Level = level;

				if (wrapper.EffectCollection.Targets == SystemTargets.Weapon)
					foreach (WeaponEntity weapon in AttatchedModule.Ship.WeaponEntities)
						wrapper.EffectCollection.OnLevelChanged(weapon, prev, level, wrapper);
				if (wrapper.EffectCollection.Targets == SystemTargets.Ship)
					wrapper.EffectCollection.OnLevelChanged(AttatchedModule.Ship, prev, level, wrapper);

				//wrapper.EffectCollection.OnLevelChanged(this, prev, level, wrapper); // og code
			}
		}
	}
}
