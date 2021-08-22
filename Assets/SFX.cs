using Celeritas.Game;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas
{
	[System.Serializable]
	public class SFXList
	{
		public GameState state;
		public List<AudioClip> clips = new List<AudioClip>();
	}

	public class SFX : Singleton<SFX>
	{
		[SerializeField]
		private List<SFXList> music;

		[SerializeField]
		private AudioSource source;

		[SerializeField]
		private float musicVolume = 0.2f;

		private List<AudioClip> playing;

		private void Start()
		{
			OnGameStateChanged(GameStateManager.Instance.GameState);
		}

		private void OnEnable()
		{
			GameStateManager.onStateChanged += OnGameStateChanged;
		}

		private void OnDisable()
		{
			GameStateManager.onStateChanged -= OnGameStateChanged;
		}

		void Update()
		{
			if (!source.isPlaying && playing != null && playing.Count > 0)
			{
				var index = playing.IndexOf(source.clip);

				if (++index >= playing.Count)
					index = 0;

				source.clip = playing[index];
				source.Play();
			}
		}

		private void OnGameStateChanged(GameState state)
		{
			foreach (var sfx in music)
			{
				if (sfx.state == state)
				{
					ChangePlaylist(sfx.clips);
					return;
				}
			}
		}

		private void ChangePlaylist(List<AudioClip> list)
		{
			if (list == null || list.Count == 0)
				return;

			playing = list;
			ChangeClip(list[Random.Range(0, list.Count - 1)]);
		}

		private const float ChangeTime = 2f;
		private Coroutine changing;
		private Tweener tweener;

		private void ChangeClip(AudioClip clip)
		{
			if (changing != null)
				StopCoroutine(changing);

			changing = StartCoroutine(ChangeClipCoroutine(clip));
		}

		private IEnumerator ChangeClipCoroutine(AudioClip clip)
		{
			if (tweener != null)
				tweener.Kill();

			if (!source.isPlaying)
			{
				source.clip = clip;
				source.Play();

				tweener = DOTween.To(() => source.volume, (x) => source.volume = x, musicVolume, ChangeTime);
				while (tweener.active && !tweener.IsComplete())
					yield return null;
			}
			else
			{
				tweener = DOTween.To(() => source.volume, (x) => source.volume = x, 0, ChangeTime);
				while (tweener.active && !tweener.IsComplete())
					yield return null;

				source.clip = clip;
				source.Play();

				tweener = DOTween.To(() => source.volume, (x) => source.volume = x, musicVolume, ChangeTime);
				while (tweener.active && !tweener.IsComplete())
					yield return null;
			}

			yield break;
		}
	}
}
