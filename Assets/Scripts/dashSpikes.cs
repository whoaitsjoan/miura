using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class dashSpikes : MonoBehaviour
{
    private Vector3 reset;

    void Awake()
    {
        reset = GameObject.Find("reset").transform.position;

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = reset;
        }
    }
}
