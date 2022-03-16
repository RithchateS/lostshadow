using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneController : MonoBehaviour
{
    [SerializeField] private GameObject LoadingCanvas;

    void Awake()
    {
        LoadSceneManager.Instance.SetLoadingCanvas(LoadingCanvas);
        LoadSceneManager.Instance.currentScene = SceneCollection.LoadScene;
        LoadSceneManager.Instance.StartLoadingScene(Appdata.Instance.sceneToLoad);
    }
}
