using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverControl : MonoBehaviour
{
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private GameObject gameOverCanvas;

    private InputActionMap playerMap;
    private InputActionMap uiMap;

    private void Start()
    {
        playerMap = InputSystem.actions.FindActionMap("Player");
        uiMap = InputSystem.actions.FindActionMap("UI");
    }

    public void OnGameOver()
    {
        Time.timeScale = 0.0f;
        
        playerMap.Disable();
        uiMap.Enable();
        
        hudCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
    }
}
