using Celeritas.Game;
using Celeritas.Game.Interfaces;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace Celeritas.UI
{
	public class FloatingText : MonoBehaviour, IPooledObject<FloatingText>
	{
		private const float MOVE_SPEED = 1f;

		private const float MAX_Y = 5;

		private const float ShowTime = 1f;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI text;

		[SerializeField, TitleGroup("Assignments")]
		private new Animation animation;

		public Entity Entity {  get; private set; }

		public float ShownDamage { get; private set; }

		public ObjectPool<FloatingText> OwningPool { get; set; }

		private float lastSetText;

		private float yoffset;

		public void Initalize(Entity entity, float damage)
		{
			Entity = entity;
			Entity.AttatchedFloatingText = this;
			transform.position = Camera.main.WorldToScreenPoint(entity.transform.position);
			animation.Play();
			SetText(damage);
		}

		private void Update()
		{
			if (Time.unscaledTime > lastSetText + ShowTime)
			{
				OwningPool.ReleasePooledObject(this);
			}
			else if (yoffset < MAX_Y)
			{
				var old = yoffset;
				yoffset = Mathf.Clamp(yoffset + (MOVE_SPEED * Time.unscaledDeltaTime), 0, MAX_Y);
				transform.position += new Vector3(0, yoffset - old, 0);
			}
		}

		public void SetText(float damage)
		{
			ShownDamage = damage;
			text.text = damage.ToString("0");
			lastSetText = Time.unscaledTime;
		}

		public void IncreaseNumber(float damage)
		{
			ShownDamage += damage;
			SetText(ShownDamage);
		}

		public void OnDespawned()
		{
			if (Entity.AttatchedFloatingText == this)
				Entity.AttatchedFloatingText = null;
		}

		public void OnSpawned()
		{
			yoffset = 0;
		}
	}
}