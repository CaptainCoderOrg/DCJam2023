using CaptainCoder.Audio;
using UnityEngine;

public class SFXAdjuster : MonoBehaviour
{
    private AudioSource _audio;

    public void Awake()
    {
        _audio = GetComponent<AudioSource>();
        if (_audio != null)
        {
            _audio.volume = MusicController.Instance.SFXVolume;
        }
    }

    public void UpdateVolume(float volume)
    {
        if (_audio == null) { return; }
        _audio.volume = volume;
    }

    public void OnEnable() => _audio?.AddSFXListener(UpdateVolume);

    public void OnDisable() => _audio?.RemoveSFXListener(UpdateVolume);

}