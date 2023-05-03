using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct Zone
{
    public Vector3Int topLeft;
    public Vector3Int bottomRight;

/*    public Zone(Vector3Int topLeft, Vector3Int bottomRight, Vector3Int door)
    {
        this.topLeft = topLeft;
        this.bottomRight = bottomRight;
        this.door = door; 
    }*/
    public Zone(Vector3Int topLeft, Vector3Int bottomRight)
    {
        this.topLeft = topLeft;
        this.bottomRight = bottomRight;
    }

    public void Activate(Tilemap buildingMap, Tilemap pathMap)
    {
        ChangeColor(buildingMap, pathMap, Color.yellow);
        // tilemap.SetColliderType(door, Tile.ColliderType.None);
    }

    public void Deactivate(Tilemap buildingMap, Tilemap pathMap)
    {
        ChangeColor(buildingMap, pathMap, Color.white);
        // tilemap.SetColliderType(door, Tile.ColliderType.Sprite);
    }

    private void ChangeColor(Tilemap buildingMap, Tilemap pathMap, Color color)
    {
        for (int x = topLeft.x - 1; x <= bottomRight.x + 1; x++)
        {
            for (int y = bottomRight.y - 1; y <= topLeft.y + 1; y++)
            {
                Vector3Int pos = new Vector3Int(x, y);
                buildingMap.SetTileFlags(pos, TileFlags.None); 
                buildingMap.SetColor(pos, color);
                pathMap.SetTileFlags(pos, TileFlags.None);
                pathMap.SetColor(pos, color);
                if (color == Color.white) pathMap.SetColliderType(pos, Tile.ColliderType.None);
                else if (color == Color.yellow) pathMap.SetColliderType(pos, Tile.ColliderType.Sprite); 
            }
        }
    }
/*    public bool CheckIfCelebInZone(Tilemap pathmap, Transform celeb)
    {
        // seems very inefficient!
        Vector3Int celebTilePosition = pathmap.WorldToCell(celeb.position);
        for (int x = topLeft.x - 1; x <= bottomRight.x + 1; x++)
        {
            for (int y = bottomRight.y - 1; y <= topLeft.y + 1; y++)
            {
                if (celebTilePosition == new Vector3Int(x, y))
                {
                    return true;
                }
            }
        }
        return false;
    }*/


}

public class Zones : MonoBehaviour
{
    public Tilemap buildingTilemap;
    public Tilemap pathTilemap;
    public Zone activeZone;
    private int lastZoneIndex = 99;
    private Zone[] zones;
    private float startTime;

    void Start()
    {
        zones = new Zone[36];
        GenerateZones();
        startTime = Time.time;
    }

    void GenerateZones()
    {
        // row 1, from top left
        zones[0] = new Zone(new Vector3Int(-15, 10), new Vector3Int(-14, 8));
        zones[1] = new Zone(new Vector3Int(-12, 10), new Vector3Int(-8, 8));
        zones[2] = new Zone(new Vector3Int(-5, 10), new Vector3Int(-4, 8));
        zones[3] = new Zone(new Vector3Int(3, 10), new Vector3Int(4, 9));
        zones[4] = new Zone(new Vector3Int(7, 10), new Vector3Int(10, 9));
        zones[5] = new Zone(new Vector3Int(12, 10), new Vector3Int(15, 9));

        // row 2 
        zones[6] = new Zone(new Vector3Int(-15, 6), new Vector3Int(-12, 5));
        zones[7] = new Zone(new Vector3Int(-10, 6), new Vector3Int(-8, 5));
        zones[8] = new Zone(new Vector3Int(-5, 6), new Vector3Int(-4, 5));
        zones[9] = new Zone(new Vector3Int(-2, 10), new Vector3Int(1, 5));
        zones[10] = new Zone(new Vector3Int(3, 7), new Vector3Int(4, 5));
        zones[11] = new Zone(new Vector3Int(7, 7), new Vector3Int(8, 5));
        zones[12] = new Zone(new Vector3Int(10, 7), new Vector3Int(11, 5));
        zones[13] = new Zone(new Vector3Int(13, 7), new Vector3Int(14, 5));

        // row 3
        zones[14] = new Zone(new Vector3Int(-15, 3), new Vector3Int(-13, 1));
        zones[15] = new Zone(new Vector3Int(-11, 3), new Vector3Int(-8, 1));
        zones[16] = new Zone(new Vector3Int(7, 3), new Vector3Int(9, 1));
        zones[17] = new Zone(new Vector3Int(11, 3), new Vector3Int(14, 1));

        // row 4
        zones[18] = new Zone(new Vector3Int(-15, -1), new Vector3Int(-12, -3));
        zones[19] = new Zone(new Vector3Int(-10, -1), new Vector3Int(-8, -3));
        zones[20] = new Zone(new Vector3Int(7, -1), new Vector3Int(10, -3));
        zones[21] = new Zone(new Vector3Int(12, -1), new Vector3Int(14, -3));

        // row 5
        zones[22] = new Zone(new Vector3Int(-15, -5), new Vector3Int(-14, -7));
        zones[23] = new Zone(new Vector3Int(-12, -5), new Vector3Int(-11, -7));
        zones[24] = new Zone(new Vector3Int(-9, -5), new Vector3Int(-8, -7));
        zones[25] = new Zone(new Vector3Int(-5, -5), new Vector3Int(-4, -7));
        zones[26] = new Zone(new Vector3Int(-2, -5), new Vector3Int(1, -10));
        zones[27] = new Zone(new Vector3Int(3, -5), new Vector3Int(4, -6));
        zones[28] = new Zone(new Vector3Int(7, -5), new Vector3Int(9, -6));
        zones[29] = new Zone(new Vector3Int(11, -5), new Vector3Int(14, -6));

        // row 6
        zones[30] = new Zone(new Vector3Int(-15 , -9), new Vector3Int(-13, -10));
        zones[31] = new Zone(new Vector3Int(-11 , -9), new Vector3Int(-8, -10));
        zones[32] = new Zone(new Vector3Int(-5 , -9), new Vector3Int(-4, -10));
        zones[33] = new Zone(new Vector3Int(3 , -8), new Vector3Int(4, -10));
        zones[34] = new Zone(new Vector3Int(7 , -8), new Vector3Int(11, -10));
        zones[35] = new Zone(new Vector3Int(13 , -8), new Vector3Int(14, -10));
    }

    public Vector3 ActivateZone()
    {
        int rand;
        if (lastZoneIndex > 0 && lastZoneIndex <= 13)
        {
            rand = Random.Range(14, zones.Length);
        } else if (lastZoneIndex > 22 && lastZoneIndex <= 35)
        {
            rand = Random.Range(0, 22);
        } else
        {
            rand = Random.Range(0, zones.Length);
        }
        // activeZone = zones[Random.Range(0, zones.Length)];
        activeZone = zones[rand];
        activeZone.Activate(buildingTilemap, pathTilemap);
        lastZoneIndex = rand;

        return CalculateBuildingCenterInWorldPosition(activeZone);
    }

    public Vector3 CalculateBuildingCenterInWorldPosition(Zone zone)
    {
        Vector3 bottomRightWorld = buildingTilemap.CellToWorld(zone.bottomRight);
        Vector3 topLeftWorld = buildingTilemap.CellToWorld(zone.topLeft);
        Vector3 offset = new Vector3((bottomRightWorld.x - topLeftWorld.x) / 1.5f, (topLeftWorld.y - bottomRightWorld.y) / 1.25f, 0);
        Vector3 center = new Vector3(topLeftWorld.x, bottomRightWorld.y, 0) + offset;
        return center;
    }

    public void DeactivateZone()
    {
        activeZone.Deactivate(buildingTilemap, pathTilemap);
    }

}
