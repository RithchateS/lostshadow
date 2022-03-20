using System;
using UnityEngine;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform startPos;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject cameraPrefab;
    
        private GameObject _player; 
        private GameObject _camera;

        #endregion

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _camera = GameObject.FindWithTag("Camera");
            startPos = GameObject.Find("StartPos").transform;
            if (_player == null) {
                Instantiate(playerPrefab, startPos.position, startPos.rotation);
                GameSaveManager.Instance.SaveGame();

            }
            
            if (_camera == null)
            {
                Instantiate(cameraPrefab, startPos.position, startPos.rotation);
            }
        }
        
        
    }
}
