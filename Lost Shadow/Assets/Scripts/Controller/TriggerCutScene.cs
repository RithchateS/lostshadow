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
                        StartCoroutine(ToNextLevel(4));
                        break;
                    }
                    case SceneCollection.ShadowForest:
                    {
                        StartCoroutine(ToNextLevel(5));
                        break;
                    }
                    case SceneCollection.LightVillage:
                    {
                        StartCoroutine(ToNextLevel(6));
                        break;
                    }
                    case SceneCollection.ShadowVillage:
                    {
                        StartCoroutine(ToNextLevel(7));
                        break;
                    }
                    case SceneCollection.MainMenu:
                    {
                        break;
                    }
                }
                
            }
        }

        private void StartCrossFade()
        {
            transition.SetTrigger("Start");
        }

        IEnumerator ToNextLevel(int sceneId)
        {
            StartCrossFade();
            yield return new WaitForSeconds(1f);
            LoadSceneManager.Instance.DestroyOnLoad();
            GameSaveManager.Instance.GoNextScene(sceneId,4);
        }
        
    }
}