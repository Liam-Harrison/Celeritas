using UnityEngine;

namespace Celeritas.UI
{
	public class OpenURL : MonoBehaviour
	{
		[SerializeField]
		private string url;

		public void LaunchURL()
		{
			Application.OpenURL(url);
		}
	}
}