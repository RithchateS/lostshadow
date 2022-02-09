using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneController : MonoBehaviour
{
    [SerializeField] private SceneCollection StartScene;
    [SerializeField] private GameObject LoadingCanvas;
    
    void Awake()
    {
        LoadSceneManager.Instance.SetLoadingCanvas(LoadingCanvas);
        LoadSceneManager.Instance.currentScene = SceneCollection.LoadScene;
        LoadSceneManager.Instance.StartLoadingScene(StartScene);
    }
}
