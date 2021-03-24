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

		public override void Initalize(ScriptableObject data)
		{
			ModuleData = data as ModuleData;
			base.Initalize(data);
		}
	}
}
