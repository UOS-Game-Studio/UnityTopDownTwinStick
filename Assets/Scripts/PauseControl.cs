using System;
using UnityEngine;
using UnityEngine.InputSystem;

// The "easiest" pause is to just set Time.timescale to 0.
// but that can cause us problems with all sorts of things in the background
// as not all functions use it to drive themselves.

// as such, we'll want to do something about changing that at some point.
public class PauseControl : MonoBehaviour
{
    public GameObject hudCanvas;
    public GameObject pauseMenu;

    private bool _isPaused;
    private InputAction _pauseAction;
    
    private void Start()
    {
        _pauseAction = InputSystem.actions.FindAction("Pause", true);

        _pauseAction.performed += IA_ActionPausePerformed;
    }

    private void PerformPause()
    {
        if (_isPaused)
        {
            hudCanvas.SetActive(true);
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            hudCanvas.SetActive(false);
            pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }

        _isPaused = !_isPaused;
    }

    private void OnDestroy()
    {
        // always reset this when the pause control is removed
        // if we're going to a different scene, we want to make sure it all just "works".
        Time.timeScale = 1.0f;
        _pauseAction.performed -= IA_ActionPausePerformed;
    }

    public void OnPauseToggle()
    {
        PerformPause();
    }
    
    private void IA_ActionPausePerformed(InputAction.CallbackContext context)
    {
        PerformPause();
    }

}
