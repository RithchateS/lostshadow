using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyBounds : MonoBehaviour
{
    private static GameObject Instance;
    private void Awake() {
        DontDestroyOnLoad(this); 
        if (Instance == null) {
                Instance = GameObject.FindWithTag("DontDestroy"); 
        }
        else {
            Destroy(gameObject);
        }
    }
}
