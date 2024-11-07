using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class S_PlayerMovment : MonoBehaviour
    {
        //[SerializeField] private float speed = 1.0f;
        private Vector2 Velocity = new();
        private Animator anim;

        public Transform aimPointObject;

        public bool shouldStopMovement;
        
        private Vector3 _aimPosition = Vector3.zero;
        private Transform _characterTransform;
        private Camera _mainCamera;

        private const float DirOffset = 2.0f;
        private const float MinDistance = 0.4f;
        
        private static readonly int Roll = Animator.StringToHash("Roll");
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");

        private bool _isPaused;
        
        public void Start()
        {
            anim = gameObject.GetComponentInChildren<Animator>();
            _mainCamera = Camera.main;
            _characterTransform = transform;
            PauseControl.OnPause.AddListener(PauseHandler);
        }

        private void PauseHandler(bool isPaused)
        {
            anim.enabled = !isPaused;
            _isPaused = isPaused;
        }
        
        public void Update()
        {
            if (_isPaused) return;
            
            float distance = Vector3.Distance(transform.position, _aimPosition);
            if (distance <= MinDistance)
            {
                if (shouldStopMovement)
                {
                    Velocity = Vector2.zero;
                }
                else
                {
                    // this approach leads to some weird behaviour once you move "past" where the aim point is.
                    _aimPosition = transform.position + (transform.forward * 2.0f);
                }
            }
            
            anim.SetFloat(VelocityX, Velocity.x);
            anim.SetFloat(VelocityZ, Velocity.y);
            _characterTransform.LookAt(_aimPosition);
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputValue = context.ReadValue<Vector2>();
            
            Vector2 characterForward = new Vector2(_characterTransform.forward.x, _characterTransform.forward.z);
            Vector2 characterRight = new Vector2(_characterTransform.right.x, _characterTransform.right.z);
            Velocity = inputValue.normalized;
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
