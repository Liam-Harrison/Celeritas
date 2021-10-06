using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	public class NukeScript : MonoBehaviour
	{
		public float delay = 3f;

		private float countDown;

		private bool hasExploded = false;

		public CircleCollider2D collider;

		// Start is called before the first frame update
		void Start()
		{
			countDown = delay;
		}

		// Update is called once per frame
		void Update()
		{
			countDown -= Time.deltaTime;
			if (countDown <= 0 && hasExploded == false)
			{
				Explode();
				hasExploded = true;
			}
		}

		private void Explode()
		{
			if (collider)
			{
				StartCoroutine(DeletionTimer(1f));
			}
		}

		private IEnumerator DeletionTimer(float timer)
		{
			collider.enabled = true;
			yield return new WaitForSeconds(timer);
			Destroy(gameObject);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			var entity = other.gameObject.GetComponentInParent<Entity>();
			if (entity != null)
			{
				OnEntityHit(entity);
			}
		}

		public void OnEntityHit(Entity other)
		{
			if (other.PlayerShip == true)
				return;

			other.TakeDamage(other, 750);
		}
	}
}

