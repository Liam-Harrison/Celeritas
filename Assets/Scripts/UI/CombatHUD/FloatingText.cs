using Celeritas.Game;
using Celeritas.Game.Interfaces;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace Celeritas.UI
{
	public class FloatingText : MonoBehaviour, IPooledObject<FloatingText>
	{
		private const float ShowTime = 1f;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI text;

		[SerializeField, TitleGroup("Assignments")]
		private new Animation animation;

		public Entity Entity {  get; private set; }

		public float ShownDamage { get; private set; }

		public ObjectPool<FloatingText> OwningPool { get; set; }

		private float lastSetText;

		private Vector3 pos;

		public void Initalize(Entity entity, float damage)
		{
			Entity = entity;
			Entity.AttatchedFloatingText = this;
			SetPosition(entity.transform.position);
			pos = entity.transform.position;
			animation.Play();
			SetText(damage);
		}

		private void Update()
		{
			if (Time.unscaledTime > lastSetText + ShowTime)
			{
				OwningPool.ReleasePooledObject(this);
			}
			else
			{
				SetPosition(pos);
			}
		}

		private void SetPosition(Vector3 pos)
		{
			transform.position = Camera.main.WorldToScreenPoint(pos);
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
			transform.SetAsLastSibling();
		}
	}
}
