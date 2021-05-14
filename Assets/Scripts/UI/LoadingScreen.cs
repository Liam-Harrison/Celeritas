using Celeritas.Game;
using UnityEngine;

namespace Celeritas.UI
{
	public class LoadingScreen : Singleton<LoadingScreen>
	{
		[SerializeField]
		private GameObject frame;

		public static void Show()
		{
			Instance.frame.SetActive(true);
		}

		public static void Hide()
		{
			Instance.frame.SetActive(false);
		}
	}
}