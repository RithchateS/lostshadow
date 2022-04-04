
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Manager;
using UnityEngine;

namespace Controller
{
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
        }

        private void Start()
        {
            _animator = transform.GetChild(0).GetComponent<Animator>();
            if (LevelManager.Instance != null)
            {
                _isLevel = true;
            }
        }

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
        }
        public IEnumerator StartTransition()
        {
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
