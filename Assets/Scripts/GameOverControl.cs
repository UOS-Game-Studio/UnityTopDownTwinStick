using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
