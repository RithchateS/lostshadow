using System.Collections;
using System.Collections.Generic;
using Controller;
using Data;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guard1AI : MonoBehaviour
{
    #region Variables
    [SerializeField] float patrolSpeed = 5f;
    [SerializeField] float chaseSpeed = 10f;
    [SerializeField] float holdTime = 2f;
    [SerializeField] float viewRange = 5f;
    [SerializeField] float guardClimbSpeed = 5f;
    [SerializeField] float climbCooldown = 20f;
    private string _guardState = "Patrol";
    private int _viewDir = 1; //Use 1 for right, -1 for left
    private Animator _guardAnimator;
    private Rigidbody2D _guardRb2D;
    private GameObject _player;
    private float _distXtoPlayer;
    private float _distYtoPlayer;
    private float _curHoldTime;
    private float _gravityScaleAtStart;
    private float _timeSinceLastClimb;
    private float _timeSinceExitLadder;
    private float _timeSinceLastGrunt;
    private PlayerController _playerController;
    [SerializeField] GameObject guardVision;
    [SerializeField] private float soundRange = 5f;
    private Vector3 _location;
    private AudioClipData _audioClipData;
    private static readonly int IsPatrolling = Animator.StringToHash("isPatrolling");

    #endregion
    
    #region Utilities
    void FlipGuardFacing() {
        transform.localScale = new Vector2 (-transform.localScale.x, 1f);
    }

    void CheckForPlayer()
    {
        if (!_playerController.GetIsHiding())
        {
            if (_viewDir == 1) {
                if (Mathf.Abs(_distXtoPlayer) < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }
            else if (_viewDir == -1) {
                if (Mathf.Abs(_distXtoPlayer) < viewRange && _distXtoPlayer < Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
            }
        }
    }
    #endregion
    private void Start() {
        _guardAnimator = GetComponent<Animator>();
        _guardRb2D = GetComponent<Rigidbody2D>();
        _audioClipData = GetComponent<AudioClipData>();
        _gravityScaleAtStart = 4f;
        _timeSinceLastGrunt = 16f;
    }

    void Update() {
        _timeSinceLastClimb += Time.deltaTime;
        _timeSinceExitLadder += Time.deltaTime;
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        var position = _player.transform.position;
        var position1 = transform.position;
        _distXtoPlayer = position.x - position1.x;
        _distYtoPlayer = position.y - position1.y;

        GuardSound();
        if (_guardState == "Patrol") {
            if (_timeSinceExitLadder > 1f) {
                _guardRb2D.gravityScale = 0f;
            }
            _guardRb2D.velocity = new Vector2 (_viewDir * patrolSpeed, _guardRb2D.velocity.y);
            _guardAnimator.SetBool(IsPatrolling, true);
            CheckForPlayer();
        }
        else if (_guardState == "Chase") {
            _guardRb2D.velocity = new Vector2 (_viewDir * chaseSpeed, _guardRb2D.velocity.y);
            _guardAnimator.SetBool(IsPatrolling, true);
            _guardRb2D.gravityScale = _gravityScaleAtStart;
            
            if (_viewDir == 1) {
                if (Mathf.Abs(_distXtoPlayer) < viewRange && _distXtoPlayer > Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
                    _guardState = "Chase";
                }
                else {
                    _guardState = "Patrol";
                }
            }
            else if (_viewDir == -1) {
                if (Mathf.Abs(_distXtoPlayer) < viewRange && _distXtoPlayer < Mathf.Epsilon && Mathf.Abs(_distYtoPlayer) < 1) {
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
            
            CheckForPlayer();

            if (_curHoldTime > holdTime) {
                _viewDir = -_viewDir;
                FlipGuardFacing();
                _guardState = "Patrol";
            }
        }
        else if (_guardState == "HoldForLadder") {
            _guardRb2D.velocity = new Vector2 (0f, 0f);
            _curHoldTime += Time.deltaTime;

            CheckForPlayer();

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

            CheckForPlayer();

            if (_curHoldTime >= holdTime) {
                _viewDir = -_viewDir;
                _curHoldTime = 0f;
                _guardState = "ClimbLadder";
                _timeSinceLastClimb = 0f;
            }
        }
        else if (_guardState == "ClimbLadder") {
            if (!_guardRb2D.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {
                _guardRb2D.gravityScale = _gravityScaleAtStart;
                _guardState = "Patrol";
                FlipGuardFacing();
                _timeSinceExitLadder = 0f;
                return;
            }
            Vector2 climbVelocity = new Vector2 (0f, guardClimbSpeed);
            _guardRb2D.velocity = climbVelocity;
            _guardRb2D.gravityScale = 0f;
        }
        else if (_guardState == "HoldForLadderDown") {
            _guardRb2D.velocity = new Vector2 (0f, 0f);
            _curHoldTime += Time.deltaTime;

            CheckForPlayer();

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

            CheckForPlayer();

            if (_curHoldTime >= holdTime) {
                _viewDir = -_viewDir;
                _curHoldTime = 0f;
                _guardState = "ClimbLadderDown";
                _timeSinceLastClimb = 0f;
            }
        }
        else if (_guardState == "ClimbLadderDown") {
            Vector2 climbVelocity = new Vector2 (0f, -guardClimbSpeed);
            _guardRb2D.velocity = climbVelocity;
            _guardRb2D.gravityScale = 0f;
        }
    }

    #region Triggers and Collision
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("GuardStop")) {
            _guardState = "Hold";
            _curHoldTime = 0f;
            _guardAnimator.SetBool(IsPatrolling, false);
        }
        else if (other.gameObject.CompareTag("GuardLadderBot"))
        {
            if (_guardState != "ClimbLadderDown") return;
            _guardRb2D.gravityScale = _gravityScaleAtStart;
            _guardState = "Patrol";
            FlipGuardFacing();
        }
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("GuardLadderBot"))
        {
            if (!(transform.position.x < other.gameObject.transform.position.x + 0.02) ||
                !(transform.position.x > other.gameObject.transform.position.x - 0.02)) return;
            if (_guardState != "Patrol" || !(_timeSinceLastClimb > climbCooldown)) return;
            _guardState = "HoldForLadder";
            _curHoldTime = 0f;
            _guardAnimator.SetBool(IsPatrolling, false);
        }
        else if (other.gameObject.CompareTag("GuardLadderTop")) {
            if (transform.position.x < other.gameObject.transform.position.x + 0.02 && transform.position.x > other.gameObject.transform.position.x - 0.02) {
                if (_guardState == "Patrol" && _timeSinceLastClimb > climbCooldown) {
                    _guardState = "HoldForLadderDown";
                    _curHoldTime = 0f;
                    _guardAnimator.SetBool(IsPatrolling, false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && !_playerController.GetIsHiding())
        {
            _guardRb2D.velocity = new Vector2(0, 0);
            _playerController.ModifyAlive(false);
        }
    }

    private void GuardSound()
    {
        if (Mathf.Abs(_distXtoPlayer) < soundRange && Mathf.Abs(_distYtoPlayer) < soundRange)
        {
            _timeSinceLastGrunt += Time.deltaTime;
            
            if (_timeSinceLastGrunt >= Random.Range(5f, 15f))
            {
                _timeSinceLastGrunt = 0;
                SoundManager.Instance.RandomSoundEffect(_audioClipData.GetAudioClipGroup(0,1),0.3f);
                Debug.Log("Guard Grunt");
            }
        }

        
    }
    #endregion
    #region Utils
    private void ShowRange() {
        var transform1 = transform;
        var position = transform1.position;
        _location = new Vector3(position.x + (viewRange / 2),position.y, position.z);
        var _guardVision = Instantiate(guardVision, _location, Quaternion.identity, this.GetComponent<Transform>());
        _guardVision.transform.localScale = new Vector3(viewRange, 2, 1);
    }
    #endregion
}
