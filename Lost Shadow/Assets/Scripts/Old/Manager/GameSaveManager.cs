using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Controller;
using Data;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Old.Manager
{
    public class GameSaveManager : MonoBehaviour
    {
        public Appdata playerData;
        public static GameSaveManager Instance { get; private set; }
        [Range(0f, 1f)] public float volume;
        private bool _paused;
        private bool _allowPause;
        [SerializeField] private CurrectSlot saveSlot;
        [SerializeField] private GameObject resumeButton;
        [SerializeField] private GameObject restartButton;
        [SerializeField] private GameObject menuButton;
        [SerializeField] private GameObject pauseOverlay;
        private AudioClipData _audioClipData;
        public bool IsSaveFile()
        {
            return Directory.Exists(Application.persistentDataPath + "/Game_Save");
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this);
            }
            playerData = Appdata.Instance.GetComponent<Appdata>();
            DontDestroyOnLoad(this);
            if (Screen.fullScreen == false)
            {
                Screen.fullScreen = true;
            }
        }
        
    
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2 || _allowPause == false) { return; }
                {
                    if (_paused)
                    {
                        ResumeGame();
                    }
                    else
                    {
                        PauseGame();
                    }
                }
            }
        }

        public void SaveGame()
        {
            if (saveSlot == CurrectSlot.Slot1)
            {
                Appdata.Instance.isUsed1 = true;
                Appdata.Instance.playerPosition1 = GameObject.FindGameObjectWithTag("Player").transform.position;
                Appdata.Instance.sceneInSave1 = Appdata.Instance.currentScene;
                Appdata.Instance.chapterNum1 = Appdata.Instance.currentChapter;
            }
            else if (saveSlot == CurrectSlot.Slot2)
            {
                Appdata.Instance.isUsed2 = true;
                Appdata.Instance.playerPosition2 = GameObject.FindGameObjectWithTag("Player").transform.position;
                Appdata.Instance.sceneInSave2 = Appdata.Instance.currentScene;
                Appdata.Instance.chapterNum2 = Appdata.Instance.currentChapter;
            }
            else if (saveSlot == CurrectSlot.Slot3)
            {
                Appdata.Instance.isUsed3 = true;
                Appdata.Instance.playerPosition3 = GameObject.FindGameObjectWithTag("Player").transform.position;
                Appdata.Instance.sceneInSave3 = Appdata.Instance.currentScene;
                Appdata.Instance.chapterNum3 = Appdata.Instance.currentChapter;
            }
            if (!IsSaveFile())
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Game_Save");
            }
            if (!Directory.Exists(Application.persistentDataPath + "/Game_Save/Player_Data"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Game_Save/Player_Data");
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/Game_Save/Player_Data/Player_save.txt");
            var json = JsonUtility.ToJson(playerData);
            bf.Serialize(file, json);
            file.Close();
            PlayerPrefs.SetFloat("HaveSave",1);
        }

        public void InitialSave()
        {
            if (!IsSaveFile())
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Game_Save");
            }
            if (!Directory.Exists(Application.persistentDataPath + "/Game_Save/Player_Data"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Game_Save/Player_Data");
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/Game_Save/Player_Data/Player_save.txt");
            var json = JsonUtility.ToJson(playerData);
            bf.Serialize(file, json);
            file.Close();
            Debug.Log("Create Save File Done");
        }
        public void LoadGame()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Game_Save/Player_Data/Player_save.txt",FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file),playerData); 
            file.Close();
        }
        public void SelectSaveSlot(int slot)
        {
            saveSlot = (CurrectSlot)slot;
        }

        // Use For Select Next Scene In game component
        public IEnumerator GoNextScene(int nextLevel)
        {
            _allowPause = false;
            StartCoroutine(TransitionController.Instance.EndTransition());
            SoundManager.Instance.PlayEffect(_audioClipData.GetAudioClip(1),0.3f);
            yield return new WaitForSeconds(2.5f);
            LoadSceneManager.Instance.StartLoadingScene(SceneCollection.LoadScene);
            Appdata.Instance.currentChapter = 0;
            Appdata.Instance.sceneToLoad = (SceneCollection)nextLevel;
            Appdata.Instance.currentScene = Appdata.Instance.sceneToLoad;
        }
    
        public void PauseGame()
        {
            pauseOverlay.SetActive(true);
            SoundManager.Instance.PlayEffect(_audioClipData.GetAudioClip(2),0.3f);
            Time.timeScale = 0;
            _paused = true;
        }

        public void ResumeGame()
        {
            pauseOverlay.SetActive(false);
            Time.timeScale = 1;
            _paused = false;
        }
        

        private void Start()
        {
            resumeButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                ResumeGame();
            });
            restartButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                SceneManager.LoadSceneAsync(Enum.GetName(typeof (SceneCollection), SceneManager.GetActiveScene().buildIndex), LoadSceneMode.Single);
                ResumeGame();
            });
            menuButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                ResumeGame();
                LoadSceneManager.Instance.StartLoadingScene(SceneCollection.MainMenu);
                LoadGame();
            });
            _audioClipData = GetComponent<AudioClipData>();
        }
        public int GetSlot()
        {
            if (saveSlot == CurrectSlot.Slot1)
            {
                return 1;
            }
            else if (saveSlot == CurrectSlot.Slot2)
            {
                return 2;
            }
            else if (saveSlot == CurrectSlot.Slot3)
            {
                return 3;
            }
            else
            {
                return 1;
            }
        }
        //slot ใส่ตำแหน่งของ save ที่จะลบเรียงจาก 1ไป3
        public void DeleteSave(int slot)
        {
            if (slot == 1)
            {
                Appdata.Instance.sceneInSave1 = SceneCollection.Tutorial01;
                Appdata.Instance.playerPosition1 = new Vector3(0f,0f,0f);
            }
            else if (slot == 2)
            {
                Appdata.Instance.sceneInSave2 = SceneCollection.Tutorial01;
                Appdata.Instance.playerPosition2 = new Vector3(0f,0f,0f);
            }
            else if (slot == 3)
            {
                Appdata.Instance.sceneInSave3 = SceneCollection.Tutorial01;
                Appdata.Instance.playerPosition3 = new Vector3(0f,0f,0f);
            }
            if (!IsSaveFile())
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Game_Save");
            }
            if (!Directory.Exists(Application.persistentDataPath + "/Game_Save/Player_Data"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Game_Save/Player_Data");
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/Game_Save/Player_Data/Player_save.txt");
            var json = JsonUtility.ToJson(playerData);
            bf.Serialize(file, json);
            file.Close();
            Debug.Log("Create Save File Done");
        }
    
    
    }
}
