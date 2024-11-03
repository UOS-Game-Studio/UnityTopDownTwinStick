using System;
using UnityEngine;

namespace PlayerCombat
{
    [RequireComponent(typeof(ReturnProjectileToPool))]
    public class PlayerProjectile : MonoBehaviour
    {
        public float moveSpeed;
        private Rigidbody _rb;
        private float _damage;
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;

            Common.Health health = other.gameObject.GetComponent<Common.Health>();
            if (health == null) return;

            health.TakeDamage(_damage);
        }

        private void OnEnable()
        {
            if(!_rb)
                _rb = GetComponent<Rigidbody>();
        }

        public void OnFire(Vector3 dir, float damage)
        {
            _rb.linearVelocity = dir * moveSpeed;
            _damage = damage;
        }
    }
}