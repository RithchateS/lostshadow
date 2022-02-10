using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialData : MonoBehaviour
{
    public static InitialData Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

}
