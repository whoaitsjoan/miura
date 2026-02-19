using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChange : MonoBehaviour
{
    public string sceneName;
    [HideInInspector]public bool isInDoor;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isInDoor = true;
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
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
