using Celeritas.Scriptables;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// The game entity for a weapon.
	/// </summary>
	public class WeaponEntity : ModuleEntity
	{
		/// <summary>
		/// The attatched weapon data.
		/// </summary>
		public WeaponData WeaponData { get; private set; }

		public override void Initalize(ScriptableObject data)
		{
			WeaponData = data as WeaponData;
			base.Initalize(data);
		}
	}
}
