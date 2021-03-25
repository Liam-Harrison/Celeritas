using Celeritas.Game;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Effect target types.
	/// </summary>
	public enum EffectTarget
	{
		Projectile,
		Module,
		Ship
	}

	/// <summary>
	/// Contains the instanced information for an effect.
	/// </summary>
	[CreateAssetMenu(fileName = "New Effect", menuName = "Celeritas/New Effect", order = 35)]
	public class EffectData : ScriptableObject
	{
		[SerializeField, Title("Common")] protected string title;
		[SerializeField] protected ModifierData[] modifiers;
		[SerializeField] protected EffectTarget target;

		/// <summary>
		/// the title of the module.
		/// </summary>
		public string Title { get => title; }

		/// <summary>
		/// The modifiers attatched to this effect.
		/// </summary>
		public ModifierData[] Modifiers { get => modifiers; }

		/// <summary>
		/// The target type of this effect.
		/// </summary>
		public EffectTarget Target { get => target; }

		/// <summary>
		/// Update the provided entity against this effect when updated.
		/// </summary>
		/// <param name="entity">The entity to update.</param>
		public void UpdateEntity(Entity entity)
		{
			if (!IsValidEntity(entity))
				return;

			foreach (var modifier in Modifiers)
			{
				modifier.OnEntityUpdated(entity);
			}
		}

		/// <summary>
		/// Update the provided entity against this effect when created.
		/// </summary>
		/// <param name="entity">The entity to update when created.</param>
		public void CreateEntity(Entity entity)
		{
			if (!IsValidEntity(entity))
				return;

			foreach (var modifier in Modifiers)
			{
				modifier.OnEntityCreated(entity);
			}
		}

		/// <summary>
		/// Update the provided entity against this effect when destroyed.
		/// </summary>
		/// <param name="entity">The entity to update when destroyed.</param>
		public void DestroyEntity(Entity entity)
		{
			if (!IsValidEntity(entity))
				return;

			foreach (var modifier in Modifiers)
			{
				modifier.OnEntityDestroyed(entity);
			}
		}

		/// <summary>
		/// Update the provided entity against this effect when hit.
		/// </summary>
		/// <param name="entity">The subject entity.</param>
		/// <param name="other">The other entity hit.</param>
		public void HitEntity(Entity entity, Entity other)
		{
			if (!IsValidEntity(entity))
				return;

			foreach (var modifier in Modifiers)
			{
				modifier.OnEntityHit(entity, other);
			}
		}

		private bool IsValidEntity(Entity entity)
		{
			if (entity is ProjectileEntity && target != EffectTarget.Projectile)
				return false;

			if (entity is ShipEntity && target != EffectTarget.Ship)
				return false;

			if (entity is ModuleEntity && target != EffectTarget.Module)
				return false;

			return true;
		}
	}
}
