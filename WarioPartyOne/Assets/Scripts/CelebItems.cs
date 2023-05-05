using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelebItems : MonoBehaviour
{
    public bool isHoldingItem;
    private Color startColor;
    private bool isVisible;

    void Start()
    {
        isVisible = true;
        isHoldingItem = false;
        startColor = GetComponent<SpriteRenderer>().color;
    }

    public bool CheckIfVisible()
    {
        return isVisible;
    }

    void StartDisguiseActivation()
    {
        StopCoroutine(ActivateDisguise());
        StartCoroutine(ActivateDisguise());
    }

    IEnumerator ActivateDisguise()
    {
        isVisible = false;
        GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(5.0f);
        isVisible = true;
        GetComponent<SpriteRenderer>().color = startColor;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isHoldingItem)
        {
            StartDisguiseActivation();
            isHoldingItem = false;
        }
    }
}
