using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShadowShift : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex != 3){
            SceneManager.LoadScene(3);
        }
        else {
            SceneManager.LoadScene(2);
        }
    }
}
