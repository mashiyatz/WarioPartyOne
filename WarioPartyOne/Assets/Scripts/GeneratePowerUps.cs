using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

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

    private List<int> occupiedIndexList;

    private Vector3Int[] initialPowerUpPositions;
    private int[] powerUpIndexArray; 

    void Start()
    {
        pathTilePositions = new List<Vector3>();
        occupiedIndexList = new List<int>();
        powerUpIndexArray = new int[2]; // lens, disguise
        
        initialPowerUpPositions = new Vector3Int[6];
        initialPowerUpPositions[0] = new Vector3Int(3, 0);
        initialPowerUpPositions[1] = new Vector3Int(-4, 0);
        initialPowerUpPositions[2] = new Vector3Int(-12, -4);
        initialPowerUpPositions[3] = new Vector3Int(11, 4);
        initialPowerUpPositions[4] = new Vector3Int(-11, 7);
        initialPowerUpPositions[5] = new Vector3Int(10, -7);

        GetTilePositions();
        InitializePowerUps();

    }

    void GetTilePositions()
    {
        foreach (Vector3Int pos in tilemapPath.cellBounds.allPositionsWithin)
        {
            Vector3 worldPlace = tilemapPath.GetCellCenterWorld(pos);
            if (tilemapPath.HasTile(pos) && !tilemapObstacle.HasTile(pos))
            {
                // if (pos != initialPowerUpPositions[0] && pos!= initialPowerUpPositions[1]) pathTilePositions.Add(worldPlace);
                pathTilePositions.Add(worldPlace);
                if (initialPowerUpPositions.Contains<Vector3Int>(pos)) occupiedIndexList.Add(pathTilePositions.Count - 1);
            }
        }
    }

    void InitializePowerUps()
    {
        Instantiate(speedUpPrefab, tilemapPath.GetCellCenterWorld(initialPowerUpPositions[0]), Quaternion.Euler(new Vector3(0, 0, 35f)), speedUpTransform);
        Instantiate(speedUpPrefab, tilemapPath.GetCellCenterWorld(initialPowerUpPositions[1]), Quaternion.Euler(new Vector3(0, 0, 35f)), speedUpTransform);

        Instantiate(batteryPrefab, tilemapPath.GetCellCenterWorld(initialPowerUpPositions[2]), Quaternion.Euler(new Vector3(0, 0, 35f)), batteryTransform);
        Instantiate(batteryPrefab, tilemapPath.GetCellCenterWorld(initialPowerUpPositions[3]), Quaternion.Euler(new Vector3(0, 0, 35f)), batteryTransform);


        Instantiate(
            cameraUpgradePrefab,
            tilemapPath.GetCellCenterWorld(initialPowerUpPositions[4]),
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            cameraUpgradeTransform
            );
        Instantiate(
            disguisePrefab,
            tilemapPath.GetCellCenterWorld(initialPowerUpPositions[5]),
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            disguiseTransform
            );

        isGeneratingSpeed = false;
        isGeneratingCamera = false;
        isGeneratingDisguise = false;
        isGeneratingBattery = false;
    }

    private int GetUnoccupiedIndex()
    {
        int randomIndex = Random.Range(0, pathTilePositions.Count);
        if (occupiedIndexList.Contains(randomIndex)) return GetUnoccupiedIndex();
        else return randomIndex;
    }

    IEnumerator GenerateBattery()
    {
        yield return new WaitForSeconds(ValueSettings.batteryGenerateWaitTime);
        Instantiate(batteryPrefab, tilemapPath.GetCellCenterWorld(initialPowerUpPositions[2]), Quaternion.Euler(new Vector3(0, 0, 35f)), batteryTransform);
        Instantiate(batteryPrefab, tilemapPath.GetCellCenterWorld(initialPowerUpPositions[3]), Quaternion.Euler(new Vector3(0, 0, 35f)), batteryTransform);
        isGeneratingBattery = false;

    }

    IEnumerator GenerateSpeedPowerUp()
    {
        yield return new WaitForSeconds(ValueSettings.speedGenerateWaitTime);
        Instantiate(speedUpPrefab, tilemapPath.GetCellCenterWorld(initialPowerUpPositions[0]), Quaternion.Euler(new Vector3(0, 0, 35f)), speedUpTransform);
        Instantiate(speedUpPrefab, tilemapPath.GetCellCenterWorld(initialPowerUpPositions[1]), Quaternion.Euler(new Vector3(0, 0, 35f)), speedUpTransform);
        isGeneratingSpeed = false;
    }

    IEnumerator GenerateCameraUpgrade()
    {
        occupiedIndexList.Remove(powerUpIndexArray[0]);
        yield return new WaitForSeconds(ValueSettings.lensGenerateWaitTime);
        powerUpIndexArray[0] = GetUnoccupiedIndex();
        Instantiate(
            cameraUpgradePrefab,
            // pathTilePositions[Random.Range(0, pathTilePositions.Count)],
            pathTilePositions[powerUpIndexArray[0]],
            Quaternion.Euler(new Vector3(0, 0, 35f)),
            cameraUpgradeTransform
            );
        isGeneratingCamera = false;
    }

    IEnumerator GenerateDisguisePowerup()
    {
        occupiedIndexList.Remove(powerUpIndexArray[1]);
        yield return new WaitForSeconds(ValueSettings.disguiseGenerateWaitTime);
        powerUpIndexArray[1] = GetUnoccupiedIndex();
        Instantiate(
            disguisePrefab,
            // pathTilePositions[Random.Range(0, pathTilePositions.Count)],
            pathTilePositions[powerUpIndexArray[1]],
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

        if (disguiseTransform.childCount == 0)
        {
            if (!isGeneratingDisguise) StartCoroutine(GenerateDisguisePowerup());
            isGeneratingDisguise = true;
        }
    }
}
