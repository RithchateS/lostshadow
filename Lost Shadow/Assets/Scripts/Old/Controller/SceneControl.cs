using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [SerializeField] string linkedScene;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] Transform startPos;
    GameObject _player;
    public Animator transition;
    private GameObject _camera;

    public string GetLinkedScene() {
        return linkedScene;
    }

    private void Start() {
        if (SceneManager.GetActiveScene().name == SceneCollection.Prolouge1_Shadow.ToString())
        {
            StartCoroutine(LoadCutSccene());
        }
        _player = GameObject.FindWithTag("Player");
        startPos = GameObject.Find("StartPos").transform;
        if (_player == null) {
            Instantiate(playerPrefab, startPos.position, startPos.rotation);
            GameSaveManager.Instance.SaveGame();
        }

        _camera = GameObject.FindWithTag("MainCamera");
        if (_camera == null)
        {
            Instantiate(cameraPrefab, startPos.position, startPos.rotation);
        }
    }
    

    IEnumerator LoadCutSccene()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(13f);
        transition.SetTrigger("End");
    }
}
