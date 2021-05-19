using Celeritas.Game;
using UnityEngine;

namespace Celeritas.UI
{
	public class LoadingScreen : Singleton<LoadingScreen>
	{
		[SerializeField]
		private GameObject frame;

		/// <summary>
		/// Show the loading screen.
		/// </summary>
		public static void Show()
		{
			Instance.frame.SetActive(true);
		}

		/// <summary>
		/// Hide the loading screen.
		/// </summary>
		public static void Hide()
		{
			Instance.frame.SetActive(false);
		}
	}
}