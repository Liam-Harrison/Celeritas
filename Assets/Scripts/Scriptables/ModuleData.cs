using UnityEngine;
using Celeritas.Extensions;
using Celeritas.Game.Entities;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a module.
	/// </summary>
	[CreateAssetMenu(fileName = "New Module", menuName = "Celeritas/New Module", order = 20)]
	public class ModuleData : EntityData
	{
		[SerializeField] protected ModuleSize size;

		protected virtual void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<ModuleEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(ModuleEntity)} attatched to it.", this);
			}
		}
	}
}
