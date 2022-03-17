using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float guardClimbSpeed = 5f;
    private float gravityScaleAtStart = 4f;
    private float timeSinceLastClimb = 0f;
    [SerializeField] float climbCooldown = 20f;
    private float timeSinceExitLadder = 0f;


    void FlipGuardFacing() {
        transform.localScale = new Vector2 (-transform.localScale.x, 1f);
    }

    private void Start() {
        _guardAnimator = GetComponent<Animator>();
        _guardRb2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        timeSinceLastClimb += Time.deltaTime;
        timeSinceExitLadder += Time.deltaTime;
        _player = GameObject.FindWithTag("Player");
        _distXtoPlayer = _player.transform.position.x - transform.position.x;
        _distYtoPlayer = _player.transform.position.y - transform.position.y;

        if (_guardState == "Patrol") {
            if (timeSinceExitLadder > 1f) {
                _guardRb2D.gravityScale = 0f;
            }
            _guardRb2D.velocity = new Vector2 (_viewDir * patrolSpeed, _guardRb2D.velocity.y);
            _guardAnimator.SetBool("isPatrolling", true);
            
            if (_viewDir == 1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }
            else if (_viewDir == -1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer < Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }
            
        }
        else if (_guardState == "Chase") {
            _guardRb2D.velocity = new Vector2 (_viewDir * chaseSpeed, _guardRb2D.velocity.y);
            _guardAnimator.SetBool("isPatrolling", true);
            
            if (_viewDir == 1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
                else {
                    _guardState = "Patrol";
                }
            }
            else if (_viewDir == -1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer < Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
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
                if (_distXtoPlayer < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }
            else if (_viewDir == -1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer < Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }

            if (_curHoldTime > holdTime) {
                _viewDir = -_viewDir;
                FlipGuardFacing();
                _guardState = "Patrol";
            }
        }
        else if (_guardState == "HoldForLadder") {
            _guardRb2D.velocity = new Vector2 (0f, 0f);
            _curHoldTime += Time.deltaTime;

            if (_viewDir == 1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }
            else if (_viewDir == -1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer < Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }

            if (_curHoldTime >= holdTime) {
                _viewDir = -_viewDir;
                FlipGuardFacing();
                _curHoldTime = 0f;
                _guardState = "HoldForLadder2";
            }
        }
        else if (_guardState == "HoldForLadder2") {
            _guardRb2D.velocity = new Vector2 (0f, 0f);
            _curHoldTime += Time.deltaTime;

            if (_viewDir == 1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }
            else if (_viewDir == -1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer < Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }

            if (_curHoldTime >= holdTime) {
                _viewDir = -_viewDir;
                _curHoldTime = 0f;
                _guardState = "ClimbLadder";
                timeSinceLastClimb = 0f;
            }
        }
        else if (_guardState == "ClimbLadder") {
            if (!_guardRb2D.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {
                _guardRb2D.gravityScale = gravityScaleAtStart;
                _guardState = "Patrol";
                FlipGuardFacing();
                timeSinceExitLadder = 0f;
                return;
            }
            Vector2 climbVelocity = new Vector2 (0f, guardClimbSpeed);
            _guardRb2D.velocity = climbVelocity;
            _guardRb2D.gravityScale = 0f;
        }
        else if (_guardState == "HoldForLadderDown") {
            _guardRb2D.velocity = new Vector2 (0f, 0f);
            _curHoldTime += Time.deltaTime;

            if (_viewDir == 1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }
            else if (_viewDir == -1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer < Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }

            if (_curHoldTime >= holdTime) {
                _viewDir = -_viewDir;
                FlipGuardFacing();
                _curHoldTime = 0f;
                _guardState = "HoldForLadderDown2";
            }
        }
        else if (_guardState == "HoldForLadderDown2") {
            _guardRb2D.velocity = new Vector2 (0f, 0f);
            _curHoldTime += Time.deltaTime;

            if (_viewDir == 1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }
            else if (_viewDir == -1) {
                if (_distXtoPlayer < viewRange && _distXtoPlayer < Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }

            if (_curHoldTime >= holdTime) {
                _viewDir = -_viewDir;
                _curHoldTime = 0f;
                _guardState = "ClimbLadderDown";
                timeSinceLastClimb = 0f;
            }
        }
        else if (_guardState == "ClimbLadderDown") {
            Vector2 climbVelocity = new Vector2 (0f, -guardClimbSpeed);
            _guardRb2D.velocity = climbVelocity;
            _guardRb2D.gravityScale = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "GuardStop") {
            _guardState = "Hold";
            _curHoldTime = 0f;
            _guardAnimator.SetBool("isPatrolling", false);
        }
        if (other.gameObject.tag == "GuardLadderBot") {
            if (_guardState == "ClimbLadderDown") {
                _guardRb2D.gravityScale = gravityScaleAtStart;
                _guardState = "Patrol";
                FlipGuardFacing();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "GuardLadderBot") {
            if (transform.position.x < other.gameObject.transform.position.x + 0.02 && transform.position.x > other.gameObject.transform.position.x - 0.02) {
                if (_guardState == "Patrol" && timeSinceLastClimb > climbCooldown) {
                    _guardState = "HoldForLadder";
                    _curHoldTime = 0f;
                    _guardAnimator.SetBool("isPatrolling", false);
                }
            }
        }
        else if (other.gameObject.tag == "GuardLadderTop") {
            if (transform.position.x < other.gameObject.transform.position.x + 0.02 && transform.position.x > other.gameObject.transform.position.x - 0.02) {
                if (_guardState == "Patrol" && timeSinceLastClimb > climbCooldown) {
                    _guardState = "HoldForLadderDown";
                    _curHoldTime = 0f;
                    _guardAnimator.SetBool("isPatrolling", false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = GameObject.FindWithTag("StartPos").transform.position;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
