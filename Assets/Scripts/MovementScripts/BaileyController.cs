using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class BaileyController : MonoBehaviour
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

    [Header("Fly")]
    
    [SerializeField] float flyForce;
    [SerializeField] private float flyDuration = 0.1f;
    [SerializeField] private float flyCooldown = 0.1f;
    private bool onGround;
    //bool isFacingRight;
    float direction;
    bool isFlying; 
    bool canFly = true;
    int flyCycle = 0;

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

        input.Bailey.Enable();

        input.Kai.Disable();
        input.Ollie.Disable();
        input.UI.Disable();
    }

    void Update(){
        //Flip();
        if (!isFlying)
        {
            playerVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = playerVelocity;
        }
        Gravity();
    }

    private void FixedUpdate(){
        onGround = ground.OnGround;
        if (onGround)
        {
            flyCycle = 1;
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

    private void OnKaiSwitch(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
        Debug.Log("Switching to Kai!");
        characterSwitch.KaiSwitch();
        }
    }

    private void OnOllieSwitch(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
        Debug.Log("Switching to Ollie!");
        characterSwitch.OllieSwitch();
        }
    }

    private void OnFly()
    {
        Debug.Log("fly");
        if (canFly&&flyCycle==1)
        {
            flyCycle = 0;
            StartCoroutine(startFly());
        }
        
    }

    private void OnInteract(InputValue inputValue)
    {
        if(sceneChange.isInDoor){
            sceneChange.loadNewScene();
        }
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
}

