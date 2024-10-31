using System;
using UnityEngine;

namespace PlayerCombat
{
    [RequireComponent(typeof(ReturnProjectileToPool))]
    public class PlayerProjectile : MonoBehaviour
    {
        public float moveSpeed;
        private Rigidbody _rb;

        private void OnEnable()
        {
            if(!_rb)
                _rb = GetComponent<Rigidbody>();
        }

        public void OnFire(Vector3 dir)
        {
            _rb.linearVelocity = dir * moveSpeed;
        }
    }
}