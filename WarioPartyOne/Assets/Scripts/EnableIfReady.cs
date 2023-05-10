using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIfReady : MonoBehaviour
{
    public GameManagerScript gmScript;
    public GameObject chibiCeleb;
    public GameObject celebDescription;
    public GameObject chibiStan;
    public GameObject stanDescription;

    void Update()
    {
        if (gmScript.celebrityIsReady && !chibiCeleb.activeSelf)
        {
            chibiCeleb.SetActive(true);
        }
        else if (!gmScript.celebrityIsReady) chibiCeleb.SetActive(false);
        if (gmScript.paparazziIsReady && !chibiStan.activeSelf)
        {
            chibiStan.SetActive(true);
        }
        else if (!gmScript.paparazziIsReady) chibiStan.SetActive(false);
    }
}
