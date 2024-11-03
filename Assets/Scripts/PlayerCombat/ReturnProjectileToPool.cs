using UnityEngine;
using UnityEngine.Pool;

namespace PlayerCombat
{
    public class ReturnProjectileToPool : MonoBehaviour
    {
        public IObjectPool<PlayerProjectile> pool;
        public PlayerProjectile projectile; 
        
        private void OnCollisionEnter(Collision collision)
        {
            if(gameObject.activeSelf)
                pool.Release(projectile);
        }
    }
}