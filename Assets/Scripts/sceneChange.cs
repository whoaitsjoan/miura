using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneName;
    [HideInInspector]public bool isInDoor;
    [SerializeField]
    private UnityEvent _newScene;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isInDoor = true;
            //_newScene?.Invoke();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isInDoor = false;
        }
    }
    public void loadNewScene()
    {
        if(isInDoor)
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
