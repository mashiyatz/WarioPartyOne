using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct Zone
{
    public Vector3Int topLeft;
    public Vector3Int bottomRight;
    public Vector3Int door;

    public Zone(Vector3Int topLeft, Vector3Int bottomRight, Vector3Int door)
    {
        this.topLeft = topLeft;
        this.bottomRight = bottomRight;
        this.door = door;
    }

    public void Activate(Tilemap tilemap)
    {
        ChangeColor(tilemap, Color.yellow);
        tilemap.SetColliderType(door, Tile.ColliderType.None);
    }

    public void Deactivate(Tilemap tilemap)
    {
        ChangeColor(tilemap, Color.white);
        // tilemap.SetColliderType(door, Tile.ColliderType.Sprite);
    }

    private void ChangeColor(Tilemap tilemap, Color color)
    {
        for (int x = topLeft.x; x <= bottomRight.x; x++)
        {
            for (int y = bottomRight.y; y <= topLeft.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y);
                tilemap.SetTileFlags(pos, TileFlags.None);
                tilemap.SetColor(pos, color);
            }
        }
    }


}

public class Zones : MonoBehaviour
{
    public Tilemap buildingTilemap;
    public Zone activeZone;
    private int lastZoneIndex = 99;
    private Zone[] zones;
    private float startTime;

    void Start()
    {
        zones = new Zone[22];
        GenerateZones();
        startTime = Time.time;
    }

    void GenerateZones()
    {
        // row 1
        zones[0] = new Zone(new Vector3Int(-9, 12), new Vector3Int(-7, 10), new Vector3Int(-8, 10));
        zones[1] = new Zone(new Vector3Int(-5, 12), new Vector3Int(-1, 10), new Vector3Int(-4, 10));
        zones[2] = new Zone(new Vector3Int(1, 12), new Vector3Int(4, 10), new Vector3Int(3, 10));
        zones[3] = new Zone(new Vector3Int(6, 12), new Vector3Int(8, 10), new Vector3Int(7, 10));
        // row 2 
        zones[4] = new Zone(new Vector3Int(-9, 8), new Vector3Int(-8, 6), new Vector3Int(-8, 6));
        zones[5] = new Zone(new Vector3Int(-6, 8), new Vector3Int(-4, 6), new Vector3Int(-5, 6));
        zones[6] = new Zone(new Vector3Int(-2, 8), new Vector3Int(1, 6), new Vector3Int(-1, 6));
        zones[7] = new Zone(new Vector3Int(3, 8), new Vector3Int(5, 6), new Vector3Int(4, 6));
        zones[8] = new Zone(new Vector3Int(7, 8), new Vector3Int(8, 6), new Vector3Int(7, 6));
        // center
        zones[9] = new Zone(new Vector3Int(-9, 4), new Vector3Int(-6, 2), new Vector3Int(-7, 2));
        zones[10] = new Zone(new Vector3Int(-9, 0), new Vector3Int(-6, -5), new Vector3Int(-8, -5));
        zones[11] = new Zone(new Vector3Int(5, 4), new Vector3Int(8, -1), new Vector3Int(6, -1));
        zones[12] = new Zone(new Vector3Int(5, -3), new Vector3Int(8, -5), new Vector3Int(7, -5));
        // row 3
        zones[13] = new Zone(new Vector3Int(-9, -7), new Vector3Int(-8, -9), new Vector3Int(-8, -9));
        zones[14] = new Zone(new Vector3Int(-6, -7), new Vector3Int(-4, -9), new Vector3Int(-5, -9));
        zones[15] = new Zone(new Vector3Int(-2, -7), new Vector3Int(1, -9), new Vector3Int(0, -9));
        zones[16] = new Zone(new Vector3Int(3, -7), new Vector3Int(5, -9), new Vector3Int(4, -9));
        zones[17] = new Zone(new Vector3Int(7, -7), new Vector3Int(8, -9), new Vector3Int(7, -9));
        // row 4
        zones[18] = new Zone(new Vector3Int(-9, -11), new Vector3Int(-7, -13), new Vector3Int(-8, -13));
        zones[19] = new Zone(new Vector3Int(-5, -11), new Vector3Int(-1, -13), new Vector3Int(-4, -13));
        zones[20] = new Zone(new Vector3Int(0, -11), new Vector3Int(4, -13), new Vector3Int(2, -13));
        zones[21] = new Zone(new Vector3Int(6, -11), new Vector3Int(8, -13), new Vector3Int(7, -13));
    }

    public Vector3Int ActivateZone()
    {
        int rand;
        if (lastZoneIndex > 0 && lastZoneIndex <= 8)
        {
            rand = Random.Range(9, zones.Length);
        } else if (lastZoneIndex > 13 && lastZoneIndex <= 21)
        {
            rand = Random.Range(0, 13);
        } else
        {
            rand = Random.Range(0, zones.Length);
        }
        // activeZone = zones[Random.Range(0, zones.Length)];
        activeZone = zones[rand];
        activeZone.Activate(buildingTilemap);
        lastZoneIndex = rand;

        return activeZone.door;
    }

    IEnumerator DeactivateZone(Zone zone)
    {
        yield return new WaitForSeconds(15.0f);
        zone.Deactivate(buildingTilemap);
    }

    public void DeactivateZone()
    {
        activeZone.Deactivate(buildingTilemap);
    }

}
