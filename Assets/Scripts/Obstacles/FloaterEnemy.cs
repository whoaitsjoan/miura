using System;
using UnityEngine;

public class FloaterEnemy : MonoBehaviour
{
    private Vector3 reset, startPos;
    private Rigidbody2D rb;

    [SerializeField]


    void Awake()
    {
        reset = GameObject.Find("reset").transform.position;
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();  

        if (transform.rotation.y == 180f)
        rb.AddForce(Vector2.left);

        else if (transform.rotation.y == 0f)
        rb.AddForce(Vector2.right);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = reset;
        }

        else if (collision.gameObject.tag == "CameraBounds")
        {
            Destroy(gameObject);
        }
    }
}
