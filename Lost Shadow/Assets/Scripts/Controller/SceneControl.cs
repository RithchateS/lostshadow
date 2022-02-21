using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
        _player = GameObject.FindWithTag("Player");
        startPos = GameObject.Find("StartPos").transform;
        if (_player == null) {
            Instantiate(playerPrefab, startPos.position, startPos.rotation);
        }

        _camera = GameObject.FindWithTag("MainCamera");
        if (_camera == null)
        {
            Instantiate(cameraPrefab, startPos.position, startPos.rotation);
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
