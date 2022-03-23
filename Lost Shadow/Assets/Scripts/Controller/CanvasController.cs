using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controller
{
    public class CanvasController : MonoBehaviour
    {
        private PlayerController _playerController;
        [SerializeField] private GameObject _gameOver;
        [SerializeField] private GameObject _restartButton;
        [SerializeField] private GameObject _menuButton;

        private void Start()
        {
            _gameOver = gameObject.transform.GetChild(1).gameObject;
            _restartButton = _gameOver.transform.GetChild(1).gameObject;
            _menuButton = _gameOver.transform.GetChild(2).gameObject;
            _restartButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                SceneManager.LoadScene(Enum.GetName(typeof (SceneCollection), Appdata.Instance.currentScene), LoadSceneMode.Single);
            });
            _menuButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                LoadSceneManager.Instance.StartLoadingScene(SceneCollection.MainMenu);
            });
        }
        
        
        private void Update()
        {
            ToggleGameOverCheck();
        }

        void ToggleGameOverCheck()
        {
            if (_playerController == null)
            {
                _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            }
            if (_playerController.IsAlive)
            {
                _gameOver.SetActive(false);
                return;
            }
            _gameOver.SetActive(true);
            
        }
    }
}
