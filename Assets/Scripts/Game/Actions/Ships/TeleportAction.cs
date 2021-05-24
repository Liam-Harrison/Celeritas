using Celeritas.Game.Entities;
using Celeritas.Scriptables;

namespace Celeritas.Game.Actions
{
	public class TeleportAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public TeleportActionData TeleportData { get; private set; }

		protected override void Execute(Entity entity)
		{
			var ship = entity as ShipEntity;

			var target = ship.Target;
			if ((target - ship.Position).magnitude > TeleportData.distance)
			{
				target = (ship.Target - ship.Position).normalized * TeleportData.distance;
			}

			ship.Position = target;

			ship.TakeDamage(null, 100000);
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			TeleportData = data as TeleportActionData;

			base.Initialize(data, isPlayer, owner);
		}
	}
}
