using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeritas
{
	public class SceneLoader : MonoBehaviour
	{
		public enum Scene
		{
			Persistent,
			Mainmenu,
			Main,
		}

		[SerializeField]
		private Scene scene;

		[SerializeField]
		private LoadSceneMode mode;

		[SerializeField, InfoBox("Load on awake only works in builds, does not occur in editor. Destroys component")]
		private bool loadOnAwake;

		private void Awake()
		{
			if (loadOnAwake)
			{
#if !UNITY_EDITOR
			LoadScene();
#endif
				Destroy(this);
			}
		}

		public void LoadScene()
		{
			string path = "";

			if (scene == Scene.Main)
				path = Constants.MAIN_SCENE_PATH;
			else if (scene == Scene.Persistent)
				path = Constants.PERSISTENT_SCENE_PATH;
			else if (scene == Scene.Mainmenu)
				path = Constants.MAINMENU_SCENE_PATH;

			if (path != "")
				SceneManager.LoadScene(path, mode);
		}
	}
}
