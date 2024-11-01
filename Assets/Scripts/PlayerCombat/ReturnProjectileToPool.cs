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
            pool.Release(projectile);
        }
    }
}