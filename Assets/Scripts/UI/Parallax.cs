using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI
{
	/// <summary>
	/// Allows a image texture to scroll in the background according to the cameras position.
	/// </summary>
	[RequireComponent(typeof(Image))]
	public class Parallax : MonoBehaviour
	{
		[SerializeField]
		private float meters;

		private float x, y;

		private Transform cameraTransform;

		private Image image;

		private void Awake()
		{
			image = GetComponent<Image>();
			cameraTransform = Camera.main.transform;
			image.material = new Material(image.material);
		}

		private void Update()
		{
			var xtarget = Mathf.Repeat(cameraTransform.position.x, meters) / meters;
			var ytarget = Mathf.Repeat(cameraTransform.position.y, meters) / meters;

			x = CMathf.LerpWrap(x, xtarget, 1f, 0.90f);
			y = CMathf.LerpWrap(y, ytarget, 1f, 0.90f);

			image.material.SetTextureOffset("_MainTex", new Vector2(x, y));
		}
	}
}
