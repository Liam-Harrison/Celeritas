using Celeritas.Game.Entities;
using Celeritas.Scriptables;

namespace Celeritas.Game.Actions
{
	public class NukeAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public NukeActionData NukeData { get; private set; }

		protected override void Execute(Entity entity, int level)
		{
			var ship = entity as ShipEntity;

		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			NukeData = data as NukeActionData;

			base.Initialize(data, isPlayer, owner);
		}
	}
}
