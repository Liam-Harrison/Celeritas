using UnityEngine;
using Sirenix.OdinInspector;

namespace Celeritas.Game
{
	/// <summary>
	/// Rotates the camera slightly in the +y direction.
	/// </summary>
	public class BasicMove : MonoBehaviour
	{
		[SerializeField, PropertyRange(0f, 1000f)]
		private float speed;

		[SerializeField]
		private float sin;

		[SerializeField]
		private float cos;

		private void Update()
		{
			var rot = Quaternion.Euler(Mathf.Sin(Time.time / sin), Mathf.Cos(Time.time / cos), 0);
			transform.position = (rot * Vector3.forward).normalized * speed;
		}
	}
}