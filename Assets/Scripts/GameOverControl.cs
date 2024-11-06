using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// GameOverControl handles pausing the game and showing the "game over" UI elements when <c>OnGameOver<c> is called
/// </summary>
public class GameOverControl : MonoBehaviour
{
    public GameObject hudCanvas;
    public GameObject gameOverCanvas;
    
    public void OnGameOver()
    {
        Time.timeScale = 0.0f;
        
        hudCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
    }
}
