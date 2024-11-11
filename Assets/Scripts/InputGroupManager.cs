using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This is used to disable user input when the game is paused (or whenever else we need to).
/// It works by disabling the input group in the input system.
/// </summary>
public class InputGroupManager : MonoBehaviour
{
    private InputActionMap _playerMap;
    private InputActionMap _uiMap;
    
    
    private void Start()
    {
        PauseControl.OnPause.AddListener(PauseHandler);
        
        _playerMap = InputSystem.actions.FindActionMap("Player");
        _uiMap = InputSystem.actions.FindActionMap("UI");
        EnablePlayerControls();
    }

    private void PauseHandler(bool isPaused)
    {
        if (isPaused)
        {
            DisablePlayerControls();
        }
        else
        {
            EnablePlayerControls();
        }
    }

    public void DisablePlayerControls()
    {
        _playerMap.Disable();
        _uiMap.Enable();
    }

    public void EnablePlayerControls()
    {
        _playerMap.Enable();
        _uiMap.Disable();
    }
    
}
