using System.Collections.Generic;
using UnityEngine;
using Util.Enums;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private List<SoundWrapper> soundClips;

        private Dictionary<SoundType, AudioClip> _soundMap;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _soundMap = new Dictionary<SoundType, AudioClip>();

            foreach (SoundWrapper soundWrapper in soundClips)
            {
                _soundMap[soundWrapper.SoundType] = soundWrapper.AudioClip;
            }
        }

        public void PlaySfx(SoundType soundType)
        {
            if (_soundMap.TryGetValue(soundType, out AudioClip clip))
            {
                sfxSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning($"SFX clip '{soundType}' not found!");
            }
        }

        public void PlaySfx(AudioClip audioClip)
        {
            sfxSource.PlayOneShot(audioClip);
        }


        public void SetSfxVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }

        public void MuteAll(bool mute)
        {
            sfxSource.mute = mute;
        }
    }
}