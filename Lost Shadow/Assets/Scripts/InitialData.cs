using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class InitialData : MonoBehaviour
{
    public static InitialData Instance { get; private set; }
    [SerializeField] private GameObject LoadingCanvas ;
    [SerializeField] private SceneCollection StartScene;
    [SerializeField] private Setting defaultSetting;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SettingManager.Instance.CheckSettingData(defaultSetting);
        LoadSceneManager.Instance.SetLoadingCanvas(LoadingCanvas);
        LoadSceneManager.Instance.currentScene = SceneCollection.Persistant;
        LoadSceneManager.Instance.StartLoadingScene(StartScene);
    }

}
