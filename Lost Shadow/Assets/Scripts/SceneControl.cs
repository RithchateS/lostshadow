using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    [SerializeField] string linkedScene;
    [SerializeField] GameObject playerPrefab;
    GameObject _player;

    public string GetLinkedScene() {
        return linkedScene;
    }

    private void Start() {
        _player = GameObject.FindWithTag("Player");
        if (_player == null) {
            Instantiate(playerPrefab, transform.position, transform.rotation);
        }
    }
}
