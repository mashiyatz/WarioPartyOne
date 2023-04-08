using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartPanelAnimation : MonoBehaviour
{
    private TextMeshProUGUI textbox;

    private void Awake()
    {
        textbox = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Mathf.Sin(Time.time / 0.5f) >= 0)
        {
            if (!textbox.enabled) textbox.enabled = true;
        } else
        {
            if (textbox.enabled) textbox.enabled = false;
        }
    }
}
