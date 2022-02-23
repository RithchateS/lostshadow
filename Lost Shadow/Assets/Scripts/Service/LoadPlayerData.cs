using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerData : MonoBehaviour
{
    
    private void Start()
    {
        if (!GameSaveManager.Instance.IsSaveFile())
        {
            GameSaveManager.Instance.InitialSave();
        }
        else
        {
            GameSaveManager.Instance.LoadGame();
        }
        Appdata.Instance.CheckPlayerData();
    }
    
    
}
