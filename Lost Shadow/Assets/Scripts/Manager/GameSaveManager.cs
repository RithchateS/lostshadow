using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSaveManager : MonoBehaviour
{
    public Appdata playerData;
    public static GameSaveManager Instance { get; private set; }
    private bool paused = false;
    [SerializeField] private CurrectSlot saveSlot;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject pauseOverlay;
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
        playerData = Appdata.Instance.GetComponent<Appdata>();
        DontDestroyOnLoad(this);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Appdata.Instance.CurrentScene == SceneCollection.MainMenu || Appdata.Instance.CurrentScene == SceneCollection.LoadScene) { return; }
            else
            {
                if (paused)
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
            Appdata.Instance.PlayerPosition1 = GameObject.FindGameObjectWithTag("Player").transform.position;
            Appdata.Instance.SceneInSave1 = Appdata.Instance.CurrentScene;
            Appdata.Instance.chapterNum1 = Appdata.Instance.currentChapter;
        }
        else if (saveSlot == CurrectSlot.Slot2)
        {
            Appdata.Instance.isUsed2 = true;
            Appdata.Instance.PlayerPosition2 = GameObject.FindGameObjectWithTag("Player").transform.position;
            Appdata.Instance.SceneInSave2 = Appdata.Instance.CurrentScene;
            Appdata.Instance.chapterNum2 = Appdata.Instance.currentChapter;
        }
        else if (saveSlot == CurrectSlot.Slot3)
        {
            Appdata.Instance.isUsed3 = true;
            Appdata.Instance.PlayerPosition3 = GameObject.FindGameObjectWithTag("Player").transform.position;
            Appdata.Instance.SceneInSave3 = Appdata.Instance.CurrentScene;
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
    public void GoNextScene(int NextLevel,int Chapter)
    {
        SceneManager.LoadScene("LoadScene");
        Appdata.Instance.currentChapter = Chapter;
        Appdata.Instance.SceneToLoad = (SceneCollection)NextLevel;
        Appdata.Instance.CurrentScene = Appdata.Instance.SceneToLoad;
    }
    public void GoMenuScene(int NextLevel)
    {
        SceneManager.LoadScene("LoadScene");
        Appdata.Instance.SceneToLoad = (SceneCollection)NextLevel;
        Appdata.Instance.CurrentScene = Appdata.Instance.SceneToLoad;
    }
    public void PauseGame()
    {
        pauseOverlay.SetActive(true);
        Time.timeScale = 0;
        paused = true;
    }

    public void ResumeGame()
    {
        pauseOverlay.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }

    private void Start()
    {
        resumeButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            ResumeGame();
        });
        menuButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            GoMenuScene(1);
            LoadGame();
        });
        exitButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            Application.Quit();
        });
    }
}
