using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameSaveManager : Singleton<GameSaveManager>
{
    public Appdata playerData;
    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/Game_Save");
    }
    
    public void SaveGame()
    {
        if (!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Game_Save");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/Game_Save/Player_Data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Game_Save/Player_Data");
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Game_Save/Player_Data/Player_save.txt");
        var json = JsonUtility.ToJson(playerData);
        bf.Serialize(file, json);
        file.Close();
    }

    public void LoadGame()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Game_Save/Player_Data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Game_Save/Player_Data");
        }
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/Game_Save/Player_Data/Player_save.txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "Game_Save/Player_Data/Player_save.txt",FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file),playerData);
            file.Close();
        }
    }

    public void InitialGameSave()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Game_Save/Player_Data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Game_Save/Player_Data");
        }
    }
}
