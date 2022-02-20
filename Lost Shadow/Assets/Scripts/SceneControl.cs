using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    [SerializeField] string linkedScene;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;
    GameObject _player;
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
}
