using UnityEngine.EventSystems;

namespace Celeritas.UI.Tooltips
{
	public class ShowTooltipUI : ShowTooltip, IPointerEnterHandler, IPointerExitHandler
	{
		protected ITooltip reference;

		protected virtual void Awake()
		{
			reference = GetComponent<ITooltip>();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Show(reference.TooltipEntity);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			HideTooltip();
		}
	}
}