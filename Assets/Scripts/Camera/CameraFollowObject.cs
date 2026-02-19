using System.Collections;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    //this script is intended to smoothly make the camera
    //move as the player turns, rather than snapping
    [Header("References")]
    [SerializeField] private Transform _playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float _flipYRotationTime = 0.05f;

    private Coroutine _turnCoroutine;

    private PlayerAnimations _playerAnimations;

    private bool _isFacingRight;
    
    private void Awake()
    {
        _playerAnimations = _playerTransform.GetComponent<PlayerAnimations>();
        _isFacingRight = _playerAnimations.IsFacingRight;
    }
    
    void Update()
    {
        //this continually makes sure the cameraFollowObject follows the player's position
        transform.position = _playerTransform.position;
    }

    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < _flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            //this line is what actually handles lerping the y rotation
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, elapsedTime / _flipYRotationTime);
            //and this is what makes the camera actually turn
            //gradually thoroughout this while loop
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        //this function just determines which way the character is going to be facing
        //to manage where the camera should turn as well
        _isFacingRight = !_isFacingRight;

        if (_isFacingRight)
        return 180f;

        else
        return 0f;
    }
}
