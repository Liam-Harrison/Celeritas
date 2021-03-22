using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Celeritas.UI
{
	/// <summary>
	/// Routes Unity scroll events to other components.
	/// </summary>
	public class DragRouter : MonoBehaviour, IScrollHandler
	{
		[SerializeField] private ScrollRect target;

		public void OnScroll(PointerEventData eventData)
		{
			target.OnScroll(eventData);
		}
	}
}
