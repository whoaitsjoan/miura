using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class MainController : MonoBehaviour
{
    //component references
    private InputSystem_Actions input;
    private Rigidbody2D rb;
    private GroundCheck ground;
    private SceneChange sceneChange;
    private PlayerAnimations playerAnimations;
    private CharacterSwitch characterSwitch;
    private GameManager gm;
    private TrailRenderer trail;

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
    bool isInDoor;
    string sceneName;
    [SerializeField] private float moveSpeed = 4.59f;

   /*  [Header("PlayerBools")]
    [SerializeField]private bool gm.isKai = true;
    [SerializeField]private bool gm.isOllie;
    [SerializeField]private bool gm.isBailey; */

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

    [Header("Player SFX")]
    [SerializeField] public AudioClip jumpSound;
    [SerializeField] public AudioClip landSound;   
    [SerializeField] public AudioClip poundSound;

    [SerializeField] public AudioClip dashSound; 

    [SerializeField] public AudioClip flySound;         

    void Awake()
    {
        gm = GameManager.instance;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>(); 
        playerAnimations = GetComponent<PlayerAnimations>();
        characterSwitch = GetComponent<CharacterSwitch>();   
        trail = GetComponent<TrailRenderer>();   
        gm = GameManager.instance;
        gm.isKai = true;
        if (gm.isOllie)
        {
            gm.isOllie = true;
            gm.isBailey = false;
            gm.isKai = false;
            jumpForce = 10f;
            Debug.Log("Switching to Ollie!");
            characterSwitch.OllieSwitch();
        }
        if (gm.isBailey)
        {
            gm.isBailey = true;
            gm.isKai = false;
            gm.isOllie = false;
            jumpForce = 11.5f;
            Debug.Log("Switching to Bailey!");
            characterSwitch.BaileySwitch();
        }
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
            SFXManager.instance.PlaySFXClip(jumpSound, transform, 1f);

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
        if (direction == 0)
        playerAnimations.NotWalking();
    }
    private void OnAbility()
    {
        if (gm.isKai && canPound && poundCycle == 1)
        {
            Debug.Log("pound");
            poundCycle = 0;
            StartCoroutine(startPound());
        }
        if(gm.isOllie && canDash){
            Debug.Log("dash");
            StartCoroutine(DashCoroutine());
        }
        if (gm.isBailey&&canFly&&flyCycle==1)
        {
            flyCycle = 0;
            StartCoroutine(startFly());
        }
    }
    private IEnumerator startPound()
    {
        canPound = false;
        isPounding = true;
        trail.emitting = true;
        rb.gravityScale = baseGravity * poundForce;
        rb.AddForce(Vector2.down*poundForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(poundDuration);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        isPounding = false;
        trail.emitting = false;
        yield return new WaitForSeconds(poundCooldown);
        canPound = true;
    }
    private IEnumerator DashCoroutine(){
        canDash = false;
        isDashing = true;
        trail.emitting = true;
        float dashDirection = playerAnimations.IsFacingRight ? -1f : 1f;
        var gravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);
        

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        isDashing = false;
        rb.gravityScale = gravity;
        trail.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private IEnumerator startFly()
    {
        canFly = false;
        isFlying = true;
        var gravity = rb.gravityScale;
        rb.gravityScale = 0;
        trail.emitting = true;
        rb.AddForce(Vector2.up*flyForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(flyDuration);
        isFlying = false;
        rb.gravityScale = gravity;
        trail.emitting = false;
        yield return new WaitForSeconds(flyCooldown);
        canFly = true;
    }
    private void OnKaiSwitch(InputValue inputValue)
    {
        if (inputValue.isPressed && !gm.isKai)
        {
            gm.isKai = true;
            gm.isBailey = false;
            gm.isOllie = false;
            jumpForce = 13f;
            jumpSound = Resources.Load<AudioClip>("Audio/jump-kai");
            Debug.Log("Switching to Kai!");
            characterSwitch.KaiSwitch();
        }
    }
    private void OnOllieSwitch(InputValue inputValue)
    {
        if (inputValue.isPressed && !gm.isOllie && gm.OllieSaved)
        {
            gm.isOllie = true;
            gm.isBailey = false;
            gm.isKai = false;
            jumpForce = 10f;
            Debug.Log("Switching to Ollie!");
            jumpSound = Resources.Load<AudioClip>("Audio/jump-ollie");
            characterSwitch.OllieSwitch();
        }
    }

    private void OnBaileySwitch(InputValue inputValue)
    {
        if (inputValue.isPressed && !gm.isBailey && gm.BaileySaved)
        {
            gm.isBailey = true;
            gm.isKai = false;
            gm.isOllie = false;
            jumpForce = 11.5f;
            Debug.Log("Switching to Bailey!");
            jumpSound = Resources.Load<AudioClip>("Audio/jump-bailey");
            characterSwitch.BaileySwitch();
        }
    }   

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            isInDoor = true;
            sceneChange = collision.gameObject.GetComponent<SceneChange>();
            Debug.Log("in door to "+sceneChange.sceneName);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            isInDoor = false;
            sceneChange = null;
            Debug.Log("exited door to "+sceneChange);
        }
    }
    /*void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            isInDoor = true;
            sceneChange = collision.gameObject.GetComponent<SceneChange>();
            Debug.Log("in door to "+sceneChange.sceneName);
        }
    }
    */

    private void OnInteract(InputValue inputValue)
    {
        //sceneChange = other.gameObject.GetComponent<SceneChange>();
        if (inputValue.isPressed && isInDoor)
        {
            SFXManager.instance.PlaySFXClip(jumpSound, transform, 1f);
            sceneChange.LoadNewScene();
        }
    }
}
