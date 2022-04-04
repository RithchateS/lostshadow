using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioSource effectsSource;
        public AudioSource musicSource;
        
        public float lowPitchRange = .95f;
        public float highPitchRange = 1.05f;

        public void Start()
        {
            if (GetComponent<AudioSource>() == null)
            {
                gameObject.AddComponent<AudioSource>();
            }

            effectsSource = GetComponent<AudioSource>();
            musicSource = GetComponent<AudioSource>();

        }

        public void PlayEffect(AudioClip clip, float volume)
        {
            effectsSource.PlayOneShot(clip, volume);
        }
        
        public void PlayMusic(AudioClip clip, float volume)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.volume = volume;
            musicSource.Play();
        }
        
        public void RandomSoundEffect(AudioClip[] clips, float volume)
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);
            
            effectsSource.pitch = randomPitch;
            effectsSource.clip = clips[randomIndex];
            effectsSource.PlayOneShot(clips[randomIndex], volume);
        }
    }
}

