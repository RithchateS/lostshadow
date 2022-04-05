using Old.Manager;
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
    
    public SceneCollection currentScene;
    public SceneCollection sceneToLoad;
    public int currentChapter;
    // Save 1
    public Vector3 playerPosition1;
    public SceneCollection sceneInSave1;
    public bool isUsed1 = false;
    public int chapterNum1 = 0;
    // Save 2
    public Vector3 playerPosition2;
    public SceneCollection sceneInSave2;
    public bool isUsed2 = false;
    public int chapterNum2 = 0;
    // Save 3
    public Vector3 playerPosition3;
    public SceneCollection sceneInSave3;
    public bool isUsed3 = false;
    public int chapterNum3 = 0;

    public void CheckPlayerData()
    {
        if (!isUsed1)
        {
            sceneInSave1 = SceneCollection.Tutorial01;
        }
        if (!isUsed2)
        {
            sceneInSave2 = SceneCollection.Tutorial01;
        }
        if (!isUsed3)
        {
            sceneInSave3 = SceneCollection.Tutorial01;
        }
        
        // Already Play
        
        if (isUsed1)
        {
            playerPosition1 = GameSaveManager.Instance.playerData.playerPosition1;
            sceneInSave1 = GameSaveManager.Instance.playerData.sceneInSave1;
            sceneToLoad = sceneInSave1;
        }
        if (isUsed2)
        {
            playerPosition2 = GameSaveManager.Instance.playerData.playerPosition2;
            sceneInSave2 = GameSaveManager.Instance.playerData.sceneInSave2;
            sceneToLoad = sceneInSave2;
        }
        if (isUsed3)
        {
            playerPosition3 = GameSaveManager.Instance.playerData.playerPosition3;
            sceneInSave3 = GameSaveManager.Instance.playerData.sceneInSave3;
            sceneToLoad = sceneInSave3;
        }
    }
    
}
