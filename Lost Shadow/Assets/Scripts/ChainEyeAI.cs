using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChainEyeAI : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _normalize;
    private Quaternion _rotation;
    [SerializeField] float eyeChaseSpeed;
    [SerializeField] float eyeBackSpeed;
    [SerializeField] float eyeRange;
    private GameObject _player;
    private PlayerController _playerController;
    [SerializeField] GameObject eyeVision;
    
    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        if (_player == null) {
            _player = GameObject.FindWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
        }
        else {
            _rotation = Quaternion.LookRotation(_player.transform.position - transform.position, transform.TransformDirection(Vector3.up));
            transform.rotation = new Quaternion(0, 0, _rotation.z, _rotation.w);
            if (checkPlayerInRange()) {
                NormalizeDirection();
                transform.position += _normalize * eyeChaseSpeed * Time.deltaTime;
            }
            else {
                NormalizeReturn();
                transform.position += _normalize * eyeBackSpeed * Time.deltaTime;
            }
        }
        
    }

    bool checkPlayerInRange() {
        return Vector3.Distance(_player.transform.position, _startPos) < eyeRange;
    }

    void NormalizeDirection() {
        _normalize = (_player.transform.position - transform.position).normalized;
    }
    void NormalizeReturn() {
        _normalize = (_startPos - transform.position).normalized;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerController.ModifyAlive(false);
        }
    }

    private void showRange() {
        GameObject _eyeVision = Instantiate(eyeVision, this.transform.position, Quaternion.identity);
        _eyeVision.transform.localScale = new Vector3(2*eyeRange, 2*eyeRange, 1);
    }
}
