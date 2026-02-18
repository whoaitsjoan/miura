using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _cameraFollowGO;
    private CameraFollowObject _cameraFollowObject;
    private float _fallSpeedDampingChangeThreshold;

    private KaiController kaiController;
    private OllieController ollieController;
    private BaileyController baileyController;


    //these variables control flipping the player left or right
    private Rigidbody2D rb;
    [HideInInspector] public bool IsFacingRight;

    //these variables will handle when to change to an idle animation
    [SerializeField]
    private float _timeUntilAFK = 3f;
    private bool _isIdle;
    private float _idleTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _cameraFollowObject = _cameraFollowGO.GetComponent<CameraFollowObject>();
        _fallSpeedDampingChangeThreshold = CameraManager.instance._fallSpeedChangeThreshold;

        /*kaiController = GetComponent<KaiController>();
        ollieController = GetComponent<OllieController>();
        baileyController = GetComponent<BaileyController>();
        */
    }

    // Update is called once per frame
    void Update()
    {
        //stateInfo is going to be used to check if an animation has finished or not
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        //as long as we are not walking or jumping, the timer for an idle anim counts up
        if (!_isIdle && _animator.GetBool("isWalking") == false 
        && _animator.GetBool("isJumping") == false)
        {
            _idleTime += Time.deltaTime;
            //if that idle timer reaches our set threshold, and the idle animation isn't playing
            //then we go ahead and change our animation
            if (_idleTime > _timeUntilAFK && stateInfo.normalizedTime % 1 < 0.02f)
            {
                _isIdle = true;
                _animator.SetBool("isAFK", true);
            }
        //otherwise, if the current state has ended its animation,
        //we reset the idle counter and change our boolean for the animation change
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }
        //if we are falling, the camera needs to update
        if (rb.linearVelocity.y < _fallSpeedDampingChangeThreshold 
        && !CameraManager.instance.IsLerpingYDamping
        && !CameraManager.instance.LerpedFromPlayerFalling)
        { CameraManager.instance.LerpYDamping(true); }

        //if standing still or moving up
        if (rb.linearVelocity.y >= 0f 
        && !CameraManager.instance.IsLerpingYDamping
        && CameraManager.instance.LerpedFromPlayerFalling)
        {
            //then the camera needs to be reset
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    void FixedUpdate()
    {
      
    }

    private void ResetIdle()
    {
        _isIdle = false;
        _idleTime = 0;
        _animator.SetBool("isAFK", false);
    }

    private void TurnCheck()
    {
        //these statements check specifically if the player
        //should turn left or right based on pos or neg velocity
        if (rb.linearVelocity.x > 0)
        IsFacingRight = true;
        else if (rb.linearVelocity.x < 0)
        IsFacingRight = false;

        Turn();
    }

    public void Walking()
    {
        _animator.SetBool("isWalking", true);
        //if the player is moving at all based on the velocity
        //then we need to handle turning the player
        if (rb.linearVelocity.x < 0 || rb.linearVelocity.x > 0)
        {
            Debug.Log("Turn check!");
            TurnCheck();
        }
        
    }

    public void NotWalking()
    {
        _animator.SetBool("isWalking", false);
    }

    public void Jumping()
    {
        _animator.SetBool("isJumping", true);
    }

    public void NotJumping()
    {
        _animator.SetBool("isJumping", false);
    }

    private void Turn()
    {
        //here the player's y angle is rotated 180 degrees pos or neg
        //changing this changes the transform.right value
        //which will be important for our camera control scripting
        if (IsFacingRight)
        {
          Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
          transform.rotation = Quaternion.Euler(rotator);
          IsFacingRight = !IsFacingRight;  

          //Turn the Camera to match
          _cameraFollowObject.CallTurn();
        }

        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            //Turn the Camera to match
          _cameraFollowObject.CallTurn();  
        }
    }
}
