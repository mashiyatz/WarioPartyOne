using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCars : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public float angleAxis;

    void Update()
    {
        if (transform.childCount == 0)
        {
            Instantiate(
                carPrefabs[Random.Range(0, carPrefabs.Length)], 
                transform.position,
                Quaternion.AngleAxis(angleAxis, Vector3.forward),
                transform
            );
        }           
    }
}
