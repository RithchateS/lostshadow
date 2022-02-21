using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [SerializeField] string linkedScene;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;
    GameObject _player;
    public Animator transition;
    private GameObject _camera;

    public string GetLinkedScene() {
        return linkedScene;
    }

    private void Start() {
        _player = GameObject.FindWithTag("Player");
        if (_player == null) {
            Instantiate(playerPrefab, transform.position, transform.rotation);
        }

        _camera = GameObject.FindWithTag("MainCamera");
        if (_camera == null)
        {
            Instantiate(cameraPrefab, transform.position, transform.rotation);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            ExitLevel();
        }
    }

    public void ExitLevel()
    {
        StartCoroutine(LoadLevel(1));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        Destroy(GameObject.FindWithTag("DontDestroy"));
        SceneManager.LoadScene(1);
    }
}
