using Old.Controller;
using UnityEngine;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform startPos;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject playerClonePrefab;
        [SerializeField] private GameObject cameraPrefab;
        [SerializeField] private PlayerController playerController;

        private GameObject _player;
        private GameObject _playerClone;
        private GameObject _camera;
        private GameObject _overlayCamera;

        #endregion

        public static LevelManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _camera = GameObject.FindWithTag("Camera");
        _overlayCamera = GameObject.FindWithTag("OverlayCamera");
        _camera = GameObject.FindWithTag("MainCamera");
        startPos = GameObject.Find("StartPos").transform;
        if (_player == null) {
            Instantiate(playerPrefab, startPos.position, startPos.rotation);
            Instantiate(playerClonePrefab, startPos.position, startPos.rotation);
            _playerClone = GameObject.FindWithTag("PlayerClone");
            _player = GameObject.FindWithTag("Player");
            playerController = _player.GetComponent<PlayerController>();
            //StartCutscene();
        }
            
        if (_camera == null)
        {
            Instantiate(cameraPrefab, startPos.position, startPos.rotation);
            _camera = GameObject.FindWithTag("MainCamera");
            _overlayCamera = GameObject.FindWithTag("OverlayCamera");
        }
        GameSaveManager.Instance.SaveGame();
        
        
        
    }

    private void Update()
    {
        FollowMain();
    }


    void FollowMain()
    {
        if (playerController.isShadow)
        {
            _overlayCamera.transform.position = _camera.transform.position - new Vector3(0, 100);
            _playerClone.transform.position = _player.transform.position - new Vector3(0, 100);
        }
        else
        {
            _overlayCamera.transform.position = _camera.transform.position + new Vector3(0, 100);
            _playerClone.transform.position = _player.transform.position + new Vector3(0, 100);
        }
        playerController.peek.transform.position = _player.transform.position;
        _overlayCamera.GetComponent<Camera>().orthographic = _camera.GetComponent<Camera>().orthographic;
    }
    

    /*Void StartCutscene()
        {
            
        }*/
        
    }
}
