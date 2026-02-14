using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool OnGround { get; private set; }
    public float Friction {get; private set; }

    //[SerializeField] private SpriteRenderer _spriteRenderer;
    private Vector2 _normal;
    //private PhysicsMaterial2D _material;
    private RaycastHit2D _hit;
   // private float _playerHalfHeight;

    private void Awake()
    {
        //_playerHalfHeight = _spriteRenderer.bounds.extents.y;
    }

    void FixedUpdate()
    {
        _hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));
        if (_hit)
        {
        OnGround = true;
        //RetrieveFriction(_hit.collider);
        }
        else{
            OnGround = false;
        }
    }

    /* private Ray ray;
private void OnDrawGizmos()
{
    ray = new Ray(transform.position, Vector2.down * 1.5f);
    Gizmos.color= Color.red;
    Gizmos.DrawRay(ray);
}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    } 

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        Friction = 0;
    }
    //this method is checking for every contact point and 
    //OnGround is true or false with the bitwise operation
    //comparing your player's normal value to the ground's position
    private void EvaluateCollision(Collision2D collision)
    {
        for (int i=0; i < collision.contactCount; i++)
        {
            _normal = collision.GetContact(i).normal;
            OnGround |= _normal.y >= 0.9f;
        }
    }
*/
    
    /* private void RetrieveFriction (Collider2D collider)
    {
        _material = collider.GetComponent<Rigidbody2D>().sharedMaterial;
        Friction = 0;

        if(_material != null)
        {
            Friction = _material.friction;
        }
    } */

}
