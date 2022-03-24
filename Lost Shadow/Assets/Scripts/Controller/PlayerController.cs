using System.Collections;
using System.Runtime.CompilerServices;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        Vector2 _moveInput;
        Rigidbody2D _myRigidbody;
        [SerializeField] Animator _myAnimator;
        BoxCollider2D _myBodyCollider;
        PolygonCollider2D _myFeetCollider;
        private GameObject _feet;
        float _gravityScaleAtStart;
        bool _isControllable;
        private bool _isCancelled;


        
        [SerializeField] float climbSpeed = 5f;
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isClimbable;
        [SerializeField] private bool isClimbing; 
        public bool isShadow;

        public bool IsAlive { get; private set; }
        [SerializeField] private bool allowShift;
        [SerializeField] public Color playerColor;
        #endregion
        
        
        void Start()
        {
            shiftCount = LevelManager.Instance.shiftCountStart;
            playerColor = gameObject.GetComponent<SpriteRenderer>().color;
            _feet = GameObject.FindWithTag("Feet");
            _myRigidbody = GetComponent<Rigidbody2D>();
            _myAnimator = GetComponent<Animator>();
            _myBodyCollider = GetComponent<BoxCollider2D>();
            _myFeetCollider = _feet.GetComponent<PolygonCollider2D>();
            _gravityScaleAtStart = _myRigidbody.gravityScale;
            _peekMask = LevelManager.Instance.peekMask;
            _isControllable = true;
            isJumpAble = true;
            isShadow = true;
            isShiftAble = true;
            isPeekAble = true;
            isPeeking = false;
            isMoving = false;
            isHiding = false;
            IsAlive = true;
            allowShift = LevelManager.Instance.allowShift;
            _peekSize = 2000;
            _peek = LevelManager.Instance.peek;
            LevelManager.Instance.CheckCutScene();
        }

        void Update()
        {
            if (_isControllable)
            {
                Run();
                ClimbLadder();
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    CalculateRun();
                }
                else
                {
                    CalculateWalk();
                }
            }
            
            FlipSprite();
            CheckGround();
            CheckLadder();
            PeekCheck();
            UpdateColor();
        }

        #region InputSystem
        #region Move
        [Header("Move")] 
        [SerializeField] private float runSpeed = 7f;
        [SerializeField] private float acceleration = 50f;
        private float _currentHorizontalSpeed;
        [SerializeField] private float deAcceleration = 30f;
        [SerializeField] private bool isMoving;
        
        void OnMove(InputValue value) {
            _moveInput = value.Get<Vector2>();
            if (isPeeking && _moveInput.x != 0)
            {
                _isCancelled = true;
            }
        }
        void Run() 
        {
            Vector2 playerVelocity = new Vector2 (_currentHorizontalSpeed, _myRigidbody.velocity.y);
            _myRigidbody.velocity = playerVelocity;
        
            bool playerHasHorizontalSpeed = Mathf.Abs(_currentHorizontalSpeed) > Mathf.Epsilon;
            _myAnimator.SetBool(IsRunning, playerHasHorizontalSpeed);
            isMoving = playerHasHorizontalSpeed;
        }
        private void CalculateRun() {
            if (_moveInput.x != 0 && !isPeeking) {
                _myAnimator.SetFloat("animSpeed", 0.8f);
                _currentHorizontalSpeed += _moveInput.x * acceleration * Time.deltaTime;
                        
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -runSpeed, runSpeed);
                        
            }
            else {
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, deAcceleration * Time.deltaTime);
            }
                    
        }
        
        private void CalculateWalk() {
            if (_moveInput.x != 0 && !isPeeking) {
                _myAnimator.SetFloat("animSpeed", 0.4f);
                _currentHorizontalSpeed += _moveInput.x * acceleration/2 * Time.deltaTime;
                        
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -runSpeed/2, runSpeed/2);
                        
            }
            else {
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, deAcceleration/2 * Time.deltaTime);
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
                    if (value.isPressed && isGrounded && isJumpAble && !isPeeking)
                    {
                        StartCoroutine(IsJumpAble());
                        var velocity = _myRigidbody.velocity;
                        velocity = new Vector2(velocity.x, 0f);
                        velocity += new Vector2(0f, jumpSpeed);
                        _myRigidbody.velocity = velocity;
                    }
                    if (isPeeking && value.isPressed)
                    {
                        _isCancelled = true;
                    }
                }

        private IEnumerator IsJumpAble()
        {
            isJumpAble = false;
            yield return new WaitForSeconds(lateJumpTime);
            isJumpAble = true;
        }
        #endregion
        #region ShadowShift

        [Header("ShadowShift")]
        [SerializeField] private bool isShiftAble;
        [SerializeField] private bool isPeekAble;
        [SerializeField] private bool isPeeking;
        [SerializeField] private int shiftCount;
        private RectTransform _peek; 
        private RectTransform _peekMask;
        private float _peekSize;

        void OnShadowShift(InputValue value)
        {
            if (value.isPressed && isShadow && isShiftAble && isPeeking && !isMoving && allowShift)
            {
                var position = transform.position;
                position = new Vector3(position.x, position.y - 100);
                transform.position = position;
                isShadow = false;
                ModifyShiftCount(-1);
            }
            else if (value.isPressed && !isShadow && isShiftAble && isPeeking && !isMoving && allowShift)
            {
                var position = transform.position;
                position = new Vector3(position.x, position.y + 100);
                transform.position = position;
                isShadow = true;
                ModifyShiftCount(-1);
            }
            if (value.isPressed && isPeekAble && !isMoving && allowShift)
            {
                TogglePeek();
            }
        }
        

        private void TogglePeek()
        {
            if (isPeeking)
            {
                StartCoroutine(PeekAnimation());
            }
            else
            {
                StartCoroutine(PeekAnimation());
            }
        }

        IEnumerator PeekAnimation()
        {

            if (isPeeking)
            {
                isPeekAble = false;
                isPeeking = false;
                _isControllable = false;
                isJumpAble = false;
                while (_peek.sizeDelta.x > 0)
                {
                    if (playerColor.a < 1)
                    {
                        playerColor.a += 0.01f;
                    }
                    playerColor.a = 1;
                    _peek.sizeDelta += new Vector2(-50, -50);
                    _peekMask.sizeDelta += new Vector2(-50, -50);
                    yield return new WaitForSeconds(0.01f);
                }
                gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                _isControllable = true;
                isJumpAble = true;
                isPeekAble = true;
                _isCancelled = false;
            }
            else
            {
                _isControllable = false;
                isJumpAble = false;
                isPeekAble = false;
                gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Front";
                while (_peek.sizeDelta.x < _peekSize)
                {
                    if (playerColor.a >= 0.5f)
                    {
                        playerColor.a -= 0.01f;
                    }
                    playerColor.a = 0.5f;
                    _peek.sizeDelta += new Vector2(25, 25);
                    _peekMask.sizeDelta += new Vector2(25, 25);
                    yield return new WaitForSeconds(0.01f);
                }
                _isControllable = true;
                isJumpAble = true;
                isPeeking = true;
                isPeekAble = true;
                _isCancelled = false;
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
        
                // if (_moveInput.x != 0 && isClimbing)
                // {
                //     isClimbing = true;
                // }
        
                if (isClimbing)
                {
                    isGrounded = true;
                    Vector2 climbVelocity = new Vector2 (_currentHorizontalSpeed, _moveInput.y * climbSpeed);
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

        #region Interact
        [Header("Interact")]
        public int colliderID;
        void OnInteract(InputValue value)
        {
            if (value.isPressed)
            {
                if (colliderID == 0)
                {
                    Debug.Log("You can't Interact Nothing");
                }
                else if (colliderID == 1001)
                {
                    if (isHiding)
                    {
                        isHiding = false;
                        _isControllable = true;
                        isJumpAble = true;
                        playerColor.a = 1f;
                        Debug.Log(isHiding);
                    }
                    else if(!isHiding)
                    {
                        _isControllable = false;
                        isJumpAble = false;
                        isHiding = true;
                        playerColor.a = 0.1f;
                        Debug.Log(isHiding);
                    }
                }
            }
        }
        #endregion

        #region InteractFunction

        [Header("InteractFunction")] 
        [SerializeField] private bool isHiding;

        #endregion
        
        
        #region Animation
        
        public void Wakeup()
        {
            StartCoroutine(PauseMovement(6));
            _myAnimator.Play("Wakeup");
        }
        
        IEnumerator PauseMovement(float pauseTime)
        {

            _isControllable = false;
            isJumpAble = false;
            isShiftAble = false;

            //Setting Time Freezeeee Here
            yield return new WaitForSeconds(pauseTime);
            
            isJumpAble = true;
            isShiftAble = true;
            _isControllable = true;
            
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
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

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

        public void CheckIfInCollider(bool inCollider)
        {
            if (inCollider)
            {
                isShiftAble = false;
            }
            else
            {
                isShiftAble = true;
            }
        
        }

        void PeekCheck()
        {
            if (isPeeking && _isCancelled)
            {
                TogglePeek();
            }
        }

        void UpdateColor()
        {
            gameObject.GetComponent<SpriteRenderer>().color = playerColor;
        }
        #endregion

        #region ModifyRules

        public void ModifyShiftCount(int count)
        {
            shiftCount += count;
            if (shiftCount <= 0)
            {
                shiftCount = 0;
                isShiftAble = false;
            }
        }

        public void ModifyAlive(bool status)
        {
            IsAlive = status;
            if (!IsAlive)
            {
                _isControllable = false;
                isJumpAble = false;
                allowShift = false;
                _myAnimator.SetBool("isDead", true);
                _myRigidbody.velocity = new Vector2(0,0);
            }
        }

        #endregion
    }
}
