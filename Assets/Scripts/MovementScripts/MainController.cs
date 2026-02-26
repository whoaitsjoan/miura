using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainController : MonoBehaviour
{
    //component references
    private InputSystem_Actions input;
    private Rigidbody2D rb;
    private GroundCheck ground;
    private sceneChange sceneChange;
    private PlayerAnimations playerAnimations;
    private CharacterSwitch characterSwitch;

    //movement
    private Vector2 playerVelocity;
    float direction;
    private bool onGround;
    public bool isPounding;
    private int poundCycle =1;
    bool canPound = true;
    bool isDashing; 
    bool canDash = true;
    bool isFlying; 
    bool canFly = true;
    int flyCycle = 0;
    [SerializeField] private float moveSpeed = 4.59f;

    [Header("PlayerBools")]
    [SerializeField]private bool isKai = true;
    [SerializeField]private bool isOllie;
    [SerializeField]private bool isBailey;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float fallSpeedMultiplier = 2f;

    [Header("Pound")]

    [SerializeField] private float poundForce = 2.5f;
    [SerializeField] private float poundDuration = 0.1f;
    [SerializeField] private float poundCooldown = 0.1f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 1.0f;
    [SerializeField] private float dashCooldown = 1.0f;

    [Header("Fly")]
    [SerializeField] float flyForce = 15f;
    [SerializeField] private float flyDuration = 0.1f;
    [SerializeField] private float flyCooldown = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>(); 
        playerAnimations = GetComponent<PlayerAnimations>();
        characterSwitch = GetComponent<CharacterSwitch>();     
    }

    void Update(){
        if(!isPounding && !isFlying && !isDashing){
            playerVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = playerVelocity;
        }
        Gravity();
    }

    private void FixedUpdate(){
        onGround = ground.OnGround;
        if (onGround)
        {
            poundCycle = 1;
            flyCycle = 1;
        }

        if (onGround && rb.linearVelocity.y == 0)
        playerAnimations.NotJumping();
    }

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

    private void OnMove(InputValue inputValue){
        direction = inputValue.Get<Vector2>().x;
        playerAnimations.Walking();
        /* if (direction == 0)
        playerAnimations.NotWalking(); */
    }
    private void OnAbility()
    {
        if (isKai && canPound && poundCycle == 1)
        {
            Debug.Log("pound");
            poundCycle = 0;
            StartCoroutine(startPound());
        }
        if(isOllie && canDash){
            Debug.Log("dash");
            StartCoroutine(DashCoroutine());
        }
        if (isBailey&&canFly&&flyCycle==1)
        {
            flyCycle = 0;
            StartCoroutine(startFly());
        }
    }
    private IEnumerator startPound()
    {
        canPound = false;
        isPounding = true;
        rb.gravityScale = baseGravity * poundForce;
        rb.AddForce(Vector2.down*poundForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(poundDuration);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        isPounding = false;
        yield return new WaitForSeconds(poundCooldown);
        canPound = true;
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
    private IEnumerator startFly()
    {
        canFly = false;
        isFlying = true;
        var gravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.AddForce(Vector2.up*flyForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(flyDuration);
        isFlying = false;
        rb.gravityScale = gravity;
        yield return new WaitForSeconds(flyCooldown);
        canFly = true;
    }
    private void OnKaiSwitch(InputValue inputValue)
    {
        if (inputValue.isPressed && !isKai)
        {
            isKai = true;
            isBailey = false;
            isOllie = false;
            jumpForce = 13f;
            Debug.Log("Switching to Kai!");
            characterSwitch.KaiSwitch();
        }
    }
    private void OnOllieSwitch(InputValue inputValue)
    {
        if (inputValue.isPressed && !isOllie)
        {
            isOllie = true;
            isBailey = false;
            isKai = false;
            jumpForce = 10f;
            Debug.Log("Switching to Ollie!");
            characterSwitch.OllieSwitch();
        }
    }

    private void OnBaileySwitch(InputValue inputValue)
    {
        if (inputValue.isPressed && !isBailey)
        {
            isBailey = true;
            isKai = false;
            isOllie = false;
            jumpForce = 11.5f;
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
}
