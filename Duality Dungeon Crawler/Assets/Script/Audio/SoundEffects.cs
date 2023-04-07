
using System.Collections;
using System.Collections.Generic;
using CaptainCoder.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Effects", menuName = "BodyMind/SoundEffects")]
public class SoundEffects : ScriptableObject
{
    public static SoundEffects Instance => GameManager.Instance.SoundEffects;
    [field: SerializeField]
    public AudioClip WallBump {get; private set; }
    [field: SerializeField]
    public AudioClip HarmonySpell { get; private set; }
    [field: SerializeField]
    public AudioClip ShadeSpell { get; private set; }
    [field: SerializeField]
    public AudioClip LightSpell { get; private set; }
    [field: SerializeField]
    public AudioClip FloatSpell { get; private set; }
    [field: SerializeField]
    public AudioClip LunsEye { get; private set; }
    [field: SerializeField]
    public AudioClip SolsEye { get; private set; }
    [field: SerializeField]
    public AudioClip DensitySpell { get; private set; }
    [field: SerializeField]
    public AudioClip SpikesExtend { get; private set; }
    [field: SerializeField]
    public AudioClip SpikesRetract { get; private set; }
    [field: SerializeField]
    public AudioClip Hurt { get; private set; }

}