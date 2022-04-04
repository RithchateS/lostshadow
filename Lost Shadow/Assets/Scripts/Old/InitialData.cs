using Manager;
using UnityEngine;

public class InitialData : MonoBehaviour
{
    public static InitialData Instance { get; private set; }
    [SerializeField] private GameObject LoadingCanvas ;
    [SerializeField] private Setting defaultSetting;
    [SerializeField] private SceneCollection startScene;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        SettingManager.Instance.CheckSettingData(defaultSetting);
        LoadSceneManager.Instance.SetLoadingCanvas(LoadingCanvas);
        LoadSceneManager.Instance.currentScene = SceneCollection.Persistant;
        LoadSceneManager.Instance.StartLoadingScene(startScene);
        Appdata.Instance.currentScene = startScene;
        SoundManager.Instance.Start();
        //SoundManager.Instance.Hello();
    }

}
