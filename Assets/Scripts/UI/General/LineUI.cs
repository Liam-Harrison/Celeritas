using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Celeritas.UI.General
{
	/// <summary>
	/// Allows for the simple drawing of a line in the UI between a UI element and a world element.
	/// </summary>
	[RequireComponent(typeof(UILineRenderer))]
	public class LineUI : MonoBehaviour
	{
		private UILineRenderer line;

		[SerializeField]
		private RectTransform uiTarget;

		[SerializeField]
		private Transform worldTarget;


		/// <summary>
		/// The UI target of this line renderer.
		/// </summary>
		public RectTransform UITarget
		{
			get => uiTarget;
			set => uiTarget = value;
		}

		/// <summary>
		/// The world target of this line renderer.
		/// </summary>
		public Transform WorldTarget
		{
			get => worldTarget;
			set => worldTarget = value;
		}

		private void Awake()
		{
			line = GetComponent<UILineRenderer>();
			line.Points = new Vector2[4];
		}

		private void Update()
		{
			if (worldTarget == null || uiTarget == null)
			{
				line.enabled = false;
				return;
			}

			line.enabled = true;

			var p = transform.InverseTransformPoint(Camera.main.WorldToScreenPoint(worldTarget.position));
			var ui = transform.InverseTransformPoint(uiTarget.position);
			var d = p - ui;

			line.Points[0] = ui;
			line.Points[1] = ui + new Vector3(Mathf.Sign(d.x) * 100, 0);
			line.Points[2] = p + new Vector3(Mathf.Sign(-d.x) * 100, 0);
			line.Points[3] = p;

			line.SetVerticesDirty();
		}
	}
}