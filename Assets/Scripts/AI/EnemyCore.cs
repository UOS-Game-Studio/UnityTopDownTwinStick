using System;
using Common;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    /// <summary>
    /// EnemyCore contains the start point for all NPC logic; it holds <c>Health</c> and <c>Attack</c> components 
    /// along with stats brought in from a ScriptableObject (see <c>EnemyStatsSO</c>)
    /// </summary>
    public class EnemyCore : MonoBehaviour, IPausable
    {
        [SerializeField] private EnemyStatsSO statsData;
        private Health _health;
        private BaseAttack _attack;
        private NavMeshAgent _navMeshAgent;
        private Transform _playerTransform;
        
        private Animator _animator;
        private CapsuleCollider _collider;
        
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int IsDead = Animator.StringToHash("IsDead");
        
        private void Awake()
        {
            _health = GetComponent<Health>();
            _attack = GetComponent<BaseAttack>();
            
            _animator = GetComponent<Animator>();
            _animator.applyRootMotion = false;
            
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updatePosition = true;
            _navMeshAgent.updateRotation = true;
            _navMeshAgent.stoppingDistance = statsData.attackRange;

            _collider = GetComponent<CapsuleCollider>();
        }

        private void Start()
        {
            _health.Initialize(statsData.maxHealth);
            _health.onTakeDamage.AddListener(RespondToDamage);
            _health.onDeath.AddListener(OnDeath);
            
            _attack.Initialize(statsData.attackRange, statsData.attackDamage, statsData.attackWindupTime);

            _playerTransform = GameObject.Find("PlayerBase")?.transform;

            PauseControl.OnPause.AddListener(PauseHandler);
        }

        void Update()
        {
            if (!_navMeshAgent.enabled) return;
            
            if (Vector3.Distance(_playerTransform.position, this.transform.position) < _navMeshAgent.stoppingDistance)
            {
               _attack.StartAttack(); 
            }
            else
            {
                _navMeshAgent.destination = _playerTransform.position;
                _animator.SetFloat(VelocityX, _navMeshAgent.velocity.magnitude);
            }
        }
        
        public void RespondToDamage()
        {
            //Debug.Log("Oh no, " + name + " took damage!");
        }

        public void OnDeath(Health characterHealth)
        {
            //isDead = true;
            _animator.SetTrigger(IsDead);
            _navMeshAgent.velocity = Vector3.zero;
            _navMeshAgent.enabled = false;
            _collider.enabled = false;
        }

        public void PauseHandler(bool isPaused)
        {
            _animator.enabled = !isPaused;
            _navMeshAgent.enabled = !isPaused;
        }
    }
}
