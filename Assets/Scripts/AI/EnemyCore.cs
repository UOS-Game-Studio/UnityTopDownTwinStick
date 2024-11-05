using System;
using Common;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class EnemyCore : MonoBehaviour
    {
        [SerializeField] private EnemyStatsSO statsData;
        private Health _health;
        private BaseAttack _attack;
        private NavMeshAgent _navMeshAgent;

        private Animator _animator;
        
        private void Awake()
        {
            _health = GetComponent<Health>();
            _attack = GetComponent<BaseAttack>();
            _animator = GetComponent<Animator>();
            
            // not sure how this will work with root motion animations?
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _health.Initialize(statsData.maxHealth);
            _health.onTakeDamage.AddListener(RespondToDamage);
            _health.onDeath.AddListener(OnDeath);
            
            _attack.Initialize(statsData.attackRange, statsData.attackDamage, statsData.attackWindupTime);

            // probably setup the FSM here and pass in the BaseAttack reference to it?
        }

        public void RespondToDamage()
        {
            Debug.Log("Oh no, " + name + " took damage!");
        }

        public void OnDeath(Health characterHealth)
        {
            Debug.Log("Oh no, " + name + " has died!");
        }
    }
}
