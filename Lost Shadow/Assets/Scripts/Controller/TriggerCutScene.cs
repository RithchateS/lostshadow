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

        public void StartCrossFade()
        {
            transition.SetTrigger("Start");
        }

        IEnumerator ToLightForest(GameObject player)
        {
            StartCrossFade();
            yield return new WaitForSeconds(1f);
            LoadSceneManager.Instance.DestroyOnLoad();
            GameSaveManager.Instance.GoNextScene(4,4);
        }
        
    }
}