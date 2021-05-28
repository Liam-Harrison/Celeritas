using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Celeritas.UI.Tooltips
{
	public class ShowTooltipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField, PropertyRange(0, 1)]
		private float waitTime = 0.3f;

		private float entered;
		private bool inside;
		private bool showing;
		private ITooltip reference;

		private void Awake()
		{
			reference = GetComponent<ITooltip>();
		}

		private void OnDisable()
		{
			if (showing)
				Tooltip.Instance.Hide();

			showing = false;
			inside = false;
		}

		private void Update()
		{
			if (inside && !Tooltip.Instance.IsShowing && Time.unscaledTime >= entered + waitTime)
			{
				Tooltip.Instance.Show(reference.TooltipEntity);
				showing = true;
			}
			else if (!inside && Tooltip.Instance.IsShowing && showing)
			{
				Tooltip.Instance.Hide();
				showing = false;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			inside = true;

			if (Tooltip.Instance.IsShowing)
			{
				Tooltip.Instance.Show(reference.TooltipEntity);
			}
			else
			{
				entered = Time.unscaledTime;
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			inside = false;
		}
	}
}