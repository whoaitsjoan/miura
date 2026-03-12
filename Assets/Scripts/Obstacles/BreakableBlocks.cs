using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class BreakableBlocks : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _hit;
    [SerializeField]
    private AudioClip _blocksHit;
    [SerializeField]
    private GameObject blockParticles;

    public Tilemap tilemap;
    private List<Vector3> validSpawnPositions = new List<Vector3>();

    void Start()
    {
        GatherValidPositions();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.collider.GetComponent<MainController>();
        Debug.Log("Collision!");
        if (player.isPounding && other.contacts[0].normal.y < 0) {
        SFXManager.instance.PlaySFXClip(_blocksHit, transform, 1f);
        _hit?.Invoke();
        }
    }
    public void SpawnParticles()
    {
        if (validSpawnPositions.Count == 0) return;
        Instantiate(blockParticles, validSpawnPositions.Last(), Quaternion.identity);
    }

    private void GatherValidPositions()
    {
        validSpawnPositions.Clear();
        BoundsInt boundsInt = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(boundsInt);
        Vector3 start = tilemap.CellToWorld(new Vector3Int(boundsInt.xMin, boundsInt.yMin, 0));

        for (int x = 0; x < boundsInt.size.x; x++)
        {
            for (int y = 0; y < boundsInt.size.y; y++)
            {
                TileBase tile = allTiles[x + y * boundsInt.size.x];
                if (tile != null)
                {
                    Vector3 place = start + new Vector3(x, y, 0);
                    validSpawnPositions.Add(place);
                }
            }
        }
    }
}