using Celeritas.Game;
using Celeritas.Game.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI
{
	class MinimapMarker : MonoBehaviour, IPooledObject
	{
		[SerializeField, Title("Minimap Marker Settings")]
		private Image sprite;

		[SerializeField]
		private Color playerColor = Color.white;

		[SerializeField]
		private Color enemyColor = Color.white;

		public Entity Entity { get; private set; }

		public RectTransform RectTransform { get; private set; }

		public void Initalize(Entity entity)
		{
			Entity = entity;

			if (entity.IsPlayer)
				sprite.color = playerColor;
			else
				sprite.color = enemyColor;
		}

		public void OnSpawned()
		{
			RectTransform = GetComponent<RectTransform>();
		}

		public void OnDespawned() { }
	}
}
