using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    Image spriteImage;
    SpriteRenderer spriteRenderer;
    public Color startColor;
    public Color endColor;

    void Start()
    {
        spriteImage = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        if (spriteImage != null) spriteImage.color = Color.Lerp(startColor, endColor, Mathf.Pow(Mathf.Sin(2 * Time.time), 2));
        if (spriteRenderer != null) spriteRenderer.color = Color.Lerp(startColor, endColor, Mathf.Pow(Mathf.Sin(2 * Time.time), 2));
    }
}
