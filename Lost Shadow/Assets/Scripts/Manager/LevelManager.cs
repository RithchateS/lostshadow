using Cinemachine;
using Controller;
using UnityEngine;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        #region Variables
        [Header("Startup")]
        [SerializeField] private Transform startPos;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject playerClonePrefab;
        [SerializeField] private GameObject cameraPrefab;
        [SerializeField] private PlayerController playerController;

        [Header("Settings")] public int shiftCountStart;

        [Header("MapBounds")]
        [SerializeField] private CinemachineConfiner2D cameraBounds;
        [SerializeField] private Collider2D mapBoundsShadow;
        [SerializeField] private Collider2D mapBoundsLight;

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


    // void CameraCollider()
    // {
    //     if (!_camera.GetComponent<Camera>().orthographic)
    //     {
    //         Debug.LogError("Camera must be Orthographic.");
    //         return;
    //     }
    //
    //     var aspect = (float)Screen.width / Screen.height;
    //     var orthoSize = _camera.GetComponent<Camera>().orthographicSize;
    //
    //     var width = 2.0f * orthoSize * aspect;
    //     var height = 2.0f * _camera.GetComponent<Camera>().orthographicSize;
    //
    //     _camera.GetComponent<BoxCollider2D>().size = new Vector2(width, height);
    // }

    void FollowMain()
    {
        if (playerController.isShadow)
        {
            _overlayCamera.transform.position = _camera.transform.position - new Vector3(0, 100);
            _playerClone.transform.position = _player.transform.position - new Vector3(0, 100);
            cameraBounds.m_BoundingShape2D = mapBoundsShadow;
        }
        else
        {
            _overlayCamera.transform.position = _camera.transform.position + new Vector3(0, 100);
            _playerClone.transform.position = _player.transform.position + new Vector3(0, 100);
            cameraBounds.m_BoundingShape2D = mapBoundsLight;
        }
        //playerController.peek.transform.position = _player.transform.position;
        playerController.peek.transform.position = _camera.transform.position;
        //_overlayCamera.transform.position = _playerClone.transform.position;
        _overlayCamera.GetComponent<Camera>().orthographicSize = _camera.GetComponent<Camera>().orthographicSize;
    }
    

    /*Void StartCutscene()
        {
            
        }*/
        
    }
}
