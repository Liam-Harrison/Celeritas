using System;
using System.Collections;
using System.Collections.Generic;
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
		if (GameStateManager.Instance.GameState != GameState.MAINMENU)
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
		    case GameState.MAINMENU:
			    // InGameMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
			    InGameMusicInstance.setParameterByName("HealthIntensity", 0);
			    break;
		    case GameState.BACKGROUND when old == GameState.MAINMENU:
			    // InGameMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
			    InGameMusicInstance.setParameterByName("HealthIntensity", 0);
			    break;
		    case GameState.BACKGROUND when old == GameState.WAVE:
			    InGameMusicInstance.start();
			    InGameMusicInstance.setParameterByName("Intensity", 0);
			    break;
		    case GameState.BACKGROUND when old != GameState.BUILD:
			    break;
		    case GameState.BOSS:
			    break;
		    case GameState.BUILD:
			    break;
		    case GameState.WAVE:
			    InGameMusicInstance.setParameterByName("Intensity", 1.0f);
			    break;
		    default:
			    InGameMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
			    InGameMusicInstance.setParameterByName("HealthIntensity", 0);
			    break;
	    }
    }
}
