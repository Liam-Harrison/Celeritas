using Assets.Scripts.Scriptables.Data;
using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Celeritas.Game.Entities.LootController;

namespace Celeritas.Game.Entities
{
	public class LootEntity : Entity
	{
		public override SystemTargets TargetType { get => SystemTargets.Loot; }

		public LootData lootData;
		private uint pickupRadius;
		private PlayerShipEntity player;

		private int amount;
		private LootType lootType;

		/// <summary>
		/// How much loot is stored in this object. eg, 3 x rare metals
		/// </summary>
		public int Amount { get => amount; set => amount = value; }

		public override void Initalize(ScriptableObject data, Entity owner = null, IList<EffectWrapper> effects = null, bool forceIsPlayer = false)
		{
			lootData = data as LootData;
			pickupRadius = lootData.PickupRadius;
			lootType = lootData.LootType;
			
			base.Initalize(data, owner, effects, forceIsPlayer);
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

		public void PickedUpByPlayer()
		{
			LootController.Instance.GivePlayerLoot(lootType, amount);

			Died = true;
		}
	}
}
