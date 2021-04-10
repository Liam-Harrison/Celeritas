using Celeritas.Game.Entities;
using UnityEngine;

namespace Celeritas.AI
{
	/// <summary>
	/// Base implemenation for all AI classes.
	/// </summary>
	[RequireComponent(typeof(ShipEntity))]
	public abstract class AIBase : MonoBehaviour
	{
		/// <summary>
		/// The ship entity that this controller is attatched to.
		/// </summary>
		public ShipEntity ShipEntity { get; private set; }

		protected virtual void Awake()
		{
			ShipEntity = GetComponent<ShipEntity>();
		}

		protected virtual void FixedUpdate()
		{
			if (EntityDataManager.Instance.ini)
		}

		protected abstract void AIUpdate();
	}
}
