using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShadowShift : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if ((currentSceneIndex % 2) == 0){
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else {
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
    }
}
