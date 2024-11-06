using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerCombat
{
    /// <summary>
    /// PlayerWeapon handles all things that relate to well, the players weapon.
    /// base stats are pulled in from the WeaponStats ScriptableObject
    /// </summary>
    
    // REVIEW: Do we rename this to just "Weapon"? It's in the PlayerCombat namespace, so it can't clash with anything else?
    public class PlayerWeapon : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public WeaponStats stats;
        
        private InputAction _fireAction;
        private WeaponProjectilePool _projectilePool;
        private bool _isFiring = false;
        private bool _canFire = true;

        private float _lastFire;
        
        private WaitForSeconds _fireDelay;
        
        void Start()
        {
            // By adding the component here, we guarantee it never gets "forgotten"
            // we could do the same thing by adding a [RequireComponent(typeof(WeaponProjectilePool)] attribute
            // above the class declaration and then doing _projectilePool = GetComponent<WeaponProjectilePool>(); here instead
            _projectilePool = gameObject.AddComponent<WeaponProjectilePool>();
            _projectilePool.projectilePrefab = projectilePrefab;
        
            // the player weapon only cares about the attack action, so we just need that from the full set of input actions
            // this way the weapon script does not need to know anything else about how Input is handled anywhere else in the game
            _fireAction = InputSystem.actions.FindAction("Attack", true);
            
            // We track when the attack action starts and when it ends (so is canceled).
            // these are events that the input system "fires"; we're adding callbacks that get called when the event fires.
            _fireAction.started += IA_FireActionOnStarted;
            _fireAction.canceled += IA_FireActionOnCanceled;

            // cache the WaitForSeconds object as the fire rate does not change as we play.
            _fireDelay = new WaitForSeconds(stats.fireDelay);
        }
        
        private void IA_FireActionOnCanceled(InputAction.CallbackContext obj)
        {
            _isFiring = false;
            _canFire = false;
        }

        private void IA_FireActionOnStarted(InputAction.CallbackContext obj)
        {
            // a guaranteed way of resetting _canFire if the last shot was fired long enough ago but _canFire is still false.
            if (Time.time < _lastFire + stats.fireDelay && !_canFire)
                _canFire = true;
            
            // if we're still waiting for a previous shot to cool down, we can't fire again
            if (!_canFire) return;
            
            _isFiring = true;
            StartCoroutine(FiringRoutine());
        }

        private void OnDestroy()
        {
            _fireAction.started -= IA_FireActionOnStarted;
            _fireAction.canceled -= IA_FireActionOnCanceled;
        }

        private void OnDisable()
        {
            _canFire = false;
            StopCoroutine(FiringRoutine());
        }

        public float GetCurrentDamage()
        {
            return stats.baseDamage + stats.adjustedValue;
        }
        
        // We do this in a coroutine as that gives us straightforward control over
        // how often we fire a projectile, it could be done in Update but the timing
        // code becomes a little more complicated.
        private IEnumerator FiringRoutine()
        {
            while (_isFiring)
            {
                // retrieve a projectile from the object pool, move it to the fire point and set it moving
                PlayerProjectile projectileObject = _projectilePool.Pool.Get();
                
                // SetPositionAndRotation is a more efficient way of doing this than setting position and rotation properties
                // individually. https://docs.unity3d.com/ScriptReference/Transform.SetPositionAndRotation.html
                projectileObject.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                projectileObject.OnFire(transform.forward, GetCurrentDamage());
                _lastFire = Time.time;
                yield return _fireDelay;
            }
            
            _canFire = true;
        }
    }
}


