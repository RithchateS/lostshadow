using System;
using System.Collections;
using Controller;
using Data;
using Manager;
using Old.Manager;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


namespace Old.Abstract
{
    public class DataTypesSpecialCode : MonoBehaviour
    {
        [SerializeField] Button applyButton;
        [SerializeField] AudioClipData audioClipData;
        [SerializeField] private GameObject credit;
        private string _text;
        private bool _isInTransition;

        private void Start()
        {
            applyButton.onClick.AddListener(Apply);
            _isInTransition = false;
            SoundManager.Instance.PlayMusic(audioClipData.GetAudioClip(0), 0.3f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_isInTransition)
            {
                StartCoroutine(Skip());
            }
            
        }

        IEnumerator Skip()
        {
            _isInTransition = true;
            StartCoroutine(TransitionController.Instance.EndTransition());
            yield return new WaitForSeconds(3f);
            LoadSceneManager.Instance.StartLoadingScene(SceneCollection.MainMenu);
            
        }

       public void ToggleChange(string text)
        {
            _text = text;
        }

       public void Apply()
       {
           StartCoroutine(ApplyCode());
           Debug.Log("Apply");
       }

       private IEnumerator ApplyCode()
        {
            if (_text == "1232123")
            {
                applyButton.onClick.RemoveListener(Apply);
                StartCoroutine(TransitionController.Instance.EndTransition());
                yield return new WaitForSeconds(3f);
                credit.SetActive(false);
            }
        }
    }
    
}
