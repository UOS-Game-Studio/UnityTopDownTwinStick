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
        _playerMap = InputSystem.actions.FindActionMap("Player");
        _uiMap = InputSystem.actions.FindActionMap("UI");
    }

    public void DisablePlayerControls()
    {
        _playerMap.Disable();
    }

    public void EnablePlayerControls()
    {
        _playerMap.Enable();
    }
    
}
