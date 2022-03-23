using System.Collections;
using System.Collections.Generic;
using Old.Manager;
using UnityEngine;

public class LoadPlayerLocation : MonoBehaviour
{
    void start()
    {
        if (GameSaveManager.Instance.GetSlot() == 1)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = Appdata.Instance.PlayerPosition1;
        }

        if (GameSaveManager.Instance.GetSlot() == 2)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = Appdata.Instance.PlayerPosition2;
        }

        if (GameSaveManager.Instance.GetSlot() == 3)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = Appdata.Instance.PlayerPosition3;
        }
    }
}
