using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartPanelAnimation : MonoBehaviour
{
    private TextMeshProUGUI textbox;
    public GameObject buttonObject;

    private void Awake()
    {
        textbox = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Mathf.Sin(Time.time / 0.5f) >= 0)
        {
            if (!textbox.enabled) textbox.enabled = true;
            if (!buttonObject.activeInHierarchy) buttonObject.SetActive(true);
        } else
        {
            if (textbox.enabled) textbox.enabled = false;
            if (buttonObject.activeInHierarchy) buttonObject.SetActive(false);
        }
    }
}
