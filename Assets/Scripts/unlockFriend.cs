using UnityEngine;

public class unlockFriend : MonoBehaviour
{   
    public GameManager gm;
    [SerializeField] private AudioClip unlockSFX;
    void Awake()
    {
        gm = GameManager.instance;
    }
    void Start()
    {
        gm = GameManager.instance;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 pointOfContact = collision.contacts[0].normal; //Grab the normal of the contact point we touched
        if(collision.gameObject.tag == "Player" && pointOfContact == new Vector2(0, -1) && this.gameObject.tag == "cagedBailey")
        {
            Debug.Log("bailey saved");
            SFXManager.instance.PlaySFXClip(unlockSFX, transform, 1f);
            this.gameObject.SetActive(false);
            gm.BaileySaved = true;
            //play an animation?
        }
        if(collision.gameObject.tag == "Player" && pointOfContact == new Vector2(0, -1) && this.gameObject.tag == "cagedOllie")
        {   
            Debug.Log("ollie saved");
            SFXManager.instance.PlaySFXClip(unlockSFX, transform, 1f);
            this.gameObject.SetActive(false);
            gm.OllieSaved = true;
            //play an animation?
        }
    }
}
