using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        yield return new WaitForSeconds(ValueSettings.disguiseBuffTime);
        isVisible = true;
        GetComponent<SpriteRenderer>().color = startColor;
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        if (PlayerInput.FindFirstPairedToDevice(context.control.device) != GetComponent<PlayerInput>()) return;
        if (context.ReadValueAsButton() && isHoldingItem)
        {
            StartDisguiseActivation();
            isHoldingItem = false;
        }
    }

    void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.Q) && isHoldingItem)
        {
            StartDisguiseActivation();
            isHoldingItem = false;
        }*/
    }
}
