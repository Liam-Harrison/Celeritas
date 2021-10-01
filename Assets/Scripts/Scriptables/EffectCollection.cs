using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for an effect.
	/// </summary>
	[CreateAssetMenu(fileName = "New Effect", menuName = "Celeritas/New Effect", order = 35)]
	public class EffectCollection : ScriptableObject
	{
		[SerializeField, Title("Common")]
		protected string title;

		[SerializeField]
		protected SystemTargets targets;

		[SerializeField]
		protected bool stacks;

		[SerializeField]
		protected List<ModifierSystem> systems;

		protected Action<Entity, EffectWrapper> onKilled;
		protected Action<Entity, EffectWrapper> onAdded;
		protected Action<Entity, EffectWrapper> onRemoved;
		protected Action<Entity, EffectWrapper> onUpdated;
		protected Action<WeaponEntity, ProjectileEntity, EffectWrapper> onFired;
		protected Action<Entity, Entity, EffectWrapper> onHit;
		protected Action<Entity, EffectWrapper> onEntityBeforeDie;
		protected Action<Entity, EffectWrapper> onEntityReset;
		protected Action<Entity, int, int, EffectWrapper> onEntityLevelChanged;

		protected virtual void OnEnable()
		{
			ResetAllListeners();
		}

		protected virtual void OnValidate()
		{
			foreach (var system in systems)
			{
				if (system == null)
				{
					Debug.LogError($"You have empty effects inside effect collection (<color=\"orange\">{Title}</color>).", this);
				}
				else if (!system.ContainsTarget(targets))
				{
					Debug.LogError($"You have an effect (<color=\"orange\">{Title}</color>) with a modifier system (<color=\"orange\">{system.Title}</color>) that does not " +
						$"support all your intended targets (<color=\"orange\">{targets}</color>), If the system supports this type add the missing targets. Otherwise remove the system.", this);
				}
			}
		}

		/// <summary>
		/// the title of the module.
		/// </summary>
		public string Title { get => title; }

		/// <summary>
		/// The intended targets for this effect.
		/// </summary>
		public SystemTargets Targets { get => targets; }

		/// <summary>
		/// The modifiers attatched to this effect.
		/// </summary>
		public IReadOnlyList<ModifierSystem> Systems { get => systems.AsReadOnly(); }

		/// <summary>
		/// Does this event collection stack with other collections.
		/// </summary>
		public bool Stacks { get => stacks; }

		/// <summary>
		/// Add a new modifier system to this effect collection.
		/// </summary>
		/// <param name="system">The system to add.</param>
		public void AddModifierSystem(ModifierSystem system)
		{
			if (!systems.Contains(system) || system.Stacks)
			{
				systems.Add(system);
				RegisterModifierSystemListeners(system);
			}
			else
			{
				Debug.LogError($"Modifier System (<color=\"orange\">{system.Title}</color>) does not stack and already exists inside Effect Collection (<color=\"orange\">{Title}</color>).", this);
			}
		}

		/// <summary>
		/// Remove an existing modifier system from this effect collection.
		/// </summary>
		/// <param name="system">The system to remove.</param>
		public void RemoveModifierSystem(ModifierSystem system)
		{
			if (systems.Contains(system))
			{
				RemoveModifierSystemListeners(system);
				systems.Remove(system);
			}
			else
			{
				Debug.LogError($"Modifier System (<color=\"orange\">{system.Title}</color>) does not exist within collection it tried to be removed from (<color=\"orange\">{Title}</color>).");
			}
		}

		/// <summary>
		/// Update the provided entity against this effect when updated.
		/// </summary>
		/// <param name="entity">The entity to update.</param>
		/// <param name="wrapper">The level of the effect.</param>
		public void UpdateEntity(Entity entity, EffectWrapper wrapper)
		{
			if (!IsValidEntity(entity))
				return;

			onUpdated?.Invoke(entity, wrapper);
		}

		/// <summary>
		/// Update the provided entity against this effect when destroyed.
		/// </summary>
		/// <param name="entity">The entity to update when destroyed.</param>
		/// <param name="wrapper">The level of the effect.</param>
		public void KillEntity(Entity entity, EffectWrapper wrapper)
		{
			if (!IsValidEntity(entity))
				return;

			onKilled?.Invoke(entity, wrapper);
		}

		/// <summary>
		/// Update the provided entity against this effect when scheduled to be destroyed.
		/// </summary>
		/// <param name="entity">The entity to update when destroyed.</param>
		/// <param name="wrapper">The level of the effect.</param>
		public void OnEntityBeforeDie(Entity entity, EffectWrapper wrapper)
		{
			if (!IsValidEntity(entity))
				return;

			onEntityBeforeDie?.Invoke(entity, wrapper);
		}

		/// <summary>
		/// Updated the provided entity against this effect when the effect is added.
		/// </summary>
		/// <param name="entity">The entity who had this effect added to.</param>
		/// <param name="level">The level of the effect.</param>
		public void OnAdded(Entity entity, EffectWrapper wrapper)
		{
			if (!IsValidEntity(entity))
				return;

			onAdded?.Invoke(entity, wrapper);
		}

		/// <summary>
		/// Updated the provided entity against this effect when the effect is removed.
		/// </summary>
		/// <param name="entity">The entity who had this effect added to.</param>
		/// <param name="wrapper">The level of the effect.</param>
		public void OnRemoved(Entity entity, EffectWrapper wrapper)
		{
			if (!IsValidEntity(entity))
				return;

			onRemoved?.Invoke(entity, wrapper);
		}

		/// <summary>
		/// Update the provided entity against this effect when hit.
		/// </summary>
		/// <param name="entity">The subject entity.</param>
		/// <param name="other">The other entity hit.</param>
		/// <param name="wrapper">The level of the effect.</param>
		public void HitEntity(Entity entity, Entity other, EffectWrapper wrapper)
		{
			if (!IsValidEntity(entity))
				return;

			onHit?.Invoke(entity, other, wrapper);
		}

		/// <summary>
		/// Update the provided entity against this effect when fired.
		/// </summary>
		/// <param name="entity">The subject entity.</param>
		/// <param name="wrapper">The wrapper of the effect.</param>
		public void OnFired(WeaponEntity entity, ProjectileEntity projectile, EffectWrapper wrapper)
		{
			if (!IsValidEntity(entity))
				return;

			onFired?.Invoke(entity, projectile, wrapper);
		}

		/// <summary>
		/// Update the provided against this effect when reset.
		/// </summary>
		/// <param name="entity">The subject entity.</param>
		/// <param name="wrapper"></param>
		public void OnReset(Entity entity, EffectWrapper wrapper)
		{
			if (!IsValidEntity(entity))
				return;

			onEntityReset?.Invoke(entity, wrapper);
		}

		public void OnLevelChanged(Entity entity, int previous, int newLevel, EffectWrapper wrapper)
		{
			if (!IsValidEntity(entity))
				return;

			onEntityLevelChanged?.Invoke(entity, previous, newLevel, wrapper);
		}

		private bool IsValidEntity(Entity entity)
		{
			if (entity is ProjectileEntity && !targets.HasFlag(SystemTargets.Projectile))
				return false;

			if (entity is ShipEntity && !targets.HasFlag(SystemTargets.Ship))
				return false;

			if (entity is WeaponEntity && !targets.HasFlag(SystemTargets.Weapon))
				return false;

			if (entity is ModuleEntity && !(entity is WeaponEntity) && !targets.HasFlag(SystemTargets.Module))
				return false;

			if (entity is Asteroid && !targets.HasFlag(SystemTargets.Asteroid))
				return false;

			return true;
		}

		private void ResetAllListeners()
		{
			onKilled = null;
			onUpdated = null;
			onHit = null;
			onAdded = null;
			onRemoved = null;
			onFired = null;
			onEntityBeforeDie = null;
			onEntityReset = null;
			onEntityLevelChanged = null;

			foreach (var modifier in systems)
			{
				RegisterModifierSystemListeners(modifier);
			}
		}

		private void RegisterModifierSystemListeners(ModifierSystem system)
		{
			if (system is IEntityKilled destroyed)
				onKilled += destroyed.OnEntityKilled;

			if (system is IEntityUpdated updated)
				onUpdated += updated.OnEntityUpdated;

			if (system is IEntityHit hit)
				onHit += hit.OnEntityHit;

			if (system is IEntityEffectAdded added)
				onAdded += added.OnEntityEffectAdded;

			if (system is IEntityEffectRemoved removed)
				onRemoved += removed.OnEntityEffectRemoved;

			if (system is IEntityFired fired)
				onFired += fired.OnEntityFired;

			if (system is IEntityBeforeDie scheduled)
				onEntityBeforeDie += scheduled.OnEntityBeforeDie;

			if (system is IEntityResetable resetable)
				onEntityReset += resetable.OnReset;

			if (system is IEntityLevelChanged changed)
				onEntityLevelChanged += changed.OnLevelChanged;
		}

		private void RemoveModifierSystemListeners(ModifierSystem system)
		{
			if (system is IEntityKilled destroyed)
				onKilled -= destroyed.OnEntityKilled;

			if (system is IEntityUpdated updated)
				onUpdated -= updated.OnEntityUpdated;

			if (system is IEntityHit hit)
				onHit -= hit.OnEntityHit;

			if (system is IEntityEffectAdded added)
				onAdded -= added.OnEntityEffectAdded;

			if (system is IEntityEffectRemoved removed)
				onRemoved -= removed.OnEntityEffectRemoved;

			if (system is IEntityFired fired)
				onFired -= fired.OnEntityFired;

			if (system is IEntityBeforeDie scheduled)
				onEntityBeforeDie -= scheduled.OnEntityBeforeDie;

			if (system is IEntityResetable resetable)
				onEntityReset -= resetable.OnReset;

			if (system is IEntityLevelChanged changed)
				onEntityLevelChanged -= changed.OnLevelChanged;
		}
	}
}
