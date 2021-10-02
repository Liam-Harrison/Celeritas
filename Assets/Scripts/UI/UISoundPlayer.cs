using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeritas.UI
{
	public class UISoundPlayer : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
	{
		[SerializeField, TitleGroup("Settings")]
		private UISound hovered = UISound.Hover;

		[SerializeField, TitleGroup("Settings")]
		private UISound pressed = UISound.Positive;

		public void OnPointerEnter(PointerEventData eventData)
		{
			SFX.Instance.PlayUISFX(hovered);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			SFX.Instance.PlayUISFX(pressed);
		}
	}
}