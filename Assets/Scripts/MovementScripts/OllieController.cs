using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OllieController : MonoBehaviour
{
    private InputSystem_Actions input;
    private Rigidbody2D rb;
    private GroundCheck ground;
    [SerializeField] private float moveSpeed;
    private Vector2 playerVelocity;
    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float fallSpeedMultiplier = 2f;
  
    private bool onGround;
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashCooldown = 0.1f;
    bool isDashing; 
    bool canDash = true;
    //bool isFacingRight = true;
    float direction;

    private PlayerAnimations playerAnimations;
    private CharacterSwitch characterSwitch;
    private sceneChange sceneChange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>(); 
        characterSwitch = GetComponent<CharacterSwitch>();  
        playerAnimations = GetComponent<PlayerAnimations>();

        input.Ollie.Enable();

        input.Kai.Disable();
        input.Bailey.Disable();
        input.UI.Disable();
    }

    void Update(){
        //Flip();
        Gravity();
    }

    private void FixedUpdate(){
        onGround = ground.OnGround;
        if (!isDashing)
        {
            playerVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = playerVelocity;
        }
        if (onGround && rb.linearVelocity.y == 0)
        playerAnimations.NotJumping();
    }

    /*private void Flip(){
        if (isFacingRight && rb.linearVelocity.x < -0.1f){
            isFacingRight = false;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
        if (!isFacingRight && rb.linearVelocity.x > 0.1f){
            isFacingRight = true;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
    */
   private void OnJump(InputValue inputValue){

        if (onGround && inputValue.isPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            playerAnimations.Jumping();
        }    
        if (onGround && !inputValue.isPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y*0.5f);
        } 
    }

    private void OnMove(InputValue inputValue){
        direction = inputValue.Get<Vector2>().x;
        playerAnimations.Walking();

        if (direction == 0)
        playerAnimations.NotWalking();
    }

        private void Gravity()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void OnDash(){
        if(canDash){
            Debug.Log("dash");
            StartCoroutine(DashCoroutine());
        }
    }

    private void OnKaiSwitch(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
        Debug.Log("Switching to Kai!");
        characterSwitch.KaiSwitch();
        }
    }

    private void OnBaileySwitch(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
        Debug.Log("Switching to Bailey!");
        characterSwitch.BaileySwitch();
        }
    }
    private void OnInteract(InputValue inputValue)
    {
        if(sceneChange.isInDoor){
            sceneChange.loadNewScene();
        }
    }


    private IEnumerator DashCoroutine(){
        canDash = false;
        isDashing = true;
        float dashDirection = playerAnimations.IsFacingRight ? -1f : 1f;
        var gravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);
        

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        isDashing = false;
        rb.gravityScale = gravity;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
