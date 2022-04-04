using System;
using System.Collections;
using Cinemachine;
using Data;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    /// <summary>
    /// This class is responsible for the player's movement and skills.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        Vector2 _moveInput;
        Rigidbody2D _myRigidbody;
        [SerializeField] Animator myAnimator;
        BoxCollider2D _myBodyCollider;
        PolygonCollider2D _myFeetCollider;
        PolygonCollider2D _myLArmCollider;
        PolygonCollider2D _myRArmCollider;
        private GameObject _feet;
        private GameObject _lArm;
        private GameObject _rArm;
        float _gravityScaleAtStart;
        private bool IsControllable { get; set; }
        private bool _isCancelled;


        [SerializeField] float climbSpeed = 5f;
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isClimbable;
        [SerializeField] private bool isClimbing; 
        public bool isShadow;

        public bool IsAlive { get; private set; }
        [SerializeField] private bool allowShift;
        [SerializeField] public Color playerColor;

        private RectTransform _eyeEffect;
        private AudioClipData _audioClipData;
        private AudioSource _audioSource;
        
        
        #endregion
        
        
        void Start()
        {
            shiftCount = LevelManager.Instance.shiftCountStart;
            playerColor = gameObject.GetComponent<SpriteRenderer>().color;
            _feet = transform.GetChild(0).gameObject;
            _lArm = transform.GetChild(1).gameObject;
            _rArm = transform.GetChild(2).gameObject;
            _myRigidbody = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            _myBodyCollider = transform.GetChild(3).gameObject.GetComponent<BoxCollider2D>();
            _myFeetCollider = _feet.GetComponent<PolygonCollider2D>();
            _myLArmCollider = _lArm.GetComponent<PolygonCollider2D>();
            _myRArmCollider = _rArm.GetComponent<PolygonCollider2D>();
            _eyeEffect = LevelManager.Instance.eyeEffect.GetComponent<RectTransform>();
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
            _audioClipData = GetComponent<AudioClipData>();
            if (allowShift)
            {
                ModifyShiftCount(0);
            }
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
        
        /// <summary>
        ///  Move the player in the direction of the input
        /// </summary>
        void Run() 
        {
            Vector2 playerVelocity = new Vector2 (_currentHorizontalSpeed, _myRigidbody.velocity.y);
            _myRigidbody.velocity = playerVelocity;
        
            bool playerHasHorizontalSpeed = Mathf.Abs(_currentHorizontalSpeed) > Mathf.Epsilon;
            myAnimator.SetBool(IsRunning, playerHasHorizontalSpeed);
            isMoving = playerHasHorizontalSpeed;
        }
        /// <summary>
        /// Calculate the speed of the player when running 
        /// </summary>
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
        /// <summary>
        /// Calculate the speed of the player when walking 
        /// </summary>
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

        /// <summary>
        /// prevent the player from jumping again until lateJumpTime has passed;
        /// </summary>
        /// <returns>
        /// IEnumerator for the lateJumpTime
        /// </returns>
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
                    LevelManager.Instance.LightAudio.volume = 0.2f;
                    LevelManager.Instance.ShadowAudio.volume = 0;
                    position = new Vector3(position.x, position.y - 100);
                    isShadow = false;
                }
                else
                {
                    LevelManager.Instance.LightAudio.volume = 0;
                    LevelManager.Instance.ShadowAudio.volume = 0.2f;
                    position = new Vector3(position.x, position.y + 100);
                    isShadow = true;
                }
                transform.position = position;
                myAnimator.SetBool("isShifting", true);
                LevelManager.Instance.shiftIndicator.SetTrigger("Shift");
                SoundManager.Instance.PlayEffect(_audioClipData.GetAudioClip(1), 0.3f);
                ModifyShiftCount(-1);
            }
            if (value.isPressed && isPeekAble && !isPressingMove && allowShift && !isClimbing)
            {
                StartCoroutine(TogglePeekAnimation());
            }
        }
        
         /// <summary>
         /// Toggle the peek animation
         /// <para>When the player is peeking, the player can't move, can't jump and climb</para>
         /// <para>if player use ShadowShift during peeking, the player will be shifted to the other side immediately</para>
         /// <para>Modify Peek-mask's size player opacity and eye-effect's size</para>
         /// </summary>

         IEnumerator TogglePeekAnimation()
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
                    _eyeEffect.sizeDelta += new Vector2(24, 20);
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
                SoundManager.Instance.PlayEffect(_audioClipData.GetAudioClip(0), 0.3f);
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
                        playerColor.a = 1f;
                        _eyeEffect.sizeDelta = new Vector2(2960, 2300);
                        break;
                    }
                    if (playerColor.a >= 0.5f)
                    {
                        playerColor.a -= 0.01f;
                    }
                    playerColor.a = 0.5f;
                    _eyeEffect.sizeDelta += new Vector2(-12, -10);
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
        /// <summary>
        /// Check if the player is climbing or not
        /// if the player is climbing, the player can't move, jump and peek
        /// <para>Add vertical force to the player to make him climb</para>
        /// </summary>
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
                        isPeekAble = true;
                        isShiftAble = true;
                        _myRigidbody.velocity = new Vector2(0,0);
                        playerColor.a = 1f;
                        _myBodyCollider.isTrigger = false;
                        _myFeetCollider.isTrigger = false;
                        _myLArmCollider.isTrigger = false;
                        _myRArmCollider.isTrigger = false;
                        _myRigidbody.gravityScale = _gravityScaleAtStart;
                        Debug.Log(isHiding);
                    }
                    else if(!isHiding)
                    {
                        IsControllable = false;
                        isJumpAble = false;
                        isHiding = true;
                        isPeekAble = false;
                        isShiftAble = false;
                        _myRigidbody.velocity = new Vector2(0,0);
                        _myRigidbody.gravityScale = 0f;
                        _myBodyCollider.isTrigger = true;
                        _myFeetCollider.isTrigger = true;
                        _myLArmCollider.isTrigger = true;
                        _myRArmCollider.isTrigger = true;
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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Interact"))
            {
                col.GetComponent<Hider>().ActivateInteract();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Interact"))
            {
                other.GetComponent<Hider>().DeactivateInteract();
            }
        }

        #endregion
        
        
        #region Animation
        /// <summary>
        /// Wakeup Animation
        /// </summary>
        public void Wakeup()
        {
            StartCoroutine(PauseMovement(8));
            myAnimator.Play("Wakeup");
            LevelManager.Instance.freeCamera.GetComponent<ObjectAim>().GameObjectToTarget(gameObject);
        }

        /// <summary>
        /// Pause Movement for a given time
        /// </summary>
        /// <param name="pauseTime"> the time used to pause everything </param>
        /// <returns></returns>
        public IEnumerator PauseMovement(float pauseTime)
        {

            IsControllable = false;
            isJumpAble = false;
            isShiftAble = false;
            isPeekAble = false;

            //Setting Time Freeze Here
            yield return new WaitForSeconds(pauseTime -2);
            IsControllable = true;
            isJumpAble = true;
            yield return new WaitForSeconds(2);
            isShiftAble = true;
            isPeekAble = true;

        }
                

        #endregion
                
        #endregion
        
        #region Passive
        /// <summary>
        /// Passive: Flip the player when he is looking at the other side
        /// </summary>
        void FlipSprite() {
            bool playerHasHorizontalSpeed = Mathf.Abs(_myRigidbody.velocity.x) > Mathf.Epsilon;
        
            if (playerHasHorizontalSpeed) {
                transform.localScale = new Vector2 (Mathf.Sign(_myRigidbody.velocity.x), 1f);
            }
        }

        
        private float _time;
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        
        /// <summary>
        /// Passive: Check if player is touching the ground
        /// <para>Also Check the Late Jump Time</para>
        /// </summary>
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

        /// <summary>
        /// Check if player is touching the Ladder
        /// </summary>
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

        /// <summary>
        /// Check if PlayerClone is int the wall
        /// </summary>
        /// <param name="inCollider"> sent from PlayerClone TriggerEnter2D</param>
        public void CheckIfInCollider(bool inCollider)
        {
            isShiftAble = !inCollider;
        }

        /// <summary>
        /// Check if player cancels the peek
        /// </summary>
        private void PeekCheck()
        {
            if (isPeeking && _isCancelled)
            {
                StartCoroutine(TogglePeekAnimation());
            }
        }

        /// <summary>
        /// Update the player opacity
        /// </summary>
        private void UpdateColor()
        {
            gameObject.GetComponent<SpriteRenderer>().color = playerColor;
        }
        #endregion

        #region ModifyRules

        /// <summary>
        /// Modify player shift count
        /// <para> Player Shift Count Can't be below zero</para>
        /// </summary>
        /// <param name="count">can be negative</param>
        public void ModifyShiftCount(int count)
        {
            shiftCount += count;
            if (shiftCount <= 0)
            {
                shiftCount = 0;
                allowShift = false;
            }
            ModifyShiftCountText();
        }

        /// <summary>
        /// Modify Player Alive Status.
        /// only Use when player is dead
        /// </summary>
        /// <param name="status"> true for alive false for dead</param>
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
                SoundManager.Instance.RandomSoundEffect(_audioClipData.GetAudioClipGroup(2,4), 0.3f);
                
            }
        }

        /// <summary>
        /// Modify the shift count text by using the shift count
        /// </summary>
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
        
        #region Getter

        public bool GetIsHiding()
        {
            return isHiding;
        }
        
        #endregion
    }
}
