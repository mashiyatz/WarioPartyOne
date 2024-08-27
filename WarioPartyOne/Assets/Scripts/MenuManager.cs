using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem.HID;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI textbox;

    [SerializeField]
    private bool useKeyboard;

    [SerializeField]
    private GameObject warningPanel;
    private float pressTime;

    [SerializeField]
    private float waitTime;

    public void StartGame(){
        SceneManager.LoadScene("PlayScene");
    }

    public void PowerUpButtonPressed(InputAction.CallbackContext context)
    {
        if (context.started) pressTime = Time.time;
        else if (context.canceled)
        {
            if (Time.time - pressTime >= waitTime)
            {
                if (!textbox.gameObject.activeInHierarchy)
                {
                    textbox.gameObject.SetActive(true);
                    textbox.text = $"idol: {PlayerPrefs.GetInt("idolWins", 0)} stan: {PlayerPrefs.GetInt("stanWins", 0)}";
                }
                else
                {
                    textbox.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ScoreButtonPressAndRelease(InputAction.CallbackContext context)
    {
        if (warningPanel.activeSelf) return;
        if (context.ReadValueAsButton()) StartGame();
    }

    private void Update()
    {
        if (InputSystem.devices.OfType<Gamepad>().Count() + InputSystem.devices.OfType<Joystick>().Count() < 2 && !warningPanel.activeSelf)
        {
            Debug.Log(InputSystem.devices.OfType<Gamepad>().Count() + InputSystem.devices.OfType<Joystick>().Count());
            warningPanel.SetActive(true);
        } else
        {
            warningPanel.SetActive(false);
        }
    }
}
