using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneName;
    [HideInInspector]public bool isInDoor;
    [SerializeField] public AudioClip footsteps;
   /* public Animator transition;

    public float endSceneWait;

    public float transitionWait;
*/
    
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
    public void LoadNewScene()
    {
        //SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        StartCoroutine(LoadScene());
        
    }

    private IEnumerator LoadScene()
    {
        SFXManager.instance.PlaySFXClip(footsteps, transform, 1f);
        yield return new WaitForSeconds(footsteps.length);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
