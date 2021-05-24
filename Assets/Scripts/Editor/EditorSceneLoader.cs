using UnityEditor;
using UnityEditor.SceneManagement;

namespace Celeritas.Editor
{
	/// <summary>
	/// Contains menu options for loading game scenes quickly in the editor.
	/// </summary>
	public class EditorSceneLoader
	{
		[MenuItem("Celeritas/Load Game Scenes", priority = 20)]
		public static void LoadGameScenes()
		{
			EditorSceneManager.OpenScene(Constants.MAIN_SCENE_PATH, OpenSceneMode.Single);
			EditorSceneManager.OpenScene(Constants.PERSISTENT_SCENE_PATH, OpenSceneMode.Additive);
		}

		[MenuItem("Celeritas/Load Main Menu Scenes", priority = 10)]
		public static void LoadMainMenuScenes()
		{
			EditorSceneManager.OpenScene(Constants.MAINMENU_SCENE_PATH, OpenSceneMode.Single);
			EditorSceneManager.OpenScene(Constants.PERSISTENT_SCENE_PATH, OpenSceneMode.Additive);
		}

		[MenuItem("Celeritas/Load Run Start Scenes", priority = 30)]
		public static void LoadRunStartScenes()
		{
			EditorSceneManager.OpenScene(Constants.RUNSTART_PATH, OpenSceneMode.Single);
			EditorSceneManager.OpenScene(Constants.PERSISTENT_SCENE_PATH, OpenSceneMode.Additive);
		}
	}
}
