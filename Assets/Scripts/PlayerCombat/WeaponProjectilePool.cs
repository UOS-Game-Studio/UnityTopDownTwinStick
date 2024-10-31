using System;
using UnityEngine;
using UnityEngine.Pool;

namespace PlayerCombat
{
    public class WeaponProjectilePool : MonoBehaviour
    {
        public GameObject projectilePrefab;
        private IObjectPool<PlayerProjectile> _pool;

        public bool collectionChecks = true;
        public int maxPoolSize = 10;

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