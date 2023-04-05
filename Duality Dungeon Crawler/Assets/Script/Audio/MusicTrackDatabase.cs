using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptainCoder.Audio
{
    [CreateAssetMenu(fileName = "MusicTrackDatabase", menuName = "BodyMind/Music Track Database")]
    public class MusicTrackDatabase : ScriptableObject
    {
        [field: SerializeField]
        public AudioClip SunTowerAmbience;
    }
}