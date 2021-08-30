using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Events
{
	public abstract class EventData : ScriptableObject
	{
		[SerializeField]
		private bool showOnMap;

		[SerializeField]
		private bool showArrow;
	}
}