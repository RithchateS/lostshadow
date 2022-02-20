using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    
    private void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
        Jump();
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value) {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            return;
        }

        if (value.isPressed) {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnShadowShift(InputValue value) {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            return;
        }

        if (value.isPressed) {
            StartCoroutine(ShadowShift());
        }
    }
    
    IEnumerator ShadowShift()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        myAnimator.SetTrigger("isShadowShift");
        Debug.Log("1");
        yield return new WaitForSeconds(0.8f);
        Debug.Log("2");
        if ((currentSceneIndex % 2) == 0){
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else {
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
    }

    void Jump()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myAnimator.SetBool("isJumping", true);
        }
        else
        {
            myAnimator.SetBool("isJumping", false);
        }
    }
    
    
    void Run() {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void ClimbLadder() {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
    
}
