using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
enum CurrectSlot
{
    Slot1, // 0
    Slot2, // 1
    Slot3 // 2
}

public class Appdata : Singleton<Appdata>
{
    
    public SceneCollection CurrentScene;
    public SceneCollection SceneToLoad;
    public int currentChapter;
    // Save 1
    public Vector3 PlayerPosition1;
    public SceneCollection SceneInSave1;
    public bool isUsed1 = false;
    public int chapterNum1 = 0;
    // Save 2
    public Vector3 PlayerPosition2;
    public SceneCollection SceneInSave2;
    public bool isUsed2 = false;
    public int chapterNum2 = 0;
    // Save 3
    public Vector3 PlayerPosition3;
    public SceneCollection SceneInSave3;
    public bool isUsed3 = false;
    public int chapterNum3 = 0;

    public void CheckPlayerData()
    {
        if (!isUsed1)
        {
            SceneInSave1 = SceneCollection.Prolouge1_Shadow;
        }
        if (!isUsed2)
        {
            SceneInSave2 = SceneCollection.Prolouge1_Shadow;
        }
        if (!isUsed3)
        {
            SceneInSave3 = SceneCollection.Prolouge1_Shadow;
        }
        
        // Already Play
        
        if (isUsed1)
        {
            PlayerPosition1 = GameSaveManager.Instance.playerData.PlayerPosition1;
            SceneInSave1 = GameSaveManager.Instance.playerData.SceneInSave1;
            SceneToLoad = SceneInSave1;
        }
        if (isUsed2)
        {
            PlayerPosition2 = GameSaveManager.Instance.playerData.PlayerPosition2;
            SceneInSave2 = GameSaveManager.Instance.playerData.SceneInSave2;
            SceneToLoad = SceneInSave2;
        }
        if (isUsed3)
        {
            PlayerPosition3 = GameSaveManager.Instance.playerData.PlayerPosition3;
            SceneInSave3 = GameSaveManager.Instance.playerData.SceneInSave3;
            SceneToLoad = SceneInSave3;
        }
    }
    
}
