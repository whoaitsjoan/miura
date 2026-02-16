using UnityEngine;
using UnityEngine.InputSystem;

public class KaiController1 : MonoBehaviour
{
    private InputSystem_Actions input;
    private Rigidbody2D rb;
    private GroundCheck ground;
    [SerializeField] private float jumpForce = 2.5f;
    [SerializeField] private float moveSpeed;
    private bool onGround;

    private PlayerAnimations playerAnimations;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>(); 
        playerAnimations = GetComponent<PlayerAnimations>();  

        input.Kai.Enable();
        input.Ollie.Disable();
        input.Bailey.Disable();
        input.UI.Disable();
    }

    private void FixedUpdate(){
        onGround = ground.OnGround;

        if (onGround && rb.linearVelocity.y == 0)
        playerAnimations.NotJumping();
    }

    private void OnJump(){
        
        if(onGround){
            rb.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
            Debug.Log("jump");
            playerAnimations.Jumping();
        }    
    }

    private void OnMove(InputValue inputValue){
        rb.linearVelocity = inputValue.Get<Vector2>()*moveSpeed;
        playerAnimations.Walking();

        if (rb.linearVelocity.x == 0)
        playerAnimations.NotWalking();
    }
}
