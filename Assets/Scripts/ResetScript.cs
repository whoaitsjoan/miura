using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour
{
    private Vector3 reset;
    private GameObject _player;
    private Collider2D _coll;
    void Start()
    {
    _player = GameObject.FindGameObjectWithTag("Player");
    _coll = GetComponent<Collider2D>();      
    reset = GameObject.Find("reset").transform.position;  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == _player)
        {
            collision.gameObject.transform.position = reset;
        }
    }
    private void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
