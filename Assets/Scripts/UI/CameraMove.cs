using UnityEngine;
using Sirenix.OdinInspector;

namespace Celeritas.Game
{
	/// <summary>
	/// Rotates the camera slightly in the +y direction.
	/// </summary>
	public class CameraMove : MonoBehaviour
	{
		[SerializeField, PropertyRange(0f, 1000f)]
		public float speed;

		[SerializeField]
		public float sin;

		[SerializeField]
		public float cos;

		private void Update()
		{
			var rot = Quaternion.Euler(Mathf.Sin(Time.time / sin), Mathf.Cos(Time.time / cos), 0);
			transform.position = (rot * Vector3.forward).normalized * speed;
		}
	}
}