using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.UI
{
	public class Quit : MonoBehaviour
	{
		public void QuitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}