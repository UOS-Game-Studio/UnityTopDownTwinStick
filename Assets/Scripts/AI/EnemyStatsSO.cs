using UnityEngine;

namespace AI
{
    
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
    public class EnemyStatsSO : ScriptableObject
    {
        public float maxHealth;
        public float attackDamage;
        public float attackWindupTime;
        public float attackRange;
    }

}