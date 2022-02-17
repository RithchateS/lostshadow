using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject camInstance;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (camInstance == null) {
            camInstance = GameObject.FindWithTag("MainCamera");
        } else {
            Destroy(gameObject);
        }
    }
}
