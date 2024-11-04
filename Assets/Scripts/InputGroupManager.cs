using UnityEngine;
using UnityEngine.InputSystem;

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
