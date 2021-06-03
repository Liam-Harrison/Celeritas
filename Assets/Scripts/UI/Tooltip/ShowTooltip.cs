using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Celeritas.UI.Tooltips
{
	/// <summary>
	/// Provides basic logic for tooltips which other classes can extend or use.
	/// </summary>
	public class ShowTooltip : MonoBehaviour
	{
		[SerializeField, PropertyRange(0, 1)]
		protected float waitTime = 0.3f;

		private float entered;
		private bool shouldShow;
		private ModuleEntity toShow;

		/// <summary>
		/// Is this tooltip being shown.
		/// </summary>
		public bool Showing { get; private set; }

		protected virtual void OnDisable()
		{
			HideTooltip();
		}

		protected virtual void Update()
		{
			if (!Showing && shouldShow && Time.unscaledTime >= entered + waitTime)
			{
				Tooltip.Instance.RequestShow(gameObject, toShow);
				Showing = true;
			}
		}

		/// <summary>
		/// Show a tooltip with the provided entity.
		/// </summary>
		/// <param name="entity">The entity to show.</param>
		public void Show(ModuleEntity entity)
		{
			if ((Showing || shouldShow) && entity == toShow)
				return;

			if (!shouldShow)
				entered = Time.unscaledTime;

			shouldShow = true;
			toShow = entity;

			if (Tooltip.Instance.IsShowing)
			{
				Showing = true;
				Tooltip.Instance.RequestShow(gameObject, entity);
			}
		}

		/// <summary>
		/// Stop showing this tooltip.
		/// </summary>
		public void HideTooltip()
		{
			if (Showing)
				Tooltip.Instance.ReleaseRequest(gameObject);

			Showing = false;
			shouldShow = false;
		}
	}
}