using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateAtSpeed : MonoBehaviour
{
    private Image logo;

    void Start()
    {
        logo = GetComponent<Image>();    
    }

    void Update()
    {
        logo.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), Mathf.Pow(Mathf.Sin(Time.time), 2));
        // transform.Rotate(0, 0, 60 * Time.deltaTime);
    }
}
