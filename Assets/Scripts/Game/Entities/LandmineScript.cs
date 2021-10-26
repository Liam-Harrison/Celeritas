using Assets.Scripts.Scriptables.Data;
using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using Celeritas.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// 
	/// </summary>

	public class LandmineScript : Entity
	{
		public override SystemTargets TargetType { get => SystemTargets.Projectile; }

		private float damage = 150;

		private Material material;

		private EntityStatBar health;

		public EntityStatBar Health { get => health; }

		public Rigidbody2D Rigidbody { get; private set; }

		private Vector3 moveToTarget;

		private float distanceApart;

		[SerializeField]
		private float healthStat;

		// Start is called before the first frame update
		void Start()
		{
			material = GetComponent<MeshRenderer>().material;
			material.color = Color.red;

			moveToTarget = new Vector3(this.transform.position.x + Random.Range(distanceApart * -1.5f, distanceApart * 1.5f), this.transform.position.y + Random.Range(distanceApart * -1.5f, distanceApart * 1.5f), this.transform.position.z);
		}

		protected override void Update()
		{

			material.color = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));
			this.transform.position = Vector3.MoveTowards(this.transform.position, moveToTarget, Time.deltaTime * 5);

			base.Update();
		}

		public void initialize(float setDamage, float duration, float amount)
		{
			damage = setDamage;
			health = new EntityStatBar(healthStat);
			distanceApart = amount;
			Object.Destroy(gameObject, duration);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			var entity = other.gameObject.GetComponentInParent<Entity>();
			if (entity != null)
			{
				if (entity.PlayerShip == true)
				{
					return;
				}
				if (entity is ProjectileEntity)
				{
					var projectile = entity as ProjectileEntity;
					CombatHUD.Instance.PrintFloatingText(this, projectile.Damage);
				}
				OnEntityHit(entity);
			}
		}

		public override void TakeDamage(Entity attackingEntity, float damage)
		{
			if (attackingEntity is ProjectileEntity || attackingEntity is ShipEntity || attackingEntity is Asteroid || attackingEntity == this)
			{
				base.TakeDamage(attackingEntity);

				health.Damage(damage);

				if (health.IsEmpty())
				{
					Destroy(gameObject);
				}
			}
		}

		public override void OnEntityHit(Entity other)
		{
			if (other.IsPlayer == IsPlayer)
			{
				if (other is Asteroid asteroid)
				{
					other.TakeDamage(other, damage);
					Destroy(gameObject);
				}
				return;
			}

			if (other is ShipEntity ship)
			{
				if (ship.Stunned == false)
				{
					ship.Stun(1.0f);
				}
			}

			other.TakeDamage(other, damage);
		}
	}
}