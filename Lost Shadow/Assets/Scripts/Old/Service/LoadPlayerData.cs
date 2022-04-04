using Data;
using Manager;
using Old.Manager;
using UnityEngine;

public class LoadPlayerData : MonoBehaviour
{
    
    private void Start()
    {
        if (!GameSaveManager.Instance.IsSaveFile())
        {
            Debug.Log("YEs");
            GameSaveManager.Instance.InitialSave();
        }
        else
        {
            Debug.Log("No");
            GameSaveManager.Instance.LoadGame();
        }
        Appdata.Instance.CheckPlayerData();
        SoundManager.Instance.PlayMusic(GetComponent<AudioClipData>().GetAudioClip(0),0.3f);
    }
    
    
}
