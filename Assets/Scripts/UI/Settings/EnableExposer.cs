using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Celeritas.UI
{
	public class EnableExposer : MonoBehaviour
	{
		[SerializeField, TitleGroup("Events")]
		private UnityEvent onEnabled;

		private void OnEnable()
		{
			onEnabled.Invoke();
		}
	}
}