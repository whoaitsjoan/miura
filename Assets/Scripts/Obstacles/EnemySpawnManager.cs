using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    //for floater enemy spawns
    [SerializeField]
    private GameObject floaterEnemy;
    [SerializeField] 
    private Transform spawnLocation;

    public Quaternion spawnRotation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void SpawnEnemy()
    {
        Instantiate(floaterEnemy, spawnLocation.position, spawnRotation);
    }


    /*public void SpawnEnemies (Collider2D spawnableAreaCollider, GameObject[] enemies)
    {
        
    }
    private Vector2 GetRandomPointInCollider(Collider2D collider, float offset = 1f)
    {
        Bounds collBounds = collider.bounds;
        Vector2 minBounds = new Vector2(collBounds.min.x + offset, collBounds.min.y + offset);
        Vector2 maxBounds = new Vector2(collBounds.max.x - offset, collBounds.max.y - offset);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);
        return new Vector2(randomX, randomY);
    }
    */
}
