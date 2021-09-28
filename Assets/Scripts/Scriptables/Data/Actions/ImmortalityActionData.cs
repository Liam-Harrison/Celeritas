using Celeritas.Game.Actions;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Temporarily take no damage from all sources
	/// </summary>
	[CreateAssetMenu(fileName = "New Immortality Action", menuName = "Celeritas/Actions/Immortality")]
	public class ImmortalityActionData : ActionData
	{
		/// <summary>
		/// How long this will last
		/// </summary>
		[SerializeField]
		public float duration;

		/// <inheritdoc/>
		public override Type ActionType => typeof(ImmortalityAction);
	}
}
