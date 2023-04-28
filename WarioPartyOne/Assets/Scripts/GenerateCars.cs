using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCars : MonoBehaviour
{
    public GameObject carPrefab;
    public float angleAxis;

    void Update()
    {
        if (transform.childCount == 0)
        {
            Instantiate(
                carPrefab, 
                transform.position,
                Quaternion.AngleAxis(angleAxis, Vector3.forward),
                transform
            );
        }           
    }
}
