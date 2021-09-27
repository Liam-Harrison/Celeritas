using Celeritas.Game;
using Celeritas.Game.Interfaces;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace Celeritas.UI
{
	public class FloatingText : MonoBehaviour, IPooledObject<FloatingText>
	{
		private const float SHOW_TIME = 1f;

		private const float ANIMATE_OUT_TIME = 0.2f;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI text;

		[SerializeField, TitleGroup("Assignments")]
		private new Animation animation;

		public Entity Entity {  get; private set; }

		public float ShownDamage { get; private set; }

		public ObjectPool<FloatingText> OwningPool { get; set; }

		private float lastSetText;

		private Vector3 pos;

		private bool crossfading = false;

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
			if (Time.unscaledTime > lastSetText + SHOW_TIME)
			{
				OwningPool.ReleasePooledObject(this);
			}
			else
			{
				if (Time.unscaledTime > lastSetText + SHOW_TIME - ANIMATE_OUT_TIME && crossfading == false)
				{
					crossfading = true;
					text.CrossFadeAlpha(0f, ANIMATE_OUT_TIME, true);
				}

				SetPosition(pos);
			}
		}

		private void SetPosition(Vector3 pos)
		{
			transform.position = Camera.main.WorldToScreenPoint(pos);
		}

		public void SetText(float damage)
		{
			if (Vector3.Distance(pos, Camera.main.WorldToScreenPoint(Entity.transform.position)) > 20)
			{
				pos = Entity.transform.position;
			}

			ShownDamage = damage;
			text.text = damage.ToString("0");
			lastSetText = Time.unscaledTime;

			crossfading = false;
			text.CrossFadeAlpha(1f, 0f, true);
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
