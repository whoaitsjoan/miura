using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class secretGuyCollection : MonoBehaviour
{
    private GameManager gm;
    public TMP_Text endGameText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm = GameManager.instance;
    }
    // add to secret guys
    void OnCollisionEnter2D(Collision2D collision)
    {
        gm.friendsCollected += 1;
    }
    // add to door in room 8
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            endGameText.text = "You have saved " + gm.friendsCollected + "/8 friends!";
        }
    }
}
