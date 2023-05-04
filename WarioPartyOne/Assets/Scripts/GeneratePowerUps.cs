using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneratePowerUps : MonoBehaviour
{
    //public Transform paparazziPowerUpTransform;
    public Transform batteryTransform;
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

    private bool isGeneratingSpeed;
    private bool isGeneratingCamera;
    private bool isGeneratingDisguise;
    private bool isGeneratingBattery;

    // private Vector3Int[] occupiedSpots;

    void Start()
    {
        pathTilePositions = new List<Vector3>();
        GetTilePositions();
        InitializePowerUps();
    }

    void InitializePowerUps()
    {
        Instantiate(speedUpPrefab, tilemapPath.GetCellCenterWorld(new Vector3Int(4, 0)), Quaternion.Euler(new Vector3(0, 0, 35f)), speedUpTransform);
        Instantiate(speedUpPrefab, tilemapPath.GetCellCenterWorld(new Vector3Int(-5, 0)), Quaternion.Euler(new Vector3(0, 0, 35f)), speedUpTransform);
        Instantiate(
            batteryPrefab,
            pathTilePositions[Random.Range(0, pathTilePositions.Count)],
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            batteryTransform
        );
        Instantiate(
            cameraUpgradePrefab,
            pathTilePositions[Random.Range(0, pathTilePositions.Count)],
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            cameraUpgradeTransform
            );
        Instantiate(
            disguisePrefab,
            pathTilePositions[Random.Range(0, pathTilePositions.Count)],
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            disguiseTransform
            );

        isGeneratingSpeed = false;
        isGeneratingCamera = false;
        isGeneratingDisguise = false;
        isGeneratingBattery = false;
    }

    void GetTilePositions()
    {
        foreach (Vector3Int pos in tilemapPath.cellBounds.allPositionsWithin)
        {
            Vector3 worldPlace = tilemapPath.GetCellCenterWorld(pos);
            if (tilemapPath.HasTile(pos) && !tilemapObstacle.HasTile(pos))
            {
                pathTilePositions.Add(worldPlace);
            }
        }
    }

    IEnumerator GenerateBattery()
    {
        yield return new WaitForSeconds(10.0f);
        Instantiate(
            batteryPrefab,
            pathTilePositions[Random.Range(0, pathTilePositions.Count)],
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            batteryTransform
        );
        isGeneratingBattery = false;

    }

    IEnumerator GenerateSpeedPowerUp()
    {
        yield return new WaitForSeconds(20.0f);
        Instantiate(speedUpPrefab, tilemapPath.GetCellCenterWorld(new Vector3Int(4, 0)), Quaternion.Euler(new Vector3(0, 0, 35f)), speedUpTransform);
        Instantiate(speedUpPrefab, tilemapPath.GetCellCenterWorld(new Vector3Int(-5, 0)), Quaternion.Euler(new Vector3(0, 0, 35f)), speedUpTransform);
        isGeneratingSpeed = false;
    }

    IEnumerator GenerateCameraUpgrade()
    {
        yield return new WaitForSeconds(30.0f);
        Instantiate(
            cameraUpgradePrefab,
            pathTilePositions[Random.Range(0, pathTilePositions.Count)],
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            cameraUpgradeTransform
            );
        isGeneratingCamera = false;
    }

    IEnumerator GenerateDisguisePowerup()
    {
        yield return new WaitForSeconds(30.0f);
        Instantiate(
            disguisePrefab,
            pathTilePositions[Random.Range(0, pathTilePositions.Count)],
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            disguiseTransform
            );
        isGeneratingDisguise = false;
    }

    void Update()
    {
        if (batteryTransform.childCount == 0)
        {
            if (!isGeneratingBattery) StartCoroutine(GenerateBattery());
            isGeneratingBattery = true;
        }


        if (speedUpTransform.childCount == 0)
        {
            if (!isGeneratingSpeed) StartCoroutine(GenerateSpeedPowerUp());
            isGeneratingSpeed = true;
        }

        if (cameraUpgradeTransform.childCount == 0)
        {
            if (!isGeneratingCamera) StartCoroutine(GenerateCameraUpgrade());
            isGeneratingCamera = true;
        }

        if (speedUpTransform.childCount == 0)
        {
            if (!isGeneratingDisguise) StartCoroutine(GenerateDisguisePowerup());
            isGeneratingDisguise = true;
        }
    }
}
