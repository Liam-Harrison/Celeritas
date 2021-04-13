using Celeritas.Game;
using Celeritas.Game.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI
{
	class MinimapMarker : MonoBehaviour, IPooledObject
	{
		[SerializeField]
		private Image sprite;

		[SerializeField]
		private Color playerColor;

		[SerializeField]
		private Color enemyColor;

		public Entity Entity { get; private set; }

		public MarkerType MarkerType { get; private set; }

		public RectTransform RectTransform { get; private set; }

		public void Initalize(Entity entity, MarkerType marker)
		{
			Entity = entity;

			if (marker == MarkerType.Player)
				sprite.color = playerColor;
			else if (marker == MarkerType.Enemy)
				sprite.color = enemyColor;
		}

		public void OnSpawned()
		{
			RectTransform = GetComponent<RectTransform>();
		}

		public void OnDespawned() { }
	}
}
