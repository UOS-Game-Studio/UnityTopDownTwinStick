using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class S_PlayerMovment: MonoBehaviour
{
    //[SerializeField] private float speed = 1.0f;
    private Vector2 Velocity = new();
    private Animator anim;
    private GameObject characterRef;

    public Transform aimPointObject;

    private Vector3 _aimPosition = Vector3.zero;
    private Transform _characterTransform;
    private Camera _mainCamera;
    
    private static readonly int Roll = Animator.StringToHash("Roll");
    private static readonly int VelocityX = Animator.StringToHash("VelocityX");
    private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");

    public void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        characterRef = gameObject.transform.GetChild(0).gameObject;
        _mainCamera = Camera.main;
        _characterTransform = characterRef.transform;
    }

    public void Update()
    {
        anim.SetFloat(VelocityX, Velocity.x);
        anim.SetFloat(VelocityZ, Velocity.y);
        _characterTransform.LookAt(_aimPosition);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        var characterForward = _characterTransform.forward;
        var characterRight = _characterTransform.right;
        Vector2 forward2d = new Vector2(characterForward.x*-1, characterForward.z);
        Vector2 right2d = new Vector2(characterRight.x, characterRight.z*-1);
        Velocity = context.ReadValue<Vector2>().y * forward2d+ context.ReadValue<Vector2>().x * right2d;

        //Debug.Log("forwaed2d:"+forward2d+" Right2d:"+right2d+" Velocity:"+Velocity);
    }

    // Rotate uses GamePad stick or Keyboard
    public void OnRotate(InputAction.CallbackContext context)
    {
        //Debug.Log("OnRotate");
        Vector2 direction = context.ReadValue<Vector2>();

        if (direction.magnitude <= 0.2f) return;
        
        const float dirOffset = 2.0f;
        
        Debug.Log(direction);
        Vector3 offsetPosition = _characterTransform.position + new Vector3(direction.x* dirOffset, 0.0f, direction.y* dirOffset);
        
        Vector3 aimPosition = new Vector3((offsetPosition.x + direction.x) * dirOffset, aimPointObject.position.y, (offsetPosition.z + direction.y) * dirOffset);
        aimPointObject.position = offsetPosition;
        _aimPosition = offsetPosition;
        //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y).normalized, Vector3.up);

    }

    // Aim follows the mouse cursor
    public void OnAim(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 worldMouse = _mainCamera.ScreenToWorldPoint(new Vector3(input.x, input.y, 10.0f));
        Vector3 worldMouseNoY = new Vector3(worldMouse.x, _characterTransform.position.y, worldMouse.z);
        
        aimPointObject.position = new Vector3(worldMouse.x, aimPointObject.position.y, worldMouse.z);

        if (worldMouseNoY != Vector3.zero)
            _aimPosition = worldMouseNoY;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            anim.SetTrigger(Roll);
        }
    }
}
