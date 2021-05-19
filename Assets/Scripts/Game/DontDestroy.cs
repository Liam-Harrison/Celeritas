using UnityEngine;

namespace Celeritas.Game
{
	/// <summary>
	/// Marks a gameobject to not be unloaded during scene changes.
	/// </summary>
	public class DontDestroy : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}