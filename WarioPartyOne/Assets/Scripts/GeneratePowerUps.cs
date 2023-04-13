using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneratePowerUps : MonoBehaviour
{
    public Transform goalParentTransform;
    public GameObject goalPrefab;
    // public GameObject[] paparazziPowerUpPrefabs;
    public Transform paparazziPowerUpTransform;
    public GameObject batteryPrefab;
    public Tilemap tilemapPath;
    public Tilemap tilemapObstacle;
    private List<Vector3> pathTilePositions;

    void Start()
    {
        pathTilePositions = new List<Vector3>();
        GetTilePositions();
        GenerateNewGoal();
        StartCoroutine(GeneratePaparazziPowerUp());
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

    IEnumerator GeneratePaparazziPowerUp()
    {
        // randomly instantiate from a list of powerups, weighing specific powerups differently 
        while (true)
        {
            if (paparazziPowerUpTransform.childCount == 0)
            {
                Instantiate(batteryPrefab, pathTilePositions[Random.Range(0, pathTilePositions.Count)] + new Vector3(.25f, .25f, 0), Quaternion.identity, paparazziPowerUpTransform);
            }
            yield return new WaitForSecondsRealtime(15);
        }
    }

    void Update()
    {
        if (goalParentTransform.childCount == 0)
        {
            GenerateNewGoal();
        }
    }
}
