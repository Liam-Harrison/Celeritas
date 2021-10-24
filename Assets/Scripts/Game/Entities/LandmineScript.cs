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

		// Start is called before the first frame update
		void Start()
		{
			material = GetComponent<MeshRenderer>().material;
			material.color = Color.red;
		}

		// Update is called once per frame
		//void Update()
		//{
		//	material.color = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));
		//}

		public void initialize(float setDamage, float duration)
		{
			damage = setDamage;
			health = new EntityStatBar(1.0f);
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

		public override void OnEntityHit(Entity other)
		{
			if (other.PlayerShip == true)
			{
				return;
			}
			else
			{
				other.TakeDamage(other, damage);
				Destroy(gameObject);
			}
		}
	}
}