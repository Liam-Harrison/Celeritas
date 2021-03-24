using Celeritas.Scriptables;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a projectile.
	/// </summary>
	public class ProjectileEntity : Entity
	{
		/// <summary>
		/// The attatched projectile data.
		/// </summary>
		public ProjectileData ProjectileData { get; private set; }

		public override void Initalize(ScriptableObject data)
		{
			ProjectileData = data as ProjectileData;
			base.Initalize(data);
		}
	}
}
