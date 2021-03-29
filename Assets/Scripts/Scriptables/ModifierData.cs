using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a modifier.
	/// </summary>
	[CreateAssetMenu(fileName = "New Modifier", menuName = "Celeritas/New Modifier", order = 50)]
	public class ModifierData : ScriptableObject
	{
		[SerializeField] protected string title;

		/// <summary>
		/// The title of the modifier.
		/// </summary>
		public string Title { get => title; }
	}
}
