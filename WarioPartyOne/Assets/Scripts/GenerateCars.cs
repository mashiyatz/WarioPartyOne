using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCars : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public float angleAxis;
    private bool isGeneratingCar;
    public static float delay;

    private void Start()
    {
        delay = 0.5f;
        isGeneratingCar = false;
    }

    void Update()
    {
        if (transform.childCount == 0 && !isGeneratingCar)
        {
            isGeneratingCar = true;
            StartCoroutine(GenerateNewCar());
        }           
    }

    IEnumerator GenerateNewCar()
    {
        Instantiate(
                carPrefabs[Random.Range(0, carPrefabs.Length)],
                transform.position,
                Quaternion.AngleAxis(angleAxis, Vector3.forward),
                transform
            );
        yield return new WaitForSeconds(delay);
        isGeneratingCar = false;
    }
}
