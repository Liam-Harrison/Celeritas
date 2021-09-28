using Celeritas.Game.Actions;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// A teleport action.
	/// </summary>
	[CreateAssetMenu(fileName = "New Shield Replenish Action", menuName = "Celeritas/Actions/Shield Replenish")]
	public class ShieldReplenishActionData : ActionData
	{
		/// <inheritdoc/>
		public override Type ActionType => typeof(ShieldReplenishAction);
	}
}
