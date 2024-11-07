using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PauseControl handles when the player hits a key bound to the Pause action in the Action Map.
/// The approach to pausing is not ideal; we set <c>Time.timescale</c> to 0 which works but causes issues
/// things like UI animations will also stop, which we may not want to happen just because we've paused the game.
/// </summary>
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

    // while most pausing takes place from the input interactions, we expose this to allow any other systems
    // in the game to trigger a pause if needs be.
    public void OnPauseToggle()
    {
        PerformPause();
    }
    
    private void IA_ActionPausePerformed(InputAction.CallbackContext context)
    {
        PerformPause();
    }

}
