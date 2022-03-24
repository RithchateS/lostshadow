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
    }
    
    
}
