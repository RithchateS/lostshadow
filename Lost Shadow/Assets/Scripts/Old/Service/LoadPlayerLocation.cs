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
            GameObject.FindGameObjectWithTag("Player").transform.position = Appdata.Instance.playerPosition1;
        }

        if (GameSaveManager.Instance.GetSlot() == 2)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = Appdata.Instance.playerPosition2;
        }

        if (GameSaveManager.Instance.GetSlot() == 3)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = Appdata.Instance.playerPosition3;
        }
    }
}
