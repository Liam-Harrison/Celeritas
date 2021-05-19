using Celeritas.Scriptables;
using UnityEngine;
using static Celeritas.Game.Entities.LootController;

namespace Assets.Scripts.Scriptables.Data
{
	/// <summary>
	/// Contains the instance information for dropped loot
	/// ie, loot that has been dropped by an enemy onto the 'ground'
	/// will be picked up by the player if they fly close enough to it
	/// </summary>
	[CreateAssetMenu(fileName = "New Loot", menuName = "Celeritas/New LootDrop", order = 50)]
	public class LootData : EntityData
	{
		[SerializeField]
		private uint pickupRadius;

		public uint PickupRadius { get => pickupRadius; }

		[SerializeField]
		private LootType lootType;

		public LootType LootType { get => lootType; }

		public override string Tooltip => "Looks like someone dropped something here...";
	}
}
