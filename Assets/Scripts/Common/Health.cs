using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Common
{
    /// <summary>
    /// Health is a universal health component, it can be attached to anything we want to take damage
    /// whether that's a PC or NPC (or anything else really)
    /// Events:
    ///    <c>onDeath</c> - invoked when <c>_currentHealth</c> drops to or below 0 - passes an instance of Health along
    ///    <c>onTakeDamage</c> - invoked whenever damage is taken
    /// </summary>
    public class Health : MonoBehaviour
    {
        private float _currentHealth = 1.0f;
        private float _maxHealth;
        public UnityEvent<Health> onDeath = new UnityEvent<Health>();
        public UnityEvent onTakeDamage = new UnityEvent();

        private bool _isDead;
        
        public void Initialize(float newMax)
        {
            _maxHealth = newMax;
            _currentHealth = _maxHealth;
        }
        
        public void TakeDamage(float damage)
        {
            if (_isDead) return;
            
            _currentHealth = Mathf.Min(0.0f, _currentHealth - damage);

            // trigger an animation or similar to indicate the hit
            // but do that elsewhere!
            onTakeDamage.Invoke();
            
            if (_currentHealth > 0.0f) return;
            
            onDeath.Invoke(this);
            _isDead = true;
            Destroy(gameObject, 0.1f); // TODO: strip this out to allow whatever handles onDeath to deal with this.
        }

        private void OnDestroy()
        {
            onDeath.RemoveAllListeners();
        }
        
        void Start()
        {
            GameController gameController = GameObject.FindAnyObjectByType<GameController>();

            Debug.Assert(gameController, "No GameController found in Scene for Health.Start");
            
            onDeath.AddListener(gameController.OnCharacterKilled);
        }
    }

}