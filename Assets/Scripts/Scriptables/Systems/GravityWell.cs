using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables.Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Scriptables.Systems {

	/// <summary>
	/// Pulls in ship entities within a certain radius
	/// </summary>
	[CreateAssetMenu(fileName = "New Gravity Well Modifier", menuName = "Celeritas/Modifiers/Gravity Well")]
	public class GravityWell : ModifierSystem, IEntityUpdated
	{
		public override bool Stacks => false;

		public override SystemTargets Targets => SystemTargets.Ship;

		[SerializeField, Title("Flat Force Multiplier"), InfoBox("Adjust this to fine-tune force. Make it negative to push enemies away.")]
		private float flatForceMultiplier; // this is here to adjust the intensity of the force

		[SerializeField, Title("Extra Force Multiplier Per Level")]
		private float extraForceMultiplierPerLevel;

		[SerializeField, Title("Force Variation")]
		private AnimationCurve forceVariation = new AnimationCurve();

		[SerializeField, Title("Radius of Effect")]
		private float radiusOfEffect;

		[SerializeField, Title("Extra Radius Per Level")]
		private float extraRadiusPerLevel;

		public void OnEntityUpdated(Entity entity, ushort level)
		{
			var ownerShip = entity as ShipEntity; // the ship that the effect originates from

			// see how big the ships' collider is, and pick the biggest dimension
			Collider2D ownerCollider = ownerShip.gameObject.GetComponent<Collider2D>();
			float colliderSize = ownerCollider.bounds.size.y;
			if (ownerCollider.bounds.size.x > ownerCollider.bounds.size.y)
				colliderSize = ownerCollider.bounds.size.x;

			// factor owner ship size into radius of effect
			float radius = radiusOfEffect + colliderSize + level * extraRadiusPerLevel;

			// find all entities within the specified radius, around ownerShip
			List <Collider2D> withinRange = new List<Collider2D>();
			ContactFilter2D filter = new ContactFilter2D();
			filter.NoFilter();
			Physics2D.OverlapCircle(entity.transform.position, radius, filter, withinRange);

			foreach (Collider2D collider in withinRange)
			{
				Rigidbody2D rigidBody = collider.attachedRigidbody;
				try
				{
					var foreignShip = rigidBody.GetComponent<ShipEntity>(); // other ship

					if (foreignShip != null)
					{
						// apply force to pull ship towards ownerShip (or push if multiplier is negative)
						Vector2 betweenShips = (ownerShip.transform.position - foreignShip.transform.position);
						Vector2 directionToPull = betweenShips.normalized;

						float curveMultiplier = forceVariation.Evaluate(betweenShips.magnitude);

						float levelMultiplier = 1 + (level * extraForceMultiplierPerLevel);

						rigidBody.AddForce(directionToPull * curveMultiplier * levelMultiplier * flatForceMultiplier * Time.smoothDeltaTime, ForceMode2D.Force);
					}
				}
				catch (NullReferenceException) {
					// do nothing - just means the entity was not a ship
				}
			}
		}

	}
}
