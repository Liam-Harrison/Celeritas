using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Actions
{
	public abstract class GameAction
	{
		public abstract string Name { get; }

		public abstract float CooldownSeconds { get; }

		public abstract SystemTargets Targets { get; }

		public abstract void ExecuteAction(Entity entity);

	}
}
