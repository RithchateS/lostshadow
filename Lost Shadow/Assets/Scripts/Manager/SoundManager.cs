using UnityEngine;


namespace Manager
{
    /// <summary>
    /// This class is used to manage sound effects.
    /// </summary>
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

        /// <summary>
        /// Play Effect one time
        /// </summary>
        /// <param name="clip">Sound Effect you want to play</param>
        /// <param name="volume">Volume of the Sound effect</param>
        public void PlayEffect(AudioClip clip, float volume)
        {
            effectsSource.PlayOneShot(clip, volume);
        }
        
        /// <summary>
        /// Play Music in loop
        /// </summary>
        /// <param name="clip">Music you want to play</param>
        /// <param name="volume">volume of the music (change globally)</param>
        public void PlayMusic(AudioClip clip, float volume)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.volume = volume;
            musicSource.Play();
        }
        
        /// <summary>
        /// Randomize the sound effect and pitch
        /// </summary>
        /// <param name="clips">Array of clips to random from</param>
        /// <param name="volume">Volume of the sound effect</param>
        public void RandomSoundEffect(AudioClip[] clips, float volume)
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);
            
            effectsSource.pitch = randomPitch;
            effectsSource.clip = clips[randomIndex];
            effectsSource.PlayOneShot(clips[randomIndex], volume);
        }
        
        /// <summary>
        /// Stop the sound effect
        /// </summary>
        public void StopMusic()
        {
            effectsSource.Stop();
        }
    }
}

