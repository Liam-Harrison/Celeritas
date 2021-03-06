using Celeritas.Game.Entities;
using Celeritas.Scriptables;

namespace Celeritas.Game.Actions
{
	public class TeleportAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public TeleportActionData TeleportData { get; private set; }

		protected override void Execute(Entity entity, int level)
		{
			var ship = entity as ShipEntity;

			var dist = TeleportData.distance + (TeleportData.distancePerLevel * level);

			var target = ship.AimTarget;
			if ((target - ship.Position).magnitude > TeleportData.distance)
			{
				target = (ship.AimTarget - ship.Position).normalized * dist;
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
