using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisionCone : MonoBehaviour
{
    public PlayerManager paparazziPM;
    private SpriteRenderer spriteRenderer;
    public ActionButton actionCone;

    public Color restColor;
    public Color inRangeColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        /*if (isActiveAndEnabled)
        {
            if (paparazziPM.resources > 0)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }*/

        if (spriteRenderer.enabled)
        {
            if (actionCone.CheckIfInRange())
            {
                spriteRenderer.color = inRangeColor;
            }
            else
            {
                spriteRenderer.color = restColor;
            }
        }
    }
}
