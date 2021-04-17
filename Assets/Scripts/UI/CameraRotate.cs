using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Rotates the camera slightly in the +y direction.
/// </summary>
public class CameraRotate : MonoBehaviour
{
	[SerializeField, PropertyRange(0.0f, 10.0f)]
	public float speed;
	[SerializeField, PropertyRange(0.0f, 10.0f)]
	public float forwardSpeed;

	private void Update()
	{
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + speed, transform.localEulerAngles.z);
	}
}
