using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioSystem
{
    [Serializable]
    public class SoundData
    {

        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public float volume = 0.01f;
        public float spatialBlend = 1.0f;
        public float maxDistance = 1000;
        public int priority = 50;
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
        public bool loop;
        public bool playOnAwake;
        public bool frequentSound;
    }
}
