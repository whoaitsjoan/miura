using System.Collections;
using Unity.VisualScripting;
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

    private sceneChange sceneChange;
    private bool onGround, isPounding;
    private int poundCycle =1;
    bool canPound = true;
    private Vector2 playerVelocity;
    //bool isFacingRight;
    float direction;

    private PlayerAnimations playerAnimations;
    private CharacterSwitch characterSwitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>(); 
        playerAnimations = GetComponent<PlayerAnimations>();
        characterSwitch = GetComponent<CharacterSwitch>();     

        input.Kai.Enable();
        input.Ollie.Disable();
        input.Bailey.Disable();
        input.UI.Disable();
    }

    void Update(){
        //Flip();
        if(!isPounding){
        Vector2 playerVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = playerVelocity;
        }
        Gravity();
    }

    private void FixedUpdate(){
        onGround = ground.OnGround;
        if (onGround)
        {
            poundCycle = 1;
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
        direction = inputValue.Get<Vector2>().x;
        playerAnimations.Walking();

        if (direction == 0)
        playerAnimations.NotWalking();
    }

    private void OnOllieSwitch(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
        Debug.Log("Switching to Ollie!");
        characterSwitch.OllieSwitch();
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
