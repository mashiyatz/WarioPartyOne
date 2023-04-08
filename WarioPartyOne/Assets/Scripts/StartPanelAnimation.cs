using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanelAnimation : MonoBehaviour
{
    public GameObject textbox;
    public static bool canMove;
    public GameObject winBox;

    void OnEnable()
    {
        canMove = false;
    }

    void Update()
    {
        if (winBox.activeSelf) return; 

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            canMove = true;
            gameObject.SetActive(false);
        }

        if (Mathf.Sin(Time.time / 0.5f) >= 0)
        {
            if (!textbox.activeSelf) textbox.SetActive(true);
        } else
        {
            if (textbox.activeSelf) textbox.SetActive(false);
        }
    }
}
