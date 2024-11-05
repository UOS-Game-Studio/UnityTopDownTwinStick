using System.Collections;
using Common;
using UnityEngine;

namespace AI
{
    public class BaseAttack : MonoBehaviour
    {
        
        [SerializeField] private LayerMask mask;
        private Transform _attackPoint;
        private float _damage;
        private float _range;
        private float _windupTime;
        private WaitForSeconds _windupWait;

        public void Awake()
        {
            _attackPoint = transform.Find("AttackPoint");
            Debug.Assert(_attackPoint, "No AttackPoint child object on " + name);
        }
        
        public void Initialize(float range, float damage, float windupTime)
        {
            _range = range;
            _damage = damage;
            _windupTime = windupTime;
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

            Ray attackRay = new Ray(_attackPoint.position, _attackPoint.forward);
            
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
