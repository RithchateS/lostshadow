using Cinemachine;
using Controller;
using Old.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        [Header("Settings")] 
        public int shiftCountStart;
        public bool allowShift;

        [Header("MapBounds")]
        [SerializeField] private CinemachineConfiner2D cameraBounds;
        [SerializeField] private Collider2D mapBoundsShadow;
        [SerializeField] private Collider2D mapBoundsLight;

        private GameObject _player;
        private GameObject _playerClone;
        public GameObject cameraObj;
        private GameObject _mainCamera;
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

        //DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        Debug.Log(Appdata.Instance.currentScene);
        _player = GameObject.FindWithTag("Player");
        cameraObj = GameObject.FindWithTag("Camera");
        _mainCamera = GameObject.FindWithTag("MainCamera");
        _overlayCamera = GameObject.FindWithTag("OverlayCamera");
        startPos = GameObject.Find("StartPos").transform;
        cameraBounds = GameObject.Find("MainCineCamera").GetComponent<CinemachineConfiner2D>();

        if (_player == null) {
            Instantiate(playerPrefab, startPos.position, startPos.rotation);
            Instantiate(playerClonePrefab, startPos.position, startPos.rotation);
            _playerClone = GameObject.FindWithTag("PlayerClone");
            _player = GameObject.FindWithTag("Player");
            //StartCutscene();
        }
            
        if (cameraObj == null)
        {
            Instantiate(cameraPrefab, startPos.position, startPos.rotation);
            _overlayCamera = GameObject.FindWithTag("OverlayCamera");
        }
        if (GameObject.Find("MapBoundsLight") != null)
        {
            mapBoundsLight = GameObject.Find("MapBoundsLight").GetComponent<Collider2D>();
        }
        mapBoundsShadow = GameObject.Find("MapBoundsShadow").GetComponent<Collider2D>();
        cameraBounds.m_BoundingShape2D = mapBoundsShadow;
        playerController = _player.GetComponent<PlayerController>();
        
        GameSaveManager.Instance.SaveGame();
        
    }

    private void Update()
    {
        FollowMain();
    }

    public void CheckCutScene()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial01")
        {
            cameraObj.GetComponent<Animator>().SetBool("Cutscene",true);
            playerController.Wakeup();
        }
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
            _overlayCamera.transform.position = _mainCamera.transform.position - new Vector3(0, 100);
            _playerClone.transform.position = _player.transform.position - new Vector3(0, 100);
            cameraBounds.m_BoundingShape2D = mapBoundsShadow;
        }
        else
        {
            _overlayCamera.transform.position = _mainCamera.transform.position + new Vector3(0, 100);
            _playerClone.transform.position = _player.transform.position + new Vector3(0, 100);
            cameraBounds.m_BoundingShape2D = mapBoundsLight;
        }
        //playerController.peek.transform.position = _player.transform.position;
        playerController.peek.transform.position = _mainCamera.transform.position;
        //_overlayCamera.transform.position = _playerClone.transform.position;
        _overlayCamera.GetComponent<Camera>().orthographicSize = _mainCamera.GetComponent<Camera>().orthographicSize;
    }
    

    /*Void StartCutscene()
        {
            
        }*/
        
    }
}
