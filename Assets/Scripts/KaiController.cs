using UnityEngine;
using UnityEngine.InputSystem;

public class KaiController : MonoBehaviour
{
    private InputSystem_Actions input;
    private Rigidbody2D rb;
    private GroundCheck ground;
    [SerializeField] private float jumpForce = 2.5f;
    [SerializeField] private float moveSpeed;
    private bool onGround;

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
}
