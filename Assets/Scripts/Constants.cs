using System.IO;
using UnityEngine;

namespace Celeritas
{
	/// <summary>
	/// Contains a collection of common constants that can be used throughout the game.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Path to the game data folder.
		/// </summary>
		public static readonly string DATA_PATH = Application.dataPath;

		/// <summary>
		/// Path to the persistent data folder.
		/// </summary>
		public static readonly string PERSISTENT_PATH = Application.persistentDataPath;

		/// <summary>
		/// Path to the temporary cache folder.
		/// </summary>
		public static readonly string CACHE_PATH = Application.temporaryCachePath;

		/// <summary>
		/// Path to the streaming assets folder.
		/// </summary>
		public static readonly string STREAMING_ASSETS = Application.streamingAssetsPath;

		/// <summary>
		/// The label used for the ship data type in the addresable system.
		/// </summary>
		public const string SHIP_TAG = "Ships";

		/// <summary>
		/// The label used for the weapon data type in the addresable system.
		/// </summary>
		public const string WEAPON_TAG = "Weapons";

		/// <summary>
		/// The label used for the module data type in the addresable system.
		/// </summary>
		public const string MODULE_TAG = "Modules";

		/// <summary>
		/// The label used for the projectile data type in the addresable system.
		/// </summary>
		public const string PROJECTILE_TAG = "Projectiles";

		/// <summary>
		/// The label used for the modifier data type in the addresable system.
		/// </summary>
		public const string SYSTEMS_TAG = "Systems";

		/// <summary>
		/// The label used for the modifier effect type in the addresable system.
		/// </summary>
		public const string EFFECTS_TAG = "Effects";
	}
}
