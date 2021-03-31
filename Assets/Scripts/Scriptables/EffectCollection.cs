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

		protected Action<Entity, ushort> onCreated;
		protected Action<Entity, ushort> onDestroyed;
		protected Action<Entity, ushort> onAdded;
		protected Action<Entity, ushort> onRemoved;
		protected Action<Entity, ushort> onUpdated;
		protected Action<Entity, Entity, ushort> onHit;

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
		/// <param name="level">The level of the effect.</param>
		public void UpdateEntity(Entity entity, ushort level)
		{
			if (!IsValidEntity(entity))
				return;

			onUpdated?.Invoke(entity, level);
		}

		/// <summary>
		/// Update the provided entity against this effect when created.
		/// </summary>
		/// <param name="entity">The entity to update when created.</param>
		/// <param name="level">The level of the effect.</param>
		public void CreateEntity(Entity entity, ushort level)
		{
			if (!IsValidEntity(entity))
				return;

			onCreated?.Invoke(entity, level);
		}

		/// <summary>
		/// Update the provided entity against this effect when destroyed.
		/// </summary>
		/// <param name="entity">The entity to update when destroyed.</param>
		/// <param name="level">The level of the effect.</param>
		public void DestroyEntity(Entity entity, ushort level)
		{
			if (!IsValidEntity(entity))
				return;

			onDestroyed?.Invoke(entity, level);
		}

		/// <summary>
		/// Updated the provided entity against this effect when the effect is added.
		/// </summary>
		/// <param name="entity">The entity who had this effect added to.</param>
		/// <param name="level">The level of the effect.</param>
		public void OnAdded(Entity entity, ushort level)
		{
			if (!IsValidEntity(entity))
				return;

			onAdded?.Invoke(entity, level);
		}

		/// <summary>
		/// Updated the provided entity against this effect when the effect is removed.
		/// </summary>
		/// <param name="entity">The entity who had this effect added to.</param>
		/// <param name="level">The level of the effect.</param>
		public void OnRemoved(Entity entity, ushort level)
		{
			if (!IsValidEntity(entity))
				return;

			onRemoved?.Invoke(entity, level);
		}

		/// <summary>
		/// Update the provided entity against this effect when hit.
		/// </summary>
		/// <param name="entity">The subject entity.</param>
		/// <param name="other">The other entity hit.</param>
		public void HitEntity(Entity entity, Entity other, ushort level)
		{
			if (!IsValidEntity(entity))
				return;

			onHit?.Invoke(entity, other, level);
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

			return true;
		}

		private void ResetAllListeners()
		{
			onCreated = null;
			onDestroyed = null;
			onUpdated = null;
			onHit = null;
			onAdded = null;
			onRemoved = null;

			foreach (var modifier in systems)
			{
				RegisterModifierSystemListeners(modifier);
			}
		}

		private void RegisterModifierSystemListeners(ModifierSystem system)
		{
			if (system is IEntityCreated created)
				onCreated += created.OnEntityCreated;

			if (system is IEntityDestroyed destroyed)
				onDestroyed += destroyed.OnEntityDestroyed;

			if (system is IEntityUpdated updated)
				onUpdated += updated.OnEntityUpdated;

			if (system is IEntityHit hit)
				onHit += hit.OnEntityHit;

			if (system is IEntityEffectAdded added)
				onAdded += added.OnEntityEffectAdded;

			if (system is IEntityEffectRemoved removed)
				onRemoved += removed.OnEntityEffectRemoved;
		}

		private void RemoveModifierSystemListeners(ModifierSystem system)
		{
			if (system is IEntityCreated created)
				onCreated -= created.OnEntityCreated;

			if (system is IEntityDestroyed destroyed)
				onDestroyed -= destroyed.OnEntityDestroyed;

			if (system is IEntityUpdated updated)
				onUpdated -= updated.OnEntityUpdated;

			if (system is IEntityHit hit)
				onHit -= hit.OnEntityHit;

			if (system is IEntityEffectAdded added)
				onAdded -= added.OnEntityEffectAdded;

			if (system is IEntityEffectRemoved removed)
				onRemoved -= removed.OnEntityEffectRemoved;
		}
	}
}
