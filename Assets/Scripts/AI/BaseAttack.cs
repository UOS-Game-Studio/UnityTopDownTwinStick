using System.Collections;
using Common;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// BaseAttack is the default attack class on our NPCs
    /// it allows for a simple "melee" attack by ray casting out from a specific point to a fixed range.
    /// the base stats are provided by EnemyCore through Initialize.
    /// </summary>
    public class BaseAttack : MonoBehaviour
    {
        public bool IsAttacking => _isAttacking;
        [SerializeField] private LayerMask mask;
        private Transform _attackPoint;

        private Animator _anim;
        private float _damage;
        private float _range;
        private float _windupTime;

        private bool _isAttacking;
        
        private static readonly int CanAttack = Animator.StringToHash("CanAttack");
        
        public void Awake()
        {
            _attackPoint = transform.Find("AttackPoint");
            Debug.Assert(_attackPoint, "No AttackPoint child object on " + name);

            _anim = GetComponent<Animator>();
        }
        
        public void Initialize(float range, float damage, float windupTime)
        {
            _range = range;
            _damage = damage;
            _windupTime = windupTime; // wind up time is used if we use a coroutine to manage the actual attack.
        }

        // We trigger the attack animation here, currently the animation includes an event that calls back to DoAttack
        // we could potentially use a Coroutine instead.
        public void StartAttack()
        {
            // triggers are one off events, they are true for a single frame and then reset by the underlying 
            // animation system.
            _anim.SetTrigger(CanAttack);
            _isAttacking = true;
        }

        public void DoAttack()
        {
            // a Ray is an infinite line with a start point and direction.
            Ray attackRay = new Ray(_attackPoint.position, _attackPoint.forward);
            
            Debug.DrawLine(_attackPoint.position, _attackPoint.position + _attackPoint.forward * _range, Color.red, 2.0f);
            
            // this uses an inline declaration of rayHit, which is unique to arguments with the "out" keyword:
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters#out-parameter-modifier
            if (!Physics.Raycast(attackRay, out RaycastHit rayHit, _range, mask)) return;
            
            Health enemyHealth = rayHit.transform.GetComponent<Health>();
            
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(_damage);
            }
        }

        public void AttackEnded()
        {
            Debug.Log("Attack Ended");
            _isAttacking = false;
        }
    }
}
