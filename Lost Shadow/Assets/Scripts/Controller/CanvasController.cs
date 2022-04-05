using System;
using System.Collections;
using Old.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controller
{
    public class CanvasController : MonoBehaviour
    {
        private PlayerController _playerController;
        [SerializeField] private GameObject gameOver;
        [SerializeField] private GameObject restartButton;
        [SerializeField] private GameObject menuButton;

        private void Start()
        {
            gameOver = transform.GetChild(1).gameObject;
            restartButton = gameOver.transform.GetChild(1).gameObject;
            menuButton = gameOver.transform.GetChild(2).gameObject;
            restartButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                StartCoroutine(EndScene(restartButton));
            });
            menuButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                StartCoroutine(EndScene(menuButton));
            });
        }


        IEnumerator EndScene(GameObject object1)
        {
            StartCoroutine(TransitionController.Instance.EndTransition());
            yield return new WaitForSeconds(3);
            if (object1 == menuButton)
            {
                LoadSceneManager.Instance.StartLoadingScene(SceneCollection.MainMenu);
            }
            if (object1 == restartButton)
            {
                SceneManager.LoadScene(Enum.GetName(typeof (SceneCollection), Appdata.Instance.currentScene), LoadSceneMode.Single);
            }
            
        }
        private void Update()
        {
            ToggleGameOverCheck();
        }
        /// <summary>
        /// Toggle the game over screen if the player is dead
        /// </summary>
        void ToggleGameOverCheck()
        {
            if (_playerController == null)
            {
                _playerController = PlayerController.Instance;
            }
            if (_playerController.IsAlive)
            {
                gameOver.SetActive(false);
                return;
            }
            gameOver.SetActive(true);
            
        }
    }
}
