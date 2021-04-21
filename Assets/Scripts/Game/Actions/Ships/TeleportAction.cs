using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using UnityEngine;

namespace Celeritas.Game.Actions
{
	public class TeleportAction : GameAction
	{
		public override SystemTargets Targets => SystemTargets.Ship;

		public TeleportActionData TeleportData { get; private set; }

		protected override void Execute(Entity entity)
		{
			var ship = entity as ShipEntity;

			var delta = (ship.Target - ship.Position).normalized * TeleportData.distance;
			ship.Position += delta;

			if (ship.IsPlayer)
			{
				var camera = CameraStateManager.Instance.Camera;
				camera.OnTargetObjectWarped(PlayerSpawner.Instance.transform, delta);
			}
		}

		public override void Initialize(ActionData data, bool isPlayer, Entity owner = null)
		{
			TeleportData = data as TeleportActionData;

			base.Initialize(data, isPlayer, owner);
		}
	}
}
