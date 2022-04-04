using UnityEngine;

namespace Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioSource effectsSource;
        public AudioSource musicSource;
        
        //public float LowPitchRange = .95f;
        //public float HighPitchRange = 1.05f;
        
        public void PlayEffect(AudioClip clip, int volume)
        {
            effectsSource.clip = clip;
            effectsSource.PlayOneShot(clip, volume);
        }
        
        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
        
        public void RandomSoundEffect(params AudioClip[] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);
            //float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

            //effectsSource.pitch = randomPitch;
            effectsSource.clip = clips[randomIndex];
            effectsSource.Play();
        }
    }
}

