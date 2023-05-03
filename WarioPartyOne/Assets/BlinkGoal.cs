using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkGoal : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color fullOpacity;
    private Color noOpacity;
    private float freq;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fullOpacity = Color.white;
        noOpacity = new Color(1, 1, 1, 0);
        freq = 1;
        // Debug.Log($"starting freq: {freq}");
        StartCoroutine(BlinkSprite());
    }

    /*void Update()
    {
        if (spriteRenderer.enabled)
        {
            spriteRenderer.color = Color.Lerp(
                fullOpacity,
                noOpacity,
                Mathf.Pow(Mathf.Sin(Time.time * freq), 2)
            );

            freq += Time.deltaTime / 5;
        }
        // Debug.Log($"freq: {freq}");
    }*/

    IEnumerator BlinkSprite()
    {
        for (float i = 0; i < 15; i+=Time.deltaTime)
        {
            spriteRenderer.color = Color.Lerp(
                fullOpacity,
                noOpacity,
                Mathf.Pow(Mathf.Sin(i * freq), 2)
            );
            freq += i;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
