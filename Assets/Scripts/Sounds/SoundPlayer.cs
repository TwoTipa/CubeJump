using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Sounds
{
    public class SoundPlayer : MonoBehaviour
    {
        public static SoundPlayer Instance { get; private set; }

        [SerializeField] private AudioSource effectSource, musicSource;
        private float _masterVolume = 0.1f;

        public void ChangeVolume(float value)
        {
            _masterVolume = value;
        }
        
        public void SwichAudio(AudioSource obj)
        {
            obj.mute = !obj.mute;
        }
        
        public void PlayClip(AudioClip clip)
        {
            effectSource.PlayOneShot(clip, _masterVolume);
        }
        
        private void Awake()
        {
            Instance = this;
        }
    }
}