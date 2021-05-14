using UnityEditor;
using UnityEditor.SceneManagement;

namespace Celeritas.Editor
{
	public class EditorSceneLoader
	{
		[MenuItem("Celeritas/Load Game Scenes", priority = 20)]
		public static void LoadGameScenes()
		{
			EditorSceneManager.OpenScene(Constants.MAIN_SCENE_PATH, OpenSceneMode.Single);
			EditorSceneManager.OpenScene(Constants.PERSISTENT_SCENE_PATH, OpenSceneMode.Additive);
			EditorSceneManager.OpenScene(Constants.GAMEBACKGROUND_SCENE_PATH, OpenSceneMode.Additive);
		}

		[MenuItem("Celeritas/Load Main Menu Scenes", priority = 10)]
		public static void LoadMainMenuScenes()
		{
			EditorSceneManager.OpenScene(Constants.MAINMENU_SCENE_PATH, OpenSceneMode.Single);
			EditorSceneManager.OpenScene(Constants.PERSISTENT_SCENE_PATH, OpenSceneMode.Additive);
		}
	}
}
