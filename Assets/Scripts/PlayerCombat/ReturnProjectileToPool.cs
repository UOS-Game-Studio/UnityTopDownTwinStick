using UnityEngine;
using UnityEngine.Pool;

namespace PlayerCombat
{
    public class ReturnProjectileToPool : MonoBehaviour
    {
        public IObjectPool<GameObject> pool;

        private void OnCollisionEnter(Collision collision)
        {
            pool.Release(gameObject);
        }
    }
}