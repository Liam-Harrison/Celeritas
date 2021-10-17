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

		public bool delaySet = false;

		public float damage = 750;

		private bool hasExploded = false;

		[SerializeField]
		private CircleCollider2D collider;

		private Material material;

		// Start is called before the first frame update
		void Start()
		{
			material = GetComponent<MeshRenderer>().material;
		}

		// Update is called once per frame
		void Update()
		{
			if (delaySet == true)
			{
				delay -= Time.deltaTime;
			}

			material.color = Color.Lerp(Color.red, Color.white, delay);

			if (delay <= 0 && hasExploded == false)
			{
				CombatHUD.Instance.ColorShift(1, Color.white);
				Explode();
				hasExploded = true;
			}
		}

		private void Explode()
		{
			if (collider)
			{
				StartCoroutine(DeletionTimer(0.1f));
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

			other.TakeDamage(other, damage);
		}
	}
}

