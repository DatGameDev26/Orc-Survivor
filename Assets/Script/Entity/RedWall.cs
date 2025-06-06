using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RedWall : MonoBehaviour
{
    [SerializeField] GameObject blowUpVFX;
    [SerializeField] Tilemap tilemap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Boss>() != null) return;

        CombatEntity combatEntity = collision.GetComponent<CombatEntity>();
        if (combatEntity != null)
        {
            combatEntity.Damage(99999);
        }
    }

    public void Terminate()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            TileBase tileAtPosition = tilemap.GetTile(position);

            if (tileAtPosition != null)
            {
                Vector3 spawnPosition = tilemap.CellToWorld(position) + new Vector3(0.5f, 0.5f, 0);
                Instantiate(blowUpVFX, spawnPosition, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}
