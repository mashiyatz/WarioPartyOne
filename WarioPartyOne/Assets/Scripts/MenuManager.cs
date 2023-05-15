using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI textbox;

    public void StartGame(){
        SceneManager.LoadScene("PlayScene");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.E))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Q))
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
