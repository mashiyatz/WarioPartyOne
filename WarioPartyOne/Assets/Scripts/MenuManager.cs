using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame(){
        SceneManager.LoadScene("PlayScene");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.E))
        {
            StartGame();
        } 
    }
}