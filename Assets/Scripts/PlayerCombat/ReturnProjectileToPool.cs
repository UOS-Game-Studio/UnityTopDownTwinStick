using UnityEngine;
using UnityEngine.Pool;

namespace PlayerCombat
{
    /// <summary>
    /// ReturnProjectileToPool is a component added to the projectile prefab
    /// it handles telling the pool that the projectile is ready for use again
    /// </summary>
    public class ReturnProjectileToPool : MonoBehaviour
    {
        public IObjectPool<PlayerProjectile> pool;
        public PlayerProjectile projectile; 
        
        private void OnCollisionEnter(Collision collision)
        {
            // we occasionally find ourselves in a position where the gameObject has hit 2 
            // things in short succession and so has already been released to the pool.
            if(gameObject.activeSelf)
                pool.Release(projectile);
        }
    }
}