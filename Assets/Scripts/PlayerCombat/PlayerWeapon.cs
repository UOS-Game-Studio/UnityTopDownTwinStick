using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerCombat
{
    public class PlayerWeapon : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public WeaponStats stats;
    
        private InputAction _fireAction;
        private WeaponProjectilePool _projectilePool;
        private bool _isFiring = false;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _projectilePool = gameObject.AddComponent<WeaponProjectilePool>();
            _projectilePool.projectilePrefab = projectilePrefab;
        
            _fireAction = InputSystem.actions.FindAction("Attack", true);
            _fireAction.started += IA_FireActionOnStarted;
            _fireAction.canceled += IA_FireActionOnCanceled;
        }

        private void IA_FireActionOnCanceled(InputAction.CallbackContext obj)
        {
            _isFiring = false;
        }

        private void IA_FireActionOnStarted(InputAction.CallbackContext obj)
        {
            _isFiring = true;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}


