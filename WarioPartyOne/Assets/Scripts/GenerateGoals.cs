using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateGoals : MonoBehaviour
{
    public GameObject goalPrefab;
/*    public GridLayout grid;
    public Tilemap tilemap;*/

    void Start()
    {
        GenerateNewGoal(); 
    }

    void GenerateNewGoal()
    {
        float xPos;
        float yPos;

        xPos = Random.Range(-5, 5) + 0.5f;
        yPos = Random.Range(-5, 5) + 0.5f;

        /*Vector3Int pos = grid.WorldToCell(new Vector3(xPos, yPos, 0));
        if (tilemap.GetTile(pos).name == "black") Debug.Log("hihi");*/

        Instantiate(goalPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, transform);
    }

    void Update()
    {
        if (transform.childCount == 0)
        {
            GenerateNewGoal();
        }
    }
}
