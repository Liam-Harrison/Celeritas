using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Celeritas.UI
{
	public class CanvasScaler : MonoBehaviour
	{

		private RectTransform rect;

		private void Awake()
		{
			rect = GetComponent<RectTransform>();
		}

		public void SetSize(float x, float y)
		{
			float sx = x / rect.sizeDelta.x;
			float sy = y / rect.sizeDelta.y;

			rect.localScale = new Vector3(sx, sy, 1);

		}
	}
}