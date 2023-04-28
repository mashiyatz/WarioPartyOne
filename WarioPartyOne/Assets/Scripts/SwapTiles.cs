using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwapTiles : MonoBehaviour
{
    public TileBase[] oldTileset;
    public TileBase[] newTileset;

    void Awake()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        for (int i = 0; i < oldTileset.Length; i++)
        {
            tilemap.SwapTile(oldTileset[i], newTileset[i]);
        }
    }
}
