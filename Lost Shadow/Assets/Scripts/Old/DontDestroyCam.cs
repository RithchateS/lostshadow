using UnityEngine;

public class DontDestroyCam : MonoBehaviour
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
