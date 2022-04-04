using Cinemachine;
using Controller;
using Data;
using Old.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    /// <summary>
    /// This class is responsible for managing the level.
    /// </summary>
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

        [Header("Camera")]
        public GameObject freeCamera;
        public GameObject mainCineCamera;
        public GameObject objectCamera;
        
        [Header("MapBounds")]
        [SerializeField] private CinemachineConfiner2D cameraBounds;
        [SerializeField] private Collider2D mapBoundsShadow;
        [SerializeField] private Collider2D mapBoundsLight;

        [Header("Canvas Object")]
        public TMP_Text shiftCountText;
        public Animator shiftIndicator;
        [SerializeField] public RectTransform peek;
        [SerializeField] public RectTransform peekMask;
        [SerializeField] public GameObject objectCam;
        [SerializeField] public GameObject objectCamMask;
        [SerializeField] public GameObject eyeEffect;
        
        [Header("Others")]
        [SerializeField] private GameObject _player;
        private GameObject _playerClone;
        public GameObject cameraObj;
        private GameObject _mainCamera;
        private GameObject _overlayCamera;
        public AudioClipData AudioClipData { get; private set; }
        public AudioSource ShadowAudio { get; private set; }
        public AudioSource LightAudio{ get; private set; }




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
        }
        
    }
    
    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        cameraObj = GameObject.FindWithTag("Camera");
        _mainCamera = GameObject.FindWithTag("MainCamera");
        _overlayCamera = GameObject.FindWithTag("OverlayCamera");
        startPos = GameObject.Find("StartPos").transform;
        cameraBounds = GameObject.Find("MainCineCamera").GetComponent<CinemachineConfiner2D>();
        peek = GameObject.FindWithTag("Peek").GetComponent<RectTransform>();
        peekMask = GameObject.FindWithTag("PeekMask").GetComponent<RectTransform>();
        freeCamera = GameObject.Find("FreeCamera");
        mainCineCamera = GameObject.Find("MainCineCamera");
        objectCamera = GameObject.Find("ObjectCamera");
        objectCam = GameObject.Find("ObjectCam");
        objectCamMask = GameObject.Find("ObjectCamMask");
        eyeEffect = GameObject.Find("EyeEffect");
        AudioClipData = GetComponent<AudioClipData>();

        if (allowShift)
        {
            shiftCountText = GameObject.Find("ShiftCount").GetComponent<TMP_Text>();
            shiftIndicator = GameObject.Find("ShiftIndicator").GetComponent<Animator>();
        }

        if (_player == null) {
            Instantiate(playerPrefab, startPos.position, startPos.rotation);
            Instantiate(playerClonePrefab, startPos.position, startPos.rotation);
            _playerClone = GameObject.FindWithTag("PlayerClone");
            _player = GameObject.FindWithTag("Player");
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
        freeCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = mapBoundsShadow;
        playerController = _player.GetComponent<PlayerController>();
        ShadowAudio = transform.GetChild(1).GetComponent<AudioSource>();
        LightAudio = transform.GetChild(2).GetComponent<AudioSource>();
        ShadowAudio.Play();
        LightAudio.Play();
        StartCoroutine(TransitionController.Instance.StartTransition());
        GameSaveManager.Instance.SaveGame();
        Debug.Log(Appdata.Instance.currentScene);

    }

    private void Update()
    {
        FollowMain();
    }

    /// <summary>
    /// Check if the animator should play cutscene animation
    /// </summary>
    public void CheckCutScene()
    {
        freeCamera.GetComponent<ObjectAim>().GameObjectToTarget(_player);
        if (SceneManager.GetActiveScene().name == "Tutorial01")
        {
            playerController.Wakeup();
        }
        else
        {
            StartCoroutine(playerController.PauseMovement(5));
        }

        cameraObj.GetComponent<Animator>().SetBool("Cutscene",true);

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

    /// <summary>
    /// Everything follow
    /// <para>Overlay Cam Follow Main Cam</para>
    /// <para>PlayerClone Follow Player</para>
    /// <para>Assign Map Bounds</para>
    /// <para> overlaycam ortho size = maincam ortho size</para>
    /// <para> objectcam ortho size = maincam ortho size</para>
    /// </summary>
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
        peek.transform.position = _mainCamera.transform.position;
        //_overlayCamera.transform.position = _playerClone.transform.position;
        _overlayCamera.GetComponent<Camera>().orthographicSize = _mainCamera.GetComponent<Camera>().orthographicSize;
        objectCamera.GetComponent<Camera>().orthographicSize = _mainCamera.GetComponent<Camera>().orthographicSize;
    }
    
    // public string ToRoman(int number)
    // {
    //     if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
    //     if (number < 1) return string.Empty;            
    //     if (number >= 1000) return "M" + ToRoman(number - 1000);
    //     if (number >= 900) return "CM" + ToRoman(number - 900); 
    //     if (number >= 500) return "D" + ToRoman(number - 500);
    //     if (number >= 400) return "CD" + ToRoman(number - 400);
    //     if (number >= 100) return "C" + ToRoman(number - 100);            
    //     if (number >= 90) return "XC" + ToRoman(number - 90);
    //     if (number >= 50) return "L" + ToRoman(number - 50);
    //     if (number >= 40) return "XL" + ToRoman(number - 40);
    //     if (number >= 10) return "X" + ToRoman(number - 10);
    //     if (number >= 9) return "IX" + ToRoman(number - 9);
    //     if (number >= 5) return "V" + ToRoman(number - 5);
    //     if (number >= 4) return "IV" + ToRoman(number - 4);
    //     if (number >= 1) return "I" + ToRoman(number - 1);
    //     throw new ArgumentOutOfRangeException("something bad happened");
    // }

    }
}
