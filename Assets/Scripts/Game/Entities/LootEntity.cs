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

		public LootData lootData;
		private uint pickupRadius; // player will pickup loot when within this radius of it
		private PlayerShipEntity player;

		private int amount; // quantity of loot player will get when picking this up
		private LootType lootType; // type of loot player will get when picking this up

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

		private void Start()
		{
			player = PlayerController.Instance.PlayerShipEntity;
		}

		protected override void Update()
		{
			// if player is within pickup radius, give player loot
			if (Vector3.Distance(transform.position, player.transform.position) <= pickupRadius)
			{
				PickedUpByPlayer();
			}

			base.Update();
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
