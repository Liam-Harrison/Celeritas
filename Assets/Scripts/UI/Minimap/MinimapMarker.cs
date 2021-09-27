using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Game.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI
{
	class MinimapMarker : MonoBehaviour, IPooledObject<MinimapMarker>
	{
		[SerializeField, Title("Minimap Marker Settings")]
		private Image sprite;

		[SerializeField]
		private Color playerColor = Color.white;

		[SerializeField]
		private Color enemyColor = Color.white;

		[SerializeField]
		private Color lootColor = Color.white;

		public Entity Entity { get; private set; }

		public RectTransform RectTransform { get; private set; }

		public ObjectPool<MinimapMarker> OwningPool { get; set; }

		public void Initalize(Entity entity)
		{
			Entity = entity;

			if (entity is LootEntity)
				sprite.color = lootColor;
			else if (entity.IsPlayer)
				sprite.color = playerColor;
			else
				sprite.color = enemyColor;
		}

		public void SetAlpha(float a)
		{
			var color = sprite.color;
			color.a = a;

			sprite.color = color;
		}

		public void OnSpawned()
		{
			RectTransform = GetComponent<RectTransform>();
		}

		public void OnDespawned() { }
	}
}
