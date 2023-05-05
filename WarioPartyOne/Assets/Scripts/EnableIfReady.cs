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
            celebDescription.SetActive(false);
        }
        if (gmScript.paparazziIsReady && !chibiStan.activeSelf)
        {
            chibiStan.SetActive(true);
            stanDescription.SetActive(false);
        }
    }
}
