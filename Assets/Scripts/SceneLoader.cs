using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using System;
using Celeritas.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeritas
{
	/// <summary>
	/// This class allows for other scripts or components in the scene to load game scenes.
	/// </summary>
	public class SceneLoader : MonoBehaviour
	{
		[Flags]
		public enum Scene
		{
			Persistent = 1 << 1,
			Mainmenu = 1 << 2,
			Main = 1 << 3,
			RunStart = 1 << 4,
		}

		[SerializeField]
		private Scene scene;

		[SerializeField]
		private LoadSceneMode mode;

		[SerializeField]
		private bool unloadAllEntities;

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

		/// <summary>
		/// Load the scenes setup in this component according to its scene flag settings.
		/// </summary>
		public void LoadScene()
		{
			bool loaded = false;

			if (unloadAllEntities)
				EntityDataManager.UnloadAllEntities();

			Time.timeScale = 1;

			if (scene.HasFlag(Scene.Main))
			{
				SceneManager.LoadSceneAsync(Constants.MAIN_SCENE_PATH, loaded ? LoadSceneMode.Additive : mode);
				loaded = true;
			}

			if (scene.HasFlag(Scene.Persistent))
			{
				SceneManager.LoadSceneAsync(Constants.PERSISTENT_SCENE_PATH, loaded ? LoadSceneMode.Additive : mode);
				loaded = true;
			}

			if (scene.HasFlag(Scene.Mainmenu))
			{
				GameStateManager.Instance.SetGameState(GameState.MAINMENU);
				SceneManager.LoadSceneAsync(Constants.MAINMENU_SCENE_PATH, loaded ? LoadSceneMode.Additive : mode);
			}

			if (scene.HasFlag(Scene.RunStart))
			{
				GameStateManager.Instance.SetGameState(GameState.MAINMENU);
				SceneManager.LoadSceneAsync(Constants.RUNSTART_PATH, loaded ? LoadSceneMode.Additive : mode);
			}
		}
	}
}
