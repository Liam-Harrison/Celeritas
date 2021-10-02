using Celeritas.Game;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas
{
	public enum UISound
	{
		Hover,
		Positive,
		Negative
	}

	[System.Serializable]
	public class SFXList
	{
		public AudioClip all;

		public AudioClip bed;

		public List<AudioClip> loops;

		public AudioClip waveStart;

		public AudioClip waveEnd;

		public AudioClip levelEnd;

		public AudioClip mainStart;

		public AudioClip mainLoop;
	}

	public class SFX : Singleton<SFX>
	{
		[SerializeField, TabGroup("Music")]
		private SFXList music;

		[SerializeField, TabGroup("Music")]
		private AudioSource primary;

		[SerializeField, TabGroup("Music")]
		private AudioSource secondary;

		[SerializeField, TabGroup("Music")]
		private AudioSource tetriary;

		[SerializeField, TabGroup("Music")]
		private AudioSource ui;

		[SerializeField, TabGroup("UI SFX")]
		private AudioClip hovered;

		[SerializeField, TabGroup("UI SFX")]
		private AudioClip positive;

		[SerializeField, TabGroup("UI SFX")]
		private AudioClip negative;

		[SerializeField]
		private float musicVolume = 0.2f;

		private readonly List<AudioClip> sequence = new List<AudioClip>();

		private int loopSequenceIndex = -1;
		private bool loopSequenceEnd = false;
		private bool sequenceSmooth = false;

		public AudioSource TetriaryDevice { get => tetriary; }

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

		void Update()
		{
			if (!primary.isPlaying && sequence.Count > 0)
			{
				var i = sequence.IndexOf(primary.clip);
				i++;

				if (i == sequence.Count)
				{
					if (loopSequenceIndex > -1)
						i = loopSequenceIndex;
					else
						sequence.Clear();
				}

				if (sequenceSmooth)
					ChangeClipOverTime(sequence[i], i == sequence.Count - 1 && loopSequenceEnd);
				else
					ChangeClip(sequence[i], i == sequence.Count - 1 && loopSequenceEnd, false);
			}
		}

		public void PlayUISFX(UISound sound)
		{
			var clip = GetAudio(sound);
			if (clip != null)
			{
				ui.PlayOneShot(clip);
			}
		}

		private AudioClip GetAudio(UISound sound)
		{
			switch (sound)
			{
				case UISound.Hover:
					return hovered;

				case UISound.Positive:
					return positive;

				case UISound.Negative:
					return negative;

				default:
					return null;
			}
		}

		private void OnGameStateChanged(GameState old, GameState state)
		{
			if (state == GameState.MAINMENU)
			{
				ChangeClipOverTime(music.mainLoop, true);
			}
			else if (state == GameState.BACKGROUND && old == GameState.MAINMENU)
			{
				ChangeClipOverTime(music.bed, true);
			}
			else if (state == GameState.BACKGROUND && old == GameState.WAVE)
			{
				ChangeSequence(new AudioClip[] { music.waveEnd, music.bed }, true, false, -1, true, true);
			}
			else if (state == GameState.BACKGROUND && old != GameState.BUILD)
			{
				ChangeClip(music.bed, true, true);
			}
			else if (state == GameState.BOSS)
			{
				ChangeClip(music.all, true, true);
			}

			if (state == GameState.WAVE)
			{
				secondary.clip = music.loops[Random.Range(0, music.loops.Count - 1)];
				secondary.time = primary.time;
				secondary.loop = true;
				secondary.volume = 0;
				secondary.Play();

				if (secondary_tweener != null && secondary_tweener.active)
					secondary_tweener.Kill();

				secondary_tweener = DOTween.To(() => secondary.volume, (x) => secondary.volume = x, musicVolume + 0.05f, ChangeTime + 1);
			}
			else
			{
				secondary_tweener.Kill();
				secondary.Stop();
			}
		}

		private void ChangeSequence(AudioClip[] list, bool sudden, bool useTime, int loopIndex, bool loopEnd, bool fadeIn)
		{
			if (list == null || list.Length == 0)
				return;

			loopSequenceIndex = loopIndex;
			loopSequenceEnd = loopEnd;
			sequenceSmooth = fadeIn;

			sequence.Clear();
			sequence.AddRange(list);

			if (sudden)
				ChangeClip(sequence[0], false, useTime);
			else
				ChangeClipOverTime(sequence[0], false);
		}

		private void ChangeClip(AudioClip clip, bool loop, bool useTime)
		{
			var start = primary.time;
			primary.clip = clip;
			primary.loop = loop;

			if (useTime)
				primary.time = start;

			primary.Play();
		}

		private const float ChangeTime = 0.5f;
		private Coroutine changing;
		private Tweener tweener;
		private Tweener secondary_tweener;

		private void ChangeClipOverTime(AudioClip clip, bool loop)
		{
			if (changing != null)
				StopCoroutine(changing);

			changing = StartCoroutine(ChangeClipCoroutine(clip, loop));
		}

		private IEnumerator ChangeClipCoroutine(AudioClip clip, bool loop)
		{
			if (tweener != null)
				tweener.Kill();

			if (!primary.isPlaying)
			{
				primary.clip = clip;
				primary.loop = loop;
				primary.Play();

				tweener = DOTween.To(() => primary.volume, (x) => primary.volume = x, musicVolume, ChangeTime);
				while (tweener.active && !tweener.IsComplete())
					yield return null;
			}
			else
			{
				tweener = DOTween.To(() => primary.volume, (x) => primary.volume = x, 0, ChangeTime);
				while (tweener.active && !tweener.IsComplete())
					yield return null;

				primary.clip = clip;
				primary.loop = loop;
				primary.Play();

				tweener = DOTween.To(() => primary.volume, (x) => primary.volume = x, musicVolume, ChangeTime);
				while (tweener.active && !tweener.IsComplete())
					yield return null;
			}

			yield break;
		}
	}
}
