using UnityEngine;
using UnityEngine.InputSystem;

public class OllieController : MonoBehaviour
{
    private InputSystem_Actions input;
    private Rigidbody2D rb;
    private GroundCheck ground;
    [SerializeField] private float jumpForce = 1.5f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashForce;
    private bool onGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>();   

        input.Ollie.Enable();

        input.Kai.Disable();
        input.Bailey.Disable();
        input.UI.Disable();
    }

    private void FixedUpdate(){
        onGround = ground.OnGround;
    }

    private void OnJump(){
        
        if(onGround){
            rb.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
            Debug.Log("jump");
        }    
    }

    private void OnMove(InputValue inputValue){
        rb.linearVelocity = inputValue.Get<Vector2>()*moveSpeed;
    }

    private void OnDash(){
        rb.AddForce(Vector2.right*dashForce, ForceMode2D.Impulse);
    }
}
