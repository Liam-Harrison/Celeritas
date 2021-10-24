using Celeritas.Game.Controllers;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// 
	/// </summary>

	public class LandmineScript : MonoBehaviour
	{

		private float damage = 150;

		private float duration = 1;

		private Material material;

		// Start is called before the first frame update
		void Start()
		{
			material = GetComponent<MeshRenderer>().material;
			
		}

		// Update is called once per frame
		void Update()
		{
			material.color = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));
		}

		public void initialize(float setDamage, float setduration)
		{
			damage = setDamage;
			duration = setduration;
			StartCoroutine(DestroyOverTime());
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
			{
				return;
			}
			else
			{
				other.TakeDamage(other, damage);
				Destroy(gameObject);
			}
		}

		IEnumerator DestroyOverTime()
		{
			yield return new WaitForSeconds(duration);
			Destroy(gameObject);
		}
	}
}