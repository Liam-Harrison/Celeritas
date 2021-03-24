using Celeritas.Scriptables;
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
	}
}
