using UnityEngine;
using Celeritas.Extensions;
using Celeritas.Game.Entities;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a projectile.
	/// </summary>
	[CreateAssetMenu(fileName = "New Projectile", menuName = "Celeritas/New Projectile", order = 40)]
	public class ProjectileData : EntityData
	{
		protected virtual void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<ProjectileEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(ProjectileEntity)} attatched to it.", this);
			}
		}
	}
}
