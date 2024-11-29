using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(SphereCollider), typeof(CapsuleCollider))]
    public class RollControl : MonoBehaviour
    {
        private CapsuleCollider _capsule;
        private SphereCollider _sphere;
        private InputAction _rollAction;
        private void Start()
        {
            _capsule = GetComponent<CapsuleCollider>();
            _sphere = GetComponent<SphereCollider>();
            
            _rollAction = InputSystem.actions.FindAction("Roll", true);
            _rollAction.performed += OnRollStart;
        }

        private void OnRollStart(InputAction.CallbackContext obj)
        {
            _capsule.enabled = false;
            _sphere.enabled = true;
        }

        public void OnRollEnd()
        {
            _capsule.enabled = true;
            _sphere.enabled = false;
        }
    }
}
