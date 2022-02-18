using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{
    public Setting userValue = null ;
    public Setting defaultValue ;
    
    public Setting GetData{
        get{
            if (userValue == null)
            {
                return defaultValue;
            }
            else
            {
                return userValue;
            }
        }
        private set {}
    }

    public void SetUserData(Setting data){
        this.userValue = data ;
    }

    public void CheckSettingData(Setting datasetting)
    {
        string defaultSettingDataString = PlayerPrefs.GetString("DefaultSettingData", "");
        if (defaultSettingDataString.Equals("") || defaultSettingDataString != JsonUtility.ToJson(datasetting))
        { 
            PlayerPrefs.SetString("DefaultSettingData", JsonUtility.ToJson(datasetting));
        } 
        defaultValue = datasetting;
        
        string userSettingDataString = PlayerPrefs.GetString("UserSettingData", "");
        if (!userSettingDataString.Equals("") )
        {
            userValue = (Setting)JsonUtility.FromJson<Setting>(userSettingDataString);
        }
    }
}
