using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerReady : MonoBehaviour
{
    private GameManagerScript gmScript;

    private void Awake()
    {
        gmScript = FindObjectOfType<GameManagerScript>();
    }

    public void Ready()
    {
        gmScript.PlayerIsReady(GetComponent<PlayerInput>());
    }

    public void NotReady()
    {
        gmScript.PlayerIsNotReady(GetComponent<PlayerInput>());
    }
}
