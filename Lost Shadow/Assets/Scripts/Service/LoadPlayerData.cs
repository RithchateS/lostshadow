using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerData : MonoBehaviour
{
    public static LoadPlayerData Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("HaveSave") == 0)
        {
            Debug.Log("YEs");
            GameSaveManager.Instance.InitialSave();
        }
        else if (PlayerPrefs.GetInt("HaveSave") == 1)
        {
            Debug.Log("No");
            //GameSaveManager.Instance.LoadGame();
            GameSaveManager.Instance.InitialSave();
        }
        Appdata.Instance.CheckPlayerData();
    }
    
}
