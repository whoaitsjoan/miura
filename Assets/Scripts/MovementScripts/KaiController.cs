using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class KaiController : MonoBehaviour
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
    [Header("Pound")]
    [SerializeField] private float poundForce = 2.5f;
     [SerializeField] private float poundDuration = 0.1f;
    [SerializeField] private float poundCooldown = 0.1f;
    private bool onGround, isPounding;
    private int poundCycle =1;
    bool canPound = true;
    private Vector2 direction;
    bool isFacingRight;
    float horizontalMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>();   

        input.Kai.Enable();
        input.Ollie.Disable();
        input.Bailey.Disable();
        input.UI.Disable();
    }

    void Update(){
        Flip();
        if(!isPounding){
        Vector2 newVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = newVelocity;}
        Gravity();
    }

    private void FixedUpdate(){
        onGround = ground.OnGround;
        if (onGround)
        {
            poundCycle = 1;
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

    private void OnPound()
    {
        if (canPound && poundCycle == 1)
        {
            Debug.Log("pound");
            poundCycle = 0;
            StartCoroutine(startPound());
        }
        
    }

    private void OnMove(InputValue inputValue){
        horizontalMovement = inputValue.Get<Vector2>().x;
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
}
