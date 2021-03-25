using Celeritas.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// Avaliable sizes for modules.
	/// </summary>
	public enum ModuleSize
	{
		Small,
		Medium,
		Large
	}

	/// <summary>
	/// The game entity for a module.
	/// </summary>
	public class ModuleEntity : Entity
	{
		/// <summary>
		/// The attatched module data.
		/// </summary>
		public ModuleData ModuleData { get; private set; }

		/// <summary>
		/// The module that this entity is attatched to.
		/// </summary>
		public Module AttatchedModule { get; private set; }

		/// <summary>
		/// Current effects on this module
		/// </summary>
		public List<EffectData> Effects { get; set; } = new List<EffectData>();

		public override void Initalize(ScriptableObject data)
		{
			ModuleData = data as ModuleData;
			base.Initalize(data);
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
		}

		/// <summary>
		/// Update effects for this entity when created.
		/// </summary>
		/// <param name="entity">The entity to update against effects.</param>
		public void OnEntityCreated(Entity entity)
		{
			foreach (var effect in Effects)
			{
				effect.CreateEntity(entity);
			}
		}

		/// <summary>
		/// Update effects for this entity when destroyed.
		/// </summary>
		/// <param name="entity">The entity to update against effects.</param>
		public void OnEntityDestroyed(Entity entity)
		{
			foreach (var effect in Effects)
			{
				effect.DestroyEntity(entity);
			}
		}

		/// <summary>
		/// Update effects for this entity when hit.
		/// </summary>
		/// <param name="entity">The target entity.</param>
		/// <param name="other">The other entity.</param>
		public void OnEntityHit(Entity entity, Entity other)
		{
			foreach (var effect in Effects)
			{
				effect.HitEntity(entity, other);
			}
		}

		/// <summary>
		/// Update effects for this entity when updated.
		/// </summary>
		/// <param name="entity">The entity to update against effects.</param>
		public void OnEntityUpdated(Entity entity)
		{
			foreach (var effect in Effects)
			{
				effect.UpdateEntity(entity);
			}
		}
	}
}
