using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor]
[CreateAssetMenu(fileName = "SoundData", menuName = "Celeritas/New Sound")]
public class SoundData : ScriptableObject
{
    [SerializeField]
    private AudioClip clip;

    private AudioSource source;

    public AudioSource Source { get => source; set => source = value; }
    public AudioClip Clip { get => clip; set => clip = value; }
}
