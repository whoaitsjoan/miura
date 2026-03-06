using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class conveyorBelt : MonoBehaviour
{
    public Transform endpoint;
    public float speed;
    public GameObject conveyor;

    void Start()
    {
        endpoint = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        conveyor = this.gameObject;
        StartCoroutine(flipBelt());
    }

    void OnCollisionStay2D(Collision2D other)
    {
        other.transform.position = Vector2.MoveTowards(other.transform.position, endpoint.position, speed * Time.deltaTime);
    }
    

    IEnumerator flipBelt()
    {
        while (conveyor.activeSelf)
        {
            Debug.Log("running flipbelt");
            yield return new WaitForSeconds(1);
            conveyor.transform.Rotate(0, 180, 0);
            Debug.Log("belt flipped");
        }
        
    }
}
