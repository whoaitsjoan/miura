using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public class CameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    private Collider2D _coll;

    private void Start()
    {
        _coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //any object that collides with the player and has this set to true
            //will cause the camera to pan over
            if (customInspectorObjects.panCameraOnContact)
            {
                CameraManager.instance.PanCameraOnContract(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
                Debug.Log("Camera pan in!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            //likewise the camera should pan back again
            //after they exit the trigger for this object
            CameraManager.instance.PanCameraOnContract(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
                Debug.Log("Camera pan out");
    }
}

[System.Serializable]

public class CustomInspectorObjects
{
    //these variables will control what the inspector has access to
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    //these variables are going to control which cameras we switch between
    [HideInInspector] public CinemachineCamera cameraOnLeft;
    [HideInInspector] public CinemachineCamera cameraOnRight;

    //these variables control which direction cameras move between
    //and how fast they move when changing
    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}
//we define a custom direction for where to pan from here
public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}
//we set up a unique editor function for our class here
//so that specific variables are visible in the inspector
//depending on which of our booleans above are true
[CustomEditor(typeof(CameraControlTrigger))]
#if UNITY_EDITOR
public class MyScriptEditor : Editor
{
    CameraControlTrigger cameraControlTrigger;

    private void OnEnable()
    {
        cameraControlTrigger = (CameraControlTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        //this sets up the regular inspector for whatever object this is attached to
        DrawDefaultInspector();

        //but if swapCameras is true, we add camera variables to the inspector
        if (cameraControlTrigger.customInspectorObjects.swapCameras)
        {
            cameraControlTrigger.customInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera On Left", cameraControlTrigger.customInspectorObjects.cameraOnLeft, 
            typeof(CinemachineCamera), true) as CinemachineCamera;
            cameraControlTrigger.customInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera On Right", cameraControlTrigger.customInspectorObjects.cameraOnRight, 
            typeof(CinemachineCamera), true) as CinemachineCamera;
        }

        //and if panCameraOnContact is true, we add the Pan variables to the inspector
        if (cameraControlTrigger.customInspectorObjects.panCameraOnContact)
        {
           cameraControlTrigger.customInspectorObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction",
           cameraControlTrigger.customInspectorObjects.panDirection);

           cameraControlTrigger.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance",
           cameraControlTrigger.customInspectorObjects.panDistance);
           cameraControlTrigger.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan Time",
           cameraControlTrigger.customInspectorObjects.panTime);  
        }

        //finally, if we selected any of these, we need to manually make sure
        //that the editor saves any changes to these variables
        if (GUI.changed) { EditorUtility.SetDirty(cameraControlTrigger); }
    }
}
#endif