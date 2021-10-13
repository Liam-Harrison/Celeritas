using Assets.Scripts.Scriptables.Data;
using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// Loot that has been dropped onto the map,
	/// awaiting the player to pick it up!
	/// (player will pick up the loot automatically when it is within a certain radius of them)
	/// </summary>
	public class LootEntity : Entity
	{
		public override SystemTargets TargetType { get => SystemTargets.Loot; }

		public const float SUCK_DIST = 50;

		public const float MAX_VEL = 15;

		public const float VEL_PER_SEC = 40;

		public LootData lootData;
		private uint pickupRadius;

		private int amount;
		private LootType lootType;

		private Vector3 vel;

		/// <summary>
		/// How much loot is stored in this object. eg, 3 x rare metals
		/// </summary>
		public int Amount { get => amount; set => amount = value; }

		public override void Initalize(EntityData data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false, bool instanced = false)
		{
			lootData = data as LootData;
			pickupRadius = lootData.PickupRadius;
			lootType = lootData.LootType;
			
			base.Initalize(data, owner, effects, forceIsPlayer, instanced);
		}

		protected override void Update()
		{
			if (PlayerController.Instance.PlayerShipEntity != null)
			{
				var d = PlayerController.Instance.PlayerShipEntity.Position - Position;
				var m = d.magnitude;
				var n = d.normalized;

				if (m <= SUCK_DIST)
				{
					var p = Mathf.Sin((1 - Mathf.Clamp01(m / SUCK_DIST)) * (Mathf.PI / 2));
					var change = n * VEL_PER_SEC * p;
					vel = Vector3.ClampMagnitude(vel + ((change - vel) * Time.smoothDeltaTime), MAX_VEL);
				}
				else
				{
					vel = Vector3.Lerp(vel, Vector3.zero, 200f * Time.smoothDeltaTime);
				}

				transform.position += vel * Time.smoothDeltaTime;

				if (m <= pickupRadius)
				{
					PickedUpByPlayer();
				}

				base.Update();
			}
		}

		/// <summary>
		/// Give the player the loot associated with this entity
		/// Will destroy this loot entity
		/// </summary>
		public void PickedUpByPlayer()
		{
			LootController.Instance.GivePlayerLoot(lootType, amount);
			KillEntity();
		}
	}
}
