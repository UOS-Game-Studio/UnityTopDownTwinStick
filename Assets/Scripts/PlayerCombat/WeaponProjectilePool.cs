using System;
using UnityEngine;
using UnityEngine.Pool;

namespace PlayerCombat
{
    public class WeaponProjectilePool : MonoBehaviour
    {
        public GameObject projectilePrefab;
        private IObjectPool<GameObject> _pool;

        public bool collectionChecks = true;
        public int maxPoolSize = 10;

        public IObjectPool<GameObject> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                        OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                }

                return _pool;
            }
        }

        private void OnDestroyPoolObject(GameObject obj)
        {
            Destroy(obj);
        }

        private void OnReturnedToPool(GameObject obj)
        {
            obj.SetActive(false);
        }

        private void OnTakeFromPool(GameObject obj)
        {
            obj.SetActive(false);
        }

        GameObject CreatePooledItem()
        {
            var projectile = Instantiate(projectilePrefab);
            var returnToPool = projectile.GetComponent<ReturnProjectileToPool>();
            returnToPool.pool = Pool;
            return projectile;
        }
    }
}