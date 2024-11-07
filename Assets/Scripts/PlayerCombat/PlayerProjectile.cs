using System;
using UnityEngine;

namespace PlayerCombat
{
    /// <summary>
    /// PlayerProjectile is attached to the projectile prefab used in <c>PlayerWeapon</c>
    /// <c>OnFire</c> is called to set the velocity of the projectile and assign a damage value (as that changes based on the damage gauge)
    /// </summary>
    [RequireComponent(typeof(ReturnProjectileToPool))]
    public class PlayerProjectile : MonoBehaviour
    {
        public float moveSpeed;
        private Rigidbody _rb;
        private float _damage;

        private void OnCollisionEnter(Collision other)
        {
            // as this is the PlayerProjectile, we only care about enemies - anything else is ignored.
            if (!other.gameObject.CompareTag("Enemy")) return;

            Common.Health health = other.gameObject.GetComponent<Common.Health>();
            if (!health) return;

            health.TakeDamage(_damage);
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void OnFire(Vector3 dir, float damage)
        {
            _rb.linearVelocity = dir * moveSpeed;
            _damage = damage;
        }
    }
}