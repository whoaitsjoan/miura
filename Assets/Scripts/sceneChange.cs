using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneName;
    [HideInInspector]public bool isInDoor;
    
    /* void OnTriggerEnter2D(Collider2D collision)
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
    } */
    public void loadNewScene()
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        
    }
}
