using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{
    [SerializeField]
    private float _timeUntilAFK;
    private bool _isIdle;
    private float _idleTime;
    private int _idleAnimation;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_isIdle && (animator.GetBool("isWalking") == false))
        {
            _idleTime += Time.deltaTime;

            if (_idleTime > _timeUntilAFK && stateInfo.normalizedTime % 1 < 0.02f)
            {
                _isIdle = true;
                _idleAnimation = 1;
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }
        animator.SetFloat("IdleState", _idleAnimation);
    }

    private void ResetIdle()
    {
        _isIdle = false;
        _idleTime = 0;
        _idleAnimation = 0;
    }
}
