using Celeritas.Game.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Scriptables
{
	[CreateAssetMenu(fileName = "New Action", menuName = "Celeritas/New Action", order = 60)]
	public class ActionData : ScriptableObject
	{
		public GameAction action;
	}
}
