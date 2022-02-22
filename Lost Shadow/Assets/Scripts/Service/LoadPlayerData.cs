using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerData : MonoBehaviour
{
    
    private void Start()
    {
        if (PlayerPrefs.GetInt("HaveSave") == 0)
        {
            GameSaveManager.Instance.InitialSave();
        }
        else if (PlayerPrefs.GetInt("HaveSave") == 1)
        {
            GameSaveManager.Instance.LoadGame();
        }
        Appdata.Instance.CheckPlayerData();
    }
    
    
}
