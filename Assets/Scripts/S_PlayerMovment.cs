using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class S_PlayerMovment: MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private float speed = 1.0f;
    private Vector2 Velocity = new();
    private Animator anim;
    private GameObject characterRef;

    public void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        characterRef = gameObject.transform.GetChild(0).gameObject;
        
    }

    public void Update()
    {


        anim.SetFloat("VelocityX", Velocity.x);
        anim.SetFloat("VelocityZ", Velocity.y);
        
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 forward2d = new Vector2(characterRef.transform.forward.x*-1, characterRef.transform.forward.z);
        Vector2 right2d = new Vector2(characterRef.transform.right.x, characterRef.transform.right.z*-1);
        Velocity = context.ReadValue<Vector2>().y * forward2d+ context.ReadValue<Vector2>().x * right2d;

        //Debug.Log("forwaed2d:"+forward2d+" Right2d:"+right2d+" Velocity:"+Velocity);
    }

    // Rotate uses GamePad stick or Keyboard
    public void OnRotate(InputAction.CallbackContext context)
    {
        Debug.Log("OnRotate");
        Vector2 direction = context.ReadValue<Vector2>();
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y).normalized, Vector3.up);
        characterRef.transform.rotation = lookRotation;
    }

    // Aim follows the mouse cursor
    public void OnAim(InputAction.CallbackContext context)
    {
        Debug.Log("OnAim");
        Vector2 direction = context.ReadValue<Vector2>();

        direction = Camera.main.ScreenToViewportPoint(direction);
        Debug.Log("pointer pos: " + direction);
        
        Vector3 lookDirection = new Vector3(direction.x, 0f, direction.y);
        var lookRotation = Camera.main.transform.TransformDirection(lookDirection);
        lookRotation = Vector3.ProjectOnPlane(lookRotation, Vector3.up);

        if (lookRotation != Vector3.zero)
        {
            //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y).normalized, Vector3.up);
            Quaternion newRotation = Quaternion.LookRotation(lookRotation);
            characterRef.transform.rotation = newRotation;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
 
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            anim.SetTrigger("Roll");
        }
    }
}
