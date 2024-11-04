using System.Collections;
using Common;
using UnityEngine;

namespace AI
{
    public class BaseAttack : MonoBehaviour
    {
        [SerializeField] private Transform attackPoint;
        [SerializeField] private LayerMask mask;
        private float _damage;
        private float _range;
        private float _windupTime;
        private WaitForSeconds _windupWait;

        public void SetRange(float range)
        {
            _range = range;
        }
        
        public void SetDamage(float newDamage)
        {
            _damage = newDamage;
        }

        public void SetWindup(float time)
        {
            _windupTime = time;
            _windupWait = new WaitForSeconds(_windupTime);
        }

        public void StartAttack()
        {
            // start the wind up.
            StartCoroutine(DoAttack());
        }

        private IEnumerator DoAttack()
        {
            yield return _windupWait;

            Ray attackRay = new Ray(attackPoint.position, attackPoint.forward);
            
            // this uses an inline declaration of rayHit, which is unique to arguments with the "out" keyword:
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters#out-parameter-modifier
            if (!Physics.Raycast(attackRay, out RaycastHit rayHit, _range, mask)) yield return null;

            Health enemyHealth = rayHit.transform.GetComponent<Health>();

            if (enemyHealth)
            {
                enemyHealth.TakeDamage(_damage);
            }
        }
    }
}
