using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	private SoundData[] sounds;

    [SerializeField]
    private AudioSource source;

    void Update () {
        if (!source.isPlaying) {
            source.clip = GetRandomSound().Clip;
            source.Play();
        }
    }

    private SoundData GetRandomSound() {
        return sounds[Random.Range(0,sounds.Length)];
    }
}
