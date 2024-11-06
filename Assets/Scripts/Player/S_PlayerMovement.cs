using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using static UnityEngine.Rendering.DebugUI;

namespace Player
{
    public class S_PlayerMovment : MonoBehaviour
    {
        //[SerializeField] private float speed = 1.0f;
        private Vector2 Velocity = new();
        private Animator anim;
        private GameObject characterRef;

        public Transform aimPointObject;

        private Vector3 _aimPosition = Vector3.zero;
        private Transform _characterTransform;
        private Camera _mainCamera;

        private const float DirOffset = 2.0f;
        
        private static readonly int Roll = Animator.StringToHash("Roll");
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");

        public void Start()
        {
            anim = gameObject.GetComponentInChildren<Animator>();
            characterRef = gameObject.transform.GetChild(0).gameObject;
            _mainCamera = Camera.main;
            _characterTransform = transform;
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
            Vector2 forward2d = new Vector2(characterForward.x * -1, characterForward.z);
            Vector2 right2d = new Vector2(characterRight.x, characterRight.z * -1);
            Velocity = context.ReadValue<Vector2>().y * forward2d + context.ReadValue<Vector2>().x * right2d;

            //Debug.Log("forwaed2d:"+forward2d+" Right2d:"+right2d+" Velocity:"+Velocity);
        }

        // Rotate uses GamePad stick or Keyboard
        public void OnRotate(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();

            // gives us a bit of a dead zone on the stick
            if (direction.magnitude <= 0.2f) return;
            
            Vector3 normalizedDir = new Vector3(direction.x, 0.0f, direction.y).normalized;
            
            Vector3 offsetPosition = _characterTransform.position + normalizedDir * DirOffset;
            Vector3 aimPosition = new Vector3(offsetPosition.x, aimPointObject.position.y, offsetPosition.z);
            
            aimPointObject.position = aimPosition;
            _aimPosition = offsetPosition;
        }
        
        // Aim follows the mouse cursor
        public void OnAim(InputAction.CallbackContext context)
        {
            if (Time.timeScale == 0) return;
            
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 worldMouse = _mainCamera.ScreenToWorldPoint(new Vector3(input.x, input.y, 10.0f));
            Vector3 worldMouseNoY = new Vector3(worldMouse.x, _characterTransform.position.y, worldMouse.z);
            
            Vector3 direction = (worldMouseNoY - _characterTransform.position).normalized * DirOffset;
            Vector3 offsetPosition = _characterTransform.position + direction;
            
            Vector3 aimPosition = new Vector3(offsetPosition.x, aimPointObject.position.y,
                offsetPosition.z);
            aimPointObject.position = aimPosition;

            if (worldMouseNoY != Vector3.zero)
                _aimPosition = worldMouseNoY;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                anim.SetTrigger(Roll);
            }
        }
    }
}
