using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielKiller : MonoBehaviour
{
    public void DestroyDaniel()
    {
        if (gameObject.CompareTag("Paparazzi"))
        {
            if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
            {
                GameObject daniel = GameObject.FindGameObjectWithTag("Celebrity");
                Destroy(daniel);
            }
        } else if (gameObject.CompareTag("Celebrity"))
        {
            if (Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.O))
            {
                GameObject daniel = GameObject.FindGameObjectWithTag("Paparazzi");
                Destroy(daniel);
            }
         }
    }
}
