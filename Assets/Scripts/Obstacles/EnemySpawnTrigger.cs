using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    //[SerializeField] private GameObject[] _enemiesToSpawn;
    //[SerializeField] private Collider2D _spawnableArea;
    private GameObject _player;
    private Collider2D _coll;
    void Start()
    {
    _player = GameObject.FindGameObjectWithTag("Player");
    _coll = GetComponent<Collider2D>();        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _player)
        {
            Vector2 exitDir = (collision.transform.position - _coll.bounds.center).normalized;
            if(exitDir.x > 0)
            {
                EnemySpawnManager.instance.SpawnEnemy();
            }
            //spawnenemies
        }
    }

}
