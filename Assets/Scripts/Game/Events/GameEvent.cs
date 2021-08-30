using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Events
{
	public class GameEvent
	{
		public EventData EventData { get; private set; }

		public virtual void Initalize(EventData data)
		{
			EventData = data;
		}
	}
}