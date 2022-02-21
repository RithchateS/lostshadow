using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller
{
    public class TriggerCutScene : MonoBehaviour
    {
        [SerializeField] SceneCollection sceneName;
        [SerializeField] Animator transition;
        private SceneControl _sceneControl;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == GameObject.FindWithTag("Player"))
            {
                switch (sceneName)
                {
                    case SceneCollection.LightForest:
                    {
                        StartCoroutine(ToLightForest(other.gameObject));
                        break;
                    }
                    case SceneCollection.MainMenu:
                    {
                        break;
                    }
                }
                
            }
        }

        IEnumerator ToLightForest(GameObject player)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(sceneName.ToString());
            player.gameObject.transform.position = GameObject.Find("StartPos").transform.position;
            Debug.Log(GameObject.Find("StartPos").transform.position);
            Debug.Log(player.gameObject.transform.position);
        }
        
        
    }
}