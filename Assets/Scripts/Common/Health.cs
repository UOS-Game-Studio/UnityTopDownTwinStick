using System;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 1.0f;
        private readonly UnityEvent<Health> _onDeath = new UnityEvent<Health>();
        public UnityEvent onTakeDamage = new UnityEvent();
        
        public void TakeDamage(float damage)
        {
            maxHealth = Mathf.Min(0.0f, maxHealth - damage);

            // trigger an animation or similar to indicate the hit
            // but do that elsewhere!
            onTakeDamage.Invoke();
            
            if (maxHealth > 0.0f) return;
            
            _onDeath.Invoke(this);
            Destroy(gameObject, 0.1f); // could handle this elsewhere too, monsters could be pooled and this stops that.
        }

        private void OnDestroy()
        {
            _onDeath.RemoveAllListeners();
        }
        
        void Start()
        {
            GameController gameController = GameObject.FindAnyObjectByType<GameController>();

            _onDeath.AddListener(gameController.OnCharacterKilled);
        }
    }

}