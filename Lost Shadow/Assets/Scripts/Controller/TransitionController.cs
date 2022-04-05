using System.Collections;
using Manager;
using UnityEngine;

namespace Controller
{
    /// <summary>
    /// This class is responsible for scene transitions.
    /// </summary>
    public class TransitionController : MonoBehaviour
    {
        private Animator _animator;
        public static TransitionController Instance;
        private bool _isLevel;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            _animator = transform.GetChild(0).GetComponent<Animator>();
        }

        private void Start()
        {

            if (LevelManager.Instance != null)
            {
                _isLevel = true;
            }
        }

        /// <summary>
        /// End transition in the scene
        /// Also Decrease the volume of the music
        /// </summary>
        /// <returns></returns>
        public IEnumerator EndTransition()
        {
            _animator.SetTrigger("End");
            while (SoundManager.Instance.musicSource.volume > 0)
            {
                yield return new WaitForSeconds(0.01f);
                SoundManager.Instance.musicSource.volume -= 0.01f;
                if (_isLevel)
                {
                    while (LevelManager.Instance.ShadowAudio.volume > 0)
                    {
                        LevelManager.Instance.ShadowAudio.volume -= 0.01f;
                        LevelManager.Instance.LightAudio.volume -= 0.01f;
                        yield return new WaitForSeconds(0.01f);
                    }

                }
            }

            _animator.ResetTrigger("Start");
        }

        /// <summary>
        /// Start transition in the scene.
        /// Also Increase the volume of the music.
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartTransition()
        {
            _animator.ResetTrigger("End");
            while (SoundManager.Instance.musicSource.volume < 1)
            {
                yield return new WaitForSeconds(0.01f);
                SoundManager.Instance.musicSource.volume += 0.01f;
                if (_isLevel)
                {
                    while (LevelManager.Instance.ShadowAudio.volume < 0.2f)
                    {
                        LevelManager.Instance.ShadowAudio.volume += 0.005f;
                        LevelManager.Instance.LightAudio.volume += 0f;
                        yield return new WaitForSeconds(0.01f);
                    }
                }
            }
            _animator.SetTrigger("Start");
        }
    }
}
