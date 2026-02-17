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
    private Vector2 direction;
    bool isFacingRight;
    float horizontalMovement;
    bool isFlying; 
    bool canFly = true;
    int flyCycle = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>();   

        input.Bailey.Enable();

        input.Kai.Disable();
        input.Ollie.Disable();
        input.UI.Disable();
    }

    void Update(){
        Flip();
        if (!isFlying)
        {
            Vector2 newVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = newVelocity;
        }
        Gravity();
    }

    private void FixedUpdate(){
        onGround = ground.OnGround;
        if (onGround)
        {
            flyCycle = 1;
        }
    }

  private void Flip(){
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

    private void OnJump(InputValue inputValue){

        if (onGround && inputValue.isPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }    
        if (onGround && !inputValue.isPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y*0.5f);
        } 
    }

    private void OnMove(InputValue inputValue){
        horizontalMovement = inputValue.Get<Vector2>().x;
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

    private void OnFly()
    {
        Debug.Log("fly");
        if (canFly&&flyCycle==1)
        {
            flyCycle = 0;
            StartCoroutine(startFly());
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

