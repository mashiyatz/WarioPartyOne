using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneratePowerUps : MonoBehaviour
{
    public Transform goalParentTransform;
    public GameObject goalPrefab;
    public Tilemap tilemapPath;
    public Tilemap tilemapObstacle;
    private List<Vector3> pathTilePositions;

    void Start()
    {
        pathTilePositions = new List<Vector3>();
        GetTilePositions();
        GenerateNewGoal();
    }

    void GetTilePositions()
    {
        foreach (Vector3Int pos in tilemapPath.cellBounds.allPositionsWithin)
        {
            Vector3 worldPlace = tilemapPath.CellToWorld(pos);
            if (tilemapPath.HasTile(pos) && !tilemapObstacle.HasTile(pos))
            {
                pathTilePositions.Add(worldPlace);
            }
        }
    }

    void GenerateNewGoal()
    {
        Instantiate(goalPrefab, pathTilePositions[Random.Range(0, pathTilePositions.Count)] + new Vector3(.25f, .25f, 0), Quaternion.identity, goalParentTransform);
    }

    void Update()
    {
        if (goalParentTransform.childCount == 0)
        {
            GenerateNewGoal();
        }
    }
}
