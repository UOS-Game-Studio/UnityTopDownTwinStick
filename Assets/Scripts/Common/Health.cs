using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Common
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float currentHealth = 1.0f;
        private float _maxHealth;
        public UnityEvent<Health> onDeath = new UnityEvent<Health>();
        public UnityEvent onTakeDamage = new UnityEvent();

        public void SetMaxHealth(float newMax)
        {
            _maxHealth = newMax;
        }
        
        public void TakeDamage(float damage)
        {
            currentHealth = Mathf.Min(0.0f, currentHealth - damage);

            // trigger an animation or similar to indicate the hit
            // but do that elsewhere!
            onTakeDamage.Invoke();
            
            if (currentHealth > 0.0f) return;
            
            onDeath.Invoke(this);
            Destroy(gameObject, 0.1f); // could handle this elsewhere too, monsters could be pooled and this stops that.
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