using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class Appdata : Singleton<Appdata>
{
    enum CurrectSlot
    {
          Slot1,
          Slot2,
          Slot3
    }
    // DefaultSave
    
    public SceneCollection CurrentScene;
    public SceneCollection SceneToLoad;
    // Save 1
    public Vector3 PlayerPosition1;
    public SceneCollection SceneInSave1;
    // Save 2
    public Vector3 PlayerPosition2;
    public SceneCollection SceneInSave2;
    // Save 3
    public Vector3 PlayerPosition3;
    public SceneCollection SceneInSave3;

    public void CheckPlayerData()
    {
        if (PlayerPosition1 != GameSaveManager.Instance.playerData.PlayerPosition1 ||
            SceneInSave1 != GameSaveManager.Instance.playerData.SceneInSave1)
        {
            PlayerPosition1 = GameSaveManager.Instance.playerData.PlayerPosition1;
            SceneInSave1 = GameSaveManager.Instance.playerData.SceneInSave1;
        }
        if (PlayerPosition2 != GameSaveManager.Instance.playerData.PlayerPosition2 ||
            SceneInSave2 != GameSaveManager.Instance.playerData.SceneInSave2)
        {
            PlayerPosition2 = GameSaveManager.Instance.playerData.PlayerPosition2;
            SceneInSave2 = GameSaveManager.Instance.playerData.SceneInSave2;
        }
        if (PlayerPosition3 != GameSaveManager.Instance.playerData.PlayerPosition3 ||
            SceneInSave3 != GameSaveManager.Instance.playerData.SceneInSave3)
        {
            PlayerPosition3 = GameSaveManager.Instance.playerData.PlayerPosition3;
            SceneInSave3 = GameSaveManager.Instance.playerData.SceneInSave3;
        }
    }
    
}
