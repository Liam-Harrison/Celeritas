using Celeritas.Game;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class FMODManager : Singleton<FMODManager>
{
	[SerializeField]
	private EventReference inGameMusic;
	public EventInstance InGameMusicInstance { get; private set; }

	protected override void Awake()
	{
		InGameMusicInstance = RuntimeManager.CreateInstance(inGameMusic);
		InGameMusicInstance.start();

		if (GameStateManager.Instance != null && GameStateManager.Instance.GameState != GameState.MAINMENU)
			OnGameStateChanged(GameState.MAINMENU, GameState.BACKGROUND);
	}

	protected override void OnGameLoaded()
    {
	    base.OnGameLoaded();

	    if (GameStateManager.Instance.GameState != GameState.MAINMENU)
		    OnGameStateChanged(GameState.MAINMENU, GameState.BACKGROUND);
    }

    private void OnEnable()
    {
	    GameStateManager.onStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
	    GameStateManager.onStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState old, GameState state)
    {
	    switch (state)
	    {
		    // Mainmenu >> Ingame
		    case GameState.BACKGROUND when old == GameState.MAINMENU:
			    InGameMusicInstance.start();
			    InGameMusicInstance.setParameterByName("HealthIntensity", 0);
			    InGameMusicInstance.setParameterByName("Intensity", 0);
			    break;
		    case GameState.MAINMENU:
			    InGameMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
			    InGameMusicInstance.setParameterByName("HealthIntensity", 0);
			    break;
		    case GameState.WAVE:
			    InGameMusicInstance.setParameterByName("Intensity", 1.0f);
			    break;
	    }
    }
}
