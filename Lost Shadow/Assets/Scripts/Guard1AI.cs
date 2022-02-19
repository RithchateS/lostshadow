using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard1AI : MonoBehaviour
{
    [SerializeField] float patrolSpeed = 5f;
    [SerializeField] float chaseSpeed = 10f;
    [SerializeField] float holdTime = 2f;
    [SerializeField] float viewRange = 5f;
    private string _guardState = "Patrol";
    private int _viewDir = 1; //Use 1 for right, -1 for left
    private Animator _guardAnimator;
    private Rigidbody2D _guardRb2D;
    private GameObject _player;
    private float _distXtoPlayer;
    private float _distYtoPlayer;
    private float _curHoldTime;

    void FlipGuardFacing() {
        transform.localScale = new Vector2 (-transform.localScale.x, 1f);
    }

    private void Start() {
        _guardAnimator = GetComponent<Animator>();
        _guardRb2D = GetComponent<Rigidbody2D>();
        _player = GameObject.FindWithTag("Player");
    }

    void Update() {
        _distXtoPlayer = _player.transform.position.x - transform.position.x;
        _distYtoPlayer = _player.transform.position.y - transform.position.y;

        if (_guardState == "Patrol") {
            _guardRb2D.velocity = new Vector2 (_viewDir * patrolSpeed, 0f);
            _guardAnimator.SetBool("isPatrolling", true);
            
            if (_viewDir == 1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 3) {
                    _guardState = "Chase";
                }
            }
            
        }
        else if (_guardState == "Chase") {
            _guardRb2D.velocity = new Vector2 (_viewDir * chaseSpeed, 0f);
            _guardAnimator.SetBool("isPatrolling", true);
            
            if (_viewDir == 1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 3) {
                    _guardState = "Chase";
                }
                else {
                    _guardState = "Patrol";
                }
            }
        }
        else if (_guardState == "Hold") {
            _guardRb2D.velocity = new Vector2 (0f, 0f);
            _curHoldTime += Time.deltaTime;
            
            if (_viewDir == 1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 3) {
                    _guardState = "Chase";
                }
            }

            Debug.Log(_curHoldTime);

            if (_curHoldTime > holdTime) {
                _viewDir = -_viewDir;
                FlipGuardFacing();
                _guardState = "Patrol";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "GuardStop") {
            _guardState = "Hold";
            _curHoldTime = 0f;
            _guardAnimator.SetBool("isPatrolling", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            Destroy(other.gameObject);
        }
    }
}
