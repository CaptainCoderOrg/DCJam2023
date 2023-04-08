using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptainCoder.Audio
{

    public class MusicController : MonoBehaviour
    {
        private static MusicController s_Instance;

        public static MusicController Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    GameObject go = new GameObject("Music Controller");
                    MusicController controller = go.AddComponent<MusicController>();
                }
                return s_Instance;
            }
        }

        [SerializeField]
        private MusicTrackDatabase _tracks;
        private AudioSource _playingAudio;
        private AudioSource _queuedAudio;
        private AudioSource _secondTrack;
        private float _fadeTime = 0;
        private bool _swapQueued;
        public event System.Action<float> OnMusicVolumeChange;
        private float? _musicVolume;
        public float MusicVolume 
        { 
            get 
            {
                if (_musicVolume == null)
                {
                    _musicVolume = PlayerPrefs.GetFloat("MusicVolume", .5f);
                }
                return _musicVolume.Value; 
            } 
            set
            {
                _musicVolume = Mathf.Clamp(value, 0, 1);
                _playingAudio.volume = _musicVolume.Value;
                _secondTrack.volume = _musicVolume.Value;
                PlayerPrefs.SetFloat("MusicVolume", _musicVolume.Value);
                OnMusicVolumeChange?.Invoke(_musicVolume.Value);
            }
        }

        public event System.Action<float> OnSFXVolumeChange;

        private float? _sfxVolume;
        public float SFXVolume 
        { 
            get 
            {
                if (_sfxVolume == null)
                {
                    _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", .5f); 
                }
                return _sfxVolume.Value; 
            } 
            set
            {
                _sfxVolume = Mathf.Clamp(value, 0, 1);
                PlayerPrefs.SetFloat("SFXVolume", _sfxVolume.Value);
                OnSFXVolumeChange?.Invoke(_sfxVolume.Value);
            }
        }

        public int MenuTrack = 0;
        public int VictoryTrack = 1;


        public void StartTrack(AudioClip clip)
        {
            AudioClip track = clip; //_tracks.Track(id);
            if (track == _playingAudio.clip) { return; }
            _fadeTime = 1;
            _swapQueued = true;
            _queuedAudio.clip = clip; //_tracks.Track(id);
            _queuedAudio.Play();
        }

        public void StartTrackImmediately(AudioClip clip)
        {
            AudioClip track = clip; //_tracks.Track(id);
            _playingAudio.clip = track;
            _playingAudio.Play();
        }

        protected void Update()
        {
            FadeMusic();
        }

        private void FadeMusic()
        {
            if (_fadeTime > 0)
            {
                _fadeTime -= Time.deltaTime;
                _playingAudio.volume = _fadeTime * MusicVolume;
                _queuedAudio.volume = (1 - _fadeTime) * MusicVolume;
            }
            else if (_swapQueued)
            {
                _swapQueued = false;

                // Swaps variables
                // For students, show them this trick but mention readability / AP exam expectations
                // (_queuedAudio, _playingAudio) = (_playingAudio, _queuedAudio);
                var temp = _playingAudio;
                _playingAudio = _queuedAudio;
                _queuedAudio = temp;
            }
        }

        public AudioSource PlaySFX(AudioClip clip)
        {
            GameObject sfx = new (clip.name);
            AudioSource source = sfx.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = SFXVolume;
            source.Play();
            void AdjustVolume(float volume)
            {
                source.volume = volume;
            }
            OnSFXVolumeChange += AdjustVolume;
            source.loop = false;
            IEnumerator DestroySource()
            {
                yield return new WaitForSeconds(clip.length + 0.05f);
                OnSFXVolumeChange -= AdjustVolume;
                GameObject.Destroy(sfx);
            }
            StartCoroutine(DestroySource());            
            return source;
        }



        public void StartSecondTrack(AudioClip clip)
        {
            _secondTrack.clip = clip;
            _secondTrack.Play();
        }

        public void StopSecondTrack()
        {
            _secondTrack.Stop();
        }

        

        protected void Awake()
        {
            Init();
            if (this.gameObject != s_Instance.gameObject)
            {
                Destroy(this.gameObject);
            }
        }

        private void Init()
        {
            if (s_Instance == null)
            {
                s_Instance = this;
                _playingAudio = gameObject.AddComponent<AudioSource>();
                _playingAudio.volume = MusicVolume;
                _playingAudio.loop = true;
                _secondTrack = gameObject.AddComponent<AudioSource>();
                _secondTrack.volume = MusicVolume;
                _secondTrack.loop = true;
                _queuedAudio = gameObject.AddComponent<AudioSource>();
                _queuedAudio.loop = true;
                // _tracks = Resources.Load<MusicTrackDatabase>("Prefabs/MusicTrackDatabase");
                // VolumeController vc = Resources.Load<VolumeController>("Prefabs/VolumeController");
                // Instantiate(vc, transform);
                DontDestroyOnLoad(this);
            }
        }
    }
}