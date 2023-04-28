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

    public Transform speedUpTransform;
    public GameObject speedUpPrefab;

    public Transform cameraUpgradeTransform;
    public GameObject cameraUpgradePrefab;

    public Transform disguiseTransform;
    public GameObject disguisePrefab;

    public Tilemap tilemapPath;
    public Tilemap tilemapObstacle;
    private List<Vector3> pathTilePositions;

    public Zones zones;
    private Coroutine co;

    private float speedUpTimer;
    private float cameraUpgradeTimer;
    private float disguiseTimer;

    void Start()
    {
        speedUpTimer = Time.time;
        cameraUpgradeTimer = Time.time;
        disguiseTimer = Time.time;

        pathTilePositions = new List<Vector3>();
        GetTilePositions();
        GenerateNewGoal();
        GenerateSpeedPowerUp();
        GenerateCameraUpgrade();
        GenerateDisguisePowerup();
        StartCoroutine(GenerateRandomPowerUp());
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
        if (co != null) StopCoroutine(co);

        Vector3Int pos = zones.ActivateZone();
        Vector3 worldPlace = tilemapPath.CellToWorld(pos);
        // Instantiate(goalPrefab, pathTilePositions[Random.Range(0, pathTilePositions.Count)] + new Vector3(.125f, .125f, 0), Quaternion.identity, goalParentTransform);
        GameObject go = Instantiate(goalPrefab, worldPlace + new Vector3(.125f, .125f, 0), Quaternion.identity, goalParentTransform);
        
        co = StartCoroutine(DeactivateGoal(go));
    }

    IEnumerator DeactivateGoal(GameObject goalObject)
    {
        yield return new WaitForSeconds(15.0f);
        if (goalObject != null)
        {
            zones.DeactivateZone();
            Destroy(goalObject);
        }
        
    }

    IEnumerator GenerateRandomPowerUp()
    {
        // randomly instantiate from a list of powerups, weighing specific powerups differently 
        while (true)
        {
            if (paparazziPowerUpTransform.childCount == 0)
            {
                Instantiate(
                    batteryPrefab, 
                    pathTilePositions[Random.Range(0, pathTilePositions.Count)] + new Vector3(.125f, .125f, 0), 
                    Quaternion.Euler(new Vector3(0, 0, 35f)), 
                    paparazziPowerUpTransform
                    );
            }
            yield return new WaitForSecondsRealtime(15);
        }
    }

    void GenerateSpeedPowerUp()
    {
        // consider the position of speed powerup
        Instantiate(speedUpPrefab, tilemapPath.CellToWorld(new Vector3Int(-1, -4)) + new Vector3(.25f, .125f, 0), Quaternion.Euler(new Vector3(0, 0, 35f)), speedUpTransform);
        
    }

    void GenerateCameraUpgrade()
    {
        Instantiate(
            cameraUpgradePrefab,
            pathTilePositions[Random.Range(0, pathTilePositions.Count)] + new Vector3(.125f, .125f, 0),
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            cameraUpgradeTransform
            );
    }

    void GenerateDisguisePowerup()
    {
        Instantiate(
            disguisePrefab,
            pathTilePositions[Random.Range(0, pathTilePositions.Count)] + new Vector3(.125f, .125f, 0),
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            disguiseTransform
            );
    }

    void Update()
    {
        if (goalParentTransform.childCount == 0)
        {
            zones.DeactivateZone();
            GenerateNewGoal();
        } 
        
        if (goalParentTransform.GetChild(0).GetComponent<SpriteRenderer>().enabled == false)
        {
            zones.DeactivateZone();
        }

        if (Time.time - speedUpTimer > 20)
        {
            if (speedUpTransform.childCount == 0)
            {
                GenerateSpeedPowerUp();
                speedUpTimer = Time.time;
            }
        }

        if (Time.time - cameraUpgradeTimer > 30)
        {
            if (cameraUpgradeTransform.childCount == 0)
            {
                GenerateCameraUpgrade();
                cameraUpgradeTimer = Time.time;
            }
        }

        if (Time.time - disguiseTimer > 30)
        {
            if (disguiseTransform.childCount == 0)
            {
                GenerateDisguisePowerup();
                disguiseTimer = Time.time;
            }
        }
    }
}
