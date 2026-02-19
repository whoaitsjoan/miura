using UnityEngine;
using UnityEngine.Events;

public class BreakableBlocks : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _hit;
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.collider.GetComponent<KaiController>();
        Debug.Log("Collision!");
        if (player.isPounding)
        _hit?.Invoke();
    }
}