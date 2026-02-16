using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    [HideInInspector] public bool _isFacingRight;

    //these variables will handle when to change to an idle animation
    [SerializeField]
    private float _timeUntilAFK;
    private bool _isIdle;
    private float _idleTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!_isIdle && animator.GetBool("isWalking") == false 
        && animator.GetBool("isJumping") == false)
        {
            _idleTime += Time.deltaTime;

            if (_idleTime > _timeUntilAFK && stateInfo.normalizedTime % 1 < 0.02f)
            {
                _isIdle = true;
                animator.SetBool("isAFK", true);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }
    }

    void FixedUpdate()
    {
      
    }

    private void ResetIdle()
    {
        _isIdle = false;
        _idleTime = 0;
        animator.SetBool("isAFK", false);
    }

    private void TurnCheck()
    {
        if (rb.linearVelocity.x > 0)
        _isFacingRight = true;
        else if (rb.linearVelocity.x < 0)
        _isFacingRight = false;

        Turn();
    }

    public void Walking()
    {
        animator.SetBool("isWalking", true);
        if (rb.linearVelocity.x < 0 || rb.linearVelocity.x > 0)
        TurnCheck();
    }

    public void NotWalking()
    {
        animator.SetBool("isWalking", false);
    }

    public void Jumping()
    {
        animator.SetBool("isJumping", true);
    }

    public void NotJumping()
    {
        animator.SetBool("isJumping", false);
    }

    private void Turn()
    {
        
        if (_isFacingRight)
        {
          Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
          transform.rotation = Quaternion.Euler(rotator);
          _isFacingRight = !_isFacingRight;  
        }

        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            _isFacingRight = !_isFacingRight;  
        }
    }
}
