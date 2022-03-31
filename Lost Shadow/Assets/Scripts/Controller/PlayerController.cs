using System.Collections;
using Cinemachine;
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
        [SerializeField] Animator myAnimator;
        BoxCollider2D _myBodyCollider;
        PolygonCollider2D _myFeetCollider;
        private GameObject _feet;
        float _gravityScaleAtStart;
        public bool IsControllable { get; private set; }
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
            myAnimator = GetComponent<Animator>();
            _myBodyCollider = GetComponent<BoxCollider2D>();
            _myFeetCollider = _feet.GetComponent<PolygonCollider2D>();
            _gravityScaleAtStart = _myRigidbody.gravityScale;
            _peekMask = LevelManager.Instance.peekMask;
            IsControllable = true;
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
            ModifyShiftCount(0);
            LevelManager.Instance.CheckCutScene();
        }

        void Update()
        {
            if (IsControllable)
            {
                ClimbLadder();
                LevelManager.Instance.cameraObj.GetComponent<Animator>().SetBool("Cutscene", false);
                
            }
            Run();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                CalculateRun();
            }
            else
            {
                CalculateWalk();
            }
            FlipSprite();
            CheckGround();
            CheckLadder();
            PeekCheck();
            UpdateColor();
            LevelManager.Instance.mainCineCamera.GetComponent<ObjectAim>().GameObjectToTarget(gameObject);
        }

        #region InputSystem
        #region Move
        [Header("Move")] 
        [SerializeField] private float runSpeed = 7f;
        [SerializeField] private float acceleration = 50f;
        private float _currentHorizontalSpeed;
        [SerializeField] private float deAcceleration = 30f;
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isPressingMove;
        
        void OnMove(InputValue value) {

            if (IsControllable)
            { 
                _moveInput = value.Get<Vector2>();
            }
            else
            {
                _moveInput = new Vector2(0, 0);
            }


            if (_moveInput == new Vector2(0, 0))
            {
                isPressingMove = false;
            }
            if (isPeeking && _moveInput.x != 0)
            {
                _isCancelled = true;
                myAnimator.SetBool("isPeeking",false);
                LevelManager.Instance.shiftIndicator.SetBool("isPeeking", false);
            }
        }
        void Run() 
        {
            Vector2 playerVelocity = new Vector2 (_currentHorizontalSpeed, _myRigidbody.velocity.y);
            _myRigidbody.velocity = playerVelocity;
        
            bool playerHasHorizontalSpeed = Mathf.Abs(_currentHorizontalSpeed) > Mathf.Epsilon;
            myAnimator.SetBool(IsRunning, playerHasHorizontalSpeed);
            isMoving = playerHasHorizontalSpeed;
        }
        private void CalculateRun() {
            if (_moveInput.x != 0 && !isPeeking) {
                myAnimator.SetFloat("runAnimSpeed", 0.8f);
                _currentHorizontalSpeed += _moveInput.x * acceleration * Time.deltaTime;
                        
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -runSpeed, runSpeed);
                        
            }
            else {
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, deAcceleration * Time.deltaTime);
            }
                    
        }
        
        private void CalculateWalk() {
            if (_moveInput.x != 0 && !isPeeking) {
                myAnimator.SetFloat("runAnimSpeed", 0.4f);
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
                        myAnimator.SetBool("isPeeking",false);
                        LevelManager.Instance.shiftIndicator.SetBool("isPeeking", false);
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
            if (value.isPressed && isShiftAble && isPeeking && !isMoving && allowShift && !isClimbing)
            {
                var position = transform.position;
                if (isShadow)
                {
                    position = new Vector3(position.x, position.y - 100);
                    isShadow = false;
                }
                else
                {
                    position = new Vector3(position.x, position.y + 100);
                    isShadow = true;
                }
                transform.position = position;
                myAnimator.SetBool("isShifting", true);
                LevelManager.Instance.shiftIndicator.SetTrigger("Shift");
                ModifyShiftCount(-1);
            }
            if (value.isPressed && isPeekAble && !isPressingMove && allowShift && !isClimbing)
            {
                TogglePeek();
            }
        }
        

        private void TogglePeek()
        {
            
            StartCoroutine(PeekAnimation());
            
            
        }

        IEnumerator PeekAnimation()
        {

            if (isPeeking)
            {
                isPeekAble = false;
                isPeeking = false;
                IsControllable = false;
                isJumpAble = false;
                myAnimator.SetBool("isPeeking",false);
                LevelManager.Instance.shiftIndicator.SetBool("isPeeking", false);

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
                IsControllable = true;
                isJumpAble = true;
                isPeekAble = true;
                _isCancelled = false;
            }
            else
            {
                IsControllable = false;
                isJumpAble = false;
                isPeekAble = false;
                isPeeking = true;
                myAnimator.SetBool("isPeeking",true);
                LevelManager.Instance.shiftIndicator.SetBool("isPeeking", true);
                gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Front";
                while (_peek.sizeDelta.x < _peekSize)
                {
                    if (myAnimator.GetBool("isShifting"))
                    {
                        _peek.sizeDelta = new Vector2(0, 0);
                        _peekMask.sizeDelta = new Vector2(0, 0);
                        isPeeking = false;
                        myAnimator.SetBool("isPeeking",false);
                        LevelManager.Instance.shiftIndicator.SetBool("isPeeking", false);

                        break;
                    }
                    if (playerColor.a >= 0.5f)
                    {
                        playerColor.a -= 0.01f;
                    }
                    playerColor.a = 0.5f;
                    _peek.sizeDelta += new Vector2(25, 25);
                    _peekMask.sizeDelta += new Vector2(25, 25);
                    yield return new WaitForSeconds(0.01f);
                }
                IsControllable = true;
                isJumpAble = true;
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

                if (_currentHorizontalSpeed != 0 && isClimbable)
                {
                    isClimbing = true;
                }

                if (isClimbing)
                {
                    isGrounded = true;
                    Vector2 climbVelocity = new Vector2 (_currentHorizontalSpeed, _moveInput.y * climbSpeed);
                    _myRigidbody.velocity = climbVelocity;
                    _myRigidbody.gravityScale = 0f;
                    bool playerHasVerticalSpeed = Mathf.Abs(_myRigidbody.velocity.y) > Mathf.Epsilon;
                    myAnimator.SetBool("isClimbing", true);
                    if (playerHasVerticalSpeed)
                    {
                        myAnimator.SetFloat("climbAnimSpeed",1);
                    }
                    else
                    {
                        myAnimator.SetFloat("climbAnimSpeed",0);
                    }
                }
                else
                {
                    _myRigidbody.gravityScale = _gravityScaleAtStart;
                    myAnimator.SetBool("isClimbing", false);
                }
            }
            else
            {
                isClimbing = false;
                _myRigidbody.gravityScale = _gravityScaleAtStart;
                myAnimator.SetBool("isClimbing", false);
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
                        IsControllable = true;
                        isJumpAble = true;
                        playerColor.a = 1f;
                        Debug.Log(isHiding);
                    }
                    else if(!isHiding)
                    {
                        IsControllable = false;
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
            myAnimator.Play("Wakeup");
            LevelManager.Instance.freeCamera.GetComponent<ObjectAim>().GameObjectToTarget(gameObject);
        }

        public IEnumerator PauseMovement(float pauseTime)
        {

            IsControllable = false;
            isJumpAble = false;
            isShiftAble = false;

            //Setting Time Freeze Here
            yield return new WaitForSeconds(pauseTime);
            
            isJumpAble = true;
            isShiftAble = true;
            IsControllable = true;
            
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
        
        private void CheckGround()
        {
            if (_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                isGrounded = true;
                isClimbing = false;
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
                myAnimator.SetBool("isJumping", false);
            }
            
            else if (!_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !isClimbing)
            {
                myAnimator.SetBool("isJumping", true);
            }
        }

        private void CheckLadder()
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
            isShiftAble = !inCollider;
        }

        private void PeekCheck()
        {
            if (isPeeking && _isCancelled)
            {
                TogglePeek();
            }
        }

        private void UpdateColor()
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
            ModifyShiftCountText();
        }

        public void ModifyAlive(bool status)
        {
            IsAlive = status;
            if (!IsAlive)
            {
                IsControllable = false;
                isJumpAble = false;
                allowShift = false;
                myAnimator.SetBool("isDead", true);
                _myRigidbody.velocity = new Vector2(0,_myRigidbody.velocity.y);
            }
        }

        private void ModifyShiftCountText()
        {
            if (shiftCount == 0)
            {
                LevelManager.Instance.shiftCountText.text = "-";
            }
            else
            {
                LevelManager.Instance.shiftCountText.text = $"{(shiftCount)}";
            }
            
        }

        #endregion
    }
}
