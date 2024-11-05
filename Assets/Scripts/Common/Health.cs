using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Common
{
    public class Health : MonoBehaviour
    {
        private float _currentHealth = 1.0f;
        private float _maxHealth;
        public UnityEvent<Health> onDeath = new UnityEvent<Health>();
        public UnityEvent onTakeDamage = new UnityEvent();
        
        public void Initialize(float newMax)
        {
            _maxHealth = newMax;
            _currentHealth = _maxHealth;
        }
        
        public void TakeDamage(float damage)
        {
            _currentHealth = Mathf.Min(0.0f, _currentHealth - damage);

            // trigger an animation or similar to indicate the hit
            // but do that elsewhere!
            onTakeDamage.Invoke();
            
            if (_currentHealth > 0.0f) return;
            
            onDeath.Invoke(this);
            Destroy(gameObject, 0.1f); // TODO: strip this out to allow whatever handles onDeath to deal with this.
        }

        private void OnDestroy()
        {
            onDeath.RemoveAllListeners();
        }
        
        void Start()
        {
            GameController gameController = GameObject.FindAnyObjectByType<GameController>();

            onDeath.AddListener(gameController.OnCharacterKilled);
        }
    }

}