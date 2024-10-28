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

        Debug.Log("forwaed2d:"+forward2d+" Right2d:"+right2d+" Velocity:"+Velocity);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y).normalized, Vector3.up);
        characterRef.transform.rotation = lookRotation;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
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

    public void OnJump(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
