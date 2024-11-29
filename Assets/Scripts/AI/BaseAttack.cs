using Common;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// BaseAttack is the default attack class for our NPCs.
    /// It allows for a simple "melee" attack by ray casting out from a specific point to a fixed range.
    /// The base stats are provided by EnemyCore through Initialize.
    /// </summary>
    public class BaseAttack : MonoBehaviour
    {
        /// <summary>
        /// Indicates whether the NPC is currently attacking.
        /// </summary>
        public bool IsAttacking => _isAttacking;

        [SerializeField] private LayerMask mask;
        private Transform _attackPoint;
        private Animator _anim;
        private float _damage;
        private float _range;
        private bool _isAttacking;
        
        private static readonly int CanAttack = Animator.StringToHash("CanAttack");
        
        /// <summary>
        /// Initializes the attack point and animator components.
        /// </summary>
        public void Awake()
        {
            _attackPoint = transform.Find("AttackPoint");
            Debug.Assert(_attackPoint, "No AttackPoint child object on " + name);
            _anim = GetComponent<Animator>();
        }
        
        /// <summary>
        /// Initializes the attack parameters.
        /// </summary>
        /// <param name="range">The range of the attack.</param>
        /// <param name="damage">The damage dealt by the attack.</param>
        /// <param name="windupTime">The windup time before the attack.</param>
        public void Initialize(float range, float damage, float windupTime)
        {
            _range = range;
            _damage = damage;
        }

        /// <summary>
        /// Starts the attack by triggering the attack animation.
        /// </summary>
        public void StartAttack()
        {
            _anim.SetTrigger(CanAttack);
            _isAttacking = true;
        }

        /// <summary>
        /// Executes the attack by using a raycast to detect and damage enemies.
        /// </summary>
        public void DoAttack()
        {
            Ray attackRay = new Ray(_attackPoint.position, _attackPoint.forward);
            Debug.DrawLine(_attackPoint.position, _attackPoint.position + _attackPoint.forward * _range, Color.red, 2.0f);
            
            if (!Physics.Raycast(attackRay, out RaycastHit rayHit, _range, mask)) return;
            
            Health enemyHealth = rayHit.transform.GetComponent<Health>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(_damage);
            }
        }

        /// <summary>
        /// Ends the attack and resets the attacking state.
        /// </summary>
        public void AttackEnded()
        {
            _isAttacking = false;
        }
    }
}