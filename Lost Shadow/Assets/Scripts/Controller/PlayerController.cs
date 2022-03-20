using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Old.Controller
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        Vector2 _moveInput;
        Rigidbody2D _myRigidbody;
        Animator _myAnimator;
        BoxCollider2D _myBodyCollider;
        BoxCollider2D _myFeetCollider;
        private GameObject _feet;
        float _gravityScaleAtStart;
        bool isControllable;
        
        [SerializeField] float climbSpeed = 5f;
        [SerializeField] bool isGrounded;
        [SerializeField] private bool isClimbable;
        [SerializeField] private bool isClimbing;
        #endregion
        
        
        void Start()
        {
            _feet = GameObject.FindWithTag("Feet");
            _myRigidbody = GetComponent<Rigidbody2D>();
            _myAnimator = GetComponent<Animator>();
            _myBodyCollider = GetComponent<BoxCollider2D>();
            _myFeetCollider = _feet.GetComponent<BoxCollider2D>();
            _gravityScaleAtStart = _myRigidbody.gravityScale;
            isJumpAble = true;
            Wakeup();

        }

        void Update()
        {
            if (isControllable != false)
            {
                Run();
                CalculateRun();
                FlipSprite();
                CheckGround();
                CheckLadder();
                ClimbLadder();
            }

        }

        #region InputSystem
        #region Run
        [Header("RUN")] 
        [SerializeField] private float runSpeed = 7f;
        [SerializeField] private float acceleration = 50f;
        private float _currentHorizontalSpeed;
        [SerializeField] private float deAcceleration = 30f;
        
        void OnMove(InputValue value) {
            _moveInput = value.Get<Vector2>();
        }
        void Run() 
        {
            Vector2 playerVelocity = new Vector2 (_currentHorizontalSpeed, _myRigidbody.velocity.y);
            _myRigidbody.velocity = playerVelocity;
        
            bool playerHasHorizontalSpeed = Mathf.Abs(_currentHorizontalSpeed) > Mathf.Epsilon;
            _myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        }
        private void CalculateRun() {
            if (_moveInput.x != 0) {
                _currentHorizontalSpeed += _moveInput.x * acceleration * Time.deltaTime;
                        
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -runSpeed, runSpeed);
                        
            }
            else {
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, deAcceleration * Time.deltaTime);
            }
                    
        }
        #endregion
        #region Jump
        [Header("JUMP")]
        [SerializeField] float jumpSpeed = 5f;
        [Range(0f,0.5f)] [SerializeField] float lateJumpTime;
        [SerializeField] bool isJumpAble;
        
        void OnJump(InputValue value)
                {
                    if (value.isPressed)
                    {
                        isClimbing = false;
                    }
                    if (value.isPressed && isGrounded && isJumpAble)
                    {
                        StartCoroutine(IsJumpAble());
                        _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, 0f);
                        _myRigidbody.velocity += new Vector2(0f, jumpSpeed);
                    }
                }
        IEnumerator IsJumpAble()
                {
                    isJumpAble = false;
                    yield return new WaitForSeconds(lateJumpTime);
                    isJumpAble = true;
                }
        #endregion
        #region ShadowShift

        void OnShadowShift(InputValue value) {
                    if (!_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
                        return;
                    }
                
                    if (value.isPressed && _moveInput == new Vector2(0, 0))
                    {
                        if (_myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Shifter")) && !_myBodyCollider.IsTouchingLayers(LayerMask.GetMask("DeadZone")))
                        {
                            StartCoroutine(ShadowShift()); 
                        }
                    }
        }
        IEnumerator ShadowShift()
                 {
                     int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                     _myAnimator.SetTrigger("isShadowShift");
                     yield return new WaitForSeconds(0.8f);
                     if ((currentSceneIndex % 2) == 0){
                         SceneManager.LoadScene(currentSceneIndex + 1);
                     }
                     else {
                         SceneManager.LoadScene(currentSceneIndex - 1);
                     }
                 }
        
        #endregion
        #region Climb
                void ClimbLadder()
                {
                    if (isClimbable)
                    {
                        if (_moveInput.y != 0)
                        {
                            isClimbing = true;
                        }
        
                        if (_moveInput.x != 0)
                        {
                            isClimbing = false;
                        }
        
                        if (isClimbing)
                        {
                            isGrounded = true;
                            Vector2 climbVelocity = new Vector2 (0, _moveInput.y * climbSpeed);
                            _myRigidbody.velocity = climbVelocity;
                            _myRigidbody.gravityScale = 0f;
                            bool playerHasVerticalSpeed = Mathf.Abs(_myRigidbody.velocity.y) > Mathf.Epsilon;
                            _myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
                        }
                        else
                        {
                            _myRigidbody.gravityScale = _gravityScaleAtStart;
                            _myAnimator.SetBool("isClimbing", false);
                        }
                    }
                    else
                    {
                        _myRigidbody.gravityScale = _gravityScaleAtStart;
                        _myAnimator.SetBool("isClimbing", false);
                    }
                    
                }
                #endregion
        #region Animation
        
        public void Wakeup()
        {
            StartCoroutine(PauseMovement());
            _myAnimator.Play("Wakeup");
        }
        
        IEnumerator PauseMovement()
        {

            isControllable = false;

            //Setting Time Freezeeee Here
            yield return new WaitForSeconds(5);
            
            isControllable = true;
            
        }
                

        #endregion
                
        #endregion
        
        #region Passive
        void FlipSprite() {
            bool playerHasHorizontalSpeed = Mathf.Abs(_myRigidbody.velocity.x) > Mathf.Epsilon;
        
            if (playerHasHorizontalSpeed) {
                transform.localScale = new Vector2 (Mathf.Sign(_myRigidbody.velocity.x), 1f);
            }
        }

        private float _time;
        void CheckGround()
        {
            if (_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                isGrounded = true;
            }
            else
            {
                _time += Time.deltaTime;
                if (_time >= lateJumpTime)
                {
                    isGrounded = false;
                    _time = 0;
                }
            }
            
            if (_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || isClimbing)
            {
                _myAnimator.SetBool("isJumping", false);
            }
            
            else if (!_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !isClimbing)
            {
                _myAnimator.SetBool("isJumping", true);
            }
        }

        void CheckLadder()
        {
            if (_myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
            {
                isClimbable = true;
            }
            else
            {
                isClimbable = false;
            }
        }
        #endregion
    }
}
