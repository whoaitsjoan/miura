using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineCamera[] _allVirtualCameras;

    [Header("Damping for Jump/Fall")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallPanTime = 0.35f;
    public float _fallSpeedChangeThreshold = -15f;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpYPanCoroutine;
    private Coroutine _panCameraCoroutine;

    private CinemachineCamera _currentCamera;
    private CinemachinePositionComposer _positionComposer;

    private float _normYPanAmount;
    private Vector2 _startingTargetOffset;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        //this will set our current camera based on which on our last is active
        for (int i = 0; i < _allVirtualCameras.Length; i++)
        {
            if (_allVirtualCameras[i].enabled)
            {
                //set the current active camera
                _currentCamera = _allVirtualCameras[i];

                //and the position composer from that current camera
                _positionComposer = _currentCamera.GetComponent<CinemachinePositionComposer>();

            }
        }
        //YDamping will be based on what we set in the inspector
        _normYPanAmount = _positionComposer.Damping.y;

        _startingTargetOffset =  _positionComposer.TargetOffset;
    }

    #region PanCamera

    public void PanCameraOnContract(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    } 

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        //first we check if we're actually supposed to be panning the camera
        if(!panToStartingPos)
        {
            //the direction and distance can be set from our enum
            switch (panDirection)
            {
                case PanDirection.Left:
                endPos = Vector2.left;
                break;
                case PanDirection.Right:
                endPos = Vector2.right;
                break;
                case PanDirection.Up:
                endPos = Vector2.up;
                break;
                case PanDirection.Down:
                endPos = Vector2.down;
                break;
                default:
                break;
            }

            endPos *= panDistance;

            startingPos = _startingTargetOffset;

            endPos += startingPos;
        }
        //otherwise we pan back to our original position
        else
        {
            startingPos = _positionComposer.TargetOffset;
            endPos = _startingTargetOffset;
        }

        //now we handle the actual panning of the camera
        float elapsedTime = 0f;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, elapsedTime / panTime);
            _positionComposer.TargetOffset = panLerp;
            yield return null;
        }
    }

    #endregion

    #region LerpYDamping
    public void LerpYDamping (bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;
        //first we need to starting amount to dampen by
        float startDampAmount = _positionComposer.Damping.y;
        float endDampAmount = 0f;

        //next we need the end dampening amount
        //which is only relevant if the player is falling
        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }

        else
        endDampAmount = _normYPanAmount;

        //then we need to lerp how long the camera should be panning for
        float elapsedTime = 0f;
        while (elapsedTime < _fallPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / _fallPanTime);
            _positionComposer.Damping.y = lerpedPanAmount;
            yield return null;
        }
        IsLerpingYDamping = false;
    }
    #endregion
}
