using System.Collections;
using System.Collections.Generic;
using CaptainCoder.Audio;
using UnityEngine;

public class SoundEffectController 
{
    public static AudioSource PlaySFX(AudioClip clip) => MusicController.Instance.PlaySFX(clip);
}

public static class SFXExtensions
{
    public static AudioSource Play(this AudioClip clip) => SoundEffectController.PlaySFX(clip);
}