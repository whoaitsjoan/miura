using System;
using UnityEngine;

public class FloaterEnemy : MonoBehaviour
{
    private Vector3 reset, startPos;
    private Vector2 pointOfContact;
    public Animator anim;
    
    public SpriteRenderer sprite;

    void Start()
    {
        //reset = GameObject.Find("reset").transform.position;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        /* if (transform.rotation.y == 180f)
        rb.AddForce(Vector2.left);

        else if (transform.rotation.y == 0f)
        rb.AddForce(Vector2.right); */
    }

    /* void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = reset;
        }

        else if (collision.gameObject.tag == "CameraBounds")
        {
            Destroy(gameObject);
        }
    } */

    void OnTriggerEnter2D(Collider2D collision)
    {
        anim.enabled = true;
        sprite.enabled = true;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        pointOfContact = collision.contacts[0].normal; //Grab the normal of the contact point we touched

        if(collision.gameObject.tag == "Player" && pointOfContact == new Vector2(0,-1))
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = reset;
        }
    }
}
