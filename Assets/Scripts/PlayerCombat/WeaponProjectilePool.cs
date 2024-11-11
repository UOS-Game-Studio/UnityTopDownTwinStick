using System;
using UnityEngine;
using UnityEngine.Pool;

namespace PlayerCombat
{
    /// <summary>
    /// For objects that have short life times and where there are potentially lots of them being used,
    /// we want to avoid instantiating and destroying too often. A pool lets us avoid that by storing a set of objects
    /// that are instantiated ahead of time and then activated and used, when they "die" we deactivate them and they go
    /// back into the pool.
    /// </summary>
    public class WeaponProjectilePool : MonoBehaviour
    {
        public GameObject projectilePrefab;
        
        // IObjectPool is from Unity, while it's relatively easy to write our own, this works and does what we want
        private IObjectPool<PlayerProjectile> _pool;

        public bool collectionChecks = true;
        public int maxPoolSize = 10;

        // The Pool property allows access to the object pool variable, but we cannot overwrite it externally
        // notice the "get" block but there's no matching "set" (ie, get is public and set is private).
        public IObjectPool<PlayerProjectile> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new ObjectPool<PlayerProjectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                        OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                }

                return _pool;
            }
        }

        private void OnDestroy()
        {
            // the ?. thing here is a null conditional operator; if _pool is null it won't try and call Clear
            // Unity does some strange things for GameObjects and Components where we can't really use this, so be careful!
            // in PlayerProjectile.OnCollisionEnter you can see an if statement checking for a health component instead of this operator for just that reason.
            _pool?.Clear();
        }

        // if the pool is full when an object is returned to it, it gets destroyed instead
        private void OnDestroyPoolObject(PlayerProjectile obj)
        {
            Destroy(obj);
        }
        
        private void OnReturnedToPool(PlayerProjectile proj)
        {
            proj.gameObject.SetActive(false);
        }

        private void OnTakeFromPool(PlayerProjectile proj)
        {
            proj.gameObject.SetActive(true);
        }

        // if we ask for an object from the pool and there are not any available, we create one.
        PlayerProjectile CreatePooledItem()
        {
            GameObject projectile = Instantiate(projectilePrefab);
            PlayerProjectile playerProj = projectile.GetComponent<PlayerProjectile>();
            ReturnProjectileToPool returnToPool = projectile.GetComponent<ReturnProjectileToPool>();
            
            returnToPool.pool = Pool;
            returnToPool.projectile = playerProj;
            return playerProj;
        }
    }
}